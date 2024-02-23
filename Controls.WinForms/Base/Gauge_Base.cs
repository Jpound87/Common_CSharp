using Common;
using Common.Extensions;
using Datam.WinForms.Constants;
using Datam.WinForms.Interface;
using Parameters;
using Parameters.Interface;
using Runtime;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Unit.Interface;

namespace Datam.WinForms.Base
{
    [TypeDescriptionProvider(typeof(AbstractDescriptionProvider<Gauge_Base, UserControl>))]
    public abstract class Gauge_Base : UserControl, IDrag
    {
        #region Identity
        public const String ControlName = nameof(Gauge_Base);

        public virtual String Identity
        {
            get
            {
                return ControlName;
            }
        }
        #endregion /Identity

        #region Constants
        private const UInt32 GAUGE_DISPLAY_MIN_WIDTH = 300;
        private readonly TimeSpan updateRate = TimeSpan.FromMilliseconds(500);
        private readonly TimeSpan GAUGE_TIMEOUT = TimeSpan.FromSeconds(5);
        #endregion /Constants

        #region Readonly 

        #region ToolTip
        protected readonly ToolTip gaugeToolTip = new()
        {
            InitialDelay = Control_Timing.TOOLTIP_INITAL_DELAY,
            ReshowDelay = Control_Timing.TOOLTIP_RESHOW_DELAY,
            AutoPopDelay = Control_Timing.TOOLTIP_MAX_TIME,
            UseAnimation = true,
            ShowAlways = true
        };
        #endregion /ToolTip

        #region Delegates
        private readonly MethodInvoker processUpdateValue_Invoker;
        #endregion /Delegates

        #endregion /Readonly 

        #region Mouse Events
        new public event MouseEventHandler MouseUp;
        protected readonly MouseEventHandler mouseUp_Handler;
        new public event MouseEventHandler MouseDown;
        protected readonly MouseEventHandler mouseDown_Handler;
        new public event MouseEventHandler MouseMove;
        protected readonly MouseEventHandler mouseMove_Handler;
        #endregion /Mouse Events

        #region Color

        #region Back 
        protected Color backColor = SystemColors.Control;
        /// <summary>Get/Set button Background image</summary>
        public override abstract Color BackColor { get; set; }
        #endregion /Back

        #region Fore
        protected Color foreColor = SystemColors.ControlText;
        /// <summary>Get/Set button Background image</summary>
        new public abstract Color ForeColor { get; set; }
        #endregion /Fore

        #region Selected 
        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set gauge highlight color"), DefaultValue(typeof(SystemColors), "HotTrack")]
        public Color HighlightColor { get; set; }
        #endregion /Selected

        #region Stale
        public Color StaleColor { get; private set; }
        #endregion /Stale

        #region Mouse Over
        private Color mouseOverBackColor;
        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button mouse over color"), DefaultValue(typeof(SystemColors), "ControlLight")]
        public Color MouseOverBackColor
        {
            get
            {
                return mouseOverBackColor;
            }
            set
            {
                mouseOverBackColor = value;
            }
        }
        #endregion /Mouse Over

        #region Mouse Down
        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button mouse over color"), DefaultValue(typeof(SystemColors), "ControlDark")]
        private Color mouseDownBackColor;
        public Color MouseDownBackColor
        {
            get
            {
                return mouseDownBackColor;
            }
            set
            {
                mouseDownBackColor = value;
            }
        }
        #endregion /Mouse Down

        #endregion /Color

        #region Parameter
        protected String formatString = "0.##";
        public Boolean HasValidParameter { get; protected set; }

        protected IParameter parameter = Parameter_CiA402.Null;
        [Browsable(false)]
        public abstract IParameter Parameter { get; set; }

        /// <summary>
        /// This accessor allows for the parameter name to be read.
        /// </summary>
        [Browsable(false)]
        public String DisplayName
        {
            get
            {
                return Parameter.Name ?? Translation_Manager.None;
            }
        }
        #endregion /Parameter

        #region Display Value
        [Browsable(false)]
        public abstract String DisplayValue { get; protected set; }

        [Browsable(false)]
        public abstract String DisplayUnit { get; protected set; }

        [Browsable(false)]
        public GaugeDisplayMode DisplayMode { get; set; } = GaugeDisplayMode.Normal;
        #endregion /Display Value

        #region Display Properties

        #region Font
        private Font titleFont;
        public Font TitleFont
        {
            get
            {
                return titleFont;
            }
            set
            {
                titleFont = value;
                SetSizeReqiuirements();
            }
        }

        private Font valueFont;
        public Font ValueFont
        {
            get
            {
                return valueFont;
            }
            set
            {
                valueFont = value;
                SetSizeReqiuirements();
            }
        }

        private FontSizeChoices fontSize;
        public FontSizeChoices FontSize
        {
            get
            {
                return fontSize;
            }
            set
            {
                if (fontSize != value)
                {
                    fontSize = value;
                    switch (value)
                    {
                        case FontSizeChoices.Small:
                        case FontSizeChoices.Medium:
                        case FontSizeChoices.Large:
                            break;
                    }
                    SetSizeReqiuirements();
                }
            }
        }
        #endregion /Font

        #region Size Requirement
        protected Int32 staticHeight = 0;
        protected Int32 nameTextHeight = 0;
        protected Int32 valueTextHeight = 0;
        protected Int32 dynamicHeight = 0;
        protected Int32 height = 0;
        protected Int32 statisticsTextHeight = 0;

        [Browsable(false)]
        public override Size MinimumSize
        {
            get
            {
                return base.MinimumSize;
            }
            set
            {
                base.MinimumSize = new Size(value.Width, height);
            }
        }

        [Browsable(false)]
        public override Size MaximumSize
        {
            get
            {
                return base.MaximumSize;
            }
            set
            {
                base.MinimumSize = new Size(value.Width, height);
            }
        }

        public Int32 MaximumWidth
        {
            get
            {
                return base.MaximumSize.Width;
            }
            set
            {
                if (base.MaximumSize.Width != value)
                {
                    base.MaximumSize = new Size(value, base.MaximumSize.Height);
                }
            }
        }
        #endregion /Size Requirement

        #region Marquee Scroll
        private Boolean overrideMarquee = false;
        /// <summary>
        /// This value will force the marquee active to the value set in
        /// OverrideMarqueeValue.
        /// </summary>
        [Browsable(false)]
        public Boolean OverrideMarquee
        {
            get
            {
                return overrideMarquee;
            }
            set
            {
                if (overrideMarquee != value)
                {
                    overrideMarquee = value;
                    SetMarqueeScrollActive();
                }
            }
        }

        private Boolean overrideMarqueeValue = false;
        /// <summary>
        /// This value represents whether to force marquee to operate when 
        /// the override is enabled.
        /// </summary>
        [Browsable(false)]
        public Boolean OverrideMarqueeValue
        {
            get
            {
                return overrideMarquee;
            }
            set
            {
                if (overrideMarqueeValue != value)
                {
                    overrideMarqueeValue = value;
                    SetMarqueeScrollActive();
                }
            }
        }

        private Boolean marqueeEnabled = false;
        /// <summary>
        /// This value represents if the marquee is enabled to be used in regular operation,
        /// if required by value size.
        /// </summary>
        [Browsable(false)]
        public Boolean MarqueeEnabled
        {
            get
            {
                return marqueeEnabled;
            }
            set
            {
                if (marqueeEnabled != value)
                {
                    marqueeEnabled = value;
                    SetMarqueeScrollActive();
                }
            }
        }

        /// <summary>
        /// This value represents if the gauge is currenlty using marquee scrolling.
        /// </summary>
        public Boolean MarqueeActive { get; private set; }

        protected String marqueeString = String.Empty;

        #endregion /Marquee Scroll

        #region Scientific Notation
        private Boolean overrideScientificNotation = false;
        public Boolean OverrideScientificNotation
        {
            get
            {
                return overrideScientificNotation;
            }
            set
            {
                if (overrideScientificNotation != value)
                {
                    overrideScientificNotation = value;
                    SetScientificNotationActive();
                }
            }
        }

        private Boolean overrideScientificNotationValue = false;
        public Boolean OverrideScientificNotationValue
        {
            get
            {
                return overrideScientificNotationValue;
            }
            set
            {
                if (overrideScientificNotationValue != value)
                {
                    overrideScientificNotationValue = value;
                    SetScientificNotationActive();
                }
            }
        }

        private Boolean sciNotationEnabled;
        public Boolean ScientificNotationEnabled
        {
            get
            {
                return sciNotationEnabled;
            }
            set
            {
                if (sciNotationEnabled != value)
                {
                    sciNotationEnabled = value;
                    SetScientificNotationActive();
                }
            }
        }

        public Boolean ScientificNotationActive { get; private set; }

        #endregion /Scientific Notation

        #endregion /Display Properties

        #region User Control

        #region Mouse Click
        public abstract event EventHandler Click_Metadata;

        public abstract event EventHandler Click_Units;

        protected LeftRightAlignment mouseButton_ClickSelect;
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set the select mouse button"), DefaultValue(typeof(SystemColors), "Control")]
        public LeftRightAlignment MouseButton_ClickSelect
        {
            get
            {
                return mouseButton_ClickSelect;
            }
            set
            {
                mouseButton_ClickSelect = value;
                SetNoneLabel();
            }
        }
        #endregion /Mouse Click

        #region Selected
        public Boolean IsSelected//CAH - Check this
        {
            get
            {
                return ContainsFocus;
            }
        }

        /// <summary>
        /// This event handler is intened to allow the user to click into and out of 
        /// the gauge display control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="me"></param>
        public void SelectControl()
        {
            if (Focused)
            {// Then we want to lose focus, which is easy for me.
                ActiveControl = null;
            }
            else
            {
                Focus();
            }
        }
        #endregion /Selected

        #region Highlight
        protected Boolean highlight;
        public abstract Boolean Highlight { set; }
        #endregion /Highlight

        #region Drag
        protected readonly FAST_Semaphore draggingWaitHandle = new(1, 1);

        protected Boolean inDrag;
        public abstract Boolean InDrag { get; set; }
        #endregion /Drag

        #endregion /User Control

        #region Constructor
        public Gauge_Base()
        {
            DoubleBuffered = true;
            mouseUp_Handler = new MouseEventHandler(OnMouseUp);
            base.MouseUp += mouseUp_Handler;
            mouseDown_Handler = new MouseEventHandler(OnMouseDown);
            base.MouseDown += mouseDown_Handler;
            mouseMove_Handler = new MouseEventHandler(OnMouseMove);
            base.MouseMove += mouseMove_Handler;
            processUpdateValue_Invoker = new MethodInvoker(ProcessUpdateValue);
            SetNoneLabel();
        }
        #endregion /Constructor

        #region Translation

        #region Update
        public abstract void UpdateText();//todo, link to event translate changed in settings
        #endregion Update

        #region None Label
        protected String noneLabel;
        protected void SetNoneLabel()
        {
            switch (mouseButton_ClickSelect)
            {
                default:
                case LeftRightAlignment.Right:
                    noneLabel = Translation_Manager.Message_RightClickSet;
                    return;
                case LeftRightAlignment.Left:
                    noneLabel = Translation_Manager.Message_LeftClickSet;
                    return;
            }
        }
        #endregion /None Label

        #endregion /Translation

        #region Update

        #region Name
        protected abstract void SetName(String name);
        #endregion /Name

        #region Value

        protected abstract void ClearValue(Boolean visible = false);

        public void UpdateValue()
        {
            if (InvokeRequired)
            {
                Invoke(processUpdateValue_Invoker);
            }
            else
            {
                ProcessUpdateValue();
            }
        }

        protected abstract void ProcessUpdateValue();

        #endregion /Value

        #region ToolTip
        /// <summary>
        /// This method sets up the gauge tooltip
        /// </summary>
        public abstract void SetToolTip();

        protected String AddUnit(String unitlessString)
        {
            try
            {
                if (parameter.Unit.Abbreviation.Length > 0 && Parameter.Unit.IsUnitDisplayble())
                {// Add the unit type abbriviateion
                    return String.Format("{0}{1}", unitlessString, parameter.Unit.Abbreviation);
                }
                return unitlessString;
            }
            catch
            {
                return unitlessString;
            }
        }
        #endregion /ToolTip

        #region Size 
        protected abstract void SetSizeReqiuirements();

        protected void SetScientificNotationActive()
        {
            if (OverrideScientificNotation)
            {
                ScientificNotationActive = OverrideScientificNotationValue;
            }
            else
            {
                ScientificNotationActive = ScientificNotationEnabled && parameter.CastToType;
            }
            formatString = ScientificNotationActive? "e2" : "0.##";
        }

        protected void SetMarqueeScrollActive()
        {
            if (OverrideMarquee)
            {
                MarqueeActive = OverrideMarqueeValue;
            }
            else
            {
                MarqueeActive = MarqueeEnabled;
            }
        }

        /// <summary>
        /// This method adjusts output formatting with respect to 
        /// the given parameter type 
        /// </summary>
        protected void CheckDisplaySize(TextBox displayTextBox)
        {
            if (displayTextBox.IsTextOverflowing_Width(out int widthFactor))
            {// If we are trying to display more digits than the minimum size window will allow 
                if (ScientificNotationActive)
                {
                    DisplayMode = GaugeDisplayMode.Scientific;
                }
                else if (MarqueeActive)
                {
                    DisplayMode = GaugeDisplayMode.Marquee;
                }
                else
                {
                    Width += widthFactor;
                }
            }
            else
            {
                Width -= widthFactor;
            }
        }
        #endregion /Size

        #region Mouse Up
        protected void OnMouseUp(Object _, MouseEventArgs mea)// Culpa.
        {
            MouseUp?.Invoke(this, mea);
        }
        #endregion /Mouse Up

        #region Mouse Down
        protected void OnMouseDown(Object _, MouseEventArgs mea)// Culpa.
        {
            MouseDown?.Invoke(this, mea);
        }
        #endregion /Mouse Down

        #region Mouse Move
        protected void OnMouseMove(Object _, MouseEventArgs mea)// Culpa.
        {
            MouseMove?.Invoke(this, mea);
        }
        #endregion /Mouse Move

        #endregion /Update
    }
}

using Common.Constant;
using Common.Extensions;
using Datam.Resources.Constant;
using Datam.WinForms.Base;
using Datam.WinForms.Constants;
using Parameters;
using Parameters.Interface;
using Runtime;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using Unit.Interface;

namespace Datam.WinForms.Controls
{
    public partial class Gauge : Gauge_Base
    {
        #region Identity
        new public const String ControlName = nameof(Gauge_Numeric);

        public override String Identity
        {
            get
            {
                return ControlName;
            }
        }
        #endregion /Identity

        #region Constants
        private const Int32 PRIOR_VALUES_COUNT = 20;
        #endregion /Constants

        #region Readonly 

        #region Prior Values
        private Int32 priorValuesFillIndex = 0;
        private readonly Double[] PriorValues = new double[PRIOR_VALUES_COUNT];
        #endregion /Prior Values

        #endregion /Readonly 

        #region Parameter
        [Browsable(false)]
        public override IParameter Parameter
        {
            get
            {
                return parameter;
            }
            set
            {
                if (!(value == null || value == parameter))
                {// If its not null or the same.
                    parameter = value;
                    HasValidParameter = !(value.Type == ParameterType.Null && value.GetHashCode() == Parameter_CiA402.Null.GetHashCode());
                    ClearValue(HasValidParameter); // Tops Brand
                    if (HasValidParameter)
                    {// We can actually set this up
                        SetName(value.FullName);
                        if (Double.TryParse(value.Value_Display, out Double doubleVaule))
                        {
                            PriorValues.Fill(doubleVaule);
                        }
                        DisplayValue = value.Value_Display;
                        if (value.Unit.IsUnitDisplayble())
                        {// We have a unit to display.
                            lblUnitDisplay.Visible = true;
                            DisplayUnit = value.Unit.Abbreviation;
                        }
                    }
                    else
                    {// Back to basics.
                        SetName(noneLabel);
                        lblUnitDisplay.Visible = false;
                    }
                }
            }
        }
        #endregion /Parameter

        #region Display Value
        [Browsable(false)]
        public override String DisplayValue
        {
            get
            {
                return txtValueDisplay.Text;
            }
            protected set
            {
                txtValueDisplay.Text = value;
            }
        }

        [Browsable(false)]
        public override String DisplayUnit
        {
            get
            {
                return lblUnitDisplay.Text;
            }
            protected set
            {
                lblUnitDisplay.Text = value;
            }
        }
        #endregion /Display Value

        #region User Control

        #region Mouse Click
        public override event EventHandler Click_Metadata
        {
            add
            {
                throw new NotImplementedException();
            }
            remove
            {
               throw new NotSupportedException();
            }
        }


        new public event EventHandler Click
        {
            add
            {
                lock (lblUnitDisplay)
                {
                    base.Click += value;
                    bgbGauge.Click += value;
                    tlpGauge.Click += value;
                    txtValueDisplay.Click += value;
                }
            }
            remove
            {
                lock (lblUnitDisplay)
                {
                    base.Click -= value;
                    bgbGauge.Click -= value;
                    tlpGauge.Click -= value;
                    txtValueDisplay.Click -= value;
                }
            }
        }

        public override event EventHandler Click_Units
        {
            add
            {
                lock (lblUnitDisplay)
                {
                    lblUnitDisplay.Click += value;
                }
            }
            remove
            {
                lock (lblUnitDisplay)
                {
                    lblUnitDisplay.Click -= value;
                }
            }
        }
        #endregion /Mouse Click

        #region Highlight
        public override bool Highlight
        {
            set
            {
                highlight = value;
                if (!inDrag)
                {
                    bgbGauge.ForeColor = highlight ? Datam_Color.Gauge_Highlight_Foreground : ForeColor;
                }
            }
        }
        #endregion /Highlight

        #region Drag
        public override bool InDrag
        {
            get
            {
                return inDrag;
            }
            set
            {
                inDrag = value;
                //Enabled = !inDrag;
                bgbGauge.ForeColor = inDrag ? Datam_Color.Gauge_Draging_Foreground : ForeColor;
                if (inDrag)
                {
                    draggingWaitHandle.Wait(0);
                }
                else
                {
                    draggingWaitHandle.TryRelease();
                }
            }
        }
        #endregion /Drag

        #endregion /User Control

        #region Color

        #region Back 
        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button background color"), DefaultValue(typeof(SystemColors), "Control")]
        public override Color BackColor
        {
            get
            {
                return backColor;
            }
            set
            {
                backColor = value;
                txtValueDisplay.BackColor = value;
                bgbGauge.BackColor = value;
            }
        }
        #endregion /Back 

        #region Fore
        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button forground color"), DefaultValue(typeof(SystemColors), "ControlText")]
        public override Color ForeColor
        {
            get
            {
                return foreColor;
            }
            set
            {
                foreColor = value;
                txtValueDisplay.ForeColor = value;
                bgbGauge.ForeColor = value;
                bgbGauge.BorderColor = value;
            }
        }
        #endregion /Fore

        #endregion /Color

        #region Font
        public override Font Font 
        { 
            get
            {
                return txtValueDisplay.Font;
            }
            set
            {
                txtValueDisplay.Font = value;
            }
        }
        #endregion /Font

        #region Constructor
        public Gauge() : base()
        {
            InitializeComponent();
            InitializeDisplay();
        }

        public Gauge(IParameter parameter) : base()
        {
            InitializeComponent();
            InitializeDisplay();
            Parameter = parameter;
        }

        private void InitializeDisplay()
        {
            bgbGauge.MouseUp += mouseUp_Handler;
            tlpGauge.MouseUp += mouseUp_Handler;
            txtValueDisplay.MouseUp += mouseUp_Handler;

            bgbGauge.MouseDown += mouseDown_Handler;
            tlpGauge.MouseDown += mouseDown_Handler;
            txtValueDisplay.MouseDown += mouseDown_Handler;

            bgbGauge.MouseMove += mouseMove_Handler;
            tlpGauge.MouseMove += mouseMove_Handler;
            txtValueDisplay.MouseMove += mouseMove_Handler;

            txtValueDisplay.ContextMenuStrip = null;
            txtValueDisplay.ShortcutsEnabled = false;

            int groupBoxVertical = bgbGauge.Margin.Vertical + bgbGauge.Padding.Vertical;
            int tableLayoutVertical = tlpGauge.Margin.Vertical + tlpGauge.Padding.Vertical;
            int valueBoxVertical = txtValueDisplay.Margin.Vertical + txtValueDisplay.Padding.Vertical;
            staticHeight = groupBoxVertical + tableLayoutVertical + valueBoxVertical;
            SetSizeReqiuirements();
            SetScientificNotationActive();
            SetMarqueeScrollActive();
            CheckDisplaySize(txtValueDisplay);
            ClearValue();
        }

        #endregion /Constructor

        #region Translation
        public override void UpdateText()//todo, link to event translate changed in settings
        {
            SetNoneLabel();
        }
        #endregion /Translation

        #region Update

        #region Name
        protected override void SetName(String name)
        {
            string strInfo = String.Format("Gauge Changed to display {0}", name);
            Log_Manager.LogVerbose(ControlName, strInfo);
            bgbGauge.Text = Control_Culture.Datam_CultureInfo_US.ToTitleCase(name);
        }
        #endregion /Name

        #region Value
        protected override void ClearValue(bool visible = false)
        {
            lock (this)
            {
                txtValueDisplay.Text = String.Empty;
                lblUnitDisplay.Text = String.Empty;
            }
        }

        protected override void ProcessUpdateValue()
        {
            lock (this)
            {
                if (HasValidParameter && !IsSelected)
                {
                    if (DisplayValue != parameter.Value_Display)
                    {
                        DisplayValue = parameter.Value_Display;
                        if (Double.TryParse(parameter.Value_Display, out Double doubleValue))
                        {
                            PriorValues[priorValuesFillIndex++ % PRIOR_VALUES_COUNT] = doubleValue;
              
                            txtValueDisplay.Text = parameter.Value_Display;
                        }
                        CheckDisplaySize(txtValueDisplay);
                        switch (DisplayMode)
                        {
                            case GaugeDisplayMode.Scientific:
                                txtValueDisplay.ConvertToScientificNotation();
                                break;
                            case GaugeDisplayMode.Marquee:
                                //marqueeString = unitValue + " | ";
                                break;
                        }
                    }
                    if (DisplayUnit != parameter.Unit.Abbreviation)
                    {
                        DisplayUnit = parameter.Unit.Abbreviation;
                    }

                    SetToolTip();
                }
            }
        }
        #endregion /Value

        #region ToolTip
        /// <summary>
        /// This method sets up the gauge tooltip
        /// </summary>
        public override void SetToolTip()
        {
            StringBuilder gaugeToolTipText = new StringBuilder();
            if (gaugeToolTip.ToolTipTitle != parameter.FullName)
            {// If the value has changed
                gaugeToolTip.ToolTipTitle = parameter.FullName;
            }
            if (HasValidParameter)
            {
                if (parameter.CastToType && parameter is Parameter_CiA402_Numeric numericParameter)
                {
                    gaugeToolTipText.AppendLine(
                    string.Format("Min: {0}\nMax: {1}\nAddress: {2}",
                    AddUnit(numericParameter.MinStr),
                    AddUnit(numericParameter.MaxStr),
                    parameter.Address));
                }
                else
                {
                    gaugeToolTipText.AppendLine(string.Format("Address: {0}", parameter.Address));
                }
            }
            if (gaugeToolTipText.Length > 0)
            {// We changed it in this session
                gaugeToolTip.SetToolTip(txtValueDisplay, gaugeToolTipText.ToString());
                gaugeToolTip.SetToolTip(bgbGauge, gaugeToolTipText.ToString());
                gaugeToolTip.SetToolTip(tlpGauge, gaugeToolTipText.ToString());
            }
        }
        #endregion /ToolTip

        #region Size 
        protected override void SetSizeReqiuirements()
        {
            lock (tlpGauge)
            {
                try
                {
                    bool testText = false;
                    txtValueDisplay.SuspendLayout();
                    if (String.IsNullOrEmpty(txtValueDisplay.Text))
                    {
                        testText = true;
                        txtValueDisplay.Text = Tokens.F3;// Could be anything, except blank ofc.
                    }
                    nameTextHeight = bgbGauge.GetTextSize_Height();
                    valueTextHeight = txtValueDisplay.GetTextSize_Height();
                    dynamicHeight = nameTextHeight + valueTextHeight + statisticsTextHeight;
                    height = dynamicHeight + staticHeight;
                    base.MinimumSize = new Size(MinimumSize.Width, height);
                    base.MaximumSize = new Size(MaximumSize.Width, height);
                    Height = height;
                    if (testText)
                    {
                        txtValueDisplay.Text = String.Empty;
                    }
                }
                finally
                {
                    txtValueDisplay.ResumeLayout();
                }
            }
        }
        #endregion

        #region Mouse Over
        protected override void OnMouseHover(EventArgs e)
        {
            gaugeToolTip.SetToolTip(this, $"Min: {MinimumSize.Height}\nMax{MaximumSize.Height}\nHeight{Height}");
            base.OnMouseHover(e);
        }
        #endregion /Mouse Over

        #endregion/Update
    }
}



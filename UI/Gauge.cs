using Common.AM_Constant;
using Common.Extensions;
using Common.Utility;
using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Common.AM_Controls
{
    /// <summary>
    /// frmGaugeList, each gauge can display one parameter value
    /// </summary>
    public class Gauge : Control_Base
    {
        #region Identity
        new public const string ClassName = nameof(Gauge);

        public override string Identity
        {
            get
            {
                return ClassName;
            }
        }

        #endregion

        #region Constants
        private const int GAUGE_DISPLAY_PADDING = 25;
        private const int GAUGE_DISPLAY_MIN_WIDTH = 300;
        private const int PADDING = 8;
        private const int MARQUEE_TIMER_LEN_MS = 350;
        private readonly TimeSpan updateRate = TimeSpan.FromMilliseconds(500);
        private readonly TimeSpan GAUGE_TIMEOUT = TimeSpan.FromSeconds(5);
        private static readonly TextInfo cultureInfo = new CultureInfo("en-US", false).TextInfo;
        #endregion

        #region Events

        #region Drag Drop Events
        public new event MouseEventHandler MouseDown
        {
            add
            {
                lock (gaugePanel)
                {
                    gaugeLabel.MouseDown += value;
                    gaugePanel.MouseDown += value;
                    gaugeDisplay.MouseDown += value;
                }
            }
            remove
            {
                lock (gaugePanel)
                {
                    gaugeLabel.MouseDown -= value;
                    gaugePanel.MouseDown -= value;
                    gaugeDisplay.MouseDown -= value;
                }
            }
        }

        public new event MouseEventHandler MouseUp
        {
            add
            {
                lock (gaugePanel)
                {
                    gaugeLabel.MouseUp += value;
                    gaugePanel.MouseUp += value;
                    gaugeDisplay.MouseUp += value;
                }
            }
            remove
            {
                lock (gaugePanel)
                {
                    gaugeLabel.MouseUp -= value;
                    gaugePanel.MouseUp += value;
                    gaugeDisplay.MouseUp -= value;
                }
            }
        }

        public new event MouseEventHandler MouseMove
        {
            add
            {
                lock (gaugePanel)
                {
                    gaugeLabel.MouseMove += value;
                    gaugePanel.MouseMove += value;
                    gaugeDisplay.MouseMove += value;
                }
            }
            remove
            {
                lock (gaugePanel)
                {
                    gaugeLabel.MouseMove -= value;
                    gaugePanel.MouseMove -= value;
                    gaugeDisplay.MouseMove -= value;
                }
            }
        }

        public new event EventHandler MouseLeave
        {
            add
            {
                lock (gaugePanel)
                {
                    gaugeLabel.MouseLeave += value;
                    gaugePanel.MouseLeave += value;
                    gaugeDisplay.MouseLeave += value;
                }
            }
            remove
            {
                lock (gaugePanel)
                {
                    gaugeLabel.MouseLeave -= value;
                    gaugePanel.MouseLeave -= value;
                    gaugeDisplay.MouseLeave -= value;
                }
            }
        }

        public bool IsMouseOver
        {
            get
            {
                return gaugeDisplay.IsMouseOverControl();
            }
        }

        public new bool Focused
        {
            get
            {
                return gaugeDisplay.Focused;
            }
        }
        public new event EventHandler LostFocus
        {
            add
            {
                lock (gaugeDisplay)
                {
                    if (!Focused)
                    {
                        gaugeDisplay.LostFocus += value;
                    }
                }
            }
            remove
            {
                lock (gaugeDisplay)
                {
                    if (Focused)
                    {
                        gaugeDisplay.LostFocus -= value;
                    }
                }
            }
        }
        #endregion

        #endregion

        #region Delegates

        #region Color
        private MethodInvoker foreColorAction;
        private MethodInvoker backColorAction;
        #endregion

        #region Font 
        private MethodInvoker gaugeFontAction;
        private MethodInvoker titleFontAction;
        #endregion

        #region Marquee
        private EventHandler marqueeHandler;
        #endregion

        #region Focus
        private EventHandler lostFocusHandler;
        #endregion

        #region Value Update
        private MethodInvoker timeoutAction;
        private Action<String> updateDisplayedAction;
        #endregion

        #endregion /Delegates

        #region Parameter
        private IParameter parameter;
        public IParameter Parameter
        {
            get
            {
                return parameter;
            }
            set
            {
                if (value != parameter)
                {
                    ClearValue(); // Tops Brand
                    parameter = value;
                    Register();
                    isNumeric = parameter is Parameter_CiA402_Numeric;
                    nullAddress = parameter.Index == 0;
                    marqueePosition = 0;
                    SetName(value.FullName);
                }
            }
        }

        public IRegistrationTicket RegistrationTicket_Gauge { get; set; }

        /// <summary>
        /// This method will register the parameter to be updated from the 
        /// upstream device.
        /// </summary>
        /// <param name="parameter">parameter to register</param>
        private void Register()
        {
            if (parameter != null && parameter != Parameter_CiA402.Null)
            {
                RegistrationTicket_Gauge = new RegistrationTicket(this, parameter, UpdateValue);
            }
        }

        public IAddress Address
        {
            get
            {
                return parameter.Address;
            }
        }

        /// <summary>
        /// This accessor allows for the parameter name to be read 
        /// </summary>
        public String ParameterName
        {
            get
            {
                return parameter.Name;
            }
        }
        #endregion

        #region Accessors

        #region Marquee Timer
        private static readonly System.Windows.Forms.Timer marqueeTimer = new System.Windows.Forms.Timer
        {
            Interval = MARQUEE_TIMER_LEN_MS,
            Enabled = false
        };
        internal bool OverrideMarquee { get; set; }
        private bool overrideMarqueeValue;
        internal bool OverrideMarqueeValue
        {
            get
            {
                return overrideMarqueeValue;
            }
            set
            {
                overrideMarqueeValue = value;
            }
        }
        public bool IsMarquee { get; set; }
        public bool MarqueeEnabled
        {
            get
            {
                if (OverrideMarquee)
                {
                    return OverrideMarqueeValue;
                }
                else
                {
                    return Settings.Default.GaugeMarqueeEnabled;
                }
            }
        }
        private string marqueeString = string.Empty;
        #endregion

        #region Timeout

        private bool timeout;
        public bool Timeout
        {
            get
            {
                return timeout;
            }
            set
            {
                if (timeout != value)
                {
                    timeout = value;
                    gaugeDisplay.Invoke_MethodInvoker(timeoutAction, false);// False since we have a ready awaiter in the invoked method. 
                }
            }
        }
        #endregion

        #region Display
        private TableLayoutPanel gaugePanel;
        private Label gaugeLabel;
        private TextBox gaugeDisplay;

        private static long marqueePosition = 0;

        public int ListIndex { get; private set; }

        public new int Height
        {
            get
            {
                return gaugePanel.Height + gaugePanel.Margin.Vertical;
            }
        }

        public new int Width
        {
            get
            {
                return gaugePanel.Width + gaugePanel.Margin.Horizontal;
            }
        }

        public int MinWidth
        {
            get
            {
                return gaugePanel.MinimumSize.Width + gaugePanel.Margin.Horizontal + 4;
            }
        }

        public Panel GaugeContainer
        {
            get
            {
                return gaugePanel;
            }
        }

        /// <summary>
        /// This accessor returns the rectangle in screen coordinates where the 
        /// gaugeBox is located
        /// </summary>
        public Rectangle RectangleOnScreen
        {
            get
            {
                try
                {
                    return gaugePanel.RectangleToScreen(gaugePanel.ClientRectangle);
                }
                catch
                {
                    return new Rectangle();
                }
            }
        }
        #endregion

        #region Display Interaction
        private String displayValue;
        public String DisplayValue
        {
            get
            {
                return displayValue;
            }
            private set
            {
                if (displayValue != value)
                {
                    displayValue = value;
                    gaugeDisplay.Invoke_Action(updateDisplayedAction, value);
                }
            }
        }

        public bool IsSelected//CAH - Check this
        {
            get
            {
                if (gaugeDisplay.ContainsFocus)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool highlight;
        public bool Highlight
        {
            set
            {
                highlight = value;
                if (!inDrag)
                {
                    gaugeLabel.ForeColor = highlight ? Settings_UI.HighlightGaugeForeColor : ForeColor;
                }
            }
        }

        private bool inDrag;
        public bool InDrag
        {
            set
            {
                inDrag = value;
                Enabled = !inDrag;
                gaugeLabel.ForeColor = inDrag ? Settings_UI.DragingGaugeForeColor : ForeColor;
            }
        }

        /// <summary>
        /// This enabled is new since we only want one of the composition of controls to react.
        /// This is the encompassing container. 
        /// </summary>
        public new bool Enabled
        {
            set
            {
                gaugeDisplay.Enabled = value;
            }
        }
        #endregion

        #region Scientific Notation
        internal bool OverrideScientificNotation { get; set; }
        private bool overrideScientificNotationValue;
        internal bool OverrideScientificNotationValue
        {
            get
            {
                return overrideScientificNotationValue;
            }
            set
            {
                overrideScientificNotationValue = value;
            }
        }
        public bool SciNotationEnabled
        {
            get
            {
                if (OverrideScientificNotation)
                {
                    return OverrideScientificNotationValue;
                }
                else
                {
                    return Settings.Default.GaugeScientificNotationEnabled;
                }
            }
        }
        public bool IsSciNotation { get; private set; }
        #endregion

        #region Font

        private Font gaugeFont = new Font("Lucida Console", 50.0f);
        public Font GaugeFont
        {
            get
            {
                return gaugeFont;
            }
            set
            {
                if (gaugeFont != value)
                {
                    gaugeFont = value;
                    gaugeDisplay.Invoke_MethodInvoker(gaugeFontAction, false);// False since we have a ready awaiter in the invoked method. 
                }
            }
        }

        public String GaugeFontName
        {
            get
            {
                return gaugeFont.Name;
            }
            private set
            {
                if (gaugeFont.Name != value)
                {
                    gaugeFont = new Font(value, Settings.Default.GaugeFontSize, GaugeFontStyle);
                }
            }
        }

        public FontStyle GaugeFontStyle
        {
            get
            {
                return gaugeFont.Style;
            }
            private set
            {
                if (gaugeFont.Style != value)
                {
                    gaugeFont = new Font(GaugeFontName, Settings.Default.GaugeFontSize, value);
                }
            }
        }

        private Font titleFont = new Font("Microsoft Sans Serif", 11f);
        public Font TitleFont
        {
            get
            {
                return titleFont;
            }
            set
            {
                if (titleFont != value)
                {
                    titleFont = value;
                    gaugeDisplay.Invoke_MethodInvoker(titleFontAction, false);// False since we have a ready awaiter in the invoked method. 
                }
            }
        }
        public String TitleFontName
        {
            get
            {
                return titleFont.Name;
            }
            private set
            {
                if (titleFont.Name != value)
                {
                    titleFont = new Font(Settings.Default.TitleFontName, 11, FontStyle.Regular);
                }
            }
        }
        #endregion

        #region Color
        private Color foreColor;
        public new Color ForeColor
        {
            get
            {
                return foreColor;
            }
            private set
            {
                if (ForeColor != Color.FromName(Settings.Default.GaugeListForeColor))
                {
                    foreColor = value;
                    gaugePanel.Invoke_MethodInvoker(foreColorAction, false);// False since we have a ready awaiter in the invoked method. 
                }
            }
        }

        private Color backColor;
        public new Color BackColor
        {
            get
            {
                return backColor;
            }
            private set
            {
                if (BackColor != Color.FromName(Settings.Default.GaugeListBackColor))
                {
                    backColor = value;
                    gaugePanel.Invoke_MethodInvoker(backColorAction, false);// False since we have a ready awaiter in the invoked method. 
                }
            }
        }

        public Color StaleColor { get; private set; }
        #endregion

        #endregion /Accessors

        #region Globals

        #region Bool
        private bool isNumeric = false;
        private bool nullAddress;
        #endregion

        #region Integer
        private int paddingWidth;
        #endregion

        #region ToolTip
        private ToolTip titleToolTip;
        private ToolTip gaugeToolTip;
        #endregion

        #endregion /Globals

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="index"></param>
        public Gauge(IParameter parameter, int index = 0) : base(false)// We will make the handle, so pass false.
        {
            ListIndex = index;
            Parameter = parameter;
            CreateGauge();
        }
        #endregion

        #region On Load

        #region Pre-Handle Actions
        protected override void OnHandleCreated()
        {
            CreateActions();
            
            InitializeMarquee();
            UpdateClickSettingDisplay();
        }
        #endregion

        #region Settings
        protected override void RegisterSettings()
        {
            this.AddTrackedSetting(SettingsManager.GAUGE_LIST_RIGHT_SELECT, UpdateClickSettingDisplay);
            gaugePanel.AddTrackedSettingWithInit(nameof(Settings.Default.GaugeListForeColor),
              () => { ForeColor = Color.FromName(Settings.Default.GaugeListForeColor); });
            gaugePanel.AddTrackedSettingWithInit(nameof(Settings.Default.GaugeListBackColor),
              () => { BackColor = Color.FromName(Settings.Default.GaugeListBackColor); });
            gaugePanel.AddTrackedSettingWithInit(nameof(Settings.Default.GaugeFontName),
              () => { GaugeFontName = Settings.Default.GaugeFontName; });
            gaugePanel.AddTrackedSettingWithInit(nameof(Settings.Default.GaugeFontStyle),
              () => { GaugeFontStyle = Settings.Default.GaugeFontStyle; });
            gaugePanel.AddTrackedSettingWithInit(nameof(Settings.Default.TitleFontName),
              () => { TitleFontName = Settings.Default.TitleFontName; });
        }
        #endregion

        #region Initialization
        private void CreateActions()
        {
            foreColorAction = new MethodInvoker(ForeColor_Update);
            backColorAction = new MethodInvoker(BackColor_Update);
            gaugeFontAction = new MethodInvoker(GaugeFont_Update);
            titleFontAction = new MethodInvoker(TitleFont_Update);
            timeoutAction = new MethodInvoker(DisplayTimeout);
            updateDisplayedAction = new Action<string>(UpdateDisplayedValue);
        }

        private void InitializeMarquee()
        {// Marquee functionality 
            marqueeHandler = new EventHandler(MarqueeControl);
            lock (marqueeTimer)
            {
                if (!marqueeTimer.Enabled)
                {
                    marqueeTimer.Tick += new EventHandler(MarqueePosition);
                    marqueeTimer.Enabled = true;
                }
            }
        }

        private void CreateGauge()
        {
            // ToolTip
            titleToolTip = new ToolTip()
            {
                InitialDelay = 250,
                AutoPopDelay = Constants.MAX_TOOLTIP_TIME,
                ShowAlways = true,
                ReshowDelay = 50,
                UseAnimation = true
            };
            gaugeToolTip = new ToolTip()
            {
                InitialDelay = 250,
                AutoPopDelay = Constants.MAX_TOOLTIP_TIME,
                ShowAlways = true,
                ReshowDelay = 50,
                UseAnimation = true
            };
            // Gauge Panel
            gaugePanel = new TableLayoutPanel
            {
                ColumnCount = 1,
                RowCount = 3,
                Anchor = Manager_Form.anchorAll,
                BackColor = Color.Transparent,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                MinimumSize = new Size(GAUGE_DISPLAY_MIN_WIDTH, 0)
            };
            //Gauge Label
            gaugeLabel = new Label
            {
                Text = parameter.Name,
                Font = TitleFont,
                BackColor = Color.Transparent
            };
            //Gauge Display
            paddingWidth = GAUGE_DISPLAY_PADDING + gaugePanel.Padding.Horizontal + gaugePanel.Margin.Horizontal;
            gaugeDisplay = new TextBox
            {
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right,
                Font = GaugeFont,
                BorderStyle = BorderStyle.None,
                Anchor = AnchorStyles.Left,
                AutoSize = true,
                HideSelection = true,
                TabStop = false,
                MinimumSize = new Size(gaugePanel.Size.Width - paddingWidth, 0)
            };
            gaugeDisplay.Dock = DockStyle.Fill;
            gaugeDisplay.KeyUp += new KeyEventHandler(Gauge_EnterKeyPressed);
            // Row 1
            gaugePanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            gaugePanel.Controls.Add(gaugeLabel, 0, 0);
            gaugeLabel.Dock = DockStyle.Top;
            // Row 2
            gaugePanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            gaugePanel.Controls.Add(gaugeDisplay, 0, 1);
            gaugePanel.SetColumnSpan(gaugeDisplay, 2);
            // Row 3
            gaugePanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            gaugePanel.Resize += new EventHandler(OnResizeEvent);
            lostFocusHandler = new EventHandler(LostFocusEvents);
            MouseLeave += lostFocusHandler;

            StaleColor = Color.DimGray;
            RegisterDisposables(gaugePanel, gaugeLabel, gaugeDisplay);
        }
        #endregion

        #endregion /On Load

        #region Methods

        #region Display Update

        #region Font
        private void GaugeFont_Update()
        {
            if (gaugeDisplay != null)
            {
                try
                {
                    SuspendLayout();
                    gaugeDisplay.Font = GaugeFont;
                    CheckDisplaySize(displayValue);
                }
                finally
                {
                    ResumeLayout();
                }
            }
        }

        private void TitleFont_Update()
        {
            if (gaugeDisplay != null)
            {
                try
                {
                    SuspendLayout();
                    gaugeLabel.Font = titleFont;
                }
                finally
                {
                    ResumeLayout();
                }
            }
        }
        #endregion /Font

        #region Color
        private void ForeColor_Update()
        {
            try
            {
                SuspendLayout();
                if (Timeout)
                {
                    gaugeDisplay.ForeColor = StaleColor;
                }
                else if (gaugeDisplay.ForeColor != ForeColor)
                {
                    gaugeDisplay.ForeColor = ForeColor;
                }
                if (gaugeLabel.ForeColor != ForeColor)
                {
                    gaugeLabel.ForeColor = ForeColor;
                }
            }
            finally
            {
                ResumeLayout();
            }
        }
    
        private void BackColor_Update()
        {
            try
            {
                SuspendLayout();
                if (gaugeDisplay.BackColor != BackColor)
                {
                    gaugeDisplay.BackColor = BackColor;
                }
            }
            finally
            {
                ResumeLayout();
            }
        }

        private void DisplayTimeout()
        {
            ForeColor_Update();
            SetToolTip();
        }
        #endregion

        #region Value
        private void ClearValue()
        {
            if (gaugeDisplay != null)
            {
                if (!String.IsNullOrEmpty(gaugeDisplay.Text))
                {
                    gaugeDisplay.Text = String.Empty;
                }
                if (!String.IsNullOrEmpty(DisplayValue))
                {
                    DisplayValue = String.Empty;
                }
            }
        }
        #endregion

        #region Name
        private void SetName(string name)
        {
            string strInfo = String.Format("Gauge Type Modified to {0}", name);
            Manager_Data.LogVerbose("Gauge.cs", strInfo);
            if (gaugeLabel != null)
            {
                gaugeLabel.Text = cultureInfo.ToTitleCase(name);
            }
        }

        private void UpdateClickSettingDisplay()
        {
            if (gaugeLabel.Text.Contains(Manager_Translation.None))
            {
                if (Settings.Default.GaugeListRightSelect)
                {
                    gaugeLabel.Text = Manager_Translation.Message_RightClickSet;
                }
                else
                {
                    gaugeLabel.Text = Manager_Translation.Message_LeftClickSet;
                }
            }
        }
        #endregion

        #endregion /Display Update

        #region Value Update
        public void UpdateValue()
        {
            Manager_Data.LogMethodCall(ClassName, nameof(UpdateValue));
            if (parameter.Index > 0 && !inDrag)
            {// Not null.
                string value = parameter.Value_Display;
                try
                {
                    Timeout = !parameter.LastValueRefreshTime.IsWithinTimeWindow(GAUGE_TIMEOUT);
                    if (!value.Contains("ERROR"))
                    {
                        if (isNumeric)
                        {
                            //Common.Utility.ConvertToPeriodDecimal(ref value);
                        }
                        DisplayValue = value;
                    }
                    else
                    {
                        string strErr = string.Format("Gauge '{0}' update contained 'ERROR'", ParameterName);
                        Manager_Data.LogError(ClassName, strErr);
                    }
                }
                catch (Exception ex)
                {// Likely one didn't get "received" correctly from drive xfer parser
                    Manager_Data.IssueAlert(ex);
                    throw (ex);
                }
            }
        }

        private void UpdateDisplayedValue(string value)
        {
            if (!(value == null || gaugeDisplay.Focused))// DeMorgans Law.
            {
                String unitValue = $"{AddUnit(value)} ";
                CheckDisplaySize(unitValue);
                if (IsMarquee)
                {
                    marqueeString = unitValue + " | ";
                }
                else if (IsSciNotation)
                {
                    gaugeDisplay.Text = AddUnit(AdjLargeNum(value)) + " ";
                }
                else
                {
                    gaugeDisplay.Text = unitValue;
                }
                SetToolTip();
            }
        }
        #endregion

        #region ToolTip Update
        /// <summary>
        /// This method sets up the gauge tooltip
        /// </summary>
        public void SetToolTip()
        {
            StringBuilder gaugeToolTipText = new StringBuilder();
            if (gaugeToolTip.ToolTipTitle != parameter.FullName)
            {// If the value has changed
                gaugeToolTip.ToolTipTitle = parameter.FullName;
                titleToolTip.SetToolTip(gaugeLabel, parameter.Description);
                if (nullAddress)
                {
                    gaugeToolTipText.AppendLine(string.Format(Manager_Translation.NotConnected, parameter.Address));
                }
            }
            if (!nullAddress)
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
                if (Timeout)
                {
                    gaugeToolTipText.AppendLine("Stale Value!");
                }
            }
            if (gaugeToolTipText.Length > 0)
            {// We changed it in this session
                gaugeToolTip.SetToolTip(gaugeDisplay, gaugeToolTipText.ToString());
            }
        }
        #endregion

        #region Size Update
        private void OnResizeEvent(object sender, EventArgs e)
        {
            gaugeDisplay.MinimumSize = new Size(gaugeLabel.Size.Width - paddingWidth, 0);
            UpdateDisplayedValue(displayValue);
        }

        /// <summary>
        /// This method adjusts output formatting with respect to 
        /// the given parameter type 
        /// </summary>
        private void CheckDisplaySize(string value)
        {
            double textWidth = TextRenderer.MeasureText(value, gaugeDisplay.Font).Width;
            bool sciNotation = false;
            bool marquee = false;
            if (textWidth > gaugeDisplay.DisplayRectangle.Width)
            {// If we are trying to display more digits than the minimum size window will allow 
                if (SciNotationEnabled && parameter.CastToType && isNumeric)
                {
                    sciNotation = true;
                }
                else if (MarqueeEnabled)
                {
                    marquee = true;
                }
            }
            IsSciNotation = sciNotation;
            if (IsMarquee != marquee)
            {
                if(marquee)
                {
                    marqueeTimer.Tick += marqueeHandler;
                }
                else
                {
                    marqueeTimer.Tick -= marqueeHandler;
                }
                IsMarquee = marquee;
            }
        }
        #endregion

        #region Value Adjusting
        private string AddUnit(string unitlessString)
        {
            try
            {
                if (parameter.Unit.Abbreviation.Length > 0 && Parameter.Unit.IsUnitDisplayble())
                {// Add the unit type abbriviateion
                    return string.Format("{0}{1}", unitlessString, parameter.Unit.Abbreviation);
                }
                return unitlessString;
            }
            catch
            {
                return unitlessString;
            }
        }

        /// <summary>
        /// This helper method is intended to be used in conjunction with the
        /// format output methods too handle the case where the number is too 
        /// long to be displayed and needs to be converted to scientific 
        /// notation in order to lower character count
        /// </summary>
        /// <param name="givenValue">String containing the output value adjusted to fit on the gauge display</param>
        /// <returns></returns>
        private string AdjLargeNum(string givenValue)
        {
            double givenValueDouble = Convert.ToDouble(givenValue);//allows for scientific notion 
            string outputValue = string.Format("{0:#.########E+0}", givenValueDouble);
            if (outputValue.Contains('E'))
            {
                string[] outputValueParts = outputValue.Split('E');
                StringBuilder significand = new StringBuilder(outputValueParts[0]);
                string exponent = outputValueParts[1];
                while (TextRenderer.MeasureText(AddUnit(outputValue), gaugeDisplay.Font).Width > gaugeDisplay.Width)
                {// While the text is too big to fit in the reduced size box, remove sig figs.
                    int at = significand.Length - 1; // Never do math twice!
                    if (significand[at] == '.')
                    {// We are coming out of decimals
                        significand.Remove(at--, 1);// Remove the decimal, then decriment at.
                    }
                    if (int.Parse(significand[at].ToString(CultureInfo.InvariantCulture)) >= 5)
                    {// We need to round up the following number.
                        int adj = 2;// Decimal and character to get rid of..
                        if (significand[0] == '-')
                        {
                            adj = 3;// Count neg sign as well..
                        }
                        significand = new StringBuilder(Math.Round(double.Parse(significand.ToString()), at - adj).ToString(CultureInfo.InvariantCulture));// C# magic                                                                                                                             
                        outputValue = string.Concat(significand.ToString(), 'E', exponent);
                    }
                    else
                    {
                        outputValue = string.Concat(significand.Remove(at, 1).ToString(), 'E', exponent);
                    }
                }
            }
            return outputValue;
        }
        #endregion

        #region Marquee
        private static void MarqueePosition(object _, EventArgs e)
        {
            marqueePosition++;
        }

        /// <summary>
        /// This event handler is meant to be triggered by the marqee timer
        /// tick event which should allow large text to be displayed in a limted area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MarqueeControl(object _, EventArgs e)
        {
            if (!String.IsNullOrEmpty(marqueeString))
            {
                int position = Convert.ToInt32(marqueePosition % Convert.ToInt64(marqueeString.Length));
                gaugeDisplay.Text = marqueeString.Substring(position) + marqueeString.Substring(0, position);
            }
        }
        #endregion

        #region Value Select
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
                LostFocus -= lostFocusHandler;
                marqueeTimer.Tick += marqueeHandler;
                gaugePanel.Focus();
            }
            else
            {
                marqueeTimer.Tick -= marqueeHandler;
                CheckDisplaySize(displayValue);
                gaugeDisplay.Focus();
                LostFocus += lostFocusHandler;
            }
        }
        #endregion

        #region Event Handler
        private void LostFocusEvents(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Default;

            if (Focused && !IsMouseOver)
            {
                LostFocus -= lostFocusHandler;
                marqueeTimer.Tick += marqueeHandler;
                gaugePanel.Focus();
            }
        }

        /// <summary>
        /// This event handler is meant to take focus off of any selected gauges when the enter key is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Gauge_EnterKeyPressed(object sender, KeyEventArgs ke)
        {
            if (ke.KeyCode == Keys.Enter)//.KeyChar == '\r')
            {// Determine if the enter key was pressed
                if (IsSelected)
                {// If it's selected, switch focus to entire gauge box, not the display
                    gaugePanel.Focus();
                }
            }
        }
        #endregion

        #region Layout
        /// <summary>
        /// Temporarily suspends usual layout logic.
        /// </summary>
        public new void SuspendLayout()
        {
            gaugePanel.SuspendLayout();
            gaugeLabel.SuspendLayout();
            gaugeDisplay.SuspendLayout();
        }

        /// <summary>
        /// Resumes usual layout logic.
        /// </summary>
        public new void ResumeLayout()
        {
            gaugePanel.ResumeLayout();
            gaugeLabel.ResumeLayout();
            gaugeDisplay.ResumeLayout();
        }
        #endregion

        #endregion /Methods

        #region Dispose
        public override void Dispose()
        {
            try
            {
                if (RegistrationTicket_Gauge != null)
                {
                    RegistrationTicket_Gauge.Active = false;
                    RegistrationTicket_Gauge.Dispose();
                }
                marqueeTimer.Tick -= marqueeHandler;
                marqueeTimer?.Dispose();
                lock (gaugePanel)
                {
                    gaugeDisplay?.Dispose();
                }
                lock (gaugePanel)
                {
                    gaugePanel?.Dispose();
                }
                lock(gaugeLabel)
                { 
                    gaugeLabel?.Dispose();
                }
            }
            finally
            {
                base.Dispose();
                Manager_Runtime.Cancel(this);
            }
        }
        #endregion
    }

    #region Drag Gauge Class
    public class DragGauge : IEquatable<Gauge>
    {
        private Gauge gauge;
        public Gauge Gauge
        {
            get
            {
                return gauge;// TODO: IGauge
            }
        }
        public IParameter Parameter
        {
            get
            {
                return gauge.Parameter;
            }
            set
            {
                gauge.Parameter = value;
            }
        }

        private readonly AlliedSemaphore draggingWaitHandle = new AlliedSemaphore(1, 1);
        private bool inDrag;
        public bool InDrag
        {
            get
            {
                return inDrag;
            }
            set
            {
                inDrag = value;
                gauge.InDrag = inDrag;
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

        public Control HostControl
        {
            get
            {
                return gauge.GaugeContainer;
            }
        }
        public DragGauge(Gauge _gauge)
        {
            gauge = _gauge;
        }
        public void SetGauge(Gauge _gauge)
        {
            gauge.InDrag = false;
            gauge = _gauge;
        }

        public void SetParameter(IParameter parameter)
        {
            gauge.Parameter = parameter;
        }

        public void Select()
        {
            gauge.SelectControl();
        }

        public bool Equals(Gauge other)
        {
            if (other == null) return false;
            return gauge == other;
        }

    }

    #endregion
}

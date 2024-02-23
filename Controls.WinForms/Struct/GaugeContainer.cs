using Common.Extensions;
using Datam.WinForms.Base;
using Datam.WinForms.Constant;
using Datam.WinForms.Constants;
using Datam.WinForms.Controls;
using Parameters;
using Parameters.Interface;
using Runtime;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Datam.WinForms.Struct
{
    public delegate void GaugePanelHandler();
    public class GaugeContainer
    {
        #region Identity
        public const String StructName = nameof(GaugeContainer);
        #endregion /Identity

        #region Events
        public event MouseEventHandler MouseUp;
        private readonly MouseEventHandler mouseUp_Handler;
        public event MouseEventHandler MouseDown;
        private readonly MouseEventHandler mouseDown_Handler;
        public event MouseEventHandler MouseMove;
        private readonly MouseEventHandler mouseMove_Handler;
        public event EventHandler MouseLeave;
        private readonly EventHandler mouseLeave_Handler;
        public event GaugePanelHandler UpdateGaugePanelSize;
        #endregion /Events

        #region Accessors

        #region DisplayPanel 
        public Boolean AutoScrollRequired { get; private set; }
        public Size AutoScrollMinSize { get; private set; }
        #endregion DisplayPanel 

        #region Color
        private Color foreColor;
        public Color ForeColor 
        { 
            get
            {
                return foreColor;
            }
            set
            {
                foreColor = value;
                for (int g = 0; g < gauges.Length; g++)
                {
                    if (gauges[g].ForeColor != value)
                    {
                        gauges[g].ForeColor = value;
                    }
                }
            }
        }

        private Color backColor;
        public Color BackColor 
        { 
            get
            {
                return backColor;
            }
            set
            {
                backColor = value;
                for (int g = 0; g < gauges.Length; g++)
                {
                    if (gauges[g].BackColor != value)
                    {
                        gauges[g].BackColor = value;
                    }
                }
            }
        }
        #endregion /Color

        #region Font

        #region Title
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
                for (int g = 0; g < gauges.Length; g++)
                {
                    if (gauges[g].Font != value)
                    {
                        gauges[g].Font = value;
                    }
                }
            }
        }
        #endregion /Title

        #region Value
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
            }
        }
        #endregion /Value

        #region Font Size Choice
        private FontSizeChoices fontSize;
        public FontSizeChoices FontSize
        {
            get
            {
                return fontSize;
            }
            set
            {
                fontSize = value;
                for (int g = 0; g < gauges.Length; g++)
                {
                    if (gauges[g].FontSize != value)
                    {
                        gauges[g].FontSize = value;
                    }
                }
            }
        }
        #endregion /Font Size Choice

        #endregion /Font

        #region Gauges
        private Gauge_Base[] gauges;

        private UInt32 count;
        /// <summary>
        /// Shows the number of visible gauges.
        /// </summary>
        public UInt32 Count
        {
            get
            {
                return count;
            }
            set
            {
                if (value != count)
                {
                    value = Math.Max(value, MinCount);
                    value = Math.Min(value, MaxCount);
                    UpdateGaugeCount(value);
                    count = value;
                    ResizeGaugePanel();
                }
            }
        }

        private UInt32 maxCount;
        public UInt32 MaxCount
        {
            get
            {
                return maxCount;
            }
            set
            {
                maxCount = value;
                if (gauges == null || gauges.Length < value)
                {
                    UpdateMaxGaugeCount(value);
                }
            }
        }

        private UInt32 minCount;
        public UInt32 MinCount
        {
            get
            {
                return minCount;
            }
            set
            {
                minCount = value;
                if (gauges == null || gauges.Length < value)
                {
                    UpdateMaxGaugeCount(value);
                }
            }
        }

        private Int32 gaugeWidth;
        public Int32 Width
        {
            get
            {
                return gaugeWidth;
            }
            set
            {
                gaugeWidth = value;
                if (gauges != null)
                {
                    for (int g = 0; g < gauges.Length; g++)
                    {
                        if (gauges[g].Width != value)
                        {
                            gauges[g].Width = value;
                        }
                    }
                }
            }
        }
        #endregion /Gauges

        #region Control Settings
        private LeftRightAlignment mouseButton_ClickSelect;
        public LeftRightAlignment MouseButton_ClickSelect
        {
            get
            {
                return mouseButton_ClickSelect;
            }
            set
            {
                mouseButton_ClickSelect = value;
                for (int g = 0; g < gauges.Length; g++)
                {
                    if (gauges[g].MouseButton_ClickSelect != value)
                    {
                        gauges[g].MouseButton_ClickSelect = value;
                    }
                }
            }
        }
        #endregion /Control Settings

        #region Display
        private Boolean isDisplayPanel = false;
        private TableLayoutPanel displayPanel;
        public TableLayoutPanel DisplayPanel 
        { 
            get
            {
                return displayPanel;
            }
            set
            {
                if(displayPanel != value)
                {
                    displayPanel = value;
                    isDisplayPanel = displayPanel != null;
                }
            }
        }
        #endregion /Display

        #region Display Settings
        private Boolean marqueeEnabled;
        public Boolean MarqueeEnabled
        {
            get
            {
                return marqueeEnabled;
            }
            set
            {
                marqueeEnabled = value;
                for (int g = 0; g < gauges.Length; g++)
                {
                    if (gauges[g].MarqueeEnabled != value)
                    {
                        gauges[g].MarqueeEnabled = value;
                    }
                }
            }
        }

        private Boolean scientificNotationEnabled;
        public Boolean ScientificNotationEnabled
        {
            get
            {
                return scientificNotationEnabled;
            }
            set
            {
                scientificNotationEnabled = value;
                for (int g = 0; g < gauges.Length; g++)
                {
                    if (gauges[g].ScientificNotationEnabled != value)
                    {
                        gauges[g].ScientificNotationEnabled = value;
                    }
                }
            }
        }
        #endregion /Display Settings

        #endregion /Accessors

        #region Update
        public void Update()
        {
            lock (gauges)
            {
                foreach (Control control in gauges)
                {
                    if (control is Gauge_Numeric numericGauge)
                    {
                        numericGauge.UpdateValue();
                    }
                }
            }
        }
        #endregion /Update

        #region Constructor
        public GaugeContainer()
        {
            gauges = Array.Empty<Gauge_Base>();
            mouseUp_Handler = new MouseEventHandler(OnMouseUp);
            mouseDown_Handler = new MouseEventHandler(OnMouseDown);
            mouseMove_Handler = new MouseEventHandler(OnMouseMove);
            mouseLeave_Handler = new EventHandler(OnMouseLeave);
        }
        #endregion /Constructor

        #region Gauge Count
        /// <summary>
        /// This function ensures the gauge number is within the tolerances 
        /// set in the settings form.
        /// </summary>
        /// <param name="GaugeNumber">Gauge number to check if in acceptable range</param>
        /// <returns>Gauge number within acceptable range</returns>
        private void UpdateMaxGaugeCount(UInt32 newCount)
        {
            Log_Manager.LogMethodCall(StructName, nameof(UpdateGaugeCount));
            lock (gauges)
            {
                try
                {
                    if (gauges.Length < maxCount)
                    {
                        uint previousLength = Convert.ToUInt32(gauges.Length);
                        Array.Resize(ref gauges, Convert.ToInt32(newCount));
                        for (uint g = previousLength; g < newCount; g++)
                        {
                            AddGaugeToContainer(g);
                        }
                    }
                    else if (gauges.Length > maxCount)
                    {
                        uint previousLength = Convert.ToUInt32(gauges.Length);
                        Array.Resize(ref gauges, Convert.ToInt32(newCount));
                    }
                }
                catch (Exception ex)
                {
                    Log_Manager.LogError(StructName, ex.Message);
                }
            }
        }

        private void AddGaugeToContainer(UInt32 gaugeIndex, GaugeType gaugeType = GaugeType.Standard)
        {
            lock (gauges)
            {
                Gauge_Base gauge;
                switch (gaugeType)
                {
                    default:
                    case GaugeType.Standard:
                        gauge = new Gauge
                        {
                            Visible = gaugeIndex < Count,// If we are less than or equal to the number desired to be shown
                            ForeColor = ForeColor,
                            BackColor = BackColor,
                            MouseButton_ClickSelect = MouseButton_ClickSelect,
                            Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right
                        };
                        break;
                    case GaugeType.Numeric:
                        gauge = new Gauge_Numeric
                        {
                            Visible = gaugeIndex < Count,// If we are less than or equal to the number desired to be shown
                            ForeColor = ForeColor,
                            BackColor = BackColor,
                            MouseButton_ClickSelect = MouseButton_ClickSelect,
                            Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right
                        };
                        break;
                }
                gauge.MouseUp += mouseUp_Handler;
                gauge.MouseDown += mouseDown_Handler;
                gauge.MouseMove += mouseMove_Handler;
                gauge.MouseLeave += mouseLeave_Handler;
                gauges[gaugeIndex] = gauge;
                RefreshGauges();
            }
        } 

        public void ResizeGaugePanel()
        {
            if (isDisplayPanel)
            {
                lock (DisplayPanel)
                {
                    try
                    {
                        DisplayPanel.SuspendLayout();
                        int offset = (int)Count * (gauges[0].Margin.Vertical + gauges[0].Padding.Vertical);
                        int gaugeTotalHeight = (int)(Count * gauges[0].Height);
                        AutoScrollRequired = (Count * gauges[0].Height) + offset > DisplayPanel.Height;
                        if (DisplayPanel.HorizontalScroll.Visible)
                        {
                            offset += SystemInformation.HorizontalScrollBarHeight;
                        }
                        AutoScrollMinSize = new Size(gauges[0].Width, gaugeTotalHeight + offset);                      
                    }
                    finally
                    {
                        DisplayPanel.ResumeLayout();
                        UpdateGaugePanelSize?.Invoke();
                    }
                }
            }
        }

        public void RefreshGauges()
        {
            if (isDisplayPanel)
            {
                lock (DisplayPanel)
                {
                    try
                    {
                        DisplayPanel.SuspendLayout();
                        if (DisplayPanel.RowCount < gauges.Length)
                        {
                            int gaugesPlus1 = gauges.Length;
                            DisplayPanel.RowCount = gaugesPlus1;
                            DisplayPanel.RowStyles.Clear();
                            DisplayPanel.AddRowStyle_AutoSize(Convert.ToUInt32(gauges.Length));
                            //DisplayPanel.AddRowStyle_Percent(1, 100);
                        }
                        DisplayPanel.Controls.Clear();
                        for(int g = 0; g<gauges.Length; g++)
                        {
                            if (gauges[g] != null)
                            {
                                DisplayPanel.Controls.Add(gauges[g], 0, g);
                            }
                        }
                    }
                    finally
                    {
                        DisplayPanel.ResumeLayout();
                    }
                }
            }
        }

        /// <summary>
        /// This function ensures the gauge number is within the tolerances 
        /// set in the settings form.
        /// </summary>
        /// <param name="GaugeNumber">Gauge number to check if in acceptable range</param>
        /// <returns>Gauge number within acceptable range</returns>
        private void UpdateGaugeCount(UInt32 newCount)
        {
            Log_Manager.LogMethodCall(StructName, nameof(UpdateGaugeCount));
            lock (gauges)
            {
                if (newCount <= gauges.Length)
                {
                    try
                    {
                        if (Count < newCount)
                        {
                            for (uint g = 0; g < newCount; g++)
                            {
                                ShowGaugeInContainer(g);
                            }
                        }
                        else if (Count > newCount)
                        {
                            for (uint g = newCount; g >= newCount; g--)
                            {
                                HideGaugeInContainer(g);
                            }
                        }
                        RefreshGauges();
                    }
                    catch (Exception ex)
                    {
                        Log_Manager.LogError(StructName, ex.Message);
                    }
                }
            }
        }

        private void ShowGaugeInContainer(UInt32 gaugeIndex)
        {
            lock (gauges)
            {
                gauges[gaugeIndex].Visible = true;
            }
        }

        private void HideGaugeInContainer(UInt32 gaugeIndex)
        {
            lock (gauges)
            {
                gauges[gaugeIndex].Visible = false;
                if (gauges[gaugeIndex] is Gauge_Numeric numericGauge)
                {
                    numericGauge.Parameter = Parameter_CiA402.Null;
                }
            }
        }
        #endregion /Gauge Count

        #region Add-Remove
        public void Add(UInt32 number = 1)
        {
            uint max = Math.Min(maxCount, Count + number);
            Count = max;
        }

        public void Remove(UInt32 number = 1)
        {
            number = Math.Min(number, Count);// We don't want overflows.
            uint min = Math.Max(minCount, Count - number);
            Count = min;
        }
        #endregion /Add-Remove

        #region Swap
        public void SwapGauges(Gauge_Base gauge1, Gauge_Base gauge2)
        {
            lock (gauges)
            {
                if (TryGetGaugeIndex(gauge1, out uint at1) && TryGetGaugeIndex(gauge2, out uint at2))
                {
                    gauges.Swap(at1, at2);
                    RefreshGauges();
                }
            }
        }
        #endregion /Swap

        #region Get
        public Boolean TryGetGaugeIndex(Gauge_Base gauge, out UInt32 atIndex)
        {
            for (atIndex = 0; atIndex < gauges.Length; atIndex++)
            {
                if (gauges[atIndex] == gauge)
                {
                    return true;
                }
            }
            return false;
        }

        public bool TryGetGauge(UInt32 atIndex, out Gauge_Base gauge)
        {
            lock (gauges)
            {
                if (atIndex < Count)
                {
                    gauge = gauges[atIndex];
                    return true;
                }
            }
            gauge = default;
            return false;
        }

        public Boolean TryGetGauge_AtCursor(out Gauge_Base gauge)
        {
            lock (gauges)
            {
                for (int g = 0; g < gauges.Length; g++)
                {
                    if (gauges[g].ClientRectangle.Contains(gauges[g].PointToClient(Cursor.Position)))
                    {
                        gauge = gauges[g];
                        return true;
                    }
                }
            }
            gauge = default;
            return false;
        }
        #endregion /Get

        #region Set
        public Boolean TrySetGauge(IParameter parameter)
        {
            lock (gauges)
            {
                if (Count < MaxCount)
                {
                    return TrySetGauge(parameter, Count++);
                }
            }
            return false;
        }

        public Boolean TrySetGauge(IParameter parameter, Gauge_Base gauge)
        {
            lock (gauges)
            {
                if (TryGetGaugeIndex(gauge, out uint atIndex))
                {
                    return TrySetGauge(parameter, atIndex);
                }
            }
            return false;
        }

        public Boolean TrySetGauge(IParameter parameter, UInt32 atIndex)
        {
            lock (gauges)
            {
                if (atIndex < MaxCount)
                {
                    if (gauges[atIndex] is Gauge_Base gauge)
                    {
                        bool isStandardGauge = gauge is Gauge;
                        if (isStandardGauge)
                        {
                            if (parameter.IsNumeric)
                            {
                                AddGaugeToContainer(atIndex, GaugeType.Numeric);
                            }
                        }
                        else if(parameter.IsNullType)
                        {// Numeric gauge
                            AddGaugeToContainer(atIndex, GaugeType.Standard);
                        }
                        gauges[atIndex].Parameter = parameter;
                        gauges[atIndex].Visible = true;
                    }
                }
            }
            return false;
        }
        #endregion /Set

        #region Mouse Up
        private void OnMouseUp(Object sender, MouseEventArgs mea)// Culpa.
        {
            MouseUp?.Invoke(sender, mea);
        }
        #endregion /Mouse Up

        #region Mouse Down
        private void OnMouseDown(Object sender, MouseEventArgs mea)// Culpa.
        {
            MouseDown?.Invoke(sender, mea);
        }
        #endregion /Mouse Down

        #region Mouse Move
        private void OnMouseMove(Object sender, MouseEventArgs mea)// Culpa.
        {
            MouseMove?.Invoke(sender, mea);
        }
        #endregion /Mouse Move

        #region Mouse Leave
        private void OnMouseLeave(Object sender, EventArgs mea)// Culpa.
        {
            MouseLeave?.Invoke(sender, mea);
        }
        #endregion /Mouse Leave
    }
}

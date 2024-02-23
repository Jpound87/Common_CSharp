using Common.Constant;
using Common.Extensions;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;

namespace Common.Controls
{


    public class SwitchControl : Control
    {
        #region Identity
        public const String ControlName = nameof(SwitchControl);
        private static readonly Assembly assembly = Assembly.GetExecutingAssembly();
        #endregion /Identity

        #region Constants
        private static readonly Color defaultColor = "#FFFFFF".FromHex();
        private static readonly SolidBrush defaultColorBrush = new(defaultColor);
        private static readonly Color accentColor = Color.DarkGray;
        private static readonly SolidBrush accentColorBrush = new SolidBrush(accentColor);

        private const Double IMAGE_PADDING_RATIO = .07;// The portion of the rectandle to reserve for padding on each edge.
        private const Double IMAGE_PADDING_RATIO_2X = 2 * IMAGE_PADDING_RATIO;
        private const Double IMAGE_PADDING_RATIO_2X_COMPLIMENT = 1 - IMAGE_PADDING_RATIO_2X;

        private static readonly String onIconDir = String.Format("{0}symbol_On_96.png", Tokens.IMAGE_DIRECTORY_COMMON);
        private static readonly String offIconDir = String.Format("{0}symbol_Off_96.png", Tokens.IMAGE_DIRECTORY_COMMON);

        private static readonly Bitmap imgOnIcon = assembly.GetImage(onIconDir);
        private static readonly Bitmap imgOffIcon = assembly.GetImage(offIconDir);
        #endregion /Constants

        #region Readonly
        private readonly Timer paintTicker = new Timer()
        {
            Interval = 1
        };

        private readonly EventHandler paintTick_Handler;
        #endregion

        #region Events
        public event EventHandler ValueChanged;
        #endregion /Events

        #region Globals

        #region Shape
        private RectangleF circle;
        private Override_Rectangle_Switch baseRectangle;
        private PointF centerLeft;
        private PointF centerRight;
        private RectangleF rectangeLeft;
        private RectangleF rectangeRight;
        #endregion

        #region State
        private Boolean isOn = false;
        /// <summary>
        /// Indicates if the control is int he 'On' or 'Off' switched state.
        /// </summary>
        [Category("HMI Properties")]
        public Boolean IsOn
        {
            get => isOn;
            set
            {
                if (isOn != value)
                {
                    paintTicker.Stop();
                    isOn = value;
                    paintTicker.Start();
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private Boolean warningState;
        /// <summary>
        /// Indicates if the control is displaying a warning state. 
        /// This is used to indicate the control state does not match
        /// the underlying data state.
        /// </summary>
        [Category("HMI Properties")]
        public Boolean WarningState
        {
            get
            {
                return warningState; 
            }
            set
            {
                if (warningState != value)
                {
                    warningState = value;
                    if (warningState)
                    {
                        OnColor = Color.YellowGreen;
                        OffColor = Color.Yellow;
                    }
                    else
                    {
                        OnColor = onColorSetting;
                        OffColor = offColorSetting;
                    }
                }
            }
        }

        private ControlIconography iconography = ControlIconography.Text;
        /// <summary>
        /// Used to select the type of icon the control should display to indicate its state.
        /// </summary>
        [Category("HMI Properties")]
        public ControlIconography Iconography
        {
            get => iconography;
            set
            {
                if (iconography != value)
                {
                    iconography = value;
                    Refresh();
                }
            }
        }
        #endregion /State

        #region Text

        private String onText = "On";
        [Category("HMI Properties")]
        public String OnText
        {
            get => onText;
            set
            {
                if (onText != value)
                {
                    onText = value;
                    Refresh();
                }
            }
        }

        private String offText = "Off";
        [Category("HMI Properties")]
        public String OffText
        {
            get => offText;
            set
            {
                if (offText != value)
                {
                    offText = value;
                    Refresh();
                }
            }
        }
        #endregion /Text

        #region Image

        //#region On
        //int onImage_HalfWidth = 0;
        //int onImage_HalfHeight = 0;
        //private Image onImage = imgOnIcon;
        //[Category("HMI Properties")]
        //public Image OnImage
        //{
        //    get => onImage;
        //    set
        //    {
        //        if (value!= null && onImage != value)
        //        {
        //            if (value.Size.HasDimensionGreater(halfSizeHorizontal_Padded))
        //            {
        //                onImage = value.ScaleImage(halfSizeHorizontal_Padded);
        //            }
        //            else
        //            {
        //                onImage = value;
        //            }
        //            onImage_HalfWidth = onImage.Width / 2;
        //            onImage_HalfHeight = onImage.Height / 2;
        //            Refresh();
        //        }
        //    }
        //}
        //[Category("HMI Properties")]
        //public int OnImage_Offset_Vertical { get; set; }
        //[Category("HMI Properties")]
        //public int OnImage_Offset_Horizontal { get; set; }
        //#endregion

        //#region Off
        //int offImage_HalfWidth = 0;
        //int offImage_HalfHeight = 0;
        //private Image offImage = imgOffIcon;
        //[Category("HMI Properties")]
        //public Image OffImage
        //{
        //    get => offImage;
        //    set
        //    {
        //        if (value != null && offImage != value)
        //        {
        //            if (value.Size.HasDimensionGreater(halfSizeHorizontal_Padded))
        //            {
        //                offImage = value.ScaleImage(halfSizeHorizontal_Padded);
        //            }
        //            else
        //            {
        //                offImage = value;
        //            }
        //            offImage_HalfWidth = offImage.Width / 2;
        //            offImage_HalfHeight = offImage.Height / 2;
        //            Refresh();
        //        }
        //    }
        //}
        //[Category("HMI Properties")]
        //public int OffImage_Offset_Vertical{ get; set; }
        //[Category("HMI Properties")]
        //public int OffImage_Offset_Horizontal { get; set; }
        //#endregion

        #endregion /Image

        #region Color

        #region Switch
        private bool colorSwitch;
        [Category("HMI Properties")]
        public bool ColorSwitch
        {
            get => colorSwitch;
            set
            {
                if (colorSwitch != value)
                {
                    colorSwitch = value;
                    Refresh();
                }
            }
        }
        #endregion /Switch

        #region On
        private Color onColor;
        private Color onColorSetting = Color.FromArgb(94, 148, 255);
        [Category("HMI Properties")]
        public Color OnColor
        {
            get => onColor;
            set
            {
                if (onColor != value)
                {
                    if (!WarningState)
                    {
                        onColor = value;
                    }
                    onColorSetting = value;
                    Refresh();
                }
            }
        }
        #endregion /On

        #region Off
        private Color offColor;
        private Color offColorSetting = accentColor;
        [Category("HMI Properties")]
        public Color OffColor
        {
            get => offColor;
            set
            {
                if (offColor != value)
                {
                    if (!WarningState)
                    {
                        offColor = value;
                    }
                    offColorSetting = value;
                    Refresh();
                }
            }
        }
        #endregion /Off

        #region Border
        private Color borderColor = accentColor;
        [Category("HMI Properties")]
        public Color BorderColor
        {
            get => borderColor;
            set
            {
                if (borderColor != value)
                {
                    borderColor = value;
                    Refresh();
                }
            }
        }
        #endregion /Border

        #region Fill
        private Color fillColor = defaultColor;
        [Category("HMI Properties")]
        public Color FillColor
        {
            get => fillColor;
            set
            {
                if (fillColor != value)
                {
                    fillColor = value;
                    Refresh();
                }
            }
        }
        #endregion /Fill

        #region Background
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override Color BackColor 
        {
            get => defaultColor;
        }
        #endregion /Background

        #endregion /Color

        #region Size
        private Single artis = 4;
        private Single diameter = 30;
        private Size halfSizeHorizontal;
        private Size halfSizeHorizontal_Padded;
        protected override Size DefaultSize => new(60, 35);
        #endregion /Size

        #endregion /Globals

        #region Constructor
        public SwitchControl()
        {
            Cursor = Cursors.Hand;
            DoubleBuffered = true;
            baseRectangle = new Override_Rectangle_Switch(2f * diameter, diameter + 2f, diameter / 2f);
            circle = new RectangleF(1f, 1f, diameter, diameter);
            SetBoundingBoxes();
            paintTick_Handler = new EventHandler(PaintTicker_Tick);
            paintTicker.Tick += paintTick_Handler;
        }
        #endregion /Constructor

        #region Methods

        #region Enabled
        protected override void OnEnabledChanged(EventArgs e)
        {
            Refresh();
            base.OnEnabledChanged(e);
        }
        #endregion

        #region Mouse Event Handlers
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                IsOn = !IsOn;
                OnMouseClick(e);
            }
        }
        #endregion

        #region Draw
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            graphics.SmoothingMode = SmoothingMode.HighQuality;

            SolidBrush parentColorBrush = new SolidBrush(Parent.BackColor);
            SolidBrush backColorBrush = new SolidBrush(FillColor);
            SolidBrush stateColorBrush = colorSwitch ? new SolidBrush(isOn ? onColor : offColor) : accentColorBrush;
            
            graphics.FillRectangle(parentColorBrush, baseRectangle.Shape);
            graphics.FillPath(backColorBrush, baseRectangle.Path);
            using (Pen pen = new Pen(borderColor, 2f))
            {
                graphics.DrawPath(pen, baseRectangle.Path);
            }
            switch (Iconography)
            {
                case ControlIconography.Text:
                    using (Font font = new Font("Century Gothic", (8.2f * diameter) / 30f, FontStyle.Bold))
                    {
                        SolidBrush b = new SolidBrush(ForeColor);
                        Size onTextSize = TextRenderer.MeasureText(onText, font);
                        int offTextWidth = TextRenderer.MeasureText(offText, font).Width;
                        float onOffset = diameter / 2;
                        float offWidthOffset = (diameter + onOffset) - (offTextWidth/2);
                        onOffset -= (onTextSize.Width / 2);
                        float heightOffset = (diameter - onTextSize.Height) / 2f;
                        graphics.DrawString(onText, font, b, onOffset, heightOffset + 1f);
                        graphics.DrawString(offText, font, b, offWidthOffset, heightOffset + 1f);
                    }
                    goto case ControlIconography.None;
                case ControlIconography.Image:
                    using (Font font = new Font("Century Gothic", (8.2f * diameter) / 30f, FontStyle.Bold))
                    {
                        //PointF adjCenterLeft = new PointF(centerLeft.X - onImage_HalfWidth + OnImage_Offset_Horizontal, centerLeft.Y - onImage_HalfHeight + OnImage_Offset_Vertical);
                        //PointF adjCenterRight = new PointF(centerRight.X - offImage_HalfWidth + OffImage_Offset_Horizontal, centerRight.Y - offImage_HalfHeight + OffImage_Offset_Vertical);
                  
                        //graphics.DrawImage(onImage, adjCenterLeft);
                        //graphics.DrawImage(offImage, adjCenterRight);
                    }
                    goto case ControlIconography.None;
                case ControlIconography.None:
                    DrawInterior(graphics, stateColorBrush);
                    break;
            }
            base.OnPaint(e);
        }

        private void DrawInterior(Graphics graphics, Brush stateColorBrush)
        {
            graphics.FillEllipse(stateColorBrush, circle);
            using (Pen pen = new Pen(borderColor, 1.2f))
            {
                graphics.DrawEllipse(pen, circle);
            }
        }
        #endregion /Draw

        #region Size

        protected override void OnResize(EventArgs e)
        {
            SetBoundingBoxes();
            base.OnResize(e);
        }

        private void SetBoundingBoxes()
        {
            Width = (Height - 2) * 2;
            diameter = Width / 2;
            artis = (4f * diameter) * 30f;
            baseRectangle = new Override_Rectangle_Switch(2f * diameter, diameter + 2f, diameter / 2f);
            circle = new RectangleF(!isOn ? 1f : ((Width - diameter) - 1f), 1f, diameter, diameter);
            halfSizeHorizontal = new Size(Width / 2, Height);
            int paddedWidth = Convert.ToInt32(halfSizeHorizontal.Width * IMAGE_PADDING_RATIO_2X_COMPLIMENT);
            int paddedHeight = Convert.ToInt32(halfSizeHorizontal.Height * IMAGE_PADDING_RATIO_2X_COMPLIMENT);
            halfSizeHorizontal_Padded = new Size(paddedWidth, paddedHeight);
        
            float halfHeight = Height / 2;
            float quaterWidth = Width / 4;

            PointF rightLocation = new PointF(Location.X + rectangeLeft.Width, Location.Y);

            centerRight = new PointF(Location.X + 3 * (quaterWidth), Location.Y + halfHeight);
            centerLeft = new PointF(Location.X + quaterWidth, Location.Y + halfHeight);

            rectangeLeft = new RectangleF(Location, halfSizeHorizontal);
            rectangeRight = new RectangleF(rightLocation, halfSizeHorizontal);
        }

        #endregion /Size

        #region Motion Simulation
        /// <summary>
        /// This method draws the transitional states so the control appears to be moving.
        /// </summary>
        /// <param name="_"></param>
        /// <param name="e"></param>
        private void PaintTicker_Tick(object _, EventArgs e)
        {
            float x = circle.X;
            if (isOn)
            {
                if ((x + artis) <= ((Width - diameter) - 1f))
                {
                    x += artis;
                    circle = new RectangleF(x, 1f, diameter, diameter);
                    Refresh();
                }
                else
                {
                    x = (Width - diameter) - 1f;
                    circle = new RectangleF(x, 1f, diameter, diameter);
                    Refresh();
                    paintTicker.Stop();
                }
            }
            else if ((x - artis) >= 1f)
            {
                x -= artis;
                circle = new RectangleF(x, 1f, diameter, diameter);
            }
            else
            {
                x = 1f;
                circle = new RectangleF(x, 1f, diameter, diameter);
                Refresh();
                paintTicker.Stop();
            }
        }
        #endregion

        #endregion /Methods

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            try
            {
                paintTicker.Tick -= paintTick_Handler;
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
        #endregion
    }
}

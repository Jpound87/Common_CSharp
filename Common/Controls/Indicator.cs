using Common.Constant;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Common.Controls
{
    public partial class Indicator : UserControl
    {
        #region Identity
        public const String ControlName = nameof(Indicator);
        #endregion /Identity

        #region Constants
        private const Int32 ARRAY_SIZE = 4;

        private const Double OUTER_BORDER_RATIO = 1;
        private const Double OUTER_BORDER_EMISSIVE_OFFSET = .15;
        private const Double INNER_BORDER_RATIO = OUTER_BORDER_RATIO - .05;
        private const Double INDICATOR_BOX_RATIO = INNER_BORDER_RATIO - .05;

        private static readonly Size AUTOSIZE_PADDING = new(60, 20);

        public const Indicator_Shape DEFAULT_LED_SHAPE = Indicator_Shape.Rectangle;

        public static readonly Color DEFAULT_COLOR_ON = Color.Lime;
        public static readonly Color DEFAULT_COLOR_OFF = Color.DarkGray;
        public static readonly Color DEFAULT_COLOR_BORDER = Color.DimGray;
        public static readonly Color DEFAULT_COLOR_BORDER_ACCENT = Color.WhiteSmoke;

        public static readonly Color DEFAULT_TEXT_COLOR = Color.White;

        public static readonly Color DEFAULT_TEXT_HIGHLIGHT_COLOR = Color.Yellow;
        #endregion /Constants

        #region Globals

        #region Properties

        #region AutoSize
        private Boolean autoSize = false;
        [Category("HMI Properties")]
        public override Boolean AutoSize
        {
            get { return autoSize; }
            set
            {
                if (autoSize != value)
                {
                    autoSize = value;
                    SetSizeFromText();
                }
            }
        }
        #endregion /AutoSize

        #region State
        private Indicator_State state = Indicator_State.Inactive;
        [Category("HMI Properties")]
        public Indicator_State State
        {
            get 
            { 
                return state; 
            }
            set
            {
                if (state != value)
                {
                    state = value;
                    On = state == Indicator_State.On;
                    switch (state)// Value = true; LED is on
                    {
                        case Indicator_State.On:
                            colorArray = new Color[1] { OnColor };
                            textColorSetting = highlighted ? fontHighlightColor : OnTextColor;
                            break;
                        case Indicator_State.Off:
                            colorArray = new Color[1] { OffColor };
                            textColorSetting = highlighted ? fontHighlightColor : OffTextColor;
                            break;
                        case Indicator_State.Override:
                            colorArray = new Color[1] { OverrideColor };
                            textColorSetting = highlighted ? fontHighlightColor : OverrideTextColor;
                            break;
                        case Indicator_State.Inactive:
                        default:
                            colorArray = new Color[1] { InactiveColor };
                            textColorSetting = highlighted ? fontHighlightColor : InactiveTextColor;
                            break;
                    }
                    Refresh();
                }
            }
        }
        private Boolean on = false;
        public Boolean On
        {
            get
            {
                return on;
            }
            private set
            {
                if(on != value)
                {
                    on = value;
                }
            }
        }
        #endregion /State

        #region Shape
        private Indicator_Shape shape = DEFAULT_LED_SHAPE;
        [Category("HMI Properties")]
        public Indicator_Shape Shape
        {
            get { return shape; }
            set
            {
                if (shape != value)
                {
                    shape = value;
                    SetShape();
                }
            }
        }

        public void SetShape()
        {
            switch (shape)
            {
                case Indicator_Shape.Circle:
                    addShape = new Action<GraphicsPath, Single, Single, float, float>((path, x, y, w, h) =>
                    {
                        path.AddEllipse(x, y, w, h);
                    });
                    fillShape = new Action<Brush, Single, Single, float, float>(g.FillEllipse);
                    break;

                case Indicator_Shape.Rectangle:
                    addShape = new Action<GraphicsPath, Single, Single, float, float>((path, x, y, w, h) =>
                    {
                        path.AddRectangle(new RectangleF(x, y, w, h));
                    });
                    fillShape = new Action<Brush, Single, Single, Single, Single>(g.FillRectangle);
                    break;
            }
            Refresh();
        }
        #endregion /Shape

        #region Graphic Effects
        private Boolean emissive = true;
        [Category("HMI Properties")]
        public Boolean Emissive
        {
            get { return emissive; }
            set
            {
                emissive = value;
                SizeCalculations();
                Refresh();
            }
        }

        private Boolean gradient = false;
        [Category("HMI Properties")]
        public Boolean Gradient
        {
            get 
            { 
                return gradient; 
            }
            set
            {
                gradient = value;
                SizeCalculations();
                Refresh();
            }
        }
        #endregion /Graphic Effects

        #region Color

        #region Setting
        private Color textColor_Regular;
        private Color[] colorArray;
        private Color textColorSetting = DEFAULT_TEXT_COLOR;
        #endregion /Setting

        #region On
        private Color onColor = DEFAULT_COLOR_ON;
        [Category("HMI Properties")]
        public Color OnColor
        {
            get { return onColor; }
            set
            {
                onColor = value;
                Refresh();
            }
        }

        private Color onColor_Text = DEFAULT_TEXT_COLOR;
        [Category("HMI Properties")]
        public Color OnTextColor
        {
            get { return onColor_Text; }
            set
            {
                onColor_Text = value;
                Refresh();
            }
        }
        #endregion

        #region Off
        private Color offColor = DEFAULT_COLOR_OFF;
        [Category("HMI Properties")]
        public Color OffColor
        {
            get { return offColor; }
            set
            {
                offColor = value;
                Refresh();
            }
        }

        private Color offColor_Text = DEFAULT_TEXT_COLOR;
        [Category("HMI Properties")]
        public Color OffTextColor
        {
            get { return offColor_Text; }
            set
            {
                offColor_Text = value;
                Refresh();
            }
        }
        #endregion

        #region Override
        private Color overrideColor = DEFAULT_COLOR_OFF;
        [Category("HMI Properties")]
        public Color OverrideColor
        {
            get { return overrideColor; }
            set
            {
                overrideColor = value;
                Refresh();
            }
        }

        private Color overrideColor_Text = DEFAULT_TEXT_COLOR;
        [Category("HMI Properties")]
        public Color OverrideTextColor
        {
            get { return overrideColor_Text; }
            set
            {
                overrideColor_Text = value;
                Refresh();
            }
        }
        #endregion /Override

        #region Inactive
        private Color inactiveColor = DEFAULT_COLOR_OFF;
        [Category("HMI Properties")]
        public Color InactiveColor
        {
            get { return inactiveColor; }
            set
            {
                inactiveColor = value;
                Refresh();
            }
        }

        private Color inactiveColor_Text = DEFAULT_TEXT_COLOR;
        [Category("HMI Properties")]
        public Color InactiveTextColor
        {
            get { return inactiveColor_Text; }
            set
            {
                inactiveColor_Text = value;
                Refresh();
            }
        }
        #endregion /Inactive

        #region Border
        [Category("HMI Properties")]
        public Boolean StyledBorder { get; set; }

        private Color borderColor_Light = DEFAULT_COLOR_BORDER_ACCENT;
        [Category("HMI Properties")]
        public Color BorderAccent
        {
            get { return borderColor_Light; }
            set
            {
                borderColor_Light = value;
                Refresh();
            }
        }

        private Color borderColor_Dark = DEFAULT_COLOR_BORDER;
        [Category("HMI Properties")]
        public Color BorderColor
        {
            get { return borderColor_Dark; }
            set
            {
                borderColor_Dark = value;
                Refresh();
            }
        }
        #endregion /Border

        #region Highlight
        /// <summary>
        /// Is the text currently set to diplay as highlighted.
        /// </summary>
        Boolean highlighted = false; //TODO: override and color setting (SA?!)

        private Color fontHighlightColor = DEFAULT_TEXT_HIGHLIGHT_COLOR;
        [Category("HMI Properties")]
        public Color FontHighlightColor
        {
            get { return fontHighlightColor; }
            set
            {
                fontHighlightColor = value;
            }
        }
        #endregion /Highlight

        #endregion /Color

        #region Text
        /// <summary>
        /// Size of the current text neets to display.
        /// </summary>
        private Size textSize;
        /// <summary>
        /// This is to store the font setting to impliment highlight.
        /// </summary>
        private Font regular_font = new(new FontFamily("Arial"), 10);
        /// <summary>
        /// This is to store the font setting to impliment highlight.
        /// </summary>
        private Font bold_font;

        private Boolean bolded = false;
        public override Font Font
        {
            get
            {
                return regular_font;
            }
            set
            {
                if (!(regular_font == value || bold_font == value))
                {
                    regular_font = new Font(value, FontStyle.Regular);
                    bold_font = new Font(value, FontStyle.Bold);
                    base.Font = bolded ? bold_font : regular_font;
                }
            }
        }

        [Category("HMI Properties")]
        public StringFormatFlags TextFormatFlags { get; set; }

        private String text = String.Empty;
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Bindable(true)]
        public override String Text
        {
            get 
            { 
                return text; 
            }
            set
            {
                if (text != value)
                {
                    text = value;
                    textSize = TextRenderer.MeasureText(Text, Font);
                    if (AutoSize)
                    {
                        SetSizeFromText();
                    }
                }
            }
        }
        #endregion /Text

        #region Size
        private readonly Double[] posX = new double[ARRAY_SIZE];
        private readonly Single[] width = new float[ARRAY_SIZE];

        private readonly Double[] posY = new double[ARRAY_SIZE];
        private readonly Single[] height = new float[ARRAY_SIZE];
        #endregion /Size

        #endregion /Properties

        #region Graphics
        Graphics g;
        #endregion /Graphics

        #region X variables
        private readonly Double[] XmN = new double[ARRAY_SIZE];
        private readonly Single[] XmNf = new float[ARRAY_SIZE];
        private readonly Int32[] XmNi = new int[ARRAY_SIZE];

        private Double XpN2;
        private Single XpN2f;
        private Int32 XpN2i;

        private Double XpN3;
        private Single XpN3f;
        private Int32 XpN3i;

        private Single XmN4f_div2;
        #endregion / X variables

        #region Y variables
        private readonly Double[] YmN = new double[ARRAY_SIZE];
        private readonly Double[] YpN = new double[ARRAY_SIZE];
        private readonly Single[] YmNf = new float[ARRAY_SIZE];
        private readonly Int32[] YmNi = new int[ARRAY_SIZE];

        private Double YpN2;
        private Single YpN2f;
        private Int32 YpN2i;

        private Double YpN3;
        private Single YpN3f;
        private Int32 YpN3i;

        private Single YmN4f_div2;
        #endregion /Y Variables

        #region Points
        PointF centerPointF;
        Point point2m;
        Point point2p;
        Point point3m;
        Point point3_2p;
        #endregion /Points

        #region Delegates
        Action<GraphicsPath, Single, Single, Single, Single> addShape;
        Action<Brush, Single, Single, Single, Single> fillShape;
        #endregion /Delegates

        #endregion /Globals

        #region Constructor
        public Indicator()
        {
            InitializeComponent();
            addShape = new Action<GraphicsPath, Single, Single, Single, Single>((path, x, y, w, h) =>
            {
                path.AddRectangle(new RectangleF(x, y, w, h));
            });
            g = CreateGraphics();
            SetShape();
            textSize = TextRenderer.MeasureText(Text, Font);
            colorArray = new Color[1] { InactiveColor };
            textColorSetting = highlighted ? fontHighlightColor : InactiveTextColor;
        }
        #endregion /Constructor

        #region Methods

        #region Size
        Double lastWidth;
        Double lastHeight;

        private void SizeCalculations()
        {
            if (!(lastWidth == Width && lastHeight == Height))
            {
                lastWidth = Width;
                lastHeight = Height;
               
                #region Size Ratios
                double outerBorderRatio = OUTER_BORDER_RATIO;
                double innerBorderRatio = INNER_BORDER_RATIO;
                double indicatorBoxRatio = INDICATOR_BOX_RATIO;

                double outerBorderOffset;
                double innerBorderOffset;
                double indicatorBoxOffset;

                if (emissive)
                {
                    outerBorderRatio -= OUTER_BORDER_EMISSIVE_OFFSET;
                    innerBorderRatio -= OUTER_BORDER_EMISSIVE_OFFSET;
                    indicatorBoxRatio -= OUTER_BORDER_EMISSIVE_OFFSET;
                }

                if (Width < Height)
                {
                    outerBorderOffset = Width - (Width * outerBorderRatio);
                    innerBorderOffset = Width - (Width * innerBorderRatio);
                    indicatorBoxOffset = Width - (Width * indicatorBoxRatio);
                }
                else
                {
                    outerBorderOffset = Height - (Height * outerBorderRatio);
                    innerBorderOffset = Height - (Height * innerBorderRatio);
                    indicatorBoxOffset = Height - (Height * indicatorBoxRatio);
                }
                #endregion /Size Ratios

                #region Size
                PointF pointF = new PointF(Width / 2f, Height / 2f);

                posX[0] = pointF.X;
                width[0] = Convert.ToSingle(Width);
                posX[1] = posX[0] - outerBorderOffset;
                width[1] = Convert.ToSingle(2f * posX[1]);
                posX[2] = posX[0] - innerBorderOffset;
                width[2] = Convert.ToSingle(2f * posX[2]);
                posX[3] = posX[0] - indicatorBoxOffset;
                width[3] = Convert.ToSingle(2f * posX[3]);

                posY[0] = pointF.Y;
                height[0] = Convert.ToSingle(2f * posY[0]);
                posY[1] = posY[0] - outerBorderOffset;
                height[1] = Convert.ToSingle(2f * posY[1]);
                posY[2] = posY[0] - innerBorderOffset;
                height[2] = Convert.ToSingle(2f * posY[2]);
                posY[3] = posY[0] - indicatorBoxOffset;
                height[3] = Convert.ToSingle(2f * posY[3]);
                #endregion /Size

                #region X variables
                XmN[0] = pointF.X - posX[0];
                XmNf[0] = Convert.ToSingle(XmN[0]);
                XmNi[0] = Convert.ToInt32(XmN[0]);

                XmN[1] = pointF.X - posX[1];
                XmNf[1] = Convert.ToSingle(XmN[1]);
                XmNi[1] = Convert.ToInt32(XmN[1]);

                XmN[2] = pointF.X - posX[2];
                XmNf[2] = Convert.ToInt32(XmN[2]);
                XmNi[2] = Convert.ToInt32(XmN[2]);

                XmN[3] = pointF.X - posX[3];
                XmNf[3] = Convert.ToSingle(XmN[3]);
                XmN4f_div2 = XmNf[3] / 2f;
                XmNi[3] = Convert.ToInt32(XmN[3]);

                XpN2 = pointF.X + posX[2];
                XpN2f = Convert.ToSingle(XpN2);
                XpN2i = Convert.ToInt32(XpN2);

                XpN3 = pointF.X + posX[2];
                XpN3f = Convert.ToSingle(XpN3);
                XpN3i = Convert.ToInt32(XpN3);
                #endregion /X variables

                #region Y variables
                YmN[0] = pointF.Y - posY[0];
                YmNf[0] = Convert.ToSingle(YmN[0]);
                YmNi[0] = Convert.ToInt32(YmN[0]);

                YmN[1] = pointF.Y - posY[1];
                YmNf[1] = Convert.ToSingle(YmN[1]);
                YmNi[1] = Convert.ToInt32(YmN[1]);

                YmN[2] = pointF.Y - posY[2];
                YmNf[2] = Convert.ToInt32(YmN[2]);
                YmNi[2] = Convert.ToInt32(YmN[2]);

                YmN[3] = pointF.Y - posY[3];
                YmNf[3] = Convert.ToSingle(YmN[3]);
                YmN4f_div2 = YmNf[3] / 2f;
                YmNi[3] = Convert.ToInt32(YmN[3]);

                YpN2 = pointF.Y + posY[2];
                YpN2f = Convert.ToSingle(YpN2);
                YpN2i = Convert.ToInt32(YpN2);

                YpN3 = pointF.Y + posY[2];
                YpN3f = Convert.ToSingle(YpN3);
                YpN3i = Convert.ToInt32(YpN3);
                #endregion /Y variables

                #region Points 
                centerPointF = new PointF(XmN4f_div2, YmN4f_div2);
                point2m = new Point(XmNi[1], YmNi[1]);
                point2p = new Point(XpN2i, XpN2i);
                point3m = new Point(XmNi[2], YmNi[2]);
                point3_2p = new Point(XpN3i, YpN2i);
                #endregion /Points
            }
        }

        private void SetSizeFromText()
        {
            int width = textSize.Width + AUTOSIZE_PADDING.Width;
            int height = textSize.Height + AUTOSIZE_PADDING.Height;
            if (Width != width)
            {
                Width = width;
            }
            if (Height != height)
            {
                Height = height;
            }
        }
        #endregion /Size

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            #region Event Graphics
            g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            #endregion /Event Graphics

            #region Draw
            FillShape();
            FillText();
            #endregion /Draw
        }

        private void FillShape()
        {
            #region Border
            if (StyledBorder)
            {
                #region Outer
                using (LinearGradientBrush outerBorder = new LinearGradientBrush(point2m, point2p, BorderAccent, BorderColor))
                {
                    fillShape(outerBorder, XmNf[1], YmNf[1], width[1], height[1]);
                }
                #endregion /Outer

                #region Inner
                using (LinearGradientBrush innerBorder = new LinearGradientBrush(point3m, point3_2p, BorderColor, BorderAccent))
                {
                    fillShape(innerBorder, XmNf[2], YmNf[2], width[2], height[2]);
                }
                #endregion /Inner
            }
            else
            {
                using (SolidBrush border = new SolidBrush(BorderColor))
                {
                    fillShape(border, XmNf[1], YmNf[1], width[1] + width[2], height[1] + height[2]);
                }
            }
            #endregion /Border

            #region Emmisive 
            if (Emissive && On)// Value = true is when LED is on
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    addShape(path, XmNf[0], YmNf[0], width[0], height[0]);
                    {
                        colorArray = new Color[1] { Color.FromArgb(1, OnColor) };
                        PathGradientBrush brushPath = new PathGradientBrush(path)
                        {
                            CenterColor = Color.FromArgb(150, OnColor),
                            SurroundColors = colorArray
                        };
                        fillShape(brushPath, XmNf[0], YmNf[0], width[0], height[0]);// Emissiveness (glow)
                        brushPath.Dispose();
                    }
                }
            }
            #endregion /Emmisive

            #region Indicator
            using (GraphicsPath path = new GraphicsPath())
            {
                addShape(path, XmNf[3], YmNf[3], width[3], height[3]);
                Brush brushPath;
                if (gradient)
                {
                    brushPath = new PathGradientBrush(path)
                    {
                        CenterColor = BorderAccent,
                        SurroundColors = colorArray,
                        CenterPoint = centerPointF
                    };
                }
                else
                {
                    brushPath = new SolidBrush(colorArray[0]);
                }
                fillShape(brushPath, XmNf[3], YmNf[3], width[3], height[3]);
                brushPath.Dispose();
            }
            #endregion /Indicator
        }

        private void FillText()
        {
            #region Position
            RectangleF textBox = new RectangleF(XmNf[1], YmNf[1], width[1], height[1]);
            #endregion

            #region Draw
            StringFormat drawFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
                FormatFlags = TextFormatFlags
            };
            using (SolidBrush brush = new SolidBrush(textColorSetting))
            {
                g.DrawString(Text, Font, brush, textBox, drawFormat); // gString
            }
            #endregion
        }

        #region Size Event Handlers
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            switch (shape)
            {
                case Indicator_Shape.Circle:
                    float num = Math.Min(Width, Height);
                    if (num < 20.0)
                    {
                        num = 20f;
                    }
                    Width = Convert.ToInt32(num);
                    Height = Convert.ToInt32(num);
                    GraphicsPath path = new GraphicsPath();
                    path.AddEllipse(0, 0, Width, Height);
                    Region = new Region(path);
                    return;
            }
            SizeCalculations();
        }
        #endregion /Size Event Handlers

        #region Mouse Event Handlers
        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
            lock (Font)
            {
                highlighted = true;
                textColor_Regular = textColorSetting;
                Font = bold_font;
                textColorSetting = FontHighlightColor;
            }
            Refresh();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            lock (Font)
            {
                if (highlighted)
                {
                    highlighted = false;
                    Font = regular_font;
                    textColorSetting = textColor_Regular;
                }
            }
            Refresh();
        }
        #endregion /Mouse Event Handlers

        #endregion /Methods
    }
}

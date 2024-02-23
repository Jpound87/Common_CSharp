using Common.Base;
using Common.Constant;
using Common.Extensions;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using ButtonState = Common.Constant.ButtonState;

namespace Common.Controls
{
    [DefaultEvent("MouseClick")]
    public partial class GlowButton : Control_Base
    {
        #region Identity
        new public const String ControlName = nameof(Control_Base);
        public override String Identity
        {
            get
            {
                return ControlName;
            }
        }
        #endregion

        #region Constants
        // glow color alpha
        private const Int32 GLOW_LEVEL = 40;
        // alpha maximum before pixel is changed
        private const Int32 GLOW_THRESHHOLD = 96;
        // glow padding
        private const Int32 SIZE_OFFSET = 2;
        private const Int32 BORDER_PADDING = 0;
        private const Int32 BORDER_PADDING_2X = BORDER_PADDING * 2;
        #endregion /Constants

        #region Static

        #region Readonly
        private static readonly Color imageMirrorColor = Color.WhiteSmoke;
        private static readonly Color fill_White_140 = Color.FromArgb(140, Color.White);
        private static readonly Color fill_White_160 = Color.FromArgb(160, Color.White);
        private static readonly Color fill_Silver = Color.FromArgb(5, Color.Silver);
        #endregion /Readonly

        #endregion Static

        #region Globals
        private Boolean checkState = false;
        private Boolean focusOnHover = false;
        private Boolean clicked = false;
        private Boolean focusedMask = false;
        private Boolean imageMirror = false;
        private Int32 glowFactor = 2;

        private CheckedStyle useCheckStyle = CheckedStyle.None;
        private ButtonState buttonState = ButtonState.Normal;

        private Image imgImage;
        private Image imgImage_Scaled;
        private Image imgBackgroundImage;
        private Image imgBackgroundImage_Scaled;
        private Bitmap picBuffer;
        private Bitmap picGlow;
        private Bitmap picMirror;
        #endregion /Globals

        #region Accessors

        #region Hidden 
        [Browsable(false)]
        public new Boolean AllowDrop
        {
            get
            {
                return base.AllowDrop;
            }
            set
            {
                base.AllowDrop = value;
            }
        }
        [Browsable(false)]
        public new AnchorStyles Anchor
        {
            get
            {
                return base.Anchor;
            }
            set
            {
                base.Anchor = value;
            }
        }
        [Browsable(false)]
        public new Boolean AutoScroll
        {
            get
            {
                return base.AutoScroll;
            }
            set
            {
                base.AutoScroll = value;
            }
        }
        [Browsable(false)]
        public new Size AutoScrollMargin
        {
            get
            {
                return base.AutoScrollMargin;
            }
            set
            {
                base.AutoScrollMargin = value;
            }
        }
        [Browsable(false)]
        public new Size AutoScrollMinSize
        {
            get
            {
                return base.AutoScrollMinSize;
            }
            set
            {
                base.AutoScrollMinSize = value;
            }
        }
        [Browsable(false)]
        public new AutoSizeMode AutoSizeMode
        {
            get
            {
                return base.AutoSizeMode;
            }
            set
            {
                base.AutoSizeMode = value;
            }
        }
        [Browsable(false)]
        public new AutoValidate AutoValidate
        {
            get
            {
                return base.AutoValidate;
            }
            set
            {
                base.AutoValidate = value;
            }
        }
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set background image layout")]
        public new ImageLayout BackgroundImageLayout
        {
            get
            {
                return base.BackgroundImageLayout;
            }
            set
            {
                base.BackgroundImageLayout = value;
            }
        }
        [Browsable(false)]
        public new ContextMenuStrip ContextMenuStrip
        {
            get
            {
                return base.ContextMenuStrip;
            }
            set
            {
                base.ContextMenuStrip = value;
            }
        }
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set the dock state")]
        public new DockStyle Dock
        {
            get
            {
                return base.Dock;
            }
            set
            {
                base.Dock = value;
            }
        }
        [Browsable(false)]
        public new Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
            }
        }
        [Browsable(false)]
        public new Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
            }
        }
        [Browsable(false)]
        public new RightToLeft RightToLeft
        {
            get
            {
                return base.RightToLeft;
            }
            set
            {
                base.RightToLeft = value;
            }
        }
        [Browsable(false)]
        public new Padding Padding
        {
            get
            {
                return base.Padding;
            }
            set
            {
                base.Padding = value;
            }
        }
        [Browsable(false)]
        public Boolean IsImage { get; private set; }
        [Browsable(false)]
        public Boolean IsBackgroundImage { get; private set; }

        #endregion /Hidden

        #region Public

        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button Background image")]
        public override Image BackgroundImage
        {
            get
            {
                return AutoSizeImage ? imgBackgroundImage_Scaled : imgBackgroundImage;
            }
            set
            {
                if (imgBackgroundImage != value)
                {
                    imgBackgroundImage = value;
                    IsBackgroundImage = imgBackgroundImage != null;
                    imgBackgroundImage_Scaled = IsBackgroundImage ? value.Scale(bounds.Size) : value;
                    DrawButton();
                }
            }
        }

        /// <summary>Get/Set Checkbox state</summary>
        [Browsable(true), Category("Behavior"),
        Description("Get/Set Checkbox state")]
        public Boolean Checked
        {
            get
            {
                return checkState;
            }
            set
            {
                if (checkState != value)
                {
                    checkState = value;
                    if (!checkState)
                    {
                        buttonState = ButtonState.Normal;
                    }
                    DrawButton();
                }
            }
        }

        private LineFill checkedBorderStyle = LineFill.Solid;
        /// <summary>Get/Set the checked border Color</summary>
        [Browsable(true), Category("Appearance"),
        Description("Get/Set the checked border style")]
        public LineFill CheckedBorderStyle
        {
            get
            {
                return checkedBorderStyle;
            }
            set
            {
                checkedBorderStyle = value;
            }
        }

        private Color checkedBorderColor = Color.WhiteSmoke;
        /// <summary>Get/Set the checked border Color</summary>
        [Browsable(true), Category("Appearance"),
        Description("Get/Set the checked border Color")]
        public Color CheckedBorderColor
        {
            get
            {
                return checkedBorderColor;
            }
            set
            {
                checkedBorderColor = value;
            }
        }

        /// <summary>Get/Set Checkbox style</summary>
        [Browsable(true), Category("Appearance"),
        Description("Get/Set Checkbox style")]
        public CheckedStyle CheckStyle
        {
            get
            {
                return useCheckStyle;
            }
            set
            {
                useCheckStyle = value;
            }
        }

        /// <summary>Get/Set Focus on button hover</summary>
        [Browsable(true), Category("Behavior"),
        Description("Get/Set Focus on button hover")]
        public Boolean FocusOnHover
        {
            get
            {
                return focusOnHover;
            }
            set
            {
                focusOnHover = value;
            }
        }

        /// <summary>Get/Set Draw a Focused mask</summary>
        [Browsable(true), Category("Appearance"),
        Description("Get/Set Draw a Focused mask")]
        public Boolean FocusedMask
        {
            get
            {
                return focusedMask;
            }
            set
            {
                focusedMask = value;
            }
        }

        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button Image Autosize")]
        public Boolean AutoSizeImage { get; set; } = true;
        /// <summary>Get/Set button image</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button Image")]
        public Image Image
        {
            get
            {
                return AutoSizeImage ? imgImage_Scaled : imgImage;
            }
            set
            {
                if (imgImage != value)
                {
                    imgImage = value;
              
                    IsImage = imgImage != null;
                    if (IsImage)
                    {
                        imgImage_Scaled = value.Scale(bounds.Size);
                        if (DesignMode && AutoSize)
                        {
                            ResetSize();
                        }
                        if (ImageMirror)
                        {
                            CreateMirror();
                        }
                        CreateGlow();
                    }
                    else
                    {
                        imgImage_Scaled = value;
                    }
                    DrawButton();
                }

            }
        }

        private Color imageCheckedColor = Color.SteelBlue;
        /// <summary>Get/Set the image checked Color</summary>
        [Browsable(true), Category("Appearance"),
        Description("Get/Set the image checked Color")]
        public Color ImageCheckedColor
        {
            get
            {
                return imageCheckedColor;
            }
            set
            {
                imageCheckedColor = value;
            }
        }

        private Color imageDisabledColor = Color.Transparent;
        /// <summary>Get/Set the image disabled Color</summary>
        [Browsable(true), Category("Appearance"),
        Description("Get/Set the image disabled Color")]
        public Color ImageDisabledColor
        {
            get
            {
                return imageDisabledColor;
            }
            set
            {
                imageDisabledColor = value;
            }
        }

        private Color imageFocusedColor = Color.SkyBlue;
        /// <summary>Get/Set the image focused Color</summary>
        [Browsable(true), Category("Appearance"),
        Description("Get/Set the image focused Color")]
        public Color ImageFocusedColor
        {
            get
            {
                return imageFocusedColor;
            }
            set
            {
                imageFocusedColor = value;
            }
        }

        private Color imageGlowColor = Color.WhiteSmoke;
        /// <summary>Get/Set the glow Color</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set the glow Color")]
        public Color ImageGlowColor
        {
            get
            {
                return imageGlowColor;
            }
            set
            {
                imageGlowColor = value;
                CreateGlow();
            }
        }

        private Color imageHoverColor = Color.LightSkyBlue;
        /// <summary>Get/Set the hover Color</summary>
        [Browsable(true), Category("Appearance"),
        Description("Get/Set the hover Color")]
        public Color ImageHoverColor
        {
            get
            {
                return imageHoverColor;
            }
            set
            {
                imageHoverColor = value;
            }
        }

        private Color imagePressedColor = Color.SteelBlue;
        /// <summary>Get/Set the pressed Color</summary>
        [Browsable(true), Category("Appearance"),
        Description("Get/Set the pressed Color")]
        public Color ImagePressedColor
        {
            get
            {
                return imagePressedColor;
            }
            set
            {
                imagePressedColor = value;
            }
        }

        /// <summary>Get/Set Glow factor Depth</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set Glow factor Depth")]
        public Int32 ImageGlowFactor
        {
            get
            {
                return glowFactor;
            }
            set
            {
                glowFactor = value;
                CreateGlow();
            }
        }

        /// <summary>Get/Set Image Mirror effect</summary>
        [Browsable(true), Category("Behavior"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set Image Mirror effect")]
        public Boolean ImageMirror
        {
            get
            {
                return imageMirror;
            }
            set
            {
                if (imageMirror != value)
                {
                    imageMirror = value;
                    if (imageMirror)
                    {
                        CreateMirror();
                    }
                    if (DesignMode && Image != null && AutoSize)
                    {
                        ResetSize();
                    }
                    DrawButton();
                }
            }
        }
        #endregion /Public

        #region Overrides
        protected override void OnEnabledChanged(EventArgs e)
        {
            if (Enabled == false && buttonState != ButtonState.Disabled)
            {
                buttonState = ButtonState.Disabled;
                DrawButton();
            }
            else if (buttonState != ButtonState.Normal)
            {
                buttonState = ButtonState.Normal;
                DrawButton();
            }
            base.OnEnabledChanged(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            if (Focused)
            {
                buttonState = ButtonState.Focused;
            }
            if (clicked)
            {
                clicked = false;
            }
            else
            {
                DrawButton();
            }
            base.OnGotFocus(e);
        }

        protected override void OnHandleCreated()
        {
            Create();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
            Dispose();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            clicked = false;
            if (buttonState != ButtonState.Normal)
            {
                buttonState = ButtonState.Normal;
                DrawButton();
            }
            base.OnLostFocus(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && SystemInformation.MouseButtonsSwapped || e.Button == MouseButtons.Left)
            {
                Checked = !Checked;
                DrawButton();
            }
            base.OnMouseClick(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && SystemInformation.MouseButtonsSwapped || e.Button == MouseButtons.Left)
            {
                buttonState = ButtonState.Pressed;
                DrawButton();
                clicked = true;
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (FocusOnHover)
            {
                buttonState = ButtonState.Focused;
                Focus();
            }
            else
            {
                buttonState = ButtonState.Hover;
                DrawButton();
            }
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (Focused)
            {
                buttonState = ButtonState.Focused;
            }
            else
            {
                buttonState = ButtonState.Normal;
                DrawButton();
            }
            base.OnMouseLeave(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (buttonState != ButtonState.Focused)
            {
                buttonState = ButtonState.Focused;
                DrawButton();
            }
            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            DrawButton();
            base.OnPaint(e);
        }

        protected override void OnResize(EventArgs e)
        {
            SetBounds();
            base.OnResize(e);
        }

        private void SetBounds()
        {
            bounds = new Rectangle(BORDER_PADDING, BORDER_PADDING, Width - BORDER_PADDING_2X, Height - BORDER_PADDING_2X);
            if (IsImage)
            {
                imgImage_Scaled = imgImage.Scale(bounds.Size);
                imageBounds = GetImageBounds(bounds, Image);
            }
            if (IsBackgroundImage)
            {
                imgBackgroundImage_Scaled = imgBackgroundImage.Scale(bounds.Size);
            }
            Create();
        }
        #endregion /Overrides

        #endregion /Accessors

        #region Globals
        private Rectangle bounds;
        private Rectangle imageBounds;
        #endregion /Globals

        #region Constructor
        public GlowButton()
        {
            InitializeComponent();
            SetBounds();
            Create();
        }

        private void Create()
        {
            picBuffer = new Bitmap(Width, Height);
        }
        #endregion /Constructor

        #region Methods
        /// <summary>Create the Glow image</summary>
        private void CreateGlow()
        {
            picGlow?.Dispose();
            if (IsImage)
            {
                Rectangle imageRect = new Rectangle(0, 0, Image.Width + ImageGlowFactor, Image.Height + ImageGlowFactor);
                picGlow = new Bitmap(imageRect.Width, imageRect.Height);
                using (Graphics g = Graphics.FromImage(picGlow))
                {
                    g.DrawImage(Image, imageRect);
                }
                for (int x = 0; x < imageRect.Height; x++)
                {
                    for (int i = 0; i < imageRect.Width; i++)
                    {
                        if (picGlow.GetPixel(i, x).A > GLOW_THRESHHOLD)
                        {
                            picGlow.SetPixel(i, x, Color.FromArgb(GLOW_LEVEL, ImageGlowColor));
                        }
                    }
                }
            }
        }

        /// <summary>Create the Mirror image</summary>
        private void CreateMirror()
        {
            picMirror?.Dispose();
            if (IsImage)
            {
                int height = Convert.ToInt32(Image.Height * .7f);
                int width = Convert.ToInt32(Image.Width * 1f);
                Rectangle imageRect = new Rectangle(0, 0, width, height);
                picMirror = new Bitmap(imageRect.Width, imageRect.Height);

                using (Graphics g = Graphics.FromImage(picMirror))
                {
                    g.DrawImage(Image, imageRect);
                }
                picMirror.RotateFlip(RotateFlipType.Rotate180FlipX);
            }
        }

        /// <summary>Backfill the buffer</summary>
        private void DrawBackground(Graphics g, Rectangle bounds)
        {
            if (BackgroundImage != null)
            {
                g.DrawImage(BackgroundImage, GetImageBounds(bounds, BackgroundImage));
            }
            else
            {
                using (Brush br = new SolidBrush(BackColor))
                {
                    g.FillRectangle(br, bounds);
                }
            }
        }

        /// <summary>Draw border on checked style</summary>
        private void DrawBorder(Graphics g, Rectangle bounds, Color clr)
        {
            bounds.Inflate(-2, -2);
            using (GraphicsPath borderPath = g.GenerateRoundedRectanglePath(bounds, 4))
            {
                Color borderColor = Color.FromArgb(140, clr);
                switch (CheckedBorderStyle)
                {
                    case LineFill.Gradient:
                        // Top-left bottom-right -dark.
                        using (LinearGradientBrush borderBrush = new LinearGradientBrush(bounds, borderColor, fill_White_140, LinearGradientMode.BackwardDiagonal))
                        {
                            Blend blnd = new Blend
                            {
                                Positions = new float[] { 0f, .5f, 1f },
                                Factors = new float[] { 1f, 0f, 1f }
                            };
                            borderBrush.Blend = blnd;
                            using (Pen borderPen = new Pen(borderBrush, 2f))
                            {
                                g.DrawPath(borderPen, borderPath);
                            }
                        }
                        break;
                    case LineFill.Solid:
                        using (SolidBrush borderBrush = new SolidBrush(borderColor))
                        {
                            using (Pen borderPen = new Pen(borderBrush, 2f))
                            {
                                g.DrawPath(borderPen, borderPath);
                            }
                        }
                        break;
                }
            }
        }

        /// <summary>Drawing hub</summary>
        private void DrawButton()
        {
            // draw into a buffer
            using (Graphics g = Graphics.FromImage(picBuffer))
            {
                g.Clear(this.GetAscendantColor());
                g.SmoothingMode = SmoothingMode.HighQuality;
                DrawBackground(g, bounds);
                if (IsImage)
                {
                    if (ImageMirror)
                    {
                        DrawMirror(g, imageBounds);
                    }
                    if (CheckStyle != CheckedStyle.None && Checked == true)
                    {
                        buttonState = ButtonState.Checked;
                    }
                    else if (CheckStyle != CheckedStyle.None && Checked == false && buttonState == ButtonState.Focused)
                    {
                        buttonState = ButtonState.Normal;
                    }
                    switch (buttonState)
                    {
                        case ButtonState.Checked:
                            switch (CheckStyle)
                            {
                                case CheckedStyle.Border:
                                    DrawBorder(g, bounds, CheckedBorderColor);
                                    goto default;
                                case CheckedStyle.ColorChange:
                                    g.DrawColoredImage(Image, imageBounds, ImageCheckedColor);
                                    break;
                                default:
                                    g.DrawColoredImage(Image, imageBounds, ImageFocusedColor);
                                    break;
                            }
                            break;
                        case ButtonState.Disabled:
                            if (ImageDisabledColor == Color.Transparent)
                            {
                                g.DrawFadedImage(Image, imageBounds);
                            }
                            else
                            {
                                g.DrawColoredImage(Image, imageBounds, ImageDisabledColor);
                            }
                            break;
                        case ButtonState.Focused:
                            g.DrawColoredImage(Image, imageBounds, ImageFocusedColor);
                            if (FocusedMask)
                            {
                                g.DrawMask(bounds, fill_White_160, fill_Silver);
                                DrawBorder(g, bounds, Color.DarkGray);
                            }
                            break;
                        case ButtonState.Hover:
                            DrawGlow(g, imageBounds);
                            g.DrawColoredImage(Image, imageBounds, ImageHoverColor);
                            break;
                        case ButtonState.Normal:
                            g.DrawImage(Image, imageBounds);
                            break;
                        case ButtonState.Pressed:
                            g.DrawColoredImage(Image, imageBounds, ImagePressedColor);
                            break;
                    }
                }
            }
            // Draw the buffer.
            using (Graphics g = Graphics.FromHwnd(Handle))
            {
                g.Clear(this.GetAscendantColor());
                g.DrawImage(picBuffer, bounds);
            }
        }

        /// <summary>Draw hover glow</summary>
        private void DrawGlow(Graphics g, Rectangle bounds)
        {
            bounds.Inflate(ImageGlowFactor, ImageGlowFactor);
            g.DrawImage(picGlow, bounds);
        }


        /// <summary>Draw a mirror effect</summary>
        private void DrawMirror(Graphics g, Rectangle bounds)
        {
            // Rectangle imageRect = GetImageBounds(bounds, this.Image);
            bounds.Y = bounds.Bottom;
            bounds.Height = picMirror.Height;
            bounds.Width = picMirror.Width;
            using (ImageAttributes ia = new ImageAttributes())
            {
                ia.SetColorMatrix(Extensions_Graphics.MirrorMatrix);
                g.DrawImage(picMirror, bounds, 0, 0, picMirror.Width, picMirror.Height, GraphicsUnit.Pixel, ia);
            }
        }

        /// <summary>Get the image size and position</summary>
        private Rectangle GetImageBounds(Rectangle bounds, Image img)
        {
            if (AutoSizeImage)
            {
                return bounds;
            }
            int top;
            int left;
            int width = img.Width;
            int height = img.Height;
            if(bounds.Height > img.Height) 
            {
                if (ImageMirror)
                {
                    top = Convert.ToInt32(((bounds.Height - (img.Height + Convert.ToInt32(img.Height * .7f))) * .5f));
                }
                else
                {
                    top = Convert.ToInt32((bounds.Height - img.Height) * .5f);
                }
            }
            else
            {
                height = width = Math.Min(bounds.Width, bounds.Height);
                top = 0;
            }
            if (bounds.Width > img.Width)
            {
                left = Convert.ToInt32((bounds.Width - img.Width) * .5f);
            }
            else
            {
                height = width = Math.Min(bounds.Width, bounds.Height);
                left = 0;
            }
            top += bounds.Top;
            left += bounds.Left;
            return new Rectangle(left, top, width, height);
        }

        private void ResetSize()
        {
            Width = Image.Width + ImageGlowFactor + SIZE_OFFSET;
            Height = Image.Height + ImageGlowFactor + SIZE_OFFSET;
            if (ImageMirror)
            {
                Height += Convert.ToInt32(Image.Height * .7f);
            }
        }
        #endregion /Methods

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                picBuffer?.Dispose();
                picGlow?.Dispose();
                picMirror?.Dispose();
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
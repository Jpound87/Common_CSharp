using Common.Constant;
using Common.Extensions;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Common.Controls
{
    public partial class SplitButton : UserControl
    {
        #region Identity
        private const String ControlName = nameof(SplitButton);
        public String Identity
        {
            get
            {
                return ControlName;
            }
        }
        #endregion

        #region Constants
        private static readonly Padding mouseExclusionBoundry = new Padding(7, 7, 7, 7);
        #endregion

        #region Events

        #region Mouse 

        #region Click

        new public event EventHandler Click
        {
            add
            {
                lock (tlpSplitButton)
                {
                    btnLeft.Click += value;
                    btnRight.Click += value;
                }
            }
            remove
            {
                lock (tlpSplitButton)
                {
                    btnLeft.Click -= value;
                    btnRight.Click -= value;
                }
            }
        }
        #endregion /Click

        #region Enter
        private bool mouseEnterFlag;
        private readonly EventHandler mouseEnter_Handler;
        private void MouseEnterTriggered(object _, EventArgs eventArgs)
        {
            lock (mouseEnter_Handler)
            {
                if (tlpSplitButton.IsMouseOverControl(out Point cursorPosition))
                {
                    mouseLeaveFlag = false;
                    if (!mouseEnterFlag)
                    {
                        mouseEnterFlag = true;
                        MouseEnter?.Invoke(this, eventArgs);
                        btnLeft.BackColor = MouseOverBackColor;
                        btnRight.BackColor = MouseOverBackColor;
                        tlpSplitButton.BackColor = MouseOverBackColor;
                        Invalidate();
                        MouseEventArgs mea = new MouseEventArgs(MouseButtons.None, 0, cursorPosition.X, cursorPosition.Y, 0);
                        MouseOver?.Invoke(this, mea);
                    }
                }
            }
        }

        new public event EventHandler MouseEnter;
        #endregion /Enter

        #region Leave
        private bool mouseLeaveFlag;
        private readonly EventHandler mouseLeave_Handler;
        private void MouseLeaveTriggered(object _, EventArgs eventArgs)
        {
            lock (mouseLeave_Handler)
            {
                //if (!tlpSplitButton.IsMouseOverControl())
                {
                    mouseEnterFlag = false;
                    if (!mouseLeaveFlag)
                    {
                        mouseLeaveFlag = true;
                        if (!IsMouseOver)
                        {
                            btnLeft.BackColor = BackColor;
                            btnRight.BackColor = BackColor;
                            tlpSplitButton.BackColor = BackColor;
                            MouseLeave?.Invoke(this, eventArgs);
                        }
                    }
                }
            }
        }
        new public event EventHandler MouseLeave;
        #endregion /Leave

        #region Down
        private readonly MouseEventHandler mouseDown_Right_Handler;
        private readonly MouseEventHandler mouseDown_Left_Handler;
        new public event MouseEventHandler MouseDown
        {
            add
            {
                lock (tlpSplitButton)
                {
                    btnLeft.MouseDown += value;
                    btnRight.MouseDown += value;
                }
            }
            remove
            {
                lock (tlpSplitButton)
                {
                    btnLeft.MouseDown -= value;
                    btnRight.MouseDown -= value;
                }
            }
        }
        #endregion /Down

        #region Up
        private readonly MouseEventHandler mouseUp_Right_Handler;
        private readonly MouseEventHandler mouseUp_Left_Handler;
        new public event MouseEventHandler MouseUp
        {
            add
            {
                lock (tlpSplitButton)
                {
                    btnLeft.MouseUp += value;
                    btnRight.MouseUp += value;
                }
            }
            remove
            {
                lock (tlpSplitButton)
                {
                    btnLeft.MouseUp -= value;
                    btnRight.MouseUp -= value;
                }
            }
        }
        #endregion /Up

        #region Over
        private readonly MouseEventHandler mouseOver_Right_Handler;// TODO: use enter and leave events for back color uniformity
        private readonly MouseEventHandler mouseOver_Left_Handler;
        public event MouseEventHandler MouseOver;
        public bool IsMouseOver
        {
            get
            {
                return btnLeft.IsMouseOverControl() | btnRight.IsMouseOverControl() || tlpSplitButton.IsMouseOverControl();// Should be largest to smallest.
            }
        }
        #endregion /Over

        #endregion /Mouse

        #endregion /Events

        #region Color
        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button background color"), DefaultValue(typeof(SystemColors), "Control")]
        new public Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                btnLeft.BackColor = value;
                btnRight.BackColor = value;
                tlpSplitButton.BackColor = value;
            }
        }

        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button forground color"), DefaultValue(typeof(SystemColors), "ControlText")]
        new public Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                btnLeft.ForeColor = value;
                btnRight.ForeColor = value;
            }
        }

        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("FlatAppearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button mouse over color"), DefaultValue(typeof(SystemColors), "ControlLight")]
        private Color mouseOverBackColor;
        public Color MouseOverBackColor
        {
            get
            {
                return mouseOverBackColor;
            }
            set
            {
                mouseOverBackColor = value;
                btnLeft.FlatAppearance.MouseOverBackColor = value;
                btnRight.FlatAppearance.MouseOverBackColor = value;
            }
        }

        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("FlatAppearance"), RefreshProperties(RefreshProperties.All),
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
                btnLeft.FlatAppearance.MouseDownBackColor = value;
                btnRight.FlatAppearance.MouseDownBackColor = value;
            }
        }
        #endregion /Color

        #region Border
        [Browsable(false), DefaultValue(BorderStyle.None)]
        new public BorderStyle BorderStyle { get; set; }
        #endregion /Border

        #region Text
        /// <summary>Get/Set button text!</summary>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always),
        Category("Appearance"), RefreshProperties(RefreshProperties.All),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Description("Get/Set button text"), DefaultValue("SplitButton"), Bindable(true)]
        new public String Text
        {
            get
            {
                return base.Text; 
            }
            set
            {
                base.Text = value;
                btnLeft.Text = value;
            }
        }

        /// <summary>Get/Set button text</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button text alignment"), DefaultValue(ContentAlignment.MiddleCenter)]
        public ContentAlignment TextAlign
        {
            get
            {
                return btnLeft.TextAlign;
            }
            set
            {
                btnLeft.TextAlign = value;
            }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always),
        Category("Appearance"), RefreshProperties(RefreshProperties.All),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Description("Get/Set button text font"), DefaultValue("Microsoft Sans Serif, 7.8pt"), Bindable(true)]
        new public Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                btnLeft.Font = value;
            }
        }
        #endregion /Text

        #region Image
        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button Background image")]
        public Image Image
        {
            get
            {
                return btnRight.BackgroundImage;
            }
            set
            {
                //base.BackgroundImage = value;
                btnRight.BackgroundImage = value;
            }
        }

        /// <summary>Get/Set button Background image</summary>
        [Browsable(false)]
        public ContentAlignment ImageAlign
        {
            get
            {
                return ContentAlignment.MiddleLeft;
            }
        }

        /// <summary>Get/Set button Background image</summary>
        [Browsable(false)]
        public override ImageLayout BackgroundImageLayout
        {
            get
            {
                return ImageLayout.Zoom;
            }
        }
        #endregion /Image

        #region Size
        public enum SplitButtonImageSize : byte
        { 
            Small,
            Medium, 
            Large
        }
        internal static readonly Size smallImage_SplitButton = new Size(32,32);
        internal static readonly Size mediumImage_SplitButton = new Size(34, 34);
        internal static readonly Size largeImage_SplitButton = new Size(36, 36);

        private SplitButtonImageSize imageSize = SplitButtonImageSize.Medium;
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button image size"), DefaultValue(SplitButtonImageSize.Medium)]
        public SplitButtonImageSize ImageSize
        {
            get
            {
                return imageSize;
            }
            set
            {
                imageSize = value;
                switch (value)
                {
                    case SplitButtonImageSize.Small:
                        btnRight.MinimumSize = smallImage_SplitButton;
                        btnRight.MaximumSize = smallImage_SplitButton;
                        break;
                    default:
                    case SplitButtonImageSize.Medium:
                        btnRight.MinimumSize = mediumImage_SplitButton;
                        btnRight.MaximumSize = mediumImage_SplitButton;
                        break;
                    case SplitButtonImageSize.Large:
                        btnRight.MinimumSize = largeImage_SplitButton;
                        btnRight.MaximumSize = largeImage_SplitButton;
                        break;
                }
                LeftRightPaddingWidth = 38 - btnRight.Size.Width;// We want 2 on your side, your... other.. side.
                int topBottomPaddingHeight = (Height - btnRight.Size.Height)/2;
                tlpSplitButton.RowStyles[0].Height = topBottomPaddingHeight;
                tlpSplitButton.RowStyles[2].Height = topBottomPaddingHeight;
            }
        }

        private int leftRightPaddingWidth = 6;
        [Browsable(false)]
        private int LeftRightPaddingWidth 
        { 
            get
            {
                return leftRightPaddingWidth;
            }
            set
            {
                leftRightPaddingWidth = value;
                switch (imagePaddingAlign)
                {
                    case LeftRightAlignment.Left:
                        tlpSplitButton.ColumnStyles[1].Width = value;
                        tlpSplitButton.ColumnStyles[3].Width = 2;
                        break;
                    default:
                    case LeftRightAlignment.Right:
                        tlpSplitButton.ColumnStyles[1].Width = 2;
                        tlpSplitButton.ColumnStyles[3].Width = value;
                        break;
                }
            }
        }

        private LeftRightAlignment imagePaddingAlign;
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button image size"), DefaultValue(LeftRightAlignment.Right)]
        public LeftRightAlignment ImagePaddingAlignment
        {
            get
            {
                return imagePaddingAlign;
            }
            set
            {
                imagePaddingAlign = value;
                switch (value)
                {
                    case LeftRightAlignment.Left:
                        tlpSplitButton.ColumnStyles[1].Width = leftRightPaddingWidth;
                        tlpSplitButton.ColumnStyles[3].Width = 2;
                        break;
                    default:
                    case LeftRightAlignment.Right:
                        tlpSplitButton.ColumnStyles[1].Width = 2;
                        tlpSplitButton.ColumnStyles[3].Width = leftRightPaddingWidth;
                        break;
                }
            }
        }

        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button minimum height"), DefaultValue(38)]
        public int MinimumHeight
        {
            get
            {
                return MinimumSize.Height;
            }
            set
            {
                base.MinimumSize = new Size(40, Math.Max(38, value));
            }
        }

        [Browsable(false)]
        public override Size MinimumSize
        {
            get
            {
                return base.MinimumSize;
            }
        }

        
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button maximum size"), DefaultValue("500,50")]
        new public Size MaximumSize
        {
            get
            {
                return base.MaximumSize;
            }
            set
            {
                if (value != Tokens.ZeroSize)
                {
                    value = new Size(Math.Max(40, value.Width), Math.Max(38, value.Height));
                }
                if (base.MaximumSize != value)
                {
                    base.MaximumSize = value;
                    tlpSplitButton.MaximumSize = new Size(value.Width, value.Height);
                    btnLeft.MaximumSize = new Size(Math.Max(0, value.Width - 40), value.Height);
                }
            }
        }
        #endregion /Size

        #region Visibility
        new public bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                if (base.Visible != value)
                {
                    base.Visible = value;
                    btnLeft.Visible = value;
                    btnRight.Visible = value;
                    tlpSplitButton.Visible = value;
                }
            }
        }
        #endregion /Visibility

        #region Constructor
        //[Designer(typeof(CustomControlDesigner_UserControl))]
        public SplitButton()
        {
            InitializeComponent();
            mouseDown_Left_Handler = new MouseEventHandler(BtnLeft_MouseDown);
            btnLeft.MouseDown += mouseDown_Left_Handler;
            mouseUp_Left_Handler = new MouseEventHandler(BtnLeft_MouseUp);
            btnLeft.MouseUp += mouseUp_Left_Handler;
            mouseOver_Left_Handler = new MouseEventHandler(BtnLeft_MouseOver);
            MouseOver += mouseOver_Left_Handler;

            mouseDown_Right_Handler = new MouseEventHandler(BtnRight_MouseDown);
            btnRight.MouseDown += mouseDown_Right_Handler;
            mouseUp_Right_Handler = new MouseEventHandler(BtnRight_MouseUp);
            btnRight.MouseUp += mouseUp_Right_Handler;
            mouseOver_Right_Handler = new MouseEventHandler(BtnRight_MouseOver);
            MouseOver += mouseOver_Right_Handler;

            mouseEnter_Handler = new EventHandler(MouseEnterTriggered);
            tlpSplitButton.MouseEnter += mouseEnter_Handler;
            btnLeft.MouseEnter += mouseEnter_Handler;
            btnRight.MouseEnter += mouseEnter_Handler;

            mouseLeave_Handler = new EventHandler(MouseLeaveTriggered);
            tlpSplitButton.MouseLeave += mouseLeave_Handler;
            btnLeft.MouseLeave += mouseLeave_Handler;
            btnRight.MouseLeave += mouseLeave_Handler;
            btnRight.BackgroundImageLayout = ImageLayout.Zoom;
            base.MinimumSize = new Size(40, 38); 
        }

        ~SplitButton()
        {
            btnLeft.MouseDown -= mouseDown_Left_Handler;
            btnLeft.MouseUp -= mouseUp_Left_Handler;

            btnRight.MouseDown -= mouseDown_Right_Handler;
            btnRight.MouseUp -= mouseUp_Right_Handler;

            btnLeft.MouseEnter -= mouseEnter_Handler;
            btnRight.MouseEnter -= mouseEnter_Handler;
            btnLeft.MouseLeave -= mouseLeave_Handler;
            btnRight.MouseLeave -= mouseLeave_Handler;

            tlpSplitButton.MouseLeave -= mouseLeave_Handler;
        }
        #endregion

        #region On Resize
        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, width, height, specified);
            switch (specified)
            {
                case BoundsSpecified.Size:
                    tlpSplitButton.Size = new Size(width, height);
                    btnLeft.Size = new Size(Math.Max(0, width - 40), height);
                    return;
                case BoundsSpecified.Width:
                    tlpSplitButton.Width = width;
                    btnLeft.Width = Math.Max(0, width - 40);
                    return;
                case BoundsSpecified.Height:
                    tlpSplitButton.Height = height;
                    btnLeft.Height = height;
                    return;
            }
            
        }

   
        #endregion /On Resize

        #region Control Event Handlers

        #region Button Left
        private void BtnLeft_MouseDown(object _, MouseEventArgs e)
        {
            lock (btnRight)
            {
                btnRight.BackColor = MouseDownBackColor;
                tlpSplitButton.BackColor = MouseDownBackColor;
            }
        }

        private void BtnLeft_MouseUp(object _, MouseEventArgs e)
        {
            lock (btnRight)
            {
                if (mouseEnterFlag)
                {// We are still in the control
                    btnRight.BackColor = MouseOverBackColor;
                    tlpSplitButton.BackColor = MouseOverBackColor;
                }
                else
                {
                    btnRight.BackColor = BackColor;
                    tlpSplitButton.BackColor = BackColor;
                }
            }
        }

        private void BtnLeft_MouseOver(object _, MouseEventArgs e)
        {
            //tlpSplitButton.BackColor = MouseOverBackColor;
            //btnRight.BackColor = MouseOverBackColor;
        }
        #endregion /Button Left

        #region Button Right
        private void BtnRight_MouseDown(object _, EventArgs e)
        {
            lock (btnLeft)
            {
                btnLeft.BackColor = MouseDownBackColor;
                tlpSplitButton.BackColor = MouseDownBackColor;
            }
        }

        private void BtnRight_MouseUp(object _, EventArgs e)
        {
            lock (btnLeft)
            {
                if (mouseEnterFlag)
                {// We are still in the control
                    btnLeft.BackColor = MouseOverBackColor;
                    tlpSplitButton.BackColor = MouseOverBackColor;
                }
                else
                {
                    btnLeft.BackColor = BackColor;
                    tlpSplitButton.BackColor = BackColor;
                }
            }
        }

        private void BtnRight_MouseOver(object _, MouseEventArgs e)
        {
            //tlpSplitButton.BackColor = MouseOverBackColor;
            //btnLeft.BackColor = MouseOverBackColor;
        }
        #endregion /Button Right

        #region Mouse Over
        private void TlpSplitButton_MouseOver(object _, EventArgs e)
        {
            //tlpSplitButton.BackColor = MouseOverBackColor;
            //btnLeft.BackColor = MouseOverBackColor;
            //btnRight.BackColor = mouseOverBackColor;
        }
        #endregion /Mouse Over

        #endregion /Control Event Handlers
    }
}

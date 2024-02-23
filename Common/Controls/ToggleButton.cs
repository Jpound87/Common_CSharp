using Common.Extensions;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Common.Controls
{
    public partial class ToggleButton : UserControl
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

        #region Enter
        private bool mouseEnterFlag;
        private readonly EventHandler mouseEnter_Handler;
        private void MouseEnterTriggered(object entered, EventArgs eventArgs)
        {
            lock (mouseEnter_Handler)
            {
                if (tlpSplitButton.IsMouseOverControl(out Point cursorPosition))
                {
                    mouseLeaveFlag = false;
                    if (!mouseEnterFlag && entered is Control enteredControl)
                    {
                        mouseEnterFlag = true;
                        MouseEnter?.Invoke(this, eventArgs);
                        //enteredControl.BackColor = MouseOverBackColor;
                        //rdoRight.BackColor = MouseOverBackColor;
                        //tlpSplitButton.BackColor = MouseOverBackColor;
                        //Invalidate();
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
                //if (!tlpSplitButton.IsMouseOverControl(mouseExclusionBoundry))
                {
                    mouseEnterFlag = false;
                    if (!mouseLeaveFlag)
                    {
                        mouseLeaveFlag = true;
                        rdoLeft.BackColor = BackColor;
                        rdoRight.BackColor = BackColor;
                        tlpSplitButton.BackColor = BackColor;
                        MouseLeave?.Invoke(this, eventArgs);
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
                    rdoLeft.MouseDown += value;
                    rdoRight.MouseDown += value;
                }
            }
            remove
            {
                lock (tlpSplitButton)
                {
                    rdoLeft.MouseDown -= value;
                    rdoRight.MouseDown -= value;
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
                    rdoLeft.MouseUp += value;
                    rdoRight.MouseUp += value;
                }
            }
            remove
            {
                lock (tlpSplitButton)
                {
                    rdoLeft.MouseUp -= value;
                    rdoRight.MouseUp -= value;
                }
            }
        }
        #endregion /Up

        #region Over
        private readonly MouseEventHandler mouseOver_Right_Handler;// TODO: use enter and leave events for back color uniformity
        private readonly MouseEventHandler mouseOver_Left_Handler;
        public event MouseEventHandler MouseOver;
        #endregion /Over

        #region Click
        new public event EventHandler Click
        {
            add
            {
                lock (tlpSplitButton)
                {
                    rdoLeft.Click += value;
                    rdoRight.Click += value;
                    base.Click += value;
                }
            }
            remove
            {
                lock (tlpSplitButton)
                {
                    rdoLeft.Click -= value;
                    rdoRight.Click -= value;
                    base.Click -= value;
                }
            }
        }

        public event MouseEventHandler MouseDown_RightButton
        {
            add
            {
                lock (tlpSplitButton)
                { 
                    rdoRight.MouseDown += value;
                }
            }
            remove
            {
                lock (tlpSplitButton)
                {
                    rdoRight.MouseDown -= value;
                }
            }
        }

        public event MouseEventHandler MouseDown_LeftButton
        {
            add
            {
                lock (tlpSplitButton)
                {
                    rdoLeft.MouseDown += value;
                }
            }
            remove
            {
                lock (tlpSplitButton)
                {
                    rdoLeft.MouseDown -= value;
                }
            }
        }
        #endregion /Click

        #endregion /Mouse

        #endregion /Events

        #region Accessors
        public bool IsOn 
        { 
            get
            {
                return rdoRight.Checked;
            }
            set
            {
                if (rdoRight.Checked != value)
                {
                    rdoRight.Checked = value;
                   
                }
            }
        }

        public bool WarningState { get; set; }
        #endregion /Accessors

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
                rdoLeft.BackColor = value;
                rdoRight.BackColor = value;
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
            }
        }

        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button forground color"), DefaultValue(typeof(SystemColors), "ControlText")]
        public Color ForeColor_Left
        {
            get
            {
                return rdoLeft.ForeColor;
            }
            set
            {
                rdoLeft.ForeColor = value;
            }
        }

        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button forground color"), DefaultValue(typeof(SystemColors), "ControlText")]
        public Color ForeColor_Right
        {
            get
            {
                return rdoRight.ForeColor;
            }
            set
            {
                rdoRight.ForeColor = value;
            }
        }

        private Color mouseOverBackColor_Left;
        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("FlatAppearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button mouse over color"), DefaultValue(typeof(SystemColors), "ControlLight")]
        public Color MouseOverBackColor_Left
        {
            get
            {
                return mouseOverBackColor_Left;
            }
            set
            {
                mouseOverBackColor_Left = value;
                rdoLeft.FlatAppearance.MouseOverBackColor = value;
            }
        }

        private Color mouseOverBackColor_Right;
        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("FlatAppearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button mouse over color"), DefaultValue(typeof(SystemColors), "ControlLight")]
        public Color MouseOverBackColor_Right
        {
            get
            {
                return mouseOverBackColor_Right;
            }
            set
            {
                mouseOverBackColor_Right = value;
                rdoRight.FlatAppearance.MouseOverBackColor = value;
            }
        }

        private Color mouseDownBackColor_Left;
        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("FlatAppearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button mouse over color"), DefaultValue(typeof(SystemColors), "ControlDark")]
        public Color MouseDownBackColor_Left
        {
            get
            {
                return mouseDownBackColor_Left;
            }
            set
            {
                mouseDownBackColor_Left = value;
                rdoLeft.FlatAppearance.MouseDownBackColor = value;
            }
        }

        private Color mouseDownBackColor_Right;
        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("FlatAppearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button mouse over color"), DefaultValue(typeof(SystemColors), "ControlDark")]
        public Color MouseDownBackColor_Right
        {
            get
            {
                return mouseDownBackColor_Right;
            }
            set
            {
                mouseDownBackColor_Right = value;
                rdoRight.FlatAppearance.MouseDownBackColor = value;
            }
        }
        #endregion /Color

        #region Border
        [Browsable(false), DefaultValue(BorderStyle.None)]
        new public BorderStyle BorderStyle { get; set; }
        #endregion

        #region Text
        /// <summary>Get/Set button text</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button text"), DefaultValue("Split Button")]
        new public String Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                    base.Text = value;
                    //rdoLeft.Text = value;
            }
        }

        /// <summary>Get/Set button text</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button text alignment"), DefaultValue(ContentAlignment.MiddleCenter)]
        public ContentAlignment TextAlign
        {
            get
            {
                return rdoLeft.TextAlign;
            }
            set
            {
                rdoLeft.TextAlign = value;
                rdoRight.TextAlign = value;
            }
        }

        new public Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                    base.Font = value;
                    rdoLeft.Font = value;
            }
        }
        #endregion /Text

        #region Image
        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button Background image")]
        public Image BackgroundImage_Right
        {
            get
            {
                return rdoRight.BackgroundImage;
            }
            set
            {
                rdoRight.BackgroundImage = value;
            }
        }

        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button Background image")]
        public Image BackgroundImage_Left
        {
            get
            {
                return rdoLeft.BackgroundImage;
            }
            set
            {
                rdoLeft.BackgroundImage = value;
            }
        }

        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button image")]
        public Image Image
        {
            get
            {
                return rdoRight.Image;
            }
            set
            {
                rdoRight.Image = value;
            }
        }

        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button image"), DefaultValue(ContentAlignment.MiddleCenter)]
        public ContentAlignment ImageAlign
        {
            get
            {
                return rdoRight.ImageAlign;
            }
            set
            {
                rdoRight.ImageAlign = value;
            }
        }
        #endregion

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
                    rdoLeft.Visible = value;
                    rdoRight.Visible = value;
                    tlpSplitButton.Visible = value;
                }
            }
        }
        #endregion /Visibility

        #region Constructor
        public ToggleButton()
        {
            InitializeComponent();
            rdoLeft.FlatStyle = FlatStyle.Flat;
            rdoRight.FlatStyle = FlatStyle.Flat;

            rdoLeft.BackgroundImageLayout = ImageLayout.Zoom;
            rdoRight.BackgroundImageLayout = ImageLayout.Zoom;
          
            mouseDown_Left_Handler = new MouseEventHandler(BtnLeft_MouseDown);
            rdoLeft.MouseDown += mouseDown_Left_Handler;
            mouseUp_Left_Handler = new MouseEventHandler(BtnLeft_MouseUp);
            rdoLeft.MouseUp += mouseUp_Left_Handler;
         
            mouseOver_Left_Handler = new MouseEventHandler(BtnLeft_MouseOver);
            MouseOver += mouseOver_Left_Handler;

            mouseDown_Right_Handler = new MouseEventHandler(BtnRight_MouseDown);
            rdoRight.MouseDown += mouseDown_Right_Handler;
            mouseUp_Right_Handler = new MouseEventHandler(BtnRight_MouseUp);
            rdoRight.MouseUp += mouseUp_Right_Handler;
            mouseOver_Right_Handler = new MouseEventHandler(BtnRight_MouseOver);
            MouseOver += mouseOver_Right_Handler;

            mouseEnter_Handler = new EventHandler(MouseEnterTriggered);
            rdoLeft.MouseEnter += mouseEnter_Handler;
            rdoRight.MouseEnter += mouseEnter_Handler;

            mouseLeave_Handler = new EventHandler(MouseLeaveTriggered);
            rdoLeft.MouseLeave += mouseLeave_Handler;
            rdoRight.MouseLeave += mouseLeave_Handler;
        }

        ~ToggleButton()
        {
            rdoLeft.MouseDown -= mouseDown_Left_Handler;
            rdoLeft.MouseUp -= mouseUp_Left_Handler;

            rdoRight.MouseDown -= mouseDown_Right_Handler;
            rdoRight.MouseUp -= mouseUp_Right_Handler;

            rdoLeft.MouseEnter -= mouseEnter_Handler;
            rdoRight.MouseEnter -= mouseEnter_Handler;
            rdoLeft.MouseLeave -= mouseLeave_Handler;
            rdoRight.MouseLeave -= mouseLeave_Handler;
        }
        #endregion

        #region On Resize
        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, width, height, specified);
            switch (specified)
            {
                case BoundsSpecified.Size:
                    //tlpSplitButton.Size = new Size(width, height);
                    return;
                case BoundsSpecified.Width:
                    //tlpSplitButton.Width = width;
                    return;
                case BoundsSpecified.Height:
                    //tlpSplitButton.Height = height;
                    return;
            }
        }
        #endregion /On Resize

        #region Control Event Handlers

        #region Button Left
        private void BtnLeft_MouseDown(object _, MouseEventArgs e)
        {
            rdoLeft.Checked = true;
            rdoRight.Checked = false;
        }

        private void BtnLeft_MouseUp(object _, MouseEventArgs e)
        {
            //lock (rdoRight)
            //{
            //    if (mouseEnterFlag)
            //    {// We are still in the control
            //        rdoRight.BackColor = MouseOverBackColor;
            //        tlpSplitButton.BackColor = MouseOverBackColor;
            //    }
            //    else
            //    {
            //        rdoRight.BackColor = BackColor;
            //        tlpSplitButton.BackColor = BackColor;
            //    }
            //}
        }

        private void BtnLeft_MouseOver(object _, MouseEventArgs e)
        {
      
        }
        #endregion /Button Left

        #region Button Right

        private void BtnRight_MouseDown(object _, MouseEventArgs e)
        {
            rdoLeft.Checked = false;
            rdoRight.Checked = true;
        }

        private void BtnRight_MouseUp(object _, MouseEventArgs e)
        {
            //lock (rdoLeft)
            //{
            //    if (mouseEnterFlag)
            //    {// We are still in the control
            //        rdoLeft.BackColor = MouseOverBackColor;
            //        tlpSplitButton.BackColor = MouseOverBackColor;
            //    }
            //    else
            //    {
            //        rdoLeft.BackColor = BackColor;
            //        tlpSplitButton.BackColor = BackColor;
            //    }
            //}
        }

        private void BtnRight_MouseOver(object _, MouseEventArgs e)
        {
           
        }
        #endregion /Button Right

        #endregion /Control Event Handlers
    }
}

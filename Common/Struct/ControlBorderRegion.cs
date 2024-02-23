using System;
using System.Drawing;
using System.Windows.Forms;

namespace Common.Struct
{
    public struct ControlBorderRegion
    {
        #region Accessors
        public Int32 Top { get; set; }
        public Int32 Left { get; set; }
        public Int32 Right { get; set; }
        public Int32 Bottom { get; set; }
        /// <summary>
        /// Total width of the left and right borders of the form.
        /// </summary>
        public readonly Int32 Width
        {
            get
            {
                return Left + Right;
            }
        }
        /// <summary>
        /// Total height of the top and bottorm borders of the form.
        /// </summary>
        public readonly Int32 Height
        {
            get
            {
                return Top + Bottom;
            }
        }
        public Size MinimumSize { get; set; }
        #endregion /Accessors

        #region Constructor
        /// <summary>
        /// This constructor will determine the distance of the control from the edge of the application
        /// </summary>
        /// <param name="control"></param>
        public ControlBorderRegion(Control control)
        {
            Rectangle screenRectangle = control.RectangleToScreen(control.ClientRectangle);
            Top = screenRectangle.Top - control.Top;
            Left = screenRectangle.Left - control.Left;
            Right = screenRectangle.Right - control.Right;
            Bottom = screenRectangle.Bottom - control.Bottom;
            MinimumSize = control.MinimumSize;
        }
        public ControlBorderRegion(Control control, Control container)
        {
            Rectangle containerRectangle = container.RectangleToScreen(container.ClientRectangle);
            Rectangle controlRectangle = control.RectangleToScreen(control.ClientRectangle);
            Top = containerRectangle.Top - controlRectangle.Top;
            Left = containerRectangle.Left - controlRectangle.Left;
            Right = containerRectangle.Right - controlRectangle.Right;
            Bottom = containerRectangle.Bottom - controlRectangle.Bottom;
            MinimumSize = control.MinimumSize;
        }
        #endregion Constructor
    }
}

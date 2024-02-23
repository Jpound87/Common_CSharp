using System;
using System.Drawing;
using System.Windows.Forms;

namespace Common.Win32
{
    public static class Win32Helper
    {
        #region Accessors
        private static readonly bool isRunningOnMono = Type.GetType("Mono.Runtime") != null;
        public static bool IsRunningOnMono { get { return isRunningOnMono; } }
        #endregion

        #region Methods
        public static Control ControlAtPoint(Point pt)
        {
            return Control.FromChildHandle(NativeMethods.WindowFromPoint(pt));
        }

        public static uint MakeLong(int low, int high)
        {
            return (uint)((high << 16) + low);
        }

        public static uint HitTestCaption(Control control)
        {
            var captionRectangle = new Rectangle(0, 0, control.Width, control.ClientRectangle.Top - control.PointToClient(control.Location).X);
            return captionRectangle.Contains(Control.MousePosition) ? (uint)2 : 0;
        }
        #endregion
    }
}

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Common
{
    public static class Extensions_Form
    {
        #region Center To Screen
        public static void CenterToScreen(this Form form, Point screenLocation)
        {
            form.Location = screenLocation;
            Screen screen = Screen.FromControl(form);
            Rectangle workingArea = screen.WorkingArea;
            form.Location = new Point()
            {
                X = Math.Max(workingArea.X, workingArea.X + (workingArea.Width - form.Width) / 2),
                Y = Math.Max(workingArea.Y, workingArea.Y + (workingArea.Height - form.Height) / 2)
            };
        }
        #endregion /Center To Screen

        #region Invoke
        //public static void Invoke(this Form form, Action action,  method)
        //{

        //}

        #endregion /Invoke
    }
}

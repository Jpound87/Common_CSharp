using System;
using System.Drawing;
using System.Windows.Forms;

namespace Common.Utility
{
    public static class Utility_ComboBox
    {
        #region Identity
        public const String ClassName = nameof(Utility_ComboBox);
        #endregion

        #region Drawing
        /// <summary>
        /// This method draws a rectangle corresponding to the color of a ComboBoxItem option
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void DrawColorRectangle_ComboBox(object sender, DrawItemEventArgs e)
        {
            if (sender is ComboBox cboSender)
            {
                Graphics g = e.Graphics;
                e.DrawBackground();
                Rectangle rect = e.Bounds;
                if (e.Index >= 0)
                {
                    string n = cboSender.Items[e.Index].ToString();
                    Font f = new Font("Arial", 9, FontStyle.Regular);
                    Color c = Color.FromName(n);
                    Brush b = new SolidBrush(c);
                    g.DrawString(n, f, Brushes.Black, rect.X, rect.Top);
                    //Set the location of the rectangle to be of the rigt side. Set its width to be 1/5 of the size of the box. The " - 10"'s and "+/- 5"'s are for padding
                    g.FillRectangle(b, rect.Width - (rect.Width / 5) - 5, rect.Y + 5, rect.Width / 5, rect.Height - 10);
                    e.DrawFocusRectangle();// EastAsianLunisolarCalendar???
                }
            }
        }
        #endregion
    }
}

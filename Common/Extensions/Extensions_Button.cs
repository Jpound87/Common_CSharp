using System;
using System.Windows.Forms;

namespace Common
{
    public static class Extensions_Button
    {
        #region Identity
        public const String FormName = nameof(Extensions_Button);
        #endregion

        #region Check
        public static bool CheckIfButton(this Control control)
        {
            if (control is Button)
            {
                return true;
            }
            return false;
        }

        public static bool CheckIfButton(this Control control, out Button button)
        {
            if (control is Button butt)// This method is butt, why did we make it?
            {
                button = butt;
                return true;
            }
            button = default;
            return false;
        }
        #endregion /Check
    }
}

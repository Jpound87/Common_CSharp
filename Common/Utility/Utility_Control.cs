using System;
using System.Drawing;
using System.Windows.Forms;

namespace Common.Utility
{
    public class Utility_Control
    {
        #region Width

        #region Maximum
        public const Int32 MAX_MEASURE_INVALID_TOKEN = -1; 
        public static Int32 MaximumWidthOf(params Control[] controls)
        {
            int maxWidth = MAX_MEASURE_INVALID_TOKEN;
            foreach(Control control in controls)
            {
                if (control != null)
                {
                    maxWidth = Math.Max(maxWidth, control.Width);
                }
            }
            return maxWidth;
        }
        #endregion /Maximum

        #region Minimum
        public static Int32 MinimumWidthOf(params Control[] controls)
        {
            int minWidth = 0;
            foreach (Control control in controls)
            {
                if (control != null)
                {
                    minWidth = Math.Min(minWidth, control.Width);
                }
            }
            return minWidth;
        }
        #endregion /Minimum

        #endregion /Width

        #region Height

        #region Maximum
        public static Int32 MaximumHeightOf(params Control[] controls)
        {
            int maxHeight = MAX_MEASURE_INVALID_TOKEN;
            foreach (Control control in controls)
            {
                if (control != null)
                {
                    maxHeight = Math.Max(maxHeight, control.Height);
                }
            }
            return maxHeight;
        }
        #endregion /Maximum

        #region Minimum
        public static Int32 MinimumHeightOf(params Control[] controls)
        {
            int minHeight = 0;
            foreach (Control control in controls)
            {
                if (control != null)
                {
                    minHeight = Math.Min(minHeight, control.Height);
                }
            }
            return minHeight;
        }
        #endregion Minimum

        #endregion /Height

        #region Size

        #region Maximum
        public static Size MaximumSizeOf(params Control[] controls)
        {
            int maxHeight = MaximumHeightOf(controls);
            int maxWidth = MaximumWidthOf(controls);
            if (maxWidth >= 0 && maxHeight >= 0)
            {
                return new Size(maxWidth, maxHeight);
            }
            return new Size(0,0);
        }
        #endregion /Maximum

        #region Minimum
        public static Size MinimumSizeOf(params Control[] controls)
        {
            int minHeight = MinimumHeightOf(controls);
            int minWidth = MinimumWidthOf(controls);
            if (minWidth >= 0 && minHeight >= 0)
            {
                return new Size(minWidth, minHeight);
            }
            return new Size(0, 0);
        }
        #endregion Minimum

        #endregion /Size
    }
}

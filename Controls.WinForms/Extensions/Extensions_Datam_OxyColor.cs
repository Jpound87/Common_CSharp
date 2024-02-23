using OxyPlot;
using System;
using System.Diagnostics;
using System.Drawing;

namespace AM_WinForms.Datam.Extensions
{
    public static class Extensions_Datam_OxyColor
    {
        #region Identity
        public const String FormName = nameof(Extensions_Datam_OxyColor);
        #endregion

        #region Get
        /// <summary>
        /// This method takes in an OxyColor and returns the equivalent Color. If no equivalent can be found, Color.Black is returned.
        /// </summary>
        /// <param name="oxyColor"></param>
        /// <returns></returns>
        public static Color ColorFromOxyColor(this OxyColor oxyColor)
        {
            try
            {
                return Color.FromArgb(oxyColor.A, oxyColor.R, oxyColor.G, oxyColor.B);
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex.Message);
#endif
                return Color.Black;
            }
        }
        /// <summary>
        /// This method takes in a Color and returns the equivalent OxyColor. If no equivalent can be found, OxyColors.Black is returned.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static OxyColor OxyColorFromColor(this Color color)
        {
            try
            {
                return OxyColor.FromArgb(color.A, color.R, color.G, color.B);
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex.Message);
#endif
                return OxyColors.Black;

            }
        }
        

        #endregion
    }
}

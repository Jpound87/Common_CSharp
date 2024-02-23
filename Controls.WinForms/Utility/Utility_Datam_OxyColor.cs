using OxyPlot;
using System;
using System.Diagnostics;
using System.Drawing;
using AM_WinForms.Datam.Extensions;

namespace AM_WinForms.Datam.Utility
{
    public static class Utility_Datam_OxyColor
    {
        #region Identity
        public const String FormName = nameof(Utility_Datam_OxyColor);
        #endregion

        #region Array
        /// <summary>
        /// This array contains the a chosen distribution of the colors
        /// for Oxyplot
        /// </summary>
        public static readonly OxyColor[] oxyColors = new OxyColor[10]
        {
            #region Colors
            OxyColors.Green,
            OxyColors.Goldenrod,
            OxyColors.Firebrick,
            OxyColors.CornflowerBlue,
            OxyColors.Chartreuse,
            OxyColors.Crimson,
            OxyColors.Blue,
            OxyColors.Gray,
            OxyColors.Aquamarine,
            OxyColors.Brown
            #endregion /Colors
        };
        #endregion /Array

        #region Get
        /// <summary>
        /// This method takes in a string and returns the equivalent OxyColor. If no equivalent can be found, OxyColors.Black is returned.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static OxyColor OxyColorFromName(string name)
        {
            try
            {
                Color color = Color.FromName(name);//Convert the string to a color
                return color.OxyColorFromColor();//Convert the color to an OxyColor
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex.Message);
#endif
                return OxyColors.Black;
            }
        }
        /// <summary>
        /// This method takes in a string and returns the equivalent OxyColor. If no equivalent can be found, OxyColors.Black is returned.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool TryGetColorsFromName(string name, out Color color, out OxyColor oxyColor)
        {
            try
            {
                color = Color.FromName(name);//Convert the string to a color
                oxyColor = color.OxyColorFromColor();//Convert the color to an OxyColor
                return true;
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex.Message);
#endif
                color = Color.Black;
                oxyColor = OxyColors.Black;
            }
            return false;
        }
        #endregion /Get
    }
}

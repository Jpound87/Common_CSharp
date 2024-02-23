using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Common
{
    public static class Extensions_Color
    {
        #region Contrast

        /// <summary>
        /// This method is designed to calculate whether text color needs to be white or black based on the luminance of the background color
        /// </summary>
        /// <param name="backgroundColor"></param>
        /// <returns></returns>
        public static Color GetTextColor_Contrasting(this Color backgroundColor)
        {
            int col;
            // Get the luminance (this equation found online) - human eye favors green
            double lum = (0.299 * backgroundColor.R + 0.587 * backgroundColor.G + 0.114 * backgroundColor.B) / 255;
            //NOTE: if it's split into three colors (White, Black, Grey)
            //there IS a case where a dark grey background will have dark grey text, and nothing will be visible
            if (lum > 0.85)
            {//bright color -> dark gray font (reduces eye strain)
                col = 68;
            }
            else if (lum > 0.5) 
            {//kinda bright -> black front
                col = 0;
            }
            else if (lum > 0.15)
            {//kinda dark bg -> white font
                col = 255;
            }
            else
            {//dark color -> light gray font (reduces eye strain)
                col = 187;
            }

            return Color.FromArgb(col, col, col);
        }
        #endregion

        #region From String
        public static Color FromHex(this string hex) => ColorTranslator.FromHtml(hex);
        #endregion

        #region Color Matrix
        public static ColorMatrix ToColorMatrix(this Color color)
        {
            return new ColorMatrix
            {// Convert and refactor color palette.
                Matrix00 = ParseColor_ToSingle(color.R),
                Matrix11 = ParseColor_ToSingle(color.G),
                Matrix22 = ParseColor_ToSingle(color.B),
                Matrix33 = ParseColor_ToSingle(color.A),
                Matrix44 = 1f
            };
        }

        /// <summary>Convert rgb to float</summary>
        public static Single ParseColor_ToSingle(this Byte color)
        {
            return color == 0 ? 0 : ((float)color / 255);
        }
        #endregion /Color Matrix
    }
}

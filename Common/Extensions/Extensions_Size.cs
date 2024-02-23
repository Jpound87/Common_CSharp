using System;
using System.Drawing;

namespace Common
{
    public static class Extensions_Size
    {
        #region Identity
        public const String ClassName = nameof(Extensions_Size);
        #endregion

        #region Comparison
        /// <summary>
        /// This method will return true if any dimension in the size is grater than the other given sizes.
        /// </summary>
        /// <param name="size">The size to check dimensions against.</param>
        /// <param name="compareSizes">The size or sizes to compare with.</param>
        /// <returns></returns>
        public static bool HasDimensionGreater(this Size size, params Size[] compareSizes)
        {
            for (int cs = 0; cs < compareSizes.Length; cs++)
            {
                if (size.Width > compareSizes[cs].Width || size.Height > compareSizes[cs].Height)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Square
        public static Size EnforceSquare_Min(this Size size)
        {
            int minSide = Math.Min(size.Height, size.Width);
            return new Size(minSide, minSide);
        }

        public static Size EnforceSquare_Max(this Size size)
        {
            int maxSide = Math.Max(size.Height, size.Width);
            return new Size(maxSide, maxSide);
        }

        public static Size EnforceSquare_Height(this Size size)
        {
            return new Size(size.Height, size.Height);
        }

        public static Size EnforceSquare_Width(this Size size)
        {
            return new Size(size.Width, size.Width);
        }
        #endregion
    }
}

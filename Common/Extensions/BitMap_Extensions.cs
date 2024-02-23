using System.Drawing;
using System.Drawing.Imaging;

namespace Common.Extensions
{
    public static class BitMap_Extensions
    {
        #region Color Matricies
        public static readonly ColorMatrix InvertColorMatrix = new ColorMatrix(new float[][]
        {
            new float[] {-1, 0, 0, 0, 0},
            new float[] {0, -1, 0, 0, 0},
            new float[] {0, 0, -1, 0, 0},
            new float[] {0, 0, 0, 1, 0},
            new float[] {1, 1, 1, 0, 1}
        });
        #endregion /Color Matricies

        #region Invert
        public static Bitmap InvertColor(this Bitmap bitmap)
        {
            Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            using (ImageAttributes imageAttributes = new ImageAttributes())
            {

                imageAttributes.SetColorMatrix(InvertColorMatrix);

                using (Graphics g = Graphics.FromImage(newBitmap))
                {
                    g.DrawImage(bitmap, new Rectangle(0, 0,
                    bitmap.Width, bitmap.Height), 0, 0,
                    bitmap.Width, bitmap.Height, GraphicsUnit.Pixel,
                    imageAttributes);
                }
            }
            return newBitmap;
        }
        #endregion /Invert
    }
}

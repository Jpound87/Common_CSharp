using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace Common.Extensions
{
    public static class Extensions_Image
    {
        #region Identity
        public static String Classame = nameof(Extensions_Image);
        #endregion

        #region Image Retrieval
        /// <summary>
        /// This method will retrieve images from the assembly stored images. 
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="directory">Image location in the assembly.</param>
        /// <returns></returns>
        public static Bitmap GetImage(this Assembly assembly, String directory)
        {// TODO: move to information manager

#if DEBUG
            string[] names = assembly.GetManifestResourceNames();
#endif
            try
            { 
                Stream imageStream = assembly.GetManifestResourceStream(directory);
                return new Bitmap(imageStream);// TODO: default no image on error
            }
            catch
            {
                return default;
            }
        }
        #endregion

        #region Scaling
        public static Image Scale(this Image img, Size size)
        {
            return Scale(img, size.Width, size.Height);
        }

        public static Image Scale(this Image img, Double maxWidth, Double maxHeight)
        {
            double ratioX = maxWidth / img.Width;
            double ratioY = maxHeight / img.Height;
            double ratio = Math.Min(ratioX, ratioY);

            int newWidth = Convert.ToInt32(img.Width * ratio);
            int newHeight = Convert.ToInt32(img.Height * ratio);

            Image newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(img, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }
        #endregion /Scaling

        #region Invert
        public static Image InvertColor(this Image image)
        {
            using (Bitmap bitmap = new Bitmap(image))
            {
                image = bitmap.InvertColor();
                return image;
            }
        }
        #endregion /Invert
    }
}

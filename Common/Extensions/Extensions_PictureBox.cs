using Common.Extensions;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Common
{
    public static class Extensions_PictureBox
    {
        #region Image

        #region Animated
        public static void RestartToFrameZero(this PictureBox pictureBox)
        {
            pictureBox.RestartToFrameIndex();
        }

        public static void RestartToFrameIndex(this PictureBox pictureBox, int index = 0)
        {
            pictureBox.Image.SelectActiveFrame(new FrameDimension(pictureBox.Image.FrameDimensionsList[0]), index);
            pictureBox.Image = pictureBox.Image;
        }
        #endregion /Animated

        #region Invert
        public static void InvertColor(this PictureBox pictureBox)
        {
            if (pictureBox.BackgroundImage != null)
            {
                pictureBox.BackgroundImage = pictureBox.BackgroundImage.InvertColor();
            }
            if (pictureBox.Image != null)
            {
                pictureBox.Image = pictureBox.Image.InvertColor();
            }
        }
        #endregion /Invert

        #endregion /Image
    }
}

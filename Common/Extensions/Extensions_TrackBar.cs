using System.Windows.Forms;

namespace Common
{
    public static class Extensions_TrackBar
    {
        #region Snap To [X]
        public static void SnapToZero(this TrackBar trackBar)
        {
            trackBar.SnapToX();
        }

        public static void SnapToX(this TrackBar trackBar, int x = 0)
        {
            double close = (trackBar.Maximum - trackBar.Minimum) * .005;
            if (trackBar.Value > x && trackBar.Value < x + close)
            {
                trackBar.Value = x;
            }
            else if (trackBar.Value < x && trackBar.Value > x - close)
            {
                trackBar.Value = x;
            }
        }
        #endregion /Snap To [X]
    }
}

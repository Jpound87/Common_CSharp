using System.Windows.Forms;

namespace Common.Controls
{
    public class DoubleBufferedTableLayoutPanel : TableLayoutPanel
    {
        public DoubleBufferedTableLayoutPanel()
            : base()
        {
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }
    }
}

using System.Drawing;
using System.Windows.Forms;

namespace Common.Interface
{
    public interface IHitTest
    {
        DockStyle HitTest(Point pt);
        DockStyle Status { get; set; }
    }
}

using System.Windows.Forms;

namespace Common.Interface
{
    public interface IDragSource
    {
        #region Accessors
        Control DragControl { get; }
        #endregion
    }
}

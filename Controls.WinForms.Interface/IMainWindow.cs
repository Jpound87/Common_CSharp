using System.Windows.Forms;

namespace Datam.WinForms.Interface
{
    public interface IMainWindow : IContainerControl
    {
        #region Accessors
        int BorderWidth { get; }

        int BorderHeight { get; }

        Control This { get; }
        #endregion
    }
}

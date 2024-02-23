using Devices.Interface;
using System.Windows.Forms;
using UI.Interface;

namespace Datam.WinForms.Interface
{
    /// <summary>
    /// The interface for tree node data.
    /// </summary>
    public interface ITreeNodeData_Scan : ITreeNodeData
    {
        #region Accessors
        TreeNode TreeNode { get; }
        ICommunicator Communicator { get; }
        IInformation_Communicator CommunicatorData { get; }
        #endregion /Accessors
    }
}

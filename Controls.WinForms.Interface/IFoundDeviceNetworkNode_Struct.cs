using Common.Constant;
using Devices.Interface.EventArgs;
using System;
using System.Windows.Forms;

namespace Datam.WinForms.Interface
{
    public interface IFoundDeviceNetworkNode_Struct
    {
        #region Accessors
        String NetworkName { get; }
        int ProtocolImageIndex { get; }
        IDeviceFoundEventArgs DeviceFoundEventArgs { get; }
        TreeNode CommunicatorTreeNode { get; }
        ProtocolType ProtocolType { get; }
        #endregion /Accessors

        #region Methods
        TreeNode CreateProtocolDeviceNode();
        #endregion
    }
}

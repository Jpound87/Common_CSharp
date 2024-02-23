using Common.Constant;
using Datam.WinForms.Interface;
using Devices.Interface.EventArgs;
using System;
using System.Windows.Forms;

namespace Datam.WinForms.Struct
{
    public struct FoundDeviceNetworkNode_Struct : IFoundDeviceNetworkNode_Struct
    {// I don't know what I've been told, but we just got a network node! (Sid Miers "Aplha Centari" callback anyone? Anyone...?)
        #region Globals
        public String NetworkName { get; private set; }
        public int ProtocolImageIndex { get; private set; }
        public IDeviceFoundEventArgs DeviceFoundEventArgs { get; private set; }
        public TreeNode CommunicatorTreeNode => DeviceFoundEventArgs.CommunicatorTreeNode;
        public ProtocolType ProtocolType => DeviceFoundEventArgs.ProtocolType;
        #endregion

        #region Constructor
        public FoundDeviceNetworkNode_Struct(String networkName, int imageIndex, IDeviceFoundEventArgs dfea)
        {
            NetworkName = networkName;
            ProtocolImageIndex = imageIndex;
            DeviceFoundEventArgs = dfea;
        }
        #endregion

        #region Methods
        /// <summary>
        /// This method uses the internally stored data to create the node for the selected network.
        /// </summary>
        public TreeNode CreateProtocolDeviceNode()
        {
            return CommunicatorTreeNode.Nodes.Add(NetworkName, NetworkName, ProtocolImageIndex, ProtocolImageIndex);
        }
        #endregion
    }
}

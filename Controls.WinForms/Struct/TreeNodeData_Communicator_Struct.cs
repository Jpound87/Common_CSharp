using Common.Constant;
using Datam.WinForms.Interface;
using Devices.Interface;
using System;
using System.Windows.Forms;

namespace Datam.WinForms.Struct
{
    public struct TreeNodeData_Communicator_Struct : ITreeNodeData_Scan
    {
        #region Identity
        public const String ClassName = nameof(TreeNodeData_Connection_Struct);
        public String Identity
        {
            get
            {
                return CommunicatorData.Name;
            }
        }
        #endregion

        #region Valididty
        public bool Valid { get; private set; }
        #endregion

        #region Globals
        public TreeNodeType TreeNodeType
        {
            get
            {
                return TreeNodeType.Communicator;
            }
        }

        public TreeNode TreeNode { get; private set; }


        public ICommunicator Communicator
        {
            get
            {
                return CommunicatorData.Communicator;
            }
        }

        public IInformation_Communicator CommunicatorData { get; private set; }
        #endregion

        #region Protocol
        public ProtocolType ProtocolType
        {
            get
            {
                return ProtocolType.None;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        #region Constructor
        public TreeNodeData_Communicator_Struct(TreeNode communicatorNode, IInformation_Communicator communicatorInfo)
        {
            TreeNode = communicatorNode;
            CommunicatorData = communicatorInfo;
            Valid = true;
        }
        #endregion
    }
}
using Common.Constant;
using System;
using UI.Interface;

namespace Datam.WinForms.Struct
{
    public struct TreeNodeData_Connection_Struct : ITreeNodeData
    {
        #region Identity
        public const String ClassName = nameof(TreeNodeData_Connection_Struct);
        public String Identity
        {
            get
            {
                return ClassName;
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
                return TreeNodeType.Network;
            }
        }

        public ProtocolType ProtocolType { get; set; }
        #endregion

        #region Constructor
        public TreeNodeData_Connection_Struct(ProtocolType connectionType)
        {
            ProtocolType = connectionType;
            Valid = true;
        }
        #endregion
    }
}

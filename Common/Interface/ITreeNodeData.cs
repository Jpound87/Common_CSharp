using Common;
using Common.Constant;

namespace UI.Interface
{
    /// <summary>
    /// The interface for tree node data.
    /// </summary>
    public interface ITreeNodeData : IValidate, IIdentifiable
    {
        #region Accessors
        TreeNodeType TreeNodeType { get; }
        ProtocolType ProtocolType { get; set; }
        #endregion
    }
}

using Common;
using Common.Constant;
using Common.Extensions;
using System;

namespace Connections.Interface
{
    public interface IMessageData : IValidate, IEquatable<IMessageData>
    {
        #region Type
        MessageType Type { get; }
        #endregion /Type

        #region Command
        Priority_Packet Priority { get; set; }
        int Net { get; }
        uint NodeID { get; }
        bool UseNodeID { get; }
        String Data { get; }
        int CommandHash { get; }
        #endregion /Command

        #region Result Action
        bool HasReaction { get; }
        Action EventAction { get; }
        void OnResultReceived();
        #endregion /Result Action
    }
}

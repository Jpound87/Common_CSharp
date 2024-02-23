using Common;
using System;

namespace Connections.Interface
{
    public interface IEventArgs_Request : IValidate
    {
        #region Packet
        IMessageData RequestPacket { get; }
        #endregion

        #region Methods
        String ToString(bool showPriority = false);
        #endregion
    }
}

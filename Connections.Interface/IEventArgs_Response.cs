using System;

namespace Connections.Interface
{
    public interface IEventArgs_Response
    {
        #region Accessors
        Boolean ValidSequence { get; }
        UInt64 SequenceNumber { get; }
        String DeviceId { get; }
        String Packet { get; }
        String Message { get; }
        #endregion /Accessors
    }
}

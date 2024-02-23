using Common.Utility;
using System;

namespace Connections.Interface
{
    public class ResponseEventArgs : IEventArgs_Response
    {
        #region Identity
        public const String ClassName = nameof(ResponseEventArgs);
        #endregion /Identity

        #region Globals
        public Boolean ValidSequence { get; private set; }
        public UInt64 SequenceNumber { get; private set; }
        public String DeviceId { get; private set; }
        public String Packet { get; private set; }
        public String Message { get; private set; }
        #endregion /Globals

        #region Constructor
        public ResponseEventArgs(String deviceId, String packet)
        {
            if (Utility_CiA309_3.TryExtractSequenceNum(packet, out ulong sequenceNum, out string message))
            {
                SequenceNumber = sequenceNum;
                Message = message;
                ValidSequence = true;
            }
            else
            {
                SequenceNumber = 0;
                Message = packet;
                ValidSequence = false;
            }
            DeviceId = deviceId;
            Packet = packet;
        }
        #endregion /Constructor
    }
}

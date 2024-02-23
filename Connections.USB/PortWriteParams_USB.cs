using Common.Utility;
using Connections.Interface;
using System;

namespace Connections.USB
{
    /// <summary>
    /// This struct is a container for the data being sent over the USB port.
    /// </summary>
    public struct PortWriteParams_USB : IPortWriteParams
    {
        #region Accessors
        public bool IsSequenced { get; private set; }
        public IMessageData Packet { get; set; }
        public readonly String Data => Packet.Data;
        private readonly String message;
        public readonly String Message => message;
        private readonly UInt64 sequenceNumber;
        public readonly UInt64 SequenceNumber => sequenceNumber;
        #endregion /Accessors

        #region Constructor
        public PortWriteParams_USB(IMessageData packet)
        {
            Packet = packet;
            IsSequenced = Utility_CiA309_3.TryExtractSequenceNum(Packet.Data, out sequenceNumber, out message);
        }
        #endregion /Constructor

        #region Static Methods
        public static PortWriteParams_USB Create(IMessageData packet)
        {
            return new PortWriteParams_USB(packet);
        }
        #endregion /Static Methods
    }
}

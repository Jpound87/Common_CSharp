using Connections.Interface;
using System;
using System.Threading;

namespace Connections.USB.Interface
{
    public interface IConnectionUSB : IConnection
    {
        #region Connection
        ISerialPort SerialPort { get; }
        Boolean IsOpen { get; }
        EventWaitHandle SendRequestEvent { get; }
        EventWaitHandle UpdateFirmwareEvent { get; }
        #endregion /Connection

        #region Packet
        Int32 BytesToRead { get; }
        #endregion /Packet

        #region Read & Write
        Int32 Read(Byte[] buffer, Int32 offset, Int32 count);
        void Write(IMessageData data);
        #endregion /Read & Write

        #region Close
        void Close();
        #endregion /Close
    }
}

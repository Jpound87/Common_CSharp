using Connections.Interface;
using RJCP.IO.Ports;
using System;

namespace Connections.USB.Interface
{
    public interface ISerialPort : IPort
    {
        #region Events
        event EventHandler<SerialDataReceivedEventArgs> DataReceived;
        #endregion

        #region Accessors
        int BytesToRead { get; }
        int ReadTimeout { get; set; }
        int WriteTimeout { get; set; }
        #endregion

        #region Methods
        void Open();
        void DiscardInBuffer();
        void DiscardOutBuffer();
        #endregion
    }
}

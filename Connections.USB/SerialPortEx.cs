using Connections.Interface;
using Connections.USB.Interface;
using RJCP.IO.Ports;
using System;

namespace Connections.USB
{
    public class SerialPortEx : SerialPortStream, ISerialPort
    {
        #region Identity
        public const string ClassName = nameof(SerialPortEx);
        public virtual string Name
        {
            get
            {
                return ClassName;
            }
        }
        #endregion

        #region Events
        public event EventHandler<IEventArgs_Request> RequestSent;
        #endregion

        #region Constructor
        // Summary:
        //     Initializes a new instance of the System.IO.Ports.SerialPort class using the
        //     specified port name.
        //
        // Parameters:
        //   portName:
        //     The port to use (for example, COM1).
        //
        // Exceptions:
        //   T:System.IO.IOException:
        //     The specified port could not be found or opened.
        public SerialPortEx(string portName) : base(portName)
        {
            WriteTimeout = 10000;
        }
        #endregion

        #region Read
        public int Read(IPortReadParams portReadParams)
        {
            if (portReadParams is PortReadParams_USB portReadParams_USB)
            {
                return base.Read(portReadParams_USB.Buffer, portReadParams_USB.Offset, portReadParams_USB.Count);
            }
            return -1;
        }
        #endregion

        #region Write
        // Summary:
        //     Writes the specified string and the System.IO.Ports.SerialPort.NewLine value
        //     to the output buffer.
        //
        // Parameters:
        //   text:
        //     The string to write to the output buffer.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The str parameter is null.
        //
        //   T:System.InvalidOperationException:
        //     The specified port is not open.
        //
        //   T:System.TimeoutException:
        //     The System.IO.Ports.SerialPort.WriteLine(System.String) method could not write
        //     to the stream.
        public void Write(IPortWriteParams portWriteParams)
        {
            if (portWriteParams is PortWriteParams_USB portWriteParams_USB)
            {
                if (portWriteParams_USB.Packet.Valid)
                {
                    base.Write(portWriteParams_USB.Packet.Data);
                    OnRequestSent(new EventArgs_Request(portWriteParams_USB.Packet));
                }
            }
        }

        protected void OnRequestSent(IEventArgs_Request requestEventArgs)
        {
            RequestSent?.Invoke(this, requestEventArgs);
        }
        #endregion /Write
    }
}

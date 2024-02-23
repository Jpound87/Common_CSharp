using Connection.Interface;
using Connection.USB.Interface;
using System;
using System.IO.Ports;

namespace Connection.USB
{
    public class SerialPortY : SerialPort, ISerialPort
    {
        #region Identity
        public const string ClassName = nameof(SerialPortY);
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
        new public event EventHandler<RJCP.IO.Ports.SerialDataReceivedEventArgs> DataReceived;
        #endregion

        #region Readonly
        private readonly SerialDataReceivedEventHandler serialDataReceived_Handler;
        #endregion

        #region Constructor
        public SerialPortY(string portName) : base(portName)
        {
            serialDataReceived_Handler = new SerialDataReceivedEventHandler(OnDataReceived);
            base.DataReceived += serialDataReceived_Handler;
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

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs serialDataReceivedEventArgs)
        {
            RJCP.IO.Ports.SerialDataReceivedEventArgs serialDataEventArgs = serialDataReceivedEventArgs.ConvertSerialDataReceivedEventArgs();
            DataReceived?.Invoke(this, serialDataEventArgs);
        }
        #endregion

        #region Write
        public void Write(IPortWriteParams portWriteParams)
        {
            if (portWriteParams is PortWriteParams_USB portWriteParams_USB)
            {
                base.Write(portWriteParams_USB.Packet.Data);
                OnRequestSent(new EventArgs_Request(portWriteParams_USB.Packet));
            }
        }

        protected void OnRequestSent(IEventArgs_Request requestEventArgs)
        {
            RequestSent?.Invoke(this, requestEventArgs);
        }
        #endregion
    }
}

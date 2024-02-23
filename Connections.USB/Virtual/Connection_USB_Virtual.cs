using Connections.Base;
using Connections.Interface;
using Connections.USB.Interface;
using Runtime;
using System;
using System.Diagnostics;
using System.Threading;

namespace Connections.USB
{
    public class Connection_USB_Virtual : Connection_Base, IConnectionUSB
    {
        #region Identity
        new public const String ClassName = nameof(Connection_USB_Virtual);
        public override String Identity
        {
            get
            {
                return ClassName;
            }
        }
        #endregion

        #region Connection
        public bool IsOpen
        {
            get
            {
                return true;
            }
        }
        public ISerialPort SerialPort
        {
            get
            {
                return Port as SerialPortEx;
            }
        }


        private readonly AutoResetEvent sendRequestEvent = new AutoResetEvent(false);
        public EventWaitHandle SendRequestEvent
        {
            get
            {
                return sendRequestEvent;
            }
        }

        private readonly AutoResetEvent updateFirmwareEvent = new AutoResetEvent(false);
        public EventWaitHandle UpdateFirmwareEvent
        {
            get
            {
                return updateFirmwareEvent;
            }
        }
        #endregion

        #region Packet
        public int BytesToRead
        {
            get
            {
                return SerialPort.BytesToRead;
            }
        }
        #endregion

        #region Constructor
        public Connection_USB_Virtual()
        {
            SerialPort_Virtual port = new SerialPort_Virtual();

            Port = port;
            Valid = true;

            WaitTimer = new Stopwatch();
            TokenSource = Runtime_Manager.GetLinkedTokenSource(this);
            EventWaitHandles = new EventWaitHandle[2] { SendRequestEvent, UpdateFirmwareEvent };// If this must change, dont forget to alter the constants!
            LastActiveTime = DateTime.Now;
            InactiveSpan = TimeSpan.Zero;
            port.DataReceived += UpdateActiveTime;
            port.RequestSent += UpdateActiveTime;
            InitializeWaitHandles();
            //Manager_Runtime.ScheduleLinkedTimerUpdateLoop(this, WatchForInactivity, inactivityTimeoutSpan);
        }
        #endregion

        #region Read
        public int Read(byte[] buffer, int offset, int count)
        {
            return Port.Read(PortReadParams_USB.Create(buffer, offset, count));
        }

        public override int Read(IPortReadParams portReadParams)
        {
            return Port.Read(portReadParams);
        }
        #endregion

        #region Write
        public void Write(IMessageData packet)
        {
            Port.Write(PortWriteParams_USB.Create(packet));
        }
        public override void Write(IPortWriteParams portWriteParams)
        {
            Port.Write(portWriteParams);
        }
        #endregion
    }
}

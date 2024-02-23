using Common;
using Common.Utility;
using Connections.Base;
using Connections.Interface;
using Connections.USB.Interface;
using Runtime;
using System;
using System.Diagnostics;
using System.Threading;

namespace Connections.USB
{
    /// <summary>
    /// This class is an access wrapper for the serial port connection given on construction.
    /// It also contains maintainace and disposal methods. 
    /// </summary>
    public class Connection_USB : Connection_Base, IConnectionUSB
    {
        #region Identity
        new public const String ClassName = nameof(Connection_USB);
        public override String Identity
        {
            get
            {
                return ClassName;
            }
        }
        #endregion /Identity

        #region Constants
        public const Int32 SEND_EVENT_INDEX = 0;
        public const Int32 FIRMWARE_EVENT_INDEX = 1;
        #endregion /Constants

        #region Connection
        public Boolean IsOpen
        {
            get
            {
                return Port.IsOpen;
            }
        }

        public ISerialPort SerialPort
        {
            get
            {
                return Port as ISerialPort;
            }
        }

        private readonly AutoResetEvent sendRequestEvent = Utility_ResetEvent.Create_AutoResetEvent();
        public EventWaitHandle SendRequestEvent
        {
            get
            {
                return sendRequestEvent;
            }
        }
        private readonly AutoResetEvent updateFirmwareEvent = Utility_ResetEvent.Create_AutoResetEvent();
        public EventWaitHandle UpdateFirmwareEvent
        {
            get
            {
                return updateFirmwareEvent;
            }
        }
        #endregion /Connection

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
        public Connection_USB(ISerialPort port, CancellationTokenSource tokenSource, bool valid)
        {
            Port = port;
            Valid = valid;
            if (valid)
            {
                WaitTimer = new Stopwatch();
                TokenSource = tokenSource;
                EventWaitHandles = new EventWaitHandle[1] { SendRequestEvent };//, UpdateFirmwareEvent };// If this must change, dont forget to alter the constants!
                LastActiveTime = DateTime.Now;
                InactiveSpan = TimeSpan.Zero;
                port.DataReceived += UpdateActiveTime;
                port.RequestSent += UpdateActiveTime;
                InitializeWaitHandles();
                //Runtime_Manager.ScheduleLinkedTimerUpdateLoop(this, WatchForInactivity, inactivityTimeoutSpan); //TODO: find better way to look for user apathy!
            }
        }

        ~Connection_USB()
        {
            Close();// Port is serialport without decorator.
            if (SerialPort != null)
            {
                SerialPort.DataReceived -= UpdateActiveTime;
                SerialPort.RequestSent -= UpdateActiveTime;
            }
            Port?.Dispose();
        }
        #endregion /Constructor

        #region Read
        public Int32 Read(Byte[] buffer, Int32 offset, Int32 count)
        {
            return Port.Read(PortReadParams_USB.Create(buffer, offset, count));
        }

        public override Int32 Read(IPortReadParams portReadParams)
        {
            return Port.Read(portReadParams);
        }
        #endregion /Read

        #region Write
        public void Write(IMessageData data)
        {
            Port.Write(PortWriteParams_USB.Create(data));
        }
        public override void Write(IPortWriteParams portWriteParams)
        {
            Port.Write(portWriteParams);
        }
        #endregion /Write

        #region On Close
        public override void Close()
        {
            base.Close();
            try
            {
                if (Port != null)
                {
                    SerialPort.DataReceived -= UpdateActiveTime;
                    SerialPort.RequestSent -= UpdateActiveTime;
                    if (IsOpen == true)
                    {
                        DiscardInBuffer();
                        DiscardOutBuffer();
                        Port.Close();
                    }
                }
            }
            catch (ObjectDisposedException ode)
            {
                Log_Manager.LogInfo(ClassName, ode.Message);
            }
        }

        public void DiscardInBuffer()
        {
            SerialPort.DiscardInBuffer();
        }
        public void DiscardOutBuffer()
        {
            SerialPort.DiscardOutBuffer();
        }
        #endregion /On Close

        #region Dispose
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                if (disposing)
                {
                    Close();// Port is serialport without decorator.
                    Port?.Dispose();
                    Runtime_Manager.Cancel(this);
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
        #endregion /Dispose
    }
}

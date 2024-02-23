using Common;
using Common.Base;
using Common.Constant;
using Common.Extensions;
using Connections.Interface;
using Connections.USB.Interface;
using Firmware.Interface.EventArgs;
using Firmware.Scheduler;
using RJCP.IO.Ports;
using Runtime;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;

namespace Connections.USB
{
    /// <summary>
    /// This class is responsible for communication between this ('Datam')
    /// and the communicator dongle
    /// </summary>
    public class SerialCommunication_USB : Dispose_Base, IConnection_Communicator
    {
        #region Identity
        new public const String ClassName = nameof(SerialCommunication_USB);
        #endregion /Identity

        #region Time Constants
        private readonly TimeSpan connectionPortOpenSpan = TimeSpan.FromSeconds(7);
        private readonly TimeSpan connectionPortCloseSpan = TimeSpan.FromSeconds(3);
        #endregion /Time Constants

        #region Static Readonly
        private static readonly ConcurrentDictionary<String, IConnectionUSB> dictComm_ConnectionRegistration = new ConcurrentDictionary<String, IConnectionUSB>();
        #endregion /Static Readonly

        #region Readonly
        private readonly BackgroundWorker communicationWorker = new()
        {
            WorkerSupportsCancellation = true
        };
        private readonly NotifyQueue_Concurrent<IMessageData> requestQueue = new();
        #endregion /Readonly

        #region Events
        public event EventHandler<IFirmwareStatusUpdateEventArgs> FirmwareStatusUpdating;
        #endregion /Events

        #region Delegates
        private readonly DoWorkEventHandler communicationWorker_Handler;
        #endregion /Delegates

        #region Accessors

        #region Connection
        public String ComPort { get; private set; }
        public IConnectionUSB Connection { get; private set; }
        private Action connectionAction_Close;
        #endregion /Connection

        #endregion /Accessors

        #region Cancellation 
        private CancellationTokenSource connectionTokenSource;
        #endregion /Cancellation

        #region Globals
        private Int32 sendHandleIndex;
        #endregion /Globals

        #region Events 
        public event EventHandler<IEventArgs_Request> PacketSent;
        public event EventHandler<IEventArgs_Response> PacketReceived;

        public event EventHandler<Exception> ExceptionThrown;
        #endregion /Events
        
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public SerialCommunication_USB()
        {
            communicationWorker_Handler = Transmission_Run;
        }

        ~SerialCommunication_USB()
        {
            communicationWorker.CancelAsync();
            Connection?.Close();
            Connection?.Dispose();
        }
        #endregion /Constructor

        #region Write
        protected void OnRequestSent(IEventArgs_Request requestEventArgs)
        {
            PacketSent?.Invoke(this, requestEventArgs);
        }
        #endregion /Write

        #region Read
        /// <summary>
        /// This method triggers the ResponseReceived event when a response is received
        /// and sends the given evnet arguments
        /// </summary>
        /// <param name="rea">The arguments to be sent with the event</param>
        protected void OnResponseReceived(IEventArgs_Response rea)
        {
            Log_Manager.LogVerbose(ClassName, $"Response Received on {rea.DeviceId}: {rea.Packet}.");
            PacketReceived?.Invoke(this, rea);
        }
        #endregion /Read

        #region Exception Handling
        protected void OnExceptionThrown(Exception exception)
        {
            ExceptionThrown?.Invoke(this, exception);
        }
        #endregion /Exception Handling

        #region Connection 

        #region Creation
        /// <summary>
        /// This method will attempt to make a connection asynconously and then return if
        /// this attempt was succesful.
        /// </summary>
        /// <param name="comPort"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Boolean AwaitConnection(String comPort, CancellationToken cancellationToken)
        {
            Log_Manager.LogVerbose(ClassName, $"AwaitConnection_Async called for com port {comPort}");
            ComPort = comPort;
            try
            {
                Log_Manager.LogDebug(ClassName, $"SerialConnect invoked for {comPort}");
                if (TrySerialConnect())
                {
                    Log_Manager.LogDebug(ClassName, $"SerialConnect finished running for {comPort}");
                    if (dictComm_ConnectionRegistration.TryLookup(comPort, out IConnectionUSB connection) && connection.IsOpen)
                    {// Success!
                        Log_Manager.LogAssert(ClassName, $"{comPort} connection successful");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log_Manager.LogAssert(ClassName, ex.Message);
                Log_Manager.IssueMessage($"Failed to add communication device on com port {comPort}", "USB Communication");
            }
            Dispose();
            return false;
        }

        /// <summary>
        /// This method connects 'Datam' to the communicator
        /// </summary>
        private Boolean TrySerialConnect()
        {
            Log_Manager.LogAssert(ClassName, $"SerialConnect Called on {ComPort}");
            bool makeConnection = true;
            lock (dictComm_ConnectionRegistration)
            {
                if (dictComm_ConnectionRegistration.TryLookup(ComPort, out IConnectionUSB lastConnection))
                {
                    if (lastConnection.IsOpen)
                    {
                        Connection = lastConnection;
                        makeConnection = false;
                    }
                    else
                    { 
                        lastConnection.Close();// Just to be sure.
                        dictComm_ConnectionRegistration.TryRemove(ComPort);
                    }
                }
                if(makeConnection)
                {
                    Connection = CreateConnection();
                    RegisterDisposables(Connection);
                    if (!dictComm_ConnectionRegistration.TryAddOrUpdate(ComPort, Connection))
                    {
                        Connection.Close();// If we cant track it dont keep it.
                        Connection.Dispose();
                    }
                }
            }
            if (Connection.Valid)
            {
                sendHandleIndex = Connection.LookupHandleIndex(Connection.SendRequestEvent);// (1)These should be as true as a tuatology! If not, major problems have been encountered.
                if (sendHandleIndex == Connection_USB.SEND_EVENT_INDEX)// && firmwareHandleIndex == Connection_USB.FIRMWARE_EVENT_INDEX)
                {
                    communicationWorker.DoWork += communicationWorker_Handler;
                   
                    communicationWorker.RunWorkerAsync();
                }
                else
                {
                    ConnectAction_Close(Connection.SerialPort);
                    throw new Exception("Wait handles not found for communicator!");
                }
            }
            else
            {
                dictComm_ConnectionRegistration.TryRemove(ComPort);
            }
            return Connection.Valid;
        }
        
        private IConnectionUSB CreateConnection()
        {
            ISerialPort portConnection = new SerialPortEx(ComPort)
            {
                BaudRate = 115200,
                DataBits = 8,
                StopBits = StopBits.One,
                Parity = Parity.None,
                Handshake = Handshake.None,
                ReadTimeout = 500,
                WriteTimeout = 500
            };

            RegisterDisposables(portConnection);
  
            Action connectionAction_Open = new Action(() => { ConnectAction_Open(portConnection); });
            connectionAction_Close = new Action(() => { ConnectAction_Close(portConnection); });

            CancellationTokenSource cancelOnSuccess = Runtime_Manager.ScheduleCancelableAction(this, connectionPortCloseSpan, connectionAction_Close);
            bool valid = Runtime_Manager.ScheduleActionTimeout(this, connectionPortOpenSpan, connectionAction_Open) && portConnection.IsOpen;
            if (valid)
            {
                cancelOnSuccess.Cancel();// This will close the connection if it fails to timeout.
                portConnection.ReadTimeout = 10000;
                portConnection.WriteTimeout = 10000;
                portConnection.RequestSent += SerialPort_RequestSent;
                portConnection.DataReceived += SerialPort_DataReceived;
            }
            else
            {
                ConnectAction_Close(portConnection);
                portConnection = null;
            }
            connectionTokenSource = Runtime_Manager.GetLinkedTokenSource(this);
            return new Connection_USB(portConnection, connectionTokenSource, valid);
        }
        #endregion /Creation

        #region Open
        private void ConnectAction_Open(ISerialPort portConnection)
        {
            try
            {
                portConnection?.Open();
            }
            catch (UnauthorizedAccessException)
            {
                Log_Manager.LogError(ClassName, $"Error: Port {ComPort} is in use");
            }
            catch (Exception ex)
            {
                Log_Manager.LogError(ClassName, $"Com Port Close Exception: {ex.Message}");
                ConnectAction_Close(portConnection);
            }
        }
        #endregion /Open

        #region Close
        private void ConnectAction_Close(ISerialPort portConnection)
        {
            try
            {
                if (portConnection != null)
                {
                    portConnection.RequestSent -= SerialPort_RequestSent;
                    portConnection.DataReceived -= SerialPort_DataReceived;
                    if (portConnection.IsOpen)
                    {
                        portConnection.DiscardOutBuffer();
                        portConnection.DiscardOutBuffer();
                        portConnection.Close();
                        portConnection.Dispose();
                    }
                }
            }
            catch { }
        }
        #endregion /Close

        #endregion /Connection 

        #region Transmission 

        #region Update Firmware
        public void UpdateFirmware(Int32 net, UInt32 nodeId, String firmwareFilePath, TimeSpan delayBeforeHidingStatus_ms)
        {
            Scheduler_FirmwareUpdate scheduler_FirmwareUpdate = new Scheduler_FirmwareUpdate();
            scheduler_FirmwareUpdate.UpdateFirmware(net, nodeId, firmwareFilePath, delayBeforeHidingStatus_ms);
        }
        #endregion /Update Firmware

        #region Worker
        /// <summary>
        /// This method begins the communication thread, which is responsible
        /// for intercepting and sending of messages
        /// </summary>
        private void Transmission_Run(Object _, DoWorkEventArgs dwea)
        {
            Log_Manager.LogAssert(ClassName, $"CommunicationThread_Run started on {ComPort}");
            while (!communicationWorker.CancellationPending)
            {
                try
                {
                    int result = Connection.WaitForExitOrEventWaits();
                    switch (result)
                    {
                        case Connection_USB.SEND_EVENT_INDEX:
                            {
                                while (requestQueue.TryDequeue(out IMessageData packet))
                                {
                                    Connection.Write(packet);
                                }
                            }
                            break;
                        //case Connection_USB.FIRMWARE_EVENT_INDEX:
                        //    {

                        //    }
                        //    break;
                        default:
                            break;
                    }
                }
                catch (InvalidOperationException ex)
                {// Prolly communicator or drive became discconected from USB
                    string strWarn = String.Format("Exception: {0} occured", ex.Message);
                    Log_Manager.LogWarning(ClassName, strWarn);
                    OnExceptionThrown(ex);
                }
                catch (IOException ioex)
                {// Prolly communicator or drive became discconected from USB
                    string strWarn = String.Format("Exception: {0}", ioex.Message);
                    Log_Manager.LogWarning(ClassName, strWarn);
                    OnExceptionThrown(ioex);
                }
                catch (Exception ex)
                {
                    Log_Manager.IssueAlert(ex);
                    OnExceptionThrown(ex);
                }
            }
        }
        #endregion /Worker

        #endregion /Transmission 

        #region Serial Port

        #region Request
        private void SerialPort_RequestSent(Object _, IEventArgs_Request e)
        {
            OnRequestSent(e);
        }

        /// <summary>
        /// This method will add the reqest to the transmit queue
        /// </summary>
        /// <param name="packet"></param>
        public void SendRequest(IMessageData packet)
        {
            Log_Manager.LogVerbose(ClassName, $"Send Request on {ComPort}: {packet}.");
            lock (Connection)
            {
                if (requestQueue.TryEnqueue(packet))
                {
                    Connection.SendRequestEvent.Set();
                }
                else
                { 
                    Log_Manager.LogAssert(ClassName, $"Send request '{packet}' failed to enqueue!");
                }
            }
        }
        #endregion /Request

        #region Recieved
        /// <summary>
        /// This method is responsible for decoding and assembling data received
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialPort_DataReceived(Object sender, SerialDataReceivedEventArgs e)
        {
            Log_Manager.LogMethodCall(ClassName, nameof(SerialPort_DataReceived));
            int startIndex = 0;
            string responseText = String.Empty;
            try
            {
                int bytesToRead = Connection.BytesToRead;
                byte[] receivedBuffer = new byte[bytesToRead];
                string[] recievedStrings = new string[bytesToRead];
                Connection.Read(receivedBuffer, 0, bytesToRead);
                for (int location_EOL = 0; location_EOL < bytesToRead; location_EOL++)
                {
                    byte atByte = receivedBuffer[location_EOL];
                    if (atByte == Tokens.CARRAGE_RETURN_BYTE)
                    {
                        int sizeOfText = location_EOL - (startIndex + 1);
                        if (sizeOfText >= 0)
                        {
                            responseText = Encoding.UTF8.GetString(receivedBuffer, startIndex, sizeOfText);
                            if (responseText.Length > 0)
                            {
                                //if (updateFirmwareCommander != null)
                                //{
                                //    updateFirmwareCommander.ProcessResponse(responseText);
                                //}
                                OnResponseReceived(new ResponseEventArgs(ComPort, responseText));
                            }
                        }
                    }
                    else if (atByte == Tokens.NEW_LINE_BYTE || atByte == Tokens.TERMINAL_CHAR_BYTE)
                    {// We don't want these.
                        startIndex = location_EOL+1;// Start at the next char.
                    }
                }
            }
            catch (Exception ex)
            {
                Log_Manager.IssueAlert(ex);
                OnExceptionThrown(ex);
            }
        }
        #endregion /Recieved

        #endregion /Serial Port

        #region Disconnect
        /// <summary>
        /// This method disconnects the communicator from Datam
        /// </summary>
        public void Disconnect()
        {
            Log_Manager.LogAssert(ClassName, $"Disconnect Called on {ComPort}");
            dictComm_ConnectionRegistration.TryRemove(ComPort);
            communicationWorker.CancelAsync();
            Connection?.Close();//TODO: why thy null sometimes?
        }
        #endregion /Disconnect

        #region Dispose
        public override void Dispose()
        {
            try
            {
                connectionAction_Close?.Invoke();
                Runtime_Manager.Cancel(this);
            }
            finally
            {
                base.Dispose();
                GC.SuppressFinalize(this);
            }
        }
        #endregion /Dispose
    }
}

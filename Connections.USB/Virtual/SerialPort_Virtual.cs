using Common.Extensions;
using Connections.Interface;
using Connections.USB.Interface;
using RJCP.IO.Ports;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connections.USB
{
    public class SerialPort_Virtual : ISerialPort, IDisposable
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

        #region Constants

        #region Communicator ID
        private const String COMMAND_IDENTIFY_COMMUNICATOR = "0 0 info version";
        private const String COMMAND_IDENTIFY_COMMUNICATOR_RESPONSE = "1005 2091 1.00 1 3 02.00 02.01";
        #endregion

        #region Device Search
        private const String COMMAND_INITIALIZE_SEARCH = "1 init 0";
        private const String COMMAND_INITIALIZE_SEARCH_RESPONSE = "OK";
        private const String COMMAND_LIST = "1 1 _list 2 3";// Range is ignored
        private const String COMMAND_LIST_STATUS_RESPONSE = "USER _LISTSTATUS 0x0002 0x0002 0x0003";
        private const String COMMAND_LIST_FOUND_RESPONSE ="[1] 0x02 \"HTB\" 0x0000082a 0x00000006 0x0000001a";
        #endregion

        #region Device Information
        private const String COMMAND_SUPPORTED_DRIVE_MODES = "[2] 1 2 r 0x6502 0x00 u32";
        private const String COMMAND_SUPPORTED_DRIVE_MODES_RESPONSE = "[2] 1 2 r 0x6502 0x00 u32";
        #endregion

        private static readonly Dictionary<String, String[]> dictStandardResponses = new Dictionary<string, string[]>
        {
            { COMMAND_IDENTIFY_COMMUNICATOR, new string[]{ COMMAND_IDENTIFY_COMMUNICATOR_RESPONSE } },
            { COMMAND_INITIALIZE_SEARCH, new string[]{COMMAND_INITIALIZE_SEARCH_RESPONSE } },

        };
        
        #endregion

        #region Readonly
        private readonly ConcurrentDictionary<uint, String> dictSequenceNumber_Response = new ConcurrentDictionary<uint, String>();
        private readonly Queue<byte[]> response_Q = new Queue<byte[]>();
        #endregion

        #region Events
        public event EventHandler<IEventArgs_Request> RequestSent;
        public event EventHandler<SerialDataReceivedEventArgs> DataReceived;
        #endregion

        #region Connection
        public bool IsOpen => true;

        public int ReadTimeout { get; set; }
        public int WriteTimeout { get; set; }
        #endregion

        #region Constructor
        public SerialPort_Virtual()
        {

        }
        #endregion

        #region Open
        public void Open()
        {

        }
        #endregion

        #region Read
        public int BytesToRead
        {
            get
            {
                if (response_Q.Count > 0)
                {
                    return response_Q.Peek().Length;
                }
                return 0;
            }
        }

        public int Read(IPortReadParams portReadParams)
        {
            lock (response_Q)
            {
                if (portReadParams is PortReadParams_USB portReadParams_USB)
                {
                    portReadParams_USB.Buffer = response_Q.Dequeue();
                    if (response_Q.Any())
                    {
                        DataReceived?.Invoke(this, new SerialDataReceivedEventArgs(SerialData.Chars));
                    }
                    //return base.Read(portReadParams_USB.Buffer, portReadParams_USB.Offset, portReadParams_USB.Count);
                }
                return -1;
            }
        }

        #endregion

        #region Write
       
        public void Write(IPortWriteParams portWriteParams)
        {
#pragma warning disable IDE0018 // Inline variable declaration
            string[] responses;
#pragma warning restore IDE0018 // Inline variable declaration
            lock(response_Q)
            {
                if (portWriteParams is PortWriteParams_USB portWriteParams_USB)
                {// We need to discern what type of message this is to spoof a response.
                 // Determine if this is sequenced.
                    
                    // Determine if this is a standard communicator command.
                    if (!dictStandardResponses.TryLookup(portWriteParams_USB.Message, out responses))
                    {// If it's not directly in the lookup it could still be a type of standard command.
                        foreach(KeyValuePair<string, string[]> commandToken_Response in dictStandardResponses)
                        {
                            if(portWriteParams_USB.Message.Contains(commandToken_Response.Key))
                            {
                                responses = commandToken_Response.Value;
                            }
                        }
                    }
                    // First, get the sequence number.
                    foreach (string response in responses)
                    {
                        if (portWriteParams_USB.IsSequenced)
                        {
                            response_Q.Enqueue(Encoding.UTF8.GetBytes($"[{portWriteParams_USB.SequenceNumber}] {response}"));
                        }
                        else
                        {

                        }
                    }
                    OnRequestSent(new EventArgs_Request(portWriteParams_USB.Packet));
                    if (response_Q.Any())
                    {
                        DataReceived?.Invoke(this, new SerialDataReceivedEventArgs(SerialData.Chars));
                    }
                }
            }
        }

        protected void OnRequestSent(IEventArgs_Request requestEventArgs)
        {
            RequestSent?.Invoke(this, requestEventArgs);
        }
        #endregion

        #region Buffer
        public void DiscardInBuffer()
        {// Why did someone call this? Don't.
            throw new NotImplementedException();
        }
        public void DiscardOutBuffer()
        {// Why did someone call this? Don't.
            throw new NotImplementedException();
        }
        #endregion

        #region Close
        public void Close()
        {
            Dispose();
        }

        #endregion

        #region Dispose
        public void Dispose()
        {
           
        }
        #endregion
    }
}

using Common.Constant;
using Common.Extensions;
using Connections.Interface;
using System;

namespace Connections.Base
{
    public struct MessageData_Base : IMessageData
    {
        #region Identity
        public const String StructName = nameof(MessageData_Base);
        #endregion /Identity

        #region Message
        public Priority_Packet Priority { get; set; }
        public Boolean Valid { get; private set; }
        private MessageType messageType;
        public MessageType Type
        {
            get
            {
                return messageType;
            }
            set
            {
                messageType = value;
                Valid = messageType != MessageType.Invalid;
            }
        }
        public String Data { get; private set; }
        private readonly Int32 packetHash;
        public Int32 CommandHash
        {
            get
            {
                return packetHash;
            }
        }
        public Int32 Net { get; private set; }
        public UInt32 NodeID { get; private set; }
        public Boolean UseNodeID { get; private set; }
        #endregion /Message

        #region Constructor
        public MessageData_Base(Priority_Packet priority, String data, Int32 net, UInt32 nodeID = UInt32.MaxValue, Action eventAction = null)
        {
            Valid = true;// This base message is valid, but the use of this is not, hence the following line.
            switch(priority)
            {
                case Priority_Packet.Communicator:
                    messageType = MessageType.Command;
                    break;
                case Priority_Packet.Firmware:
                case Priority_Packet.Write:
                    messageType = MessageType.Parameter_Write;
                    break;
                case Priority_Packet.Immediate:
                case Priority_Packet.High:
                case Priority_Packet.Low:
                    messageType = MessageType.Parameter_Read;
                    break;
                default:
                    messageType = MessageType.Invalid;
                    break;
            }
            Priority = priority;
            UseNodeID = nodeID < uint.MaxValue;
            if (UseNodeID)
            {
                Data = $"{net} {nodeID} {data}\r\n";
            }
            else
            {
                Data = $"{net} {data}\r\n";
            }
            packetHash = Data.GetHashCode();
            EventAction = eventAction;
            HasReaction = EventAction != null;
            Net = net;
            NodeID = nodeID;
        }
        #endregion /Constructor

        #region Result Action
        public Boolean HasReaction { get; private set; }
        public Action EventAction { get; private set; }
        public readonly void OnResultReceived()
        {
            if (HasReaction)
            {
                EventAction?.Invoke();
            }
        }
        #endregion /Result Action

        #region Equality
        public readonly Boolean Equals(IMessageData md)// Doogie Howser MD.
        {
            return Equals(this, md);
        }

        public static Boolean Equals(IMessageData md1, IMessageData md2)
        {
            return md1.CommandHash == md2.CommandHash && md1.Data == md2.Data;// If the hash is the same it will fail, if not then we make sure.
        }
        #endregion /Equality
    }
}

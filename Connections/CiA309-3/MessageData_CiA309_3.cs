using Common.Constant;
using Common.Extensions;
using Connections.Base;
using Connections.Interface;
using Connections.Interface.CiA309_3;
using Parameters;
using Parameters.Interface;
using System;

namespace Connections.CiA309_3
{
    public struct MessageData_CiA309_3 : IMessageData_CiA309_3
    {
        #region Identity
        public const String ClassName = nameof(MessageData_CiA309_3);
        #endregion /Identity

        #region Readonly
        public IMessageData MessageData { get; private set; }
        #endregion /Readonly

        #region Message
        public Boolean Valid { get; private set; }

        private Boolean hasSequenceNumber;
        public UInt64 SequenceNumber { get; private set; }

        public readonly MessageType Type => MessageData.Type;
        private String transmissionString;
        public readonly String Data
        {
            get
            {
                return transmissionString;
            }
        }
        public readonly Priority_Packet Priority
        {
            get
            {
                return MessageData.Priority;
            }
            set
            {
                MessageData.Priority = value;
            }
        }
        public readonly Int32 CommandHash => MessageData.CommandHash;
        public readonly Int32 Net => MessageData.Net;
        public readonly UInt32 NodeID => MessageData.NodeID;
        public readonly Boolean UseNodeID => MessageData.UseNodeID;
        #endregion /Message

        #region Parameter
        public IParameter Parameter { get; private set; }
        public readonly IAddress Address => Parameter.Address;
        #endregion /Parameter

        #region Constructor
        public MessageData_CiA309_3(Priority_Packet priority, IParameter parameter, Int32 net, UInt32 nodeID = Tokens.INVALID_NODE_ID, Action eventAction = null)
        {
            hasSequenceNumber = false;
            SequenceNumber = Tokens.INVALID_SEQUENCE_NUMBER;// Invalid token
            Parameter = parameter;
            Valid = (parameter is Parameter_CiA402 parameter_CiA402 || parameter.GetType().IsAssignableFrom(typeof(Parameter_CiA402)));
            if (Valid)
            {
                if (Valid)
                {
                    Valid = parameter != Parameter_CiA402.Null;
                    if (Valid)
                    {
                        string message;
                        MessageType type;
                        switch (priority)
                        {
                            case Priority_Packet.Write:
                                type = MessageType.Parameter_Write;
                                Valid = parameter.TryGetMessage_Write(out message);
                                break;
                            default:
                                type = MessageType.Parameter_Read;
                                Valid = parameter.TryGetMessage_Read(out message);
                                break;
                        }
                        if (Valid)
                        {// Look close! Different valid.
                            lock (parameter)
                            {// We don't want to be working on the same parameter in parallel.
                                MessageData = new MessageData_Base(priority, message, net, nodeID, eventAction)
                                {
                                    Type = type
                                };
                                transmissionString = MessageData.Data;
                                return;
                            }
                        }
                    }
                }
            }
            MessageData = new MessageData_Base();
            transmissionString = MessageData.Data;
        }
        #endregion /Constructor

        #region Result Action
        public readonly Boolean HasReaction => MessageData.HasReaction;
        public readonly Action EventAction => MessageData.EventAction;
        public readonly void OnResultReceived() => MessageData.OnResultReceived();
        #endregion /Result Action

        #region Sequence Number
        public void SetSequenceNumber(UInt64 sequenceNumber)
        {
            if (!hasSequenceNumber && sequenceNumber != Tokens.INVALID_SEQUENCE_NUMBER)
            {
                SequenceNumber = sequenceNumber;
                transmissionString = $"[{sequenceNumber}] {MessageData.Data}";
                hasSequenceNumber = true;
            }
        }
        #endregion /Sequence Number

        #region Equality
        public readonly Boolean Equals(IMessageData md)// Doogie Howser MD.
        {
            return MessageData_Base.Equals(this, md);
        }

        public readonly Boolean Equals(IMessageData_CiA309_3 md)// Doogie Howser MD.
        {
            return Equals(this, md);
        }

        public static Boolean Equals(IMessageData_CiA309_3 md1, IMessageData_CiA309_3 md2)
        {
            return md1.CommandHash == md2.CommandHash && md1.Data == md2.Data;// If the hash is the same it will fail, if not then we make sure.
        }
        #endregion /Equality
    }
}

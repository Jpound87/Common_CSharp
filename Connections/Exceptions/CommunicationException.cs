using Common.Constant;
using Connections.Interface.Exceptions;
using System;

namespace Connections.Exceptions
{
    [Serializable]
    public class CommunicationException : Exception, ICommunicationException
    {
        #region Accessors
        public Priority_Packet Priority { get; private set; }
        #endregion

        #region Constructor
        public CommunicationException(string message, Priority_Packet priority) : base(message)
        {
            Priority = priority;
        }
        #endregion /Constructor
    }
}

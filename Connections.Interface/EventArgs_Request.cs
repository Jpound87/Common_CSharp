using System;

namespace Connections.Interface
{
    public struct EventArgs_Request : IEventArgs_Request
    {
        #region Accessors
        public bool Valid { get; private set; }
        public IMessageData RequestPacket { get; private set; }
        #endregion

        #region Constructor
        public EventArgs_Request(IMessageData requestPacket)
        {
            RequestPacket = requestPacket;
            Valid = true;
        }
        #endregion

        #region Methods
        public String ToString(bool showPriority = false)
        {
            switch (showPriority)
            {
                case true:
                    return $"{RequestPacket.Priority} - {RequestPacket.Data}"; 
                default:
                    return $"{RequestPacket.Data}";
            }
        }

        public override String ToString()
        {
            return $"{RequestPacket.Data}";
        }
        #endregion /Methods
    }
}

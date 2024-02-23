using Common.Constant;
using System;

namespace Connections.Interface.Exceptions
{
    public interface ICommunicationException
    {
        #region Accessors
        String Message { get; }
        Priority_Packet Priority { get; }
        #endregion /Accessors
    }
}

namespace Connections.Interface.CiA309_3
{
    public interface IEventArgs_Request_CiA309_3 : IEventArgs_Request
    {
        #region Accessors
        new IMessageData_CiA309_3 RequestPacket { get; }
        #endregion /Accessors
    }
}

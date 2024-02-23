using RJCP.IO.Ports;

namespace Connections.USB.Interface
{
    public interface ISerialDataReceivedEventArgs
    {
        #region Accessors
        SerialData EventType { get; }
        #endregion
    }
}

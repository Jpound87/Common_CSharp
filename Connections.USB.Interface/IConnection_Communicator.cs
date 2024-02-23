using Firmware.Interface.EventArgs;
using System;
using System.Threading;

namespace Connections.Interface
{
    public interface IConnection_Communicator : IDisposable
    {
        #region Events
        event EventHandler<IEventArgs_Request> PacketSent;
        event EventHandler<IEventArgs_Response> PacketReceived;
        event EventHandler<IFirmwareStatusUpdateEventArgs> FirmwareStatusUpdating;
        event EventHandler<Exception> ExceptionThrown;
        #endregion /Events

        #region Connection Controls
        Boolean AwaitConnection(String comPort, CancellationToken cancellationToken);

        void Disconnect();

        void SendRequest(IMessageData request);

        void UpdateFirmware(Int32 net, UInt32 nodeId, String firmwareFilePath, TimeSpan delayBeforeHidingStatusInMs);
        #endregion /Connection Controls
    }
}

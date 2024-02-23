using Connections.USB.Interface;

namespace Connections.USB
{
    public class EventArgs_Device_USB : IDeviceEventArgs_USB
    {
        #region Accessors
        public IPortData_USB PortData_USB { get; private set; }
        #endregion

        #region Constructor

        public EventArgs_Device_USB(IPortData_USB portData_USB)
        {
            PortData_USB = portData_USB;
        }
        #endregion
    }
}

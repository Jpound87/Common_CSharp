using System;

namespace Connections.USB.Interface
{
    public interface IPortData_USB
    {
        #region Accessors
        bool Valid { get; }
        String ComPort { get; }
        String DeviceID { get; }
        String Service { get; }
        #endregion
    }
}

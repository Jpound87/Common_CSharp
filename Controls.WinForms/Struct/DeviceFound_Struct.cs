using Datasheets.Interface;
using Devices.Interface.EventArgs;

namespace Datam.WinForms.Struct
{
    internal struct DeviceFound_Struct
    {
        #region Accessors
        public IDeviceFoundEventArgs DeviceFoundEventArgs { get; set; }
        public IDatasheet Datasheet { get; set; }
        #endregion /Accessors

        #region Constructor
        public DeviceFound_Struct(IDeviceFoundEventArgs dfea, IDatasheet datasheet)
        {
            DeviceFoundEventArgs = dfea;
            Datasheet = datasheet;
        }
        #endregion /Constructor
    }
}

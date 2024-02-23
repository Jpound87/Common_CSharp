using System;

namespace Datam.WinForms.Interface
{
    public interface IDeviceData : IEquatable<IDeviceData>
    {
        #region Accessors
        String DeviceName { get; }
        String ManufacturerName { get; }
        #endregion
    }
}

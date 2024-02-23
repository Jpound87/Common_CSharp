using Devices.Interface;
using Devices.Interface.CiA402;

namespace Datam.WinForms.Interface
{
    public interface IDockForm_Device_CiA402 : IDockForm_Device
    {
        #region Accessors
        ICommunicatorDevice_CiA402 Device_CiA402 { get; }
        #endregion /Accessors
    }
}

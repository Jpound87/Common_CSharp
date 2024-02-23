using Devices.Interface;

namespace Datam.WinForms.Interface
{
    public interface IDeviceTreeNodeData_CiA402 : IDeviceTreeNodeData
    {
        #region Accessors
        IInformation_Communicator CommunicatorData { get; }
        IInformation_Device DeviceData_CiA402 { get; }
        ICommunicatorDevice Device_CiA402 { get; }
        #endregion /Accessors
    }
}

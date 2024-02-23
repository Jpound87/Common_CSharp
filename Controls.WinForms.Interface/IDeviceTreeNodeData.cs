using Devices.Interface;
using UI.Interface;

namespace Datam.WinForms.Interface
{
    public interface IDeviceTreeNodeData : ITreeNodeData, IDeviceData
    {
        #region Accessors
        IInformation_Device DeviceData { get; }

        ICommunicatorDevice Device { get; }

        string ID { get; }
        #endregion /Accessors
    }
}

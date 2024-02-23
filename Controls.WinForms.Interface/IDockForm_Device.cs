using Devices.Interface;

namespace Datam.WinForms.Interface
{
    public interface IDockForm_Device : IDockForm, IAddressable
    {
        #region Accessors
        ICommunicatorDevice Device { get; }
        bool DeviceEnabled { get; }
        bool ReadyForCommunication { get; }
        #endregion /Accessors
    }
}

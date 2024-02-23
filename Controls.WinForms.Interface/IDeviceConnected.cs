using Devices.Interface;

namespace Datam.WinForms.Interface
{
    /// <summary>
    /// This interface is for classes that are designed to use data from a communication device.
    /// </summary>
    public interface IDeviceConnected
    {
        #region Accessors
        /// <summary>
        /// This should contain the device the window is referencing, and which it 
        /// communicates with for data. 
        /// </summary>
        ICommunicatorDevice Device { get; }
        #endregion /Accessors
    }
}

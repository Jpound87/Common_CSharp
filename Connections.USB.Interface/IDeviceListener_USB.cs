using System;

namespace Connections.USB.Interface
{
    public interface IDeviceEventListener_USB
    {
        #region Events
        event EventHandler<IDeviceEventArgs_USB> InsertEvent;
        event EventHandler<IDeviceEventArgs_USB> RemoveEvent;
        #endregion

        #region Methods
        void Start();
        void Stop();
        void RegisterAction_Inserted(object registrant, Action<IPortData_USB> deviceAction);
        void DeregisterInsertedAction(object registrant, Action<IPortData_USB> deviceAction = null);
        void RegisterAction_Removed(object registrant, Action<IPortData_USB> deviceAction);
        void DeregisterRemovedAction(object registrant, Action<IPortData_USB> deviceAction = null);
        #endregion
    }
}

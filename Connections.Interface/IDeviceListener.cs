using System;
using System.Management;

namespace Connections.Interface
{
    public interface IEventListener_Device
    {
        #region Events
        event EventArrivedEventHandler InsertEvent;
        event EventArrivedEventHandler RemoveEvent;
        #endregion /Events

        #region Methods
        void Start();
        void Stop();
        void RegisterAction_Inserted(object registrant, Action<ManagementBaseObject> deviceAction = null);
        void DeregisterInsertedAction(object registrant, Action<ManagementBaseObject> deviceAction = null);
        void RegisterAction_Removed(object registrant, Action<ManagementBaseObject> deviceAction);
        void DeregisterRemovedAction(object registrant, Action<ManagementBaseObject> deviceAction = null);
        #endregion /Methods
    }
}

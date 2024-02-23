using Common.Extensions;
using Common.Utility;
using Connections.Interface;
using Connections.USB.Extensions;
using Connections.USB.Interface;
using System;
using System.Collections.Generic;
using System.Management;
using System.Threading.Tasks;

namespace Connections.USB
{
    public sealed class DeviceListener_USB : IDeviceEventListener_USB
    {
        #region Identity
        public const string ClassName = nameof(DeviceListener_USB);
        #endregion

        #region Events
        public delegate void DeviceEventHandler_USB(IDeviceEventArgs_USB deviceEventArgs_USB);
        public event EventHandler<IDeviceEventArgs_USB> InsertEvent;
        public event EventHandler<IDeviceEventArgs_USB> RemoveEvent;
        #endregion

        #region Instance
        private static readonly Lazy<IDeviceEventListener_USB> instance = new Lazy<IDeviceEventListener_USB>
            (
                () => new DeviceListener_USB()
            );
        // The above kinda looks like a face!
        public static IDeviceEventListener_USB Instance
        {
            get
            {
                return instance.Value;
            }
        }
        #endregion /Instance

        #region Static Readonly
        private static readonly IEventListener_Device deviceEventListener = EventListener_Device.Instance;// DELI for short.
        private static readonly Dictionary<Object, HashSet<Action<IPortData_USB>>> dictRegistrant_InsertActions
            = new Dictionary<Object, HashSet<Action<IPortData_USB>>>();
        private static readonly Dictionary<Object, HashSet<Action<IPortData_USB>>> dictRegistrant_RemoveActions
           = new Dictionary<Object, HashSet<Action<IPortData_USB>>>();
        #endregion Static Readonly

        #region Static Globals
        private static bool listen;
        #endregion

        #region Constructor
        private DeviceListener_USB()
        {
            deviceEventListener.RegisterAction_Inserted(this, OnDeviceInserted);
            deviceEventListener.RegisterAction_Removed(this, OnDeviceRemoved);
            deviceEventListener.Start();
        }
        #endregion

        #region Control
        public void Start()
        {
            listen = true;
        }

        public void Stop()
        {
            listen = false;
        }
        #endregion

        #region Listener
        private void OnDeviceInserted(ManagementBaseObject mbo)
        {
            dictRegistrant_InsertActions.RemoveNullKeys();
            if (listen && mbo.TryCreatePortData_USB(out IPortData_USB portData_USB))
            {
                IDeviceEventArgs_USB deviceEventArgs_USB = new EventArgs_Device_USB(portData_USB);
                InsertEvent?.Invoke(this, deviceEventArgs_USB);
                if (dictRegistrant_InsertActions.TryExtractAll_Set(out Action<IPortData_USB>[] insertActions))
                {
                    Parallel.For(0, insertActions.Length, (a) =>
                    {
                        insertActions[a].Invoke(portData_USB);
                    });
                }
            }
        }

        private void OnDeviceRemoved(ManagementBaseObject mbo)
        {
            dictRegistrant_RemoveActions.RemoveNullKeys();
            if (listen && mbo.TryCreatePortData_USB(out IPortData_USB portData_USB))
            {
                IDeviceEventArgs_USB deviceEventArgs_USB = new EventArgs_Device_USB(portData_USB);
                RemoveEvent?.Invoke(this, deviceEventArgs_USB);
                if (dictRegistrant_RemoveActions.TryExtractAll_Set(out Action<IPortData_USB>[] removeActions))
                {
                    Parallel.For(0, removeActions.Length, (a) =>
                    {
                        removeActions[a].Invoke(portData_USB);
                    });
                }
            }
        }
        #endregion

        #region Registration
        public void RegisterAction_Inserted(Object registrant, Action<IPortData_USB> deviceAction)
        {
            lock (dictRegistrant_InsertActions)
            {
                if (dictRegistrant_InsertActions.CheckOrCreate(registrant, Utility_HashSet.Func_HashSet<Action<IPortData_USB>>()))
                {
                    dictRegistrant_InsertActions[registrant].Add(deviceAction);
                }
            }
        }

        public void DeregisterInsertedAction(Object registrant, Action<IPortData_USB> deviceAction = null)
        {
            lock (dictRegistrant_InsertActions)
            {
                if (deviceAction == null)
                {// Remove all by the registrant.
                    dictRegistrant_InsertActions.TryRemove(registrant);
                }
                else if (dictRegistrant_InsertActions.TryCheck(registrant))
                {
                    dictRegistrant_InsertActions[registrant].Remove(deviceAction);
                }
            }
        }

        public void RegisterAction_Removed(Object registrant, Action<IPortData_USB> deviceAction)
        {
            if (dictRegistrant_RemoveActions.CheckOrCreate(registrant, Utility_HashSet.Func_HashSet<Action<IPortData_USB>>()))
            {
                dictRegistrant_RemoveActions[registrant].Add(deviceAction);
            }
        }

        public void DeregisterRemovedAction(Object registrant, Action<IPortData_USB> deviceAction = null)
        {
            lock (dictRegistrant_RemoveActions)
            {
                if (deviceAction == null)
                {// Remove all by the registrant.
                    dictRegistrant_RemoveActions.TryRemove(registrant);
                }
                else if (dictRegistrant_RemoveActions.TryCheck(registrant))
                {
                    dictRegistrant_RemoveActions[registrant].Remove(deviceAction);
                }
            }
        }
        #endregion
    }
}

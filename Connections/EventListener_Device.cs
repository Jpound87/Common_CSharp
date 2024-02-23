using Common.Constant;
using Common.Extensions;
using Common.Utility;
using Connections.Interface;
using Runtime;
using System;
using System.Collections.Generic;
using System.Management;
using System.Threading.Tasks;

namespace Connections
{
    /// <summary>
    /// This is a singleton because you only ever need one
    /// </summary>
    public sealed class EventListener_Device : IEventListener_Device
    {
        #region Identity
        public const string ClassName = nameof(EventListener_Device);
        #endregion

        #region Instance
        private static readonly Lazy<IEventListener_Device> instance = new Lazy<IEventListener_Device>
            (
                () => new EventListener_Device()
            );
        // The above kinda looks like a face!
        public static IEventListener_Device Instance
        {
            get
            {
                return instance.Value;
            }
        }
        #endregion

        #region Constants
        private static readonly WqlEventQuery insertQuery = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity'");
        private static readonly WqlEventQuery removeQuery = new WqlEventQuery("SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity'");
        #endregion

        #region Events
        public event EventArrivedEventHandler InsertEvent
        {
            add
            {
                lock (insertWatcher)
                {
                    insertWatcher.EventArrived += value;
                }
            }
            remove
            {
                lock (insertWatcher)
                {
                    insertWatcher.EventArrived -= value;
                }
            }
        }

        public event EventArrivedEventHandler RemoveEvent
        {
            add
            {
                lock (removeWatcher)
                {
                    removeWatcher.EventArrived += value;
                }
            }
            remove
            {
                lock (removeWatcher)
                {
                    removeWatcher.EventArrived -= value;
                }
            }
        }
        #endregion

        #region Static Readonly
        private static readonly ManagementEventWatcher insertWatcher = new ManagementEventWatcher(insertQuery);
        private static readonly ManagementEventWatcher removeWatcher = new ManagementEventWatcher(removeQuery);

        private static readonly Dictionary<Object, HashSet<Action<ManagementBaseObject>>> dictRegistrant_InsertActions
            = new Dictionary<Object, HashSet<Action<ManagementBaseObject>>>();
        private static readonly Dictionary<Object, HashSet<Action<ManagementBaseObject>>> dictRegistrant_RemoveActions
           = new Dictionary<Object, HashSet<Action<ManagementBaseObject>>>();
        #endregion

        #region Readonly
        private readonly EventArrivedEventHandler InsertEventHandler;
        private readonly EventArrivedEventHandler RemoveEventHandler;
        #endregion

        #region Constructor
        /// <summary>
        /// This class allows the subscriver to listen to system events for device 
        /// insertion and removal.
        /// </summary>
        private EventListener_Device()
        {
            Log_Manager.LogMethodCall(ClassName, nameof(EventListener_Device));
            InsertEventHandler = new EventArrivedEventHandler(DeviceInsertedEvent);
            RemoveEventHandler = new EventArrivedEventHandler(DeviceRemovedEvent);
            insertWatcher.EventArrived += InsertEventHandler;
            removeWatcher.EventArrived += RemoveEventHandler;
        }

        ~EventListener_Device()
        {
            try
            {
                insertWatcher.EventArrived -= InsertEventHandler;
                removeWatcher.EventArrived -= RemoveEventHandler;
            }
            finally
            {
                insertWatcher?.Dispose();
                removeWatcher?.Dispose();
                dictRegistrant_InsertActions?.Clear();
                dictRegistrant_RemoveActions?.Clear();
            }
        }
        #endregion

        #region Control
        public void Start()
        {
            insertWatcher.Start();
            removeWatcher.Start();
        }

        public void Stop()
        {
            insertWatcher.Stop();
            removeWatcher.Stop();
        }
        #endregion

        #region Listener
        private void DeviceInsertedEvent(object _, EventArrivedEventArgs e)
        {
            if (e.NewEvent["TargetInstance"] is ManagementBaseObject mbo)
            {
                string deviceID = mbo.GetPropertyValue(Tokens.DEVICE_ID)?.ToString() ?? Tokens.UNDEFINED;
                Log_Manager.LogDebug(ClassName, $"Device '{deviceID}' insertion detected.");
                OnDeviceInsertedEvent(mbo);
            }
        }

        private static void OnDeviceInsertedEvent(ManagementBaseObject managementBaseObject)
        {
            dictRegistrant_InsertActions.RemoveNullKeys();
            if (dictRegistrant_InsertActions.TryExtractAll_Set(out Action<ManagementBaseObject>[] insertActions))
            {
                try
                {
                    Parallel.For(0, insertActions.Length, (a) =>
                    {
                        insertActions[a].Invoke(managementBaseObject);
                    });
                }
                catch(Exception ex)
                {
                    Log_Manager.LogAssert(ClassName, ex.Message);
                }
            }
        }

        private void DeviceRemovedEvent(object sender, EventArrivedEventArgs e)
        {
            if (e.NewEvent["TargetInstance"] is ManagementBaseObject mbo)
            {
                string deviceID = mbo.GetPropertyValue(Tokens.DEVICE_ID)?.ToString() ?? Tokens.UNDEFINED;
                Log_Manager.LogDebug(ClassName, $"Device '{deviceID}' removal detected.");
                OnDeviceRemovedEvent(mbo);
            }
        }

        private static void OnDeviceRemovedEvent(ManagementBaseObject managementBaseObject)
        {
            dictRegistrant_RemoveActions.RemoveNullKeys();
            if (dictRegistrant_RemoveActions.TryExtractAll_Set(out Action<ManagementBaseObject>[] removeActions))
            {
                Parallel.For(0, removeActions.Length, (a) =>
                {
                    removeActions[a].Invoke(managementBaseObject);
                });
            }
        }
        #endregion

        #region Registration
        public void RegisterAction_Inserted(object registrant, Action<ManagementBaseObject> deviceAction)
        {
            lock (dictRegistrant_InsertActions)
            {
                if (dictRegistrant_InsertActions.CheckOrCreate(registrant, Utility_HashSet.Func_HashSet<Action<ManagementBaseObject>>()))
                {
                    dictRegistrant_InsertActions[registrant].Add(deviceAction);
                }
            }
        }

        public void DeregisterInsertedAction(object registrant, Action<ManagementBaseObject> deviceAction = null)
        {
            lock (dictRegistrant_InsertActions)
            {
                if(deviceAction == null)
                {// Remove all by the registrant.
                    dictRegistrant_InsertActions.TryRemove(registrant);
                }
                else if (dictRegistrant_InsertActions.TryCheck(registrant))
                {
                    dictRegistrant_InsertActions[registrant].Remove(deviceAction);
                }
            }
        }

        public void RegisterAction_Removed(object registrant, Action<ManagementBaseObject> deviceAction)
        {
            if (dictRegistrant_RemoveActions.CheckOrCreate(registrant, Utility_HashSet.Func_HashSet<Action<ManagementBaseObject>>()))
            {
                dictRegistrant_RemoveActions[registrant].Add(deviceAction);
            }
        }

        public void DeregisterRemovedAction(object registrant, Action<ManagementBaseObject> deviceAction = null)
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

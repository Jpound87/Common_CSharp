using System;
using System.Windows.Forms;

namespace Common
{
    public static class Extensions_EventHandler
    {
        #region Identity
        public const String ClassName = nameof(Extensions_EventHandler);
        #endregion

        #region Remove 
        public static void RemoveAllHandlers<T>(this EventHandler<T> eventHandler) where T : EventArgs
        {
            foreach (Delegate d in eventHandler.GetInvocationList())
            {
                eventHandler -= (EventHandler<T>)d;
            }
        }
        #endregion /Remove

        #region Count
        public static int CountHandlers<T>(this EventHandler<T> eventHandler) where T : EventArgs
        {
            return eventHandler.GetInvocationList().Length;
        }
        #endregion /Count

        #region Get
        public static EventHandler GetEventHandler(this Control classInstance, String eventName)
        {
            //RoutedEvent[] routedEvents = EventManager.GetRoutedEventsForOwner(classInstance.GetType());


            //if (String.IsNullOrEmpty(eventName))
            //{
            //    return null;
            //}
            //FieldInfo eventField = typeof(Control).GetField(eventName, BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Static);
            //if (eventField == null)
            //{
            //    return null;
            //}

            //var value = eventField.GetValue(classInstance);
            //EventHandler eventDelegate = (EventHandler)value;

            //// eventDelegate will be null if no listeners are attached to the event
            //if (eventDelegate == null)
            //{
            //    return null;
            //}

            //return eventDelegate;
            return null;
        }
        #endregion
    }
}

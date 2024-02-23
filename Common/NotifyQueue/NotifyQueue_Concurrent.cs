using System;
using System.Collections.Concurrent;

namespace Common
{
    public class NotifyQueue_Concurrent<T> : NotifyQueue_Base<T>, INotify
    {
        #region Identity
        new public const String ClassName = nameof(NotifyQueue_Base<T>);
        public override String Identity
        {
            get
            {
                return ClassName;
            }
        }
        #endregion /Identity

        #region Queue
        private readonly ConcurrentQueue<T> queue = new ConcurrentQueue<T>();
        #endregion Queue

        #region Methods
        public override bool TryEnqueue(T item)
        {
            try
            {
                queue.Enqueue(item);
                TriggerAdded();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool TryDequeue(out T item)
        {
            try
            {
                bool success = queue.TryDequeue(out item);
                if(success)
                {
                    TriggerRemoved();
                }
                return success;
            }
            catch
            {
                item = default;
                return false;
            }
        }
        #endregion /Methods
    }
}

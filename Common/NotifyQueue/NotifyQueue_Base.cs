using System;

namespace Common
{
    #region Queue Interface
    public interface IQueue<T>
    {
        bool TryEnqueue(T item);
        bool TryDequeue(out T item);
    }
    #endregion

    public abstract class NotifyQueue_Base<T> : IQueue<T>, INotify, IIdentifiable
    {
        #region Identity
        public const String ClassName = nameof(NotifyQueue_Base<T>);
        public virtual String Identity
        {
            get
            {
                return ClassName;
            }
        }
        #endregion

        #region Events
        public event Notify OnAdded;
        public event Notify OnRemoved;
        public event Notify OnChanged;
        #endregion

        #region Methods
        public abstract bool TryEnqueue(T item);

        protected void TriggerAdded()
        {
            OnChanged?.Invoke();
            OnAdded?.Invoke();
        }

        public abstract bool TryDequeue(out T item);

        protected void TriggerRemoved()
        {
            OnChanged?.Invoke();
            OnRemoved?.Invoke();
        }
        #endregion
    }
}

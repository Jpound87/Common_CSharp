using System;

namespace Common
{
    #region Interface
    public interface IIsObject<T> : IDisposable
    {
        bool IsInstance { get; }
        T Instance { get; set; }
        DateTime InstanceTime { get; }
        void Set(in T value);
        T Get();
        bool TryGetInstance(out T value);
        void Clear();
    }
    #endregion /Interface

    #region Struct
    /// <summary>
    /// This class is designed to wrap an object with an 'IsInstance' boolean to 
    /// show if it has been instanced
    /// </summary>
    public struct IsObject<T> : IIsObject<T>
    {
        #region Identity
        public const String StructName = nameof(IsObject<T>);
        #endregion

        #region Accessors
        private bool isInstance;
        /// <summary>
        /// Bool showing if the object was instanced.
        /// </summary>
        public bool IsInstance
        {
            get
            {
                lock (StructName)
                {
                    return isInstance;
                }
            }
        }

        private T instance;
        /// <summary>
        /// The instanced object.
        /// </summary>
        public T Instance
        {
            get
            {
                return instance;
            }
            set
            {
                lock (StructName)
                {
                    instance = value;
                    isInstance = instance != null;
                    InstanceTime = DateTime.Now;
                }
            }
        }
        /// <summary>
        /// Shows time object was instanced
        /// </summary>
        public DateTime InstanceTime { get; private set; }
        #endregion /Accessors

        #region Constructor
        public IsObject(T instance)
        {
            this.instance = instance;
            isInstance = instance != null;
            InstanceTime = DateTime.Now;
        }
        #endregion /Constructor

        #region Methods
        public void Set(in T value)
        {
            Instance = value;
        }

        public T Get()
        {
            return Instance;
        }

        public bool TryGetInstance(out T value)
        {
            lock (StructName)
            {
                value = Instance;
                return isInstance;
            }
        }

        public void Clear()
        {
            lock (StructName)
            {
                if (isInstance)
                {
                    if (instance is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                    instance = default;
                    isInstance = false;
                    InstanceTime = DateTime.MinValue;
                }
            }
        }
        #endregion /Methods

        #region Dispose
        /// <summary>
        /// To detect redundant calls
        /// </summary>
        private bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            try
            {
                if (!disposed)
                {
                    if (disposing)
                    {
                        Clear();
                    }
                }
            }
            finally
            {
                disposed = true;
            }
        }
        #endregion /Dispose
    }
    #endregion /Struct
}

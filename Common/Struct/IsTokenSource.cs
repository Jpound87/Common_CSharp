using System;
using System.Threading;

namespace Common.Struct
{
    #region Interface
    public interface IIsTokenSource : IIsObject<CancellationTokenSource>
    {
        CancellationTokenSource TokenSource { get; }
        CancellationToken Token { get; }
        bool IsCancelled { get; }
        void Cancel();
    }
    #endregion

    public struct IsTokenSource : IIsTokenSource
    {
        #region Identity
        public const String StructName = nameof(IsTokenSource);
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

        private CancellationTokenSource instance;
        /// <summary>
        /// The instanced object.
        /// </summary>
        public CancellationTokenSource Instance
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
                }
            }
        }

        public DateTime InstanceTime { get; private set; }

        public CancellationTokenSource TokenSource
        {
            get
            {
                if(Instance == null)
                {
                    Instance = new CancellationTokenSource();
                }
                return Instance;
            }
        }

#pragma warning disable CS0649 // Field 'IsTokenSource.cancellationToken' is never assigned to, and will always have its default value
        private CancellationToken cancellationToken;
#pragma warning restore CS0649 // Field 'IsTokenSource.cancellationToken' is never assigned to, and will always have its default value
        public CancellationToken Token
        {
            get
            {
                return cancellationToken;
            }
        }


        /// <summary>
        /// Cancel culture.
        /// </summary>
        public bool IsCancelled => Token.IsCancellationRequested;
        #endregion

        #region Methods
        public void Set(in CancellationTokenSource value)
        {
            Instance = value;
        }

        public CancellationTokenSource Get()
        {
            return Instance;
        }

        public bool TryGetInstance(out CancellationTokenSource value)
        {
            lock (StructName)
            {
                value = Instance;
                return isInstance;
            }
        }

        public void Cancel() => TokenSource?.Cancel();

        public void Clear()
        {
            lock (StructName)
            {
                Cancel();
                instance = default;
                isInstance = false;
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
}

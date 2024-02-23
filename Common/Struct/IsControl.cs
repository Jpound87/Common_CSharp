using System;
using System.Windows.Forms;

namespace Common.Struct
{
    #region Interface
    public interface IIsControl : IIsObject<Control>
    {
        Control Control { get; }
    }
    #endregion

    #region Struct
    public struct IsControl : IIsControl
    {
        #region Identity
        public const String StructName = nameof(IsControl);
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

        private Control instance;
        /// <summary>
        /// The instanced object.
        /// </summary>
        public Control Instance
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

        public Control Control => Instance;
        #endregion

        #region Methods
        public void Set(in Control value)
        {
            Instance = value;
        }

        public Control Get()
        {
            return Instance;
        }

        public bool TryGetInstance(out Control value)
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
    #endregion /Struct
}

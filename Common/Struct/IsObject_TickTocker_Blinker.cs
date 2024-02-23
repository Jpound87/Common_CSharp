using Common.Timers;
using System;

namespace Common.Struct
{
    public class IsObject_TickTocker_Blinker : IIsObject<TickTocker_Blinker>
    {
        #region Identity
        public const String StructName = nameof(IsObject_TickTocker_Blinker);
        #endregion /Identity

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

        private TickTocker_Blinker instance;
        /// <summary>
        /// The instanced object.
        /// </summary>
        public TickTocker_Blinker Instance
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
        public IsObject_TickTocker_Blinker(int interval_ms)
        {
            instance = new TickTocker_Blinker(interval_ms);
            isInstance = instance != null;
            InstanceTime = DateTime.Now;
        }
        #endregion /Constructor

        #region Methods

        #region TickTocker Control
        public void Start()
        {
            instance?.Start();
        }

        public void Stop()
        {
            instance?.Stop();
        }

        public void UpdateInterval(int interval)
        {
            instance?.UpdateInterval(interval);
        }

        public void TryUpdateControlColor(BlinkerControlColors[] blinkerControlColors)
        {
            instance?.TryUpdateControlColor(blinkerControlColors);
        }
        #endregion /TickTocker Control

        #region Set
        public void Set(in TickTocker_Blinker value)
        {
            Instance = value;
        }
        #endregion /Set

        #region Get
        public TickTocker_Blinker Get()
        {
            return Instance;
        }

        public bool TryGetInstance(out TickTocker_Blinker value)
        {
            lock (StructName)
            {
                value = Instance;
                return isInstance;
            }
        }
        #endregion /Get

        #region Clear
        public void Clear()
        {
            lock (StructName)
            {
                if (isInstance)
                {
                    instance.Stop();
                    instance.Dispose();
                    instance = default;
                    isInstance = false;
                    InstanceTime = DateTime.MinValue;
                }
            }
        }
        #endregion /Clear

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

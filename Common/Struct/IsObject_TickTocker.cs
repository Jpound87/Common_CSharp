using Common.Timers;
using System;
using System.Collections.Generic;

namespace Common.Struct
{
    public struct IsObject_TickTocker : IIsObject<TickTocker>
    {
        #region Identity
        public const String StructName = nameof(IsObject_TickTocker);
        #endregion /Identity

        #region Readonly
        private readonly List<Action> tickRegistrants = new();
        #endregion /Readonly

        #region Events
        public event Action Tick
        {
            add
            {

                lock (instance)
                {
                    if (isInstance)
                    {
                        try
                        {
                            Instance.Tick += value;
                            tickRegistrants.Add(value);
                        }
                        catch (NullReferenceException)
                        {
                            tickRegistrants.Remove(value);
                        }
                    }
                }
            }
            remove
            {
                lock (instance)
                {
                    if (isInstance)
                    {
                        try
                        {
                            Instance.Tick -= value;
                            tickRegistrants.Remove(value);
                        }
                        catch (NullReferenceException)
                        {
                            // Not sure what to do here yet...
                        }
                    }
                }
            }
        }
        #endregion /Events

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

        private TickTocker instance;
        /// <summary>
        /// The instanced object.
        /// </summary>
        public TickTocker Instance
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
        public IsObject_TickTocker(int interval_ms = 500, bool start = true)
        {
            instance = new TickTocker(interval_ms);
            if (start)
            {
                instance.Start();
            }
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
        #endregion /TickTocker Control

        #region Set
        public void Set(in TickTocker value)
        {
            Instance = value;
        }
        #endregion /Set

        #region Get
        public TickTocker Get()
        {
            return Instance;
        }
        
        public bool TryGetInstance(out TickTocker value)
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
                    foreach (Action action in tickRegistrants)
                    {
                        Instance.Tick -= action;
                    }
                    tickRegistrants.Clear();
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

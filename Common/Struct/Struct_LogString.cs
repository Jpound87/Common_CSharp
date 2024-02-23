using System;
#if DEBUG
using System.Collections.Concurrent;
#endif

namespace Common.Struct
{
    public struct Struct_LogString : IReportError
    {
        #region Error
#if DEBUG
        private static readonly ConcurrentBag<String> backLog = new ConcurrentBag<String>();
#endif
        private IIsObject<String> isError;
        public bool ErrorOccured => isError.IsInstance;
        public String LastError => isError.Instance;
        public DateTime LastErrorTime => isError.InstanceTime;
        #endregion /Error

        #region Constructor
        public Struct_LogString(String logString)
        {
#if DEBUG
            backLog.Add(logString);
#endif
            isError = new IsObject<String>(logString);
        }
        #endregion

        #region Methods
        public void Set(String logString)
        {
#if DEBUG
            backLog.Add(logString);
#endif
            isError = new IsObject<String>(logString);
        }
        #endregion
    }
}

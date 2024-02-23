using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public struct TaskCompletionStruct
    {
        #region Events
        public AutoResetEvent AutoResetEvent { get; private set; }
        #endregion

        #region Syncronization 
        public TaskCompletionSource<String> TaskCompletionSource { get; private set; }
        #endregion

        #region Constructor
        public TaskCompletionStruct(AutoResetEvent autoResetEvent, TaskCompletionSource<String> taskCompletionSource)
        {
            AutoResetEvent = autoResetEvent;
            TaskCompletionSource = taskCompletionSource;
        }
        #endregion

        #region Dispose 
        public void Dispose()
        {
            AutoResetEvent.Dispose();
            TaskCompletionSource.TrySetCanceled();
        }
        #endregion
    }

}

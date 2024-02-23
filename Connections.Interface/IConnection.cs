using Common;
using System;
using System.Diagnostics;
using System.Threading;

namespace Connections.Interface
{
    #region Delegate
    public delegate bool processListResultDelagate(String result);
    #endregion

    public interface IConnection : IValidate, IIdentifiable, IDisposable
    {
        #region Connection
        IPort Port { get; }
        #endregion /Connection

        #region Timing
        Stopwatch WaitTimer { get; }
        TimeSpan InactiveSpan { get; }
        DateTime LastActiveTime { get; }
        #endregion /Timing

        #region Cancellation
        CancellationTokenSource TokenSource { get; }
        CancellationToken Token { get; }
        #endregion /Cancellation

        #region Syncronization
        WaitHandle TokenHandle { get; }
        EventWaitHandle[] EventWaitHandles { get; }
        Int32 WaitForExitOrEventWaits();
        Int32 LookupHandleIndex(EventWaitHandle eventWaitHandle);
        #endregion /Syncronization
    }
}

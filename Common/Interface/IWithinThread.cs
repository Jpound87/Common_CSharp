using System;
using System.Collections.Generic;
using System.Threading;

namespace Common
{
    public interface IWithinThread
    {
        #region Accessors
        EventWaitHandle ExitEventWaitHandle { get; }
        IList<EventWaitHandle> EventWaitHandles { get; }
        #endregion /Accessors

        #region Methods

        #region State
        /// <summary>
        /// Is the thread requested to exit.
        /// </summary>
        /// <returns>true (when exiting), false (otherwise).</returns>
        Boolean IsExiting();
        #endregion /State

        #region Event 
        Int32 AddEventWait(EventWaitHandle eventWaitHandle);

        void RemoveEventWait(EventWaitHandle eventWaitHandle);
        #endregion /Event

        #region Wait
        Int32 WaitForExitOrEventWaits();
        Int32 WaitForExitOrEventWaits(TimeSpan timeout);
        Int32 WaitForExitOrEventWaits(Int32 millisecondsTimeout);
        Int32 WaitForExitOrEventWaits(TimeSpan timeout, Boolean exitContext);
        Int32 WaitForExitOrEventWaits(Int32 millisecondsTimeout, Boolean exitContext);
        Int32 WaitForGivenEventsWaits(IList<EventWaitHandle> givenEventWaitHandles);
        Int32 WaitForGivenEventWaits(IList<EventWaitHandle> givenEventWaitHandles, TimeSpan timeout);
        Int32 WaitForGivenEventWaits(IList<EventWaitHandle> givenEventWaitHandles, Int32 millisecondsTimeout);
        Int32 WaitForGivenEventWaits(IList<EventWaitHandle> givenEventWaitHandles, TimeSpan timeout, Boolean exitContext);
        Int32 WaitForGivenEventWaits(IList<EventWaitHandle> givenEventWaitHandles, Int32 millisecondsTimeout, Boolean exitContext);
        #endregion /Wait

        #endregion /Methods
    }
}

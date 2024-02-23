using System;
using System.Collections.Generic;
using System.Security;
using System.Threading;

namespace Common
{
    public class AM_Thread : IWithinThread, IIdentifiable
    {
        #region Identity
        public const String ClassName = nameof(AM_Thread);
        public String Identity => ClassName;
        #endregion /Identity

        #region Volitile
        private volatile Boolean stopped = false;
        private volatile Boolean exiting = false;
        #endregion /Volitile

        #region Readonly
        private readonly AutoResetEvent exitEvent = new(false);
        private readonly IList<EventWaitHandle> eventWaitHandles;
        private readonly ThreadStart threadStart_Run;
        private readonly Action threadAction;
        #endregion /Readonly

        #region Accessors
        public bool IsStopped => stopped;

        public EventWaitHandle ExitEventWaitHandle
        {
            get
            {
                return exitEvent;
            }
        }

        public IList<EventWaitHandle> EventWaitHandles
        {
            get
            {
                return eventWaitHandles;
            }
        }
        #endregion /Accessors

        #region Thread Accessor
        public Thread Thread { get; private set; }
        #endregion /Thread Accessor

        #region Constructor
        public AM_Thread(Action threadAction)
        {
            this.threadAction = threadAction;
            exiting = false;
            stopped = false;
            threadStart_Run = new ThreadStart(Run);
            Thread = new Thread(threadStart_Run);
            eventWaitHandles = new List<EventWaitHandle>()
            {
                exitEvent
            };
        }
        #endregion Constructor

        #region Thread Setup
        private void Run()
        {
            threadAction();
            stopped = true;
        }
        #endregion /Threrad Setup

        #region Control
        public void Start()
        {
            stopped = false;
            exiting = false;
            Thread.Start();
        }

        public void Stop(Int32 millisecondsTimeout = 500)
        {
            exiting = true;
            exitEvent.Set();
            try
            {
                if (!stopped && Thread.Join(millisecondsTimeout))
                {
                    if (!stopped)
                    {
                        Thread.Interrupt(); //TODO: What does this do? 
                    }
                }
            }
            catch
            {

            }
        }
        #endregion /Control

        #region Wait Handles

        #region Add
        public int AddEventWait(EventWaitHandle eventWaitHandle)
        {
            eventWaitHandles.Add(eventWaitHandle);
            return (eventWaitHandles.Count - 1);
        }
        #endregion /Add

        #region Remove
        public void RemoveEventWait(EventWaitHandle eventWaitHandle)
        {
            eventWaitHandles.Remove(eventWaitHandle);
        }
        #endregion /Remove

        #region Clear
        public void ClearEventWaits()
        {
            eventWaitHandles.Clear();
            eventWaitHandles.Add(exitEvent);
        }
        #endregion /Clear

        #region Wait 
        // Summary:
        //     Waits for any of the elements in the specified array to receive a signal, using
        //     a 32-bit signed integer to specify the time interval.
        //
        // Parameters:
        //   millisecondsTimeout:
        //     The number of milliseconds to wait, or System.Threading.Timeout.Infinite (-1)
        //     to wait indefinitely.
        //
        // Returns:
        //     The array index of the object that satisfied the wait, or System.Threading.WaitHandle.WaitTimeout
        //     if no object satisfied the wait and a time interval equivalent to millisecondsTimeout
        //     has passed.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The waitHandles parameter is null.-or-One or more of the objects in the waitHandles
        //     array is null.
        //
        //   T:System.NotSupportedException:
        //     The number of objects in waitHandles is greater than the system permits.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     millisecondsTimeout is a negative number other than -1, which represents an infinite
        //     time-out.
        //
        //   T:System.Threading.AbandonedMutexException:
        //     The wait completed because a thread exited without releasing a mutex. This exception
        //     is not thrown on Windows 98 or Windows Millennium Edition.
        //
        //   T:System.ArgumentException:
        //     waitHandles is an array with no elements.
        //
        //   T:System.InvalidOperationException:
        //     The waitHandles array contains a transparent proxy for a System.Threading.WaitHandle
        //     in another application domain.
        public Int32 WaitForExitOrEventWaits(Int32 millisecondsTimeout)
        {
            EventWaitHandle[] eventWaitHandlesArray = new EventWaitHandle[eventWaitHandles.Count];
            eventWaitHandles.CopyTo(eventWaitHandlesArray, 0);
            return WaitHandle.WaitAny(eventWaitHandlesArray, millisecondsTimeout);
        }

        public Int32 WaitForGivenEventWaits(IList<EventWaitHandle> givenEventWaitHandles, Int32 millisecondsTimeout)
        {
            EventWaitHandle[] givenEventWaitHandlesArray = new EventWaitHandle[givenEventWaitHandles.Count];
            givenEventWaitHandles.CopyTo(givenEventWaitHandlesArray, 0);
            return WaitHandle.WaitAny(givenEventWaitHandlesArray, millisecondsTimeout);
        }

        //
        // Summary:
        //     Waits for any of the elements in the specified array to receive a signal.
        //
        // Returns:
        //     The array index of the object that satisfied the wait.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The waitHandles parameter is null.-or-One or more of the objects in the waitHandles
        //     array is null.
        //
        //   T:System.NotSupportedException:
        //     The number of objects in waitHandles is greater than the system permits.
        //
        //   T:System.ApplicationException:
        //     waitHandles is an array with no elements, and the .NET Framework version is 1.0
        //     or 1.1.
        //
        //   T:System.Threading.AbandonedMutexException:
        //     The wait completed because a thread exited without releasing a mutex. This exception
        //     is not thrown on Windows 98 or Windows Millennium Edition.
        //
        //   T:System.ArgumentException:
        //     waitHandles is an array with no elements, and the .NET Framework version is 2.0
        //     or later.
        //
        //   T:System.InvalidOperationException:
        //     The waitHandles array contains a transparent proxy for a System.Threading.WaitHandle
        //     in another application domain.
        public Int32 WaitForExitOrEventWaits()
        {
            EventWaitHandle[] eventWaitHandlesArray = new EventWaitHandle[eventWaitHandles.Count];
            eventWaitHandles.CopyTo(eventWaitHandlesArray, 0);
            return WaitHandle.WaitAny(eventWaitHandlesArray);
        }

        public Int32 WaitForGivenEventsWaits(IList<EventWaitHandle> givenEventWaitHandles)
        {
            EventWaitHandle[] givenEventWaitHandlesArray = new EventWaitHandle[givenEventWaitHandles.Count];
            givenEventWaitHandles.CopyTo(givenEventWaitHandlesArray, 0);
            return WaitHandle.WaitAny(givenEventWaitHandlesArray);
        }

        //
        // Summary:
        //     Waits for any of the elements in the specified array to receive a signal, using
        //     a System.TimeSpan to specify the time interval.
        //
        // Parameters:
        //   timeout:
        //     A System.TimeSpan that represents the number of milliseconds to wait, or a System.TimeSpan
        //     that represents -1 milliseconds to wait indefinitely.
        //
        // Returns:
        //     The array index of the object that satisfied the wait, or System.Threading.WaitHandle.WaitTimeout
        //     if no object satisfied the wait and a time interval equivalent to timeout has
        //     passed.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The waitHandles parameter is null.-or-One or more of the objects in the waitHandles
        //     array is null.
        //
        //   T:System.NotSupportedException:
        //     The number of objects in waitHandles is greater than the system permits.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     timeout is a negative number other than -1 milliseconds, which represents an
        //     infinite time-out. -or-timeout is greater than System.Int32.MaxValue.
        //
        //   T:System.Threading.AbandonedMutexException:
        //     The wait completed because a thread exited without releasing a mutex. This exception
        //     is not thrown on Windows 98 or Windows Millennium Edition.
        //
        //   T:System.ArgumentException:
        //     waitHandles is an array with no elements.
        //
        //   T:System.InvalidOperationException:
        //     The waitHandles array contains a transparent proxy for a System.Threading.WaitHandle
        //     in another application domain.
        public Int32 WaitForExitOrEventWaits(TimeSpan timeout)
        {
            EventWaitHandle[] eventWaitHandlesArray = new EventWaitHandle[eventWaitHandles.Count];
            eventWaitHandles.CopyTo(eventWaitHandlesArray, 0);
            return WaitHandle.WaitAny(eventWaitHandlesArray, timeout);
        }

        public Int32 WaitForGivenEventWaits(IList<EventWaitHandle> givenEventWaitHandles, TimeSpan timeout)
        {
            EventWaitHandle[] givenEventWaitHandlesArray = new EventWaitHandle[givenEventWaitHandles.Count];
            givenEventWaitHandles.CopyTo(givenEventWaitHandlesArray, 0);
            return WaitHandle.WaitAny(givenEventWaitHandlesArray, timeout);
        }

        // Summary:
        //     Waits for any of the elements in the specified array to receive a signal, using
        //     a System.TimeSpan to specify the time interval and specifying whether to exit
        //     the synchronization domain before the wait.
        //
        // Parameters:
        //   timeout:
        //     A System.TimeSpan that represents the number of milliseconds to wait, or a System.TimeSpan
        //     that represents -1 milliseconds to wait indefinitely.
        //
        //   exitContext:
        //     true to exit the synchronization domain for the context before the wait (if in
        //     a synchronized context), and reacquire it afterward; otherwise, false.
        //
        // Returns:
        //     The array index of the object that satisfied the wait, or System.Threading.WaitHandle.WaitTimeout
        //     if no object satisfied the wait and a time interval equivalent to timeout has
        //     passed.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The waitHandles parameter is null.-or-One or more of the objects in the waitHandles
        //     array is null.
        //
        //   T:System.NotSupportedException:
        //     The number of objects in waitHandles is greater than the system permits.
        //
        //   T:System.ApplicationException:
        //     waitHandles is an array with no elements, and the .NET Framework version is 1.0
        //     or 1.1.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     timeout is a negative number other than -1 milliseconds, which represents an
        //     infinite time-out. -or-timeout is greater than System.Int32.MaxValue.
        //
        //   T:System.Threading.AbandonedMutexException:
        //     The wait completed because a thread exited without releasing a mutex. This exception
        //     is not thrown on Windows 98 or Windows Millennium Edition.
        //
        //   T:System.ArgumentException:
        //     waitHandles is an array with no elements, and the .NET Framework version is 2.0
        //     or later.
        //
        //   T:System.InvalidOperationException:
        //     The waitHandles array contains a transparent proxy for a System.Threading.WaitHandle
        //     in another application domain.
        public Int32 WaitForExitOrEventWaits(TimeSpan timeout, Boolean exitContext)
        {
            EventWaitHandle[] eventWaitHandlesArray = new EventWaitHandle[eventWaitHandles.Count];
            eventWaitHandles.CopyTo(eventWaitHandlesArray, 0);
            return WaitHandle.WaitAny(eventWaitHandlesArray, timeout, exitContext);
        }

        public Int32 WaitForGivenEventWaits(IList<EventWaitHandle> givenEventWaitHandles, TimeSpan timeout, Boolean exitContext)
        {
            EventWaitHandle[] givenEventWaitHandlesArray = new EventWaitHandle[givenEventWaitHandles.Count];
            givenEventWaitHandles.CopyTo(givenEventWaitHandlesArray, 0);
            return WaitHandle.WaitAny(givenEventWaitHandlesArray, timeout, exitContext);
        }

        //
        // Summary:
        //     Waits for any of the elements in the specified array to receive a signal, using
        //     a 32-bit signed integer to specify the time interval, and specifying whether
        //     to exit the synchronization domain before the wait.
        //
        // Parameters:
        //   millisecondsTimeout:
        //     The number of milliseconds to wait, or System.Threading.Timeout.Infinite (-1)
        //     to wait indefinitely.
        //
        //   exitContext:
        //     true to exit the synchronization domain for the context before the wait (if in
        //     a synchronized context), and reacquire it afterward; otherwise, false.
        //
        // Returns:
        //     The array index of the object that satisfied the wait, or System.Threading.WaitHandle.WaitTimeout
        //     if no object satisfied the wait and a time interval equivalent to millisecondsTimeout
        //     has passed.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The waitHandles parameter is null.-or-One or more of the objects in the waitHandles
        //     array is null.
        //
        //   T:System.NotSupportedException:
        //     The number of objects in waitHandles is greater than the system permits.
        //
        //   T:System.ApplicationException:
        //     waitHandles is an array with no elements, and the .NET Framework version is 1.0
        //     or 1.1.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     millisecondsTimeout is a negative number other than -1, which represents an infinite
        //     time-out.
        //
        //   T:System.Threading.AbandonedMutexException:
        //     The wait completed because a thread exited without releasing a mutex. This exception
        //     is not thrown on Windows 98 or Windows Millennium Edition.
        //
        //   T:System.ArgumentException:
        //     waitHandles is an array with no elements, and the .NET Framework version is 2.0
        //     or later.
        //
        //   T:System.InvalidOperationException:
        //     The waitHandles array contains a transparent proxy for a System.Threading.WaitHandle
        //     in another application domain.
        [SecuritySafeCritical]
        public Int32 WaitForExitOrEventWaits(Int32 millisecondsTimeout, Boolean exitContext)
        {
            EventWaitHandle[] eventWaitHandlesArray = new EventWaitHandle[eventWaitHandles.Count];
            eventWaitHandles.CopyTo(eventWaitHandlesArray, 0);
            return WaitHandle.WaitAny(eventWaitHandlesArray, millisecondsTimeout, exitContext);
        }

        public Int32 WaitForGivenEventWaits(IList<EventWaitHandle> givenEventWaitHandles, Int32 millisecondsTimeout, Boolean exitContext)
        {
            EventWaitHandle[] givenEventWaitHandlesArray = new EventWaitHandle[givenEventWaitHandles.Count];
            givenEventWaitHandles.CopyTo(givenEventWaitHandlesArray, 0);
            return WaitHandle.WaitAny(givenEventWaitHandlesArray, millisecondsTimeout, exitContext);
        }
        #endregion /Wait

        #endregion /Wait Handles

        #region Exiting
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean IsExiting()
        {
            return exiting;
        }
        #endregion /Exiting
    }
}

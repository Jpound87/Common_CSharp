using Common.Utility;
using System;
using System.Threading;

namespace Common.Extensions
{
    public static class Extensions_WaitHandle
    {
        #region Wait
    
        /// <summary>
        /// This method will wait for the handle to signal for as much time as required. 
        /// The optional exti context paramater will cause it to wait on another thread. 
        /// It must be manually set to false if a same thread wait is desired. 
        /// </summary>
        /// <param name="waitHandle">The wait handle who's signal we are waiting on.</param>
        /// <param name="exitContext">True to wait on another thread (default), else false to wait on the current thread.</param>
        /// <returns></returns>
        public static bool WaitIndefinitely(this WaitHandle waitHandle, bool exitContext = true)
        {
            return waitHandle.WaitOne(Utility_WaitHandle.WaitForever, exitContext);
        }

        /// <summary>
        /// This method will wait on the first signaled wait handle. If it matches the signal handle it will return true,
        /// otherwise it will set the signal handle to the triggered handle then return false. 
        /// </summary>
        /// <param name="timeoutSpan">The time to wiat for a wait handle signal.</param>
        /// <param name="signalHandle">The desired wait handle.</param>
        /// <param name="waitHandles">The handles to wait for a signal on.</param>
        /// <returns>True if the desired handle was signaled first, else false with the reference being set to the signaled handle or null if timeout.</returns>
        public static bool WaitForCertainHandleIndex(this WaitHandle[] waitHandles, ref int signalIndex, TimeSpan timeoutSpan, bool exitContext = true)
        {
            int index = WaitHandle.WaitAny(waitHandles, timeoutSpan, exitContext);
            if (index != WaitHandle.WaitTimeout)
            {
                if(signalIndex == index)
                {
                    return true;
                }
                signalIndex = index;
                return false;
            }
            signalIndex = WaitHandle.WaitTimeout;
            return false;
        }

        /// <summary>
        /// This method will wait on the first signaled wait handle. If it matches the signal handle it will return true,
        /// otherwise it will set the signal handle to the triggered handle then return false. 
        /// </summary>
        /// <param name="timeoutSpan">The time to wiat for a wait handle signal.</param>
        /// <param name="signalHandle">The desired wait handle.</param>
        /// <param name="waitHandles">The handles to wait for a signal on.</param>
        /// <returns>True if the desired handle was signaled first, else false with the reference being set to the signaled handle or null if timeout.</returns>
        public static bool WaitForCertainHandle(this WaitHandle[] waitHandles, ref WaitHandle signalHandle, TimeSpan timeoutSpan, bool exitContext = true)
        {            
            int index = WaitHandle.WaitAny(waitHandles, timeoutSpan, exitContext);
            if (index != WaitHandle.WaitTimeout)
            {
                if (signalHandle == waitHandles[index])
                {
                    return true;
                }
                else
                {
                    signalHandle = waitHandles[index];
                    return false;
                }
            }
            signalHandle = null;
            return false;
        }

        /// <summary>
        /// This method will wait on the first signaled wait handle. If it matches the signal handle it will return true,
        /// otherwise it will set the signal handle to the triggered handle then return false. 
        /// </summary>
        /// <param name="timeoutSpan">The time to wiat for a wait handle signal.</param>
        /// <param name="signalHandle">The desired wait handle.</param>
        /// <param name="waitHandles">The handles to wait for a signal on.</param>
        /// <returns>True if the desired handle was signaled first, else false with the reference being set to the signaled handle or null if timeout.</returns>
        public static bool WaitForCertainHandle(this WaitHandle[] waitHandles, ref WaitHandle signalHandle, int timeoutSpan_ms = -1, bool exitContext = true)
        {
            int index = WaitHandle.WaitAny(waitHandles, timeoutSpan_ms, exitContext);
            if (index != WaitHandle.WaitTimeout)
            {
                if (signalHandle == waitHandles[index])
                {
                    return true;
                }
                else
                {
                    signalHandle = waitHandles[index];
                    return false;
                }
            }
            signalHandle = null;
            return false;
        }
        #endregion /Wait
    }
}

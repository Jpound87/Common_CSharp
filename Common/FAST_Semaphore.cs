using System;
using System.Threading;

namespace Common
{
    public class FAST_Semaphore : IIdentifiable
    {
        #region Identity
        public const String ControlName = nameof(FAST_Semaphore);
        public String Identity
        {
            get
            {
                return ControlName;
            }
        }
        #endregion

        #region Readonly
        private readonly int limit = 0;
        private readonly Mutex locker = new Mutex();
        #endregion

        #region Globals
        private int count = 0;
        #endregion

        #region Constructor

        public FAST_Semaphore(int initialCount, int maximumCount)
        {
            count = initialCount;
            limit = maximumCount;
        }
        #endregion

        #region Wait
        public void Wait(int timeout = Timeout.Infinite)
        {
            lock (locker)
            {
                if (count == 0)
                {
                    System.Threading.Monitor.Wait(locker, timeout);
                }
                count--;
            }
        }
        #endregion

        #region Release
        public bool TryRelease()
        {
            lock (locker)
            {
                if (count < limit)
                {
                    count++;
                    System.Threading.Monitor.PulseAll(locker);
                    return true;
                }
                return false;
            }
        }
        #endregion
    }
}

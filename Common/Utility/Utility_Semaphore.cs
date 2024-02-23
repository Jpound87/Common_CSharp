using System;
using System.Threading;

namespace Common.Utility
{
    public static class Utility_Semaphore
    {
        #region Create
        /// <summary>
        /// Creates a slim semaphore that has one untaken signal.
        /// </summary>
        /// <returns>The constructed slim semaphore.</returns>
        public static SemaphoreSlim Create_Slim_Single()
        {
            return new SemaphoreSlim(1, 1);
        }

        /// <summary>
        /// Creates a slim semaphore that has one signal in the given state.
        /// </summary>
        /// <param name="taken">The taken state of the semaphore, if true the semaphore will be blocking, 
        /// else if false it can be taken.</param>
        /// <returns>The constructed slim semaphore.</returns>
        public static SemaphoreSlim Create_Slim_Single(Boolean taken)
        {
            return new SemaphoreSlim(taken ? 0 : 1, 1);
        }

        public static SemaphoreSlim Create_Slim(Int32 initalCount = 0, Int32 maxCount = 0)
        {
            return new SemaphoreSlim(initalCount, maxCount);
        }
        #endregion /Create
    }
}

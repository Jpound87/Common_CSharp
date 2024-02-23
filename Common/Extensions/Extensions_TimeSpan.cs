using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static class Extensions_TimeSpan
    {
        #region Identity
        public const String ClassName = nameof(Extensions_TimeSpan);
        #endregion

        #region Comparison
        /// <summary>
        /// This method is intended to determine if the given time is within 
        /// the given time span of the current time 
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="lastUpdateTime"></param>
        /// <returns></returns>
        public static bool IsWithinTimeWindow(this DateTime lastUpdateTime, TimeSpan timeSpan)
        {
            DateTime endTime = lastUpdateTime + timeSpan;
            if (DateTime.Now < endTime)
            {
                return true;
            }
            return false;
        }
        #endregion /Comaprison

        #region Average
        public static TimeSpan Average_TimeSpan(this ICollection<TimeSpan> timeSpans)
        {
            return timeSpans.ToArray().Average_TimeSpan();
        }

        public static TimeSpan Average_TimeSpan(this TimeSpan[] timeSpans)
        {
            long totalTime = timeSpans[0].Ticks;
            for (int ts = 1; ts < timeSpans.Length; ts++)
            {
                totalTime += timeSpans[ts].Ticks;
            }
            return TimeSpan.FromTicks(totalTime / timeSpans.Length);
        }
        #endregion /Average
    }
}

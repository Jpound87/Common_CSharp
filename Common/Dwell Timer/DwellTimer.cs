using Common.Extensions;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Common
{
    public class DwellTimer : IDwellTimer
    {
        #region Identity
        public const String ClassName = nameof(DwellTimer);
        #endregion

        #region Timer
        private readonly Stopwatch dwellTimer = new Stopwatch();
        #endregion

        #region Current Time [Timespan]
        public TimeSpan CurrentDwell
        { 
            get
            {
                return dwellTimes[index];
            }
        }
        public TimeSpan AverageDwell
        {
            get
            {
                return dwellTimes.Average_TimeSpan();
            }
        }
        #endregion

        #region Last Time [double ms]
        public double LastDwell_ms { get; private set; }
        
        public double LastAverageDwell_ms { get; private set; }
        #endregion

        #region Readings
        private int messageCount;
        private ulong index;
        private readonly TimeSpan[] dwellTimes;
        #endregion

        #region Constructor
        public DwellTimer(TimeSpan startingDwell, uint numberOfReadings)
        {
            dwellTimes = new TimeSpan[numberOfReadings];
            Parallel.For(0, dwellTimes.Length, (r) =>
            {
                dwellTimes[r] = startingDwell;
            });
        }
        #endregion

        #region Control
        public void StartReading()
        {
            messageCount = 1;
            dwellTimer.Start();
        }

        /// <summary>
        /// Thsi method allows for the timing of mutuple parallel processed messges.
        /// </summary>
        /// <param name="messageCount"></param>
        public void StartReading(int messageCount)
        {
            this.messageCount = Math.Max(messageCount, 1);
            dwellTimer.Start();
        }

        public void EndReading()
        {
            if (dwellTimer.IsRunning)
            {
                TimeSpan reading = dwellTimer.Elapsed;
                LastDwell_ms = reading.TotalMilliseconds / messageCount;
                dwellTimer.Reset();
                dwellTimes[index++ % (ulong)dwellTimes.Length] = TimeSpan.FromMilliseconds(LastDwell_ms);
                LastAverageDwell_ms = dwellTimes.Average_TimeSpan().TotalMilliseconds;
            }

        }
        #endregion
    }
}

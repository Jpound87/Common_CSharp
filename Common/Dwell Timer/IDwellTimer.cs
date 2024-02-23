using System;

namespace Common
{
    public interface IDwellTimer
    {
        #region Current Time [Timespan]
        TimeSpan CurrentDwell { get; }
        TimeSpan AverageDwell{ get; }
        #endregion

        #region Last Time [double ms]
        double LastDwell_ms { get; }

        double LastAverageDwell_ms{ get; }
        #endregion

        #region Control
        void StartReading();
        void StartReading(int messageCount);
        void EndReading();
        #endregion
    }
}

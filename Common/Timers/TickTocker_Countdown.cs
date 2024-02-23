using System;

namespace Common.Timers
{
    public class TickTocker_Countdown : TickTocker
    {
        #region Identity
        new public const String ClassName = nameof(TickTocker_Countdown);
        #endregion

        #region Events
        public event Action Timeout;
        #endregion

        #region Accessors
        public int RemainingTime_ms { get; private set; }
        #endregion

        #region Constructor
        public TickTocker_Countdown(int countdown_ms, int interval_ms) : base(interval_ms)
        {
            Tick += CheckTime;
            RemainingTime_ms = countdown_ms;
        }
        #endregion

        #region Check Time
        private void CheckTime()
        {
            RemainingTime_ms -= Interval;
            if(RemainingTime_ms <= 0)
            {
                RemainingTime_ms = 0;
                Timeout?.Invoke();
            }
        }
        #endregion
    }
}

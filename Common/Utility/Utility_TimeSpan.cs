using System;

namespace Common.Utility
{
    public static class Utility_TimeSpan
    {
        #region Identity
        public const String ClassName = nameof(Utility_TimeSpan);
        #endregion

        #region Constants

        #region Functions
        public static readonly Func<TimeSpan> FuncZeroTimeSpan = CreateZeroTimeSpan_Func();
        #endregion

        #endregion /Constants

        #region Function Creation
        public static Func<TimeSpan> CreateZeroTimeSpan_Func()
        {
            return new Func<TimeSpan>(() => { return TimeSpan.Zero; });
        }

        public static Func<TimeSpan> CreateTimeSpan_ms_Func(int span_milliseconds = 0)
        {
            return new Func<TimeSpan>(() => { return TimeSpan.FromMilliseconds(span_milliseconds); });
        }

        public static Func<TimeSpan> CreateTimeSpan_s_Func(int span_seconds = 0)
        {
            return new Func<TimeSpan>(() => { return TimeSpan.FromSeconds(span_seconds); });
        }

        public static Func<TimeSpan> CreateTimeSpan_mins_Func(int span_minutes = 0)
        {
            return new Func<TimeSpan>(() => { return TimeSpan.FromMinutes(span_minutes); });
        }
        #endregion /Function Creation
    }
}

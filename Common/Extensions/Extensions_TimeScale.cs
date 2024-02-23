using Common.Constant;

namespace Common.Extensions
{
    public static class Extensions_TimeScale
    {
        #region Adjust
        public static void AdjustTimeScale(this TimeScale currentTimeScale, TimeScale newTimeScale, ref double value)
        {
            if (currentTimeScale != newTimeScale)
            {
                switch (currentTimeScale)
                {
                    case TimeScale.Hours:
                        switch (newTimeScale)
                        {
                            case TimeScale.Hours:
                                break;
                            case TimeScale.Minutes:
                                AdjustTime(ref value, 60.0);
                                break;
                            case TimeScale.Seconds:
                                AdjustTime(ref value, 3600.0);
                                break;
                            case TimeScale.Milliseconds:
                                AdjustTime(ref value, 3600000.0);
                                break;
                            case TimeScale.Microseconds:
                                AdjustTime(ref value, 3600000000.0);
                                break;
                        }
                        break;
                    case TimeScale.Minutes:
                        switch (newTimeScale)
                        {
                            case TimeScale.Hours:
                                AdjustTime(ref value, 1.0 / 60.0);
                                break;
                            case TimeScale.Minutes:
                                break;
                            case TimeScale.Seconds:
                                AdjustTime(ref value, 60.0);
                                break;
                            case TimeScale.Milliseconds:
                                AdjustTime(ref value, 60000.0);
                                break;
                            case TimeScale.Microseconds:
                                AdjustTime(ref value, 600000000.0);
                                break;
                        }
                        break;
                    case TimeScale.Seconds:
                        switch (newTimeScale)
                        {
                            case TimeScale.Hours:
                                AdjustTime(ref value, 1.0 / 3600.0);
                                break;
                            case TimeScale.Minutes:
                                AdjustTime(ref value, 1.0 / 60.0);
                                break;
                            case TimeScale.Seconds:
                                break;
                            case TimeScale.Milliseconds:
                                AdjustTime(ref value, 1000.0);
                                break;
                            case TimeScale.Microseconds:
                                AdjustTime(ref value, 1000000.0);
                                break;
                        }
                        break;
                    case TimeScale.Milliseconds:
                        switch (newTimeScale)
                        {
                            case TimeScale.Hours:
                                AdjustTime(ref value, 1.0 / 3600000.0);
                                break;
                            case TimeScale.Minutes:
                                AdjustTime(ref value, 1.0 / 60000.0);
                                break;
                            case TimeScale.Seconds:
                                AdjustTime(ref value, 1.0 / 1000.0);
                                break;
                            case TimeScale.Milliseconds:
                                break;
                            case TimeScale.Microseconds:
                                AdjustTime(ref value, 1000.0);
                                break;
                        }
                        break;
                    case TimeScale.Microseconds:
                        switch (newTimeScale)
                        {
                            case TimeScale.Hours:
                                AdjustTime(ref value, 1.0 / 3600000000.0);
                                break;
                            case TimeScale.Minutes:
                                AdjustTime(ref value, 1.0 / 60000000.0);
                                break;
                            case TimeScale.Seconds:
                                AdjustTime(ref value, 1.0 / 1000000.0);
                                break;
                            case TimeScale.Milliseconds:
                                AdjustTime(ref value, 1.0 / 1000.0);
                                break;
                            case TimeScale.Microseconds:
                                break;
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// This method adjusts the time scale of the plot by a scale of 'factor'
        /// </summary>
        /// <param name="factor">Number to adjust time scale by</param>
        private static void AdjustTime(ref double value, double factor)
        {
            value *= factor;
        }
        #endregion /Adjust
    }
}

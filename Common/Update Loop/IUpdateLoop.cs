using Common.Interface;
using System;
using System.Threading;

namespace Common
{
    public interface IUpdateLoop : IAlive, IDisposable
    {
        #region Accessors

        #region State
        int LoopCount { get; set; }
        TimeSpan UpdateLoopTime { get; }
        DateTime LastUpdateTime { get; }
        DateTime NextUpdateTime { get; }
        CancellationToken Token { get; }
        CancellationTokenSource TokenSource { get; }
        #endregion /State

        #region Control
        bool Lock { get; set; }
        TimeSpan UpdateRate { get; }
        TimeSpan MinimumUpdateRate { get; set; }
        TimeSpan RequiredDelta_RateChange { get; set; }
        #endregion /Control

        #endregion /Accessors

        #region Methods
        bool SetUpdateRate(TimeSpan newUpdateRate);
        #endregion /Methods
    }
}

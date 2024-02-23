using System;
using System.Threading;

namespace Common
{
    public class UpdateLoop_Timer : UpdateLoop_Base
    {
        #region Identity
        new public const String ClassName = nameof(UpdateLoop_Timer);
        public override String Identity
        {
            get
            {
                return ClassName;
            }
        }
        #endregion

        #region Readonly
        private readonly object updateLock = new();
        #endregion /Readonly

        #region Globals
        private int atLoopCount;
        private Action<Object> wrappedAction;
        private Timer updateTimer;
        private TimerCallback timerCallback;
        #endregion

        #region Constructor

        // General way I think these timers work:
        // 1. Try to process the updateAction on each timer call-back
        // 2. wrappedAction need to be reentrant, because timerCallback is called using ThreadPool.
        // 3. If still processing the updateAction for the previous timeout then skip this updateAction.
        //    This prevents queueing of updateAction which can occur due to timerCallback using ThreadPool
        //    and updateAction taking too long to finish. This also means the updateAction is not suitable
        //    for things that HAVE to be handled for every timeout.
        // 4. Detect if timer should be stopped and handle it. Not guaranteed to stop imediately
        //    as timers are not fully disposed until all currently queued callbacks have completed

        public UpdateLoop_Timer(TimeSpan updateRate, Action updateAction, CancellationToken cancellationToken)
            : this(updateRate, TimeSpan.Zero, updateAction, cancellationToken)
        {
        }

        public UpdateLoop_Timer(TimeSpan updateRate, TimeSpan requiredDelta, Action updateAction, CancellationToken cancellationToken) 
            : base(updateRate, requiredDelta, updateAction, cancellationToken)
        {
            wrappedAction = new Action<object>((o) =>
            {
                blocker.WaitOne(); // Tim: not sure about what this is for (why we suspend this...?)

                if (cancellationToken.IsCancellationRequested)
                {
                    updateTimer.Dispose();
                }
                else
                {
                    if (System.Threading.Monitor.TryEnter(updateLock))
                    {
                        try
                        {
                            updateAction.Invoke();
                            UpdateStatistics();
                        }
                        finally
                        {
                            // Always release the lock no matter what happened (e.g. exceptions)
                            System.Threading.Monitor.Exit(updateLock);
                        }
                    }
                }
            });

            timerCallback = new TimerCallback(wrappedAction);
            updateTimer = new Timer(timerCallback, null, TimeSpan.Zero, updateRate);
            RegisterDisposables(updateTimer);
        }

        public UpdateLoop_Timer(TimeSpan updateRate, Action updateAction, CancellationToken cancellationToken, int loopCount) 
            : base(updateRate, updateAction, cancellationToken, loopCount)
        {
            Interlocked.Exchange(ref atLoopCount, loopCount);

            wrappedAction = new Action<object>((o) =>
            {
                blocker.WaitOne(); // Tim: not sure about what this is for (why we suspend this...?)

                if (cancellationToken.IsCancellationRequested || Interlocked.Decrement(ref atLoopCount) <= 0)
                {
                    updateTimer.Dispose();
                }
                else
                {
                    if (System.Threading.Monitor.TryEnter(updateLock))
                    {
                        try
                        {
                            updateAction.Invoke();
                            UpdateStatistics();
                        }
                        finally
                        {
                            // Always release the lock no matter what happened (e.g. exceptions)
                            System.Threading.Monitor.Exit(updateLock);
                        }
                    }
                }
            });

            timerCallback = new TimerCallback(wrappedAction);
            updateTimer = new Timer(timerCallback, null, TimeSpan.Zero, updateRate);
            RegisterDisposables(updateTimer);
        }
        #endregion /Constructor

        #region Update Rate
        protected override void UpdateStatistics()
        {
            MinimumUpdateRate = DateTime.Now - LastUpdateTime; // The minimum rate is the time it takes for one 'loop'
            LastUpdateTime = DateTime.Now;
        }

        protected override void ChangeUpdateRate(TimeSpan newUpdateRate)
        {
            TimeSpan deltaTime = (newUpdateRate - UpdateRate).Duration();
            if (RequiredDelta_RateChange < deltaTime)
            {
                UpdateRate = newUpdateRate;

                TimeSpan timeToNext = NextUpdateTime - DateTime.Now;
                if (timeToNext > TimeSpan.Zero)
                {
                    updateTimer.Change(timeToNext, newUpdateRate);
                }
                else
                {
                    updateTimer.Change(TimeSpan.Zero, newUpdateRate);
                }
            }
        }
        #endregion

        #region Dispose
        public override void Dispose()
        {
            updateTimer.Dispose();

            timerCallback = null;
            wrappedAction = null;

            base.Dispose();
        }
        #endregion
    }
}

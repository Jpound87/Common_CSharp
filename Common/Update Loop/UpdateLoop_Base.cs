using Common.Base;
using Common.Constant;
using Common.Extensions;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    #region UpdateTimerEventArgs

    #region Interface
    public interface IUpdateLoopEventArgs
    {
        Exception Exception { get; }
        String Target { get; }
        String Method { get; }
    }
    #endregion

    #region Class
    public class UpdateLoopEventArgs : IUpdateLoopEventArgs
    {
        #region Methods
        public Exception Exception { get; private set; }
        public String Target { get; private set; }
        public String Method { get; private set; }
        #endregion

        #region Constructor
        public UpdateLoopEventArgs(Exception ex, Action action) 
        {
            this.Exception = ex;
            if (action != null)
            {
                Target = action.Target.ToString();
                Method = action.Method.Name;
            }
        }
        #endregion
    }
    #endregion

    #endregion

    public abstract class UpdateLoop_Base : Dispose_Base, IUpdateLoop
    {
        #region DEBUG
#if DEBUG
        private String methodName = Tokens.NONE; 
        protected static readonly List<String> activeLoopMethodNames = new List<String>();
        protected static readonly Dictionary<String, int> loopNameActiveCount = new Dictionary<String, int>();
#endif
        #endregion /DEBUG

        #region Exception
        public delegate void UpdateTimerExceptionHandler(object sender, IUpdateLoopEventArgs ex);
        public event UpdateTimerExceptionHandler UpdateTimerException;
        #endregion

        #region Timing
        private static readonly TimeSpan MAX_WAIT_TIME = TimeSpan.FromSeconds(5);
        #endregion

        #region Readonly
        private bool semaphoreSignaled = true;
        private readonly SemaphoreSlim awaiter = Utility_Semaphore.Create_Slim_Single();
        private readonly AsyncCallback asyncCallback;
        protected readonly ManualResetEvent blocker = new ManualResetEvent(true);
        #endregion

        #region Globals

        #region Cancellation
        public bool IsAlive { get; protected set; }
        public bool IsCancellationRequested
        {
            get
            {
                return TokenSource == null || TokenSource.IsCancellationRequested;
            }
        }
        public CancellationTokenSource TokenSource { get; private set; }
        public CancellationToken Token { get; private set; }
        #endregion

        #region Timing
        public TimeSpan UpdateRate { get; protected set; }
        public TimeSpan MinimumUpdateRate { get; set; }
        public TimeSpan RequiredDelta_RateChange { get; set; }
        public TimeSpan UpdateLoopTime { get; set; }
        public DateTime LastUpdateTime { get; protected set; }
        public DateTime NextUpdateTime
        {
            get
            {
                return LastUpdateTime + UpdateRate;
            }
        }
        public int LoopCount { get; set; }
        #endregion

        #region Action
        protected Action updateAction;
        #endregion

        #region Control
        private const uint MAX_CONSECUTIVE_ERROR = 3;// A nice arbitrary number.
        private uint consecutiveErrorCount;
        private bool locke = false;// 'lock' is a reseved word
        public bool Lock
        {
            get
            {
                return locke;
            }
            set
            {
                lock (blocker)
                {
                    if (locke != value)
                    {
                        locke = value;
                        if(locke)
                        {
                            blocker.Reset();
                        }
                        else
                        {
                            blocker.Set();
                        }
                    }
                }
            }
        }

        private TimeSpan timeoutSpan = MAX_WAIT_TIME;
        public TimeSpan TimeoutSpan
        {
            get
            {
                return timeoutSpan;
            }
            set
            {
                timeoutSpan = value;
            }
        }
        #endregion

        #endregion /Globals

        #region Constructors

        public UpdateLoop_Base(CancellationToken cancellationToken, int loopCount = 0)
        {
            asyncCallback = new AsyncCallback(CallBack);
            InitilizeProperties(TimeSpan.Zero, TimeSpan.Zero, cancellationToken, loopCount);
        }

        public UpdateLoop_Base(Action updateAction, CancellationToken cancellationToken, int loopCount = 0)
        {
            asyncCallback = new AsyncCallback(CallBack);
            this.updateAction = updateAction;
            InitilizeProperties(TimeSpan.Zero, TimeSpan.Zero, cancellationToken, loopCount);
        }

        public UpdateLoop_Base(TimeSpan updateRate, Action updateAction, CancellationToken cancellationToken, int loopCount = 0)
        {
            asyncCallback = new AsyncCallback(CallBack);
            this.updateAction = updateAction;
            InitilizeProperties(updateRate, TimeSpan.Zero, cancellationToken, loopCount);
        }

        public UpdateLoop_Base(TimeSpan updateRate, TimeSpan requiredDelta, Action updateAction, CancellationToken cancellationToken, int loopCount = 0)
        {
            asyncCallback = new AsyncCallback(CallBack);
            this.updateAction = updateAction;
            InitilizeProperties(updateRate, requiredDelta, cancellationToken, loopCount);
        }

        private void InitilizeProperties(TimeSpan updateRate, TimeSpan requiredDelta, CancellationToken cancellationToken, int loopCount = 0)
        {
            TokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Token = TokenSource.Token;
            UpdateRate = updateRate;
            MinimumUpdateRate = updateRate;
            RequiredDelta_RateChange = requiredDelta;
            LoopCount = loopCount;
            LastUpdateTime = DateTime.Now;
            IsAlive = true;
        }

        #endregion

        #region Update Action
        private void CallBack(IAsyncResult asyncResult)
        {
            if (semaphoreSignaled)
            {
                awaiter.Release();
            }
        }

        protected virtual void TimerAction_Run()
        {
#if DEBUG
            methodName = updateAction.Method.Name;
            activeLoopMethodNames.Add(methodName);
            if(loopNameActiveCount.TryCheckIsOrAdd(methodName, 0))
            {
                Debug.WriteLine($"Loop {methodName} already running");
            }
#endif
            while(!IsCancellationRequested)
            {
#if DEBUG
                loopNameActiveCount[methodName]++;
                if (loopNameActiveCount[methodName] > 1)
                {
                    throw new Exception("WTF");// What a Terrible Failure
                }
#endif
                try
                {
                    blocker.WaitOne(-1, true);
                    updateAction?.Invoke();
                    Token.WaitHandle.WaitOne(UpdateRate, true);
                }
                catch (ObjectDisposedException odium)// The Way of Kings.
                {
                    ThrowException(odium);
                    Dispose();
                }
                catch (TaskCanceledException tex)
                {
                    ThrowException(tex);
                    Dispose();
                }
                catch (Exception ex)
                {
                    ThrowException(ex);
                    if (consecutiveErrorCount++ > MAX_CONSECUTIVE_ERROR)
                    {
                        Dispose();
                    }
                }
                finally
                {
#if DEBUG
                    loopNameActiveCount.TryDecriment(methodName);
#endif
                }
            }
        }

        protected virtual void DeltaTimerAction_Run()
        {
#if DEBUG
            methodName = updateAction.Method.Name;
            activeLoopMethodNames.Add(methodName);
#endif
            while (!IsCancellationRequested)
            {
                try
                {
                    blocker.WaitOne(-1, true);
                    if (IsCancellationRequested)
                    {
                        break;
                    }
                    LastUpdateTime = DateTime.Now;
                    updateAction.Invoke();
                    UpdateStatistics();
                    SetUpdateRate(UpdateLoopTime);// Adjust the rate of the loop to match the time it takes to complete
                    Token.WaitHandle.WaitOne(UpdateRate, true);
                    consecutiveErrorCount = 0;
                }
                catch (ObjectDisposedException odium)// The Way of Kings
                {
                    ThrowException(odium);
                    Dispose();
                }
                catch (TaskCanceledException tex)
                {
                    ThrowException(tex);
                    Dispose();
                }
                catch (Exception ex)
                {
                    ThrowException(ex);
                    if (consecutiveErrorCount++ > MAX_CONSECUTIVE_ERROR)
                    {
                        Dispose();
                    }
                }
            }
            return;
        }

        protected virtual void LoopCountTimerAction_Run()
        {
#if DEBUG
            methodName = updateAction.Method.Name;
            activeLoopMethodNames.Add(methodName);
#endif
            for (int count = 0; count < LoopCount; count++)
            {
                blocker.WaitOne(-1, true);
                if (IsCancellationRequested)
                {
                    break;
                }
                LastUpdateTime = DateTime.Now;
                try
                {
                    updateAction.Invoke();
                    Token.WaitHandle.WaitOne(UpdateRate, true);
                }
                catch (ObjectDisposedException odium)// The Way of Kings
                {
                    ThrowException(odium);
                    Dispose();
                }
                catch (TaskCanceledException tex)
                {
                    ThrowException(tex);
                    Dispose();
                }
                catch (Exception ex)
                {
                    ThrowException(ex);
                }
            }
            return;
        }

        protected void ThrowException(Exception ex)
        {
            IUpdateLoopEventArgs args = new UpdateLoopEventArgs(ex, updateAction);
            UpdateTimerException?.Invoke(this, args);
        }

        #endregion /Update Action

        #region Update Rate
        protected virtual void UpdateStatistics()
        {
            UpdateLoopTime = DateTime.Now - LastUpdateTime;
            LastUpdateTime = DateTime.Now;
        }

        public bool SetUpdateRate(TimeSpan newUpdateRate)
        {
            if (newUpdateRate != UpdateRate)
            {
                if (MinimumUpdateRate > newUpdateRate)
                {
                    newUpdateRate = MinimumUpdateRate;
                }
                ChangeUpdateRate(newUpdateRate);
                return true;
            }
            return false;
        }

        protected virtual void ChangeUpdateRate(TimeSpan newUpdateRate)
        {
            TimeSpan deltaTime = (newUpdateRate - UpdateRate).Duration();
            if (RequiredDelta_RateChange < deltaTime)
            {
                UpdateRate = newUpdateRate;
            }
        }
        #endregion

        #region Dispose
        public override void Dispose()
        {
            IsAlive = false;
#if DEBUG
            loopNameActiveCount.TryRemove(methodName);
            if (updateAction != null)
            {
                activeLoopMethodNames.Remove(methodName);
            }
#endif
            TokenSource.Cancel();// Make sure its cancelled
            blocker.Set();// Need to release the threads 
            updateAction = null;
            LoopCount = 0;
            try
            {
                awaiter?.Dispose();
                blocker?.Dispose();
                TokenSource.Cancel();
            }
            finally
            {
                base.Dispose();
            }
        }
        #endregion
    }
}

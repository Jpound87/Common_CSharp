using System;
using System.Threading;

namespace Common
{
    public class UpdateLoop_Thread : UpdateLoop_Base
    {
        #region Thread
        private readonly TimeSpan joinTimeout = TimeSpan.FromMilliseconds(100); 
        private readonly Thread updateThread;
        #endregion

        #region Constructor
        public UpdateLoop_Thread(TimeSpan updateRate, Action updateAction, CancellationToken cancellationToken)
            : base(updateRate, updateAction, cancellationToken)
        {
            updateThread = new Thread(new ThreadStart(TimerAction_Run));
            updateThread.Start();
        }

        public UpdateLoop_Thread(TimeSpan updateRate, TimeSpan requiredDelta, Action updateAction, CancellationToken cancellationToken)
              : base(updateRate, requiredDelta, updateAction, cancellationToken)
        {
            updateThread = new Thread(new ThreadStart(DeltaTimerAction_Run));
            updateThread.Start();
        }

        public UpdateLoop_Thread(TimeSpan updateRate, int loopCount, Action updateAction, CancellationToken cancellationToken)
            : base(updateRate, updateAction, cancellationToken, loopCount)
        {
            updateThread = new Thread(new ThreadStart(LoopCountTimerAction_Run));
            updateThread.Start();
        }
        #endregion

        #region Dispose
        public override void Dispose()
        {
            try
            {
                updateThread?.Join(joinTimeout);
                if (updateThread != null && updateThread.IsAlive)
                {
                    updateThread.Interrupt();
                }
            }
            finally
            {
                base.Dispose();
            }
        }
        #endregion
    }
}

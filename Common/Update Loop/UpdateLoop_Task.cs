using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public class UpdateLoop_Task : UpdateLoop_Base
    {
        #region Task
        private Task awaitingTask;
        #endregion

        #region Constructor

        public UpdateLoop_Task(Action updateAction, CancellationToken cancellationToken) : base(updateAction, cancellationToken)
        {
            Task scheduledTask = new Task(updateAction, Token);
            FireTask(scheduledTask);
        }

        public UpdateLoop_Task(Action<CancellationToken> updateActionCT, CancellationToken cancellationToken)
            : base(cancellationToken)
        {
            updateAction = new Action(() => updateActionCT.Invoke(cancellationToken));
            Task scheduledTask = new Task(updateAction, Token);
            FireTask(scheduledTask);
        }
        #endregion

        #region Control
        private async void FireTask(params Task[] scheduledTasks)
        {
            Parallel.For(0, scheduledTasks.Length, (st) =>
            {
                scheduledTasks[st].Start();
            });
            awaitingTask = Task.WhenAll(scheduledTasks);
            await awaitingTask;
            Parallel.For(0, scheduledTasks.Length, (st) =>
            {
                scheduledTasks[st].Dispose();
            });
        }

        public async Task Await(CancellationToken cancellationToken)
        {
            if (awaitingTask != null)
            {
                Task cancellationTask = new Task(() => { cancellationToken.WaitHandle.WaitOne();});
                cancellationTask.Start();
                await Task.WhenAny(awaitingTask, cancellationTask);
            }
        }
        #endregion
    }
}

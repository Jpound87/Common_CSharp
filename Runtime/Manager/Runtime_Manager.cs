using Common;
using Common.Extensions;
using Common.Struct;
using Common.Timers;
using Devices.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Runtime
{
    public static partial class Runtime_Manager 
    {
        #region Time Constants
        private static readonly TimeSpan CHECK_CYCLE_RATE = TimeSpan.FromSeconds(30);
        private static readonly TimeSpan MAX_WAIT_HANDLE_WAIT_SPAN = TimeSpan.FromSeconds(10);
        #endregion /Time Constants

        #region Update Timer
        private static readonly TickTocker update_TickTocker = new(CHECK_CYCLE_RATE);
        #endregion /Update Timer

        #region Message Boxes
        /// <summary>
        /// This method displays a message box informing the user that the device they are using is not supported.
        /// </summary>
        public static DialogResult ShowDeviceResetDialog(ICommunicatorDevice device)
        {
            String message = String.Format(Translation_Manager.Msg_ResetDevice, device.DisplayName);
            return MessageBox.Show(message, Translation_Manager.Msg_PowerCycle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
        #endregion /Message Boxes

        #region Global Cancellation Token Source
        /// <summary>
        /// This global cancellation token source should be used to cancel any tasks that have no local 
        /// cancel logic, or to cancel operations on program close.
        /// </summary>
        private static readonly CancellationTokenSource globalTokenSource = new();

        private static readonly CancellationToken globalToken = globalTokenSource.Token;

        private static readonly WaitHandle CancellationWaitHandle = globalToken.WaitHandle;
        #endregion /Global Cancellation Token Source

        #region Management Objects

        #region Task Completion Source Containers
        private static readonly TaskCompletionSource<Boolean> finalizeCompletionSource = new();
        #endregion /Task Completion Source Containers

        #region Thread Containers
        private static readonly ISet<Thread> threads = new HashSet<Thread>();
        #endregion /Thread Containers

        #region Cancellation Token Containers
        private static readonly IDictionary<Object, CancellationTokenSource> dictObject_CancellationTokenSource = new ConcurrentDictionary<object, CancellationTokenSource>();
        private static readonly IDictionary<Object, CancellationTokenSource> dictObject_TimeoutCancellationTokenSource = new ConcurrentDictionary<object, CancellationTokenSource>();
        #endregion /Cancellation Token Containers

        #region Semaphore Containers
        private static readonly IDictionary<WaitHandle[], SemaphoreSlim> dictHandles_Semaphore = new ConcurrentDictionary<WaitHandle[], SemaphoreSlim>();
        #endregion /Semaphore Containers

        #region Reset Event Conatiners
        private static readonly ConcurrentStack<ManualResetEvent> resetEventStack = new();
        #endregion /Reset Event Conatiners

        #region Update Loop Containers
        private static readonly IDictionary<Object, ISet<IUpdateLoop>> dictScheduler_UpdateLoops = new ConcurrentDictionary<object, ISet<IUpdateLoop>>();
        #endregion /Update Loop Containers

        #endregion Management Objects

        #region Controls
        /// <summary>
        /// This method will initialize the runtime manager to its started state, which it
        /// should stay in until closing (when Stop is called). 
        /// </summary>
        /// <param name="context"></param>
        public static void Start(SynchronizationContext context)
        {
            Application.ApplicationExit += new EventHandler(Shutdown);
            runTimer.Start();
            Context = context;
            update_TickTocker.Start();
        }

        /// <summary>
        /// Reset should close this devices windows and reset the hardware
        /// </summary>
        public static void Reset()
        {
            try
            {
                Alive = false;
                runTimer.Stop();
                ThreadPool.QueueUserWorkItem(EndRunningProcesses);
                Application.Restart();
            }
            catch
            {
                Log_Manager.IssueDebugAlert("Runtime shutdown stopped prematurely.");
            }
        }
        #endregion /Controls

        #region Update Tracker

        #region Container
        private static readonly ISet<Control> updatingControls = new HashSet<Control>();
        #endregion /Container

        #region Mark
        public static void MarkUpdating(this Control control, Boolean updating = true)
        {
            lock (updatingControls)
            {
                switch (updating)
                {
                    case true:
                        updatingControls.Add(control);
                        return;
                    default:
                        if (updatingControls.Contains(control))
                        {
                            updatingControls.Remove(control);
                        }
                        return;
                }
            }
        }
        #endregion /Mark

        #region Check
        public static Boolean CheckIsUpdating(this Control control)
        {
            lock (updatingControls)
            {
                return updatingControls.Contains(control);
            }
        }
        #endregion /Check

        #endregion /Update Tracker

        #region Alive
        /// <summary>
        /// Boolean representing whether the program is still in its running state.
        /// </summary>
        public static Boolean Alive
        {
            get
            {
                return !globalTokenSource.IsCancellationRequested;
            }
            private set
            {
                if(!value)
                {
                    
                }
            }
        }
        #endregion /Alive

        #region Status
        /// <summary>
        /// Boolean representing the debug state of the program.
        /// </summary>
        public static Boolean Debug
        {
            get
            {
                #pragma warning disable CS0162 // Unreachable code detected
                #if DEBUG
                return true;
                #endif
                return false;
                #pragma warning restore CS0162 // Unreachable code detected
            }
        }
        #endregion /Status

        #region Run Time
        /// <summary>
        /// Keeps track of the total run time of the program.
        /// </summary>
        private static readonly Stopwatch runTimer = new();
        #endregion /Run Time

        #region Main Thread Context
        /// <summary>
        /// The main running context
        /// </summary>
        public static SynchronizationContext Context { get; private set; }
        #endregion Main Thread Context

        #region Cancellation 
        public static WaitHandle GetCancellationWaitHandle(CancellationToken token)
        {
            return GetLinkedTokenSource(token).Token.WaitHandle;
        }

        public static CancellationToken GetToken()
        {
            return globalTokenSource.Token;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static CancellationToken GetToken(this Object _)
        {
            return globalTokenSource.Token;
        }

        public static CancellationToken GetToken(TimeSpan timeout)
        {
            CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(GetToken());
            cts.CancelAfter(timeout);
            return cts.Token;
        }

        public static CancellationToken GetToken<T>(T linkedObject)
        {
            CancellationTokenSource tokenSource = GetTokenSourceCommon(dictObject_CancellationTokenSource, linkedObject);
            return tokenSource.Token;
        }

        public static CancellationToken GetToken<T>(T linkedObject, TimeSpan timeout)
        {
            CancellationTokenSource tokenSource = GetTokenSourceCommon(dictObject_TimeoutCancellationTokenSource, linkedObject);
            tokenSource.CancelAfter(timeout);
            return tokenSource.Token;
        }

        private static CancellationTokenSource GetTokenSourceCommon(IDictionary<Object, CancellationTokenSource> dictObj_TokenSource, Object linkedObject)
        {
            #pragma warning disable IDE0018 // Inline variable declaration
            CancellationTokenSource tokenSource;
            #pragma warning restore IDE0018 // Inline variable declaration
            if (dictObj_TokenSource.TryLookup(linkedObject, out tokenSource))
            {
                if (!tokenSource.IsCancellationRequested)
                {// We dont want to return a cancelled token.
                    return tokenSource;
                }
                Cancel(linkedObject);// This will clean it up.
            }
            tokenSource = CancellationTokenSource.CreateLinkedTokenSource(globalTokenSource.Token);
            dictObj_TokenSource.Add(linkedObject, tokenSource);
            return tokenSource;
        }

        public static CancellationTokenSource GetLinkedTokenSource()
        {
            return CancellationTokenSource.CreateLinkedTokenSource(GetToken());
        }

        public static CancellationTokenSource GetLinkedTokenSource(this Object linkedObject)
        {
            return CancellationTokenSource.CreateLinkedTokenSource(GetToken(linkedObject));
        }

        public static CancellationTokenSource GetTimedTokenSource(TimeSpan waitTime)
        {
            CancellationToken timedToken = new CancellationTokenSource(waitTime).Token;
            return CancellationTokenSource.CreateLinkedTokenSource(GetToken(), timedToken);
        }


        public static CancellationTokenSource GetLinkedTimedTokenSource(Object linkedObject, TimeSpan waitTime)
        {
            CancellationToken timedToken = new CancellationTokenSource(waitTime).Token;
            return CancellationTokenSource.CreateLinkedTokenSource(GetToken(linkedObject), timedToken);
        }

        public static void Cancel()
        {
            globalTokenSource.Cancel();
        }

        public static void Cancel<T>(T linkedObject)
        {
            if (dictObject_CancellationTokenSource.TryLookup(linkedObject, out CancellationTokenSource tokenSource))
            {
                tokenSource.Cancel();
                tokenSource.Dispose();
                dictObject_CancellationTokenSource.Remove(linkedObject);
            }
            if (dictObject_TimeoutCancellationTokenSource.TryLookup(linkedObject, out CancellationTokenSource timedTokenSource))
            {
                timedTokenSource.Cancel();
                timedTokenSource.Dispose();
                dictObject_TimeoutCancellationTokenSource.Remove(linkedObject);
            }
            if (dictScheduler_UpdateLoops.TryLookup(linkedObject, out ISet<IUpdateLoop> updateTimerList))
            {
                ConcurrentBag<IUpdateLoop> remainingTimersBag = new ConcurrentBag<IUpdateLoop>(); 
                foreach(var updateTimer in updateTimerList)
                {
                    if(!updateTimer.Token.IsCancellationRequested)
                    {// This should never be the case, but put it in the just in case bag.
                        remainingTimersBag.Add(updateTimer);
                    }
                }
                dictScheduler_UpdateLoops[linkedObject] = remainingTimersBag.ToHashSet();
            }
        }
        #endregion /Cancellation

        #region Delay 
        /// <summary>
        /// Delay with global cancellation token.
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static async Task Delay(UInt32 milliseconds)
        {
            await Task.Delay(Convert.ToInt32(milliseconds), globalTokenSource.Token);
        }

        /// <summary>
        /// Delay with global cancellation token.
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static async Task Delay_Async(this TimeSpan timeSpan)
        {
            await Task.Delay(timeSpan, globalTokenSource.Token);
        }

        public static void Delay(this TimeSpan timeSpan)
        {
            Task.Delay(timeSpan, globalToken).Wait(globalToken);
        }

        public static void DelayAction(this Control scheduler, MethodInvoker invoker, TimeSpan timeSpan)
        {
            Action action = new Action(() => 
            {
                try
                {
                    if (scheduler.IsHandleCreated)
                    {
                        scheduler.BeginInvoke(invoker);
                    }
                }
                catch
                {
                    action = null;
                }
            });
            MonitorTask(DelayAction_Async(scheduler, action, timeSpan, GetToken(scheduler)));
        }

        public static void DelayAction(this Object scheduler, Action action, TimeSpan timeSpan)
        {
            MonitorTask(DelayAction_Async(scheduler, action, timeSpan, GetToken(scheduler)));
        }

        public static async Task DelayAction_Async(this Object scheduler, Action action, TimeSpan timeSpan)
        {
            await DelayAction_Async(scheduler, action, timeSpan, GetToken(scheduler));
        }

        public static async Task DelayAction_Async(this Object scheduler, Action action, TimeSpan timeSpan, CancellationToken token)
        {
            CancellationToken delayToken = CancellationTokenSource.CreateLinkedTokenSource(token, GetToken(scheduler)).Token;
            await Task.Delay(timeSpan, delayToken);
            if (!delayToken.IsCancellationRequested)
            {
                if (action != null)
                {
                    action.Invoke();
                }
            }
        }

        public static async void DelayAction(this Object scheduler, Action action, TimeSpan timeSpan, CancellationToken token)
        {
            CancellationToken delayToken = CancellationTokenSource.CreateLinkedTokenSource(token, GetToken(scheduler)).Token;
            await Task.Delay(timeSpan, delayToken);
            if (!delayToken.IsCancellationRequested)
            {
                action.Invoke();
            }
        }
        #endregion /Delay

        #region Cancelable
        public static Boolean ScheduleActionTimeout(Object scheduler, TimeSpan timeout, Action action)
        {
            CancellationTokenSource tokenSource = GetLinkedTimedTokenSource(scheduler, timeout);
            CancellationToken token = tokenSource.Token;
            return Task.Run(action.Invoke, token).Wait(Convert.ToInt32(timeout.TotalMilliseconds), token);
        }

        public static CancellationTokenSource ScheduleCancelableAction(this Object scheduler, TimeSpan timeout, Action action)
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            CancellationToken contextToken = GetToken(scheduler);
            Action checkForCancelAction = new Action(async () =>
            {
                try
                {
                    await Task.Delay(timeout, contextToken).ConfigureAwait(true);
                    if (!token.IsCancellationRequested && !contextToken.IsCancellationRequested)
                    {// The outer token wasnt cancelled and we havent requested a global cancellation so we must need to commit to the action
                        action.Invoke();
                    }
                }
                catch { };
            });
            MakeTask(scheduler, checkForCancelAction);
            return tokenSource;
        }

        public static CancellationTokenSource ScheduleCancelableAction(this Object scheduler, TimeSpan timeout, MethodInvoker invoker)
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            CancellationToken contextToken = GetToken(scheduler);
            Action checkForCancelAction = new Action(async () =>
            {
                try
                {
                    await Task.Delay(timeout, contextToken).ConfigureAwait(true);
                    if (!token.IsCancellationRequested && !contextToken.IsCancellationRequested)
                    {// The outer token wasnt cancelled and we havent requested a global cancellation so we must need to commit to the action
                        invoker.Invoke();
                    }
                }
                catch { };
            });
            MakeTask(scheduler, checkForCancelAction);
            return tokenSource;
        }

        public static CancellationTokenSource ScheduleCancelableAction(this Object scheduler, TimeSpan timeout, WaitHandle cancelHandle, Action action)
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            CancellationToken contextToken = GetToken(scheduler);
            Action checkForTimeoutAction = new Action(() =>
            {
                try
                {
                    cancelHandle.WaitOne(timeout, true);
                    if (!token.IsCancellationRequested && !contextToken.IsCancellationRequested)
                    {// The outer token wasnt cancelled and we havent requested a global cancellation so we must need to commit to the action
                        action.Invoke();
                    }
                }
                catch { };
            });
            MakeTask(scheduler, checkForTimeoutAction);
            return tokenSource;
        }

        public static CancellationTokenSource ScheduleCancelableAction(this Form scheduler, TimeSpan timeout, MethodInvoker invoker)
        {
            CancellationTokenSource tokenSource = GetLinkedTokenSource();
            CancellationToken token = tokenSource.Token;
            Action checkForCancelAction = new Action(async () =>
            {
                try
                {
                    await Task.Delay(timeout, token);
                    if (!token.IsCancellationRequested)
                    {// The outer token wasnt cancelled and we havent requested a global cancellation so we must need to commit to the action
                        scheduler.Invoke(invoker);
                    }
#if DEBUG
                    else
                    {
                        Console.WriteLine("Cancelled Action");
                    }
#endif
                }
                catch { };
            });
            MakeTask(scheduler, checkForCancelAction);
            return tokenSource;
        }

        public static CancellationTokenSource ScheduleCancelableAction<T>(Object scheduler, TimeSpan timeout, Action<T> action, T paramArray)
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            CancellationToken contextToken = GetToken(scheduler);
            Action checkForCancelAction = new Action(async () =>
            {
                try
                {
                    await Task.Delay(timeout, contextToken);
                    if (!token.IsCancellationRequested && !contextToken.IsCancellationRequested)
                    {// The outer token wasnt cancelled and we havent requested a global cancellation so we must need to commit to the action
                        action.Invoke(paramArray);
                    }
                }
                catch { };
            });
            MakeTask(scheduler, checkForCancelAction); 
            return tokenSource;
        }

        public static CancellationTokenSource ScheduleCancelableAction<T>(Object scheduler, TimeSpan timeout, Action<T[]> action, params T[] paramArray)
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            CancellationToken contextToken = GetToken(scheduler);
            Action checkForCancelAction = new Action(async () =>
            {
                await Task.Delay(timeout, contextToken);
                if (!token.IsCancellationRequested && !contextToken.IsCancellationRequested)
                {// The outer token wasnt cancelled and we havent requested a golbal cancellation so we must need to commit to the action
                    action.Invoke(paramArray);
                }
            });
            MakeTask(scheduler, checkForCancelAction);
            return tokenSource;
        }
        #endregion /Cancelable

        #region Action Factory
        public static Action MakeInvocation(this Control actor, MethodInvoker invoker)
        {
            return new Action(() => { actor.BeginInvoke(invoker); });
        }
        #endregion /Action Factory

        #region Task Factory
        /// <summary>
        /// Add this task to the runtime monitor to track its completion status, 
        /// and to allow for it to be closed on program close.
        /// </summary>
        /// <param name="task"></param>
        public static void MakeTask(this Action action)
        {
            TaskStruct.AddTasks(Task.Run(action, GetToken()));
        }

        public static Task ProduceTask(this Action action)
        {
            Task task = Task.Run(action, GetToken());
            TaskStruct.AddTasks(task);
            return task;
        }

        public static Task ProduceTask_Null()
        {
            return new Task(new Action(() => { }));
        }

        public async static void MakeTask(Object scheduler, Action action)
        {
            try
            {
                Task task;
                if (scheduler is Control control)
                {
                    task = Task.Run(() => { control.BeginInvoke(action); }, GetToken(scheduler));
                }
                else
                {
                    task = Task.Run(action, GetToken(scheduler));
                }
                TaskStruct.AddTasks(task);
                await task;
            }
            catch (Exception ex)
            {
                Log_Manager.LogError(nameof(MakeTask), $"MakeTask encountered exception {ex.Message}");
            }
        }

        public static void MakeTaskWithContinuation(this Control actor, Action action, params MethodInvoker[] invokeOnComplete)
        {
            Task task = Task.Run(action, GetToken());
            MonitorTask(task);
            MonitorTask(task.ContinueWith((t) =>
            {
                foreach (MethodInvoker invoker in invokeOnComplete)
                {
                    try
                    {
                        actor.BeginInvoke(invoker);
                    }
                    catch(Exception ex)
                    {
                        Log_Manager.LogError(nameof(MakeTaskWithContinuation_Async), ex.Message);
                    }
                }
            }, globalToken));
        }

        public static async void MakeTaskWithContinuation_Async<T>(this Control actor, Func<T> function, params Action<T>[] invokeOnComplete)
        {
            Task<T> task = Task.Run(function, GetToken());
            MonitorTask(task);
            T result = await task;
            foreach (Action<T> invoker in invokeOnComplete)
            {
                try
                {
                    actor.BeginInvoke(invoker, result);
                }
                catch (Exception ex)
                {
                    Log_Manager.LogError(nameof(MakeTaskWithContinuation_Async), ex.Message);
                }
            }
        }

        /// <summary>
        /// Add this task to the runtime monitor to track its completion status, 
        /// and to allow for it to be closed on program close.
        /// </summary>
        /// <param name="task"></param>
        public static void MakeTask<T>(Func<T> function)
        {
            TaskStruct.AddTasks(Task.Run(function, GetToken()));
        }

        public static void MakeTask<T>(Object scheduler, Func<T> function)
        {
            TaskStruct.AddTasks(Task.Run(function, GetToken(scheduler)));
        }

        public static async Task MakeAwait_Tasks(Task[] tasks, Boolean configureAwait = false)
        {
            await Task.WhenAny(Task.WhenAll(tasks), finalizeCompletionSource.Task).ConfigureAwait(configureAwait);
        }

        public static T TaskResult<T>(this Task<T> task)
        {
            TaskCompletionSource<T> completionSource = new TaskCompletionSource<T>();
            MakeTask(async () =>
            {
                completionSource.SetResult(await task);
            });
            return completionSource.Task.ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// This method will wrap an action into a managed Task that is awaitable.
        /// If configure await is given as true, this task context will be used on contunation
        /// after the task is complete. This is not suatable for UI so by default this is false.
        /// </summary>
        /// <param name="action">The method to run in an awaitable thread off the main thread.</param>
        /// <param name="configureAwait">If true this will keep the task thread context for the remaining code run
        /// after this method to run in this context. If false, the method calling this will return to
        /// its previous context to run its remaining code.</param>
        /// <returns>An awaitable task that will run the method action.</returns>
        public static async Task MakeAwait(Action action, Boolean configureAwait = false)
        {
            Task awaitTask = Task.Run(action, GetToken());
            TaskStruct.AddTasks(awaitTask);
            await awaitTask.ConfigureAwait(configureAwait);
        }

        /// <summary>
        /// This method will wrap an action into a managed Task that is awaitable.
        /// If configure await is given as true, this task context will be used on contunation
        /// after the task is complete. This is not suatable for UI so by default this is false.
        /// </summary>
        /// <param name="caller">The object calling the method on which to register the cancellation token.</param>
        /// <param name="action">The method to run in an awaitable thread off the main thread.</param>
        /// <param name="configureAwait">If true this will keep the task thread context for the remaining code run
        /// after this method to run in this context. If false, the method calling this will return to
        /// its previous context to run its remaining code.</param>
        /// <returns>An awaitable task that will run the method action.</returns>
        public static async Task MakeAwait(this Object caller, Action action, Boolean configureAwait = false)
        {
            Task awaitTask = Task.Run(action, GetToken(caller));
            TaskStruct.AddTasks(awaitTask);
            await awaitTask.ConfigureAwait(configureAwait);
        }

        /// <summary>
        /// This method will 
        /// </summary>
        /// <param name="action"></param>
        public static void Wait(Action action)
        {
            Task.Run(action, GetToken()).Wait(GetToken());
        }

        public static int WaitAny(this Task[] tasks)
        {
            return Task.WaitAny(tasks, GetToken());
        }

        public static void WaitAll(this Task[] tasks)
        {
            Task.WaitAll(tasks, GetToken());
        }

        public static async Task AwaitAll(this Task[] tasks, Boolean configureAwait = false)
        {
            try
            {
                await Task.Run(() => Task.WaitAll(tasks, GetToken())).ConfigureAwait(configureAwait);
            }
            catch(TaskCanceledException tcex)
            {
                Log_Manager.LogDebug(nameof(AwaitAll), tcex.Message);
            }
            catch (AggregateException aex)
            {
                if (aex.InnerException is TaskCanceledException)
                {
                    Log_Manager.LogDebug(nameof(AwaitAll), aex.Message);
                }
                else
                {
                    throw aex;
                }
            }
        }

        public static async Task<T> MakeAwait<T>(Func<T> function, Boolean configureAwait = false)
        {
            return await Task.Run(function, GetToken()).ConfigureAwait(configureAwait);
        }

        public static T Wait<T>(Func<T> function)
        {
            return Task.Run(function, GetToken()).Result;
        }

        /// <summary>
        /// This method will make a task that will run after some delay when the trigger is set.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="trigger"></param>
        public static async Task<Boolean> AwaitTrigger(this Object caller, ManualResetEvent trigger, TimeSpan triggerWait_span, Int32 delay_ms = 0)
        {
            Func<TimeSpan, bool> waitFunction = new Func<TimeSpan, bool>(trigger.WaitOne);
            resetEventStack.Push(trigger);
            Task<bool> awaitTask = Task.Run(() =>
            {
                bool success = false;
                try
                {
                    success = waitFunction.Invoke(triggerWait_span);
                    Task.Delay(delay_ms, GetToken(caller)).Wait(GetToken(caller));
                }
                catch (Exception ex)
                {
                    Log_Manager.LogCaughtException(ex);
                }; 
                return success;
            });
            TaskStruct.AddTasks(awaitTask);
            return await awaitTask;
        }


        /// <summary>
        /// This method will make a task that will run after some delay when the trigger is set.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="trigger"></param>
        public static void MakeTriggerTask(Action action, TimeSpan delay, ref ManualResetEvent trigger)
        {
            resetEventStack.Push(trigger);
            Func<bool> waitFunction = new Func<bool>(trigger.WaitOne);
            TaskStruct.AddTasks(Task.Run(() =>
            {
                try
                {
                    waitFunction.Invoke();
                    Task.Delay(delay, GetToken()).Wait(GetToken());
                    action.Invoke();
                }
                catch (Exception ex)
                {
                    Log_Manager.LogCaughtException(ex);
                };
            }));
        }

        #endregion /Task Factory

        #region Monitor
        /// <summary>
        /// Add this task to the runtime monitor to track its completion status, 
        /// and to allow for it to be closed on program close.
        /// </summary>
        /// <param name="task"></param>
        public static void MonitorTask(this Task task)//TODO: Require its cancellation token or run it here with this one
        {
            TaskStruct.AddTasks(task);
        }

        /// <summary>
        /// Add this task to the runtime monitor to track its completion status, 
        /// and to allow for it to be closed on program close.
        /// </summary>
        /// <param name="task"></param>
        public static void MonitorTask(params Task[] tasks)//TODO: Require its cancellation token or run it here with this one
        {
            TaskStruct.AddTasks(tasks);
        }

        public static async void MonitorTaskWithFollowingActions_Async<T>(this Control actor, Func<Task<T>> function, params Action<T>[] invokeOnComplete)
        {
            T result = await function.Invoke();
            foreach (Action<T> invoker in invokeOnComplete)
            {
                try
                {
                    actor.BeginInvoke(invoker, result);
                }
                catch (Exception ex)
                {
                    Log_Manager.LogError(nameof(MonitorTaskWithFollowingActions_Async), ex.Message);
                }
            }
        }

        /// <summary>
        /// Add this thread to the runtime monitor to track its completion status, 
        /// and to allow for it to be closed on program close.
        /// </summary>
        /// <param name="thread"></param>
        public static void MonitorThreadStart(Thread thread)
        {
            if (thread != null)
            {
                if (!thread.IsAlive)
                {
                    thread.Start();
                }
                MonitorThread(thread);
            }
        }

        /// <summary>
        /// Add this thread to the runtime monitor to track its completion status, 
        /// and to allow for it to be closed on program close.
        /// </summary>
        /// <param name="amThread"></param>
        public static void MonitorThreadStart(AM_Thread amThread)
        {
            if (amThread != null)
            {
                if (Alive && !amThread.Thread.IsAlive)
                {
                    amThread.Start();
                }
                MonitorThread(amThread.Thread);
            }
        }

        /// <summary>
        /// Add this thread to the runtime monitor to track its completion status, 
        /// and to allow for it to be closed on program close.
        /// </summary>
        /// <param name="thread"></param>
        public static void MonitorThread(Thread thread)
        {
            if (thread != null)
            {
                lock (threads)
                {
                    threads.Add(thread);
                }
            }
        }
        #endregion /Monitor

        #region Process Tracker
        private static void ProcessTracker()
        {
            object[] linkedObjects;
            Task[] _tasks;
            Thread[] _threads;
            ConcurrentBag<IUpdateLoop> completedUpdateTimers = new ConcurrentBag<IUpdateLoop>();
            ConcurrentBag<Task> completedTasks = new ConcurrentBag<Task>();
            ConcurrentBag<Thread> completedThreads = new ConcurrentBag<Thread>();
            // Cancellation
            linkedObjects = dictObject_CancellationTokenSource.Keys.ToArray();
            foreach(var linkedObject in linkedObjects)
            {
                if (linkedObject is Control control)
                {
                    if (control.IsDisposed)
                    {// If the form is displosed, cancel any processes
                        Cancel(control);
                    }
                }
            }
            try
            {
                #region Update Timers
                try
                {
                    lock (dictScheduler_UpdateLoops)
                    {
                        foreach(var updateTimers in dictScheduler_UpdateLoops.Values)
                        {
                            if (updateTimers != null)
                            {
                                for(int ut = 0; ut < updateTimers.Count; ut++)// ut0, read: ut-oh
                                {
                                    IUpdateLoop updateTimer = updateTimers.ElementAt(ut);
                                    try
                                    {
                                        if (updateTimer == null || !updateTimer.IsAlive)
                                        {
                                            completedUpdateTimers.Add(updateTimer);
                                            updateTimer.Dispose();
                                            Log_Manager.LogAssert(nameof(ProcessTracker), "An update timer is running and not cancelled by the global cancellation source.");
                                        }
                                    }
                                    finally
                                    {
                                        updateTimer = null;
                                    }
                                }
                                updateTimers.ExceptWith(completedUpdateTimers);
                            }
                        }
                    }
                }
                finally
                {
                    
                }
                #endregion /Update Timers

                #region Tasks

                _tasks = TaskStruct.Tasks;
                
                foreach(var task in _tasks)
                {
                    if (task == null)
                    {
                        completedTasks.Add(task);
                    }
                    else if (task.IsCompleted)
                    {// If the task is done, we can stop tracking it.
                        completedTasks.Add(task);
                        task.Dispose();
                    }
                }
                lock (_tasks)
                {
                    TaskStruct.ExceptWith(completedTasks.ToHashSet());
                }
                #endregion /Tasks

                #region Threads
                lock (threads)
                {
                    _threads = threads.ToArray();
                }
                for(uint t = 0; t < _threads.Length; t++)
                {
                    Thread thread = _threads[t];
                    if (thread == null)
                    {
                        completedThreads.Add(thread);
                    }
                    else if (!thread.IsAlive)
                    {// If the thread has completed, we can stop tracking it.
                        completedThreads.Add(thread);
                        thread.Join(100);// Already reported as completed so this should never wait.
                        thread = null;
                    }
                }
                lock (threads)
                {
                    threads.ExceptWith(completedThreads.ToHashSet());
                }
                #endregion /Threads
            }
            catch (Exception ex)
            {
                Log_Manager.IssueAlert(ex);
            }
        }
        #endregion /Process Tracker

        #region Close
        private static void EndRunningProcesses(Object _)
        {
            #region Global Cancellation
            if (!globalTokenSource.IsCancellationRequested)
            {
                globalTokenSource.Cancel();
            }
            #endregion /Global Cancellation

            #region Reset Events
            while (resetEventStack.TryPop(out ManualResetEvent resetEvent))
            {
                resetEvent.Set();
            }
            resetEventStack.Clear();// Just in case
            #endregion /Reset Events

            #region Update Timers
            try
            {
                lock (dictScheduler_UpdateLoops)
                {
                    foreach(ISet<IUpdateLoop> updateTimers in dictScheduler_UpdateLoops.Values)
                    {
                        if (updateTimers != null)
                        {
                            for(int ut = 0; ut < updateTimers.Count; ut++)
                            {
                                IUpdateLoop updateTimer = updateTimers.ElementAt(ut);
                                try
                                {
                                    if (updateTimer != null && updateTimer.IsAlive)
                                    {
                                        updateTimer.Dispose();
                                        Log_Manager.LogAssert(nameof(EndRunningProcesses), "An update timer is running and not cancelled by the global cancellation source.");
                                    }
                                }
                                finally
                                {
                                    updateTimer = null;
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                dictScheduler_UpdateLoops.Clear();
            }
            #endregion /Update Timers

            #region Tasks
            TaskStruct.Clear();
            #endregion /Tasks

            #region Threads
            try
            {
                for(int t = 0; t < threads.Count; t++)
                {
                    Thread thread = threads.ElementAt(t);
                    try
                    {
                        if (thread != null && thread.IsAlive)
                        {
                            thread.Interrupt();
                        }
                    }
                    finally
                    {
                        thread = null;
                    }
                }
            }
            finally
            {
                threads.Clear();
            }
            #endregion /Threads
        }
        #endregion /Close

        #region Run Timers
        public static UInt64 GetRunTimeS()
        {
            return Convert.ToUInt64(runTimer.ElapsedMilliseconds / 1000);
        }

        public static UInt64 GetRunTimeMs()
        {
            return Convert.ToUInt64(runTimer.ElapsedMilliseconds);
        }

        public static TimeSpan GetRunTime()
        {
            return runTimer.Elapsed;
        }
        #endregion /Run Timers

        #region Task Awaiters
        public static async Task<Boolean> WaitForAllEvents(TimeSpan timeoutSpan, params WaitHandle[] waitHandles)
        {
            bool result;
            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
            if (waitHandles == null || !waitHandles.Any())
            {
                Action waitAction = new Action(() =>
                {
                    result = WaitHandle.WaitAll(waitHandles, timeoutSpan, false);
                    taskCompletionSource.SetResult(result);
                });
                MakeTask(waitAction);
            }
            else
            {
                taskCompletionSource.SetResult(true);// Nothing to wait for...
            }
            return await taskCompletionSource.Task;
        }

        public static Int32 WaitForAnyEvent(TimeSpan timeoutSpan, params WaitHandle[] waitHandles)
        {
            int result = waitHandles.Length;
            Array.Resize(ref waitHandles, waitHandles.Length + 1);
            waitHandles[result] = CancellationWaitHandle;// Don't worry, if this is signaled we dont need concern ourselves with the function returning something strange... the program is closing.
            return WaitHandle.WaitAny(waitHandles, timeoutSpan, true);
        }

        public static async Task<Int32> WaitForAnyEvent_Async(TimeSpan timeoutSpan, params WaitHandle[] waitHandles)
        {
            int result = waitHandles.Length;
            Array.Resize(ref waitHandles, waitHandles.Length + 1);
            waitHandles[result] = CancellationWaitHandle;// Don't worry, if this is signaled we dont need concern ourselves with the function returning something strange... the program is closing.
            TaskCompletionSource<int> taskCompletionSource = new TaskCompletionSource<int>();
            if (waitHandles == null || waitHandles.Any())
            {
                Action waitAction = new Action(() =>
                {
                    result = WaitHandle.WaitAny(waitHandles, timeoutSpan, false);
                    taskCompletionSource.SetResult(result);
                });
                MakeTask(waitAction);
            }
            else
            {
                taskCompletionSource.SetResult(result);// Nothing to wait for...
            }
            return await taskCompletionSource.Task;
        }
        #endregion /Task Awaiters

        #region Shutdown
        public static void Shutdown(Object sender, EventArgs e)
        {
            Log_Manager.LogAssert("Manager_Runtime", "Shutdown Commanded.");
            try
            {
                finalizeCompletionSource.SetResult(false);
                ThreadPool.QueueUserWorkItem(EndRunningProcesses);
                Alive = false;
                runTimer.Stop();
            }
            catch
            {
                Log_Manager.IssueDebugAlert("Runtime shutdown stopped prematurely.");
            }
        }

        [System.Runtime.InteropServices.LibraryImport("Kernel32")]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        private static partial Boolean CloseHandle(IntPtr handle);
        #endregion /Shutdown
    }
}

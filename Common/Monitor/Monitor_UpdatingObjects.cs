using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common.Monitor
{
    public class Monitor_UpdatingObjects<T> : IDisposable, IIdentifiable 
    {
        #region Identity
        public const String ClassName = nameof(Monitor_UpdatingObjects<T>);
        public virtual String Identity
        {
            get
            {
                return ClassName;
            }
        }
        #endregion /Identity

        #region Readonly
        private readonly HashSet<T> objectsAwaitingUpdate = new();
        #endregion /Readonly

        #region Check Method
        /// <summary>
        /// This helper method checks to see if the control should be 
        /// updated this cycle. 
        /// </summary>
        /// <param name="control">Control to be checked</param>
        /// <returns></returns>
        public virtual bool CanScheduleUpdate(T control)
        {
            bool containsControl = objectsAwaitingUpdate.Contains(control);
            return !containsControl;
        }
        #endregion /Check Method

        #region Schedule Methods

        #region Synchronous
        public void Update(T @object, MethodInvoker updateMethod)
        {
            try
            {
                objectsAwaitingUpdate.Add(@object);// Set wont allow doubles.
                updateMethod.Invoke();
            }
            finally
            {
                objectsAwaitingUpdate.Remove(@object);
            }
        }
        #endregion /Synchronous

        #region Task
        public async Task AwaitUpdate_Async(T @object, Task awaitTask)
        {
            try
            {
                objectsAwaitingUpdate.Add(@object);// Set wont allow doubles.
                await awaitTask.ConfigureAwait(true);
            }
            finally
            {
                objectsAwaitingUpdate.Remove(@object);
            }
        }

        public async Task<bool> AwaitUpdate_Async(T @object, Task<bool> awaitTask)
        {
            try
            {
                objectsAwaitingUpdate.Add(@object);// Set wont allow doubles.
                return await awaitTask.ConfigureAwait(true);
            }
            finally
            {
                objectsAwaitingUpdate.Remove(@object);
            }
        }
        #endregion /Task

        #region Function
        public async Task<bool> AwaitUpdate_Func(T @object, Func<bool> awaitFunction)
        {
            try
            {
                objectsAwaitingUpdate.Add(@object);
                return await Task.Run(awaitFunction).ConfigureAwait(true);
            }
            finally
            {
                objectsAwaitingUpdate.Remove(@object);
            }
        }
        #endregion /Function

        #endregion /Schedule Methods

        #region Dispose
        public void Dispose()
        {
            objectsAwaitingUpdate.Clear();
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Struct
{
    public readonly struct TaskStruct
    {
        #region Static Readonly
        private static readonly ISet<Task> tasks = new HashSet<Task>();
        #endregion /Static Readonly

        #region Accessors
        public static Task[] Tasks
        {
            get
            {
                lock (tasks)
                {
                    return tasks.ToArray();
                }
            }
        }
        #endregion /Accessors

        #region Add
        public static void AddTasks(params Task[] newTasks)
        {
            lock (tasks)
            {
                foreach (Task newTask in newTasks)
                {
                    tasks.Add(newTask);
                }
            }
        }
        #endregion /Add

        #region Except With (ISet)

        public static void ExceptWith(ISet<Task> otherTasks)
        {
            lock (tasks)
            {
                tasks.ExceptWith(otherTasks);
            }
        }
        #endregion /Except With (ISet)

        #region Clear
        public static void Clear()
        {
            lock (tasks)
            {
                for (int t = 0; t < Tasks.Length; t++)
                {
                    Task task = Tasks[t];
                    try
                    {
                        if (task != null && !task.IsCompleted && !task.IsCanceled)
                        {
                            task.Dispose();
                        }
                    }
                    catch (Exception) { } // Nothing needed. 
                    finally
                    {
                        task = null;
                    }
                }
                tasks.Clear();
            }
        }
        #endregion /Clear
    }
}

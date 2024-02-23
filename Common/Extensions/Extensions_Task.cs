using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static class Extensions_Task
    {
        /// <summary>
        /// Wrapper for Task that will await then retrun to the original thread.
        /// </summary>
        /// <param name="task">Task to await on.</param>
        /// <returns>The awaiter. It awaits.</returns>
        public static ConfiguredTaskAwaitable ConfigureAwaitOnThis(this Task task)
        {
            return task.ConfigureAwait(true);
        }

        /// <summary>
        /// Wrapper for Task that will await then retrun to the original thread.
        /// </summary>
        /// <typeparam name="T">The tasks return type.</typeparam>
        /// <param name="task">Task to await on.</param>
        /// <returns>The awaiter. It awaits.</returns>
        public static ConfiguredTaskAwaitable<T> ConfigureAwaitOnThis<T>(this Task<T> task)
        {
            return task.ConfigureAwait(true);
        }

        /// <summary>
        /// Wrapper for Task that will await then retrun to the original thread.
        /// </summary>
        /// <typeparam name="T">The tasks return type.</typeparam>
        /// <returns>The awaiter. It awaits.</returns>
        public static ConfiguredTaskAwaitable ConfigureAwaitOnCapured(this Task task)
        {
            return task.ConfigureAwait(false);
        }

        /// <summary>
        /// Wrapper for Task that will await then retrun to the original thread.
        /// </summary>
        /// <typeparam name="T">The tasks return type.</typeparam>
        /// <param name="task">Task to await on.</param>
        /// <returns>The awaiter. It awaits.</returns>
        public static ConfiguredTaskAwaitable<T> ConfigureAwaitOnCapured<T>(this Task<T> task)
        {
            return task.ConfigureAwait(false);
        }
    }
}

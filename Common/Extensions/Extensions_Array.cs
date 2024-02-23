using System;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static class Extensions_Array
    {
        #region Identity
        public const String ClassName = nameof(Extensions_Array);
        #endregion

        #region Resize
        /// <summary>
        /// Resize the queue so it can accept more nodes.  All currently enqueued nodes are remain.
        /// Attempting to decrease the queue size to a size too small to hold the existing nodes results in undefined behavior
        /// O(n)
        /// </summary>
        public static void Resize<T>(this T[] array, int maxNodes = 1)
        {
#if DEBUG
            if (maxNodes <= 0)
            {
                throw new InvalidOperationException("Queue size cannot be smaller than 1");
            }
            if (maxNodes < array.Length)
            {
                throw new InvalidOperationException("Called Resize(" + maxNodes + "), but current queue contains " + array.Length + " nodes");
            }
#endif
            T[] newArray = new T[maxNodes + 1];
            int highestIndexToCopy = Math.Min(maxNodes, array.Length);
            Array.Copy(array, newArray, highestIndexToCopy + 1);
            array = newArray;
        }


        #endregion /Resize

        #region Migrate
        public static void Migrate<T>(this T[] copyArray, ref T[] newArray)
        {
            if (copyArray != null)
            {// We should copy the old data
                for (uint td = 1; td <= newArray.Length; td++)
                {
                    if (copyArray.Length - td >= 0)
                    {
                        newArray[newArray.Length - td] = copyArray[copyArray.Length - td];
                    }
                }
            }
        }
        #endregion /Migrate

        #region Remove
        public static T[] ArrayRemoveResize<T>(this T[] array, int removeIndex)
        {
            return array.Where((source, index) => index != removeIndex).ToArray();
        }
        #endregion /Remove

        #region Swap
        public static void Swap<T>(this T[] container, uint index1, uint index2)
        {
            if(index1 < container.Length && index2 < container.Length)
            {
                T temp = container[index1];
                container[index1] = container[index2];
                container[index2] = temp;
            }
        }
        #endregion /Swap

        #region Concatenate
        public static T[] Concatenate<T>(this T[] first, T[] second)
        {
            if (first == null)
            {
                return second;
            }
            if (second == null)
            {
                return first;
            }
            return first.Concat(second).ToArray();
        }
        #endregion /Concatenate

        #region Load
        /// <summary>
        /// This method adds a value to the array of known values
        /// associated with a paramInfo.
        /// </summary>
        /// <param name="value">Value to add to the array</param>
        /// <param name="parameter">Associated ParamInfo used as key value</param>
        public static void LoadValue<T>(this T[] array, T value)
        {
            if (array != null && array.Any())
            {
                int size = array.Length - 1;
                for (int at = 0; at < size; at++)
                {
                    array[at] = array[at + 1];
                }
                array[size] = value;
            }
        }
        #endregion /Load

        #region Fill

        public static void ParallelFill<T>(this T[] array, T value)
        {
            if (array == null)
            {
                throw new ArgumentNullException("Array");
            }
            Parallel.For(0, array.Length, (e) =>
            {
                array[e] = value;
            });
        }

        public static void Fill<T>(this T[] array, T value)
        {
            if (array == null)
            {
                throw new ArgumentNullException("Array");
            }
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }
        #endregion /Fill

    }
}

using System;

namespace Common
{
    public static class Utilites_Search
    {
        #region Binary Search
        /// <summary>
        /// This method takes in an array array and a target. The method uses a while loop to repeatedly divide
        /// the portion of the array being searched in half until the target is found or it is clear that the target is not present 
        /// in the array. The index of the target element is returned if found, or -1 is returned if the target is not found.
        /// </summary>
        /// <typeparam name="T">Type of the values to search through.</typeparam>
        /// <param name="arr">Array of elements to search through for the target.</param>
        /// <param name="target">The target of our search.</param>
        /// <returns>Index of the found value in the array.</returns>
        public static int BinarySearch<T>(T[] arr, T target) where T : IComparable<T>
        {
            int left = 0;
            int right = arr.Length - 1;
            while (left <= right)
            {
                int middle = (left + right) / 2;
                if (arr[middle].CompareTo(target) == 0)
                {
                    return middle;
                }
                else if (arr[middle].CompareTo(target) < 0)
                {
                    left = middle + 1;
                }
                else
                {
                    right = middle - 1;
                }
            }
            return -1;
        }

        /// <summary>
        /// This method uses binary search to see if the array contains the traget value 
        /// of boolean for later logic arguments so all elements may not need to be searched. 
        /// Worst case is O(n);
        /// </summary>
        /// <param name="arr">Array of elements to search through for the target.</param>
        /// <param name="target">The target of our search.</param>
        /// <returns></returns>
        public static bool BinarySearch_Boolean(bool[] arr, bool target)
        {
            int left = 0;
            int right = arr.Length - 1;
            while (left <= right)
            {
                int middle = (left + right) / 2;
                if (arr[middle] == target)
                {
                    return true;
                }
                else if (arr[middle].CompareTo(target) < 0)
                {
                    left = middle + 1;
                }
                else
                {
                    right = middle - 1;
                }
            }
            return false;
        }
        #endregion
    }
}

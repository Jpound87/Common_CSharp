using System;

namespace Common.Utility
{
    public static class Utility_Sort
    {
        #region Identity
        public const String FormName = nameof(Utility_Sort);
        #endregion

        #region Merge Sort
        /// <summary>
        /// This method takes in an array of Controls and 
        /// recursively sorts them from by width from 
        /// smallest to largest
        /// -=CAH=-
        /// </summary>
        /// <param name="input">Array of controls to sort through</param>
        /// <param name="left">Lowest/starting index (usually zero)</param>
        /// <param name="right">Highest index (last element of the array)</param>
        /// <returns></returns>
        public static T[] MergeSort<T>(this T[] input, int left, int right) where T : IComparable<T>
        {
            if (left < right)
            {
                int middle = (left + right) / 2; // Divide
                input.MergeSort(left, middle); // Conquer
                input.MergeSort(middle + 1, right);
                input.Merge(left, middle, right); // Combine
            }
            return input;
        }

        /// <summary>
        /// This method takes in an array of Controls, left, right, and middle values,
        /// and returns a sorted Control array using Linear-Time Merging
        /// </summary>
        /// <param name="input"></param>
        /// <param name="left"></param>
        /// <param name="middle"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static T[] Merge<T>(this T[] input, int left, int middle, int right) where T : IComparable<T>
        {
            // Create two sorted subarrays (n1 and n2) from input array
            // n1 will contain elements of input[left, middle]
            // n2 will contain elements of input[middle + 1, right]
            T[] bottomHalf = new T[middle - left + 1];
            T[] topHalf = new T[right - middle];

            Array.Copy(input, left, bottomHalf, 0, middle - left + 1);
            Array.Copy(input, middle + 1, topHalf, 0, right - middle);

            int i = 0; // tracks the subsequent element of bottomHalf that remains uncopied back into the array
            int j = 0; // tracks the subsequent element of topHalf that remains uncopied back into the array

            // Compares the two subarrays to reorder input[] to go from smallest to largest
            for (int k = left; k < right + 1; k++)
            {
                if (i == bottomHalf.Length)
                {
                    input[k] = topHalf[j];
                    j++;
                }
                else if (j == topHalf.Length)
                {
                    input[k] = bottomHalf[i];
                    i++;
                }
                else if (bottomHalf[i].CompareTo(topHalf[j]) <= 0) // Compares next uncopied elements of subarrays
                {
                    // if next element in bottomHalf array is smaller, it gets copied to next index of input
                    input[k] = bottomHalf[i]; 
                    i++;
                }
                else
                {
                    // else, next element of topHalf gets copied
                    input[k] = topHalf[j];
                    j++;
                }
            }
            return input;
        }
        #endregion
    }
}

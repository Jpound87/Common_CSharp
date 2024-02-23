using Common.Extensions;
using System;

namespace Common.Utility
{
    public static class Utility_Array
    {
        #region Identity
        public const String ClassName = nameof(Utility_Array);
        #endregion

        #region Create
        public static T[] CreateFilledArray<T>(int size, T initialValue)
        {
            T[] newArray = new T[size];
            newArray.Fill(initialValue);
            return newArray;
        }
        #endregion

        #region Resize
        public static void ResizeContainerArrays<T>(uint newSize, ref T[] array, Action<uint> addAction = null, Action<uint> removeAction = null)
        {
            if (newSize == array.Length)
            {
                return;
            }
            T[] newArray = new T[newSize];
            if (array.Length > newSize)
            {// We have more containers than we want, so we need to remove some.

                for (int ns = 0; ns < newSize; ns++)
                {
                    newArray[ns] = array[ns];
                }
                if (removeAction != null)
                {
                    for (uint ns = newSize; ns < array.Length; ns++)
                    {// Perform remove actions.
                        removeAction(ns);
                    }
                }
                array = newArray;
            }
            else
            {// We need to add some containers to get to the number we want in total.
                uint lenArray = Convert.ToUInt32(array.Length);
                if (array != null)
                {// We need to move over the old containers.
                    for (int a = 0; a < array.Length; a++)
                    {
                        newArray[a] = array[a];
                    }
                }
                array = newArray;
                if (addAction != null)
                {
                    for (uint ns = lenArray; ns < newSize; ns++)
                    {// Perform add actions. 
                        addAction(ns);
                    }
                }
            }
        }
        #endregion /Resize 
    }
}

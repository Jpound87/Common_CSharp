using System.Collections.Concurrent;

namespace Core
{
    public enum AccessorUpdateResult : byte
    {
        Updated,
        Equal,
        Null,
        Error
    }
    /// <summary>
    /// This is the library of static utility functions that require no project references.
    /// </summary>
    public static class Utility
    {
        #region Accessor Helpers
        public static AccessorUpdateResult AccessorUpdate<T>(T updateValue, ref T memoryValue)
        {
            if (updateValue != null && memoryValue != null)
            {
                try
                {
                    if (!memoryValue.Equals(updateValue))
                    {
                        memoryValue = updateValue;
                        return AccessorUpdateResult.Updated;
                    }
                    return AccessorUpdateResult.Equal;
                }
                catch
                {
                    return AccessorUpdateResult.Error;
                }
            }
            return AccessorUpdateResult.Null;
        }

        public static AccessorUpdateResult TriggerOnAccessorUpdate<T>(this Action updateAction, T updateValue, ref T memoryValue) 
        {
            if (updateValue != null && memoryValue != null)
            {
                try
                {
                    if (!memoryValue.Equals(updateValue))
                    {
                        memoryValue = updateValue;
                        updateAction.Invoke();
                        return AccessorUpdateResult.Updated;
                    }
                    return AccessorUpdateResult.Equal;
                }
                catch 
                {
                    return AccessorUpdateResult.Error;
                }
            }
            return AccessorUpdateResult.Null;
        }
        #endregion Accessor Helpers

        #region Thread
        public static bool Abort(Thread thread)
        {
            return thread.ThreadState.Equals(ThreadState.AbortRequested);
        }
        #endregion

        #region Dictionary
        /// <summary>
        /// This method allows for safe dictionary lookups by wrappping the IDictionary 
        /// inheriting class in a try-catch wrapper that checks for the key first
        /// before attempting lookup. 
        /// </summary>
        /// <typeparam name="T1">Dictionay key type</typeparam>
        /// <typeparam name="T2">Dictionary value type</typeparam>
        /// <param name="dict">The dictionatry in which to lookup a value</param>
        /// <param name="key">The key value to look up</param>
        /// <param name="value">The value found if succesful, else default value</param>
        /// <returns>True if the lookup was successful</returns>
        public static Boolean TryDictionaryLookup<T1, T2>(this IDictionary<T1, T2> dict, T1 key, out T2? value)
        {
            if (!(dict==null || key == null))
            {
                try
                {
                    if (dict.ContainsKey(key))
                    {
                        value = dict[key];
                        return true;
                    }
                }
                catch { }// False.
            }
            value = default;
            return false;
        }

        public static Boolean TryDictionaryRemove<T1, T2>(this IDictionary<T1, T2> dict, T1 key)
        {
            if (!(dict == null || key == null))
            {
                try
                {
                    if (dict.ContainsKey(key))
                    {
                        dict.Remove(key);
                        return true;
                    }
                }
                catch { }// False.
            }
            return false;
        }


        /// <summary>
        /// This method will add or update the value in the given dictionary, else return false if the operation fails.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean TryDictionaryAddOrUpdate<T1, T2>(this IDictionary<T1, T2> dict, T1 key, T2 value)
        {
            if (!(dict == null || key == null || value == null))
            {
                try
                {
                    if (dict.ContainsKey(key))
                    {
                        dict[key] = value;
                    }
                    else
                    {
                        dict.Add(key, value);
                    }
                    return true;
                }
                catch { }// False.
            }
            return false;
        }

        /// <summary>
        /// Thsi method will check to see if the value exists in the dictionary. If it does it will return true, 
        /// else it will add the value and return true, otherwise it will return false if the operation fails.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean TryDictionaryCheckOrAdd<T1, T2>(this IDictionary<T1, T2> dict, T1 key, T2 value)
        {
            if (!(dict == null || key == null || value == null))
            {
                try
                {
                    if (dict.ContainsKey(key))
                    {
                        return true;
                    }
                    else
                    {
                        dict.Add(key, value);
                    }
                    return true;
                }
                catch { }// False.
            }
            return false;
        }
        #endregion

        #region String Collection/Array
        public static void GenerateNewNameNotInArray(ref string startName, string[] existingNames)
        {
            string newName = startName;
            int n = 0;
            uint suffix = 0;
            while (n < existingNames.Length)
            {
                if (existingNames[n] == newName)
                {
                    newName = $"{startName}({suffix++})";
                    n = 0;// Unfortuantely this means we must start again
                }
                else n++;
            }
            startName = newName;
        }

        public static Dictionary<T2, T1> DictionaryInvert<T1, T2>(IDictionary<T1, T2> invertableDict)
        {
            if (TryDictionaryInvert(invertableDict, out Dictionary<T2, T1> invertedDict))
            {
                return invertedDict;
            }
            return new Dictionary<T2, T1>();
        }

        public static bool TryDictionaryInvert<T1, T2>(IDictionary<T1, T2> invertableDict, out Dictionary<T2, T1> invertedDict)
        {
            ConcurrentDictionary<T2, T1> concurretDictionary = new();
            bool successful = true;
            Parallel.ForEach(invertableDict, (kvp) =>
            {
                if (!concurretDictionary.TryAdd(kvp.Value, kvp.Key))
                {
                    successful = false;
                }
            });
            if (successful)
            {
                invertedDict = new Dictionary<T2, T1>(concurretDictionary);
                return successful;
            }
            Dictionary<T2, T1> dictionary = new Dictionary<T2, T1>(concurretDictionary);
            try
            {
                foreach (KeyValuePair<T1, T2> kvp in invertableDict)
                {
                    if (!dictionary.ContainsKey(kvp.Value))
                    {
                        dictionary.Add(kvp.Value, kvp.Key);
                    }
                }
                invertedDict = dictionary;
                return invertedDict.Count == invertableDict.Count;
            }
            catch(Exception ex)
            {
                string mes = ex.Message;
            }
            invertedDict = default;
            return false;
        }

        #endregion

        #region Double Collection/Array
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lastValues"></param>
        /// <returns></returns>
        public static double Average(double[] lastValues)
        {
            return lastValues.Average();
        }
        #endregion

        #region Generic Collection/Array
        public static void ParallelFill<T>(T value, T[] array)
        {
            Parallel.For(0, array.Length, (e) =>
            {
                array[e] = value;
            });

        }
        #endregion

        #region String Manipulation
        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        #endregion String Manipulation

        //public static string GetAppPath(string saveName)
        //{
        //    return Path.Combine(Constants.DATAM_CONFIG_DIRECTORY, $"{saveName}{Constants.DATAM_CONFIG_EXT}");
        //}

        #region String Validity
        public static Boolean IsAnyNullOrWhiteSpace(this String[] strings)
        {
            for (int i = 0; i < strings.Length; i++)
            {
                if (String.IsNullOrWhiteSpace(strings[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public static Boolean HasAnyNullOrWhiteSpace(params String[] strings)
        {
            for (int i = 0; i < strings.Length; i++)
            {
                if (String.IsNullOrWhiteSpace(strings[i]))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion String Validity

        #region Random
        private static readonly Random random = new();

        public static Byte GetRandom()
        {
            return Convert.ToByte(random.Next(Byte.MinValue, Byte.MaxValue));
        }
        #endregion Random

    }
}

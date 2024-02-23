using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common.Extensions
{

    public static class Extensions_Dictionary
    {
#pragma warning disable IDE0090 // Use 'new(...)'

        #region Key
        /// <summary>
        /// This method allows for safe dictionary lookups by wrappping the IDictionary 
        /// inheriting class in a try-catch wrapper that checks for the key first
        /// before attempting lookup. 
        /// </summary>
        /// <typeparam name="T1">Dictionay key type</typeparam>
        /// <typeparam name="T2">Dictionary value type</typeparam>
        /// <param name="dict">The dictionatry in which to lookup a value</param>
        /// <param name="key">The key value to look up</param>
        /// <returns>True if the lookup was successful</returns>
        public static Boolean TryVerifyContainsKey<T1, T2>(this IDictionary<T1, T2> dict, T1 key)
        {
            if (!(dict == null || key == null))
            {
                try
                {
                    if (dict.ContainsKey(key))
                    {
                        return true;
                    }
                }
                catch { }// False.
            }
            return false;
        }

        /// <summary>
        /// This method will check to see if the Value given will return a valid Key from the dictionary. It it can,
        /// it will return true as well as the key, otherwise it will return false and return default.
        /// </summary>
        /// <typeparam name="K">Key Type</typeparam>
        /// <typeparam name="V">Value type</typeparam>
        /// <param name="dict">Dictionary to search</param>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <returns>True if key can be retreived. False if it cannot.</returns>
        public static Boolean TryGetKeyFromValue<K, V>(this IDictionary<K, V> dict, V value, out K key)
        {
            key = default;
            if (!(dict == null || value == null))//Check that entered values aren't null
            {
                try
                {
                    key = GetKeyFromValue<K, V>(dict, value);
                    return true;
                }
                catch
                {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        /// This method searches through a dictionary and returns the Key associated with the input Value
        /// </summary>
        /// <typeparam name="K">Key type</typeparam>
        /// <typeparam name="V">Value type</typeparam>
        /// <param name="dict">Dictionary to search</param>
        /// <param name="value">Value to find</param>
        /// <returns>Key associated with the value to find</returns>
        public static K GetKeyFromValue<K, V>(this IDictionary<K, V> dict, V value)
        {
            K key = default;
            if (!(dict == null || value == null))//Check that entered values aren't null
            {
                foreach (KeyValuePair<K, V> pair in dict)
                {
                    if (EqualityComparer<V>.Default.Equals(pair.Value, value))
                    {
                        key = pair.Key;
                        break;
                    }
                }
            }
            return key;
        }
        #endregion /Key

        #region Lookup
        public static Boolean TryGetFirst<T1, T2>(this IDictionary<T1, T2> dict, out T2 value)
        {
            if (dict != null && dict.Any())
            {
                try
                {
                    value = dict.First().Value;
                    return true;
                }
                catch { }// False.
            }
            value = default;
            return false;
        }

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
        public static Boolean TryLookup<T1, T2>(this IDictionary<T1, T2> dict, T1 key, out T2 value)
        {
            if (!(dict == null || key == null))
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
        #endregion /Lookup

        #region Remove
        public static Boolean TryRemoveFirst<T1, T2>(this IDictionary<T1, T2> dict, out T2 value)
        {
            return dict.TryRemove(dict.First().Key, out value);
        }

        public static Boolean TryRemove<T1, T2>(this IDictionary<T1, T2> dict, T1 key)
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

        public static Boolean TryRemove<T1, T2>(this IDictionary<T1, T2> dict, T1 key, out T2 value)
        {
            if (!(dict == null || key == null))
            {
                try
                {
                    if (dict.ContainsKey(key))
                    {
                        value = dict[key];
                        dict.Remove(key);
                        return true;
                    }
                }
                catch { }// False.
            }
            value = default;
            return false;
        }
        #endregion /Remove

        #region Add or Update
        /// <summary>
        /// This method will add or update the value in the given dictionary, else return false if the operation fails.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean TryAddOrUpdate<T1, T2>(this IDictionary<T1, T2> dict, T1 key, T2 value)
        {
            if (!(dict == null || key == null || value == null))
            {
                try
                {
                    if (dict.ContainsKey(key))
                    {
                        dict[key] = value;
                        return true;
                    }
                    dict.Add(key, value);
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
        public static Boolean TryCheckOrAdd<T1, T2>(this IDictionary<T1, T2> dict, T1 key, T2 value)
        {
            if (!(dict == null || key == null || value == null))
            {
                try
                {
                    if (dict.ContainsKey(key))
                    {
                        return true;
                    }
                    dict.Add(key, value);
                    return true;
                }
                catch { }// False.
            }
            return false;
        }

        /// <summary>
        /// This method will check to see if the value exists in the dictionary. If it does it will return false, 
        /// else it will add the value and return true, otherwise it will return false if the operation fails.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean TryCheckNotOrAdd<T1, T2>(this IDictionary<T1, T2> dict, T1 key, T2 value)
        {
            if (!(dict == null || key == null || value == null))
            {
                try
                {
                    if (dict.ContainsKey(key))
                    {
                        return false;
                    }
                    dict.Add(key, value);
                    return true;
                }
                catch { }// False.
            }
            return false;
        }

        /// <summary>
        /// This method will check to see if the value exists in the dictionary. If it does it will return false, 
        /// else it will add the value and return true, otherwise it will return false if the operation fails.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean TryCheckNotOrAdd<T1, T2>(this IDictionary<T1, T2> dict, KeyValuePair<T1, T2> entry)
        {
            if (dict != null)
            {
                try
                {
                    if (dict.ContainsKey(entry.Key))
                    {
                        return false;
                    }
                    dict.Add(entry);
                    return true;
                }
                catch { }// False.
            }
            return false;
        }

        /// <summary>
        /// Returns true if already in dict, otherwise adds and returns false.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean TryCheckIsOrAdd<T1, T2>(this IDictionary<T1, T2> dict, T1 key, T2 value)
        {
            if (!(dict == null || key == null || value == null))
            {
                try
                {
                    if (dict.ContainsKey(key))
                    {
                        return true;
                    }
                    dict.Add(key, value);
                    return false;
                }
                catch { }// False.
            }
            return false;
        }
        #endregion /Add or Update

        #region Counter 
        public static Boolean TryDecriment<T1>(this IDictionary<T1, int> dict, T1 key)
        {
            if (!(dict == null || key == null))
            {
                try
                {
                    if (dict.ContainsKey(key))
                    {
                        dict[key]--;
                        return true;
                    }
                }
                catch { }// False.
            }
            return false;
        }
        #endregion /Counter

        #region Check
        public static Boolean TryCheck<T1, T2>(this IDictionary<T1, T2> dict, T1 key)
        {
            if (!(dict == null || key == null))
            {
                try
                {
                    if (dict.ContainsKey(key))
                    {
                        return true;
                    }
                }
                catch { }// False.
            }
            return false;
        }
        #endregion /Check

        #region Create
        /// <summary>
        /// This method will check for an entry and if not create the entry using the provided function. 
        /// It will then return true if it succeeded.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="valueCreator"></param>
        /// <returns></returns>
        public static Boolean CheckOrCreate<T1, T2>(this IDictionary<T1, T2> dict, T1 key, Func<T2> valueCreator)
        {
            if (!(dict == null || key == null || valueCreator == null))
            {
                lock (dict)
                {
                    try
                    {
                        if (dict.ContainsKey(key))
                        {
                            return true;
                        }
                        var t2 = valueCreator.Invoke();
                        dict.Add(key, t2);
                        return true;
                    }
                    catch { }// False.
                }
            }
            return false;
        }

        /// <summary>
        /// This method will check for an entry and if not create the entry using the provided function. 
        /// It will then return true if it succeeded.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="valueCreator"></param>
        /// <returns></returns>
        public static Boolean CheckOrCreateWith_Key<T1, T2>(this IDictionary<T1, T2> dict, T1 key, Func<T1,T2> valueCreator)
        {
            if (!(dict == null || key == null || valueCreator == null))
            {
                lock (dict)
                {
                    try
                    {
                        if (dict.ContainsKey(key))
                        {
                            return true;
                        }
                        var t2 = valueCreator.Invoke(key);
                        dict.Add(key, t2);
                        return true;
                    }
                    catch { }// False.
                }
            }
            return false;
        }

        /// <summary>
        /// This method will check for an entry and if not create the entry using the provided function. 
        /// It will then return true if it succeeded.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="valueCreator"></param>
        /// <returns></returns>
        public static Boolean CheckNotOrCreate<T1, T2>(this IDictionary<T1, T2> dict, T1 key, Func<T2> valueCreator)
        {
            if (!(dict == null || key == null || valueCreator == null))
            {
                lock (dict)
                {
                    try
                    {
                        if (dict.ContainsKey(key))
                        {
                            return false;
                        }
                        var t2 = valueCreator.Invoke();
                        dict.Add(key, t2);
                        return true;
                    }
                    catch { }// False.
                }
            }
            return false;
        }

        public static Boolean CheckOrCreate<T1, T2, T3>(this IDictionary<T1, T2> dict, T1 key, Func<T3, T2> valueCreator, T3 param)
        {
            if (!(dict == null || key == null || valueCreator == null))
            {
                lock (dict)
                {
                    try
                    {
                        if (dict.ContainsKey(key))
                        {
                            return false;
                        }
                        var t2 = valueCreator.Invoke(param);
                        dict.Add(key, t2);
                        return true;
                    }
                    catch
                    {
                        throw;
                    }// False.
                }
            }
            return false;
        }


        public static Boolean CheckOrCreate<T1, T2, T3, T4>(this IDictionary<T1, T2> dict, T1 key, Func<T3, T4, T2> valueCreator, T3 param1, T4 param2)
        {
            if (!(dict == null || key == null || valueCreator == null))
            {
                lock (dict)
                {
                    try
                    {
                        if (dict.ContainsKey(key))
                        {
                            return false;
                        }
                        var t2 = valueCreator.Invoke(param1, param2);
                        dict.Add(key, t2);
                        return true;
                    }
                    catch
                    {
                        throw;
                    }// False.
                }
            }
            return false;
        }

        /// <summary>
        /// This method will check for an entry and if not create the entry using the provided function. 
        /// It will then return the value if sucesssful, and throw an exception if not.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="valueCreator"></param>
        /// <returns>Value at the given key.</returns>
        public static T2 ProvideOrCreate<T1, T2>(this IDictionary<T1, T2> dict, T1 key, Func<T2> valueCreator)
        {
            if (!(dict == null || key == null || valueCreator == null))
            {
                lock (dict)
                {
                    try
                    {
                        if (dict.ContainsKey(key))
                        {
                            return dict[key];
                        }
                        var t2 = valueCreator.Invoke();
                        dict.Add(key, t2);
                        return t2;
                    }
                    catch { }// False.
                }
            }
            throw new ArgumentOutOfRangeException("Failed to provide or add to dictionary!");
        }

        /// <summary>
        /// This method will check for an entry and if not create the entry using the provided function. 
        /// It will return true if the value wasnt previously present, false if it was, or throw an exception if unsuccesssful.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="valueCreator"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean TryCheckNotProvideOrCreate<T1, T2>(this IDictionary<T1, T2> dict, T1 key, Func<T2> valueCreator, out T2 value)
        {
            if (!(dict == null || key == null || valueCreator == null))
            {
                lock (dict)
                {
                    try
                    {
                        if (dict.ContainsKey(key))
                        {
                            value = dict[key];
                            return false;
                        }
                        value = valueCreator.Invoke();
                        dict.Add(key, value);
                        return true;
                    }
                    catch { }// False.
                }
            }
            throw new ArgumentOutOfRangeException("Failed to provide or add to dictionary!");
        }
        #endregion /Create

        #region Extract
        public static Boolean TryExtractAll_Collections<T1, T2, T3>(this IDictionary<T1, T2> dict, out T3[] contents) where T2 : ICollection<T3>
        {
            if (dict != null)
            {
                IEnumerable<T3> outList = new List<T3>();
                try
                {
                    foreach (T2 collection in dict.Values)
                    {
                        outList = outList.Union(collection);
                    }
                    contents = outList.ToArray();
                    return true;
                }
                catch { }// False.
            }
            throw new ArgumentOutOfRangeException("Failed to provide or add to dictionary!");
        }

        public static Boolean TryExtractAll_Set<T1, T2, T3>(this IDictionary<T1, T2> dict, out T3[] contents) where T2 : ISet<T3>
        {
            if (dict != null)
            {
                IEnumerable<T3> outSet = new HashSet<T3>();// The best terminator (jk I dont have an opinion on that).
                try
                {
                    foreach (T2 set in dict.Values)
                    {
                        outSet = outSet.Union(set);
                    }
                    contents = outSet.ToArray();
                    return true;
                }
                catch { }// False.
            }
            throw new ArgumentOutOfRangeException("Failed to provide or add to dictionary!");
        }
        #endregion /Extract

        #region Null Check
        public static void RemoveNullKeys<T1, T2>(this IDictionary<T1, T2> dict)
        {
            if (dict != null)
            {
                ConcurrentBag<T1> removeBag = new ConcurrentBag<T1>();// The worst terminator (jk I dont have an opinion on that).
                try
                {
                    lock (dict)
                    {
                        Parallel.ForEach(dict.Keys, (key) =>
                        {
                            if (key == null)
                            {
                                removeBag.Add(key);
                            }
                        });
                        foreach (T1 key in removeBag)
                        {
                            dict.Keys.Remove(key);
                        }
                    }
                }
                catch { }// False.
            }
        }
        #endregion /Null Check

        #region Sort
        /// <summary>
        /// This method takes a Dictonary and sorts it by its key.
        /// </summary>
        /// <typeparam name="T2">Dictionary Value Type</typeparam>
        /// <param name="sortingDict">Dictionary to sort.</param>
        /// <returns></returns>
        public static Dictionary<TabPage, T2> Sort<T2>(this IDictionary<TabPage, T2> sortingDict)
        {
            if (sortingDict is null)
            {
                return new Dictionary<TabPage, T2>();
            }
            Dictionary<TabPage, T2> sortedDict = new Dictionary<TabPage, T2>();
            foreach (var item in sortingDict.OrderBy(i => i.Key.Text))
            {
                sortedDict.Add(item.Key, item.Value);
            }
            return sortedDict;
        }

        /// <summary>
        /// This method takes a Dictonary and sorts it by its key.
        /// </summary>
        /// <typeparam name="T1">Dictionary Key Type</typeparam>
        /// <typeparam name="T2">Dictionary Value Type</typeparam>
        /// <param name="sortingDict">Dictionary to sort.</param>
        /// <returns></returns>
        public static IDictionary<TabPage, T2> Sort_TabPage<T2>(this IDictionary<TabPage, T2> sortingDict, String[] sortOrder)
        {
            IDictionary<TabPage, T2> sortedDict = new Dictionary<TabPage, T2>();
            for (int sO = 0; sO < sortOrder.Length; sO++)
            {
                for (int sD = 0; sD < sortingDict.Count; sD++)
                {
                    if (sortingDict.ElementAt(sD).Key.Text == sortOrder[sO])
                    {
                        sortedDict.Add(sortingDict.ElementAt(sD).Key, sortingDict.ElementAt(sD).Value);
                        break;
                    }
                }
            }
            return sortedDict;
        }

        /// <summary>
        /// This method takes a Dictonary and sorts it by its key.
        /// </summary>
        /// <typeparam name="T1">Dictionary Key Type</typeparam>
        /// <typeparam name="T2">Dictionary Value Type</typeparam>
        /// <param name="sortingDict">Dictionary to sort.</param>
        /// <returns></returns>
        public static IDictionary<T1, T2> Sort_Custom<T1, T2>(this IDictionary<T1, T2> sortingDict) where T1 : IComparable
        {
            Dictionary<T1, T2> sortedDict = new Dictionary<T1, T2>();
            foreach (var item in sortingDict.OrderBy(i => i.Key))
            {
                sortedDict.Add(item.Key, item.Value);
            }
            return sortedDict;
        }

        /// <summary>
        /// This method takes a Dictonary and sorts it by its key.
        /// </summary>
        /// <typeparam name="T1">Dictionary Key Type</typeparam>
        /// <typeparam name="T2">Dictionary Value Type</typeparam>
        /// <param name="sortingDict">Dictionary to sort.</param>
        /// <returns></returns>
        public static ConcurrentDictionary<T1, T2> Sort_Custom<T1, T2>(this ConcurrentDictionary<T1, T2> sortingDict) where T1 : IComparable
        {
            ConcurrentDictionary<T1, T2> sortedDict = new ConcurrentDictionary<T1, T2>();
            foreach (var item in sortingDict.OrderBy(i => i.Key))
            {
                sortedDict.AddOrUpdate(item.Key, item.Value, (key, preValue) => item.Value);// Replace value if it exists
            }
            return sortedDict;
        }
        #endregion /Sort

        #region Invert
        public static IDictionary<T2, T1> Invert<T1, T2>(this IDictionary<T1, T2> invertableDict)
        {
            if (TryInvert(invertableDict, out Dictionary<T2, T1> invertedDict))
            {
                return invertedDict;
            }
            return new Dictionary<T2, T1>();
        }

        public static Boolean TryInvert<T1, T2>(this IDictionary<T1, T2> invertableDict, out Dictionary<T2, T1> invertedDict)
        {
            ConcurrentDictionary<T2, T1> concurretDictionary = new ConcurrentDictionary<T2, T1>();
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
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            invertedDict = default;
            return false;
        }
        #endregion /Invert

#pragma warning restore IDE0090 // Use 'new(...)'
    }
}

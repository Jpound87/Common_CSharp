using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Common
{
    public static class Extensions_Serialization
    {
        #region Object Serialization

        #region Dictionary
        public static void SerializeDictionary<T1, T2>(this IDictionary<T1, T2> dictionary, string tag, ref SerializationInfo info)
            where T1 : ISerializable where T2 : ISerializable
        {
            try
            {
                string keyTag = string.Format("key{0}", tag);
                string valueTag = string.Format("value{0}", tag);
                dictionary.Keys.ToArray().SerializeArray(keyTag, ref info);
                dictionary.Values.ToArray().SerializeArray(valueTag, ref info);
            }
            catch
            {
                throw;
            }
        }

        public static void SerializeDictionary<T2>(this IDictionary<uint, T2> dictionary, string tag, ref SerializationInfo info)
            where T2 : ISerializable
        {
            try
            {
                string keyTag = string.Format("key{0}", tag);
                string valueTag = string.Format("value{0}", tag);
                dictionary.Keys.ToArray().SerializeArray(keyTag, ref info);
                dictionary.Values.ToArray().SerializeArray(valueTag, ref info);
            }
            catch
            {
                throw;
            }
        }

        public static void SerializeDictionary(this IDictionary<string, int> dictionary, string tag, ref SerializationInfo info)
        {
            try
            {
                string keyTag = string.Format("key{0}", tag);
                string valueTag = string.Format("value{0}", tag);
                dictionary.Keys.ToArray().SerializeArray(keyTag, ref info);
                dictionary.Values.ToArray().SerializeArray(valueTag, ref info);
            }
            catch
            {
                throw;
            }
        }

        public static void RecallSerializedDictionary<T1, T2>(ref IDictionary<T1, T2> dictionary, string tag, SerializationInfo info)
             where T1 : ISerializable where T2 : ISerializable
        {
            if (dictionary == null || dictionary.Count > 0)
            {
                throw new ArgumentException("dictionary must be initialized and empty");
            }
            string keyTag = string.Format("key{0}", tag);
            string valueTag = string.Format("value{0}", tag);
            T1[] keys = RecallSerializedCollection<T1>(keyTag, info);
            T2[] values = RecallSerializedCollection<T2>(valueTag, info);
            if (keys.Length == values.Length)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    dictionary.Add(keys[i], values[i]);
                }
            }
            else
            {
                throw new DataMisalignedException("number of keys not equal to number of values.");
            }
        }


        public static void RecallSerializedDictionary<T2>(ref IDictionary<uint, T2> dictionary, string tag, SerializationInfo info)
             where T2 : ISerializable
        {
            if (dictionary == null || dictionary.Count > 0)
            {
                throw new ArgumentException("dictionary must be initialized and empty");
            }
            string keyTag = string.Format("key{0}", tag);
            string valueTag = string.Format("value{0}", tag);
            uint[] keys = RecallSerializedUintCollection(keyTag, info);
            T2[] values = RecallSerializedCollection<T2>(valueTag, info);
            if (keys.Length == values.Length)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    dictionary.Add(keys[i], values[i]);
                }
            }
            else
            {
                throw new DataMisalignedException("number of keys not equal to number of values.");
            }
        }


        public static void RecallSerializedDictionary(ref IDictionary<string, int> dictionary, string tag, SerializationInfo info)
        {
            if (dictionary == null || dictionary.Count > 0)
            {
                throw new ArgumentException("dictionary must be initialized and empty");
            }
            string keyTag = string.Format("key{0}", tag);
            string valueTag = string.Format("value{0}", tag);
            string[] keys = RecallSerializedStringCollection(keyTag, info);
            int[] values = RecallSerializedIntCollection(valueTag, info);
            if (keys.Length == values.Length)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    dictionary.Add(keys[i], values[i]);
                }
            }
            else
            {
                throw new DataMisalignedException("number of keys not equal to number of values.");
            }
        }
        #endregion

        #region Collection

        public static void SerializeCollection<T>(this ICollection<T> collection, string tag, ref SerializationInfo info) where T : ISerializable
        {
            collection.ToArray().SerializeArray(tag, ref info);
        }

        public static void RecallSerializedCollection<T>(string listName, SerializationInfo info, ref ICollection<T> collection) where T : ISerializable
        {
            foreach (T element in RecallSerializedCollection<T>(listName, info))
            {
                collection.Add(element);
            }
        }

        public static void RecallSerializedCollection<T>(string listName, SerializationInfo info, ref ISet<T> set) where T : ISerializable
        {
            foreach (T element in RecallSerializedCollection<T>(listName, info))
            {
                set.Add(element);
            }
        }

        public static T[] RecallSerializedCollection<T>(string listName, SerializationInfo info) where T : ISerializable
        {
            string[] elementNames = new string[0];
            if (info.GetValue(listName, typeof(string[]))
                is string[] _elementNames)
            {
                elementNames = _elementNames;
            }
            List<T> elements = new List<T>();
            foreach (string name in elementNames)
            {
                if (info.GetValue(name, typeof(T)) is T element)
                {
                    elements.Add(element);
                }
            }
            return elements.ToArray();
        }

        public static string[] RecallSerializedStringCollection(string listName, SerializationInfo info)
        {
            string[] elementNames = new string[0];
            if (info.GetValue(listName, typeof(string[]))
                is string[] _elementNames)
            {
                elementNames = _elementNames;
            }
            List<string> elements = new List<string>();
            foreach (string name in elementNames)
            {
                if (info.GetValue(name, typeof(string)) is string element)
                {
                    elements.Add(element);
                }
            }
            return elements.ToArray();
        }

        public static int[] RecallSerializedIntCollection(string listName, SerializationInfo info)
        {
            string[] elementNames = new string[0];
            if (info.GetValue(listName, typeof(string[]))
                is string[] _elementNames)
            {
                elementNames = _elementNames;
            }
            List<int> elements = new List<int>();
            foreach (string name in elementNames)
            {
                if (info.GetValue(name, typeof(int)) is int element)
                {
                    elements.Add(element);
                }
            }
            return elements.ToArray();
        }


        public static uint[] RecallSerializedUintCollection(string listName, SerializationInfo info)
        {
            string[] elementNames = new string[0];
            if (info.GetValue(listName, typeof(string[]))
                is string[] _elementNames)
            {
                elementNames = _elementNames;
            }
            List<uint> elements = new List<uint>();
            foreach (string name in elementNames)
            {
                if (info.GetValue(name, typeof(uint)) is uint element)
                {
                    elements.Add(element);
                }
            }
            return elements.ToArray();
        }
        #endregion

        #region Array
        /// <summary>
        /// This method is designed to seralize an array of the some generic type.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="array"></param>
        /// <param name="info"></param>
        public static void SerializeArray<T>(this T[] array, string tag, ref SerializationInfo info) where T : ISerializable
        {
            List<string> elementNames = new List<string>();
            for (int pdIndex = 0; pdIndex < array.Length; pdIndex++)
            {
                string elementName = string.Format("{0}{1}", tag, pdIndex);
                elementNames.Add(elementName);
                info.AddValue(elementName, array[pdIndex], typeof(T));
            }
            info.AddValue(tag, elementNames.ToArray());
        }

        /// <summary>
        /// This method is designed to seralize an array of integers.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="array"></param>
        /// <param name="info"></param>
        public static void SerializeArray(this int[] array, string tag, ref SerializationInfo info)
        {
            List<string> elementNames = new List<string>();
            for (int pdIndex = 0; pdIndex < array.Length; pdIndex++)
            {
                string elementName = string.Format("{0}{1}", tag, pdIndex);
                elementNames.Add(elementName);
                info.AddValue(elementName, array[pdIndex]);
            }
            info.AddValue(tag, elementNames.ToArray());
        }


        /// <summary>
        /// This method is designed to seralize an array of integers.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="array"></param>
        /// <param name="info"></param>
        public static void SerializeArray(this uint[] array, string tag, ref SerializationInfo info)
        {
            List<string> elementNames = new List<string>();
            for (int pdIndex = 0; pdIndex < array.Length; pdIndex++)
            {
                string elementName = string.Format("{0}{1}", tag, pdIndex);
                elementNames.Add(elementName);
                info.AddValue(elementName, array[pdIndex]);
            }
            info.AddValue(tag, elementNames.ToArray());
        }

        /// <summary>
        /// This method is designed to seralize an array of string.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="array"></param>
        /// <param name="info"></param>
        public static void SerializeArray(this string[] array, string tag, ref SerializationInfo info)
        {
            List<string> elementNames = new List<string>();
            for (int pdIndex = 0; pdIndex < array.Length; pdIndex++)
            {
                string elementName = string.Format("{0}{1}", tag, pdIndex);
                elementNames.Add(elementName);
                info.AddValue(elementName, array[pdIndex]);
            }
            info.AddValue(tag, elementNames.ToArray());
        }
        #endregion

        #endregion
    }
}

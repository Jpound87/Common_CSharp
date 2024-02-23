using Common.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Utility
{
    public struct FM_Index_Data
    {
        #region Identity
        public const string StructName = nameof(FM_Index_Data);
        public String Identity
        {
            get
            {
                return StructName;
            }
        }
        #endregion

        #region Accessors
        private String transformedString;
        private String storedData;
        public String Data 
        { 
            get
            {
                return storedData;
            }
            private set
            {
                Add(value);
            }
        }
        private int[] suffixArray;
        private String bwt;
        public String CompressedData { get; private set; }
        #endregion /Accessors

        #region Constructor
        public FM_Index_Data(String data)
        {
            storedData = data;
            transformedString = storedData + Tokens.START_TOKEN;
            suffixArray = FM_Index.GenerateSuffixArray(transformedString);
            bwt = FM_Index.GenerateBWT(transformedString, suffixArray);
            CompressedData = FM_Index.RunLengthEncoding(bwt);
        }

        public FM_Index_Data(params String[] datum)
        {
            storedData = Utility_String.ConcatWithSpaces(datum);
            transformedString = storedData + Tokens.START_TOKEN;
            suffixArray = FM_Index.GenerateSuffixArray(transformedString);
            bwt = FM_Index.GenerateBWT(transformedString, suffixArray);
            CompressedData = FM_Index.RunLengthEncoding(bwt);
        }
        #endregion /Constructor

        #region Add
        public void Add(params String[] datum)
        {
            if (!String.IsNullOrEmpty(storedData))
            {
                Array.Resize(ref datum, datum.Length + 1);
                datum[datum.Length] = storedData;
            }
            storedData = Utility_String.ConcatWithSpaces(datum);
            lock (storedData)
            {
                transformedString = storedData + Tokens.START_TOKEN;
                suffixArray = FM_Index.GenerateSuffixArray(transformedString);
                bwt = FM_Index.GenerateBWT(transformedString, suffixArray);
                CompressedData = FM_Index.RunLengthEncoding(bwt);
            }
        }
        #endregion /Add

        #region Remove
        public void Remove(params String[] datum)
        {
            if (!String.IsNullOrEmpty(storedData))
            {
                int offset = 0;// as we remove values the index locations will change.
                SortedDictionary<int, int> dictLocationLength = new SortedDictionary<int, int>();
                lock (storedData)
                {
                    int at;
                    foreach (string data in datum)
                    {
                        if (Find(data, out at))
                        {
                            dictLocationLength.Add(at, data.Length);
                        }
                    }
                    if (dictLocationLength.Keys.Any())
                    {// Any changes?
                        foreach (KeyValuePair<int, int> locationLength_kvp in dictLocationLength)
                        {
                            at = locationLength_kvp.Key - offset;
                            offset += locationLength_kvp.Value;
                            storedData.Remove(at, locationLength_kvp.Value);
                        }
                        CompressedData = FM_Index.Compress(storedData);
                    }
                }
            }
        }
        #endregion /Remove

        #region Search
        public bool Find(String data, out int at)
        {
            if (!String.IsNullOrEmpty(storedData))
            {//No stored data implied no compressed data, and this keeps the lock consistant
                lock (storedData)
                {
                    int result = FM_Index.FindLocation(CompressedData, data);
                    if (result > 0)
                    {
                        at = result;
                        return true;
                    }
                }
            }
            at = -1;
            return false;
        }

        public bool Contains(String data)
        {
            if (!String.IsNullOrEmpty(storedData))
            {//No stored data implied no compressed data, and this keeps the lock consistant
                lock (storedData)
                {
                    return FM_Index.Contains(CompressedData, data);
                }
            }
            return false;
        }

        public uint CountOccurances(String data)
        {
            if (!String.IsNullOrEmpty(storedData))
            {//No stored data implied no compressed data, and this keeps the lock consistant
                lock (storedData)
                {
                    FM_Index.CountOccurrences(bwt, data);
                }
            }
            return 0;
        }
        #endregion /Search

        #region Clear
        public void Clear()
        {
            storedData = String.Empty;
            CompressedData = String.Empty;
        }
        #endregion /Clear
    }

    public static class FM_Index
    {
        /// <summary>
        /// Compresses the input string using FM-Indexing
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String Compress(String input)
        {
            string transformedString = input + Tokens.START_TOKEN;
            var suffixArray = GenerateSuffixArray(transformedString);
            string bwt = GenerateBWT(transformedString, suffixArray);
            string compressedString = RunLengthEncoding(bwt);
            return compressedString;
        }

        /// <summary>
        /// Decompresses the input string using FM-Indexing
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String Decompress(String input)
        {
            string bwt = RunLengthDecoding(input);
            string transformedString = InverseBWT(bwt);
            string decompressedString = transformedString.Substring(0, transformedString.Length - 1);
            return decompressedString;
        }

        /// <summary>
        /// Adds a string to the already compressed data
        /// </summary>
        /// <param name="compressedData"></param>
        /// <param name="newString"></param>
        /// <returns></returns>
        public static String AddString(String compressedData, String newString)
        {
            string decompressedData = Decompress(compressedData);
            string mergedString = decompressedData + newString;
            string compressedString = Compress(mergedString);
            return compressedString;
        }

        /// <summary>
        /// Removes a string from the already compressed data
        /// </summary>
        /// <param name="compressedData"></param>
        /// <param name="stringToRemove"></param>
        /// <returns></returns>
        public static String RemoveString(String compressedData, String stringToRemove)
        {
            string decompressedData = Decompress(compressedData);
            string modifiedString = decompressedData.Replace(stringToRemove, string.Empty);
            string compressedString = Compress(modifiedString);
            return compressedString;
        }

        /// <summary>
        /// Searches for a string in the compressed data using FM-Indexing
        /// </summary>
        /// <param name="compressedData"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public static bool Contains(String compressedData, String searchString)
        {
            string decompressedData = Decompress(compressedData);
            string transformedString = decompressedData + Tokens.START_TOKEN;
            int[] suffixArray = GenerateSuffixArray(transformedString);
            string bwt = GenerateBWT(transformedString, suffixArray);
            var occurrences = CountOccurrences(bwt, searchString);
            return occurrences > 0;
        }

        /// <summary>
        /// Counts the occurrences of a string in the BWT
        /// </summary>
        /// <param name="bwt"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public static int CountOccurrences(String bwt, String searchString)
        {
            int count = 0;
            int index = bwt.IndexOf(searchString);
            while (index >= 0)
            {
                count++;
                index = bwt.IndexOf(searchString, index + 1);
            }
            return count;
        }

        // Finds the location of a string in the uncompressed data using FM-Indexing
        public static int FindLocation(String compressedData, String searchString)
        {
            string bwt = RunLengthDecoding(compressedData);
            string transformedString = InverseBWT(bwt);

            int startMarkerCount = transformedString.Length - bwt.Length;

            int location = -1;
            int rank = 0;
            int index = transformedString.IndexOf(searchString);

            while (index >= 0)
            {
                if (index >= startMarkerCount)
                {
                    rank++;
                }
                index = transformedString.IndexOf(searchString, index + 1);
            }

            if (rank > 0)
            {
                location = transformedString.Length - searchString.Length * rank + rank - 1;
            }

            return location;
        }

        /// <summary>
        /// Generates the suffix array for the given string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int[] GenerateSuffixArray(String str)
        {
            List<string> suffixes = new List<string>();
            for (int i = 0; i < str.Length; i++)
            {
                suffixes.Add(str.Substring(i));
            }
            suffixes.Sort();
            int[] suffixArray = new int[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                suffixArray[i] = str.Length - suffixes[i].Length;
            }
            return suffixArray;
        }

        /// <summary>
        /// Generates the Burrows-Wheeler Transform (BWT) for the given string and its suffix array.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="suffixArray"></param>
        /// <returns></returns>
        public static String GenerateBWT(String str, int[] suffixArray)
        {
            StringBuilder bwt = new StringBuilder();
            for (int i = 0; i < suffixArray.Length; i++)
            {
                int index = suffixArray[i] - 1;
                if (index < 0)
                {
                    index += str.Length;
                }
                bwt.Append(str[index]);
            }
            return bwt.ToString();
        }

        /// <summary>
        /// Performs run-length encoding on the given string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String RunLengthEncoding(String str)
        {
            StringBuilder encodedString = new StringBuilder();
            int count = 1;
            for (int i = 1; i < str.Length; i++)
            {
                if (str[i] == str[i - 1])
                {
                    count++;
                }
                else
                {
                    encodedString.Append(str[i - 1]);
                    encodedString.Append(count);
                    count = 1;
                }
            }
            encodedString.Append(str[str.Length - 1]);
            encodedString.Append(count);
            return encodedString.ToString();
        }

        /// <summary>
        /// Performs run-length decoding on the given string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static String RunLengthDecoding(String str)
        {
            StringBuilder decodedString = new StringBuilder();
            for (int i = 0; i < str.Length; i += 2)
            {
                char ch = str[i];
                int count = int.Parse(str[i + 1].ToString());
                decodedString.Append(ch, count);
            }
            return decodedString.ToString();
        }

        /// <summary>
        /// Performs the inverse Burrows-Wheeler Transform (BWT) on the given string.
        /// </summary>
        /// <param name="bwt"></param>
        /// <returns></returns>
        private static String InverseBWT(String bwt)
        {
            int[] count = new int[256];
            int[] next = new int[bwt.Length];
            char[] sortedChars = new char[bwt.Length];
            for (int i = 0; i < bwt.Length; i++)
            {
                count[bwt[i]]++;
            }
            int sum = 0;
            for (int i = 0; i < count.Length; i++)
            {
                int temp = count[i];
                count[i] = sum;
                sum += temp;
            }
            for (int i = 0; i < bwt.Length; i++)
            {
                char ch = bwt[i];
                int index = count[ch];
                next[index] = i;
                count[ch]++;
                sortedChars[index] = ch;
            }
            StringBuilder inverse = new StringBuilder();
            int current = next[0];
            for (int i = 0; i < bwt.Length; i++)
            {
                inverse.Append(sortedChars[current]);
                current = next[current];
            }
            return inverse.ToString();
        }
    }
}

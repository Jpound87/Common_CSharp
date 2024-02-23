using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static class Extensions_Byte
    {
        #region Identity
        public const String ClassName = nameof(Extensions_Byte);
        #endregion

        #region Byte Array

        #region To String
        /// <summary>
        /// This method will convert the provided byte array collection to a base 64 string. 
        /// The provided byte arrays must be of the same length.
        /// </summary>
        /// <param name="byteArraysToConvert">Collection of byte arrays to be converted to a base 64 string.</param>
        /// <returns></returns>
        public static String ByteArraysToBase64String(params byte[][] byteArraysToConvert)
        {
            if (byteArraysToConvert == null || byteArraysToConvert[0] == null)
            {// A direct failure.
                return String.Empty;
            }
            int len = byteArraysToConvert[0].Length;// Get the length of small array. (each converted number)
            byte[] fullArray = new byte[len * byteArraysToConvert.Length]; // The big array that will contain all converted numbers.
            
            try
            {
                Parallel.For(0, fullArray.Length, (at) =>
                {// Filling an array can be parallel due to fixed pointers.
                    int count = at / len; // Count is the value index.
                    if (byteArraysToConvert[count].Length != len)
                    {
                        throw new FormatException("The given byte arrays are not the same length");
                    }
                    fullArray[at] = byteArraysToConvert[count][at % len];
                });
            }
            catch
            {// Fail sort of gracefully.
                throw;
            }
            return fullArray.ByteArrayToBase64String();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytesToConvert"></param>
        /// <returns></returns>
        public static String ByteArrayToBase64String(this byte[] bytesToConvert)
        {
            return Convert.ToBase64String(bytesToConvert);
        }
        #endregion /To String

        #region Decompression
        public static byte[] Decompress_GZ(this byte[] fileToDecompress_GZ)
        {
            using (MemoryStream originalFileStream = new MemoryStream(fileToDecompress_GZ))
            {
                using (MemoryStream decompressedFileStream = new MemoryStream())
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        //Copy the decompression stream into the output file.
                        byte[] buffer = new byte[4096];
                        int numRead;
                        while ((numRead = decompressionStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            decompressedFileStream.Write(buffer, 0, numRead);
                        }
                        return decompressedFileStream.ToArray();
                    }
                }
            }
        }
        #endregion /Decompression

        #region Fill
        public static void FillByteArray_Zero(this byte[] bytes)// This bytes.
        {
            FillByteArray(bytes);
        }

        public static void FillByteArray(this byte[] bytes, byte fillByte = 0)// This bytes.
        {
            Parallel.For(0, bytes.Length, (b) =>
            {
                bytes[b] = fillByte;
            });
        }
        #endregion

        #endregion /Byte Array

        #region Byte Pattern Finder
        public static int IndexOf(this byte[] arrayToSearchThrough, int startIndex, byte[] patternToFind)
        {
            if (patternToFind.Length > arrayToSearchThrough.Length)
                return -1;

            for (int searchIndex = startIndex; searchIndex < arrayToSearchThrough.Length - patternToFind.Length; searchIndex++)
            {
                bool found = true;
                for (int patternIndex = 0; patternIndex < patternToFind.Length; patternIndex++)
                {
                    if (arrayToSearchThrough[searchIndex + patternIndex] != patternToFind[patternIndex])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    return searchIndex;
                }
            }
            return -1;
        }
        #endregion /Byte Pattern Finder
    }
}

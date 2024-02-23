using System;
using System.Collections.Generic;

namespace Common.Utility
{
    public static class Utility_Compression
    {
        #region Constants
        private const int HASH_TABLE_SIZE = 4096;
        private const long MAGIC_LZ4_HASH = 2654435761;
        #endregion /Constants

        #region Lz4

        #region Compress
        public static byte[] Compress_Lz4(this byte[] input)
        {
            List<byte> compressedData = new();
            List<byte> literals = new();
            Dictionary<int, List<int>> hashTable = new();

            int inputLength = input.Length;

            for (int i = 0; i < inputLength;)
            {
                int matchLength = 0;
                int matchIndex = -1;

                if (!hashTable.TryGetValue(Hash(input, i), out List<int> hashEntry))
                {
                    hashEntry = new List<int>();
                    hashTable[Hash(input, i)] = hashEntry;
                }

                foreach (int index in hashEntry)
                {
                    if (i - index >= 4 && i + matchLength < inputLength && input[index + matchLength] == input[i + matchLength])
                    {
                        int len = 1;
                        while (i + len < inputLength && input[index + len] == input[i + len] && len < 0x0F)
                        {
                            len++;
                        }
                        if (len > matchLength)
                        {
                            matchLength = len;
                            matchIndex = index;
                        }
                    }
                }

                if (matchLength >= 3)
                {
                    compressedData.Add((byte)((matchLength << 4) | ((i - matchIndex) & 0x0F)));
                    compressedData.Add((byte)(((i - matchIndex) >> 4) & 0xFF));
                    i += matchLength;
                }
                else
                {
                    literals.Add(input[i]);
                    i++;
                }
                if (i >= HASH_TABLE_SIZE)
                {
                    hashTable[Hash(input, i - HASH_TABLE_SIZE)].RemoveAll(index => index < i - HASH_TABLE_SIZE + 1);
                }
                hashTable[Hash(input, i - 1)].Add(i - 1);
            }

            // Encode literals using Huffman encoding (not a complete implementation)
            compressedData.AddRange(EncodeHuffman(literals.ToArray()));

            return compressedData.ToArray();
        }
        #endregion /Compress

        #region Decompress
        public static byte[] Decompress(byte[] compressedData, int originalLength)
        {
            List<byte> decompressedData = new List<byte>();
            int compressedLength = compressedData.Length;
            int i = 0;

            while (i < compressedLength)
            {
                byte token = compressedData[i];
                i++;

                int literalLength = token >> 4;
                if (literalLength == 0x0F)
                {
                    byte lengthByte;
                    do
                    {
                        lengthByte = compressedData[i];
                        i++;
                        literalLength += lengthByte;
                    } while (lengthByte == 0xFF);
                }

                for (int j = 0; j < literalLength; j++)
                {
                    decompressedData.Add(compressedData[i]);
                    i++;
                }

                if (i >= compressedLength)
                {
                    break;
                }

                int matchLength = token & 0x0F;
                if (matchLength == 0x0F)
                {
                    byte lengthByte;
                    do
                    {
                        lengthByte = compressedData[i];
                        i++;
                        matchLength += lengthByte;
                    } while (lengthByte == 0xFF);
                }

                int offset = compressedData[i] | (compressedData[i + 1] << 8);
                i += 2;

                int matchIndex = decompressedData.Count - offset;

                for (int j = 0; j < matchLength; j++)
                {
                    decompressedData.Add(decompressedData[matchIndex + j]);
                }
            }

            if (decompressedData.Count != originalLength)
            {
                throw new Exception("Decompressed data length does not match the original length.");
            }

            return decompressedData.ToArray();
        }
        #endregion /Decompress

        #region Hash Function
        private static int Hash(byte[] data, int index)
        {
            unchecked
            {
                int h = (int)(BitConverter.ToInt32(data, index) * MAGIC_LZ4_HASH);
                return (h >> 12) & (HASH_TABLE_SIZE - 1);
            }
        }
        #endregion /Hash Function

        #region Huffman Encoding
        private static byte[] EncodeHuffman(byte[] data)
        {
            // This is a placeholder for Huffman encoding logic
           
            return data;
        }
        #endregion /Huffman Encoding

        #endregion /Lz4
    }
}




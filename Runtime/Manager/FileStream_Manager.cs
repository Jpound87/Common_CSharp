using Common.Extensions;
using Common.Utility;
using LZ4;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Runtime
{
    public static class Manager_File
    {
        #region Identity
        public static string ClassName = nameof(Manager_File);
        #endregion

        #region Syncronization Objects
        private static readonly ConcurrentDictionary<String, Mutex> dictLock = new ConcurrentDictionary<String, Mutex>();
        #endregion

        #region Create
        public static bool TryCheckExistsOrCreate(String filePath, bool issueAlert = false)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    return true;
                }
                File.Create(filePath);
                if (File.Exists(filePath))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log_Manager.LogError(ClassName, $"Text file method {nameof(TryCheckExistsOrCreate)} failed using path: '{filePath}', Exception: {ex.Message}");
            }
            if(issueAlert)
            {
                Log_Manager.IssueAlert(Translation_Manager.File, Translation_Manager.BadFileName);
            }
            return false;
        }
        #endregion /Text

        #region Binary
        public static bool TryBinarySave_BIN(object SaveObject, string filePath)
        {
            Log_Manager.LogVerbose(ClassName, "TryBinarySave called");
            if (!dictLock.TryLookup(filePath, out Mutex saveLock))
            {
                saveLock = new Mutex();
                dictLock.TryAdd(filePath, saveLock);
            }
            lock (saveLock)
            {
                Log_Manager.LogVerbose(ClassName, "TryBinarySave aquired lock");
                try
                {
                    string directory = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    using (FileStream saveStream = new FileStream(filePath, FileMode.OpenOrCreate))
                    {
                        using (DeflateStream compressionStream = new DeflateStream(saveStream, CompressionLevel.Fastest))
                        {
                            using (BinaryWriter binaryWriter = new BinaryWriter(compressionStream))
                            {
#pragma warning disable SYSLIB0011
                                BinaryFormatter binaryFormatter = new BinaryFormatter();
                                binaryFormatter.Serialize(compressionStream, SaveObject);
#pragma warning restore SYSLIB0011
                                Log_Manager.LogVerbose(ClassName, "TryBinarySave released lock");
                                return true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log_Manager.IssueAlert(ex);
                    if (File.Exists(filePath))
                    {// We have failed and the file should not exist.
                        File.Delete(filePath);
                    }
                    //// We will tell the caller and they decide how to fail.
                }
                Log_Manager.LogVerbose(ClassName, "TryBinarySave released lock");
                return false;
            }
        }

        public static bool TryBinaryLoad_BIN<T>(string filePath, out T recoveredObject)
        {
            Log_Manager.LogVerbose(ClassName,"TryBinaryLoad called");
            if (!dictLock.TryLookup(filePath, out Mutex loadLock))
            {
                loadLock = new Mutex();
                dictLock.TryAdd(filePath, loadLock);
            }
            lock (loadLock)
            {
                Log_Manager.LogVerbose(ClassName, "TryBinaryLoad aquired lock");
                try
                {
                    using (FileStream loadStream = new FileStream(filePath, FileMode.Open))
                    {
                        Log_Manager.LogVerbose(ClassName, "TryBinaryLoad connected load stream");
                        using (DeflateStream decompressionStream = new DeflateStream(loadStream, CompressionMode.Decompress))
                        {
                            Log_Manager.LogVerbose(ClassName, "TryBinaryLoad connected delflate stream");
                            using (BinaryReader binaryWriter = new BinaryReader(decompressionStream))
                            {
                                Log_Manager.LogVerbose(ClassName, "TryBinaryLoad Connected reading binary");
#pragma warning disable SYSLIB0011
                                BinaryFormatter binaryFormatter = new BinaryFormatter();
                                object loadedObject = binaryFormatter.Deserialize(decompressionStream);
#pragma warning restore SYSLIB0011
                                if (loadedObject is T typeCastObject)
                                {
                                    recoveredObject = typeCastObject;
                                    Log_Manager.LogVerbose(ClassName, "TryBinaryLoad released lock");
                                    return true;
                                }
                            }
                        }
                    }
                }
                catch (FileNotFoundException ex)
                {
                    Log_Manager.LogCaughtException(ex);
                }
                catch(Exception ex)
                {
                    Log_Manager.LogCaughtException(ex);
                }
                recoveredObject = default;
                Log_Manager.LogVerbose(ClassName, "TryBinaryLoad released lock");
                return false;
            }
        }
        #endregion /Binary 

        #region Compression

        public static void Compress()
        {
            byte[] input = System.Text.Encoding.ASCII.GetBytes("aaaabbbbcccccddddd");
            byte[] compressed = input.Compress_Lz4();

            Console.WriteLine("Original: " + input.Length + " bytes");
            Console.WriteLine("Compressed: " + compressed.Length + " bytes");

            // Decompression
            using (MemoryStream decompressedStream = new MemoryStream())
            {
                using (MemoryStream compressedStream = new MemoryStream(compressed))
                {
                    using (LZ4Stream lz4Stream = new LZ4Stream(compressedStream, LZ4StreamMode.Decompress))
                    {
                        lz4Stream.CopyTo(decompressedStream);
                    }
                }

                byte[] decompressed = decompressedStream.ToArray();
                Console.WriteLine("Decompressed: " + decompressed.Length + " bytes");
                Console.WriteLine("Decompressed Data: " + System.Text.Encoding.ASCII.GetString(decompressed));
            }
        }

        #endregion /Compression

        #region Externally Launch File
        public static void OpenOutsideFile(String fileName)
        {
            try
            {
                var p = new Process();
                p.StartInfo = new ProcessStartInfo(fileName)
                {
                    UseShellExecute = true
                };
                _ = p.Start();
            }
            catch (Exception ex)
            {
                Log_Manager.LogAssert(ClassName, ex.Message);
            }
        }
        #endregion /Externally Launch File
    }
}

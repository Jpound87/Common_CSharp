using Common.Constant;
using Common.Timers;
using Logging.Interface;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;

namespace Logging
{
    /// <summary>
    /// Responsible for file aspects of logging. This includes:
    /// * Queuing the logging requests.
    /// * Writing to the log messages to disk.
    /// * Managing log files (creating, incrementing number, deleting oldest).
    /// </summary>
    public class FileLogger : ILog
    {
        #region Constants
        private const UInt32 MAX_LOG_FILE_SIZE_MB = 5;
        private const UInt32 MAX_LOG_FILE_SIZE_BYTES = MAX_LOG_FILE_SIZE_MB * Tokens.ONE_MB;
        private const UInt32 MAX_LOG_FILE_COUNT = 10;
        #endregion /Constants

        #region Readonly
        private readonly String LogsDirPath;
        private readonly TickTocker LogTickTocker = new(TimeSpan.FromSeconds(1), false);
        private readonly ConcurrentQueue<LogEntryData> LogEntryDataLines;
        #endregion /Readonly

        #region Globals
        private String CurrentFileName = null;
        #endregion /Globals

        #region Constructor
        public FileLogger(String logsDirPath)
        {
            LogsDirPath = logsDirPath;
            CreateLogFileDirectoryIfNotExists();
            string mostRecent = FindMostRecentLogFileName();
            CurrentFileName = mostRecent ?? "log_0.log";
            LogEntryDataLines = new ConcurrentQueue<LogEntryData>();
            LogTickTocker.Tick += DequeueAndWriteToDisk;
            LogTickTocker.Start();
        }
        #endregion /Constructor

        #region Add
        /// <summary>
        /// Logs entry to disk.
        /// Log file writes are queued and written periodically to disk.
        /// </summary>
        /// <param name="logEntryData">data to log</param>
        public void Add(LogEntryData logEntryData)
        {
            if (LogTickTocker.Running)
            {
                LogEntryDataLines.Enqueue(logEntryData);
            }
        }
        #endregion /Add

        #region Write
        private void DequeueAndWriteToDisk()
        {
            if (LogEntryDataLines.Any())
            {
                FileInfo fileInfo = new FileInfo(Path.Combine(LogsDirPath, CurrentFileName));
                if (fileInfo.Exists && fileInfo.Length >= MAX_LOG_FILE_SIZE_BYTES)
                { // Switch to new log file if current one is getting too large
                    int newIndex = GetFileIndexFromFileName(CurrentFileName) + 1;
                    if (newIndex >= MAX_LOG_FILE_COUNT)
                    {
                        newIndex = 0;
                    }
                    CurrentFileName = $"log_{newIndex}.log";
                    if (File.Exists(Path.Combine(LogsDirPath, CurrentFileName)))
                    {// Delete existing file (old log file)
                        File.Delete(Path.Combine(LogsDirPath, CurrentFileName));
                    }
                }
                // Write to the log file
                using (StreamWriter streamWriter = File.AppendText(Path.Combine(LogsDirPath, CurrentFileName)))
                {
                    string toWrite = DequeueToString();
                    streamWriter.Write(toWrite);
                }
            }
        }
     
        private String DequeueToString()
        {
            StringBuilder logEntrySB = new StringBuilder();
            while (LogEntryDataLines.TryDequeue(out LogEntryData logEntryData))
            {
                logEntrySB.Append(logEntryData.Text);
            }
            return logEntrySB.ToString();
        }
        #endregion /Write

        #region File Helpers
        private void CreateLogFileDirectoryIfNotExists()
        {
            if (!Directory.Exists(LogsDirPath))
            {
                DirectoryInfo logDirectoryInfo = Directory.CreateDirectory(LogsDirPath);
            }
        }

        private String FindMostRecentLogFileName()
        {
            string[] existingLogFiles = Directory.GetFiles(LogsDirPath, "log_*.log");
            if (existingLogFiles.Any())
            {
                DateTime mostRecentTime = DateTime.MinValue;
                string mostRecentFilePath = existingLogFiles[0];
                foreach (var existingFile in existingLogFiles)
                {
                    FileInfo info = new FileInfo(existingFile);
                    if (info.LastWriteTimeUtc > mostRecentTime)
                    {
                        mostRecentTime = info.LastWriteTimeUtc;
                        mostRecentFilePath = existingFile;
                    }
                }
                return Path.GetFileName(mostRecentFilePath);
            }
            else
            {
                return null;
            }
        }

        private static Int32 GetFileIndexFromFileName(String fileName)
        {
            String stripped = fileName.Replace("log_", "");// Remove everything except the digits
            if (!String.IsNullOrWhiteSpace(stripped) && stripped.All(Char.IsDigit))
            {// If we have text and its all digits...
                return Int32.Parse(stripped);
            }
            else
            {// If existing filename doesn't conform then just start at zero
                return 0;
            }
        }
        #endregion /File Helpers
    }

}

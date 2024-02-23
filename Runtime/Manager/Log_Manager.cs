using Common;
using Common.Constant;
using Common.Interface;
using Common.Struct;
using Connections.Interface.Exceptions;
using Logging;
using Logging.Interface;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Runtime
{
    /// <summary>
    /// This class is designed to control messages out to the user.
    /// </summary>
    public static class Log_Manager
    {
        #region ComboBox Choice Array
        private static readonly ComboBoxItem[] choiceBoxes_LogLevels = new ComboBoxItem[6]// V, D, I, W, E, A 
        {
            new ComboBoxItem("Verbose", Priority_Log.Verbose),
            new ComboBoxItem("Debug", Priority_Log.Debug),
            new ComboBoxItem("Information", Priority_Log.Information),
            new ComboBoxItem("Warning", Priority_Log.Warning),
            new ComboBoxItem("Error", Priority_Log.Error),
            new ComboBoxItem("Assert", Priority_Log.Assert)
        };

        public static ComboBoxItem[] ChoiceBoxes_LogLevels
        {
            get
            {
                return choiceBoxes_LogLevels;
            }
        }
        #endregion

        #region Constants
        private const int MAX_ERROR_COUNT = 10;
        #endregion /Constants

        #region Time Constants
        private static readonly TimeSpan alertCollectionTime = TimeSpan.FromMilliseconds(100);
        #endregion

        #region Readonly
        private static readonly ILog Flogger = new FileLogger(Tokens.LOG_FILES_PATH);
        private static readonly ConcurrentBag<string> alertMessages = new ConcurrentBag<string>();
        #endregion /Readonly

        #region Settings
        public static Priority_Log MinimumPriorityToLog { get; set; } = Priority_Log.Verbose;
        #endregion /Settings

        #region Structs

        #region Communication Handler
        public struct CommunicationHandler
        {
            #region Identity
            public const String ClassName = nameof(CommunicationHandler);
            public String Name
            {
                get
                {
                    return ClassName;
                }
            }
            #endregion

            #region Constants
            const int ERROR_LEVEL_COUNT = 3;
            #endregion

            #region Error
            public bool IsCommunicationError { get; private set; }

            public int TotalErrorCount { get; private set; }

            public int ContinuousErrorCount { get; private set; }
            #endregion

            #region Exception Handler
            public void HandleException(ICommunicationException communicationException)
            {
                TotalErrorCount++;
                ContinuousErrorCount++;

                if (ContinuousErrorCount > ERROR_LEVEL_COUNT)
                {
                    IsCommunicationError = true;
                }
            }
            #endregion

            #region Success Handler
            public void SuccessfulCommunication()
            {
                ContinuousErrorCount = 0;
                IsCommunicationError = false;
            }
            #endregion
        }
        #endregion

        #endregion Structs

        #region Version Information
        public static String Version { get; private set; }
        public static void SetVersion(String version)
        {
            Version = version;
        }
        #endregion

        #region Syncronization Objects
        private static Mutex errorMessageOccurred = new Mutex();
        #endregion

        #region Communication Error
        private static readonly CommunicationHandler communicationHandler = new CommunicationHandler();
        public static CommunicationHandler CommunicationError
        {
            get
            {
                return communicationHandler;
            }
        }
        #endregion 

        #region Error Count
        private static bool errorCountIgnored = false;
        private static int errorCount = 0;
        #endregion

        #region Logging Methods

        /// This method should generically handle the logging of constructor calls.
        /// </summary>
        /// <param name="callerName"></param>
        public static void LogConstructor(String callerName)
        {
            LogCommon($"Constructor for {callerName} called.", callerName, Priority_Log.Verbose);
        }

        public static void LogAssert(String caller, String entry)
        {
            LogCommon(entry, caller, Priority_Log.Assert);
        }

        public static void LogError(String caller, String entry)
        {
            LogCommon(entry, caller, Priority_Log.Error);
        }

        public static void LogCaughtException(this Exception ex)
        {
            ExceptionData details = new ExceptionData(ex);
            LogCommon(details.Message, "Exception Occured", Priority_Log.Warning);
        }

        public static void LogError(this ILoggable loggable)
        {
            LogCommon(loggable.LogEntry, loggable.Identity, Priority_Log.Error);
        }

        public static void LogWarning(String caller, String entry)
        {
            LogCommon(entry, caller, Priority_Log.Warning);
        }

        public static void LogInfo(String caller, String entry)
        {
            LogCommon(entry, caller, Priority_Log.Information);
        }

        public static void LogDebug(String caller, String entry)
        {
            LogCommon(entry, caller, Priority_Log.Debug);
        }

        public static void LogVerbose(String caller, String entry)
        {
            LogCommon(entry, caller, Priority_Log.Verbose);
        }

        /// <summary>
        /// This method should generically handle the logging of method calls.
        /// </summary>
        /// <param name="callerName"></param>
        /// <param name="methodName"></param>
        public static void LogMethodCall(string callerName, string methodName)
        {
            LogCommon($"Method {methodName} called.", callerName, Priority_Log.Verbose);
        }

        private static void LogCommon(string caller, string entry, Priority_Log level)
        {
            if (level >= MinimumPriorityToLog)
            {
                Flogger?.Add(new LogEntryData(entry, caller, level));
            }
        }
        #endregion Logging Methods

        #region Debug File
        public static void DeleteDebugFile(string name)
        {
            if (Runtime_Manager.Debug && !String.IsNullOrWhiteSpace(name))
            {//TODO: add debug options to a seperate settings tab with special open characteristics
                string path = Path.Combine(Tokens.DOCUMENTS_DATAM_PATH, $"{name}.txt");
                File.Delete(path);
            }
        }
        public static void WriteDebugFile(string name, string data)
        {
            if (Runtime_Manager.Debug && !String.IsNullOrWhiteSpace(name))
            {//TODO: add debug options to a sepetate settings tab with special open characteristics
                string path = Path.Combine(Tokens.DOCUMENTS_DATAM_PATH, $"{name}.txt");
                File.WriteAllText(path, data);
            }
        }

        public static void AppendDebugFile(string name, string data)
        {
            if (Runtime_Manager.Debug && !String.IsNullOrWhiteSpace(name))
            {//TODO: add debug options to a sepetate settings tab with special open characteristics
                string path = Path.Combine(Tokens.DOCUMENTS_DATAM_PATH, $"{name}.txt");
                File.AppendAllText(path, data);
            }
        }

        #endregion

        #region User Alert
        public static void IssueDebugAlert(String message)
        {
#if DEBUG
            LogDebug("Debug Message Issued", message);
            Debug.WriteLine(message);
#endif
        }

        public static void IssueDebugAlert(String message, String title)
        {
#if DEBUG
            LogDebug(title, message);
            Debug.WriteLine(message);
#endif
        }

        public static void IssueMessage(String message)
        {
            LogInfo("Message Issued", message);
            Runtime_Manager.MakeTask(new Action(() =>
            {
                IssueMessage(message, "Datam");
            }));
        }

        public static void IssueMessage(String message, String title)
        {
            LogInfo(title, message);
                Runtime_Manager.MakeTask(new Action(() =>
                {
                    DialogResult result = MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }));
        }

        public static void IssueAlert(String message, Form caller)// Todo: move to log class
        {
            ExceptionData details = new ExceptionData(message, caller);
            LogError(caller.Text, message);
            IssueUserAlertCommon(details.Message);
        }

        public static void IssueAlert(String message, Object caller)// Todo: move to log class
        {
            ExceptionData details = new ExceptionData(message, caller);
            LogError(nameof(caller), message);
            Flogger.Add(new LogEntryData(details.Message, "Alert Issued!", Priority_Log.Assert));
            IssueUserAlertCommon(details.Message);
        }

        public static void IssueAlert(String title, String message)// Todo: move to log class
        {
            ExceptionData details = new ExceptionData(title, message);
            LogError(title, message);
            Flogger.Add(new LogEntryData(details.Message, "Alert Issued!", Priority_Log.Assert));
            IssueUserAlertCommon(details.Message);
        }

        public static void IssueAlert(Exception ex)
        {
            ExceptionData details = new ExceptionData(ex);
            LogError("Exception Alert Issued!", details.Message);
            IssueUserAlertCommon(details.Message);
        }

        /// <summary>
        /// This method displays a message box informing the user that the form they're trying to access is not supported.
        /// </summary>
        public static DialogResult IssueNotSupported()
        {
            if (Runtime_Manager.Debug)
            {
                DialogResult result = MessageBox.Show(Translation_Manager.Err_PanelNotSupported, Translation_Manager.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return result;
            }
            return DialogResult.None;
        }

        /// <summary>
        /// This method displays a message box informing the user that the device they are using does not have a saved datasheet.
        /// </summary>
        public static DialogResult IssueInvalidDatasheet()
        {
            return MessageBox.Show(Translation_Manager.Msg_DatasheetInvalid, Translation_Manager.Msg_DatasheetError, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        /// <summary>
        /// This method displays a message box informing the user that the device they are using is not supported.
        /// </summary>
        public static DialogResult IssueInvalidDevice()
        {
            return MessageBox.Show(Translation_Manager.Msg_DeviceInvalid, Translation_Manager.Msg_DatasheetError, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        /// <summary>
        /// This method displays a message box informing the user that the form they attempted to open closed unexpectedly.
        /// </summary>
        public static DialogResult IssueUnexpectedClose()
        {
            if (Runtime_Manager.Debug)
            {
                DialogResult result = MessageBox.Show(Translation_Manager.Err_PanelNotSupported, Translation_Manager.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return result;
            }
            return DialogResult.None;
        }

#pragma warning disable IDE0051 // Remove unused private members
        /// <summary>
        /// This is here for SM, who is unfortunatly no longer with us, perhaps it can be used to report catistrphic falure
        /// </summary>
        private static void WhatTerribleFailure()
        {

        }
#pragma warning restore IDE0051 // Remove unused private members

        private static void IssueUserAlertCommon(string message)
        {
            errorCount++;
            if (!errorCountIgnored && errorCount > MAX_ERROR_COUNT)
            {
                lock (errorMessageOccurred)
                {
                    if (errorMessageOccurred == null)
                    {
                        errorMessageOccurred = new Mutex();
                        //DialogResult result = MessageBox.Show("Error count exceeds stable running conditions, shutdown is reccomended. Proceed?", RUNTIME_ERROR, MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        if (Runtime_Manager.Debug)
                        {
                            DialogResult result = MessageBox.Show(Translation_Manager.Err_ErrorCount, Translation_Manager.Err_Runtime, MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                            if (result == DialogResult.Yes)
                            {
                                Runtime_Manager.Reset();
                            }
                            else
                            {
                                errorCountIgnored = true;
                            }
                        }
                        else
                        {
                            //TODO: reset or ignore, rn up to the user.
                        }
                    }
                }
            }
#if DEBUG
            System.Diagnostics.Debug.WriteLine(message);
            if (Runtime_Manager.Debug)
            {
                alertMessages.Add(message);
                Runtime_Manager.MakeTask(AlertBuilder_Run);
            }
#endif
        }


        private static async void AlertBuilder_Run()
        {
            // If too many errors are pending in a row, then allow exiting the application
            await Task.Delay(alertCollectionTime);
            if (alertMessages.Count > MAX_ERROR_COUNT)
            {
                DialogResult result = MessageBox.Show(Translation_Manager.Err_ErrorCount, Translation_Manager.Err_Runtime, MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (result == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }

            if (Runtime_Manager.Debug)
            {
                StringBuilder messanger = new StringBuilder();
                if (alertMessages.TryTake(out String message))
                {
                    if (!string.IsNullOrWhiteSpace(Version))
                    {
                        messanger.AppendLine($"Version: {Version}");
                    }

                    messanger.AppendLine(message);
                    PushNotification(messanger.ToString());
                }
            }
        }

        private static void PushNotification(String message, String title = Tokens.RUNTIME_ERROR)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            errorCount = 0;
        }
        #endregion /User Alert

        #region Standard Messages
        public static String StandardExceptionMessage(this Object sender, Exception ex)
        {
            return StandardExceptionMessage(nameof(sender), ex);
        }
        public static String StandardExceptionMessage(string caller, Exception ex)
        {
            return $"Method {caller} failed with excption message: {ex.Message}";
        }
        #endregion
    }
}


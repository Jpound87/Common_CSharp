using Common.Constant;
using System;
using System.Globalization;

namespace Logging.Interface
{
    #region LogEntryData Interface
    public interface ILog
    {
        #region Methods
        void Add(LogEntryData item);
        #endregion
    }
    #endregion

    /// <summary>
    /// This struct contains all information needed for logging
    /// and is intened for use with the Logging class Add method
    /// </summary>
    public struct LogEntryData
    {
        #region Accessors
        /// <summary>
        /// Log entry time
        /// </summary>
        public DateTime DateTime { get; private set; }

        /// <summary>
        /// Process the calling method is running on
        /// </summary>
        public String Process { get; private set; }

        /// <summary>
        /// Thread the calling process is running on
        /// </summary>
        public String Thread { get; private set; }

        /// <summary>
        /// Software package of the caller
        /// </summary>
        public String Package { get; private set; }

        /// <summary>
        /// Calling method name
        /// </summary>
        public String Tag { get; private set; }

        /// <summary>
        /// The log message
        /// </summary>
        public String Message { get; private set; }

        /// <summary>
        /// Priority level of the calling thread
        /// </summary>
        public Priority_Log Priority { get; private set; }

        /// <summary>
        /// Exception
        /// </summary>
        public Exception Exception { get; private set; }

        public String Text { get; private set; }
        #endregion /Accessors

        #region Constructor
        /// <summary>
        /// This constructor will automatically collect much of the required data
        /// for a log entry specific to the AtControls software
        /// </summary>
        /// <param name="message">String containing the log message</param>
        /// <param name="tag">String containing the calling method name</param>
        /// <param name="priority">Byte cotaining the prority level, can be 
        /// set using the logPriorityLevel enum</param>
        public LogEntryData(string message, string tag, Priority_Log priority) :
            this(message, tag, priority, null)
        {
        }

        /// <summary>
        /// This constructor will automatically collect much of the required data
        /// for a log entry specific to the AtControls software
        /// </summary>
        /// <param name="message">String containing the log message</param>
        /// <param name="tag">String containing the calling method name</param>
        /// <param name="priority">Byte cotaining the prority level, can be 
        /// set using the logPriorityLevel enum</param>
        /// <param name="exception">Exception that needs to logged with null implying no exception</param>
        public LogEntryData(string message, string tag, Priority_Log priority, Exception exception) :
            this("Datam", message, tag, priority, exception)
        {
        }

        /// <summary>
        /// This constructor will automatically collect much of the required data
        /// for a log entry that allows the caller to specify the package
        /// </summary>
        /// <param name="package">String containing the name of the software package</param>
        /// <param name="message">String containing the log message</param>
        /// <param name="tag">String containing the calling method name</param>
        /// <param name="priority">Byte cotaining the prority level, can be 
        /// set using the logPriorityLevel enum</param>
        public LogEntryData(string package, string message, string tag, Priority_Log priority) :
            this(package, message, tag, priority, null)
        {
        }

        /// <summary>
        /// This constructor will automatically collect much of the required data
        /// for a log entry that allows the caller to specify the package
        /// </summary>
        /// <param name="package">String containing the name of the software package</param>
        /// <param name="message">String containing the log message</param>
        /// <param name="tag">String containing the calling method name</param>
        /// <param name="priority">Byte cotaining the prority level, can be 
        /// set using the logPriorityLevel enum</param>
        /// <param name="exception">Exception that needs to logged with null implying no exception</param>
        public LogEntryData(string package, string message, string tag, Priority_Log priority, Exception exception)
        {
            DateTime = DateTime.Now;
            //not sure if this will get the info from the calling method
            Process = System.Diagnostics.Process.GetCurrentProcess().Id.ToString(CultureInfo.InvariantCulture);
            Thread = System.Threading.Thread.CurrentThread.ToString();
            //
            Package = package;
            Tag = tag;
            Message = message;
            Priority = priority;
            Exception = exception;
            Text = string.Format(CultureInfo.InvariantCulture,
                   "{0} {1} {2}-{3}/{4} {5}/{6}: {7}{8}",
                   DateTime.Date.ToShortDateString(),
                   DateTime.ToString("T", CultureInfo.InvariantCulture),
                   Process,
                   Thread,
                   Package,
                   Priority,
                   Tag,
                   Message,
                   Environment.NewLine);
        }
        #endregion /Constructor
    }
}

using GrowthWare.Framework.Model.Enumerations;
using log4net;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;

namespace GrowthWare.Framework.Common
{
    /// <summary>
    /// Class Logger
    /// </summary>
    public class Logger : IDisposable
    {
        const string FILE_NAME_FORMAT = "yyyy_MM_dd";
        private static Logger s_Logger;
        private static Mutex s_Mutex = new Mutex();
        private log4net.Layout.PatternLayout m_Layout = new log4net.Layout.PatternLayout();
        private string m_LogFileName = string.Empty;
        private string m_LogFilePath = ConfigSettings.LogPath;
        private string m_LogFile = string.Empty;
        private log4net.Appender.FileAppender m_Appender = null;
        private DateTime m_AppenderLastUsed;
        private StackTrace m_StackTrace = new StackTrace();
        private int m_CurrentLogLevel;

        private static string s_SCurrentLogLevel = string.Empty;
        private delegate void closeAppender();


        /// <summary>
        /// Gets the current log level.
        /// </summary>
        /// <value>The current log level.</value>
        public int CurrentLogLevel
        {
            get { return this.m_CurrentLogLevel; }
        }

        private Logger()
        {
            init();
        }

        private void init()
        {
            m_LogFile = m_LogFilePath + m_LogFileName;
            m_StackTrace = new StackTrace();
            m_Layout.ConversionPattern = ConfigSettings.ConversionPattern;
            s_SCurrentLogLevel = ConfigSettings.LogPriority.ToString().ToUpper(CultureInfo.InvariantCulture);
            switch (ConfigSettings.LogPriority.ToString().ToUpper(CultureInfo.InvariantCulture))
            {
                case "DEBUG":
                    m_CurrentLogLevel = 0;
                    break;
                case "INFO":
                    m_CurrentLogLevel = 1;
                    break;
                case "WARN":
                    m_CurrentLogLevel = 2;
                    break;
                case "ERROR":
                    m_CurrentLogLevel = 3;
                    break;
                case "FATAL":
                    m_CurrentLogLevel = 4;
                    break;
                default:
                    m_CurrentLogLevel = 3;
                    break;
            }
            deleteOldLogs();
            closeAppender mManageAppender = new closeAppender(manageAppender);
            mManageAppender.BeginInvoke(null, null);
        }

        private void manageAppender()
        {
            do
            {
                TimeSpan mTimeSpan = DateTime.Now.Subtract(m_AppenderLastUsed);
                if (mTimeSpan.TotalMilliseconds >= 3000)
                {
                    if ((m_Appender != null))
                    {
                        m_Appender.Close();
                        m_Appender = null;
                    }
                    //break; // TODO: might not be correct. Was : Exit Do
                }
                Thread.Sleep(3000);
            } while (true);
        }

        private log4net.Appender.FileAppender getAppender()
        {
            m_AppenderLastUsed = DateTime.Now;
            if (m_LogFileName != DateTime.Now.ToString(FILE_NAME_FORMAT, CultureInfo.InvariantCulture) + ".txt")
            {
                m_LogFileName = DateTime.Now.ToString(FILE_NAME_FORMAT, CultureInfo.InvariantCulture) + ".txt";
                m_LogFile = m_LogFilePath + m_LogFileName;
            }
            if (m_Appender == null)
            {
                try
                {
                    m_Appender = new log4net.Appender.FileAppender(m_Layout, m_LogFile, true);
                }
                catch (DirectoryNotFoundException)
                {
                    m_LogFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\";
                    m_LogFile = m_LogFilePath + m_LogFileName;
                    try
                    {
                        Directory.CreateDirectory(m_LogFilePath);
                        m_LogFile = m_LogFilePath + m_LogFileName;
                    }
                    catch
                    {
                        throw;
                    }
                    m_Appender = new log4net.Appender.FileAppender(m_Layout, m_LogFile, true);
                }
            }
            m_Appender.Name = "Logger";
            m_Appender.Threshold = convertPriorityTextToPriority(s_SCurrentLogLevel);
            m_Appender.ImmediateFlush = true;
            log4net.Config.BasicConfigurator.Configure(m_Appender);
            m_Appender.ActivateOptions();
            return m_Appender;
        }

        private void deleteOldLogs()
        {
            int mCounter = 0;
            int mPosSep = 0;
            string[] mAFiles = null;
            string mFile = null;
            int mLogRetention = 0;
            mLogRetention = int.Parse(ConfigSettings.LogRetention, CultureInfo.InvariantCulture);
            if (mLogRetention > 0)
            {
                mLogRetention = mLogRetention * -1;
                System.DateTime mRetentionDate = System.DateTime.Now.AddDays(mLogRetention);
                if (System.IO.Directory.Exists(m_LogFilePath))
                {
                    mAFiles = System.IO.Directory.GetFiles(m_LogFilePath);
                    for (mCounter = 0; mCounter <= mAFiles.GetUpperBound(0); mCounter++)
                    {
                        // Get the position of the trailing separator.
                        mPosSep = mAFiles[mCounter].LastIndexOf("\\", StringComparison.OrdinalIgnoreCase);
                        mFile = mAFiles[mCounter].Substring((mPosSep + 1), mAFiles[mCounter].Length - (mPosSep + 1));
                        mFile = m_LogFilePath + mFile;
                        if (File.GetCreationTime(mFile) < mRetentionDate)
                        {
                            File.Delete(mFile);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns>Logger.</returns>
        public static Logger Instance()
        {
            try
            {
                s_Mutex.WaitOne();
                if (s_Logger == null)
                {
                    s_Logger = new Logger();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                s_Mutex.ReleaseMutex();
            }
            return s_Logger;
        }

        /// <summary>
        /// Sets the threshold.
        /// </summary>
        /// <param name="threshold">The threshold.</param>
        [CLSCompliant(false)]
        public void SetThreshold(LogPriority threshold)
        {
            if (threshold.Equals(LogPriority.Debug))
            {
                s_SCurrentLogLevel = "DEBUG";
                this.m_CurrentLogLevel = 0;
            }
            else if (threshold.Equals(LogPriority.Info))
            {
                s_SCurrentLogLevel = "INFO";
                this.m_CurrentLogLevel = 1;
            }
            else if (threshold.Equals(LogPriority.Warn))
            {
                s_SCurrentLogLevel = "WARN";
                this.m_CurrentLogLevel = 2;
            }
            else if (threshold.Equals(LogPriority.Error))
            {
                s_SCurrentLogLevel = "ERROR";
                this.m_CurrentLogLevel = 3;
            }
            else if (threshold.Equals(LogPriority.Fatal))
            {
                s_SCurrentLogLevel = "FATAL";
                this.m_CurrentLogLevel = 4;
            }
        }

        /// <summary>
        /// Debugs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Debug(object message)
        {
            log(message, LogPriority.Debug);
        }

        /// <summary>
        /// Infoes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info(object message)
        {
            log(message, LogPriority.Info);
        }

        /// <summary>
        /// Warns the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Warn(object message)
        {
            log(message, LogPriority.Warn);
        }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Error(object message)
        {
            log(message, LogPriority.Error);
        }

        /// <summary>
        /// Fatals the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Fatal(object message)
        {
            log(message, LogPriority.Fatal);
        }

        /// <summary>
        /// Will attempt to log the message or exception
        /// </summary>
        /// <param name="message">message or exception object</param>
        /// <param name="priority">LogPriority</param>
        /// <remarks>Will consume any errors ... no need to crash the application because the log did not work.</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void log(object message, LogPriority priority)
        {
            Exception mException = null;
            string mName = m_StackTrace.GetFrame(1).GetMethod().ReflectedType.Name;
            StackFrame mStackFrame = m_StackTrace.GetFrames()[1];
            var mMethod = mStackFrame.GetMethod();
            string mMethodName = mMethod.Name;
            if (mMethodName == "onApplicationError" && m_StackTrace.GetFrames()[2] != null)
            {
                mStackFrame = m_StackTrace.GetFrames()[2];
                mName = mStackFrame.GetMethod().ReflectedType.Name;
                mMethod = mStackFrame.GetMethod();
                mMethodName = mMethod.Name;
            }
            mName += ":" + mMethodName + "()";
            if (!object.ReferenceEquals(message.GetType(), typeof(string)))
            {
                mException = new Exception("Calling: " + mName, mException);
            }
            try
            {
                ILog mLog = LogManager.GetLogger(getAppender().Name);
                switch (priority)
                {
                    case LogPriority.Debug:
                        if (object.ReferenceEquals(message.GetType(), typeof(string)))
                        {
                            mLog.Debug(mName + " " + message);
                        }
                        else
                        {
                            mLog.Debug(mException);
                        }
                        break;
                    case LogPriority.Info:
                        if (object.ReferenceEquals(message.GetType(), typeof(string)))
                        {
                            mLog.Info(mName + " " + message);
                        }
                        else
                        {
                            mLog.Info(mException);
                        }
                        break;
                    case LogPriority.Warn:
                        if (object.ReferenceEquals(message.GetType(), typeof(string)))
                        {
                            mLog.Warn(mName + " " + message);
                        }
                        else
                        {
                            mLog.Warn(mException);
                        }
                        break;
                    case LogPriority.Error:
                        if (object.ReferenceEquals(message.GetType(), typeof(string)))
                        {
                            mLog.Error(mName + " " + message);
                        }
                        else
                        {
                            mLog.Error(mException);
                        }
                        break;
                    case LogPriority.Fatal:
                        if (object.ReferenceEquals(message.GetType(), typeof(string)))
                        {
                            mLog.Fatal(mName + " " + message);
                        }
                        else
                        {
                            mLog.Fatal(mException);
                        }
                        break;
                    default:
                        if (object.ReferenceEquals(message.GetType(), typeof(string)))
                        {
                            mLog.Error(mName + " " + message);
                        }
                        else
                        {
                            mLog.Error(mException);
                        }
                        break;
                }
            }
            catch
            {
                // do nothing ... do not crash the application for
                // the sake of logging.
                // better to be missing a log entry!!!!
                //throw;
            }
        }

        /// <summary>
        /// The convertPriorityTextToPriority method returns the log4net priority given a text value
        /// </summary>
        /// <param name="priority">String value for the desired priority.  Valid values are Debug, Info, Warn, Error, and Fatal any other will return Error</param>
        /// <returns>Returns a Log4Net Priority object.</returns>
        /// <remarks></remarks>
        private static log4net.Priority convertPriorityTextToPriority(string priority)
        {
            log4net.Priority retPriority = null;
            switch (priority.ToUpper(CultureInfo.InvariantCulture))
            {
                case "DEBUG":
                    retPriority = log4net.Priority.DEBUG;
                    break;
                case "INFO":
                    retPriority = log4net.Priority.INFO;
                    break;
                case "WARN":
                    retPriority = log4net.Priority.WARN;
                    break;
                case "ERROR":
                    retPriority = log4net.Priority.ERROR;
                    break;
                case "FATAL":
                    retPriority = log4net.Priority.FATAL;
                    break;
                default:
                    retPriority = log4net.Priority.ERROR;
                    break;
            }
            return retPriority;
        }

        /// <summary>
        /// Gets the log priority from text.
        /// </summary>
        /// <param name="priority">The priority.</param>
        /// <returns>LogPriority.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public LogPriority GetLogPriorityFromText(string priority)
        {
            LogPriority mRetVal = LogPriority.Error;
            if (!String.IsNullOrEmpty(priority)) 
            {
                switch (priority.ToUpper(CultureInfo.InvariantCulture))
                {
                    case "DEBUG":
                        mRetVal = LogPriority.Debug;
                        break;
                    case "INFO":
                        mRetVal = LogPriority.Info;
                        break;
                    case "WARN":
                        mRetVal = LogPriority.Warn;
                        break;
                    case "ERROR":
                        mRetVal = LogPriority.Error;
                        break;
                    case "FATAL":
                        mRetVal = LogPriority.Fatal;
                        break;
                    default:
                        mRetVal = LogPriority.Error;
                        break;
                }
            }
            return mRetVal;
        }

        // To detect redundant calls

        private bool disposedValue = false;
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if ((m_Appender != null))
                {
                    m_Appender.Close();
                    m_Appender = null;
                }
                if (disposing)
                {
                    // TODO: free shared unmanaged resources
                }
            }
            this.disposedValue = true;
        }

        #region " IDisposable Support "
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}

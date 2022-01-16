using GrowthWare.Framework.Enumerations;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;

namespace GrowthWare.Framework
{
    /// <summary>
    /// Class Logger
    /// </summary>
    public class Logger : IDisposable
    {
        /*
        * TODO: If we are going to log information to the database
        *   then this class will be the exception to the rule that no object
        *   will have access to the database outside of the DataAccess project!
        */
        const string FILE_NAME_FORMAT = "yyyy_MM_dd";
        private static Logger s_Logger;
        private static Mutex s_Mutex = new Mutex();
        private NLog.Logger m_Logger = NLog.LogManager.GetCurrentClassLogger();
        private string m_LogFileName = string.Empty;
        private string m_LogFilePath = ConfigSettings.LogPath;
        private string m_LogFile = string.Empty;
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
            DeleteOldLogs();
        }
        private void DeleteOldLogs()
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
            StackTrace mStackTrace = new StackTrace();
            Exception mException = null;
            string mName = string.Empty;
            string mMethodName = string.Empty;

            StackFrame[] mStackFrames = mStackTrace.GetFrames();  // get method calls (frames)

            int mCurrentStackFrameNumber = 0;
            foreach (StackFrame stackFrame in mStackFrames)
            {
                mCurrentStackFrameNumber += 1;
                //Console.WriteLine(stackFrame.GetMethod().Name);   // write method name
                if (stackFrame.GetMethod().Name == "log")
                {
                    mName = mStackTrace.GetFrame(mCurrentStackFrameNumber + 1).GetMethod().ReflectedType.Name;
                    mMethodName = mStackTrace.GetFrame(mCurrentStackFrameNumber + 1).GetMethod().Name;
                    break;
                }
            }
            mName += "::" + mMethodName + "()";
            if (!object.ReferenceEquals(message.GetType(), typeof(string)))
            {
                mException = (Exception)message;
                mException = new Exception("Calling: " + mName, mException);
            }
            try
            {
                switch (priority)
                {
                    case LogPriority.Debug:
                        if (object.ReferenceEquals(message.GetType(), typeof(string)))
                        {
                            m_Logger.Debug(mName + " " + message);
                        }
                        else
                        {
                            m_Logger.Debug(mException);
                        }
                        break;
                    case LogPriority.Info:
                        if (object.ReferenceEquals(message.GetType(), typeof(string)))
                        {
                            m_Logger.Info(mName + " " + message);
                        }
                        else
                        {
                            m_Logger.Info(mException);
                        }
                        break;
                    case LogPriority.Warn:
                        if (object.ReferenceEquals(message.GetType(), typeof(string)))
                        {
                            m_Logger.Warn(mName + " " + message);
                        }
                        else
                        {
                            m_Logger.Warn(mException);
                        }
                        break;
                    case LogPriority.Error:
                        if (object.ReferenceEquals(message.GetType(), typeof(string)))
                        {
                            m_Logger.Error(mName + " " + message);
                        }
                        else
                        {
                            m_Logger.Error(mException);
                        }
                        break;
                    case LogPriority.Fatal:
                        if (object.ReferenceEquals(message.GetType(), typeof(string)))
                        {
                            m_Logger.Fatal(mName + " " + message);
                        }
                        else
                        {
                            m_Logger.Fatal(mException);
                        }
                        break;
                    default:
                        if (object.ReferenceEquals(message.GetType(), typeof(string)))
                        {
                            m_Logger.Error(mName + " " + message);
                        }
                        else
                        {
                            m_Logger.Error(mException);
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

        private bool disposedValue = false;
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
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

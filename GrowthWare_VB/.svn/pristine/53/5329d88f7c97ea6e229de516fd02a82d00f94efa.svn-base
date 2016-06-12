Imports System.IO
Imports System.Threading
Imports System.Web
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Profiles
Imports log4net

Namespace Utilities
	Public NotInheritable Class LogUtility
		Implements IDisposable

		Const FILE_NAME_FORMAT As String = "yyyy_MM_dd"

		Private m_DisposedValue As Boolean = False		' To detect redundant calls
		Private Shared mLogUtility As LogUtility
		Private Shared mMutex As New Mutex
		Private Shared mLayout As New log4net.Layout.PatternLayout
		Private Shared mLogFileName As String = DateTime.Now.ToString(FILE_NAME_FORMAT) & ".txt"
		Private Shared mLogFilePath As String = ConfigSettings.LogPath
		Private Shared mLogFile As String = mLogFilePath & mLogFileName
		Private Shared mStackTrace As StackTrace = New StackTrace
		Private Shared mCurrentLogLevel As Integer
		Private Shared mSCurrentLogLevel As String = String.Empty
		Private Shared m_FileAppender As log4net.Appender.FileAppender = GetAppender()

		''' <summary>
		''' Inticates the current logging level.
		''' </summary>
		''' <value>integer</value>
		''' <returns>integer</returns>
		''' <remarks></remarks>
		Public ReadOnly Property CurrentLogLevel() As Integer
			Get
				Return mCurrentLogLevel
			End Get
		End Property

		Private Sub New()
			init()
		End Sub

		'*********************************************************************
		'
		' Sub init performs all of the first time intialization for the LogUtility
		'
		'*********************************************************************
		Private Sub init()
			mLayout.ConversionPattern = ConfigSettings.ConversionPattern
			Select Case ConfigSettings.LogPriority.ToLower
				Case "debug"
					mCurrentLogLevel = 0
					mSCurrentLogLevel = "DEBUG"
				Case "info"
					mCurrentLogLevel = 1
					mSCurrentLogLevel = "INFO"
				Case "warn"
					mCurrentLogLevel = 2
					mSCurrentLogLevel = "WARN"
				Case "error"
					mCurrentLogLevel = 3
					mSCurrentLogLevel = "ERROR"
				Case "fatal"
					mCurrentLogLevel = 4
					mSCurrentLogLevel = "FATAL"
				Case Else
					mCurrentLogLevel = 3
					mSCurrentLogLevel = "ERROR"
			End Select
			DeleteOldLogs()
		End Sub	'init

		Private Shared Function GetAppender() As log4net.Appender.FileAppender
			Dim retAppender As log4net.Appender.FileAppender = Nothing
			Try
				retAppender = New log4net.Appender.FileAppender(mLayout, mLogFile, True)
			Catch ex As Exception
				mLogFilePath = HttpContext.Current.Server.MapPath("~\Logs\")
				mLogFile = mLogFilePath & mLogFileName
				Try
					Dim DirectoryProfile As New MDirectoryProfile
					FileUtility.CreateDirectory(HttpContext.Current.Server.MapPath("~\"), "Logs", DirectoryProfile)
					mLogFile = mLogFilePath & mLogFileName
				Catch ex2 As Exception
					Throw ex2
				End Try
				retAppender = New log4net.Appender.FileAppender(mLayout, mLogFile, True)
			End Try
			'retAppender.AppendToFile = ConfigSettings.AppendToFile
			retAppender.Name = mStackTrace.GetFrame(1).GetMethod.ReflectedType.Name
			'retAppender.Name = "LogUtility"
			retAppender.Threshold = ConvertPriorityTextToPriority(mSCurrentLogLevel)
			retAppender.ImmediateFlush = True
			log4net.Config.BasicConfigurator.Configure(retAppender)
			retAppender.ActivateOptions()
			Return retAppender
		End Function

		Private Sub DeleteOldLogs()
			Dim mCounter As Integer
			Dim mPosSep As Integer
			Dim mAFiles() As String
			Dim mFile As String
			Dim mLogRetention As Integer = 0
			mLogRetention = Integer.Parse(ConfigSettings.LogRetention)
			If mLogRetention > 0 Then
				mLogRetention = mLogRetention * -1
				Dim mRetentionDate As Date = Date.Now.AddDays(mLogRetention)
				If System.IO.Directory.Exists(mLogFilePath) Then
					mAFiles = System.IO.Directory.GetFiles(mLogFilePath)
					For mCounter = 0 To mAFiles.GetUpperBound(0)
						' Get the position of the trailing separator.
						mPosSep = mAFiles(mCounter).LastIndexOf("\")
						mFile = mAFiles(mCounter).Substring((mPosSep + 1), mAFiles(mCounter).Length - (mPosSep + 1))
						mFile = mLogFilePath & mFile
						If File.GetCreationTime(mFile) < mRetentionDate Then
							Try
								File.Delete(mFile)
							Catch ex As Exception
								' could not delete don't worrie about it.
							End Try
						End If
					Next mCounter
				End If
			End If
		End Sub

		Public Shared Function GetInstance() As LogUtility
			Try
				mMutex.WaitOne()
				If mLogUtility Is Nothing Then
					mLogUtility = New LogUtility
				End If
			Catch ex As Exception
				Throw ex
			Finally
				mMutex.ReleaseMutex()
				mStackTrace = New StackTrace
				mLogFile = mLogFilePath & mLogFileName
			End Try
			Return mLogUtility
		End Function	'GetInstance

		' SetThreshold allows a developer to set the 
		' logging priority during runtime
		Public Sub SetThreshold(ByVal threshold As log4net.Priority)
			If Not threshold Is Nothing Then
				If threshold.Equals(log4net.Priority.DEBUG) Then
					mSCurrentLogLevel = "DEBUG"
					mCurrentLogLevel = 0
				ElseIf threshold.Equals(log4net.Priority.INFO) Then
					mSCurrentLogLevel = "INFO"
					mCurrentLogLevel = 1
				ElseIf threshold.Equals(log4net.Priority.WARN) Then
					mSCurrentLogLevel = "WARN"
					mCurrentLogLevel = 2
				ElseIf threshold.Equals(log4net.Priority.ERROR) Then
					mSCurrentLogLevel = "ERROR"
					mCurrentLogLevel = 3
				ElseIf threshold.Equals(log4net.Priority.FATAL) Then
					mSCurrentLogLevel = "FATAL"
					mCurrentLogLevel = 4
				End If
			End If
		End Sub

		Public Sub Debug(ByVal message As Object)
			'Dim myAppender As log4net.Appender.FileAppender = GetAppender()
			Dim log As ILog = LogManager.GetLogger(m_FileAppender.Name)
			log.Debug(message)
			'm_FileAppender.Close()
		End Sub

		Public Sub Info(ByVal message As Object)
			'Dim myAppender As log4net.Appender.FileAppender = GetAppender()
			Dim log As ILog = LogManager.GetLogger(m_FileAppender.Name)
			log.Info(message)
			'm_FileAppender.Close()
		End Sub

		Public Sub Warn(ByVal message As Object)
			'Dim myAppender As log4net.Appender.FileAppender = GetAppender()
			Dim log As ILog = LogManager.GetLogger(m_FileAppender.Name)
			log.Warn(message)
			'myAppender.Close()
		End Sub

		Public Sub [Error](ByVal message As Object)
			'Dim myAppender As log4net.Appender.FileAppender = GetAppender()
			Dim log As ILog = LogManager.GetLogger(m_FileAppender.Name)
			log.Error(message)
			'myAppender.Close()
		End Sub

		Public Sub Fatal(ByVal message As Object)
			'Dim myAppender As log4net.Appender.FileAppender = GetAppender()
			Dim log As ILog = LogManager.GetLogger(m_FileAppender.Name)
			log.Error(message)
			'myAppender.Close()
		End Sub

		''' <summary>
		''' The convertPriorityTextToPriority method returns the log4net priority given a text value
		''' </summary>
		''' <param name="priority">String value for the desired priority.  Valid values are Debug, Info, Warn, Error, and Fatal any other will return Error</param>
		''' <returns>Returns a Log4Net Priority object.</returns>
		''' <remarks></remarks>
		Public Shared Function ConvertPriorityTextToPriority(ByVal priority As String) As log4net.Priority
			Dim retPriority As log4net.Priority
			Select Case priority.ToUpper
				Case "DEBUG"
					retPriority = log4net.Priority.DEBUG
				Case "INFO"
					retPriority = log4net.Priority.INFO
				Case "WARN"
					retPriority = log4net.Priority.WARN
				Case "ERROR"
					retPriority = log4net.Priority.ERROR
				Case "FATAL"
					retPriority = log4net.Priority.FATAL
				Case Else
					retPriority = log4net.Priority.ERROR
			End Select
			Return retPriority
		End Function

		' IDisposable
		Public Sub Dispose(ByVal disposing As Boolean)
			If Not Me.m_DisposedValue Then
				If disposing Then
					' nothing to do at the moment
				End If
				' TODO: free shared unmanaged resources
			End If
			Me.m_DisposedValue = True
		End Sub

#Region " IDisposable Support "
		' This code added by Visual Basic to correctly implement the disposable pattern.
		Public Sub Dispose() Implements IDisposable.Dispose
			' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
			Dispose(True)
			GC.SuppressFinalize(Me)
		End Sub
#End Region


	End Class
End Namespace
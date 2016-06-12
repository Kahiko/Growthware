Imports System.Threading
Imports System.Reflection
Imports System.Diagnostics
Imports log4net
#Region " Notes "
' The AppLogger class gives access to a common logger
' in this case a open source projected called log4net
#End Region
Public Class AppLogger
	Private Shared _mAppLogger As AppLogger
	Private Shared _mu As New Mutex
	Private Shared myLayout As New log4net.Layout.PatternLayout
	Private Shared LogFileName As String = BaseHelper.LogPath & Format(Now, "MMddyyyy") & ".txt"
	Private Shared myAppender As New log4net.Appender.FileAppender(myLayout, LogFileName)
	Private Shared theStackTrace As StackTrace = New StackTrace
	Private Shared _CurrentLogLevel As Integer

	Public ReadOnly Property CurrentLogLevel() As Integer
		' Returns the current logging level
		Get
			Return _CurrentLogLevel
		End Get
	End Property

	Private Sub New()
		init()
	End Sub

	'*********************************************************************
	'
	' Sub init performs all of the first time intialization for the AppLogger
	'
	'*********************************************************************
	Private Sub init()
		myLayout.ConversionPattern = ConfigurationSettings.AppSettings("ConversionPattern")
		myAppender.AppendToFile = CBool(ConfigurationSettings.AppSettings("AppendToFile"))
		Dim InitialLogPriority As log4net.Priority
		Select Case ConfigurationSettings.AppSettings(BaseHelper.Environment & "LogPriority").ToLower
			Case "debug"
				InitialLogPriority = log4net.Priority.DEBUG
				_CurrentLogLevel = 0
			Case "info"
				InitialLogPriority = log4net.Priority.INFO
				_CurrentLogLevel = 1
			Case "warn"
				InitialLogPriority = log4net.Priority.WARN
				_CurrentLogLevel = 2
			Case "error"
				InitialLogPriority = log4net.Priority.ERROR
				_CurrentLogLevel = 3
			Case "fatal"
				InitialLogPriority = log4net.Priority.FATAL
				_CurrentLogLevel = 4
			Case Else
				InitialLogPriority = log4net.Priority.ERROR
				_CurrentLogLevel = 3
		End Select
		myAppender.Threshold = InitialLogPriority
		myAppender.Name = theStackTrace.GetFrame(1).GetMethod.ReflectedType.Name
		log4net.Config.BasicConfigurator.Configure(myAppender)
	End Sub	'init

	Public Shared Function GetInstance() As AppLogger
		' returns an instance of the AppLogger
		Try
			_mu.WaitOne()
			If _mAppLogger Is Nothing Then
				_mAppLogger = New AppLogger
			End If
		Catch ex As Exception
			'Throw ex
		Finally
			_mu.ReleaseMutex()
			theStackTrace = New StackTrace
			LogFileName = ConfigurationSettings.AppSettings("LogPath") & Format(Now, "MMddyyyy") & ".txt"
			myAppender.File = LogFileName
			myAppender.Name = theStackTrace.GetFrame(1).GetMethod.ReflectedType.Name
			log4net.Config.BasicConfigurator.Configure(myAppender)
		End Try
		Return _mAppLogger
	End Function	'GetInstance

	' SetThreshold allows a developer to set the 
	' logging priority during runtime
	Public Sub SetThreshold(ByVal Threshold As log4net.Priority)
		myAppender.Threshold = Threshold
		log4net.Config.BasicConfigurator.Configure(myAppender)
		If Threshold.Equals(log4net.Priority.DEBUG) Then
			_CurrentLogLevel = 0
		ElseIf Threshold.Equals(log4net.Priority.INFO) Then
			_CurrentLogLevel = 1
		ElseIf Threshold.Equals(log4net.Priority.WARN) Then
			_CurrentLogLevel = 2
		ElseIf Threshold.Equals(log4net.Priority.ERROR) Then
			_CurrentLogLevel = 3
		ElseIf Threshold.Equals(log4net.Priority.FATAL) Then
			_CurrentLogLevel = 4
		End If
	End Sub

	Public Sub Debug(ByVal Message As Object)
		Dim log As ILog = LogManager.GetLogger(myAppender.Name)
		log.Debug(Message)
	End Sub

	Public Sub Info(ByVal Message As Object)
		Dim log As ILog = LogManager.GetLogger(myAppender.Name)
		log.Info(Message)
	End Sub

	Public Sub Warn(ByVal Message As Object)
		Dim log As ILog = LogManager.GetLogger(myAppender.Name)
		log.Warn(Message)
	End Sub

	Public Sub [Error](ByVal Message As Object)
		Dim log As ILog = LogManager.GetLogger(myAppender.Name)
		log.Error(Message)
	End Sub

	Public Sub Fatal(ByVal Message As Object)
		Dim log As ILog = LogManager.GetLogger(myAppender.Name)
		log.Error(Message)
	End Sub


	'*********************************************************************
	'
	' The convertPriorityTextToPriority method returns the
	' log4net priority given a text value
	'
	'*********************************************************************
	Public Function convertPriorityTextToPriority(ByVal priority As String) As log4net.Priority
		Dim retPriority As log4net.Priority
		Select Case ConfigurationSettings.AppSettings(BaseHelper.Environment & "LogPriority").ToLower
			Case "debug"
				retPriority = log4net.Priority.DEBUG
			Case "info"
				retPriority = log4net.Priority.INFO
			Case "warn"
				retPriority = log4net.Priority.WARN
			Case "error"
				retPriority = log4net.Priority.ERROR
			Case "fatal"
				retPriority = log4net.Priority.FATAL
			Case Else
				retPriority = log4net.Priority.ERROR
		End Select
		Return retPriority
	End Function	'convertPriorityTextToPriority
End Class 'AppLogger
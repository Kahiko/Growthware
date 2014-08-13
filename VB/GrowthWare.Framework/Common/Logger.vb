Imports GrowthWare.Framework.Model.Enumerations
Imports log4net
Imports System
Imports System.Diagnostics
Imports System.Globalization
Imports System.Threading
Imports System.IO
Imports System.Reflection

Namespace Common
    Public Class Logger
        Implements IDisposable

        Const FILE_NAME_FORMAT As String = "yyyy_MM_dd"
        Private Shared s_GWLogger As Logger
        Private Shared s_Mutex As New Mutex()
        Private m_Layout As New log4net.Layout.PatternLayout()
        Private m_LogFileName As String = String.Empty
        Private m_LogFilePath As String = ConfigSettings.LogPath
        Private m_LogFile As String = m_LogFilePath + m_LogFileName
        Private m_Appender As log4net.Appender.FileAppender = Nothing
        Private m_AppenderLastUsed As DateTime
        Private m_CurrentLogLevel As Integer
        Private Shared s_SCurrentLogLevel As String = String.Empty

        Private Delegate Sub closeAppender()

        ''' <summary>
        ''' Gets the current log level.
        ''' </summary>
        ''' <value>The current log level.</value>
        Public ReadOnly Property CurrentLogLevel() As Integer
            Get
                Return Me.m_CurrentLogLevel
            End Get
        End Property

        Private Sub New()
            init()
        End Sub

        Private Sub init()
            m_StackTrace = New StackTrace()
            m_LogFile = m_LogFilePath & m_LogFileName
            m_Layout.ConversionPattern = ConfigSettings.ConversionPattern
            s_SCurrentLogLevel = ConfigSettings.LogPriority.ToString().ToUpper(CultureInfo.InvariantCulture)
            Select Case ConfigSettings.LogPriority.ToString().ToUpper(CultureInfo.InvariantCulture)
                Case "debug"
                    Me.m_CurrentLogLevel = 0
                    Exit Select
                Case "info"
                    Me.m_CurrentLogLevel = 1
                    Exit Select
                Case "warn"
                    Me.m_CurrentLogLevel = 2
                    Exit Select
                Case "error"
                    Me.m_CurrentLogLevel = 3
                    Exit Select
                Case "fatal"
                    Me.m_CurrentLogLevel = 4
                    Exit Select
                Case Else
                    Me.m_CurrentLogLevel = 3
                    Exit Select
            End Select
            deleteOldLogs()
            Dim mManageAppender As closeAppender = New closeAppender(AddressOf manageAppender)
            mManageAppender.BeginInvoke(Nothing, Nothing)
        End Sub

        Private Sub manageAppender()
            Do
                Dim mTimeSpan As TimeSpan = DateTime.Now().Subtract(m_AppenderLastUsed)
                If mTimeSpan.TotalMilliseconds >= 3000 Then
                    If Not m_Appender Is Nothing Then
                        m_Appender.Close()
                        m_Appender = Nothing
                    End If
                    'Exit Do
                End If
                Thread.Sleep(3000)
            Loop
        End Sub

        Private Function getAppender() As log4net.Appender.FileAppender
            If m_LogFileName <> DateTime.Now.ToString(FILE_NAME_FORMAT, CultureInfo.InvariantCulture) + ".txt" Then
                m_LogFileName = DateTime.Now.ToString(FILE_NAME_FORMAT, CultureInfo.InvariantCulture) + ".txt"
                m_LogFile = m_LogFilePath + m_LogFileName
            End If
            m_AppenderLastUsed = DateTime.Now()
            If m_Appender Is Nothing Then
                Try
                    m_Appender = New log4net.Appender.FileAppender(m_Layout, m_LogFile, True)
                Catch ex As DirectoryNotFoundException
                    m_LogFilePath = AppDomain.CurrentDomain.BaseDirectory + "\Logs\"
                    m_LogFile = m_LogFilePath & m_LogFileName
                    Try
                        Directory.CreateDirectory(m_LogFilePath)
                        m_LogFile = m_LogFilePath & m_LogFileName
                    Catch generatedExceptionName As Exception
                        Throw
                    End Try
                    m_Appender = New log4net.Appender.FileAppender(m_Layout, m_LogFile, True)
                End Try
            End If
            m_Appender.Name = "LogUtility"
            m_Appender.Threshold = convertPriorityTextToPriority(s_SCurrentLogLevel)
            m_Appender.ImmediateFlush = True
            log4net.Config.BasicConfigurator.Configure(m_Appender)
            m_Appender.ActivateOptions()
            Return m_Appender
        End Function

        Private Sub deleteOldLogs()
            Dim mCounter As Integer = 0
            Dim mPosSep As Integer = 0
            Dim mAFiles As String() = Nothing
            Dim mFile As String = Nothing
            Dim mLogRetention As Integer = 0
            mLogRetention = Integer.Parse(ConfigSettings.LogRetention, CultureInfo.InvariantCulture)
            If mLogRetention > 0 Then
                mLogRetention = mLogRetention * -1
                Dim mRetentionDate As System.DateTime = System.DateTime.Now.AddDays(mLogRetention)
                If System.IO.Directory.Exists(m_LogFilePath) Then
                    mAFiles = System.IO.Directory.GetFiles(m_LogFilePath)
                    For mCounter = 0 To mAFiles.GetUpperBound(0)
                        ' Get the position of the trailing separator.
                        mPosSep = mAFiles(mCounter).LastIndexOf("\", StringComparison.OrdinalIgnoreCase)
                        mFile = mAFiles(mCounter).Substring((mPosSep + 1), mAFiles(mCounter).Length - (mPosSep + 1))
                        mFile = m_LogFilePath & mFile
                        If File.GetCreationTime(mFile) < mRetentionDate Then
                            File.Delete(mFile)
                        End If
                    Next
                End If
            End If
        End Sub

        ''' <summary>
        ''' Gets the instance.
        ''' </summary>
        ''' <returns>LogUtility.</returns>
        Public Shared Function Instance() As Logger
            Try
                s_Mutex.WaitOne()
                If s_GWLogger Is Nothing Then
                    s_GWLogger = New Logger()
                End If
            Catch
                Throw
            Finally
                s_Mutex.ReleaseMutex()
            End Try
            Return s_GWLogger
        End Function

        ''' <summary>
        ''' Sets the threshold.
        ''' </summary>
        ''' <param name="Threshold">The threshold.</param>
        <CLSCompliant(False)> _
        Public Sub SetThreshold(ByVal threshold As LogPriority)
            If threshold.Equals(Model.Enumerations.LogPriority.Debug) Then
                s_SCurrentLogLevel = "DEBUG"
                Me.m_CurrentLogLevel = 0
            ElseIf threshold.Equals(Model.Enumerations.LogPriority.Info) Then
                s_SCurrentLogLevel = "INFO"
                Me.m_CurrentLogLevel = 1
            ElseIf threshold.Equals(Model.Enumerations.LogPriority.Warn) Then
                s_SCurrentLogLevel = "WARN"
                Me.m_CurrentLogLevel = 2
            ElseIf threshold.Equals(Model.Enumerations.LogPriority.[Error]) Then
                s_SCurrentLogLevel = "ERROR"
                Me.m_CurrentLogLevel = 3
            ElseIf threshold.Equals(Model.Enumerations.LogPriority.Fatal) Then
                s_SCurrentLogLevel = "FATAL"
                Me.m_CurrentLogLevel = 4
            End If
        End Sub

        ''' <summary>
        ''' Debugs the specified message.
        ''' </summary>
        ''' <param name="message">The message.</param>
        Public Sub Debug(ByVal message As Object)
            Me.log(message, Model.Enumerations.LogPriority.Debug)
        End Sub

        ''' <summary>
        ''' Infoes the specified message.
        ''' </summary>
        ''' <param name="message">The message.</param>
        Public Sub Info(ByVal message As Object)
            Me.log(message, Model.Enumerations.LogPriority.Info)
        End Sub

        ''' <summary>
        ''' Warns the specified message.
        ''' </summary>
        ''' <param name="message">The message.</param>
        Public Sub Warn(ByVal message As Object)
            Me.log(message, Model.Enumerations.LogPriority.Warn)
        End Sub

        ''' <summary>
        ''' Errors the specified message.
        ''' </summary>
        ''' <param name="message">The message.</param>
        Public Sub [Error](ByVal message As Object)
            Me.log(message, Model.Enumerations.LogPriority.Error)
        End Sub

        ''' <summary>
        ''' Fatals the specified message.
        ''' </summary>
        ''' <param name="message">The message.</param>
        Public Sub Fatal(ByVal message As Object)
            Me.log(message, Model.Enumerations.LogPriority.Error)
        End Sub

        ''' <summary>
        ''' Will attempt to log the message or exception
        ''' </summary>
        ''' <param name="message">message or exception object</param>
        ''' <param name="priority">LogPriority</param>
        ''' <remarks>Will consume any errors ''' no need to crash the application because the log did not work.</remarks>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
        Private Sub log(ByVal message As Object, priority As LogPriority)
            Dim mFramed As StackFrame = New StackFrame(2, True)
            Dim mException As Exception = Nothing
            Dim mName As String = mFramed.GetMethod.ReflectedType.Name + ":" + mFramed.GetMethod.Name
            If message.GetType() IsNot GetType(String) Then
                mException = New Exception("Calling: " + mName, message)
            End If
            Try
                Dim mLog As ILog = LogManager.GetLogger(getAppender().Name)

                Select Case priority
                    Case Model.Enumerations.LogPriority.Debug
                        If message.GetType() Is GetType(String) Then
                            mLog.Debug(mName + " " + message)
                        Else
                            mLog.Debug(mException)
                        End If
                        Exit Select
                    Case Model.Enumerations.LogPriority.Info
                        If message.GetType() Is GetType(String) Then
                            mLog.Info(mName + " " + message)
                        Else
                            mLog.Info(mException)
                        End If
                        Exit Select
                    Case Model.Enumerations.LogPriority.Warn
                        If message.GetType() Is GetType(String) Then
                            mLog.Warn(mName + " " + message)
                        Else
                            mLog.Warn(mException)
                        End If
                        Exit Select
                    Case Model.Enumerations.LogPriority.Error
                        If message.GetType() Is GetType(String) Then
                            mLog.Error(mName + " " + message)
                        Else
                            mLog.Error(mException)
                        End If
                        Exit Select
                    Case Model.Enumerations.LogPriority.Fatal
                        If message.GetType() Is GetType(String) Then
                            mLog.Fatal(mName + " " + message)
                        Else
                            mLog.Fatal(mException)
                        End If
                        Exit Select
                    Case Else
                        If message.GetType() Is GetType(String) Then
                            mLog.Error(mName + " " + message)
                        Else
                            mLog.Error(mException)
                        End If
                End Select

            Catch ex As Exception
                ' do nothing ''' do not crash the application for
                ' the sake of logging.
                ' better to be missing a log entry!!!!
                'throw;
            End Try
        End Sub

        ''' <summary>
        ''' The convertPriorityTextToPriority method returns the log4net priority given a text value
        ''' </summary>
        ''' <param name="priority">String value for the desired priority.  Valid values are Debug, Info, Warn, Error, and Fatal any other will return Error</param>
        ''' <returns>Returns a Log4Net Priority object.</returns>
        ''' <remarks></remarks>
        <CLSCompliant(False)> _
        Private Shared Function convertPriorityTextToPriority(ByVal priority As String) As log4net.Priority
            Dim retPriority As log4net.Priority = Nothing
            Select Case priority.ToUpper(New CultureInfo("en-US", False))
                Case "DEBUG"
                    retPriority = log4net.Priority.DEBUG
                    Exit Select
                Case "INFO"
                    retPriority = log4net.Priority.INFO
                    Exit Select
                Case "WARN"
                    retPriority = log4net.Priority.WARN
                    Exit Select
                Case "ERROR"
                    retPriority = log4net.Priority.[ERROR]
                    Exit Select
                Case "FATAL"
                    retPriority = log4net.Priority.FATAL
                    Exit Select
                Case Else
                    retPriority = log4net.Priority.[ERROR]
                    Exit Select
            End Select
            Return retPriority
        End Function

        ''' <summary>
        ''' Gets the log priority from text.
        ''' </summary>
        ''' <param name="priority">The priority.</param>
        ''' <returns>LogPriority.</returns>
        Public Function GetLogPriorityFromText(ByVal priority As String) As LogPriority
            Dim mRetVal As LogPriority = Model.Enumerations.LogPriority.[Error]
            If Not String.IsNullOrEmpty(priority) Then
                Select Case priority.ToUpper(New CultureInfo("en-US", False))
                    Case "DEBUG"
                        mRetVal = Model.Enumerations.LogPriority.Debug
                        Exit Select
                    Case "INFO"
                        mRetVal = Model.Enumerations.LogPriority.Info
                        Exit Select
                    Case "WARN"
                        mRetVal = Model.Enumerations.LogPriority.Warn
                        Exit Select
                    Case "ERROR"
                        mRetVal = Model.Enumerations.LogPriority.[Error]
                        Exit Select
                    Case "FATAL"
                        mRetVal = Model.Enumerations.LogPriority.Fatal
                        Exit Select
                    Case Else
                        mRetVal = Model.Enumerations.LogPriority.[Error]
                        Exit Select
                End Select
            End If
            Return mRetVal
        End Function


#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class

End Namespace

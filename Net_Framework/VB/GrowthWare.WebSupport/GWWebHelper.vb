Imports System.Web
Imports GrowthWare.Framework.Common
Imports System.Globalization

''' <summary>
''' GWWebHelper Contains non volatile data needed throughout the system.
''' </summary>
''' <remarks></remarks>
Public Class GWWebHelper
    Private Shared s_ExceptionError As Exception = Nothing
    Private Shared s_Version As String = String.Empty

    ''' <summary>
    ''' Constant value of 3 representing the Link Behavior for the name
    ''' </summary>
    Public Const LinkBehaviorNameValuePairSequenceId As Integer = 3

    ''' <summary>
    ''' Constant value of 1 representing the Link Behavior for navigation
    ''' </summary>
    Public Const LinkBehaviorNavigationTypesSequenceId As Integer = 1

    ''' <summary>
    ''' Constant value of 1 representing the DataKeyField for Roles
    ''' </summary>
    Public Const RoleDataKeyField As String = "ROLE_SEQ_ID"

    ''' <summary>
    ''' Gets the core web administration version.
    ''' </summary>
    ''' <value>The core web administration version.</value>
    Public Shared ReadOnly Property CoreWebAdministrationVersion() As String
        Get
            Dim mVersion As String = String.Empty
            Dim mAssembly As Reflection.Assembly = Nothing
            ' TODO: Should change this to find by either appdomain path or something rather than the hard coding here.
            mAssembly = Reflection.Assembly.Load(ConfigSettings.WebAssemblyName)

            If Not mAssembly Is Nothing Then
                mVersion = mAssembly.GetName.Version.ToString
            End If
            Return mVersion
        End Get
    End Property

    ''' <summary>
    ''' Returns MapPath("~\Public\Skins\")
    ''' </summary>
    ''' <value>String</value>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property SkinPath() As String
        Get
            Return HttpContext.Current.Server.MapPath("~\Content\Skins\")
        End Get
    End Property

    ''' <summary>
    ''' Returns MapPath("~\Content\FormStyles\")
    ''' </summary>
    ''' <value>String</value>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property StylePath() As String
        Get
            Return HttpContext.Current.Server.MapPath("~\Content\FormStyles\")
        End Get
    End Property

    ''' <summary>
    ''' Returns http(s)://FQDN(/AppName)
    ''' </summary>
    ''' <value>String</value>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property RootSite() As String
        Get
            Dim myRoot_Site As String = String.Empty
            Dim myHTTP_Schema As String = String.Empty
            If ConfigSettings.ForceHttps Then
                myHTTP_Schema = "HTTPS"
            Else
                myHTTP_Schema = HttpContext.Current.Request.Url.Scheme
            End If
            If HttpContext.Current.Request.ApplicationPath = "/" Then
                myRoot_Site = myHTTP_Schema & "://" & HttpContext.Current.Request.ServerVariables("HTTP_HOST") & "/"
            Else
                myRoot_Site = myHTTP_Schema & "://" & HttpContext.Current.Request.ServerVariables("HTTP_HOST") & "/" & ConfigSettings.AppName & "/"
            End If
            Return myRoot_Site
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets an exception.
    ''' </summary>
    ''' <value>The exception error.</value>
    Public Shared Property ExceptionError() As Exception
        Get
            Return s_ExceptionError
        End Get
        Set(ByVal Value As Exception)
            s_ExceptionError = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets the frame work version.
    ''' </summary>
    ''' <value>The frame work version.</value>
    Public Shared ReadOnly Property FrameworkVersion() As String
        Get
            Dim myVersion As String = String.Empty
            Dim myAssembly As Reflection.Assembly = Reflection.Assembly.Load("GrowthWare.Framework")
            If Not myAssembly Is Nothing Then
                myVersion = myAssembly.GetName.Version.ToString
            End If
            Return myVersion
        End Get
    End Property

    Public Shared Function GetLogLevel(ByVal logPriority As String) As Integer
        Dim mRetVal As Integer = 0
        Select Case logPriority.ToUpper(CultureInfo.InvariantCulture)
            Case "DEBUG"
                mRetVal = 0
                Exit Select
            Case "INFO"
                mRetVal = 1
                Exit Select
            Case "WARN"
                mRetVal = 2
                Exit Select
            Case "ERROR"
                mRetVal = 3
                Exit Select
            Case "FATAL"
                mRetVal = 4
                Exit Select
            Case Else
                mRetVal = 3
                Exit Select
        End Select
        Return mRetVal
    End Function

    ''' <summary>
    ''' Gets the new GUID.
    ''' </summary>
    ''' <returns>System.String.</returns>
    Public Shared ReadOnly Property GetNewGuid As String
        Get
            Dim retVal As String
            retVal = Guid.NewGuid().ToString()
            Return retVal
        End Get
    End Property

    ''' <summary>
    ''' Gets the query or form value.
    ''' </summary>
    ''' <param name="request">The HttpContext.Current.Request</param>
    ''' <param name="queryString">The query string.</param>
    ''' <returns>String.Empty or value as string.</returns>
    Public Shared Function GetQueryValue(ByVal request As HttpRequest, ByVal queryString As String) As String
        If request Is Nothing Then Throw New ArgumentNullException("request", "request can not be null (Nothing in VB)")
        Dim mRetVal As String = String.Empty
        If Not request(queryString) Is Nothing Then
            mRetVal = request(queryString)
        End If
        Return mRetVal
    End Function

    ''' <summary>
    ''' Gets the random number.
    ''' </summary>
    ''' <param name="startingNumber">The starting number.</param>
    ''' <param name="endingNumber">The ending number.</param>
    ''' <returns>System.String.</returns>
    Shared Function GetRandomNumber(ByVal startingNumber As Integer, ByVal endingNumber As Integer) As String
        Dim retVal As Integer = 0
        'initialize random number generator
        Dim r As New Random(System.DateTime.Now.Millisecond)
        'if passed incorrect arguments, swap them
        'can also throw exception or return 0
        If startingNumber > endingNumber Then
            Dim t As Integer = startingNumber
            startingNumber = endingNumber
            endingNumber = t
        End If
        retVal = r.Next(startingNumber, endingNumber)
        Sleep((System.DateTime.Now.Millisecond * (retVal / 100)))
        Return retVal.ToString(CultureInfo.InvariantCulture)
    End Function

    ''' <summary>
    ''' Determines if a call is for the API
    ''' </summary>
    ''' <returns>bool</returns>
    Shared ReadOnly Property IsWebApiRequest() As Boolean
        Get
            Dim mRetVal As Boolean = False
            If HttpContext.Current.Request.Path.ToUpper(CultureInfo.InvariantCulture).IndexOf("/API/", StringComparison.OrdinalIgnoreCase) <> -1 Then
                mRetVal = True
            End If
            If HttpContext.Current.Request.RawUrl.ToUpper(CultureInfo.InvariantCulture).EndsWith(".UPLOAD", StringComparison.OrdinalIgnoreCase) Then
                mRetVal = True
            End If
            Return mRetVal
        End Get
    End Property

    ''' <summary>
    ''' Gets the display environment.
    ''' </summary>
    ''' <value>The display environment.</value>
    Shared ReadOnly Property DisplayEnvironment As String
        Get
            Return ConfigSettings.EnvironmentDisplayed
        End Get
    End Property

    Shared ReadOnly Property Version As String
        Get
            If String.IsNullOrEmpty(s_Version) Then
                s_Version = System.Reflection.Assembly.GetCallingAssembly().GetName().Version.ToString()
            End If
            Return s_Version
        End Get
    End Property

End Class

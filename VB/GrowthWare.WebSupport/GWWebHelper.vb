Imports System.Web
Imports GrowthWare.Framework.Common

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
            Dim myVersion As String = String.Empty
            Dim myAssembly As Reflection.Assembly = Reflection.Assembly.Load("GrowthWare.WebApplication")
            If Not myAssembly Is Nothing Then
                myVersion = myAssembly.GetName.Version.ToString
            End If
            Return myVersion
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
            Return HttpContext.Current.Server.MapPath("~\Public\Skins\")
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

    ''' <summary>
    ''' Gets the new GUID.
    ''' </summary>
    ''' <returns>System.String.</returns>
    Public Shared Function GetNewGuid() As String
        Dim retVal As String
        retVal = Guid.NewGuid().ToString()
        Return retVal
    End Function

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
        Sleep(CType((System.DateTime.Now.Millisecond * (retVal / 100)), Long))
        Return retVal
    End Function

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

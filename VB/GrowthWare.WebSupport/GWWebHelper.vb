Imports System.Web
Imports GrowthWare.Framework.Common

Public Module GWWebHelper
    Private s_ExceptionError As Exception = Nothing
    Private s_Version As String = String.Empty

    ''' <summary>
    ''' Gets the core web administration verison.
    ''' </summary>
    ''' <value>The core web administration verison.</value>
    ReadOnly Property CoreWebAdministrationVersion() As String
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
    ReadOnly Property SkinPath() As String
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
    ReadOnly Property RootSite() As String
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
    Property ExceptionError() As Exception
        Get
            Return s_ExceptionError
        End Get
        Set(ByVal Value As Exception)
            s_ExceptionError = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets the frame work verison.
    ''' </summary>
    ''' <value>The frame work verison.</value>
    ReadOnly Property FrameworkVersion() As String
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
    ''' Gets the query value.
    ''' </summary>
    ''' <param name="request">The request.</param>
    ''' <param name="queryString">The query string.</param>
    ''' <returns>System.String.</returns>
    Public Function GetQueryValue(ByVal request As HttpRequest, ByVal queryString As String) As String
        If request Is Nothing Then Throw New ArgumentNullException("request", "request can not be null (Nothing in VB)")
        Dim mRetVal As String = String.Empty
        If Not request.QueryString(queryString) Is Nothing Then
            mRetVal = request.QueryString(queryString)
        End If
        Return mRetVal
    End Function

    ''' <summary>
    ''' Gets the display environment.
    ''' </summary>
    ''' <value>The display environment.</value>
    ReadOnly Property DisplayEnvironment As String
        Get
            Return ConfigSettings.EnvironmentDisplayed
        End Get
    End Property

    ReadOnly Property Version As String
        Get
            If String.IsNullOrEmpty(s_Version) Then
                s_Version = System.Reflection.Assembly.GetCallingAssembly().GetName().Version.ToString()
            End If
            Return s_Version
        End Get
    End Property

    End Module

Imports System.Web

Public Module GWWebHelper
    Private s_ExceptionError As Exception = Nothing


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
        If request Is Nothing Then Throw New ArgumentNullException("request", "request can not be null (Nothing in VB)!")
        Dim mRetVal As String = String.Empty
        If Not request.QueryString(queryString) Is Nothing Then
            mRetVal = request.QueryString(queryString)
        End If
        Return mRetVal
    End Function
End Module

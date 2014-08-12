Imports System.Web

Public NotInheritable Class GWWebHelper
    Private Shared m_ExceptionError As Exception = Nothing

    ''' <summary>
    ''' Gets the core web administration verison.
    ''' </summary>
    ''' <value>The core web administration verison.</value>
    Shared ReadOnly Property CoreWebAdministrationVersion() As String
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
    Shared Property ExceptionError() As Exception
        Get
            Return m_ExceptionError
        End Get
        Set(ByVal Value As Exception)
            m_ExceptionError = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets the frame work verison.
    ''' </summary>
    ''' <value>The frame work verison.</value>
    Shared ReadOnly Property FrameworkVersion() As String
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
    Public Shared Function GetQueryValue(ByVal request As HttpRequest, ByVal queryString As String) As String
        Dim mRetVal As String = String.Empty
        If Not request.QueryString(queryString) Is Nothing Then
            mRetVal = request.QueryString(queryString)
        End If
        Return mRetVal
    End Function
End Class

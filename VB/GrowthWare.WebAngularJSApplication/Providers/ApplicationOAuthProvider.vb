Imports System.Threading.Tasks
Imports Microsoft.Owin.Security.OAuth

Public Class ApplicationOAuthProvider
    Inherits OAuthAuthorizationServerProvider
    Private ReadOnly _publicClientId As String

    Public Sub New(publicClientId As String)
      If publicClientId Is Nothing Then
        Throw New ArgumentNullException("publicClientId")
      End If

      _publicClientId = publicClientId
    End Sub

    Public Overrides Function ValidateClientRedirectUri(context As OAuthValidateClientRedirectUriContext) As Task
        If context.ClientId = _publicClientId Then
            Dim expectedRootUri As New Uri(context.Request.Uri, "/")

            If expectedRootUri.AbsoluteUri = context.RedirectUri Then
                context.Validated()
            ElseIf context.ClientId = "web" Then
                Dim expectedUri = New Uri(context.Request.Uri, "/")
                context.Validated(expectedUri.AbsoluteUri)
            End If
        End If

      Return Task.FromResult(Of Object)(Nothing)
    End Function
End Class

Imports Microsoft.AspNet.Identity
Imports Microsoft.Owin.Security
Imports System.Globalization
Imports GrowthWare.WebSupport

Public Class OpenAuthProviderLogon
    Inherits System.Web.UI.Page

    Public Property ReturnUrl() As String
        Get
            Return m_ReturnUrl
        End Get
        Set(value As String)
            m_ReturnUrl = value
        End Set
    End Property

    Private m_ReturnUrl As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Dim provider = Request.Form("provider")
            Dim provider As String = GWWebHelper.GetQueryValue(Request, "provider")
            If String.IsNullOrEmpty(provider) Then
                Return
            End If
            ' Request a redirect to the external login provider
            Dim redirectUrl As String = ResolveUrl([String].Format(CultureInfo.InvariantCulture, "~/Functions/System/Accounts/RegisterExternalLogin&{0}={1}&returnUrl={2}", IdentityHelper.ProviderNameKey, provider, ReturnUrl))
            Dim properties As AuthenticationProperties = New AuthenticationProperties() With {.RedirectUri = redirectUrl}
            'Add xsrf verification when linking accounts
            If (Context.User.Identity.IsAuthenticated) Then
                properties.Dictionary.Item(IdentityHelper.XsrfKey) = Context.User.Identity.GetUserId()
            End If
            Context.GetOwinContext().Authentication.Challenge(properties, provider)
            Response.StatusCode = 401
            Response.[End]()
        Catch ex As Exception

        End Try
    End Sub

End Class
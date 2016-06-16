Imports System
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin
Imports Microsoft.Owin.Security.Cookies
Imports Microsoft.Owin.Security.DataProtection
Imports Microsoft.Owin.Security.Google
Imports Owin
Imports GrowthWare.Framework.Common

Partial Public Class Startup

    ' For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301883
    Public Sub ConfigureAuth(app As IAppBuilder)
        ' Create a CookieAuthenticationOptions for use in app.UseCookieAuthentication
        Dim mCookieAuthenticationOptions As New CookieAuthenticationOptions()
        mCookieAuthenticationOptions.AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie
        mCookieAuthenticationOptions.LoginPath = New PathString("/?Action=Logon")

        'Enable the application to use a cookie to store information for the signed in account
        app.UseCookieAuthentication(mCookieAuthenticationOptions)

        ' Use a cookie to temporarily store information about a user logging in with a third party login provider
        app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie)

        ' Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
        app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5))

        ' Enables the application to remember the second login verification factor such as phone or email.
        ' Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
        ' This is similar to the RememberMe option when you log in.
        app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie)

        ' Add settings based on the web.config file.
        If ConfigSettings.GetAppSettingValue("EnableMicrosoftAccountAuthentication", True).ToUpperInvariant() = "TRUE" Then
            app.UseMicrosoftAccountAuthentication(
                clientId:=ConfigSettings.GetAppSettingValue("MicrosoftAccountClientId", True),
                clientSecret:=ConfigSettings.GetAppSettingValue("MicrosoftAccountClientSecret", True))
        End If

        If ConfigSettings.GetAppSettingValue("EnableTwitterAuthentication", True).ToUpperInvariant() = "TRUE" Then
            app.UseTwitterAuthentication(
               consumerKey:=ConfigSettings.GetAppSettingValue("TwitterConsumerKey", True),
               consumerSecret:=ConfigSettings.GetAppSettingValue("TwitterConsumerSecret", True))
        End If

        If ConfigSettings.GetAppSettingValue("EnableFacebookAuthentication", True).ToUpperInvariant() = "TRUE" Then
            app.UseFacebookAuthentication(
               appId:=ConfigSettings.GetAppSettingValue("FacebookAppId", True),
               appSecret:=ConfigSettings.GetAppSettingValue("FacebookAppSecret", True))
        End If

        If ConfigSettings.GetAppSettingValue("EnableGoogleAuthentication", True).ToUpperInvariant() = "TRUE" Then
            app.UseGoogleAuthentication(New GoogleOAuth2AuthenticationOptions() With {
               .ClientId = ConfigSettings.GetAppSettingValue("GoogleClientId", True),
               .ClientSecret = ConfigSettings.GetAppSettingValue("GoogleClientSecret", True)})
        End If
    End Sub
End Class

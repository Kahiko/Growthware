Imports System.Web.SessionState
Imports System.Web.Http
Imports System.Web.Routing
Imports System.Web.Optimization

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started
        GlobalConfiguration.Configure(AddressOf WebApiConfig.Register)
        'RouteConfig.RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles(BundleTable.Bundles)
    End Sub

    Protected Sub Application_PostAuthorizeRequest()
        If IsWebApiRequest() Then HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required)
    End Sub

    Private Shared Function IsWebApiRequest() As Boolean
        Dim mRetVal As Boolean = False
        If HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.Contains("/gw/") Or HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.Contains("/api/") Then
            mRetVal = True
        End If
        Return mRetVal
    End Function

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub

End Class
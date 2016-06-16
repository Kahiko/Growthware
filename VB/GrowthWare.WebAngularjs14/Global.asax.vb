Imports System.Web.Http
Imports System.Web.Optimization

Public Class WebApiApplication
    Inherits System.Web.HttpApplication

    Protected Sub Application_Start()
        GlobalConfiguration.Configure(AddressOf WebApiConfig.Register)
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
End Class

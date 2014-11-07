Imports System.Web.Http
Imports System.Web.Optimization
Imports System.Web.Routing

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
        Return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith("api")
    End Function

End Class

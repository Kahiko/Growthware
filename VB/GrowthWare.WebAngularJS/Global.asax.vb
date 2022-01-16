Imports System.Web.Http
Imports System.Web.Optimization
Imports GrowthWare.WebSupport

Public Class WebApiApplication
    Inherits System.Web.HttpApplication

    Protected Sub Application_Start()
        GlobalConfiguration.Configure(AddressOf WebApiConfig.Register)
        BundleConfig.RegisterBundles(BundleTable.Bundles)
    End Sub

    Protected Sub Application_PostAuthorizeRequest()
        If GWWebHelper.IsWebApiRequest Then HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required)
    End Sub
End Class

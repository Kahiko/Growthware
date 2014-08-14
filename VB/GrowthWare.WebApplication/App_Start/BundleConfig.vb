Imports System.Web
Imports System.Web.Optimization
Public Module BundleConfig
    Public Sub RegisterBundles(bundles As BundleCollection)
        bundles.Add(
            New ScriptBundle("~/bundles/jquery").Include(
            "~/Scripts/jquery-{version}.js"
            )
        )

        bundles.Add(
            New ScriptBundle("~/bundles/angular").Include(
            "~/Scripts/angular.js"
            )
        )

        bundles.Add(
            New ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"
            )
        )

        bundles.Add(
            New ScriptBundle("~/bundles/GrowthWare").Include(
            "~/Scripts/GrowthWare/GW.Common.js",
            "~/Scripts/GrowthWare/GW.FileManager.js",
            "~/Scripts/GrowthWare/GW.Model.js",
            "~/Scripts/GrowthWare/GW.NavigationController.js",
            "~/Scripts/GrowthWare/GW.NavigationHandler.js",
            "~/Scripts/GrowthWare/GW.Search.js",
            "~/Scripts/GrowthWare/GW.Upload.js"
        )
)
    End Sub
End Module

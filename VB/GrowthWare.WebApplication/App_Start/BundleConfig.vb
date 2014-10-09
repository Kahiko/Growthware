Imports System.Web
Imports System.Web.Optimization
Public Module BundleConfig
    Public Sub RegisterBundles(bundles As BundleCollection)
        bundles.Add(
            New ScriptBundle("~/bundles/jquery").Include(
            "~/Public/Scripts/jquery-{version}.js"
            )
        )

        bundles.Add(
            New ScriptBundle("~/bundles/jqueryUI").Include(
            "~/Public/Scripts/jquery-ui.js"
            )
        )

        bundles.Add(
            New ScriptBundle("~/bundles/angular").Include(
            "~/Public/Scripts/angular-{version}.js"
            )
        )

        bundles.Add(
            New ScriptBundle("~/bundles/bootstrap").Include(
                "~/Public/Scripts/bootstrap.js",
                "~/Public/Scripts/respond.js"
            )
        )

        bundles.Add(
            New ScriptBundle("~/bundles/GrowthWare").Include(
            "~/Public/Scripts/GrowthWare/GW.Common.js",
            "~/Public/Scripts/GrowthWare/GW.FileManager.js",
            "~/Public/Scripts/GrowthWare/GW.Model.js",
            "~/Public/Scripts/GrowthWare/GW.NavigationController.js",
            "~/Public/Scripts/GrowthWare/GW.NavigationHandler.js",
            "~/Public/Scripts/GrowthWare/GW.Search.js",
            "~/Public/Scripts/GrowthWare/GW.Upload.js"
            )
        )

        bundles.Add(
            New StyleBundle("~/Content/BootstrapCSS").Include(
                "~/Public/CSS/BootStrap/bootstrap-theme.css",
                "~/Public/CSS/BootStrap/bootstrap-theme.css.map",
                "~/Public/CSS/BootStrap/bootstrap.css",
                "~/Public/CSS/BootStrap/bootstrap.css.map"
            )
        )

        bundles.Add(
            New StyleBundle("~/Content/GrowthWare").Include(
                "~/Public/CSS/GrowthWare.css"
            )
        )

        bundles.Add(
            New StyleBundle("~/Content/jQueryUIRedmond").Include(
                "~/Public/CSS/jQueryUIThemes/Redmond/jquery-ui.css",
                "~/Public/CSS/jQueryUIThemes/Redmond/jquery-ui.structure.css",
                "~/Public/CSS/jQueryUIThemes/Redmond/jquery-ui.theme.css"
            )
        )
    End Sub
End Module

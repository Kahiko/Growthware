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
            "~/Public/GrowthWare/Scripts/GW.Common.js",
            "~/Public/GrowthWare/Scripts/GW.FileManager.js",
            "~/Public/GrowthWare/Scripts/GW.Model.js",
            "~/Public/GrowthWare/Scripts/GW.NavigationController.js",
            "~/Public/GrowthWare/Scripts/GW.NavigationHandler.js",
            "~/Public/GrowthWare/Scripts/GW.Search.js",
            "~/Public/GrowthWare/Scripts/GW.Upload.js",
            "~/Public/Scripts/jSon2.js"
            )
        )

        bundles.Add(
            New StyleBundle("~/Public/CSS/Bootstrap").Include(
                "~/Public/CSS/BootStrap/bootstrap-theme.css",
                "~/Public/CSS/BootStrap/bootstrap.css"
            )
        )

        bundles.Add(
            New StyleBundle("~/Content/GrowthWare").Include(
                "~/Public/Growthware/Styles/GrowthWare.css"
            )
        )

        bundles.Add(
            New StyleBundle("~/Public/CSS/jQueryUIThemes/Redmond/jQueryUIRedmond").Include(
                "~/Public/CSS/jQueryUIThemes/Redmond/jquery-ui.css",
                "~/Public/CSS/jQueryUIThemes/Redmond/jquery-ui.structure.css",
                "~/Public/CSS/jQueryUIThemes/Redmond/jquery-ui.theme.css"
            )
        )
    End Sub
End Module

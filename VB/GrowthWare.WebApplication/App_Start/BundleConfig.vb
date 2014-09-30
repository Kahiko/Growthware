Imports System.Web
Imports System.Web.Optimization
Public Module BundleConfig
    Public Sub RegisterBundles(bundles As BundleCollection)
        bundles.Add(
            New ScriptBundle("~/bundles/jquery").Include(
            "~/Public/Scripts/jquery.js"
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

        'bundles.Add(
        '    New ScriptBundle("~/bundles/json").Include(
        '    "~/Public/Scripts/jSon2.js"
        '    )
        ')

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
            New StyleBundle("~/Content/SiteCSS").Include(
                "~/Public/CSS/bootstrap.css",
                "~/Public/CSS/bootstrap-theme.css",
                "~/Public/CSS/SiteStyle.css"
            )
        )
        bundles.Add(
            New StyleBundle("~/Content/jQueryUIRedmond").Include(
                "~/Public/jQueryUIThemes/Redmond/jquery-ui.css",
                "~/Public/jQueryUIThemes/Redmond/jquery-ui.structure.css",
                "~/Public/jQueryUIThemes/Redmond/jquery-ui.theme.css"
            )
        )
    End Sub
End Module

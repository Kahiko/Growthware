Imports System.Web
Imports System.Web.Optimization
Imports GrowthWare.Framework.Common

Public Module BundleConfig
    Public Sub RegisterBundles(bundles As BundleCollection)
        bundles.Add(
            New ScriptBundle("~/bundles/jquery").Include(
            "~/" + ConfigSettings.AppName + "Public/Scripts/jquery-{version}.js"
            )
        )

        bundles.Add(
            New ScriptBundle("~/bundles/jqueryUI").Include(
            "~/" + ConfigSettings.AppName + "Public/Scripts/jquery-ui.js"
            )
        )

        bundles.Add(
            New ScriptBundle("~/bundles/angular").Include(
            "~/" + ConfigSettings.AppName + "Public/Scripts/angular-{version}.js"
            )
        )

        bundles.Add(
            New ScriptBundle("~/bundles/bootstrap").Include(
                "~/" + ConfigSettings.AppName + "Public/Scripts/bootstrap.js",
                "~/" + ConfigSettings.AppName + "Public/Scripts/respond.js"
            )
        )

        bundles.Add(
            New ScriptBundle("~/bundles/GrowthWare").Include(
            "~/" + ConfigSettings.AppName + "Public/GrowthWare/Scripts/GW.Common.js",
            "~/" + ConfigSettings.AppName + "Public/GrowthWare/Scripts/GW.FileManager.js",
            "~/" + ConfigSettings.AppName + "Public/GrowthWare/Scripts/GW.Model.js",
            "~/" + ConfigSettings.AppName + "Public/GrowthWare/Scripts/GW.NavigationController.js",
            "~/" + ConfigSettings.AppName + "Public/GrowthWare/Scripts/GW.NavigationHandler.js",
            "~/" + ConfigSettings.AppName + "Public/GrowthWare/Scripts/GW.Search.js",
            "~/" + ConfigSettings.AppName + "Public/GrowthWare/Scripts/GW.Upload.js",
            "~/" + ConfigSettings.AppName + "Public/Scripts/jSon2.js"
            )
        )

        bundles.Add(
            New StyleBundle("~/Public/CSS/Bootstrap").Include(
                "~/" + ConfigSettings.AppName + "Public/CSS/BootStrap/bootstrap-theme.css",
                "~/" + ConfigSettings.AppName + "Public/CSS/BootStrap/bootstrap.css"
            )
        )

        bundles.Add(
            New StyleBundle("~/Content/GrowthWare").Include(
                "~/" + ConfigSettings.AppName + "Public/Growthware/Styles/GrowthWare.css"
            )
        )

        bundles.Add(
            New StyleBundle("~/Public/CSS/jQueryUIThemes/Redmond/jQueryUIRedmond").Include(
                "~/" + ConfigSettings.AppName + "Public/CSS/jQueryUIThemes/Redmond/jquery-ui.css",
                "~/" + ConfigSettings.AppName + "Public/CSS/jQueryUIThemes/Redmond/jquery-ui.structure.css",
                "~/" + ConfigSettings.AppName + "Public/CSS/jQueryUIThemes/Redmond/jquery-ui.theme.css"
            )
        )
    End Sub
End Module

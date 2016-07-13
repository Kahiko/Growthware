Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Optimization
Imports GrowthWare.Framework.Common

Public Class BundleConfig
    ' For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkID=303951
    Public Shared Sub RegisterBundles(ByVal bundles As BundleCollection)

        bundles.Add(
            New ScriptBundle("~/bundles/jquery").Include(
            "~/" + ConfigSettings.AppName + "/Content/Scripts/jquery-{version}.js"
            )
        )

        'bundles.Add(
        '    New ScriptBundle("~/bundles/GrowthWare").Include(
        '    "~/" + ConfigSettings.AppName + "/Scripts/GrowthWare/GW.Common.js",
        '    "~/" + ConfigSettings.AppName + "/Scripts/GrowthWare/GW.FileManager.js",
        '    "~/" + ConfigSettings.AppName + "/Scripts/GrowthWare/GW.Model.js",
        '    "~/" + ConfigSettings.AppName + "/Scripts/GrowthWare/GW.NavigationController.js",
        '    "~/" + ConfigSettings.AppName + "/Scripts/GrowthWare/GW.NavigationHandler.js",
        '    "~/" + ConfigSettings.AppName + "/Scripts/GrowthWare/GW.Search.js",
        '    "~/" + ConfigSettings.AppName + "/Scripts/GrowthWare/GW.Upload.js"
        '    )
        ')

        bundles.Add(
            New ScriptBundle("~/bundles/GrowthWare").Include(
            "~/" + ConfigSettings.AppName + "/Content/GrowthWare/Scripts/GW.Common.js",
            "~/" + ConfigSettings.AppName + "/Content/GrowthWare/Scripts/GW.Model.js",
            "~/" + ConfigSettings.AppName + "/Content/GrowthWare/Scripts/GW.Search.js",
            "~/" + ConfigSettings.AppName + "/Content/GrowthWare/Scripts/GW.NavigationController.js",
            "~/" + ConfigSettings.AppName + "/Content/Scripts/date.format.js"
            )
        )

        bundles.Add(
            New ScriptBundle("~/bundles/GrowthwareApp").Include(
            "~/" + ConfigSettings.AppName + "/app/growthware/GrowthwareApp.js",
            "~/" + ConfigSettings.AppName + "/app/growthware/factories/ClientChoicesSvc.js",
            "~/" + ConfigSettings.AppName + "/app/growthware/services/AccountSvc.js",
            "~/" + ConfigSettings.AppName + "/app/growthware/services/SearchSvc.js",
            "~/" + ConfigSettings.AppName + "/app/growthware/services/SecurityEntitySvc.js",
            "~/" + ConfigSettings.AppName + "/app/growthware/controllers/AccountCtrl.js",
            "~/" + ConfigSettings.AppName + "/app/growthware/controllers/ClientChoicesCtrl.js",
            "~/" + ConfigSettings.AppName + "/app/growthware/controllers/HierarchicalMenuCtrl.js",
            "~/" + ConfigSettings.AppName + "/app/growthware/controllers/HorizontalMenuCtrl.js",
            "~/" + ConfigSettings.AppName + "/app/growthware/controllers/VerticalMenuCtrl.js",
            "~/" + ConfigSettings.AppName + "/app/growthware/views/Accounts/LogonCtrl.js",
            "~/" + ConfigSettings.AppName + "/app/growthware/views/Accounts/LogoffCtrl.js",
            "~/" + ConfigSettings.AppName + "/app/growthware/views/Search/SearchCtrl.js",
            "~/" + ConfigSettings.AppName + "/app/growthware/views/Administration/Accounts/AddEditAccountCtrl.js",
            "~/" + ConfigSettings.AppName + "/app/growthware/views/SecurityEntities/SelectSecurityEntityCtrl.js",
            "~/" + ConfigSettings.AppName + "/app/growthware/directives/LoadingDirective.js",
            "~/" + ConfigSettings.AppName + "/app/growthware/directives/PickList/PicklistDirective.js"
            )
        )

        bundles.Add(
            New ScriptBundle("~/bundles/jqueryUI").Include(
            "~/" + ConfigSettings.AppName + "/Content/Scripts/jquery-ui-{version}.js"
            )
        )

        bundles.Add(
            New ScriptBundle("~/bundles/bootstrap").Include(
                "~/" + ConfigSettings.AppName + "/Content/Bootstrap-3.1.1/js/bootstrap.js",
                "~/" + ConfigSettings.AppName + "/Content/BootstrapDialog/bootstrap-dialog.js",
                "~/" + ConfigSettings.AppName + "/Scripts/respond.js"
            )
        )

        bundles.Add(
            New StyleBundle("~/Content/jQueryUIThemes/Redmond/UI").Include(
                "~/" + ConfigSettings.AppName + "/Content/jQueryUIThemes/Redmond/jquery-ui.css",
                "~/" + ConfigSettings.AppName + "/Content/jQueryUIThemes/Redmond/jquery-ui.structure.css",
                "~/" + ConfigSettings.AppName + "/Content/jQueryUIThemes/Redmond/jquery-ui.theme.css"
            )
        )

        bundles.Add(
            New StyleBundle("~/Content/Bootstrap").Include(
                "~/" + ConfigSettings.AppName + "/Content/Bootstrap-3.1.1/css/bootstrap.css",
                "~/" + ConfigSettings.AppName + "/Content/Bootstrap-3.1.1/css/bootstrap-theme.css",
                "~/" + ConfigSettings.AppName + "/Content/BootstrapDialog/bootstrap-dialog.css"
            )
        )

        bundles.Add(
            New StyleBundle("~/Content/Growthware/Styles/UI").Include(
                "~/" + ConfigSettings.AppName + "/Content/Growthware/Styles/GrowthWare.css"
            )
        )

        ' Use the Development version of Modernizr to develop with and learn from. Then, when you’re
        ' ready for production, use the build tool at http://modernizr.com to pick only the tests you need
        bundles.Add(New ScriptBundle("~/bundles/modernizr").Include(
                        "~/" + ConfigSettings.AppName + "/Content/Scripts/modernizr-*"))

        ScriptManager.ScriptResourceMapping.AddDefinition("respond", New ScriptResourceDefinition() With {
                .Path = "~/" + ConfigSettings.AppName + "/Content/Scripts/respond.min.js",
                .DebugPath = "~/" + ConfigSettings.AppName + "/Content/Scripts/respond.js"})

    End Sub
End Class

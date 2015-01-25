Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Optimization
Imports GrowthWare.Framework.Common

Public Class BundleConfig
    ' For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkID=303951
    Public Shared Sub RegisterBundles(ByVal bundles As BundleCollection)


        bundles.Add(
            New ScriptBundle("~/bundles/GrowthWare").Include(
            "~/" + ConfigSettings.AppName + "/Public/GrowthWare/Scripts/GW.Common.js",
            "~/" + ConfigSettings.AppName + "/Public/GrowthWare/Scripts/GW.FileManager.js",
            "~/" + ConfigSettings.AppName + "/Public/GrowthWare/Scripts/GW.Model.js",
            "~/" + ConfigSettings.AppName + "/Public/GrowthWare/Scripts/GW.NavigationController.js",
            "~/" + ConfigSettings.AppName + "/Public/GrowthWare/Scripts/GW.NavigationHandler.js",
            "~/" + ConfigSettings.AppName + "/Public/GrowthWare/Scripts/GW.Search.js",
            "~/" + ConfigSettings.AppName + "/Public/GrowthWare/Scripts/GW.Upload.js",
            "~/" + ConfigSettings.AppName + "/Public/Scripts/jSon2.js"
            )
        )

        bundles.Add(
            New ScriptBundle("~/bundles/jquery").Include(
            "~/" + ConfigSettings.AppName + "/Scripts/jquery-{version}.js",
            "~/" + ConfigSettings.AppName + "/Scripts/jquery.tmpl.js"
            )
        )

        bundles.Add(
            New ScriptBundle("~/bundles/jqueryUI").Include(
            "~/" + ConfigSettings.AppName + "/Scripts/jquery-ui.js"
            )
        )

        bundles.Add(
            New ScriptBundle("~/bundles/bootstrap").Include(
                "~/" + ConfigSettings.AppName + "/Scripts/bootstrap.js",
                "~/" + ConfigSettings.AppName + "/Scripts/bootstrap-dialog.js",
                "~/" + ConfigSettings.AppName + "/Scripts/respond.js"
            )
        )

        bundles.Add(
            New StyleBundle("~/Public/CSS/jQueryUIThemes/Redmond/jQueryUIRedmond").Include(
                "~/" + ConfigSettings.AppName + "/Public/CSS/jQueryUIThemes/Redmond/jquery-ui.css",
                "~/" + ConfigSettings.AppName + "/Public/CSS/jQueryUIThemes/Redmond/jquery-ui.structure.css",
                "~/" + ConfigSettings.AppName + "/Public/CSS/jQueryUIThemes/Redmond/jquery-ui.theme.css"
            )
        )

        bundles.Add(
            New StyleBundle("~/Public/CSS/Bootstrap").Include(
                "~/" + ConfigSettings.AppName + "/Content/bootstrap-theme.css",
                "~/" + ConfigSettings.AppName + "/Content/bootstrap.css"
            )
        )

        bundles.Add(
            New StyleBundle("~/Content/GrowthWare").Include(
                "~/" + ConfigSettings.AppName + "/Public/Growthware/Styles/GrowthWare.css"
            )
        )

        'bundles.Add(New ScriptBundle("~/bundles/WebFormsJs").Include(
        '                "~/" + ConfigSettings.AppName + "/Scripts/WebForms/WebForms.js",
        '                "~/" + ConfigSettings.AppName + "/Scripts/WebForms/WebUIValidation.js",
        '                "~/" + ConfigSettings.AppName + "/Scripts/WebForms/MenuStandards.js",
        '                "~/" + ConfigSettings.AppName + "/Scripts/WebForms/Focus.js",
        '                "~/" + ConfigSettings.AppName + "/Scripts/WebForms/GridView.js",
        '                "~/" + ConfigSettings.AppName + "/Scripts/WebForms/DetailsView.js",
        '                "~/" + ConfigSettings.AppName + "/Scripts/WebForms/TreeView.js",
        '                "~/" + ConfigSettings.AppName + "/Scripts/WebForms/WebParts.js"))

        '' Order is very important for these files to work, they have explicit dependencies
        'bundles.Add(New ScriptBundle("~/bundles/MsAjaxJs").Include(
        '        "~/" + ConfigSettings.AppName + "/Scripts/WebForms/MsAjax/MicrosoftAjax.js",
        '        "~/" + ConfigSettings.AppName + "/Scripts/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js",
        '        "~/" + ConfigSettings.AppName + "/Scripts/WebForms/MsAjax/MicrosoftAjaxTimer.js",
        '        "~/" + ConfigSettings.AppName + "/Scripts/WebForms/MsAjax/MicrosoftAjaxWebForms.js"))

        ' Use the Development version of Modernizr to develop with and learn from. Then, when you’re
        ' ready for production, use the build tool at http://modernizr.com to pick only the tests you need
        bundles.Add(New ScriptBundle("~/bundles/modernizr").Include(
                        "~/" + ConfigSettings.AppName + "/Scripts/modernizr-*"))

        ScriptManager.ScriptResourceMapping.AddDefinition("respond", New ScriptResourceDefinition() With {
                .Path = "~/" + ConfigSettings.AppName + "/Scripts/respond.min.js",
                .DebugPath = "~/" + ConfigSettings.AppName + "/Scripts/respond.js"})

    End Sub
End Class

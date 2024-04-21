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
            "~/" + ConfigSettings.AppName + "/Content/GrowthWare/Scripts/GW.Enum.js",
            "~/" + ConfigSettings.AppName + "/Content/GrowthWare/Scripts/GW.Model.js",
            "~/" + ConfigSettings.AppName + "/Content/GrowthWare/Scripts/GW.Search.js",
            "~/" + ConfigSettings.AppName + "/Content/GrowthWare/Scripts/GW.NavigationController.js",
            "~/" + ConfigSettings.AppName + "/Content/GrowthWare/Scripts/GW.Upload.js",
            "~/" + ConfigSettings.AppName + "/Content/Scripts/date.format.js"
            )
        )

        bundles.Add(
            New ScriptBundle("~/bundles/GrowthwareApp").Include(
                "~/" + ConfigSettings.AppName + "/app/growthware/GrowthwareApp.js",
 _
                "~/" + ConfigSettings.AppName + "/app/growthware/controllers/AccountCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/controllers/ClientChoicesCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/controllers/ConfigInfoCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/controllers/HierarchicalMenuCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/controllers/HorizontalMenuCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/controllers/VerticalMenuCtrl.js",
 _
                "~/" + ConfigSettings.AppName + "/app/growthware/directives/ClientMessage/ClientMessageDirective.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/directives/DerivedRoles/DerivedRolesDirective.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/directives/PickList/PicklistDirective.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/directives/LoadingDirective.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/directives/NumberOnly.js",
 _
                "~/" + ConfigSettings.AppName + "/app/growthware/services/AccountSvc.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/services/ConfigurationSvc.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/services/FileSvc.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/services/FunctionSvc.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/services/GroupSvc.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/services/MessagesSvc.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/services/RoleSvc.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/services/SearchSvc.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/services/SecurityEntitySvc.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/services/StatesSvc.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/services/ModalSvc.js",
 _
                "~/" + ConfigSettings.AppName + "/app/growthware/views/LineCountCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/views/TestNaturalSortCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/views/Accounts/ChangePasswordCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/views/Accounts/LogoffCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/views/Accounts/LogonCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/views/Accounts/SelectPreferencesCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/views/Accounts/UpdateSessionCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/views/Administration/Accounts/AddEditAccountCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/views/Administration/AnonymousAccount/UpdateAnonymousCacheCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/views/Administration/Configuration/AddEditDBInformationCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/views/Administration/Encrypt/EncryptDecryptCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/views/Administration/Encrypt/GUIDHelperCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/views/Administration/Encrypt/RandomNumbersCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/views/Administration/Functions/AddEditFunctionCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/views/Administration/Functions/CopyFunctionSecurityCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/views/Administration/Groups/AddEditGroupCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/views/Administration/Logs/SetLogLevelCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/views/Administration/Messages/AddEditMessageCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/views/Administration/NVP/AddEditNVPDetailCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/views/Administration/Roles/AddEditRoleCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/views/Administration/SecurityEntities/AddEditSecurityEntityCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/views/Administration/States/AddEditStateCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/views/Administration/WorkFlow/AddEditWorkFlowCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/views/Calendar/CommunityCalendarCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/views/FileManagement/AddDirectoryCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/views/FileManagement/FileManagerCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/views/Home/GenericHomeCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/views/SecurityEntities/SelectSecurityEntityCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/views/Search/SearchCtrl.js",
                "~/" + ConfigSettings.AppName + "/app/growthware/views/Templates/ModalPopupCtrl.js"
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
            New StyleBundle("~/Content/jQueryUIThemes").Include(
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

using GrowthWare.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.UI;

namespace GrowthWare.WebApplication
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkID=303951
        public static void RegisterBundles(BundleCollection bundles)
        {
        bundles.Add(
            new ScriptBundle("~/bundles/jquery").Include(
            "~/" + ConfigSettings.AppName + "Public/Scripts/jquery-{version}.js"
            )
        );

        bundles.Add(
            new ScriptBundle("~/bundles/jqueryUI").Include(
            "~/" + ConfigSettings.AppName + "Public/Scripts/jquery-ui.js"
            )
        );

        bundles.Add(
            new ScriptBundle("~/bundles/angular").Include(
            "~/" + ConfigSettings.AppName + "Public/Scripts/angular-{version}.js"
            )
        );

        bundles.Add(
            new ScriptBundle("~/bundles/bootstrap").Include(
                "~/" + ConfigSettings.AppName + "Public/Scripts/bootstrap.js",
                "~/" + ConfigSettings.AppName + "Public/Scripts/respond.js"
            )
        );

        bundles.Add(
            new ScriptBundle("~/bundles/GrowthWare").Include(
            "~/" + ConfigSettings.AppName + "Public/GrowthWare/Scripts/GW.Common.js",
            "~/" + ConfigSettings.AppName + "Public/GrowthWare/Scripts/GW.FileManager.js",
            "~/" + ConfigSettings.AppName + "Public/GrowthWare/Scripts/GW.Model.js",
            "~/" + ConfigSettings.AppName + "Public/GrowthWare/Scripts/GW.NavigationController.js",
            "~/" + ConfigSettings.AppName + "Public/GrowthWare/Scripts/GW.NavigationHandler.js",
            "~/" + ConfigSettings.AppName + "Public/GrowthWare/Scripts/GW.Search.js",
            "~/" + ConfigSettings.AppName + "Public/GrowthWare/Scripts/GW.Upload.js",
            "~/" + ConfigSettings.AppName + "Public/Scripts/jSon2.js"
            )
        );

        bundles.Add(
            new StyleBundle("~/Public/CSS/Bootstrap").Include(
                "~/" + ConfigSettings.AppName + "Public/CSS/BootStrap/bootstrap-theme.css",
                "~/" + ConfigSettings.AppName + "Public/CSS/BootStrap/bootstrap.css"
            )
        );

        bundles.Add(
            new StyleBundle("~/Content/GrowthWare").Include(
                "~/" + ConfigSettings.AppName + "Public/Growthware/Styles/GrowthWare.css"
            )
        );

        bundles.Add(
            new StyleBundle("~/Public/CSS/jQueryUIThemes/Redmond/jQueryUIRedmond").Include(
                "~/" + ConfigSettings.AppName + "Public/CSS/jQueryUIThemes/Redmond/jquery-ui.css",
                "~/" + ConfigSettings.AppName + "Public/CSS/jQueryUIThemes/Redmond/jquery-ui.structure.css",
                "~/" + ConfigSettings.AppName + "Public/CSS/jQueryUIThemes/Redmond/jquery-ui.theme.css"
            )
        );
        }
    }
}
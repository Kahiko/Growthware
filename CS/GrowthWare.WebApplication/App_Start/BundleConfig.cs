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
            "~/" + ConfigSettings.AppName + "/Scripts/jquery-{version}.js",
            "~/" + ConfigSettings.AppName + "/Scripts/jquery.tmpl.js"
            )
        );

        // Use the development version of Modernizr to develop with and learn from. Then, when you're
        // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
        bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                    "~/Scripts/modernizr-*"
            )
        );

        bundles.Add(
            new ScriptBundle("~/bundles/jqueryUI").Include(
            "~/" + ConfigSettings.AppName + "/Scripts/jquery-ui.js"
            )
        );

        bundles.Add(
            new ScriptBundle("~/bundles/bootstrap").Include(
                "~/" + ConfigSettings.AppName + "/Scripts/bootstrap.js",
                "~/" + ConfigSettings.AppName + "/Scripts/bootstrap-dialog.js",
                "~/" + ConfigSettings.AppName + "/Scripts/respond.js"
            )
        );

        bundles.Add(
            new ScriptBundle("~/bundles/GrowthWare").Include(
            "~/" + ConfigSettings.AppName + "/Public/GrowthWare/Scripts/GW.Common.js",
            "~/" + ConfigSettings.AppName + "/Public/GrowthWare/Scripts/GW.FileManager.js",
            "~/" + ConfigSettings.AppName + "/Public/GrowthWare/Scripts/GW.Model.js",
            "~/" + ConfigSettings.AppName + "/Public/GrowthWare/Scripts/GW.NavigationController.js",
            "~/" + ConfigSettings.AppName + "/Public/GrowthWare/Scripts/GW.NavigationHandler.js",
            "~/" + ConfigSettings.AppName + "/Public/GrowthWare/Scripts/GW.Search.js",
            "~/" + ConfigSettings.AppName + "/Public/GrowthWare/Scripts/GW.Upload.js",
            "~/" + ConfigSettings.AppName + "/Public/Scripts/jSon2.js"
            )
        );

        bundles.Add(
            new StyleBundle("~/Public/CSS/Bootstrap").Include(
                "~/" + ConfigSettings.AppName + "/Content/BootStrap/bootstrap-theme.css",
                "~/" + ConfigSettings.AppName + "/Content/BootStrap/bootstrap.css"
            )
        );

        bundles.Add(
            new StyleBundle("~/Content/GrowthWare").Include(
                "~/" + ConfigSettings.AppName + "/Public/Growthware/Styles/GrowthWare.css"
            )
        );

        bundles.Add(
            new StyleBundle("~/Public/CSS/jQueryUIThemes/Redmond/jQueryUIRedmond").Include(
                "~/" + ConfigSettings.AppName + "/Content/jQueryUIThemes/Redmond/jquery-ui.css",
                "~/" + ConfigSettings.AppName + "/Content/jQueryUIThemes/Redmond/jquery-ui.structure.css",
                "~/" + ConfigSettings.AppName + "/Content/jQueryUIThemes/Redmond/jquery-ui.theme.css"
            )
        );
        }
    }
}
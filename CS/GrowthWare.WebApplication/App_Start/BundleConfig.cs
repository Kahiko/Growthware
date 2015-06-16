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

            bundles.Add(
                new ScriptBundle("~/bundles/jqueryUI").Include(
                "~/" + ConfigSettings.AppName + "/Scripts/jquery-ui-{version}.js"
                )
            );

            bundles.Add(
                new ScriptBundle("~/bundles/GrowthWare").Include(
                "~/" + ConfigSettings.AppName + "/Scripts/GrowthWare/GW.Common.js",
                "~/" + ConfigSettings.AppName + "/Scripts/GrowthWare/GW.FileManager.js",
                "~/" + ConfigSettings.AppName + "/Scripts/GrowthWare/GW.Model.js",
                "~/" + ConfigSettings.AppName + "/Scripts/GrowthWare/GW.NavigationController.js",
                "~/" + ConfigSettings.AppName + "/Scripts/GrowthWare/GW.NavigationHandler.js",
                "~/" + ConfigSettings.AppName + "/Scripts/GrowthWare/GW.Search.js",
                "~/" + ConfigSettings.AppName + "/Scripts/GrowthWare/GW.Upload.js"
                )
            );

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"
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
                new StyleBundle("~/Content/Bootstrap").Include(
                    "~/" + ConfigSettings.AppName + "/Content/bootstrap-theme.css",
                    "~/" + ConfigSettings.AppName + "/Content/bootstrap.css"
                )
            );

            bundles.Add(
                new StyleBundle("~/Content/Growthware/Styles/UI").Include(
                    "~/" + ConfigSettings.AppName + "/Content/GrowthWare/Styles/GrowthWare.css"
                )
            );

            bundles.Add(
                new StyleBundle("~/Public/CSS/jQueryUIThemes/Redmond/UI").Include(
                    "~/" + ConfigSettings.AppName + "/Public/CSS/jQueryUIThemes/Redmond/jquery-ui.css",
                    "~/" + ConfigSettings.AppName + "/Public/CSS/jQueryUIThemes/Redmond/jquery-ui.structure.css",
                    "~/" + ConfigSettings.AppName + "/Public/CSS/jQueryUIThemes/Redmond/jquery-ui.theme.css"
                )
            );

            ScriptResourceDefinition respond = new ScriptResourceDefinition();
            respond.Path = "~/" + ConfigSettings.AppName + "/Scripts/respond.min.js";
            respond.DebugPath = "~/" + ConfigSettings.AppName + "/Scripts/respond.js";
            ScriptManager.ScriptResourceMapping.AddDefinition("respond", respond); 

        }
    }
}
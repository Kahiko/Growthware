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
            "~/Public/Scripts/jquery-{version}.js"
            )
        );

        bundles.Add(
            new ScriptBundle("~/bundles/jqueryUI").Include(
            "~/Public/Scripts/jquery-ui.js"
            )
        );

        bundles.Add(
            new ScriptBundle("~/bundles/angular").Include(
            "~/Public/Scripts/angular-{version}.js"
            )
        );

        bundles.Add(
            new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Public/Scripts/bootstrap.js",
                "~/Public/Scripts/respond.js"
            )
        );

        bundles.Add(
            new ScriptBundle("~/bundles/GrowthWare").Include(
            "~/Public/GrowthWare/Scripts/GW.Common.js",
            "~/Public/GrowthWare/Scripts/GW.FileManager.js",
            "~/Public/GrowthWare/Scripts/GW.Model.js",
            "~/Public/GrowthWare/Scripts/GW.NavigationController.js",
            "~/Public/GrowthWare/Scripts/GW.NavigationHandler.js",
            "~/Public/GrowthWare/Scripts/GW.Search.js",
            "~/Public/GrowthWare/Scripts/GW.Upload.js",
            "~/Public/Scripts/jSon2.js"
            )
        );

        bundles.Add(
            new StyleBundle("~/Public/CSS/Bootstrap").Include(
                "~/Public/CSS/BootStrap/bootstrap-theme.css",
                "~/Public/CSS/BootStrap/bootstrap.css"
            )
        );

        bundles.Add(
            new StyleBundle("~/Content/GrowthWare").Include(
                "~/Public/Growthware/Styles/GrowthWare.css"
            )
        );

        bundles.Add(
            new StyleBundle("~/Public/CSS/jQueryUIThemes/Redmond/jQueryUIRedmond").Include(
                "~/Public/CSS/jQueryUIThemes/Redmond/jquery-ui.css",
                "~/Public/CSS/jQueryUIThemes/Redmond/jquery-ui.structure.css",
                "~/Public/CSS/jQueryUIThemes/Redmond/jquery-ui.theme.css"
            )
        );
        }
    }
}
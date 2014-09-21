Imports System.Web
Imports System.Web.Optimization

Public Module BundleConfig
    ' For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
    Public Sub RegisterBundles(bundles As BundleCollection)
        bundles.Add(New ScriptBundle("~/bundles/jquery").Include(
            "~/Scripts/jquery-{version}.js"))

        bundles.Add(New ScriptBundle("~/bundles/jqueryval").Include(
            "~/Scripts/jquery.unobtrusive*",
            "~/Scripts/jquery.validate*"))

        bundles.Add(New ScriptBundle("~/bundles/knockout").Include(
            "~/Scripts/knockout-{version}.js",
            "~/Scripts/knockout.validation.js"))

        bundles.Add(New ScriptBundle("~/bundles/app").Include(
            "~/Scripts/sammy-{version}.js",
            "~/Scripts/app/common.js",
            "~/Scripts/app/app.datamodel.js",
            "~/Scripts/app/app.viewmodel.js",
            "~/Scripts/app/home.viewmodel.js",
            "~/Scripts/app/_run.js"))

        ' Use the development version of Modernizr to develop with and learn from. Then, when you're
        ' ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
        bundles.Add(New ScriptBundle("~/bundles/modernizr").Include(
            "~/Scripts/modernizr-*"))

        bundles.Add(New ScriptBundle("~/bundles/bootstrap").Include(
            "~/Scripts/bootstrap.js",
            "~/Scripts/respond.js"))

        bundles.Add(New StyleBundle("~/Content/css").Include(
            "~/Content/bootstrap.css",
            "~/Content/Site.css"))

        ' Set EnableOptimizations to false for debugging. For more information,
        ' visit http://go.microsoft.com/fwlink/?LinkId=301862
        BundleTable.EnableOptimizations = True
    End Sub
End Module

﻿using System.Web;
using System.Web.Optimization;

namespace CustomImageBrowser.Web
{
    public static class Bundles
    {
        public static class JS
        {
            public static string Shared = "~/bundles/js";
        }
    }

    public class BundleConfig
    {
        // Remove built in ignore list and create our own.
        // This solves the problem of not having the unminified version
        // of a script and allows us to only include the *.min.js versions.
        public static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            ignoreList.Clear();
            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");
            ignoreList.Ignore("*.debug.js");
        }

        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            AddDefaultIgnorePatterns(bundles.IgnoreList);

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle(Bundles.JS.Shared).Include(
                    "~/Scripts/tinymce/tinymce.min.js",
                    "~/Scripts/jquery.magnific-popup.min.js",
                    "~/Scripts/fileuploader.js",
                    "~/Scripts/bootstrap.min.js",
                    "~/Scripts/site.js"
            ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/Content/bootstrap/css/bootstrap.min.css",
                    "~/Content/magnific-popup/magnific-popup.css",
                    "~/Content/fileuploader/fileuploader.css",
                    "~/Content/site.css"
                    ));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

        }
    }
}
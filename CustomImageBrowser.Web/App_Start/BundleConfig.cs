using System.Web;
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
                        "~/Scripts/jquery-{version}.min.js"));

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
    }
}
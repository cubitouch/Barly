using System.Web;
using System.Web.Optimization;

namespace Barly
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-2.2.2.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/dialog-polyfill.js",
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/mdl").Include(
                      "~/Scripts/chosen.jquery.js",
                      "~/Scripts/main.js",
                      "~/Scripts/material.min.js"));

            bundles.Add(new StyleBundle("~/Content/mdl").Include(
                      "~/Content/animate.css",
                      "~/Content/chosen.css",
                      "~/Content/ionicons.css",
                      "~/Content/material.css",
                      "~/Content/reset.css",
                      "~/Content/styles.css",
                      "~/Content/dialog-polyfill.css"));
        }
    }
}

using System.Web;
using System.Web.Optimization;

namespace CIS726_Assignment2
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
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

            // @russfeld 
            // Added a bundle for the site's javascript file
            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                        "~/Scripts/site.js"));

            // @russfeld 
            // Added a bundle for the site's flowchart javascript file
            bundles.Add(new ScriptBundle("~/bundles/flowchart").Include(
                        "~/Scripts/jqModal.js",
                        "~/Scripts/flowchart.js"));

            // @russfeld 
            // Added a bundle for the site's popup javascript file
            bundles.Add(new ScriptBundle("~/bundles/popup").Include(
                        "~/Scripts/popup.js"));

            // @russfeld 
            // Added a bundle for the site's popup javascript file
            bundles.Add(new ScriptBundle("~/bundles/flowchartstatic").Include(
                        "~/Scripts/flowchartstatic.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/Site.css", "~/Content/jqModal.css"));
            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/style.css"));

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
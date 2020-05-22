using System.Web.Optimization;

namespace OSDevGrp.ReduceFoodWaste.WebApplication
{
    public static class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-3.5.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-1.12.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate.min.js",
                "~/Scripts/jquery.validate.unobtrusive.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                "~/Content/themes/base/core.css",
                "~/Content/themes/base/resizable.css",
                "~/Content/themes/base/selectable.css",
                "~/Content/themes/base/accordion.css",
                "~/Content/themes/base/autocomplete.css",
                "~/Content/themes/base/button.css",
                "~/Content/themes/base/dialog.css",
                "~/Content/themes/base/slider.css",
                "~/Content/themes/base/tabs.css",
                "~/Content/themes/base/datepicker.css",
                "~/Content/themes/base/progressbar.css",
                "~/Content/themes/base/theme.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrap/css").Include(
                "~/Content/bootstrap-grid.min.css",
                "~/Content/bootstrap.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/umd/popper.min.js",
                "~/Scripts/bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/Content/progress.overlay/css").Include(
                "~/Content/progress.overlay.css"));

            bundles.Add(new ScriptBundle("~/bundles/progress.overlay").Include(
                "~/Scripts/progress.overlay.js"));
        }
    }
}
using System.Web;
using System.Web.Optimization;

namespace TaoStore
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/Admin/css").Include(
                "~/Asset/Admin/css/bootstrap.min.css",
                "~/Asset/Admin/css/font-awesome.min.css",
                "~/Asset/Admin/css/nalika-icon.css",
                "~/Asset/Admin/css/owl.carousel.css",
                "~/Asset/Admin/css/owl.theme.css",
                "~/Asset/Admin/css/owl.transitions.css",
                "~/Asset/Admin/css/animate.css",
                "~/Asset/Admin/css/normalize.css",
                "~/Asset/Admin/css/meanmenu.min.css",
                "~/Asset/Admin/css/main.css",
                "~/Asset/Admin/css/morrisjs/morris.css",
                "~/Asset/Admin/css/scrollbar/jquery.mCustomScrollbar.min.css",
                "~/Asset/Admin/css/metisMenu/metisMenu.min.css",
                "~/Asset/Admin/css/metisMenu/metisMenu-vertical.css",
                "~/Asset/Admin/css/calendar/fullcalendar.min.css",
                "~/Asset/Admin/css/calendar/fullcalendar.print.min.css",
                "~/Asset/Admin/style.css",
                "~/Asset/Admin/css/responsive.css"
                ));
            bundles.Add(new ScriptBundle("~/bundles/admin/js").Include(
               "~/Asset/Admin/js/vendor/jquery-1.12.4.min.js",
               "~/Asset/Admin/js/bootstrap.min.js",
               "~/Asset/Admin/js/wow.min.js",
               "~/Asset/Admin/js/jquery-price-slider.js",
               "~/Asset/Admin/js/jquery.meanmenu.js",
               "~/Asset/Admin/js/owl.carousel.min.js",
               "~/Asset/Admin/js/jquery.sticky.js",
               "~/Asset/Admin/js/jquery.scrollUp.min.js",
               "~/Asset/Admin/js/scrollbar/jquery.mCustomScrollbar.concat.min.js",
               "~/Asset/Admin/js/scrollbar/mCustomScrollbar-active.js",
               "~/Asset/Admin/js/metisMenu/metisMenu.min.js",
               "~/Asset/Admin/js/metisMenu/metisMenu-active.js",
               "~/Asset/Admin/js/sparkline/jquery.sparkline.min.js",
               "~/Asset/Admin/js/sparkline/jquery.charts-sparkline.js",
               "~/Asset/Admin/js/calendar/moment.min.js",
               "~/Asset/Admin/js/calendar/fullcalendar.min.js",
               "~/Asset/Admin/js/calendar/fullcalendar-active.js",
               "~/Asset/Admin/js/flot/jquery.flot.js",
               "~/Asset/Admin/js/flot/jquery.flot.resize.js",
                "~/Asset/Admin/js/flot/curvedLines.js",
                "~/Asset/Admin/js/flot/flot-active.js",
                "~/Asset/Admin/js/plugins.js"
                ));
            bundles.Add(new StyleBundle("~/bundles/page/css").Include(
                "~/Asset/Page/style.css"));
            bundles.Add(new ScriptBundle("~/bundles/page/js").Include(
              "~/Asset/Page/script.js"));
        }
    }
}

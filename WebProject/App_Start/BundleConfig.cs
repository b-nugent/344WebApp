using System.Web;
using System.Web.Optimization;

namespace WebApplication5
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*",
                        "~/Scripts/GoogleAnalytics.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            // FullCalendar Script and Style bundles
            bundles.Add(new StyleBundle("~/Content/fullcalendarcss").Include(
                     "~/Content/fullcalendar.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                      "~/Scripts/moment.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/fullcalendar").Include(
                       "~/Scripts/fullcalendar.js"));
            
            // This is the bundle for all of the JS needed for homepage, etc.
            bundles.Add(new ScriptBundle("~/bundles/core").Include(
                        "~/Scripts/jquery.min.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/jquery-ui-1.8.20.js",
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/Chat.js",
                        "~/Scripts/jquery.validate.unobtrusive.js",
                        "~/Scripts/GoogleAnalytics.js",
                        "~/Scripts/highstock.js"));
        }
    }
}

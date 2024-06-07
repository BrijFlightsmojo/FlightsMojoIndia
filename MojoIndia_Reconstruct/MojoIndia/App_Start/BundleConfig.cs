using System.Web;
using System.Web.Optimization;

namespace MojoIndia
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/MojoIndiaJs").Include(
                "~/Scripts/jquery-3.4.1.js",              
                "~/Scripts/jquery-ui.js",              
                "~/Scripts/jquery.validate_New.min.js",
                "~/Scripts/SearchEngine.js",
                "~/Scripts/CommanJs.js"));
           
            bundles.Add(new ScriptBundle("~/bundles/MojoIndiaJsResult").Include(
                "~/Scripts/jquery-3.4.1.js",              
                "~/Scripts/jquery-ui.js",
                "~/Scripts/SearchEngine.js",
                "~/Scripts/CommanJs.js",
                "~/Scripts/angular.min.js",             
                "~/Scripts/ng-infinite-scroll.js" ));

            bundles.Add(new ScriptBundle("~/bundles/mojoFlightIndiaPassenger").Include(
                "~/Scripts/jquery-3.4.1.js",
                "~/Scripts/jquery.validate_New.min.js",
                "~/Scripts/CommanJs.js",
                "~/Scripts/PassengerPage.js",
                "~/Scripts/jquery.custom-scrollbar.js"));

            bundles.Add(new ScriptBundle("~/bundles/MojoIndiaJsPayment").Include(
              "~/Scripts/jquery-3.4.1.js",
              "~/Scripts/jquery.custom-scrollbar.js",
              "~/Scripts/jquery.validate_New.min.js",
              "~/Scripts/PaymentPage.js",
              "~/Scripts/respond.js",
              "~/Scripts/CommanJs.js"));

            bundles.Add(new StyleBundle("~/Content/cssIndia").Include(
                "~/Content/bootstrap.min.css",
                "~/Content/jquery-ui.css",
                "~/Content/stylesheet.css",
                "~/Content/responsive.css",
                "~/Content/popstyle.css"));

            bundles.Add(new StyleBundle("~/Content/cssIndiaResult").Include(
                "~/Content/bootstrap.min.css",
                "~/Content/jquery-ui.css",
                "~/Content/owl.carousel.min.css",
                "~/Content/stylesheet.css",
                "~/Content/responsive.css",
                "~/engine1/style.css",
                "~/Content/popstyle.css",
                "~/Content/PassengerCss.css"));


            System.Web.Optimization.BundleTable.EnableOptimizations = GlobalData.isBundle;
        }
    }
}

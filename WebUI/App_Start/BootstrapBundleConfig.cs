using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace BootstrapSupport
{
    public class BootstrapBundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js").Include(
                "~/Scripts/jquery-migrate-{version}.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/bootstrap-select.min.js",
                "~/Scripts/bootstrap-datepicker.js"

                ));

            bundles.Add(new StyleBundle("~/content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/body.css",
                "~/Content/bootstrap-responsive.css",
                "~/Content/bootstrap-mvc-validation.css",
                "~/Content/bootstrap-select.min.css",
                "~/Content/datepicker.css"
                ));
        }
    }
}
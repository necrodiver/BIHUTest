using System.Web;
using System.Web.Optimization;

namespace WebBiHuGadget
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/group").Include(
                    "~/Content/semantic.css",
                    "~/Content/iconfont/iconfont.css",
                    "~/Content/index.css"));
            bundles.Add(new ScriptBundle("~/bundles/group").Include(
                    "~/Scripts/jquery/jquery-{version}.js",
                    "~/Scripts/ctrl/helper.js",
                    "~/Scripts/vue.js",
                    "~/Scripts/semantic.js",
                    "~/Scripts/iconfont.js",
                    "~/Scripts/ctrl/home.js"));

            bundles.Add(new StyleBundle("~/Content/analysis").Include(
                        "~/Content/analys/analysis.css"));

            bundles.Add(new ScriptBundle("~/bundles/analysis").Include(
                        "~/Scripts/ctrl/analysisExcel.js"));
        }
    }
}

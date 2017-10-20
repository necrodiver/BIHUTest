using System.Web;
using System.Web.Mvc;
using BiHuGadget.Helpers;

namespace WebBiHuGadget
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new UserAuthorizeAttribute());
        }
    }
}

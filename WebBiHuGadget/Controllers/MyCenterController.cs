using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBiHuGadget.Controllers
{
    public class MyCenterController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.IsLogin = 0;
            if (this.Account != null)
            {
                ViewBag.IsLogin = 1;
            }
            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBiHuGadget.Controllers
{
    public class LeaveController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.LeftTime = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd HH:mm");
            ViewBag.RightTime = DateTime.Now.AddDays(30).ToString("yyyy-MM-dd HH:mm");
            ViewBag.UserName = this.Account.UserName;
            return View();
        }
    }
}
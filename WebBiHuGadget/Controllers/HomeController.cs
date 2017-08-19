using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BiHuGadget.Helpers;
using BiHuGadget.Models;
using WebBiHuGadget.Models;
using BiHuGadget.Bll;

namespace WebBiHuGadget.Controllers
{
    public class HomeController : Controller
    {
        User_Bll userBll = new User_Bll();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ModelValidationMVCFilter]
        [ValidateAntiForgeryToken]
        public JsonResult LoginIn(View_Login request)
        {
            return Json("1", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSingleUser(int userId)
        {
            var userModel = userBll.GetSingleUser(new UserModel { UserId = userId });
            return Json(userModel, JsonRequestBehavior.AllowGet);
        }
    }
}
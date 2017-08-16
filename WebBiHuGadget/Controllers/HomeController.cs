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
        Users_Bll usersBll = new Users_Bll();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ModelValidationMVCFilter]
        [ValidateAntiForgeryToken]
        public JsonResult LoginIn(View_Login request)
        {
            UserModel userModel = new UserModel();
            userModel.UserName = request.UserName;
            userModel.Pwd = request.Pwd;
            usersBll.SetUser(userModel);
            return Json("1", JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUsers()
        {
            return Json(usersBll.GetUsers(), JsonRequestBehavior.AllowGet);
        }
    }
}
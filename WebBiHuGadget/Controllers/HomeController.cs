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
    public class HomeController : BaseController
    {
        User_Bll userBll = new User_Bll();
        public ActionResult Index()
        {
            if (this.Account == null)
                ViewBag.IsLogin = 1;
            if (this.Account != null &&this.Account.UserId!=null)
            {
                ViewBag.IsLogin = 0;
            }
            return View();
        }

        [HttpPost]
        [ModelValidationMVCFilter]
        [ValidateAntiForgeryToken]
        public JsonResult LoginIn(View_Login request)
        {
            MessageModel msgModel = new MessageModel();
            msgModel.MsgTitle = "登录";
            msgModel.MsgStatus = false;

            UserModel userModel = new UserModel();
            userModel.Email = request.Email;
            userModel.Pwd = request.Pwd;
            var backUser = userBll.GetSingleUser(userModel);
            if (backUser == null || string.IsNullOrWhiteSpace(backUser.Email))
            {
                msgModel.MsgContent = "登录失败，当前账号不存在";
                return Json(msgModel, JsonRequestBehavior.AllowGet);
            }
            SessionHelper.SaveSession(userModel, "UserInfo");
            msgModel.MsgStatus = true;
            msgModel.MsgContent = "登录成功";
            return Json(msgModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSingleUser(int userId)
        {
            var userModel = userBll.GetSingleUser(new UserModel { UserId = userId });
            return Json(userModel, JsonRequestBehavior.AllowGet);
        }
    }
}
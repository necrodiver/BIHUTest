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
        RolesAndAuthority_Bll raBll = new RolesAndAuthority_Bll();
        public ActionResult Index()
        {
            ViewBag.IsLogin = 0;
            if (this.Account != null)
            {
                ViewBag.IsLogin = 1;
            }
            return View();
        }
        public ActionResult About()
        {
            ViewBag.IsLogin = 0;
            if (this.Account != null)
            {
                ViewBag.IsLogin = 1;
            }
            return View();
        }

        [HttpPost]
        [ModelValidationMVCFilter]
        [ValidateAntiForgeryToken]
        public JsonResult LoginIn(View_Login request)
        {
            MessageModel msgModel = new MessageModel
            {
                MsgTitle = "登录",
                MsgStatus = false
            };


            UserModel userModel = new UserModel
            {
                Email = request.Email,
                Pwd = request.Pwd
            };

            var backUser = userBll.GetSingleUser(userModel);
            if (backUser == null || string.IsNullOrWhiteSpace(backUser.Email) || backUser.CreateTime == null || backUser.UserId == null)
            {
                msgModel.MsgContent = "登录失败，当前账号用户名或密码错误";
                return Json(msgModel, JsonRequestBehavior.AllowGet);
            }
            if (backUser.RoleId != null && backUser.RoleId > -1)
            {
                RoleModel roleModel = raBll.GetSingleRole(Convert.ToInt32(backUser.RoleId));
                if (roleModel == null)
                {
                    msgModel.MsgStatus = false;
                    msgModel.MsgContent = "登录失败";
                    return Json(msgModel, JsonRequestBehavior.AllowGet);
                }
                List<AuthorityModel> authorityList = raBll.GetAuthorityList(Convert.ToInt32(roleModel.RoleId));
                AccountUser account = new AccountUser
                {
                    UserId = Convert.ToInt32(backUser.UserId),
                    UserName = backUser.UserName,
                    Email = backUser.Email,
                    Pwd = backUser.Pwd,
                    CreateTime = Convert.ToDateTime(backUser.CreateTime),
                    RoleId = roleModel.RoleId,
                    RoleName = roleModel.RoleName,
                    AuthorityList = authorityList
                };
                SessionHelper.SaveSession(account, Settings.AccountSessionKey);
                msgModel.MsgStatus = true;
                msgModel.MsgContent = "登录成功";
                return Json(msgModel, JsonRequestBehavior.AllowGet);
            }
            msgModel.MsgStatus = false;
            msgModel.MsgContent = "登录失败";
            return Json(msgModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSingleUser(int userId)
        {
            var userModel = userBll.GetSingleUser(new UserModel { UserId = userId });
            return Json(userModel, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult SingleOut()
        {
            SessionHelper.RemoveAllSession();
            return RedirectToAction("Index","Home");
            //return Json(null, JsonRequestBehavior.AllowGet);
        }
    }
}
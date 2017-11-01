using BiHuGadget.Bll;
using BiHuGadget.Helpers;
using BiHuGadget.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBiHuGadget.Controllers
{
    public class MyCenterController : BaseController
    {
        private static User_Bll userBll = new User_Bll();
        public ActionResult Index()
        {
            ViewBag.IsLogin = 0;
            if (this.Account != null)
            {
                ViewBag.IsLogin = 1;
            }
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult GetUserList()
        {
            MessageModel msg = new MessageModel
            {
                MsgTitle = "获取用户列表",
                MsgStatus = false
            };
            try
            {
                var userList = userBll.GetListUser(new UserModel { });
                var userViewList = new List<UserViewModel>();
                if (userList != null)
                {
                    userList.ForEach(u =>
                    {
                        var userChild = new UserViewModel
                        {
                            UserId = Convert.ToInt32(u.UserId),
                            UserName = u.UserName,
                            Email = u.Email,
                            RoleId = Convert.ToInt32(u.RoleId),
                            CreateTime = Convert.ToDateTime(u.CreateTime).ToString("yyyy-MM-dd HH:mm:ss")
                        };
                        userViewList.Add(userChild);
                    });
                }
                msg.MsgStatus = true;
                msg.MsgContent = userViewList;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("获取用户列表错误：" + ex.ToString());
            }
            msg.MsgContent = "获取用户列表出错，请查看日志检查错误";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
using BiHuGadget.Bll;
using BiHuGadget.Helpers;
using BiHuGadget.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebBiHuGadget.Models;

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

        [HttpPost, ValidateAntiForgeryToken, ModelValidationMVCFilter]
        public JsonResult AddUserList(UserNameModel userNameModel)
        {
            var userNames = userNameModel.UserNames.Split(',').ToList<string>();
            var userNameList = ChineseTransformPinYinHelper.TransFormUserEmailList(userNames);
            MessageModel msg = new MessageModel();
            msg.MsgTitle = "批量新增用户";
            msg.MsgStatus = false;
            try
            {
                if (userBll.AddUserList(userNameList))
                {
                    msg.MsgContent = "批量添加成功";
                    msg.MsgStatus = true;
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
            msg.MsgContent = "添加失败,你可以查看日志来检查错误";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
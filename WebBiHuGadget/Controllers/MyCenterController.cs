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
        private static Group_Bll groupBll = new Group_Bll();
        public ActionResult Index()
        {
            ViewBag.IsLogin = 0;
            if (this.Account != null)
            {
                ViewBag.IsLogin = 1;
            }
            ViewBag.RoleId = this.Account.RoleId;
            ViewBag.UserId = this.Account.UserId;
            ViewBag.UserName = this.Account.UserName;
            ViewBag.Email = this.Account.Email;
            ViewBag.CreateTime = this.Account.CreateTime;
            ViewBag.GroupId = this.Account.GroupId;
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
                var userList = userBll.GetListUser();
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
                            CreateTime = Convert.ToDateTime(u.CreateTime).ToString("yyyy-MM-dd HH:mm:ss"),
                            GroupId = Convert.ToInt32(u.GroupId)
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
            msg.MsgContent = "获取用户列表出错，你可以重试或者查看日志检查错误";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken, ModelValidationMVCFilter]
        public JsonResult AddUserList(UserNameModel userNameModel)
        {
            MessageModel msg = new MessageModel();
            msg.MsgTitle = "批量新增用户";
            msg.MsgStatus = false;
            try
            {
                var userNames = userNameModel.UserNames.Split(',').ToList();
                var userNameList = ChineseTransformPinYinHelper.TransFormUserEmailList(userNames);

                List<UserModel> userList = new List<UserModel>();
                userList = userNameList.ConvertAll(u => new UserModel
                {
                    UserName = u.Key,
                    Email = u.Value2,
                    Pwd = AESHelper.AESEncrypt(u.Value),
                    CreateTime = DateTime.Now,
                    RoleId = Settings.AddDefaultRole,
                    GroupId=Account.GroupId
                });
                List<UserModel> seletUserList = userBll.GetListUser();
                var selectedList = userList.Where((u, i) => seletUserList.SingleOrDefault(s => s.UserName == u.UserName) != null).ToList();
                if (selectedList != null && selectedList.Count > 0)
                {
                    msg.MsgContent = "添加失败,已经存在有相同的用户";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }

                if (userBll.AddUserList(userList))
                {
                    msg.MsgContent = "批量添加成功";
                    msg.MsgStatus = true;
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("批量添加用户账号错误：" + ex.ToString());
            }
            msg.MsgContent = "添加失败,你可以重试或者查看日志来检查错误";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        [HttpPost, ValidateAntiForgeryToken, ModelValidationMVCFilter]
        public JsonResult DeleteUserList(UserListModel userIdModel)
        {
            MessageModel msg = new MessageModel();
            msg.MsgTitle = "删除用户账号";
            msg.MsgStatus = false;
            try
            {
                var userIdList = userIdModel.UserIds.Split(',').ToList();
                var selectUser = userIdList.SingleOrDefault(u => Convert.ToInt32(u) == Account.UserId);
                if (selectUser != null && selectUser.Length == 1)
                {
                    msg.MsgContent = "你不能删除自己的账号！";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                if (userBll.DeleteUserList(userIdList))
                {
                    msg.MsgContent = "删除用户账号成功";
                    msg.MsgStatus = true;
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("删除用户账号失败:" + ex.ToString());
            }
            msg.MsgContent = "删除失败，你可以重试或者查看日志来检查错误";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken, ModelValidationMVCFilter]
        public JsonResult EditUser(UserUpdateModel userModel)
        {
            MessageModel msg = new MessageModel();
            msg.MsgTitle = "修改用户账号信息";
            msg.MsgStatus = false;
            if (userModel.RoleId != null && userModel.RoleId < this.Account.RoleId)
            {
                msg.MsgContent = "你的权限等级太低，无法赋予其他用户高于你自己的权限";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var userList = userBll.GetListUser();
                var list1 = userList.SingleOrDefault(u => u.UserId == userModel.UserId);
                if (list1 == null || list1.UserId == null)
                {
                    msg.MsgContent = "无此用户,无法更新";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                var list2 = userList.SingleOrDefault(u => u.Email == userModel.Email && u.UserId != userModel.UserId);
                if (list2 != null && list2.UserId != null)
                {
                    msg.MsgContent = "当前用户的Email已有人使用，请重新选择";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                var list3 = userList.SingleOrDefault(u => u.Email == userModel.Email && u.UserId == userModel.UserId);
                if (list3 != null && list3.UserId != null)
                {
                    msg.MsgContent = "当前更改的Email地址与原Email地址相同，不予修改";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }

                UserModel user = new UserModel
                {
                    UserId = userModel.UserId,
                    Pwd = userModel.Pwd,
                    Email = userModel.Email,
                    RoleId = userModel.RoleId,
                    GroupId=userModel.GroupId
                };
                if (userBll.EditUser(user))
                {
                    msg.MsgContent = "修改用户账号账号信息成功";
                    msg.MsgStatus = true;
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("修改用户账号信息失败:" + ex.ToString());
            }
            msg.MsgContent = "修改失败，你可以重试或者查看日志来检查错误";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取分组列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetGroupList()
        {
            MessageModel msg = new MessageModel
            {
                MsgTitle = "获取分组列表",
                MsgStatus = true
            };
            var groupList = groupBll.GetGroupList();
            msg.MsgContent = groupList;

            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 操作分组，增、删、改
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken, ModelValidationMVCFilter]
        public JsonResult EditGroup(View_GroupModel request)
        {
            MessageModel msg = new MessageModel
            {
                MsgTitle = "操作分组",
                MsgStatus = false
            };
            GroupModel groupModel = new GroupModel
            {
                GroupId = request.GroupId,
                GroupName = request.GroupName
            };
            switch (request.MarkIUD)
            {
                case MarkIUD.Delete:
                    msg.MsgStatus = groupBll.DeleteGroup(groupModel);
                    break;
                case MarkIUD.Insert:
                    msg.MsgStatus = groupBll.AddGroup(groupModel.GroupName);
                    break;
                case MarkIUD.Update:
                    msg.MsgStatus = groupBll.EditGroup(groupModel);
                    break;
            }
            if (msg.MsgStatus)
                msg.MsgContent = "操作成功！";
            else
                msg.MsgContent = "操作分组失败，请重新尝试或者查看日志！";

            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
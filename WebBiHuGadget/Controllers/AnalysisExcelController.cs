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
    public class AnalysisExcelController : BaseController
    {
        private Attendance_Bll attendanceBll = new Attendance_Bll();
        private User_Bll userBll = new User_Bll();
        string BasePath = AppDomain.CurrentDomain.BaseDirectory + "XLS\\";
        string fileType = ".xls";
        public AnalysisExcelController()
        {
            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }
        }
        public ActionResult Index(string type)
        {
            ViewBag.IsLogin = 0;
            if (this.Account != null)
            {
                ViewBag.IsLogin = 1;
            }
            ViewBag.Show = false;
            if (!string.IsNullOrWhiteSpace(type) && type == "admin")
            {
                ViewBag.Show = true;
            }
            if (Account != null && Account.RoleId == 1)
            {
                ViewBag.Show = true;
            }
            return View();
        }

        /// <summary>
        /// 上传文件并获取全部内容
        /// </summary>
        /// <param name="requestFile">上传数据</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadXls(HttpPostedFileBase requestFile)
        {
            Log4NetHelper.Info("上传文件：FileName:" + requestFile.FileName);
            MessageModel msgModel = new MessageModel
            {
                MsgTitle = "上传文件",
                MsgStatus = false
            };

            if (requestFile == null || requestFile.FileName == null)
            {
                msgModel.MsgContent = "上传的文件不存在";
                return Json(msgModel, JsonRequestBehavior.AllowGet);
            }

            string fileName = requestFile.FileName;
            FileInfo fileInfo = new FileInfo(fileName);
            string fileExt = fileInfo.Extension;
            if (!fileExt.Equals(fileType, StringComparison.OrdinalIgnoreCase))
            {
                msgModel.MsgContent = "文件类型错误";
                return Json(msgModel, JsonRequestBehavior.AllowGet);
            }
            var fullName = Path.Combine(BasePath, Path.GetFileName(requestFile.FileName));
            if (!System.IO.File.Exists(fullName))
            {
                try
                {
                    requestFile.SaveAs(fullName);
                    msgModel.MsgStatus = true;
                    msgModel.MsgContent = "文件上传并保存成功";

                    //上传文件成功后首先读取文件到cache中，如果读取失败则直接删除并返回错误信息
                    var userList = SerializeAttendance.GetUserAllList(fullName);
                    if (userList == null)
                    {
                        System.IO.File.Delete(fullName);
                        msgModel.MsgStatus = false;
                        msgModel.MsgContent = "拒绝文件保存,或文件格式不正确,无法读取内容,请检查文件是否是Microsoft Excel格式的（非wps Excel格式）或查看日志文件";
                        return Json(msgModel, JsonRequestBehavior.AllowGet);
                    }
                    if (!CacheHelper.DisposeCache(fileName, userList))
                    {
                        msgModel.MsgStatus = false;
                        msgModel.MsgContent = "文件缓存失败，请查看日志";
                        return Json(msgModel, JsonRequestBehavior.AllowGet);
                    }
                    return Json(msgModel, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    Log4NetHelper.Error("保存文件:" + ex.ToString());
                    msgModel.MsgContent = "文件保存失败,请查看日志";
                    return Json(msgModel, JsonRequestBehavior.AllowGet);
                }
            }
            msgModel.MsgContent = "文件已存在，请勿重复上传";
            return Json(msgModel, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 获取考勤数据
        /// </summary>
        /// <param name="monthName">考勤名称</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetMonthData(int monthNum)
        {
            Log4NetHelper.Info("获取考勤数据：monthNum:" + monthNum);
            MessageModel msgModel = new MessageModel
            {
                MsgTitle = "读取考勤数据",
                MsgStatus = false
            };

            if (monthNum == 0 || monthNum > 12)
            {
                msgModel.MsgContent = "查询数据错误，请勿直接调用";
            }
            var userListOld = CacheHelper.GetCache(monthNum.ToString());
            if (userListOld != null)
            {
                try
                {
                    Dictionary<string, Dictionary<string, UserAll>> userListNew = (Dictionary<string, Dictionary<string, UserAll>>)userListOld;
                    msgModel.MsgStatus = true;
                    msgModel.MsgTitle = monthNum.ToString();
                    msgModel.MsgContent = Json(userListNew).Data;
                    return Json(msgModel, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    Log4NetHelper.Error("从缓存中拿数据并进行转换:" + ex.ToString());
                    msgModel.MsgContent = "查询数据不存在(查询条件输入失败或请上传相关文件再查询)，或请检查日志";
                    return Json(msgModel, JsonRequestBehavior.AllowGet);
                }
            }
            var monthFileName = $"{monthNum}月考勤.xls";
            var fullName = Path.Combine(BasePath, Path.GetFileName(monthFileName));
            if (!System.IO.File.Exists(fullName))
            {
                msgModel.MsgContent = "查询数据不存在(查询条件输入失败或请上传相关文件再查询)";
                return Json(msgModel, JsonRequestBehavior.AllowGet);
            }

            var userList = SerializeAttendance.GetUserAllList(fullName);
            if (userList == null)
            {
                msgModel.MsgContent = "读取文件失败，请检查错误日志进行查看错误";
                return Json(msgModel, JsonRequestBehavior.AllowGet);
            }
            CacheHelper.DisposeCache(monthNum.ToString(), userList);//若在内存中未找到则直接再次查询文件，然后把数据保存到内存中
            msgModel.MsgStatus = true;
            msgModel.MsgTitle = monthNum.ToString();
            msgModel.MsgContent = Json(userList).Data;
            return Json(msgModel, JsonRequestBehavior.AllowGet);
        }
        #region 增删改打卡记录
        [HttpPost]
        [ModelValidationMVCFilter]
        public JsonResult EditMarkStatus(View_EditMark request)
        {
            MessageModel msg = new MessageModel
            {
                MsgTitle = "操作打卡备注说明",
                MsgStatus = false
            };
            AttendanceModel attendanceModel = new AttendanceModel
            {
                AttendanceId = request.AttendanceId,
                ClockYear = request.ClockTime.Year,
                ClockTime = request.ClockTime.ToString("yyyy-MM-dd"),
                UDayStateId = request.UDayStateId,
                TimeSlot = request.TimeSlot,
                ClockContent = request.ClockContent,
                UserId = request.UserId
            };

            #region 查询当前用户信息
            if (!request.UserName.Equals(Account.UserName) && Account.RoleId > 1)
            {
                msg.MsgContent = "你的权限不足以操作别人的打卡备注！";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var userModel = userBll.GetSingleUser(new UserModel
                {
                    UserName = request.UserName
                });
                if (userModel == null)
                {
                    msg.MsgContent = "当前操作打卡备注的用户不存在！";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                attendanceModel.UserId = userModel.UserId;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("查询打卡备注用户信息出错：" + request.UserName + "  错误信息：" + ex.ToString());
                msg.MsgContent = "查询打卡备注用户出错！请查看日志";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            #endregion

            switch (request.MarkIUD)
            {
                case MarkIUD.Delete:
                    if (request.UserId <= 0 || request.UserId != Account.UserId)
                    {
                        msg.MsgContent = "当前操作的用户打卡备注与登录用户不同，请使用正确的账号登录";
                        return Json(msg, JsonRequestBehavior.AllowGet);
                    }
                    if (attendanceModel.AttendanceId <= 0)
                    {
                        msg.MsgContent = "当前打卡备注为空！";
                        msg.MsgStatus = false;
                        return Json(msg, JsonRequestBehavior.AllowGet);
                    }
                    return DeleteMarkContent(attendanceModel.AttendanceId, msg);
                case MarkIUD.Insert:
                    if (this.Account.UserName.Equals(attendanceModel))
                        attendanceModel.UserId = this.Account.UserId;
                    return AddMarkContent(attendanceModel, msg);
                case MarkIUD.Update:
                    if (request.UserId <= 0 || request.UserId != this.Account.UserId)
                    {
                        msg.MsgContent = "当前操作的用户打卡备注与登录用户不同，请使用正确的账号登录";
                        return Json(msg, JsonRequestBehavior.AllowGet);
                    }
                    return EditMarkContent(attendanceModel, msg);
            }
            msg.MsgContent = "未知错误!请检查错误日志";
            msg.MsgStatus = false;
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        //添加打卡记录
        private JsonResult AddMarkContent(AttendanceModel aModel, MessageModel msg)
        {
            try
            {
                if (attendanceBll.AddAttendance(aModel))
                {
                    msg.MsgContent = "插入打卡备注说明成功";
                    msg.MsgStatus = true;
                }
                else
                {
                    msg.MsgContent = "插入打卡备注说明失败，你可以重新插入，或请检查日志";
                    msg.MsgStatus = false;
                }
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("插入打卡备注说明：" + ex.ToString());
                msg.MsgContent = "插入失败，你可以重新插入，或请检查日志";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }
        //修改打卡记录
        private JsonResult EditMarkContent(AttendanceModel aModel, MessageModel msg)
        {
            try
            {
                if (attendanceBll.EditAttendance(aModel))
                {
                    msg.MsgContent = "修改打卡备注说明成功";
                    msg.MsgStatus = true;
                }
                else
                {
                    msg.MsgContent = "修改打卡备注说明失败，你可以重新提交，或请检查日志";
                    msg.MsgStatus = false;
                }
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("修改打卡备注说明：" + ex.ToString());
                msg.MsgContent = "插入失败，你可以重新插入，或请检查日志";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }
        //删除打卡记录
        private JsonResult DeleteMarkContent(int? attendanceId, MessageModel msg)
        {
            if (attendanceId == null)
            {
                Log4NetHelper.Info("EditMarkContent操作获取的attendanceId值为空");
                msg.MsgContent = "要删除的条件不存在，请重新提交或检查日志";
                msg.MsgStatus = false;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            try
            {
                if (attendanceBll.DeleteAttendance(Convert.ToInt32(attendanceId)))
                {
                    msg.MsgContent = "删除打卡备注说明成功";
                    msg.MsgStatus = true;
                }
                else
                {
                    msg.MsgContent = "删除打卡备注说明失败，你可以重新提交，或请检查日志";
                    msg.MsgStatus = false;
                }
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("删除打卡备注说明：" + ex.ToString());
                msg.MsgContent = "删除失败，你可以重新删除，或请检查日志";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        /// <summary>
        /// 获取单个用户的打卡备注说明
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelValidationMVCFilter]
        public JsonResult GetUserMarkData(View_MonthInUserMark request)
        {
            MessageModel msg = new MessageModel
            {
                MsgTitle = "获取打卡备注",
                MsgStatus = false
            };

            try
            {
                AttendanceSearchModel asModel = new AttendanceSearchModel
                {
                    UserId = request.UserId,
                    UserName = request.UserName,
                    ClockYear = request.Year
                };
                List<AttendanceModel> adList = attendanceBll.GetUserMarks(asModel);
                if (adList == null || adList.Count == 0)
                {
                    msg.MsgStatus = false;
                    msg.MsgContent = "查询无记录";
                }
                else
                {
                    msg.MsgStatus = true;
                    msg.MsgContent = adList;
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("获取用户打卡备注：" + ex.ToString());
                msg.MsgStatus = false;
                msg.MsgContent = "获取用户打卡备注集合失败，请重新尝试或者查看日志记录";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
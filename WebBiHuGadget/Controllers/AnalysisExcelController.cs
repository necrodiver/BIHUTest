using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BiHuGadget.Helpers;
using BiHuGadget.Models;

namespace WebBiHuGadget.Controllers
{
    public class AnalysisExcelController : BaseController
    {
        string BasePath = AppDomain.CurrentDomain.BaseDirectory + "XLS\\";
        string fileType = ".xls";
        public AnalysisExcelController()
        {
            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }
        }
        public ActionResult Index()
        {
            ViewBag.IsLogin = 0;
            if (this.Account != null)
            {
                ViewBag.IsLogin = 1;
            }

            ViewBag.Show = false;
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
    }
}
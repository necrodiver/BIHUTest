using System;
using System.Web.Mvc;
using BiHuGadget.Helpers;
using BiHuGadget.Models;

namespace WebBiHuGadget.Controllers
{
    public class AnalysisStatisticController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.IsLogin = 0;
            if (this.Account != null)
            {
                ViewBag.IsLogin = 1;
            }
            return View();
        }

        [HttpPost]
        public JsonResult GetAnalysisUserList()
        {
            MessageModel msgModel = new MessageModel();
            msgModel.MsgTitle = "统计数据";
            msgModel.MsgStatus = false;
            try
            {
                var usersList = AnalysisStatistics.AnalysisUserData();
                if (usersList == null || usersList.Count == 0)
                {
                    msgModel.MsgContent = "数据获取失败，请查看日志文件";
                    return Json(msgModel, JsonRequestBehavior.AllowGet);
                }
                msgModel.MsgContent = usersList;
                msgModel.MsgStatus = true;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("获取统计数据:" + ex.ToString());
                msgModel.MsgContent = "数据获取错误，请查看日志文件";
                return Json(msgModel, JsonRequestBehavior.AllowGet);
            }
            return Json(msgModel, JsonRequestBehavior.AllowGet);
        }
    }
}
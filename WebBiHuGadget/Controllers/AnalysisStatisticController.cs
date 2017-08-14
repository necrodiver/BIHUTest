﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BiHuGadget.Helpers;
using BiHuGadget.Models;

namespace WebBiHuGadget.Controllers
{
    public class AnalysisStatisticController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetUserList()
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
                LogCollectHelper.ErrorLog("获取统计数据：" + ex.ToString());
                msgModel.MsgContent = "数据获取错误，请查看日志文件";
                return Json(msgModel, JsonRequestBehavior.AllowGet);
            }
            return Json(msgModel, JsonRequestBehavior.AllowGet);
        }
    }
}
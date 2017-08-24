using BiHuGadget.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BiHuGadget.Helpers
{
    /// <summary>
    /// 模型过滤器验证
    /// </summary>
    public class ModelValidationMVCFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var modelState = filterContext.Controller.ViewData.ModelState;
            if (!modelState.IsValid)
            {
                var errorMessage = modelState.Values
                    .SelectMany(m => m.Errors)
                    .Select(m => m.ErrorMessage)
                    .First();
                filterContext.Result = new JsonResult()
                {
                    Data = new { status = -1, msg = errorMessage }
                };
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }

    /// <summary>
    /// 权限过滤器
    /// </summary>
    public class UserAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string controllerName = filterContext.RouteData.Values["controller"].ToString().ToLower();
            string actionName = filterContext.RouteData.Values["action"].ToString().ToLower();
            var account = SessionHelper.GetSession<AccountUser>("Account");
            MessageModel msgModel = new MessageModel();
            msgModel.MsgTitle = "权限问题";
            msgModel.MsgStatus = false;
            if (account != null && account.AuthorityList != null && account.AuthorityList.Count > 0)
            {
                var thisAuthorize = account.AuthorityList.Find(a => a.ActionName.ToLower() == actionName && a.ControllerName.ToLower() == controllerName);
                //var thisAuthorize = account.AuthorityList.SingleOrDefault(a => a.ActionName == actionName && a.ControllerName == controllerName);
                if (thisAuthorize != null)
                    return;
                msgModel.MsgContent = "权限不足";
            }
            else
            {
                msgModel.MsgContent = "请先登录再进行操作";
            }
            filterContext.Result = new EmptyResult();
            filterContext.HttpContext.Response.Write(ObjectToJSON(msgModel));
            //HttpContext.Current.Response.Write("请先进行登录!");
            base.OnAuthorization(filterContext);
        }
        public static string ObjectToJSON<T>(T obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            string result = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                ms.Position = 0;

                using (StreamReader read = new StreamReader(ms))
                {
                    result = read.ReadToEnd();
                }
            }
            return result;
        }
    }

    public class ExceptionFilters : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                string controllerName = filterContext.RouteData.Values["controller"].ToString().ToLower();
                string actionName = filterContext.RouteData.Values["action"].ToString().ToLower();
                Log4NetHelper.Error("异常检测：controllerName:" + controllerName + ",actionName:" + actionName, filterContext.Exception.Message);
            }
            base.OnException(filterContext);
        }
    }
}

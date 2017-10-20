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
                    Data = new { MsgStatus = false, MsgContent = errorMessage }
                };
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }

    /// <summary>
    /// 权限过滤器
    /// </summary>
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string controllerName = filterContext.RouteData.Values["controller"].ToString().ToLower();
            string actionName = filterContext.RouteData.Values["action"].ToString().ToLower();
            var account = SessionHelper.GetSession<AccountUser>(Settings.AccountSessionKey);
            MessageModel msgModel = new MessageModel
            {
                MsgTitle = "权限问题",
                MsgStatus = false
            };
            if (account != null && account.AuthorityList != null && account.AuthorityList.Count > 0)
            {
                var thisAuthorize = account.AuthorityList.Find(a => a.ActionName.ToLower() == actionName && a.ControllerName.ToLower() == controllerName);
                if (thisAuthorize != null)
                    return;
                msgModel.MsgContent = "权限不足";
            }
            else
            {
                msgModel.MsgContent = "请先登录再进行操作";
            }
            filterContext.Result = new EmptyResult();
            filterContext.HttpContext.Response.StatusCode = 403;
            filterContext.HttpContext.Response.Write(msgModel.ObjectToJSON());
            base.OnAuthorization(filterContext);
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

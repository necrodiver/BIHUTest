using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
}

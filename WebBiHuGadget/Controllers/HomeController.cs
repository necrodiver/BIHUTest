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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LoginIn()
        {

            //        HttpCookie cookie = new HttpCookie("cookieName");
            //        cookie.Value = "name1"
            //        HttpContext.Current.Response.Cookies.Add(cookie);
            //        读取：

            //HttpContext.Current.Request.Cookies["cookieName"].Value
            //判断cookie是否存在：

            //if(HttpContext.Current.Request.Cookies["cookieName"]==null){
            ////do something
            //}
            //设置cookie有效期

            //cookie.Expires = DateTime.Now.AddDays(1);
        }
    }
}
using BiHuGadget.Helpers;
using BiHuGadget.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBiHuGadget.Controllers
{
    public class BaseController : Controller
    {
        protected UserModel Account
        {
            get
            {
                return Session != null ? SessionHelper.GetSession<UserModel>("UserInfo") : (UserModel)System.Web.HttpContext.Current.Session["UserInfo"];
            }
            set
            {
                SessionHelper.SaveSession(value, "UserInfo");
            }
        }
    }
}
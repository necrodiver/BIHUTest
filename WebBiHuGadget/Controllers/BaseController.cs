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
        public BaseController()
        {
            ViewBag.IsLogin = Account != null ? 1 : 0;
        }
        protected AccountUser Account
        {
            get
            {
                return Session != null ? SessionHelper.GetSession<AccountUser>(Settings.AccountSessionKey) : (AccountUser)System.Web.HttpContext.Current.Session[Settings.AccountSessionKey];
            }
            set
            {
                SessionHelper.SaveSession(value, Settings.AccountSessionKey);
            }
        }
    }
}
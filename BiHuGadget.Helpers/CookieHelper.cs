using BiHuGadget.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BiHuGadget.Helpers
{
    public static class CookieHelper
    {
        public static bool SetCookie(this CookieModel cookieModel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cookieModel.Name)) return false;
                HttpCookie cookie = new HttpCookie(cookieModel.Name);
                cookie.Value = cookieModel.Value;
                cookie.Expires = DateTime.Now.Add(cookieModel.ExpireTime);
                return true;
            }
            catch (Exception ex)
            {
                LogCollectHelper.ErrorLog("写入Cookie:" + ex.ToString());
            }
            return false;
        }
        public static string GetCookie(this string cookieName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cookieName)) return null;
                var cookie = HttpContext.Current.Request.Cookies[cookieName];
                if (cookie == null) return null;
                return cookie.Value;
            }
            catch (Exception ex)
            {
                LogCollectHelper.ErrorLog("读取Cookie存储值:" + ex.ToString());
            }
            return null;
        }
        public static DateTime? GetCookieExpireTime(this string cookieName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cookieName)) return null;
                var cookie = HttpContext.Current.Request.Cookies[cookieName];
                if (cookie == null) return null;
                return cookie.Expires;
            }
            catch (Exception ex)
            {
                LogCollectHelper.ErrorLog("读取Cookie过期时间:" + ex.ToString());
            }
            return null;
        }
    }
}

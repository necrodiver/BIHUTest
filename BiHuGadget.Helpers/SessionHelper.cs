﻿using System.Web;

namespace BiHuGadget.Helpers
{
    public class SessionHelper
    {
        public SessionHelper()
        {
            HttpContext.Current.Session.Timeout = 10;
        }
        /// <summary>
        /// 存储Session
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="sessionContent">存储内容</param>
        /// <param name="sessionKey">Key值</param>
        /// <returns></returns>
        public static bool SaveSession<T>(T sessionContent, string sessionKey) where T : class, new()
        {
            HttpContext.Current.Session[sessionKey] = sessionContent;
            return true;
        }

        /// <summary>
        /// 获取Session
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="sessionKey">Key值</param>
        /// <returns></returns>
        public static T GetSession<T>(string sessionKey) where T : class, new()
        {
            if (HttpContext.Current.Session[sessionKey] == null)
            {
                return null;
            }
            return HttpContext.Current.Session[sessionKey] as T;
        }
        /// <summary>
        /// 删除Session
        /// </summary>
        /// <param name="sessionKey">Key值</param>
        /// <returns></returns>
        public static bool RemoveSession(string sessionKey)
        {
            if (HttpContext.Current.Session[sessionKey] == null)
            {
                return false;
            }
            HttpContext.Current.Session.Remove(sessionKey);
            return true;
        }
        /// <summary>
        /// 删除所有当前会话的Session
        /// </summary>
        /// <returns></returns>
        public static void RemoveAllSession()
        {
            HttpContext.Current.Session.Abandon();
        }
    }

}

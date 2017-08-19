using BiHuGadget.Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;

namespace BiHuGadget.Helpers
{
    public class CacheHelper
    {
        /// <summary>
        /// 截取文件名中的数字，然后存储缓存中的用户数据
        /// </summary>
        /// <param name="fileName">文件名(非绝对路径)</param>
        /// <param name="userList">缓存的数据</param>
        /// <returns></returns>
        public static bool DisposeCache(string fileName, Dictionary<string, Dictionary<string, UserAll>> userList)
        {
            string cacheKey = string.Empty;
            int num = 0;

            cacheKey = System.Text.RegularExpressions.Regex.Replace(fileName, @"[^0-9]+", "");
            if (!int.TryParse(cacheKey, out num))
            {
                return false;
            }
            if (num > 0 && num < 13 && userList != null && userList.Count > 0)
            {
                TimeSpan tp = new TimeSpan(30, 0, 0, 0, 0);
                return SetCache(num.ToString(), userList, tp);
            }
            return false;
        }
        /// <summary>  
        /// 获取数据缓存  
        /// </summary>  
        /// <param name="CacheKey">键</param>  
        public static object GetCache(string CacheKey)
        {
            try
            {
                return HttpRuntime.Cache[CacheKey];
            }
            catch (Exception ex)
            {
                Log4NetHelper.Fatal("读取缓存数据:" + ex.ToString());
            }
            return null;
        }

        /// <summary>  
        /// 设置数据缓存  
        /// </summary>  
        public static bool SetCache(string CacheKey, object objObject)
        {
            try
            {
                HttpRuntime.Cache.Insert(CacheKey, objObject);
                return true;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Fatal("写入缓存数据0:" + ex.ToString());
            }
            return false;
        }

        /// <summary>  
        /// 设置数据缓存  
        /// </summary>  
        public static bool SetCache(string CacheKey, object objObject, TimeSpan Timeout)
        {
            try
            {
                HttpRuntime.Cache.Insert(CacheKey, objObject, null, DateTime.MaxValue, Timeout, CacheItemPriority.NotRemovable, null);
                return true;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Fatal("写入缓存数据1:" + ex.ToString());
            }
            return false;

        }

        /// <summary>  
        /// 设置数据缓存  
        /// </summary>  
        public static bool SetCache(string CacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            try
            {
                HttpRuntime.Cache.Insert(CacheKey, objObject, null, absoluteExpiration, slidingExpiration);
                return true;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Fatal("写入缓存数据2:" + ex.ToString());
            }
            return false;
        }

        /// <summary>  
        /// 移除指定数据缓存  
        /// </summary>  
        public static bool RemoveAllCache(string CacheKey)
        {
            try
            {
                HttpRuntime.Cache.Remove(CacheKey);
                return true;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Fatal("移除指定数据缓存:" + ex.ToString());
            }
            return false;
        }

        /// <summary>  
        /// 移除全部缓存  
        /// </summary>  
        public static bool RemoveAllCache()
        {
            try
            {
                Cache _cache = HttpRuntime.Cache;
                while (_cache.GetEnumerator().MoveNext())
                {
                    _cache.Remove(_cache.GetEnumerator().Key.ToString());
                }
                return true;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Fatal("移除全部缓存:" + ex.ToString());
            }
            return false;

        }
    }
}
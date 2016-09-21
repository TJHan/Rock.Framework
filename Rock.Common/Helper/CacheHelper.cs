using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web;

namespace Rock.Common.Helper
{
    /// <summary>
    /// 高速缓存助手类
    /// 告诉缓存策略：
    /// 缓存信息包括键对应的值，以及把键添加到用户或者全局的键列表集合里并缓存此键列表
    /// </summary>
    public static class CacheHelper
    {
        /// <summary>
        /// 全局缓存键名称
        /// </summary>
        private static string GLBOALCACHEKEYLISTNAME = ConfigurationManager.AppSettings["GlobalCacheListName"];

        /// <summary>
        /// 用户缓存键名称
        /// </summary>
        private static string USERCACHEKEYLISTNAME = ConfigurationManager.AppSettings["UserCacheListName"];

        /// <summary>
        /// 当前web应用域名
        /// </summary>
        private static string GetRequestKey
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return "APP";
                }
                return HttpContext.Current.Request.Url.Host;
            }
        }

        /// <summary>
        /// 当前站点所有全局缓存键列表,默认缓存时间为5小时
        /// </summary>
        private static List<string> GlobalCacheKeyList
        {
            get
            {
                List<string> list = new List<string>();
                System.Web.Caching.Cache cache = HttpRuntime.Cache;
                if (cache[GLBOALCACHEKEYLISTNAME] != null)
                {
                    list = (List<string>)HttpRuntime.Cache[GLBOALCACHEKEYLISTNAME];
                }
                else
                {
                    cache.Insert(GLBOALCACHEKEYLISTNAME, list, null, DateTime.Now.AddHours(5), TimeSpan.Zero);
                }
                return list;
            }
            set
            {
                if (value != null)
                {
                    System.Web.Caching.Cache cache = HttpRuntime.Cache;
                    cache.Remove(GLBOALCACHEKEYLISTNAME);
                    cache.Insert(GLBOALCACHEKEYLISTNAME, value, null, DateTime.Now.AddHours(5), TimeSpan.Zero);
                }
            }
        }

        /// <summary>
        /// 当前站点所有用户缓存键列表,默认缓存时间为5小时
        /// </summary>
        private static List<string> UserCacheKeyList
        {
            get
            {
                List<string> list = new List<string>();
                System.Web.Caching.Cache cache = HttpRuntime.Cache;
                if (cache[USERCACHEKEYLISTNAME] != null)
                {
                    list = (List<string>)HttpRuntime.Cache[USERCACHEKEYLISTNAME];
                }
                else
                {
                    cache.Insert(USERCACHEKEYLISTNAME, list, null, DateTime.Now.AddHours(5), TimeSpan.Zero);
                }
                return list;
            }
            set
            {
                if (value != null)
                {
                    System.Web.Caching.Cache cache = HttpRuntime.Cache;
                    cache.Remove(USERCACHEKEYLISTNAME);
                    cache.Insert(USERCACHEKEYLISTNAME, value, null, DateTime.Now.AddHours(5), TimeSpan.Zero);
                }
            }
        }


        /// <summary>
        /// 用于生成全局缓存键名称的函数
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="userGUID">用户主键GUID</param>
        /// <returns></returns>
        private static string GetCacheKeyString(string key, string userGUID = "")
        {
            if (!string.IsNullOrEmpty(userGUID))
            {
                return string.Format(@"{0}$_U_{1}_{2}", GetRequestKey, key, userGUID);
            }
            else
            {
                return string.Format(@"{0}$_G_{1}", GetRequestKey, key);
            }
        }

        #region 添加缓存
        /// <summary>
        /// 添加Cache
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="userGUID">用户主键GUID</param>
        /// <param name="cacheMinutes">缓存有效期，默认60分钟</param>        
        public static void SetCache(string key, object value, string userGUID = "", int cacheMinutes = 60)
        {
            string cacheKey = GetCacheKeyString(key, userGUID);
            if (value != null)
            {
                //检索当前缓存键列表，并加入此缓存键
                if (!string.IsNullOrEmpty(userGUID))
                {
                    //添加到用户缓存键列表
                    List<string> cachekeylist = UserCacheKeyList;
                    if (cachekeylist.IndexOf(cacheKey) == -1)
                    {
                        cachekeylist.Add(cacheKey);
                        UserCacheKeyList = cachekeylist;
                    }
                }
                else
                {
                    //添加到全局缓存键列表
                    List<string> cachekeylist = GlobalCacheKeyList;
                    if (cachekeylist.IndexOf(cacheKey) == -1)
                    {
                        cachekeylist.Add(cacheKey);
                        GlobalCacheKeyList = cachekeylist;
                    }
                }

                System.Web.Caching.Cache cache = HttpRuntime.Cache;
                cache.Remove(cacheKey);
                cache.Insert(cacheKey, value, null, DateTime.Now.AddMinutes(cacheMinutes), TimeSpan.Zero);
            }
        }
        #endregion

        #region 获取缓存
        /// <summary>
        /// 获取Cache
        /// </summary>
        /// <typeparam name="T">缓存值类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="userGUID">用户的主键GUID</param>
        /// <returns></returns>        
        public static T GetCache<T>(string key, string userGUID = "")
        {
            string cacheKey = GetCacheKeyString(key, userGUID);
            System.Web.Caching.Cache cache = HttpRuntime.Cache;
            if (cache[cacheKey] == null)
            {
                return default(T);
            }
            T cacheValue = (T)cache[cacheKey];
            if (cacheValue == null)
            {
                return default(T);
            }
            return cacheValue;
        }
        #endregion

        #region 删除缓存
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="userGUID">用户主键GUID</param>        
        public static void ClearCache(string key, string userGUID = "")
        {
            string cacheKey = GetCacheKeyString(key, userGUID);
            if (HttpRuntime.Cache[cacheKey] != null)
            {
                HttpRuntime.Cache.Remove(cacheKey);
                if (!string.IsNullOrEmpty(userGUID))
                {
                    //从用户级缓存键列表中删除指定缓存键
                    List<string> cachekeylist = UserCacheKeyList;
                    if (cachekeylist.IndexOf(cacheKey) > -1)
                    {
                        cachekeylist.Remove(cacheKey);
                        UserCacheKeyList = cachekeylist;
                    }
                }
                else
                {
                    //从全局缓存键列表中删除指定缓存键
                    List<string> cachekeylist = GlobalCacheKeyList;
                    if (cachekeylist.IndexOf(cacheKey) > -1)
                    {
                        cachekeylist.Remove(cacheKey);
                        GlobalCacheKeyList = cachekeylist;
                    }
                }
            }
        }

        /// <summary>
        /// 清除某个用户的所有用户级缓存
        /// </summary>
        /// <param name="userGUID">用户主键GUID</param>        
        public static void ClearAllUserCache(string userGUID)
        {
            List<string> cacheList = UserCacheKeyList.Where(d => d.IndexOf(userGUID) > -1).ToList<string>();
            if (cacheList != null && cacheList.Count > 0)
            {
                var list = UserCacheKeyList;
                System.Web.Caching.Cache cache = HttpRuntime.Cache;
                //清除所有此用户的缓存，并把缓存键从缓存键列表中移除
                foreach (string item in cacheList)
                {
                    cache.Remove(item);
                    list.Remove(item);
                }
                UserCacheKeyList = list;
            }
        }

        /// <summary>
        /// 清除所有全局缓存
        /// </summary>
        public static void ClearAllGlobalCache()
        {
            if (GlobalCacheKeyList != null && GlobalCacheKeyList.Count > 0)
            {
                System.Web.Caching.Cache cache = HttpRuntime.Cache;
                foreach (string item in GlobalCacheKeyList)
                {
                    cache.Remove(item);
                }
                GlobalCacheKeyList.Clear();
            }
        }

        /// <summary>
        /// 清除站点缓存
        /// </summary>
        public static void ClearAllCache()
        {
            //清除全局缓存键列表中键及对应的缓存
            if (GlobalCacheKeyList.Count > 0)
            {
                System.Web.Caching.Cache cache = HttpRuntime.Cache;
                foreach (string item in GlobalCacheKeyList)
                {
                    cache.Remove(item);
                }
                GlobalCacheKeyList.Clear();
            }
            //清除用户缓存键列表中键及对应的缓存
            if (UserCacheKeyList.Count > 0)
            {
                System.Web.Caching.Cache cache = HttpRuntime.Cache;
                foreach (string item in UserCacheKeyList)
                {
                    cache.Remove(item);
                }
                UserCacheKeyList.Clear();
            }
        }

        #endregion
    }
}

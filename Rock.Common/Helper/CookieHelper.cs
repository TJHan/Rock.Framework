using Rock.Common.Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Rock.Common.Helper
{
    public static class CookieHelper
    {
        /// <summary>
        /// 根据cookie名称获取Cookie
        /// </summary>
        /// <param name="cookieName">Cookie名</param>
        /// <returns></returns>
        public static HttpCookie GetCookie(string cookieName)
        {
            if (HttpContext.Current.Request == null)
                return null;
            if (HttpContext.Current.Request.Cookies[cookieName] == null)
                return null;
            return HttpContext.Current.Request.Cookies[cookieName];
        }

        /// <summary>
        /// 获取cookie值
        /// </summary>
        /// <param name="cookieName">cookie名称</param>
        /// <param name="key">键名</param>
        /// <returns></returns>
        public static String GetCookieValue(string cookieName, string key)
        {
            HttpCookie cookie = GetCookie(cookieName);
            if (cookie != null)
            {
                string result = cookie[key];
                if (!string.IsNullOrEmpty(result))
                {
                    return DESEncrypt.Decrypt(result, PublicCryptKeys.Key);
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 添加cookie值
        /// </summary>
        /// <param name="cookieName">cookie名称</param>
        /// <param name="dicValues">需要缓存的键值对</param>
        /// <param name="expiresMinutes">过期日期 单位：分钟</param>
        public static void SetCookieValue(string cookieName, Dictionary<string, string> dicValues, int expiresMinutes = 0)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            bool isNew = true;
            if (cookie == null)
            {
                cookie = new HttpCookie(cookieName);
                cookie.Path = HttpContext.Current.Request.ApplicationPath;
                foreach (var item in dicValues)
                {
                    cookie.Values.Add(item.Key, DESEncrypt.Encrypt(item.Value, PublicCryptKeys.Key));
                }

            }
            else
            {
                isNew = false;
                cookie.Path = HttpContext.Current.Request.ApplicationPath;
                foreach (var item in dicValues)
                {
                    cookie[item.Key] = DESEncrypt.Encrypt(item.Value, PublicCryptKeys.Key);
                }
            }
            if (expiresMinutes > 0)
                cookie.Expires = DateTime.Now.AddMinutes(expiresMinutes);

            //cookie.Domain = HttpContext.Current.Request.Url.Host;
            if (isNew)
                HttpContext.Current.Response.Cookies.Add(cookie);
            else
                HttpContext.Current.Response.Cookies.Set(cookie);
        }

        /// <summary>
        /// 清理Cookie缓存值
        /// 此函数只清理某cookie的缓存值，并不删除此cookie
        /// </summary>
        /// <param name="cookieName">cookie名称</param>
        public static void ClearCookie(string cookieName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null)
            {
                cookie.Path = HttpContext.Current.Request.ApplicationPath;
                cookie.Expires = new DateTime(1900, 1, 1);
                cookie.Values.Clear();                
                HttpContext.Current.Response.Cookies.Set(cookie);
            }            
        }

    }
}

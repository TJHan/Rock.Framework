using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;

namespace Rock.Common.Helper
{
    /// <summary>
    /// 站点Session助手类
    /// 备注：童鞋们是不是尽量不使用session呢？
    /// </summary>
    public static class SessionHelper
    {
        /// <summary>
        /// 添加session
        /// </summary>
        /// <param name="key">session键</param>
        /// <param name="value">值</param>
        public static void SetSession(string key, object value)
        {
            string sessionName = GetSessionName(key);
            HttpContext.Current.Session.Add(sessionName, value);
        }

        /// <summary>
        /// 获取session
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">session键</param>
        /// <returns></returns>
        public static T GetSession<T>(string key)
        {
            try
            {
                string sessionName = GetSessionName(key);
                object result = HttpContext.Current.Session[sessionName];
                if (result == null)
                {
                    return default(T);
                }
                return (T)result;
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 生成Session名称
        /// </summary>
        /// <param name="key">session键</param>
        /// <returns></returns>
        private static string GetSessionName(string key)
        {
            return string.Format(@"{0}_{1}", GetRequestKey, key);
        }

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
    }

    /// <summary>
    /// 站点高速缓存键
    /// </summary>
    public static class SiteKeys
    {
        #region 登录功能键
        /// <summary>
        /// 登录验证码
        /// </summary>
        public const string LOGIN_VALIDATION_CODE = "BRAND_LOGIN_VALIDATION_CODE";

        /// <summary>
        /// 一个月免登录
        /// </summary>
        public const string LOGIN_REMEBER_USER_ACCOUNT = "BRAND_LOGIN_REMEBER_USER_ACCOUNT";

        /// <summary>
        /// 手机验证码
        /// </summary>
        public const string USER_MOBILE_VALIDATION_CODE = "BRAND_USER_MOBILE_VALIDATION_CODE";

        /// <summary>
        /// 登录用户的用户详情
        /// </summary>
        public const string USER_MOBILE_LOGIN_INFO = "BRAND_USER_MOBILE_LOGIN_INFO";
        #endregion

    }
}

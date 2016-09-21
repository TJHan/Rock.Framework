using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Rock.Logging
{
    public class LogHelper<T>
    {
        static ILog log = LogManager.GetLogger(typeof(T));

        public static void LogInfo(string msg)
        {
            log.Info(msg);
        }

        public static void LogWarin(string msg)
        {
            log.Warn(msg);
        }

        public static void LogError(string msg)
        {
            log.Error(msg);
        }

        public static void LogException(Exception ex)
        {
            string msg = string.Format(@"发生异常：{0}， Message：{1}", ex.StackTrace, ex.Message);
            log.Error(msg, ex);
        }

        public static void LogDebug(string msg)
        {
            log.Debug(msg);
        }
    }
}

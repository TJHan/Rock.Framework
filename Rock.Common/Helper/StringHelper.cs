using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rock.Common.Helper
{
    public static class StringHelper
    {
        #region 字符串拓展方法
        public static int ToInt(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return 0;
            int result = 0;
            if (int.TryParse(str, out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        public static Decimal ToDecimal(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return 0;
            decimal result = 0;
            str = str.Trim();
            if (Decimal.TryParse(str, out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        public static DateTime ToDateTime(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return DateTime.MinValue;
            DateTime result = DateTime.MinValue;
            str = str.Trim();
            if (DateTime.TryParse(str, out result))
            {
                return result;
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        #endregion
    }
}

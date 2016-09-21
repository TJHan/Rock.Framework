using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Rock.Common.Encryption;
using Rock.Logging;

namespace Rock.Business.Controllers
{
    public class ShopController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                Dictionary<string, string> list = new Dictionary<string, string>();
                list.Add("SHA1", HashCrypt.SHA1Encrypt("abc"));
                list.Add("SHA1W", HashCrypt.SHA1("abc"));
                list.Add("MD5", HashCrypt.MD5Encrypt("visualstudio@1"));
                list.Add("MD5W", HashCrypt.MD5("abc"));
                ExceptionTest.CreateException();
                return View(list);
            }
            catch {
                throw;
            }
        }
    }
    public class ExceptionTest
    {
        public static void CreateException()
        {
            try
            {
                int i = 0, j = 1;
                int result = j / i;
            }
            catch (Exception ex)
            {
                LogHelper<ExceptionTest>.LogException(ex);
                throw;
            }
        }
    }
}

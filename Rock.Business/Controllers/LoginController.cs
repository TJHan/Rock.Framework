using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Rock.Business.Models;
using Rock.Common.Helper;
using System.Web;
using Rock.Business.Filters;

namespace Rock.Business.Controllers
{
    public class LoginController : BaseController
    {
        public ActionResult Index()
        {
            LoginModel model = new LoginModel();
            HttpCookie cookie = CookieHelper.GetCookie("LoginUser");
            if (cookie != null)
            {
                model.UserName = CookieHelper.GetCookieValue("LoginUser", "UserName");
                model.Password = CookieHelper.GetCookieValue("LoginUser", "Password");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            if (model.IsKeepLogin)
            {
                Dictionary<string, string> unameDict = new Dictionary<string, string>();
                unameDict.Add("UserName", model.UserName);
                unameDict.Add("Password", model.Password);
                CookieHelper.SetCookieValue("LoginUser", unameDict, 30);
            }
            return RedirectToAction("Index", "Shop");
        }

    }
}

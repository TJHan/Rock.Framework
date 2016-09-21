using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Rock.Business.Controllers
{
    public class BaseController : Controller
    {
        protected override void Execute(System.Web.Routing.RequestContext requestContext)
        {
            requestContext.HttpContext.Response.Write(string.Format(@"Controller: {0}, Action: {1}"
                ,requestContext.RouteData.Values["controller"].ToString()
                ,requestContext.RouteData.Values["action"]));
        }
    }
}

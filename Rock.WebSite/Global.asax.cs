﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using log4net;
using log4net.Config;
using System.IO;

namespace Rock.WebSite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            InitLog4Net();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void InitLog4Net()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception error = Server.GetLastError();
            if (error != null)
            {
                int errorCode = error.GetHashCode();
                switch (errorCode)
                {
                    case 404:
                        break;
                    default:
                        break;
                }
            }
            Response.Redirect("~/Error/404.html");
        }
    }
}

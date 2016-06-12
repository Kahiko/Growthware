using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;

namespace GrowthWare.WebApplication
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            GlobalConfiguration.Configure(WebApiConfig.Register);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthorizeRequest()
        {
            if (IsWebApiRequest())
            {
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            }
        }

        private static Boolean IsWebApiRequest()
        {
            bool mRetval = false;
            if (HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.Contains("/gw/") || HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.Contains("/api/"))
            {
                mRetval = true;
            }
            return mRetval;
        }
    }
}
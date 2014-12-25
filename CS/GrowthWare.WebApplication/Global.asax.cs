using GrowthWare.WebApplication.Controllers;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;

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
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.Contains("/gw/api/");
        }
    }
}
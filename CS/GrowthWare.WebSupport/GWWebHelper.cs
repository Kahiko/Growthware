using GrowthWare.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GrowthWare.WebSupport
{
    /// <summary>
    /// GWWebHelper Contains non volital data needed throughout the system.
    /// </summary>
    /// <remarks></remarks>
    public static class GWWebHelper
    {
        private static Exception s_ExceptionError = null;

        /// <summary>
        /// Gets the core web administration verison.
        /// </summary>
        /// <value>The core web administration verison.</value>
        public static string CoreWebAdministrationVersion
        {
            get
            {
                string myVersion = string.Empty;
                Assembly myAssembly = Assembly.Load("GrowthWare.WebApplication");
                if ((myAssembly != null))
                {
                    myVersion = myAssembly.GetName().Version.ToString();
                }
                return myVersion;
            }
        }

        private static string s_Version = string.Empty;

        /// <summary>
        /// Gets the environment.
        /// </summary>
        /// <value>The environment.</value>
        public static String DisplayEnvironment
        {
            get
            {
                return ConfigSettings.EnvironmentDisplayed;
            }
        }

        /// <summary>
        /// Gets or sets the exception error.
        /// </summary>
        /// <value>The exception error.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static Exception ExceptionError
        {
            get { return s_ExceptionError; }
            set { s_ExceptionError = value; }
        }

        /// <summary>
        /// Gets the frame work verison.
        /// </summary>
        /// <value>The frame work verison.</value>
        public static string FrameworkVersion
        {
            get
            {
                string myVersion = string.Empty;
                Assembly myAssembly = Assembly.Load("GrowthWare.WebSupport");
                if ((myAssembly != null))
                {
                    myVersion = myAssembly.GetName().Version.ToString();
                }
                return myVersion;
            }
        }

        /// <summary>
        /// Gets the query value.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns>String.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static String GetQueryValue(HttpRequest request, String queryString)
        {
            if (request == null) throw new ArgumentNullException("request", "request cannot be a null reference (Nothing in Visual Basic)! (Nothing in VB)!");
            String mRetVal = String.Empty;
            if (request.QueryString[queryString] != null)
            {
                mRetVal = request.QueryString[queryString].ToString();
            }
            return mRetVal;
        }

        /// <summary>
        /// Returns http(s)://FQDN(/AppName)
        /// </summary>
        /// <value>String</value>
        /// <returns>String</returns>
        /// <remarks></remarks>
        public static string RootSite
        {
            get
            {
                string myRoot_Site = string.Empty;
                string myHTTP_Schema = string.Empty;
                if (ConfigSettings.ForceHttps)
                {
                    myHTTP_Schema = "HTTPS";
                }
                else
                {
                    myHTTP_Schema = HttpContext.Current.Request.Url.Scheme;
                }
                if (HttpContext.Current.Request.ApplicationPath == "/")
                {
                    myRoot_Site = myHTTP_Schema + "://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + "/";
                }
                else
                {
                    myRoot_Site = myHTTP_Schema + "://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + "/" + ConfigSettings.AppName + "/";
                }
                return myRoot_Site;
            }
        }

        /// <summary>
        /// Returns MapPath("~\Public\Skins\")
        /// </summary>
        /// <value>String</value>
        /// <returns>String</returns>
        /// <remarks></remarks>
        public static string SkinPath
        {
            get { return HttpContext.Current.Server.MapPath(@"~\Public\Skins\"); }
        }

        /// <summary>
        /// Gets the verison.
        /// </summary>
        /// <value>The verison.</value>
        public static string Version
        {
            get
            {
                if (String.IsNullOrEmpty(s_Version))
                {
                    s_Version = System.Reflection.Assembly.GetCallingAssembly().GetName().Version.ToString();
                }
                return s_Version;
            }
        }
    }
}

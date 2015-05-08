using GrowthWare.Framework.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;

namespace GrowthWare.WebSupport
{
    /// <summary>
    /// GWWebHelper Contains non volatile data needed throughout the system.
    /// </summary>
    /// <remarks></remarks>
    public static class GWWebHelper
    {
        /// <summary>
        /// Constant value of 3 representing the Link Behavior for the name
        /// </summary>
        public const int LinkBehaviorNameValuePairSequenceId = 3;
        /// <summary>
        /// Constant value of 1 representing the Link Behavior for navigation
        /// </summary>
        public const int LinkBehaviorNavigationTypesSequenceId = 1;

        /// <summary>
        /// Constant value of 1 representing the DataKeyField for Roles
        /// </summary>
        public const string RoleDataKeyField = "ROLE_SEQ_ID";

        private static Exception s_ExceptionError = null;

        private static Random s_Random = new Random(System.DateTime.Now.Millisecond);

        /// <summary>
        /// Gets the core web administration version.
        /// </summary>
        /// <value>The core web administration version.</value>
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
        /// Gets the frame work version.
        /// </summary>
        /// <value>The frame work version.</value>
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
        /// Gets the random password.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetNewGuid
        {
            get
            {
                string retVal = null;
                retVal = System.Guid.NewGuid().ToString();
                return retVal;
            }
        }

        /// <summary>
        /// Gets the query or form value.
        /// </summary>
        /// <param name="request">The HttpContext.Current.Request</param>
        /// <param name="queryString">The query string.</param>
        /// <returns>String.Empty or value as string.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static String GetQueryValue(HttpRequest request, String queryString)
        {
            if (request == null) throw new ArgumentNullException("request", "request cannot be a null reference (Nothing in Visual Basic)! (Nothing in VB)!");
            String mRetVal = String.Empty;
            if (request[queryString] != null)
            {
                mRetVal = request[queryString].ToString();
            }
            return mRetVal;
        }

        /// <summary>
        /// Gets the random number.
        /// </summary>
        /// <param name="startingNumber">The starting number.</param>
        /// <param name="endingNumber">The ending number.</param>
        /// <returns>System.String.</returns>
        public static string GetRandomNumber(int startingNumber, int endingNumber)
        {
            int retVal = 0;
            //if passed incorrect arguments, swap them
            //can also throw exception or return 0
            if (startingNumber > endingNumber)
            {
                int t = startingNumber;
                startingNumber = endingNumber;
                endingNumber = t;
            }
            retVal = s_Random.Next(startingNumber, endingNumber);
            NativeMethods.Sleep((int)(System.DateTime.Now.Millisecond * (retVal / 100)));
            return retVal.ToString(CultureInfo.InvariantCulture);
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
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
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

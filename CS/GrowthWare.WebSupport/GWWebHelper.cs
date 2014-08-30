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
    static class GWWebHelper
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
            if (request == null) throw new ArgumentNullException("request", "request can not be null (Nothing in VB)!");
            String mRetVal = String.Empty;
            if (request.QueryString[queryString] != null)
            {
                mRetVal = request.QueryString[queryString].ToString();
            }
            return mRetVal;
        }
    }
}

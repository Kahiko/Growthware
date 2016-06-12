using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using GrowthWare.Framework.Model.Enumerations;
using System.Configuration;
using System.Globalization;
using GrowthWare.Framework.Common;

namespace GrowthWare.Framework.Web
{
	/// <summary>
	/// Servers as a collection of configuration information specific to the web.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1053:StaticHolderTypesShouldNotHaveConstructors")]
	public sealed class WebConfigSettings : ConfigSettings
	{

		/// <summary>
		/// Private constructure
		/// </summary>
		private WebConfigSettings() 
		{ 
		
		}

		/// <summary>
		/// Returns the application path
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public static string AppPath
		{
			get
			{
				if (HttpContext.Current.Request.ApplicationPath == "/")
				{
					return string.Empty;
				}
				return HttpContext.Current.Request.ApplicationPath;
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
			get { return HttpContext.Current.Server.MapPath("~\\Public\\Skins\\"); }
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
				if (ForceHTTPS)
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
					myRoot_Site = myHTTP_Schema + "://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + "/" + AppName + "/";
				}
				return myRoot_Site;
			}
		}

		/// <summary>
		/// Returns a Fully Quilifed Domain Name and Page
		/// </summary>
		/// <value>String</value>
		/// <returns>String</returns>
		/// <remarks>Calculated value based on RootSite and BasePage properties</remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "FQDN")]
		public static string FQDNPage
		{
			get { return RootSite + ConfigSettings.BasePage; }
		}

		/// <summary>
		/// Returns "Public/Images/"
		/// </summary>
		/// <value>String</value>
		/// <returns>String</returns>
		/// <remarks>Should be moved a CONFIG setting</remarks>
		public static string ImagePath
		{
			get { return RootSite + "Public/Images/"; }
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.Framework.Web.Utilities;
using System.Text;
using System.Configuration;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using System.Runtime.InteropServices;
using System.Reflection;

namespace GrowthWare.Framework.Web
{
	/// <summary>
	/// GWWebHelper Contains non volital data needed throughout the system.
	/// </summary>
	/// <remarks></remarks>
	public class GWWebHelper
	{
		private static Exception _ExceptionError = null;
		private static string _FQDNBasePage = string.Empty;
		private static string _Version = string.Empty;

		private static Exception m_ExceptionError = null;
		private static String m_FQDNBasePage = String.Empty;
		private static String m_skin = "Default";
		private static String m_Version = String.Empty;

		public const String DBStatusOnline = "OnLine";
		public const String DBStatusOffline = "OffLine";
		public const String DBStatusInstall = "Install";
		public const String UnderConstructionPage = "UnderConstruction.aspx";
		public const String UnderConstructionPath = "~/Public/Pages/";
		public const String InstallPage = "Install.aspx";
		public const String InstallPath = "~/Public/Pages/";
		public const String TimeoutPage = "TimeOut.aspx";
		public const String TimeoutPath = "~/Public/Pages/";


		[DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern void Sleep(long dwMilliseconds);



		private GWWebHelper()
		{
		}

		public static void CustomStyle(MClientChoicesState choices, ref StringBuilder strBuilder, ref System.Web.UI.HtmlControls.HtmlGenericControl yourCustomStyles, string bgColor)
		{
			strBuilder.Append("             .BG_HEAD_COLOR {background-color: " + choices[MClientChoices.HeadColor].ToString() + "}" + Environment.NewLine);
			strBuilder.Append("             .BG_SUB_HEAD_COLOR {background-color: " + choices[MClientChoices.SubheadColor] + "}" + Environment.NewLine);
			yourCustomStyles.InnerHtml = strBuilder.ToString();
		}

		public static void CustomScript(ref StringBuilder strBuilder, string headColor, string subHeadColor)
		{
			strBuilder.Append("        <script language=\"JavaScript\" type=\"text/javascript\">" + Environment.NewLine);
			strBuilder.Append("           var imagePath='" + WebConfigSettings.ImagePath + "';" + Environment.NewLine);
			strBuilder.Append("           var HeadColor='" + headColor + "';" + Environment.NewLine);
			strBuilder.Append("           var SubheadColor='" + subHeadColor + "';" + Environment.NewLine);
			strBuilder.Append("           var calendarPage='" + WebConfigSettings.RootSite + "Public/Pages/System/DatePicker.aspx" + "';" + Environment.NewLine);
			strBuilder.Append("        </script>" + Environment.NewLine);
		}

		public static Exception ExceptionError
		{
			get { return _ExceptionError; }
			set { _ExceptionError = value; }
		}

		public static string FQDNBasePage
		{
			get
			{
				if (_FQDNBasePage == string.Empty)
				{
					_FQDNBasePage = WebConfigSettings.RootSite + WebConfigSettings.BasePage;
				}
				else
				{
					if (!(_FQDNBasePage == WebConfigSettings.RootSite + WebConfigSettings.BasePage))
					{
						_FQDNBasePage = WebConfigSettings.RootSite + WebConfigSettings.BasePage;
					}
				}
				return _FQDNBasePage;
			}
		}
		// FQDNBasePage

		public static string GetRandomNumber(int startingNumber, int endingNumber)
		{
			int retVal = 0;
			//initialize random number generator
			Random r = new Random(System.DateTime.Now.Millisecond);
			//if passed incorrect arguments, swap them
			//can also throw exception or return 0
			if (startingNumber > endingNumber)
			{
				int t = startingNumber;
				startingNumber = endingNumber;
				endingNumber = t;
			}
			retVal = r.Next(startingNumber, endingNumber);
			Sleep((long)(System.DateTime.Now.Millisecond * (retVal / 100)));
			return retVal.ToString();
		}

		public static void RemoveCookie(string CookieName)
		{
			HttpCookie Cookie = null;
			Cookie = new HttpCookie(CookieName);
			Cookie.Value = null;
			Cookie.Expires = new System.DateTime(1999, 10, 12);
			HttpContext.Current.Response.AppendCookie(Cookie);
		}

		//*********************************************************************
		//
		// ResolveAbsoluteUrl Method
		//
		// This method translates a relative path into an
		// absolute path.
		//
		//*********************************************************************
		public static string ResolveAbsoluteUrl(string Url)
		{
			return WebConfigSettings.RootSite + Url;
			//If HttpContext.Current.Request.IsSecureConnection Then
			//    Return "https://" + PrimaryDomain + Url
			//Else
			//    Return "http://" + PrimaryDomain + Url
			//End If
		}

		public static void ShowUnderConstruction()
		{
			try
			{
				if (WebConfigSettings.UnderConstruction)
				{
					HttpContext.Current.Response.Redirect(WebConfigSettings.RootSite + "Public/Pages/System/UnderConstruction.aspx");
				}
			}
			catch
			{
				// do nothing
			}
		}

		public static string CoreWebAdministrationVerison
		{
			get
			{
				string myVersion = string.Empty;
				Assembly myAssembly = Assembly.Load("CoreWebAdministration");
				if ((myAssembly != null))
				{
					myVersion = myAssembly.GetName().Version.ToString();
				}
				return myVersion;
			}
		}

		public static string FrameWorkVerison
		{
			get
			{
				string myVersion = string.Empty;
				Assembly myAssembly = Assembly.Load("GrowthWareFramework");
				if ((myAssembly != null))
				{
					myVersion = myAssembly.GetName().Version.ToString();
				}
				return myVersion;
			}
		}

		public static string Verison
		{
			get
			{
				if (_Version == string.Empty)
				{
					_Version = System.Reflection.Assembly.GetCallingAssembly().GetName().Version.ToString();
				}
				return _Version;
			}
		}

		public static string GetRandomPassword()
		{
			string retVal = null;
			retVal = System.Guid.NewGuid().ToString();
			return retVal;
		}
		// GetRandomString

		public static string GetURL()
		{
			HttpContext context = null;
			context = HttpContext.Current;
			object item = null;
			string myURL = "&";
			foreach (var item_loopVariable in context.Request.QueryString)
			{
				item = item_loopVariable;
				if (item.ToString() != "action" & item.ToString() != "returnurl" & item.ToString() != "wfn")
				{
					myURL += item + "=" + context.Server.UrlEncode(context.Request.QueryString[item.ToString()]) + "&";
				}
			}
			myURL = myURL.Substring(0, myURL.Length - 1);
			return myURL;
		}
		// GetURL

		public static bool IsNumeric(string strInteger)
		{
			try
			{
				int intTemp = Int32.Parse(strInteger);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static string Skin(int securityEntityID)
		{
			MSecurityEntityProfile mySEProfile = SecurityEntityUtility.GetProfile(securityEntityID);
			return mySEProfile.Skin;
		}

		/// <summary>
		/// Chops off the end of a string when the string is longer than a maximum length.
		/// </summary>
		/// <param name="text">string</param>
		/// <param name="length">int</param>
		/// <returns>string</returns>
		public static string Truncate(string text, int length)
		{
			if (text.Length > length)
			{
				text = text.Substring(0, length);
			}
			return text;
		}

		public static string BuStyle(int securityEntityID)
		{
			MSecurityEntityProfile mySEProfile = SecurityEntityUtility.GetProfile(securityEntityID);
			return mySEProfile.Style;
		}

		public static void SetEnvironmentValue(Configuration config, bool isNew, string ConfigName, string ConfigValue, bool deleteEnvironment)
		{
			if (!deleteEnvironment)
			{
				if (!isNew)
				{
					try
					{
						System.Configuration.KeyValueConfigurationElement configSetting = config.AppSettings.Settings[ConfigName];
						configSetting.Value = ConfigValue;
					}
					catch
					{
						config.AppSettings.Settings.Add(ConfigName, ConfigValue);
					}
				}
				else
				{
					config.AppSettings.Settings.Add(ConfigName, ConfigValue);
				}
			}
			else
			{
				config.AppSettings.Settings.Remove(ConfigName);
			}
		}
	}
}

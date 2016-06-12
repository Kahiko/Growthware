using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using System.Web;
using System.Globalization;
using GrowthWare.Framework.Web.Utilities;
using GrowthWare.Framework.Model.Profiles;

namespace GrowthWare.Framework.Web.Context
{
	/// <summary>
	/// The HttpContextModule add to context the ClientChoices (A seporate HTTP context module)
	/// as well as adding the requested function and current security for the requested function.
	/// </summary>
	/// <remarks></remarks>
	class HttpContextModule : IHttpModule, IRequiresSessionState
	{
		private bool m_Disposing = false;

		public void Init(HttpApplication context)
		{
			if(context != null)
			{
				ClientChoicesHttpModule mInitClientChoices = new ClientChoicesHttpModule();
				mInitClientChoices.Init(context);
				context.Error += this.applicationError;
				//'AddHandler context.BeginRequest, AddressOf Me.beginRequest
				context.AcquireRequestState += this.acquireRequestState;
				//context.EndRequest += this.EndRequest;
			}
		}

		public void Dispose()
		{
			if (!m_Disposing) 
			{
				m_Disposing = true;
			}
		}

		private void applicationError(Object sender, EventArgs e)
		{
			//Dim mEx As Exception = HttpContext.Current.Server.GetLastError
			//Dim mLog As LogUtility = LogUtility.GetInstance()
			//mLog.Error(mEx)
			//Select Case mEx.Message
			//    Case "Cannot redirect after HTTP headers have been sent"
			//        Exit Sub
			//    Case "Session state has created a session id, but cannot save it because the response was already flushed by the application."
			//        Exit Sub
			//End Select
			//'WebHelper.ExceptionError = mEx
			//'If mEx.Message.ToUpper.StartsWith("CANNOT OPEN DATABASE") Then
			//'	mLog.Error(mEx)
			//'	WebHelper.ExceptionError = mEx
			//'	HttpContext.Current.Server.ClearError()
			//'	Dim config As Configuration = WebConfigurationManager.OpenWebConfiguration("~")
			//'	Dim appSettingsSection As AppSettingsSection = TryCast(config.GetSection("appSettings"), AppSettingsSection)
			//'	WebHelper.SetEnvironmentValue(config, False, "DB_Status", "OffLine", False)
			//'	HttpContext.Current.Response.Redirect("~/Public/Pages/UnderConstruction.aspx")
			//'Else
			//'	HttpContext.Current.Server.ClearError()
			//'	NavControler.NavTo(mEx)
			//'End If
		}

		private void acquireRequestState(Object sender, EventArgs e)
		{
			if(HttpContext.Current.Session == null) return;
			if(HttpContext.Current.Request.Path.ToUpper(new CultureInfo("en-US", false)).IndexOf(".ASPX", StringComparison.Ordinal) > -1){
				if(HttpContext.Current.Request.QueryString["Action"] != null)
				{
					String mAction = HttpContext.Current.Request.QueryString["Action"].ToString();
					//process security
					AccountUtility mAccountUtilty = new AccountUtility();
					MAccountProfile mAccountProfile = mAccountUtilty.GetCurrentProfile();
					HttpContext.Current.Items.Add("CurrentAccount", mAccountProfile);
					FunctionUtility mFunctionUtilty = new FunctionUtility();
					MFunctionProfile mFunctionProfile = mFunctionUtilty.GetFunction(mAction);
					MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mAccountProfile.AssignedRoles);
				}else
				{
					//' check requesting page ... if it's the same as the defautpage then
					//' redirect using either the defaultaction or defaultauthenticatedaction
				}
			}
		}
	}
}

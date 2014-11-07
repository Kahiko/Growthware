using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using System.Web;
using System.Globalization;
using System.Configuration;
using System.Web.Configuration;
using System.Text.RegularExpressions;
using GrowthWare.WebSupport.Utilities;
using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Enumerations;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.Framework.BusinessData;

namespace GrowthWare.WebSupport.Context
{
    /// <summary>
    /// The HttpContextModule add to context the ClientChoices (A separate HTTP context module)
    /// as well as adding the requested function and current security for the requested function.
    /// </summary>
    /// <remarks></remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public class HttpContextModule : IHttpModule, IRequiresSessionState
    {
        private bool m_Disposing = false;
        private OutputFilterStream m_Filter;

        /// <summary>
        /// Initializes the class
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            if (context != null && ConfigSettings.DBStatus.ToUpper(CultureInfo.InvariantCulture) != "INSTALL")
            {
                ClientChoicesHttpModule mInitClientChoices = new ClientChoicesHttpModule();
                mInitClientChoices.Init(context);
                context.Error += this.onApplicationError;
                context.AcquireRequestState += this.onAcquireRequestState;
                context.BeginRequest += this.onBeginRequest;
                context.EndRequest += this.onEndRequest;
            }
        }

        /// <summary>
        /// Ons the begin request.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void onBeginRequest(object sender, EventArgs e)
        {
            if (processRequest())
            {
                HttpResponse mResponse = HttpContext.Current.Response;
                m_Filter = new OutputFilterStream(mResponse.Filter);
                mResponse.Filter = m_Filter;
            }
            bool mStartLogInfo = false;
            if (HttpContext.Current.Application["StartLogInfo"] == null)
            {
                HttpContext.Current.Application["StartLogInfo"] = mStartLogInfo;
            }
            else
            {
                if (HttpContext.Current.Application["StartLogInfo"].ToString().ToUpperInvariant() == "FALSE")
                {
                    mStartLogInfo = false;
                }
                else
                {
                    mStartLogInfo = true;
                }
            }

            bool mClearedCache = true;
            if (HttpContext.Current.Application["ClearedCache"] == null)
            {
                HttpContext.Current.Application["ClearedCache"] = mClearedCache;
            }
            else
            {
                if (HttpContext.Current.Application["ClearedCache"].ToString().ToUpperInvariant() == "FALSE")
                {
                    mClearedCache = false;
                }
                else
                {
                    mClearedCache = true;
                }
            }
            if (!mStartLogInfo)
            {
                HttpContext.Current.Application["StartLogInfo"] = "True";
                Logger mLog = Logger.Instance();
                mLog.SetThreshold(LogPriority.Info);
                mLog.Info("Starting Core Web Administration Version: " + GWWebHelper.CoreWebAdministrationVersion);
                mLog.Info("Framework Version: " + GWWebHelper.FrameworkVersion);
                string mCurrentLevel = ConfigSettings.LogPriority.ToUpper(CultureInfo.InvariantCulture);
                mLog.SetThreshold(mLog.GetLogPriorityFromText(mCurrentLevel));
            }
        }

        /// <summary>
        /// Ons the application error.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void onApplicationError(Object sender, EventArgs e)
        {
            Exception mEx = HttpContext.Current.Server.GetLastError();
            if (mEx != null)
            {
                Logger mLog = Logger.Instance();
                mLog.Error(mEx);
                if (mEx.GetType() == typeof(BusinessLogicLayerException)) 
                {
                    Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                    ConfigSettings.SetEnvironmentValue(config, false, "DB_Status", "OffLine", false);
                }
            }
            HttpContext.Current.Server.ClearError();
        }

        /// <summary>
        /// Ons the state of the acquire request.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void onAcquireRequestState(object sender, EventArgs e)
        {
            Logger mLog = Logger.Instance();
            mLog.Debug("onAcquireRequestState():: Started");

            if ((HttpContext.Current.Session != null))
            {
                if (HttpContext.Current.Session["EditId"] != null) HttpContext.Current.Items["EditId"] = HttpContext.Current.Session["EditId"];
                if (processRequest())
                {
                    if ((HttpContext.Current.Request.QueryString["Action"] != null))
                    {
                        string mAction = HttpContext.Current.Request.QueryString["Action"].ToString(CultureInfo.InvariantCulture);
                        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(mAction);
                        if (mFunctionProfile != null) FunctionUtility.SetCurrentFunction(mFunctionProfile);

                        string mHashCode = string.Empty;
                        string mWindowUrl = HttpContext.Current.Request.Url.ToString();
                        string[] mUrlParts = mWindowUrl.Split('?');

                        if (mUrlParts.Length > 1)
                            mHashCode = mUrlParts[1];
                        mLog.Debug("hashCode: " + mHashCode);
                        mLog.Debug("Processing action: " + mAction);

                        if (mFunctionProfile != null && !mFunctionProfile.Source.ToUpper(CultureInfo.InvariantCulture).Contains("MENUS") & !(mAction.ToUpper(CultureInfo.InvariantCulture) == "LOGOFF" | mAction.ToUpper(CultureInfo.InvariantCulture) == "LOGON" | mAction.ToUpper(CultureInfo.InvariantCulture) == "CHANGEPASSWORD"))
                        {
                            MAccountProfile mAccountProfile = AccountUtility.CurrentProfile();
                            if (mAccountProfile != null) 
                            {
                                if (!(mAccountProfile.Status == (int)SystemStatus.ChangePassword))
                                {
                                    mLog.Debug("Processing for account " + mAccountProfile.Account);
                                    MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mAccountProfile);
                                    if(mSecurityInfo != null) HttpContext.Current.Items["SecurityInfo"] = mSecurityInfo;
                                    if (!mSecurityInfo.MayView)
                                    {
                                        if (mAccountProfile.Account.ToUpper(CultureInfo.InvariantCulture) == "ANONYMOUS")
                                        {
                                            WebSupportException mException = new WebSupportException("Your session has timed out.<br/>Please sign in.");
                                            GWWebHelper.ExceptionError = mException;
                                            HttpContext.Current.Response.Redirect(GWWebHelper.RootSite + ConfigSettings.AppName + "/Functions/System/Logon/Logon.aspx");
                                        }
                                        mLog.Warn("Access was denied to Account: " + mAccountProfile.Account + " for Action: " + mFunctionProfile.Action);
                                        HttpContext.Current.Response.Redirect(GWWebHelper.RootSite + ConfigSettings.AppName + "/Functions/System/Errors/AccessDenied.aspx");
                                    }
                                }
                                else
                                {
                                    WebSupportException mException = new WebSupportException("Your password needs to be changed before any other action can be performed.");
                                    GWWebHelper.ExceptionError = mException;
                                    HttpContext.Current.Response.Redirect(GWWebHelper.RootSite + ConfigSettings.AppName + "/Functions/System/Accounts/ChangePassword.aspx#?Action=ChangePassword");
                                }                            
                            }
                        }
                        else
                        {
                            mLog.Debug("Menu data or Logoff/Logon requested");
                        }
                    }
                    else
                    {
                        mLog.Debug("QueryString(Action) is nothing");
                    }
                }
                else
                {
                    mLog.Debug("Request is not for a processing event.");
                }
            }
            else
            {
                mLog.Debug("No session exiting");
            }

            mLog.Debug("onAcquireRequestState():: Done");
        }

        /// <summary>
        /// Ons the end request.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.Exception"></exception>
        private void onEndRequest(Object sender, EventArgs e)
        {
            if (processRequest())
            {
                HttpContext mContext = (sender as HttpApplication).Context;
                bool mSendError = false;
                try
                {
                    if (mContext.Response.Headers["jsonerror"] != null)
                    {
                        string mError = string.Empty;
                        if (m_Filter != null)
                        {
                            mError = m_Filter.ReadStream();
                            if (mContext.Response.Headers["jsonerror"].ToString().ToUpperInvariant().Trim() == "TRUE")
                            {
                                mSendError = true;
                                formatError(ref mError);
                                throw (new WebSupportException(String.Concat("An AJAX error has occurred: ", Environment.NewLine, mError)));
                            }
                        }
                        else
                        {
                            if (mContext.Response.Headers["jsonerror"].ToString().ToUpperInvariant().Trim() == "TRUE")
                            {
                                mSendError = true;
                                throw (new WebSupportException(String.Concat("An AJAX error has occurred: ", Environment.NewLine)));
                            }
                        }
                    }
                }
                catch (WebSupportException ex)
                {
                    if (mSendError)
                    {
                        if (!ex.ToString().Contains("Invalid JSON primitive"))
                        {
                            Logger mLog = Logger.Instance();
                            mLog.Error(ex);
                        }
                        if (mContext != null) 
                        {
                            HttpResponse mCurrentResponse = mContext.Response;
                            if (mCurrentResponse != null)
                            {
                                mCurrentResponse.Clear();
                                mCurrentResponse.Write("{\"Message\":\"We are very sorry but an error has occurred, please try your request again.\"}");
                                mCurrentResponse.ContentType = "text/html";
                                mCurrentResponse.StatusDescription = "500 Internal Error";
                                mCurrentResponse.StatusCode = 500;
                                mCurrentResponse.TrySkipIisCustomErrors = true;
                                mCurrentResponse.Flush();
                                HttpContext.Current.Server.ClearError();
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                            }
                        }
                    }
                }
                finally
                {
                    if (m_Filter != null)
                    {
                        m_Filter.Dispose();
                        m_Filter = null;
                    }
                }
            }
        }

        private string charMatch(Match match)
        {
            var code = match.Groups["code"].Value;
            int value = Convert.ToInt32(code, 16);
            return ((char)value).ToString();
        }

        private void formatError(ref string message)
        {
            //http://stackoverflow.com/questions/6990347/how-to-decode-u0026-in-a-url
            message = message.Replace(@"\r\n", Environment.NewLine);
            message = HttpUtility.UrlDecode(message);
            message = Regex.Replace(message, @"\\u(?<code>\d{4})", charMatch);
        }

        private static bool processRequest()
        {
            bool mRetVal = false;
            if (HttpContext.Current != null) 
            {
                string mPath = HttpContext.Current.Request.Path.ToUpper(CultureInfo.InvariantCulture);
                string mFileExtension = mPath.Substring(mPath.LastIndexOf(".", StringComparison.OrdinalIgnoreCase) + 1);
                string[] mProcessingTypes = { "ASPX", "ASHX", "ASMX" };
                if (mProcessingTypes.Contains(mFileExtension) || mPath.IndexOf("/API/", StringComparison.OrdinalIgnoreCase) > -1) 
                {
                    mRetVal = true;
                } 
            }
            return mRetVal;
        }

        /// <summary>
        /// Implements IDispose
        /// </summary>
        /// <param name="disposing">Boolean</param>
        /// <remarks></remarks>
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!m_Disposing)
            {
                if (disposing)
                {
                    if (m_Filter != null) m_Filter.Dispose();
                }
                // TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                // TODO: set large fields to null.

            }
            m_Disposing = true;
        }

        /// <summary>
        /// Implements Dispose
        /// </summary>
        /// <remarks></remarks>
        public void Dispose()
        {
            Dispose(true);
        }
    }
}

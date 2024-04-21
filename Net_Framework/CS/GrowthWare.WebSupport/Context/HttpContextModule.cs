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
using System.IO;

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
                //ClientChoicesHttpModule mInitClientChoices = new ClientChoicesHttpModule();
                //mInitClientChoices.Init(context);
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
            string mAccountName = AccountUtility.HttpContextUserName();
            var mAccountProfile = AccountUtility.GetProfile(mAccountName);
            WebSupportException mException = null/* TODO Change to default(_) if this is not a reference type */;
            string mAction = GWWebHelper.GetQueryValue(HttpContext.Current.Request, "Action");
            MFunctionProfile mFunctionProfile = FunctionUtility.CurrentProfile();
            // Log some startup debug messages
            mLog.Debug("Started");
            mLog.Debug("CurrentExecutionFilePath " + HttpContext.Current.Request.CurrentExecutionFilePath);
            mLog.Debug("HttpContextUserName: " + mAccountName);
            // this is necessary only in vb.net c# does not need this or have this issue of multible posts
            if (!processRequest())
            {
                mLog.Debug("Request not for processing!");
                mLog.Debug("Ended");
                return;
            }

            if (mAccountProfile == null & mAccountName.ToUpper(CultureInfo.InvariantCulture) != "ANONYMOUS")
            {
                string mMessage = "Could not find account '" + mAccountName + "'";
                mLog.Info(mMessage);
                if (ConfigSettings.AutoCreateAccount)
                {
                    mMessage = "Creating new account for '" + mAccountName + "'";
                    mLog.Info(mMessage);
                    AccountUtility.AutoCreateAccount();
                }
                mAccountProfile = AccountUtility.GetProfile(mAccountName);
            }

            if (mAccountProfile.Account.ToUpper(CultureInfo.InvariantCulture) != "ANONYMOUS" & (mAccountProfile.LastLogOn.ToShortDateString() != DateTime.Now.ToShortDateString()))
            {
                mAccountProfile.LastLogOn = DateTime.Now;
                AccountUtility.Save(mAccountProfile, false, false);
            }

            if (string.IsNullOrEmpty(mAction))
            {
                mLog.Debug("No Action!");
                mLog.Debug("Ended");
                return;
            }
            // should the function profile be nul we'll get one based on the action
            if (mFunctionProfile == null)
            {
                mFunctionProfile = FunctionUtility.GetProfile(mAction);
            }
            // now that we've pulled it from the database or cache place it into session using the function utility
            FunctionUtility.SetCurrentProfile(mFunctionProfile);
            // we should now be able to get the security information
            var mSecurityInfo = new MSecurityInfo(mFunctionProfile, mAccountProfile);
            HttpContext.Current.Items["SecurityInfo"] = mSecurityInfo;

            // with the support of anjularjs it is now possible to have request's made for .html or static content
            // we will also have request for the api that may or may not have a session 
            // (though we have code to ensure api calls require session)
            if (HttpContext.Current.Session == null)
            {
                HandleRedirect(mLog, mAccountProfile, mException, mAction, mFunctionProfile, mSecurityInfo);
                mLog.Debug("No Session!");
                mLog.Debug("Ended");
                return;
            }
            // now we can take care of the normal session items
            MClientChoicesState mClientChoicesState = ClientChoicesUtility.GetClientChoicesState(mAccountName);
            HttpContext.Current.Items[MClientChoices.SessionName] = mClientChoicesState;
            if (!(HttpContext.Current.Session["EditId"] == null))
            {
                HttpContext.Current.Items["EditId"] = HttpContext.Current.Session["EditId"];
            }
            FunctionUtility.SetCurrentProfile(mFunctionProfile);
            HandleRedirect(mLog, mAccountProfile, mException, mAction, mFunctionProfile, mSecurityInfo);
        }

        /// <summary>
        /// Handles any redirection that may or may not be needed
        /// </summary>
        /// <param name="log"></param>
        /// <param name="accountProfile"></param>
        /// <param name="webSupportException"></param>
        /// <param name="action"></param>
        /// <param name="functionProfile"></param>
        /// <param name="securityInfo"></param>
        private static void HandleRedirect(Logger log, MAccountProfile accountProfile, WebSupportException webSupportException, string action, MFunctionProfile functionProfile, MSecurityInfo securityInfo)
        {
            if (!GWWebHelper.IsWebApiRequest)
            {
                string[] mFunctionsToIgnore = new[] { "MENUS", "LOGOFF", "LOGON", "CHANGEPASSWORD" };
                MFunctionProfile mFunctionProfile = functionProfile; // Byref parameters can not be used in a lambda expression
                string mRedirectPage = String.Empty;

                String mSource = functionProfile.Source;
                if (ConfigSettings.IsAngularJSApplication) {
                    if (mSource.IndexOf("Functions/System/") >= 0)
                    {
                        mSource = mSource.Replace("Functions/System/", "app/growthware/views/");
                        mSource = mSource.Replace(".aspx", ".html");
                    }
                }
                String mFile = HttpContext.Current.Server.MapPath("~/").ToString() + mSource;
                mFile = mFile.Replace("/", @"\"); // not necessary but...
                if (!File.Exists(mFile)) {
                    webSupportException = new WebSupportException(String.Format("Requested resource does not exist {0}", mFile));
                    log.Error(webSupportException);
                    functionProfile = FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_UnknownAction", true));
                    mRedirectPage = GWWebHelper.RootSite + ConfigSettings.AppName + functionProfile.Source;
                    redirect(functionProfile, mRedirectPage);
                }


                if (!mFunctionsToIgnore.Any(functionSource => mFunctionProfile.Source.ToUpper(CultureInfo.InvariantCulture).Contains(functionSource)))
                {
                    mRedirectPage = GWWebHelper.RootSite + ConfigSettings.AppName + mFunctionProfile.Source;
                    switch (accountProfile.Status)
                    {
                        case (int)SystemStatus.ChangePassword:
                            {
                                webSupportException = new WebSupportException("Your password needs to be changed before any other action can be performed.");
                                GWWebHelper.ExceptionError = webSupportException;
                                mFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_ChangePassword", true));
                                mRedirectPage = GWWebHelper.RootSite + ConfigSettings.AppName + mFunctionProfile.Source;
                                redirect(functionProfile, mRedirectPage);
                                break;
                            }

                        case (int)SystemStatus.SetAccountDetails:
                            {
                                mFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditAccount", true));
                                if (action.ToUpper(CultureInfo.InvariantCulture) != mFunctionProfile.Action.ToUpper(CultureInfo.InvariantCulture))
                                {
                                    webSupportException = new WebSupportException("Your account details need to be set.");
                                    GWWebHelper.ExceptionError = webSupportException;
                                    mRedirectPage = GWWebHelper.RootSite + ConfigSettings.AppName + mFunctionProfile.Source;
                                    redirect(functionProfile, mRedirectPage);
                                }

                                break;
                            }

                        default:
                            {
                                if (!securityInfo.MayView)
                                {
                                    if (accountProfile.Account.ToUpper(CultureInfo.InvariantCulture) == "ANONYMOUS")
                                    {
                                        webSupportException = new WebSupportException("Your session has timed out.<br/>Please sign in.");
                                        GWWebHelper.ExceptionError = webSupportException;
                                        mFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_Logon", true));
                                        mRedirectPage = GWWebHelper.RootSite + ConfigSettings.AppName + mFunctionProfile.Source;
                                        redirect(functionProfile, mRedirectPage);
                                    }
                                    mFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_AccessDenied", true));
                                    log.Warn("Access was denied to Account: " + accountProfile.Account + " for Action: " + mFunctionProfile.Action);
                                    mRedirectPage = GWWebHelper.RootSite + ConfigSettings.AppName + mFunctionProfile.Source;
                                    redirect(functionProfile, mRedirectPage);
                                }
                                break;
                            }
                    }
                }
                else
                {
                    log.Debug("Menu data or Logoff/Logon or ChangePassword requested");
                }
            }
        }

        /// <summary>
        /// Provide a relitive URL for use with Server.Transfer
        /// </summary>
        /// <param name="functionProfile"></param>
        /// <returns>string</returns>
        private static string GetRelativeURL(MFunctionProfile functionProfile)
        {
            string mRedirectPage = functionProfile.Source;
            mRedirectPage = mRedirectPage.Replace("Functions/System/", "app/growthware/views/");
            mRedirectPage = mRedirectPage.Replace(".aspx", ".html");
            mRedirectPage += "?Action=" + functionProfile.Action;
            mRedirectPage = "~/" + mRedirectPage;
            return mRedirectPage;
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
                MClientChoicesState mState = (MClientChoicesState)HttpContext.Current.Items[MClientChoices.SessionName];
                //Save ClientChoicesState back to data store
                if (mState != null)
                {
                    if (mState.IsDirty)
                    {
                        ClientChoicesUtility.Save(mState);
                    }
                }
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
                string[] mProcessingTypes = { "ASPX", "ASHX", "ASMX", "HTM", "HTML" };
                if (mProcessingTypes.Contains(mFileExtension) || mPath.IndexOf("/API/", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    mRetVal = true;
                }
            }
            return mRetVal;
        }

        /// <summary>
        /// Performs a redirection
        /// </summary>
        /// <param name="functionProfile">MFunctionProfile</param>
        /// <param name="redirectPage">String</param>
        private static void redirect(MFunctionProfile functionProfile, String redirectPage) {
            if(ConfigSettings.IsAngularJSApplication) {
                redirectPage = GetRelativeURL(functionProfile);
                HttpContext.Current.Server.Transfer(redirectPage, false);
            } else {
                HttpContext.Current.Response.Redirect(redirectPage + "?Action=" + functionProfile.Action);
            }
        }

        private string charMatch(Match match)
        {
            var code = match.Groups["code"].Value;
            int value = Convert.ToInt32(code, 16);
            return ((char)value).ToString();
        }

        //private static void processOverridePage(MFunctionProfile functionProfile) 
        //{
        //    if (HttpContext.Current.Request.Path.ToUpper(CultureInfo.InvariantCulture).IndexOf("/API/", StringComparison.OrdinalIgnoreCase) == -1 && functionProfile != null) 
        //    {
        //        Logger mLog = Logger.Instance();
        //        String mPage = @"/" + ConfigSettings.AppName + functionProfile.Source;
        //        MSecurityEntityProfile mSecProfile = SecurityEntityUtility.CurrentProfile();
        //        String mSkinLocation = "/Public/Skins/" + mSecProfile.Skin + "/";
        //        mPage = mPage.Replace("/", @"\");
        //        String currentExecutionFilePath = HttpContext.Current.Request.CurrentExecutionFilePath;
        //        String mSystemOverridePage = mPage.Replace(@"\System\", @"\Overrides\");
        //        String mSkinOverridePage = mPage.Replace(@"\System\", mSkinLocation);
        //        if (File.Exists(HttpContext.Current.Server.MapPath(mSystemOverridePage))) 
        //        {
        //            mLog.Debug("Transferring to system override page: " + mSystemOverridePage);
        //            HttpContext.Current.Server.Execute(mSystemOverridePage, false);
        //            HttpContext.Current.ApplicationInstance.CompleteRequest();
        //        }
        //        else if (File.Exists(HttpContext.Current.Server.MapPath(mSkinOverridePage)))
        //        {
        //            mLog.Debug("Transferring to skin override override page: " + mSkinOverridePage);
        //            HttpContext.Current.Server.Execute(mSkinOverridePage, false);
        //            HttpContext.Current.ApplicationInstance.CompleteRequest();
        //        }
        //    }
        //}

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

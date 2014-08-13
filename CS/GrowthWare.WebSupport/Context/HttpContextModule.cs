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

namespace GrowthWare.WebSupport.Context
{
    /// <summary>
    /// The HttpContextModule add to context the ClientChoices (A separate HTTP context module)
    /// as well as adding the requested function and current security for the requested function.
    /// </summary>
    /// <remarks></remarks>
    class HttpContextModule : IHttpModule, IRequiresSessionState
    {
        private bool m_Disposing = false;
        private OutputFilterStream m_Filter;
        public void Init(HttpApplication context)
        {
            if (context != null && ConfigSettings.DBStatus.ToUpper(new CultureInfo("en-US", false)) != "INSTALL")
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
        /// Implemetation of dispose
        /// </summary>
        public void Dispose()
        {
            if (!m_Disposing)
            {
                m_Disposing = true;
            }
        }

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
                if (HttpContext.Current.Application["StartLogInfo"].ToString().ToLower(new CultureInfo("en-US", false)) == "false")
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
                if (HttpContext.Current.Application["ClearedCache"].ToString().ToLower(new CultureInfo("en-US", false)) == "false")
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
                // Pre-compile the site
                System.Diagnostics.Process mProcess;
                mLog.Debug("Starting pre-compile process");
                mProcess = System.Diagnostics.Process.Start(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory().ToString() + "aspnet_compiler.exe", "-p " + HttpContext.Current.Server.MapPath("~\\") + " -u");
                mLog.Debug("Finished pre-compile process");
                string mCurrentLevel = ConfigSettings.LogPriority.ToUpper(new CultureInfo("en-US", false));
                mLog.SetThreshold(mLog.GetLogPriorityFromText(mCurrentLevel));
            }
        }

        private void onApplicationError(Object sender, EventArgs e)
        {
            Exception mEx = HttpContext.Current.Server.GetLastError();
            if (mEx != null)
            {
                Logger mLog = Logger.Instance();
                mLog.Error(mEx);
                String mAction = "Unknown";
                if (HttpContext.Current.Request.QueryString["Action"] != null)
                {
                    mAction = HttpContext.Current.Request.QueryString["Action"];
                }
                switch (mEx.Message)
                {
                    case "Cannot redirect after HTTP headers have been sent":
                        return;
                    case "Session state has created a session id, but cannot save it because the response was already flushed by the application.":
                        return;
                }
                mLog.Error(mEx);
                GWWebHelper.ExceptionError = mEx;
                if (mEx.Message.ToUpper().StartsWith("CANNOT OPEN DATABASE", StringComparison.OrdinalIgnoreCase))
                {
                    mLog.Error(mEx);
                    GWWebHelper.ExceptionError = mEx;
                    HttpContext.Current.Server.ClearError();
                    Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                    AppSettingsSection appSettingsSection = config.GetSection("appSettings") as AppSettingsSection;
                    ConfigSettings.SetEnvironmentValue(config, false, "DB_Status", "OffLine", false);
                    HttpContext.Current.Response.Redirect("~/Public/Pages/UnderConstruction.aspx");
                }
                else
                {
                    HttpContext.Current.Server.ClearError();
                    mLog.Error(mEx);
                    HttpContext.Current.Response.Redirect("~/Functions/System/Errors/DisplayError.aspx?ReturnURL=" + mAction);
                }
            }
        }

        private void onAcquireRequestState(Object sender, EventArgs e)
        {
            if (HttpContext.Current.Session == null) return;
            Logger mLog = Logger.Instance();
            mLog.Debug("onAcquireRequestState():: Started");
            mLog.Debug("onAcquireRequestState():: Done");
        }

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
                                throw (new Exception(String.Concat("An AJAX error has occurred: ", Environment.NewLine, mError)));
                            }
                        }
                        else
                        {
                            if (mContext.Response.Headers["jsonerror"].ToString().ToUpperInvariant().Trim() == "TRUE")
                            {
                                mSendError = true;
                                throw (new Exception(String.Concat("An AJAX error has occurred: ", Environment.NewLine)));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (mSendError)
                    {
                        try
                        {
                            if (!ex.ToString().Contains("Invalid JSON primitive"))
                            {
                                Logger mLog = Logger.Instance();
                                mLog.Error(ex);
                            }
                            HttpResponse mCurrentResponse = mContext.Response;
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
                        catch { }
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

        private bool processRequest()
        {
            bool mRetVal = false;
            if (HttpContext.Current != null) 
            {
                string mPath = HttpContext.Current.Request.Path.ToUpper(new CultureInfo("en-US", false));
                string mFileExtension = mPath.Substring(mPath.LastIndexOf(".", StringComparison.OrdinalIgnoreCase) + 1);
                string[] mProcessingTypes = { "ASPX", "ASHX", "ASMX" };
                if (mProcessingTypes.Contains(mFileExtension) || mPath.IndexOf("/API/", StringComparison.OrdinalIgnoreCase) > -1) 
                {
                    mRetVal = true;
                } 
            }
            return mRetVal;
        }
    }
}

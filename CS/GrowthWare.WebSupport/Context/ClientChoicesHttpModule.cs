using System;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Globalization;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport.Utilities;
using GrowthWare.Framework.Common;

namespace GrowthWare.WebSupport.Context
{
    /// <summary>
    /// The ClientChoicesModule ensures that the client choices are avalible 
    /// to the HTTPCONTEXT
    /// </summary>
    /// <remarks></remarks>
    public class ClientChoicesHttpModule : IHttpModule, IRequiresSessionState
    {

        private MSecurityEntityProfile m_SEProfile = SecurityEntityUtility.GetCurrentProfile();
        private bool m_Disposing = false;

        /// <summary>
        /// Initializes the ClientChoicesHttpModule subscribing to HttpModule events.
        /// </summary>
        /// <param name="httpApplication"></param>
        public void Init(HttpApplication httpApplication)
        {
            httpApplication.AcquireRequestState += this.AcquireRequestState;
            httpApplication.EndRequest += this.EndRequest;
        }

        /// <summary>
        /// Implements dispose required by IHttpModule
        /// </summary>
        public void Dispose()
        {
            if (!m_Disposing)
            {
                m_Disposing = true;
                m_SEProfile = null;
            }
        }

        /// <summary>
        /// Keeps the MClientChoicesState in context.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        public void AcquireRequestState(object sender, EventArgs e)
        {
            if (ConfigSettings.DBStatus.ToUpper() != ConfigSettings.DBStatus.ToUpper()) return;
            if (!processRequest()) return;
            if (HttpContext.Current.Session == null) return;
            String mAccountName = AccountUtility.GetHttpContextUserName();
            MClientChoicesState mClientChoicesState = null;
            mClientChoicesState = ClientChoicesUtility.GetClientChoicesState(mAccountName);
            // Add ClientChoicesState object to the context items for use
            // throughout the application.
            HttpContext.Current.Items[MClientChoices.SessionName] = mClientChoicesState;
        }

        /// <summary>
        /// Saves changes to MClientChoicesState to the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void EndRequest(object sender, EventArgs e)
        {
            if (!processRequest()) return;
            MClientChoicesState mState = (MClientChoicesState)HttpContext.Current.Items[MClientChoices.SessionName];
            //Save ClientChoicesState back to data store
            if (mState != null)
            {
                if (mState.IsDirty)
                {
                    ClientChoicesUtility.Save(mState);
                }
            }
        }

        /// <summary>
        /// Determines if the request if one of "ASPX", "ASCX", "ASHX", OR "ASMX"
        /// </summary>
        /// <returns>boolean</returns>
        /// <remarks>There's no need to process logic for the other file types or extension</remarks>
        private bool processRequest()
        {
            bool mRetVal = false;
            string mPath = HttpContext.Current.Request.Path.ToUpper(new CultureInfo("en-US", false));
            string mFileExtension = mPath.Substring(mPath.LastIndexOf(".") + 1);
            string[] mProcessingTypes = { "ASPX", "ASCX", "ASHX", "ASMX" };
            if (mProcessingTypes.Contains(mFileExtension)) mRetVal = true;
            return mRetVal;
        }
    }
}

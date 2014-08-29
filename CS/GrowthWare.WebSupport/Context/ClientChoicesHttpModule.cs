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

        private bool m_Disposing = false;

        /// <summary>
        /// Initializes the ClientChoicesHttpModule subscribing to HttpModule events.
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            if (context == null) throw new ArgumentNullException("context", "context can not be null (Nothing in VB)!");
            context.AcquireRequestState += this.AcquireRequestState;
            context.EndRequest += this.EndRequest;
        }

        /// <summary>
        /// Implements dispose required by IHttpModule
        /// </summary>
        public void Dispose()
        {
            if (!m_Disposing)
            {
                m_Disposing = true;
            }
        }

        /// <summary>
        /// Keeps the MClientChoicesState in context.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="eventArgs">EventArgs</param>
        public void AcquireRequestState(object sender, EventArgs eventArgs)
        {
            if (ConfigSettings.DBStatus.ToUpper(CultureInfo.InvariantCulture) != ConfigSettings.DBStatus.ToUpper(CultureInfo.InvariantCulture)) return;
            if (!processRequest()) return;
            if (HttpContext.Current.Session == null) return;
            String mAccountName = AccountUtility.HttpContextUserName();
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
        /// <param name="eventArgs"></param>
        public void EndRequest(object sender, EventArgs eventArgs)
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
        private static bool processRequest()
        {
            bool mRetVal = false;
            if (HttpContext.Current != null) 
            {
                string mPath = HttpContext.Current.Request.Path.ToUpper(CultureInfo.InvariantCulture);
                string mFileExtension = mPath.Substring(mPath.LastIndexOf(".", StringComparison.OrdinalIgnoreCase) + 1);
                string[] mProcessingTypes = { "ASPX", "ASCX", "ASHX", "ASMX" };
                if (mProcessingTypes.Contains(mFileExtension) || mPath.IndexOf("/API/", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    mRetVal = true;
                } 
            }
            return mRetVal;
        }
    }
}

using System;
using System.Web;
using System.Web.SessionState;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.Framework.Web.Utilities;

namespace GrowthWare.Framework.Web.Context
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
			if(WebConfigSettings.DBStatus.ToUpper() != GWWebHelper.DBStatusOnline.ToUpper()) return;
			if(HttpContext.Current.Request.Path.ToUpper().IndexOf(".ASPX") == -1) return;
			if(HttpContext.Current.Session == null) return;
			String mAccountName = "Anonymous";
			if(HttpContext.Current.Request.IsAuthenticated)
			{
				mAccountName = HttpContext.Current.User.Identity.Name;
			}

			MClientChoicesState mClientChoicesState = null;
			ClientChoicesUtility mClientChoicesUtility = new ClientChoicesUtility();
			mClientChoicesState = mClientChoicesUtility.GetClientChoicesState(mAccountName);
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
			if (HttpContext.Current.Request.Path.ToUpper().IndexOf(".ASPX") == -1) return;
			ClientChoicesUtility mClientChoicesUtility = new ClientChoicesUtility();
			MClientChoicesState mState = (MClientChoicesState)HttpContext.Current.Items[MClientChoices.SessionName];
			//Save ClientChoicesState back to data store
			if (mState != null) 
			{
				if (mState.isDirty) 
				{
					mClientChoicesUtility.Save(ref mState);
				}			
			}
		}
	}

}

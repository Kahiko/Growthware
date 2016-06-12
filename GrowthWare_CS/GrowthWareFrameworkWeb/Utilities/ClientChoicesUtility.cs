using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrowthWare.Framework.BusinessLogicLayer;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.Framework.Web.Controllers;
using System.Web;
using System.Globalization;

namespace GrowthWare.Framework.Web.Utilities
{
	/// <summary>
	/// ClientChoicesUtility serves as the focal point for any web application needing to utiltize the GrowthWare framework
	/// with regards to ClientChoices.
	/// </summary>
	public class ClientChoicesUtility
	{
		private MSecurityEntityProfile m_SecurityEntityProfile = null;
		private BClientChoices m_BClientChoices = null;
		private String m_CachedAnonymousChoicesState = "AnonymousClientChoicesState";

		/// <summary>
		/// Constructor sets up the private fields
		/// </summary>
		public ClientChoicesUtility() 
		{ 
			m_SecurityEntityProfile = SecurityEntityUtility.GetDefaultProfile();
			m_BClientChoices = new BClientChoices(ref m_SecurityEntityProfile, WebConfigSettings.CentralManagement);
		}

		/// <summary>
		/// Returns the client choices given the account
		/// </summary>
		/// <param name="account">String</param>
		/// <returns>MClientChoicesState</returns>
		public MClientChoicesState GetClientChoicesState(String account) 
		{ 
			MClientChoicesState mRetVal = null;
			if(HttpContext.Current.Session != null)
			{
				mRetVal = (MClientChoicesState)(HttpContext.Current.Session[MClientChoices.SessionName]);
			}
			if(mRetVal == null)
			{
				if (account.Trim().ToLower(CultureInfo.CurrentCulture) == "anonymous")
				{
					mRetVal = (MClientChoicesState)HttpContext.Current.Cache[m_CachedAnonymousChoicesState];
					if (mRetVal == null)
					{
						mRetVal = m_BClientChoices.GetClientChoicesState(account);
						CacheController.AddToCacheDependency(m_CachedAnonymousChoicesState, mRetVal);
					}
				}
				else 
				{
					if (mRetVal.AccountName != account)
					{
						mRetVal = m_BClientChoices.GetClientChoicesState(account);
					}
				}
			}
			if(HttpContext.Current.Session != null)
			{
				HttpContext.Current.Session.Add(MClientChoices.SessionName, mRetVal);
			}
			return mRetVal;
		}

		/// <summary>
		/// Save the client choices to the database.
		/// </summary>
		/// <param name="clientChoicesState">MClientChoicesState</param>
		/// <remarks></remarks>
		public void Save(ref MClientChoicesState clientChoicesState) 
		{ 
			m_BClientChoices.Save(ref clientChoicesState);
			if(HttpContext.Current.Session != null)
			{
				HttpContext.Current.Items[MClientChoices.SessionName] = clientChoicesState;
			}
		}
	}
}

using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Globalization;
using GrowthWare.Framework.BusinessLogicLayer;
using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Enumerations;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.Framework.Web.Controllers;

namespace GrowthWare.Framework.Web.Utilities
{
	/// <summary>
	/// AccountUtility serves as the focal point for any web application needing to utiltize the GrowthWare framework.
	/// Web needs such as caching are handeled here
	/// </summary>
	public class AccountUtility
	{
		//private String m_SecutiryEntityCacheKey = "SecurityEntity_XX_Account";
		private MSecurityEntityProfile m_SecurityEntityProfile = null;
		private BAccounts m_BAccount = null;
		private String m_CachedAnonymousAccount = "AnonymousProfile";
		private String m_AnonymousAccount = "Anonymous";
		private String m_SessionName = "_Session";

		/// <summary>
		/// Constructor sets up the private fields
		/// </summary>
		public AccountUtility()
		{
			m_SecurityEntityProfile = SecurityEntityUtility.GetCurrentProfile();
			m_BAccount = new BAccounts(m_SecurityEntityProfile, ConfigSettings.CentralManagement);
		}

		/// <summary>
		/// Retruns a collection of MAccountProfiles given an MAccountProfile and the current SecurityEntitySeqID
		/// </summary>
		/// <param name="profile">MAccountProfile</param>
		/// <remarks>If the Profiles.IsSysAdmin is true then all accounts will be returned</remarks>
		public Collection<MAccountProfile> GetAccounts(MAccountProfile profile)
		{
			// was thinking of adding cache here but
			// when you think of it it's not needed
			// account information needs to come from
			// the db to help ensure passwords are correct and what not.
			// besides which a list of accounts is only necessary
			// when editing an account and it that case
			// what accounts that are returned are dependend on the requesting account.IsSysAdmin bit.
			return m_BAccount.GetAccounts(profile);
		}

		/// <summary>
		/// Retrieves the current profile.
		/// </summary>
		/// <returns>MAccountProfile</returns>
		/// <remarks>If context does not contain a referance to an account anonymous will be returned</remarks>
		public MAccountProfile GetCurrentProfile()
		{
			MAccountProfile mRetProfile = null;
			String mAccountName = HttpContext.Current.User.Identity.Name;
			if(mAccountName == String.Empty) mAccountName = m_AnonymousAccount;
			if(mAccountName.Trim() == m_AnonymousAccount)
			{
				if(HttpContext.Current.Cache != null)
				{
					mRetProfile = (MAccountProfile)(HttpContext.Current.Cache[m_CachedAnonymousAccount]);
					if(mRetProfile == null)
					{
						mRetProfile = this.GetProfile(mAccountName);
						CacheController.AddToCacheDependency(m_CachedAnonymousAccount, mRetProfile);
					}
				}
			}
			if(mRetProfile == null)
			{
				mRetProfile = (MAccountProfile)(HttpContext.Current.Session[mAccountName]);
				if(mRetProfile == null)
				{
					mRetProfile = this.GetProfile(mAccountName);
					HttpContext.Current.Session.Add(mAccountName + m_SessionName, mRetProfile);
				}
			}
			return mRetProfile;
		}

		/// <summary>
		/// Retrieves an account profile given the account
		/// </summary>
		/// <param name="account">String</param>
		/// <returns>a populated MAccountProfile</returns>
		public MAccountProfile GetProfile(String account)
		{
			return m_BAccount.GetAccountProfile(account);
		}

		/// <summary>
		/// Inserts or updates account information
		/// </summary>
		/// <param name="profile">MAccountProfile</param>
		/// <param name="saveRoles">Boolean</param>
		/// <param name="saveGroups">Boolean</param>
		/// <remarks>Changes will be reflected in the profile passed as a reference.</remarks>
		public MAccountProfile Save(MAccountProfile profile, bool saveRoles, bool saveGroups)
		{
			m_BAccount.Save(ref profile, saveRoles, saveGroups);
			return profile;
		}
	}

}

using System.Data;
using GrowthWare.Framework.Model.Profiles;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GrowthWare.Framework.BusinessLogicLayer.Interfaces
{
	/// <summary>
	/// Provide the contract for the accounts business logic object.
	/// </summary>
	public interface IBAccounts
	{

		/// <summary>
		/// Retrieves a generic collection of MAccountProfiles
		/// </summary>
		/// <param name="Profile">MAccountProfile</param>
		/// <returns>A generic collection of MAccountProfiles</returns>
		/// <remarks>
		/// The IsSystemAdmin propery and the Account property are used from the profile object. 
		/// If isSysAdmin then all accounts are returned.
		/// If not isSysAdmin only accounts with an associated role to the current security entity will be retruned.
		/// </remarks>
		Collection<MAccountProfile> GetAccounts(MAccountProfile Profile);

		/// <summary>
		/// Returns a populated MAccountProfile
		/// </summary>
		/// <param name="Account">String</param>
		/// <returns>MAccountProfile</returns>
		MAccountProfile GetAccountProfile(string Account);

		/// <summary>
		/// Deletes a account record and all of the associated roles and groups.
		/// </summary>
		/// <param name="AccountID">int</param>
		void Delete(int AccountID);

		/// <summary>
		/// Saves the account profile information to the datastore.
		/// </summary>
		/// <param name="Profile">MAccountProfile</param>
		/// <param name="SaveRoles">Boolean</param>
		/// <param name="SaveGroups">Boolean</param>
		/// <remarks>The profile should be updated as necessary from the business logic layer.</remarks>
		void Save(ref MAccountProfile Profile, bool SaveRoles, bool SaveGroups);
	}
}

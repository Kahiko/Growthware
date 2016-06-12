using GrowthWare.Framework.DataAccessLayer.Interfaces.Base;
using GrowthWare.Framework.Model.Profiles;
using System.Data;

namespace GrowthWare.Framework.DataAccessLayer.Interfaces
{
	interface IDSecurityEntity : IDDBInteraction
	{
		/// <summary>
		/// Retrieves all Security Entities as a data table.
		/// </summary>
		/// <returns>DataTable</returns>
		/// <remarks></remarks>
		DataTable GetSecurityEntities();

		/// <summary>
		/// Retrieves security entities for a given account.
		/// </summary>
		/// <param name="account">String</param>
		/// <param name="securityEntityID">int or Integer</param>
		/// <param name="isSecurityEntityAdministrator">Boolean or bool</param>
		/// <returns>Datatable</returns>
		/// <remarks></remarks>
		DataTable GetSecurityEntities(string account, int securityEntityID, bool isSecurityEntityAdministrator);

		/// <summary>
		/// Saves security entity information to the datastore.
		/// </summary>
		/// <param name="profile">MSecurityEntityProfile</param>
		/// <remarks></remarks>
		int Save(ref MSecurityEntityProfile profile);
	}
}

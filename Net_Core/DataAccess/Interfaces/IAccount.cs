using GrowthWare.DataAccess.Interfaces.Base;
using GrowthWare.Framework.Enumerations;
using GrowthWare.Framework.Models;
using System.Data;
using System.Threading.Tasks;

namespace GrowthWare.DataAccess.Interfaces;

public interface IAccount : IDBInteraction
{
	/// <summary>
	/// Used by all methods and must be set to send parameters to the datastore
	/// </summary>
	MAccountProfile Profile { get; set; }

	/// <summary>
	/// Check the DB to see if the refreshToken is already in use (exists)
	/// </summary>
	/// <param name="refreshToken"></param>
	/// <returns>bool</returns>
	Task<bool> RefreshTokenExists(string refreshToken);

	/// <summary>
	/// Check the DB to see if the resetToken is already in use (exists)
	/// </summary>
	/// <param name="resetToken">string to check for</param>
	/// <returns>bool</returns>
	Task<bool> ResetTokenExists(string resetToken);

	/// <summary>
	/// Used by all methods and must be set to send parameters to the datastore
	/// </summary>
	int SecurityEntitySeqId { get; set; }

	/// <summary>
	/// Deletes an account
	/// </summary>
	Task Delete();

	/// <summary>
	/// Retrieves Account information via stored procedure ZGWSecurity.Get_Account
	/// </summary>
	/// <returns>
	/// Dataset:
	/// 	1. Table 0, row 0 - Account Details,
	/// 	2. Table 1 - Refresh Tokens,
	/// 	3. Table 2 - Assigned Roles,
	/// 	4. Table 3 - Assigned Groups,
	/// 	5. Table 4 - Derived Roles
	/// </returns>
	Task<DataSet> GetAccount();

	/// <summary>
	/// Retrieves Account information given the reset token
	/// </summary>
	Task<DataSet> GetAccountByResetToken();

	/// <summary>
	/// Retrieves Account information given the verification token
	/// </summary>
	Task<DataSet> GetAccountByVerificationToken();

	/// <summary>
	/// Retrieves Account information given the JWT
	/// </summary>
	/// <returns>DataRow</returns>
	Task<DataRow> GetAccountByRefreshToken();

	Task<DataTable> GetAccounts();

	/// <summary>
	/// Retrieves menu data for a given account and MenuType
	/// </summary>
	/// <param name="account">String</param>
	/// <param name="menuType">MenuType</param>
	/// <param name="securityEntitySeqId">int</param>
	/// <returns>DataTable</returns>
	/// <remarks></remarks>
	Task<DataTable> GetMenu(string account, MenuType menuType, int securityEntitySeqId);

	/// <summary>
	/// Returns all roles either direct association or by association via
	/// groups.
	/// </summary>
	/// <returns>DataTable</returns>
	Task<DataTable> Security();

	/// <summary>
	/// Inserts or updates account information
	/// </summary>
	/// <returns>int</returns>
	Task<int> Save();

	/// <summary>
	/// Save groups by passing a string or comma separated groups to the database.
	/// </summary>
	Task SaveGroups();

	/// <summary>
	/// Save refresh tokens by passing an array of IDatabaseFunctions objects to the database.
	/// </summary>
	Task SaveRefreshTokens();

	/// <summary>
	/// Save roles by passing a string or comma separated roles to the database.
	/// </summary>
	Task SaveRoles();

	Task<bool> VerificationTokenExists(string token);
}
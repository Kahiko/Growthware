using GrowthWare.DataAccess.Interfaces.Base;
using GrowthWare.Framework.Models;
using System.Data;
using System.Threading.Tasks;

namespace GrowthWare.DataAccess.Interfaces;

public interface IRoles : IDBInteraction
{
    /// <summary>
    /// Gets or sets the security entity seq ID.
    /// </summary>
    /// <value>The security entity seq ID.</value>
    int SecurityEntitySeqID { get; set; }

    /// <summary>
    /// Gets or sets the profile.
    /// </summary>
    /// <value>The profile.</value>
    MRole Profile { get; set; }

    /// <summary>
    /// Gets the accounts in role.
    /// </summary>
    /// <returns>DataTable</returns>
    Task<DataTable> AccountsInRole();

    /// <summary>
    /// Gets the accounts not in role.
    /// </summary>
    /// <returns>DataTable.</returns>
    Task<DataTable> AccountsNotInRole();

    /// <summary>
    /// Updates all accounts for role.
    /// </summary>
    /// <param name="roleSeqId">The role seq ID.</param>
    /// <param name="securityEntityId">The security entity ID.</param>
    /// <param name="accounts">The accounts.</param>
    /// <param name="accountSeqId">The account seq ID.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
    Task<bool> UpdateAllAccountsForRole(int roleSeqId, int securityEntityId, string[] accounts, int accountSeqId);

    /// <summary>
    /// Saves this instance.
    /// </summary>
    Task<int> Save(MRole profile);

    /// <summary>
    /// Deletes the role.
    /// </summary>
    Task DeleteRole(string roleName, int securityEntitySeqId);

    /// <summary>
    /// Gets the profile data.
    /// </summary>
    /// <returns>DataRow.</returns>
    Task<DataRow> ProfileData(int roleSeqId);

    /// <summary>
    /// Gets the roles by BU.
    /// </summary>
    /// <returns>DataTable.</returns>
    Task<DataTable> RolesBySecurityEntity(int securityEntitySeqId);
}
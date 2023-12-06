using GrowthWare.DataAccess.Interfaces.Base;
using GrowthWare.Framework.Models;
using System.Data;

namespace GrowthWare.DataAccess.Interfaces
{
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
        /// <returns>DataTable.</returns>
        DataTable AccountsInRole();

        /// <summary>
        /// Gets the accounts not in role.
        /// </summary>
        /// <returns>DataTable.</returns>
        DataTable AccountsNotInRole();

        /// <summary>
        /// Updates all accounts for role.
        /// </summary>
        /// <param name="RoleSeqID">The role seq ID.</param>
        /// <param name="SecurityEntityID">The security entity ID.</param>
        /// <param name="Accounts">The accounts.</param>
        /// <param name="AccountSeqID">The account seq ID.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        bool UpdateAllAccountsForRole(int RoleSeqID, int SecurityEntityID, string[] Accounts, int AccountSeqID);

        /// <summary>
        /// Saves this instance.
        /// </summary>
        int Save();

        /// <summary>
        /// Deletes the role.
        /// </summary>
        void DeleteRole();

        /// <summary>
        /// Gets the profile data.
        /// </summary>
        /// <returns>DataRow.</returns>
        DataRow ProfileData();

        /// <summary>
        /// Gets the roles by BU.
        /// </summary>
        /// <returns>DataTable.</returns>
        DataTable RolesBySecurityEntity();
    }
}

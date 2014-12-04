using GrowthWare.Framework.BusinessData.BusinessLogicLayer;
using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Web;

namespace GrowthWare.WebSupport.Utilities
{
    /// <summary>
    /// Class RoleUtility
    /// </summary>
    public static class RoleUtility
    {

        /// <summary>
        /// Securities the name of the entities roles cache.
        /// </summary>
        /// <param name="securityEntitySeqId">The SECURIT y_ ENTIT y_ SE q_ ID.</param>
        /// <returns>System.String.</returns>
        public static string SecurityEntitiesRolesCacheName(int securityEntitySeqId)
        {
            string retVal = "SecurityEntityRoles" + securityEntitySeqId.ToString(CultureInfo.InvariantCulture);
            return retVal;
        }

        /// <summary>
        /// Gets the profile.
        /// </summary>
        /// <param name="roleId">The role ID.</param>
        /// <returns>MRoleProfile.</returns>
        public static MRoleProfile GetProfile(int roleId)
        {
            MRoleProfile myProfile = new MRoleProfile();
            myProfile.Id = roleId;
            BRoles myBRoles = new BRoles(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            myBRoles.GetProfile(myProfile);
            return myProfile;
        }

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public static void DeleteRole(MRoleProfile profile)
        {
            if(profile == null)  throw new ArgumentNullException("profile", "profile cannot be blank or a null reference (Nothing in Visual Basic)");
            BRoles myBRoles = new BRoles(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            myBRoles.DeleteRole(profile);
            RemoveRoleCache(profile.SecurityEntityId);
            FunctionUtility.RemoveCachedFunctions();
        }

        /// <summary>
        /// Gets the accounts in role.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns>ArrayList.</returns>
        public static ArrayList GetAccountsInRole(MRoleProfile profile)
        {
            if (profile == null) throw new ArgumentNullException("profile", "profile cannot be blank or a null reference (Nothing in Visual Basic)");
            ArrayList colAccounts = new ArrayList();
            DataRow accountsRow = null;
            BRoles myBRoles = new BRoles(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            DataTable myDataTable = myBRoles.GetAccountsInRole(profile);
            foreach (DataRow accountsRow_loopVariable in myDataTable.Rows)
            {
                accountsRow = accountsRow_loopVariable;
                colAccounts.Add((string)accountsRow["ACCT"]);
            }
            return colAccounts;
        }

        /// <summary>
        /// Gets the accounts not in role.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns>ArrayList.</returns>
        public static ArrayList GetAccountsNotInRole(MRoleProfile profile)
        {
            if (profile == null) throw new ArgumentNullException("profile", "profile cannot be blank or a null reference (Nothing in Visual Basic)");
            ArrayList colAccounts = new ArrayList();
            DataRow accountsRow = null;
            BRoles myBRoles = new BRoles(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            DataTable myDataTable = myBRoles.GetAccountsNotInRole(profile);
            foreach (DataRow accountsRow_loopVariable in myDataTable.Rows)
            {
                accountsRow = accountsRow_loopVariable;
                colAccounts.Add((string)accountsRow["ACCT"]);
            }
            return colAccounts;
        }

        /// <summary>
        /// Gets all roles by BU.
        /// </summary>
        /// <param name="securityEntitySeqId">The SECURIT y_ ENTIT y_ SE q_ ID.</param>
        /// <returns>DataTable.</returns>
        public static DataTable GetAllRolesBySecurityEntity(int securityEntitySeqId)
        {
            DataTable mySecurityEntityRoles = null;
            // attempt to retrieve the information from cache
            mySecurityEntityRoles = (DataTable)HttpContext.Current.Cache[SecurityEntitiesRolesCacheName(securityEntitySeqId)];
            // if the information was not avalible in cache
            // then retieve the information from the DB and put it into
            // cache for subsequent use.
            if (mySecurityEntityRoles == null)
            {
                BRoles myBRoles = new BRoles(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
                mySecurityEntityRoles = myBRoles.GetRolesBySecurityEntity(securityEntitySeqId);
                CacheController.AddToCacheDependency(SecurityEntitiesRolesCacheName(securityEntitySeqId), mySecurityEntityRoles);
            }
            return mySecurityEntityRoles;
        }

        /// <summary>
        /// Gets the roles array list by BU.
        /// </summary>
        /// <param name="securityEntitySeqId">The SECURIT y_ ENTIT y_ SE q_ ID.</param>
        /// <returns>ArrayList.</returns>
        public static ArrayList GetRolesArrayListBySecurityEntity(int securityEntitySeqId)
        {
            DataTable mySecurityEntityRoles = null;
            ArrayList colRoles = new ArrayList();
            mySecurityEntityRoles = GetAllRolesBySecurityEntity(securityEntitySeqId);
            foreach (DataRow roleRow in mySecurityEntityRoles.Rows)
            {
                colRoles.Add((string)roleRow["NAME"]);
            }
            return colRoles;
        }

        /// <summary>
        /// Removes the role cache.
        /// </summary>
        /// <param name="securityEntitySeqId">The security entity Seq ID.</param>
        public static void RemoveRoleCache(int securityEntitySeqId)
        {
            CacheController.RemoveFromCache(SecurityEntitiesRolesCacheName(securityEntitySeqId));
        }

        /// <summary>
        /// Saves the specified profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public static void Save(MRoleProfile profile)
        {
            if (profile == null) throw new ArgumentNullException("profile", "profile cannot be blank or a null reference (Nothing in Visual Basic)");
            BRoles myBRoles = new BRoles(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            myBRoles.Save(profile);
            RemoveRoleCache(profile.SecurityEntityId);
            FunctionUtility.RemoveCachedFunctions();
        }

        /// <summary>
        /// Searches the specified search critera.
        /// </summary>
        /// <param name="searchCriteria">The search critera.</param>
        /// <returns>DataTable.</returns>
        public static DataTable Search(MSearchCriteria searchCriteria)
        {
            if (searchCriteria == null) throw new ArgumentNullException("searchCriteria", "searchCriteria cannot be blank or a null reference (Nothing in Visual Basic)");
            BRoles mBRoles = new BRoles(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            return mBRoles.Search(searchCriteria);
        }

        /// <summary>
        /// Updates all accounts for role.
        /// </summary>
        /// <param name="roleId">The role ID.</param>
        /// <param name="securityEntitySeqId">The security entity Seq ID.</param>
        /// <param name="accounts">The accounts.</param>
        /// <param name="accountId">The account ID.</param>
        /// <returns><c>true</c> if no errors, <c>true</c> otherwise false</returns>
        public static bool UpdateAllAccountsForRole(int roleId, int securityEntitySeqId, string[] accounts, int accountId)
        {
            if (accounts == null) throw new ArgumentNullException("accounts", "accounts cannot be blank or a null reference (Nothing in Visual Basic)");
            bool success = false;
            BRoles myBRoles = new BRoles(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            success = myBRoles.UpdateAllAccountsForRole(roleId, securityEntitySeqId, accounts, accountId);
            return success;
        }

    }
}

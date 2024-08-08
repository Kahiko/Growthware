using System.Collections;
using System.Data;
using System.Runtime.CompilerServices;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.Web.Support.Helpers;

namespace GrowthWare.Web.Support.Utilities;

public static class RoleUtility
{

    private static BRoles m_BusinessLogic = null;
    private static CacheHelper m_CacheHelper = CacheHelper.Instance();

    /// <summary>
    /// Retrieves an array of strings representing the accounts in the specified role.
    /// </summary>
    /// <param name="profile">The role profile to retrieve accounts from.</param>
    /// <returns>An array of strings representing the accounts in the specified role.</returns>
    private static string[] getAccountsInRole(MRole profile)
    {
        // intended to be used when editing so no cache is needed
        DataTable mDataTable = getBusinessLogic().GetAccountsInRole(profile);
        string[] mRetVal = getStrings(mDataTable);
        return mRetVal;
    }

    /// <summary>
    /// Retrieves an array of strings representing the accounts not in the specified role.
    /// </summary>
    /// <param name="profile">The role to check against.</param>
    /// <returns>An array of strings representing the accounts not in the specified role.</returns>
    private static string[] getAccountsNotInRole(MRole profile)
    {
        // intended to be used when editing so no cache is needed
        DataTable mDataTable = getBusinessLogic().GetAccountsNotInRole(profile);
        string[] mRetVal = getStrings(mDataTable);
        return mRetVal;
    }

    /// <summary>
    /// Retrieves an array of strings from a given DataTable.
    /// </summary>
    /// <param name="mDataTable">The DataTable to retrieve strings from.</param>
    /// <returns>An array of strings retrieved from the DataTable.</returns>
    private static string[] getStrings(DataTable mDataTable)
    {
        string[] mRetVal = new string[] { };
        ArrayList mArrayList = new ArrayList();
        if (mDataTable != null)
        {
            foreach (DataRow mDataRow in mDataTable.Rows)
            {
                if (mDataRow["ACCT"] != null)
                {
                    mArrayList.Add(mDataRow["ACCT"].ToString());
                }
            }
        }
        mRetVal = (string[])mArrayList.ToArray(typeof(string));
        return mRetVal;
    }

    /// <summary>
    /// Retrieves all roles for a given security entity.
    /// </summary>
    /// <param name="securityEntityId">The ID of the security entity.</param>
    /// <returns>A DataTable containing the roles.</returns>
    static DataTable GetAllRolesBySecurityEntity(int securityEntityId)
    {
        DataTable mRoles = m_CacheHelper.GetFromCache<DataTable>(securityEntityId.ToString() + "_Roles");
        if (mRoles == null)
        {
            mRoles = getBusinessLogic().GetRolesBySecurityEntity(securityEntityId);
            m_CacheHelper.AddToCache(securityEntityId.ToString() + "_Roles", mRoles);
        }
        return mRoles;
    }

    /// <summary>
    /// Returns the business logic object used to access the database.
    /// </summary>
    /// <returns></returns>
    private static BRoles getBusinessLogic()
    {
        if(m_BusinessLogic == null || ConfigSettings.CentralManagement == true)
        {
            m_BusinessLogic = new(SecurityEntityUtility.CurrentProfile);
        }
        return m_BusinessLogic;
    }

    /// <summary>
    /// Deletes a role from the database.
    /// </summary>
    /// <param name="roleSeqId">The sequence ID of the role to delete.</param>
    /// <param name="securityEntitySeqId">The sequence ID of the security entity.</param>
    /// <returns>A boolean indicating whether the role was successfully deleted.</returns>
    public static bool DeleteRole(int roleSeqId, int securityEntitySeqId)
    {
        UIRole mRoleFromDB = GetUIProfile(roleSeqId, securityEntitySeqId);
        MRole mRoleToDelete = new MRole(mRoleFromDB);
        mRoleToDelete.Id = roleSeqId;
        mRoleToDelete.SecurityEntityID = securityEntitySeqId;
        getBusinessLogic().DeleteRole(mRoleToDelete);
        m_CacheHelper.RemoveFromCache(securityEntitySeqId.ToString() + "_Roles");
        return true;
    }

    /// <summary>
    /// Retrieves an ArrayList of roles by the given security entity ID.
    /// </summary>
    /// <param name="securityEntityId">The ID of the security entity.</param>
    /// <returns>An ArrayList of role names.</returns>
    public static ArrayList GetRolesArrayListBySecurityEntity(int securityEntityId)
    {
        DataTable mGroupsTable = GetAllRolesBySecurityEntity(securityEntityId);
        ArrayList mRetVal = new ArrayList();
        foreach (DataRow item in mGroupsTable.Rows)
        {
            mRetVal.Add((string)item["NAME"]);
        }
        return mRetVal;
    }

    /// <summary>
    /// Retrieves a UIRole object based on the provided role sequence ID and security entity sequence ID.
    /// </summary>
    /// <param name="roleSeqId">The sequence ID of the role.</param>
    /// <param name="securityEntitySeqId">The sequence ID of the security entity.</param>
    /// <returns>A UIRole object containing the role profile, accounts in role, and accounts not in role.</returns>
    public static UIRole GetUIProfile(int roleSeqId, int securityEntitySeqId)
    {
        MRole mRoleProfile = new MRole();
        mRoleProfile.Id = roleSeqId;
        mRoleProfile.SecurityEntityID = securityEntitySeqId;
        mRoleProfile = getBusinessLogic().GetProfile(mRoleProfile);
        UIRole mRetVal = new UIRole(mRoleProfile);
        mRetVal.AccountsInRole = getAccountsInRole(mRoleProfile);
        mRetVal.AccountsNotInRole = getAccountsNotInRole(mRoleProfile);
        return mRetVal;
    }

    /// <summary>
    /// Saves a role profile and updates all accounts for the role.
    /// </summary>
    /// <param name="roleProfile">The role profile to save.</param>
    /// <param name="accountsInRole">The accounts in the role.</param>
    /// <returns>The saved UIRole object.</returns>
    public static UIRole Save(MRole roleProfile, string[] accountsInRole)
    {
        MRole mRoleToSave = new MRole(roleProfile);
        BRoles mBRoles = getBusinessLogic();
        if (roleProfile.Id > -1)
        {
            MRole mProfileFromDB = mBRoles.GetProfile(mRoleToSave);
            mRoleToSave.AddedBy = mProfileFromDB.AddedBy;
            mRoleToSave.AddedDate = mProfileFromDB.AddedDate;
        }
        mRoleToSave.Id = getBusinessLogic().Save(mRoleToSave);
        UpdateAllAccountsForRole(mRoleToSave.Id, mRoleToSave.SecurityEntityID, accountsInRole, mRoleToSave.UpdatedBy);
        m_CacheHelper.RemoveFromCache(mRoleToSave.SecurityEntityID.ToString() + "_Roles");
        return new UIRole(mRoleToSave);
    }

    /// <summary>
    /// Updates all accounts for a given role.
    /// </summary>
    /// <param name="roleId">The ID of the role.</param>
    /// <param name="securityEntitySeqId">The security entity sequence ID.</param>
    /// <param name="accounts">An array of accounts.</param>
    /// <param name="accountId">The account ID.</param>
    /// <returns>True if the update was successful, false otherwise.</returns>
    public static bool UpdateAllAccountsForRole(int roleId, int securityEntitySeqId, string[] accounts, int accountId)
    {
        return getBusinessLogic().UpdateAllAccountsForRole(roleId, securityEntitySeqId, accounts, accountId);
    }
}
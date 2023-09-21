using System.Collections;
using System.Data;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GrowthWare.Web.Support.Utilities;

public static class RoleUtility
{
    static BRoles m_BRoles = new BRoles(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);

    private static string[] getAccountsInRole(MRole profile) 
    {
        DataTable mDataTable = m_BRoles.GetAccountsInRole(profile);
        string[] mRetVal = getStrings(mDataTable);
        return mRetVal;
    }

    private static string[] getAccountsNotInRole(MRole profile)
    {
        DataTable mDataTable = m_BRoles.GetAccountsNotInRole(profile);
        string[] mRetVal = getStrings(mDataTable);
        return mRetVal;
    }

    private static string[] getStrings(DataTable mDataTable)
    {
        string[] mRetVal = new string[] { };
        ArrayList mArrayList = new ArrayList();
        if(mDataTable != null)
        {
            foreach (DataRow mDataRow in mDataTable.Rows)
            {
                if(mDataRow["ACCT"] != null)
                {
                    mArrayList.Add(mDataRow["ACCT"].ToString());                    
                }
            }
        }
        mRetVal = (string[])mArrayList.ToArray(typeof(string));
        return mRetVal;
    }

    static DataTable GetAllRolesBySecurityEntity(int securityEntityId)
    {
        // TODO: Add cache
        BRoles mBRoles = new BRoles(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
        DataTable mRoles = mBRoles.GetRolesBySecurityEntity(securityEntityId);
        return mRoles;
    }

    public static bool DeleteRole(int roleSeqId, int securityEntitySeqId)
    {
        UIRole mRoleFromDB = GetUIProfile(roleSeqId, securityEntitySeqId);
        MRole mRoleToDelete = new MRole(mRoleFromDB);
        mRoleToDelete.Id = roleSeqId;
        mRoleToDelete.SecurityEntityID = securityEntitySeqId;
        m_BRoles.DeleteRole(mRoleToDelete);
        return true;
    }

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

    public static UIRole GetUIProfile(int roleSeqId, int securityEntitySeqId)
    {
        MRole mRoleProfile = new MRole();
        mRoleProfile.Id = roleSeqId;
        mRoleProfile.SecurityEntityID = securityEntitySeqId;
        mRoleProfile = m_BRoles.GetProfile(mRoleProfile);
        UIRole mRetVal = new UIRole(mRoleProfile);
        mRetVal.AccountsInRole = getAccountsInRole(mRoleProfile);
        mRetVal.AccountsNotInRole = getAccountsNotInRole(mRoleProfile);
        return mRetVal;
    }

    public static UIRole Save(MRole roleProfile, string[] accountsInRole)
    {
        MRole mRoleToSave = new MRole(roleProfile);
        if (roleProfile.Id > -1) 
        {
            MRole mProfileFromDB = m_BRoles.GetProfile(mRoleToSave);
            mRoleToSave.AddedBy = mProfileFromDB.AddedBy; 
            mRoleToSave.AddedDate = mProfileFromDB.AddedDate;
        }
        mRoleToSave.Id = m_BRoles.Save(mRoleToSave);
        UpdateAllAccountsForRole(mRoleToSave.Id, mRoleToSave.SecurityEntityID, accountsInRole, mRoleToSave.UpdatedBy);
        return new UIRole(mRoleToSave);
    }

    public static bool UpdateAllAccountsForRole(int roleId, int securityEntitySeqId, string[] accounts, int accountId)
    {
        return m_BRoles.UpdateAllAccountsForRole(roleId, securityEntitySeqId, accounts, accountId);
    }
}
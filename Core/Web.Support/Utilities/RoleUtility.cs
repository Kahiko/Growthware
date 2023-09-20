using System.Collections;
using System.Data;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;

namespace GrowthWare.Web.Support.Utilities;

public static class RoleUtility
{
    static BRoles m_BRoles = new BRoles(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);

    private static string[] GetAccountsNotInRole(MRole profile)
    {
        string[] mRetVal = new string[] { };
        DataTable mDataTable = m_BRoles.GetAccountsInRole(profile);
        ArrayList mArrayList = new ArrayList();
        if(mDataTable != null)
        {
            foreach (DataRow mDataRow in mDataTable.Rows)
            {
                if(mDataRow["Account"] != null)
                {
                    mArrayList.Add(mDataRow["Account"].ToString());                    
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

    public static UIRole GetUIProfile(int roleSeqId, MSecurityEntity securityEntity)
    {
        MRole mRoleProfile = new MRole();
        mRoleProfile.Id = roleSeqId;
        mRoleProfile.SecurityEntityID = securityEntity.Id;
        mRoleProfile = m_BRoles.GetProfile(mRoleProfile);
        UIRole mRetVal = new UIRole(mRoleProfile);
        return mRetVal;
    }

    public static UIRole Save(MRole roleProfile)
    {
        m_BRoles.Save(roleProfile);
        m_BRoles.GetProfile(roleProfile);
        UIRole mRetVal = new UIRole(roleProfile);
        return mRetVal;
    }
}
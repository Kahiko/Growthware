using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;

namespace GrowthWare.Web.Support.Utilities;
public static class GroupUtility
{
    static BGroups m_BGroups = new BGroups(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);

    public static UIGroupProfile GetUIGroupProfile(int groupSeqId, int securityEntityId)
    {
        UIGroupProfile mRetVal = new UIGroupProfile();
        MGroupProfile mGroupProfile = m_BGroups.GetProfile(groupSeqId);
        MGroupRoles mGroupRoles = new MGroupRoles();
        // Populate mRetVal
        mRetVal.Description = mGroupProfile.Description;
        mRetVal.Id = mGroupProfile.Id;
        mRetVal.Name = mGroupProfile.Name;
        // Populate mGroupRoles
        mGroupRoles.Id = mGroupProfile.Id;
        mGroupRoles.GroupSeqId = mGroupRoles.Id;
        mGroupRoles.SecurityEntityID = securityEntityId;
        if(groupSeqId != -1)
        {
            mRetVal.RolesInGroup = GroupUtility.GetSelectedRoles(mGroupRoles);
        }
        ArrayList mRolesForSecurityEntity = RoleUtility.GetRolesArrayListBySecurityEntity(securityEntityId);
        List<string> mRolesNotInGroup = new List<string>();
        foreach (string role in mRolesForSecurityEntity)
        {
            if(!mRetVal.RolesInGroup.Any(s => s == role)) 
            {
                mRolesNotInGroup.Add(role);
            }
        }
        mRetVal.RolesNotInGroup = mRolesNotInGroup.ToArray();
        return mRetVal;
    }

    public static MGroupProfile GetGroupProfile(int groupSeqId) 
    {
        MGroupProfile mRetVal = m_BGroups.GetProfile(groupSeqId);
        return mRetVal;
    }

    private static string[] GetSelectedRoles(MGroupRoles groupRoles) 
    {
        string[] mRetVal = new string[]{};
        mRetVal = m_BGroups.GetSelectedRoles(groupRoles);
        return mRetVal;
    }

    public static ArrayList GetGroupsArrayListBySecurityEntity(int securityEntityId)
    {
        DataTable mGroupsTable = getAllGroupsBySecurityEntity(securityEntityId);
        ArrayList mRetVal = new ArrayList();
        foreach (DataRow item in mGroupsTable.Rows)
        {
            mRetVal.Add((string)item["NAME"]);
        }
        return mRetVal;
    }

    private static DataTable getAllGroupsBySecurityEntity(int securityEntityId)
    {
        // TODO: Add cache
        DataTable mGroups = m_BGroups.GetGroupsBySecurityEntity(securityEntityId);
        return mGroups;
    }
}
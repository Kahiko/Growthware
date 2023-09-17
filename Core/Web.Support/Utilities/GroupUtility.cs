using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using Microsoft.AspNetCore.Http.Features;

namespace GrowthWare.Web.Support.Utilities;
public static class GroupUtility
{
    static BGroups m_BGroups = new BGroups(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);

    public static UIGroupProfile Save(MGroupProfile profile, MGroupRoles groupRoles)
    {
        // Save the profile
        int mSavedGroupSeqId = m_BGroups.Save(profile);
        MGroupProfile mSavedGroupProfile = m_BGroups.GetProfile(mSavedGroupSeqId);
        // set the groupRoles id's
        groupRoles.Id = mSavedGroupSeqId;
        groupRoles.GroupSeqId = groupRoles.Id;
        // Save the group roles
        m_BGroups.UpdateGroupRoles(groupRoles);
        // get the profile from the DB
        UIGroupProfile mRetVal = GetUIGroupProfile(groupRoles.Id, groupRoles.SecurityEntityID);
        return mRetVal;
    }

    public static void UpdateGroupRoles(MGroupRoles groupRoles)
    {
        m_BGroups.UpdateGroupRoles(groupRoles);
    }

    public static UIGroupProfile GetUIGroupProfile(int groupSeqId, int securityEntityId)
    {
        UIGroupProfile mRetVal = new UIGroupProfile();
        MGroupProfile mGroupProfile = new MGroupProfile();
        MGroupRoles mGroupRoles = new MGroupRoles();
        if(groupSeqId != -1)
        {
            mGroupProfile = m_BGroups.GetProfile(groupSeqId);
        }
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

    /// <summary>
    /// Deletes the group.
    /// </summary>
    /// <param name="profile"></param>
    /// <exception cref="ArgumentNullException"></exception> <summary>
    /// 
    /// </summary>
    /// <param name="profile"></param>
    public static void Delete(MGroupProfile profile)
    {
        if (profile == null) throw new ArgumentNullException("profile", "profile cannot be a null reference (Nothing in VB) or empty!");
        if(profile.Id != -1) 
        {
            bool success = false;
            BGroups mBGroups = new BGroups(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            success = mBGroups.DeleteGroup(profile);
        }
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
using System;
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
    private static CacheController m_CacheController = CacheController.Instance();

    public static UIGroupProfile Save(MGroupProfile profile, MGroupRoles groupRoles)
    {
        BGroups mBGroups = new BGroups(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
        // Save the profile
        int mSavedGroupSeqId = mBGroups.Save(profile);
        MGroupProfile mSavedGroupProfile = mBGroups.GetProfile(mSavedGroupSeqId);
        // set the groupRoles id's
        groupRoles.Id = mSavedGroupSeqId;
        groupRoles.GroupSeqId = groupRoles.Id;
        // Save the group roles
        mBGroups.UpdateGroupRoles(groupRoles);
        // get the profile from the DB
        UIGroupProfile mRetVal = GetUIGroupProfile(groupRoles.Id, groupRoles.SecurityEntityID);
        return mRetVal;
    }

    public static void UpdateGroupRoles(MGroupRoles groupRoles)
    {
        BGroups mBGroups = new BGroups(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
        mBGroups.UpdateGroupRoles(groupRoles);
    }

    public static UIGroupProfile GetUIGroupProfile(int groupSeqId, int securityEntityId)
    {
        UIGroupProfile mRetVal = new UIGroupProfile();
        MGroupProfile mGroupProfile = new MGroupProfile();
        MGroupRoles mGroupRoles = new MGroupRoles();
        if(groupSeqId != -1)
        {
            BGroups mBGroups = new BGroups(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            mGroupProfile = mBGroups.GetProfile(groupSeqId);
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
        BGroups mBGroups = new BGroups(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
        MGroupProfile mRetVal = mBGroups.GetProfile(groupSeqId);
        return mRetVal;
    }

    private static string[] GetSelectedRoles(MGroupRoles groupRoles) 
    {
        string[] mRetVal = new string[]{};
        BGroups mBGroups = new BGroups(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
        mRetVal = mBGroups.GetSelectedRoles(groupRoles);
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
        String mCacheName = securityEntityId.ToString() + "_Groups";
        DataTable mRetVal = m_CacheController.GetFromCache<DataTable>(mCacheName);
        if(mRetVal == null)
        {
            BGroups mBGroups = new BGroups(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            mRetVal = mBGroups.GetGroupsBySecurityEntity(securityEntityId);
            m_CacheController.AddToCache(mCacheName, mRetVal);
        }
        return mRetVal;
    }
}
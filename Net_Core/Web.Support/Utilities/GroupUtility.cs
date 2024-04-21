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

    /// <summary>
    /// Saves the groups and roles for the given MGroupProfile and MGroupRoles to the database.
    /// </summary>
    /// <param name="profile">The UIGroupProfile to be saved.</param>
    /// <param name="groupRoles">The MGroupRoles to be saved.</param>
    /// <returns>The saved UIGroupProfile.</returns>
    public static UIGroupProfile Save(MGroupProfile profile, MGroupRoles groupRoles)
    {
        BGroups mBGroups = new BGroups(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement);
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

    /// <summary>
    /// Updates the group roles for a given group.
    /// </summary>
    /// <param name="groupRoles">The group roles to update.</param>
    /// <returns>No return value.</returns>
    public static void UpdateGroupRoles(MGroupRoles groupRoles)
    {
        BGroups mBGroups = new BGroups(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement);
        mBGroups.UpdateGroupRoles(groupRoles);
    }

    /// <summary>
    /// Retrieves a UIGroupProfile object based on the provided groupSeqId and 
    /// securityEntityId. The UIGroupProfile object contains information about 
    /// the group, its roles, and other details used in the UI.
    /// </summary>
    /// <param name="groupSeqId">The ID of the group.</param>
    /// <param name="securityEntityId">The ID of the security entity.</param>
    /// <returns>A UIGroupProfile object representing the group and its roles.</returns>
    public static UIGroupProfile GetUIGroupProfile(int groupSeqId, int securityEntityId)
    {
        UIGroupProfile mRetVal = new UIGroupProfile();
        MGroupProfile mGroupProfile = new MGroupProfile();
        MGroupRoles mGroupRoles = new MGroupRoles();
        if(groupSeqId != -1)
        {
            BGroups mBGroups = new BGroups(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement);
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
            BGroups mBGroups = new BGroups(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement);
            success = mBGroups.DeleteGroup(profile);
        }
    }

    /// <summary>
    /// Retrieves the group profile for a given group sequence ID.
    /// </summary>
    /// <param name="groupSeqId">The ID of the group.</param>
    /// <returns>The group profile for the given group sequence ID.</returns>
    public static MGroupProfile GetGroupProfile(int groupSeqId) 
    {
        BGroups mBGroups = new BGroups(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement);
        MGroupProfile mRetVal = mBGroups.GetProfile(groupSeqId);
        return mRetVal;
    }

    /// <summary>
    /// Retrieves the selected roles from the given MGroupRoles object.
    /// </summary>
    /// <param name="groupRoles">The MGroupRoles object from which to retrieve the selected roles.</param>
    /// <returns>An array of strings representing the selected roles.</returns>
    private static string[] GetSelectedRoles(MGroupRoles groupRoles) 
    {
        string[] mRetVal = new string[]{};
        BGroups mBGroups = new BGroups(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement);
        mRetVal = mBGroups.GetSelectedRoles(groupRoles);
        return mRetVal;
    }

    /// <summary>
    /// Retrieves a list of groups as an ArrayList for a given security entity ID.
    /// </summary>
    /// <param name="securityEntityId">The ID of the security entity.</param>
    /// <returns>An ArrayList of group names.</returns>
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

    /// <summary>
    /// Retrieves all groups by the specified security entity.
    /// </summary>
    /// <param name="securityEntityId">The ID of the security entity.</param>
    /// <returns>A DataTable containing the groups.</returns>
    private static DataTable getAllGroupsBySecurityEntity(int securityEntityId)
    {
        String mCacheName = securityEntityId.ToString() + "_Groups";
        DataTable mRetVal = m_CacheController.GetFromCache<DataTable>(mCacheName);
        if(mRetVal == null)
        {
            BGroups mBGroups = new BGroups(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement);
            mRetVal = mBGroups.GetGroupsBySecurityEntity(securityEntityId);
            m_CacheController.AddToCache(mCacheName, mRetVal);
        }
        return mRetVal;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.Web.Support.Helpers;

namespace GrowthWare.Web.Support.Utilities;
public static class GroupUtility
{
    private static BGroups m_BusinessLogic = null;
    private static CacheHelper m_CacheHelper = CacheHelper.Instance();

    /// <summary>
    /// Saves the groups and roles for the given MGroupProfile and MGroupRoles to the database.
    /// </summary>
    /// <param name="profile">The UIGroupProfile to be saved.</param>
    /// <param name="groupRoles">The MGroupRoles to be saved.</param>
    /// <returns>The saved UIGroupProfile.</returns>
    public static async Task<UIGroupProfile> Save(MGroupProfile profile, MGroupRoles groupRoles)
    {
        BGroups mBusinessLogic = getBusinessLogic();
        // Save the profile
        int mGroupSeqId = await mBusinessLogic.Save(profile);
        // set the groupRoles id's
        // groupRoles.Id = mGroupSeqId;
        groupRoles.GroupSeqId = mGroupSeqId;
        // Save the group roles
        await mBusinessLogic.UpdateGroupRoles(groupRoles);
        // get the profile from the DB
        UIGroupProfile mRetVal = await GetUIGroupProfile(mGroupSeqId, groupRoles.SecurityEntityID);
        return mRetVal;
    }

    /// <summary>
    /// Updates the group roles for a given group.
    /// </summary>
    /// <param name="groupRoles">The group roles to update.</param>
    /// <returns>No return value.</returns>
    // public static async Task UpdateGroupRoles(MGroupRoles groupRoles)
    // {
    //     BGroups mBGroups = getBusinessLogic();
    //     await mBGroups.UpdateGroupRoles(groupRoles);
    // }

    /// <summary>
    /// Retrieves a UIGroupProfile object based on the provided groupSeqId and 
    /// securityEntityId. The UIGroupProfile object contains information about 
    /// the group, its roles, and other details used in the UI.
    /// </summary>
    /// <param name="groupSeqId">The ID of the group.</param>
    /// <param name="securityEntityId">The ID of the security entity.</param>
    /// <returns>A UIGroupProfile object representing the group and its roles.</returns>
    public static async Task<UIGroupProfile> GetUIGroupProfile(int groupSeqId, int securityEntityId)
    {
        /*
         * Getting a Group Profile does cause multiple queries to the data store, however, in this instance 
         * this is not a large issue since this operation is only done when editing a group.  
         * Once Security has been set up the need to change a group will be low or even done
         * in the data store itself, it is far more likely that the the roles/groups associated 
         * with an Account will be changed.
         */
        MGroupProfile mGroupProfile = new MGroupProfile();
        if(groupSeqId != -1)
        {
            BGroups mBusinessLogic = getBusinessLogic();
            // Get the group profile from the data store
            mGroupProfile = await mBusinessLogic.GetProfile(groupSeqId);
        }
        // Populate mRetVal with the group profile
        UIGroupProfile mRetVal = new (mGroupProfile);
        // Get a Group Roles object
        MGroupRoles mGroupRoles = new (groupSeqId, securityEntityId);
        // Populate mRetVal with the roles associated with the group if a groupSeqId is provided
        if(groupSeqId != -1)
        {
            mRetVal.RolesInGroup = await GroupUtility.GetSelectedRoles(mGroupRoles);    
        }
        // Populate mRetVal with the roles not associated with the group
        ArrayList mRolesForSecurityEntity = await RoleUtility.GetRolesArrayListBySecurityEntity(securityEntityId);
        List<string> mRolesNotInGroup = new List<string>();
        foreach (string role in mRolesForSecurityEntity)
        {
            if(groupSeqId == -1 || !mRetVal.RolesInGroup.Any(s => s == role)) 
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
    public static async Task Delete(MGroupProfile profile)
    {
        if (profile == null) throw new ArgumentNullException(nameof(profile), "profile cannot be a null reference (Nothing in VB) or empty!");
        if(profile.Id != -1) 
        {
            bool success = false;
            BGroups mBusinessLogic = getBusinessLogic();
            success = await mBusinessLogic.DeleteGroup(profile);
        }
    }

    /// <summary>
    /// Retrieves the group profile for a given group sequence ID.
    /// </summary>
    /// <param name="groupSeqId">The ID of the group.</param>
    /// <returns>The group profile for the given group sequence ID.</returns>
    public static async Task<MGroupProfile> GetGroupProfile(int groupSeqId) 
    {
        BGroups mBusinessLogic = getBusinessLogic();
        MGroupProfile mRetVal = await mBusinessLogic.GetProfile(groupSeqId);
        return mRetVal;
    }

    /// <summary>
    /// Retrieves the selected roles from the given MGroupRoles object.
    /// </summary>
    /// <param name="groupRoles">The MGroupRoles object from which to retrieve the selected roles.</param>
    /// <returns>An array of strings representing the selected roles.</returns>
    private static async Task<string[]> GetSelectedRoles(MGroupRoles groupRoles) 
    {
        string[] mRetVal = new string[]{};
        BGroups mBusinessLogic = getBusinessLogic();
        mRetVal = await mBusinessLogic.GetSelectedRoles(groupRoles);
        return mRetVal;
    }

    /// <summary>
    /// Retrieves a list of groups as an ArrayList for a given security entity ID.
    /// </summary>
    /// <param name="securityEntityId">The ID of the security entity.</param>
    /// <returns>An ArrayList of group names.</returns>
    public static async Task<ArrayList> GetGroupsArrayListBySecurityEntity(int securityEntityId)
    {
        DataTable mGroupsTable = await getAllGroupsBySecurityEntity(securityEntityId);
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
    private static async Task<DataTable> getAllGroupsBySecurityEntity(int securityEntityId)
    {
        String mCacheName = securityEntityId.ToString() + "_Groups";
        DataTable mRetVal = m_CacheHelper.GetFromCache<DataTable>(mCacheName);
        if(mRetVal == null)
        {
            BGroups mBusinessLogic = getBusinessLogic();
            mRetVal = await mBusinessLogic.GetGroupsBySecurityEntity(securityEntityId);
            m_CacheHelper.AddToCache(mCacheName, mRetVal);
        }
        return mRetVal;
    }
    
    /// <summary>
    /// Returns the business logic object used to access the database.
    /// </summary>
    /// <returns></returns>
    private static BGroups getBusinessLogic()
    {
        if(m_BusinessLogic == null || ConfigSettings.CentralManagement == true)
        {
            m_BusinessLogic = new(SecurityEntityUtility.CurrentProfile);
        }
        return m_BusinessLogic;
    }
}
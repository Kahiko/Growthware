using GrowthWare.Framework.BusinessData.BusinessLogicLayer;
using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Web;

namespace GrowthWare.WebSupport.Utilities
{
    /// <summary>
    /// Class GroupUtility
    /// </summary>
    public static class GroupUtility
    {
        /// <summary>
        /// Saves the specified profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public static void Save(MGroupProfile profile)
        {
            if (profile == null) throw new ArgumentNullException("profile", "profile cannot be a null reference (Nothing in VB) or empty!");
            MSecurityEntityProfile mSecurityProfile = SecurityEntityUtility.CurrentProfile();
            BGroups mBGroups = new BGroups(mSecurityProfile, ConfigSettings.CentralManagement);
            profile.SecurityEntityId = mSecurityProfile.Id;
            mBGroups.Save(profile);
            CacheController.RemoveFromCache(SecurityEntitiesGroupsCacheName(profile.SecurityEntityId));
            CacheController.RemoveAllCache();
        }

        /// <summary>
        /// Searches the groups using the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>DataTable.</returns>
        public static DataTable Search(MSearchCriteria searchCriteria)
        {
            if (searchCriteria == null) throw new ArgumentNullException("searchCriteria", "searchCriteria cannot be a null reference (Nothing in VB) or empty!");
            BGroups mBGroups = new BGroups(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            return mBGroups.Search(searchCriteria);
        }

        /// <summary>
        /// Securities the name of the entities groups cache.
        /// </summary>
        /// <param name="securityEntityId">The Security Entity Id.</param>
        /// <returns>System.String.</returns>
        public static string SecurityEntitiesGroupsCacheName(int securityEntityId)
        {
            string retVal = securityEntityId + "_SecurityEntityGroups";
            return retVal;
        }

        /// <summary>
        /// Gets all groups by BU.
        /// </summary>
        /// <param name="securityEntityId">The SECURIT y_ ENTIT y_ SE q_ ID.</param>
        /// <returns>DataTable.</returns>
        public static DataTable GetAllGroupsBySecurityEntity(int securityEntityId)
        {
            DataTable mySecurityEntityGroups = null;
            // attempt to retrieve the information from cache
            mySecurityEntityGroups = (DataTable)HttpContext.Current.Cache[SecurityEntitiesGroupsCacheName(securityEntityId)];
            // if the information was not avalible in cache
            // then retieve the information from the DB and put it into
            // cache for subsequent use.
            if (mySecurityEntityGroups == null)
            {
                BGroups mBGroups = new BGroups(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
                mySecurityEntityGroups = mBGroups.GetGroupsBySecurityEntity(securityEntityId);
                CacheController.AddToCacheDependency(SecurityEntitiesGroupsCacheName(securityEntityId), mySecurityEntityGroups);
            }
            return mySecurityEntityGroups;
        }

        /// <summary>
        /// Gets the groups array list by BU.
        /// </summary>
        /// <param name="securityEntityId">The SECURIT y_ ENTIT y_ SE q_ ID.</param>
        /// <returns>ArrayList.</returns>
        public static ArrayList GetGroupsArrayListBySecurityEntity(int securityEntityId)
        {
            DataTable mySecurityEntityGroups = null;
            ArrayList colGroups = new ArrayList();
            DataRow groupRow = null;
            mySecurityEntityGroups = GetAllGroupsBySecurityEntity(securityEntityId);
            foreach (DataRow groupRow_loopVariable in mySecurityEntityGroups.Rows)
            {
                groupRow = groupRow_loopVariable;
                colGroups.Add((string)groupRow["NAME"]);
            }
            return colGroups;
        }

        /// <summary>
        /// Gets the profile.
        /// </summary>
        /// <param name="groupId">The group ID.</param>
        /// <returns>MGroupProfile.</returns>
        public static MGroupProfile GetProfile(int groupId)
        {
            BGroups mBGroups = new BGroups(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            return mBGroups.GetProfile(groupId);
        }

        /// <summary>
        /// Gets the selected roles.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns>System.String[][].</returns>
        public static string[] GetSelectedRoles(MGroupRoles profile) 
        {
            if (profile == null) throw new ArgumentNullException("profile", "profile cannot be a null reference (Nothing in VB) or empty!");
            BGroups mBGroups = new BGroups(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            return mBGroups.GetSelectedRoles(profile);
        }

        /// <summary>
        /// Adds the group.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public static void AddGroup(MGroupProfile profile)
        {
            if (profile == null) throw new ArgumentNullException("profile", "profile cannot be a null reference (Nothing in VB) or empty!");
            BGroups mBGroups = new BGroups(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            mBGroups.AddGroup(profile);
            CacheController.RemoveFromCache(SecurityEntitiesGroupsCacheName(profile.SecurityEntityId));
            CacheController.RemoveAllCache();
        }

        /// <summary>
        /// Deletes the group.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns><c>true</c> if no errors, <c>true</c> otherwise false</returns>
        public static void Delete(MGroupProfile profile)
        {
            if (profile == null) throw new ArgumentNullException("profile", "profile cannot be a null reference (Nothing in VB) or empty!");
            bool success = false;
            BGroups mBGroups = new BGroups(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            success = mBGroups.DeleteGroup(profile);
            if (success)
            {
                CacheController.RemoveFromCache(SecurityEntitiesGroupsCacheName(profile.SecurityEntityId));
                CacheController.RemoveAllCache();
            }
        }

        /// <summary>
        /// Updates the group roles.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public static void UpdateGroupRoles(MGroupRoles profile)
        {
            if (profile == null) throw new ArgumentNullException("profile", "profile cannot be a null reference (Nothing in VB) or empty!");
            BGroups mBGroups = new BGroups(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            try
            {
                mBGroups.UpdateGroupRoles(profile);
            }
            catch (Exception ex)
            {
                Logger mLogger = Logger.Instance();
                mLogger.Error(ex);
                throw new Exception("Could not associate the roles to the group please see the logs for details.");
            }
            CacheController.RemoveAllCache();
        }
    }
}

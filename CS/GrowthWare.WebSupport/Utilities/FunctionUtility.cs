using GrowthWare.Framework.BusinessData.BusinessLogicLayer;
using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Web;
using System.Globalization;
using System.Data;
using GrowthWare.Framework.Model.Enumerations;

namespace GrowthWare.WebSupport.Utilities
{
    /// <summary>
    /// FunctionUtility serves as the focal point for any web application needing to utiltize the GrowthWare framework.
    /// Web needs such as caching are handeled here
    /// </summary>
    public static class FunctionUtility
    {
        /// <summary>
        /// The function profile info name
        /// </summary>
        public static readonly string m_FunctionProfileInfoName = "FunctionProfileInfo";

        /// <summary>
        /// Retrieves all functions from the either the database or cache
        /// </summary>
        /// <returns>A Collection of MFunctinProfiles</returns>
        public static Collection<MFunctionProfile> GetFunctions()
        {
            MSecurityEntityProfile mSecurityEntityProfile = SecurityEntityUtility.GetCurrentProfile();
            BFunctions mBFunctions = new BFunctions(mSecurityEntityProfile, ConfigSettings.CentralManagement);
            String mCacheName = mSecurityEntityProfile.Id.ToString() + "_Functions";
            Collection<MFunctionProfile> mRetVal = null;
            mRetVal = (Collection<MFunctionProfile>)(HttpContext.Current.Cache[mCacheName]);
            if (mRetVal == null)
            {
                mRetVal = mBFunctions.GetFunctions(mSecurityEntityProfile.Id);
                CacheController.AddToCacheDependency(mCacheName, mRetVal);
            }
            return mRetVal;
        }

        /// <summary>
        /// Gets the current profile.
        /// </summary>
        /// <returns>MFunctionProfile.</returns>
        public static MFunctionProfile GetCurrentProfile()
        {
            MFunctionProfile mRetVal = (MFunctionProfile)HttpContext.Current.Items[m_FunctionProfileInfoName];
            return mRetVal;
        }

        /// <summary>
        /// Get a single function given it's action.
        /// </summary>
        /// <param name="action">String</param>
        /// <returns>MFunctionProfile</returns>
        /// <remarks>Returns null object if not found</remarks>
        public static MFunctionProfile GetProfile(String action)
        {
            MFunctionProfile mRetVal = null;
            if (!string.IsNullOrEmpty(action))
            {
                var mResult = from mProfile in GetFunctions()
                              where mProfile.Action.ToLower(CultureInfo.CurrentCulture) == action.ToLower(CultureInfo.CurrentCulture)
                              select mProfile;
                mRetVal = new MFunctionProfile();
                try
                {
                    mRetVal = mResult.First();
                }
                catch (NullReferenceException)
                {
                    mRetVal = null;
                }
            }
            return mRetVal;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public static DataTable GetFunctionMenuOrder(ref MFunctionProfile profile)
        {
            BFunctions mBFunctions = new BFunctions(SecurityEntityUtility.GetCurrentProfile(), ConfigSettings.CentralManagement);
            return mBFunctions.GetMenuOrder(ref profile);
        }

        /// <summary>
        /// Get a single function given it's id.
        /// </summary>
        /// <param name="id">int or Integer</param>
        /// <returns>MFunctionProfile</returns>
        /// <remarks>Returns null object if not found</remarks>
        public static MFunctionProfile GetProfile(int id)
        {
            var mResult = from mProfile in GetFunctions()
                          where mProfile.Id == id
                          select mProfile;
            MFunctionProfile mRetVal = null;
            try
            {
                mRetVal = mResult.First();
            }
            catch (NullReferenceException)
            {
                mRetVal = null;
            }
            return mRetVal;
        }

        /// <summary>
        /// Moves the specified profiles menu order.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <param name="Direction">The direction.</param>
        /// <param name="updatedBy">The updated by.</param>
        /// <param name="upDatedDate">Up dated date.</param>
        public static void Move(ref MFunctionProfile profile, DirectionType Direction, int updatedBy, DateTime upDatedDate)
        {
            BFunctions mBAppFunctions = new BFunctions(SecurityEntityUtility.GetCurrentProfile(), ConfigSettings.CentralManagement);
            mBAppFunctions.MoveMenuOrder(ref profile, Direction);
            RemoveCachedFunctions();
        }

        /// <summary>
        /// Removes all fucntion cache for each Security Entity
        /// </summary>
        public static void RemoveCachedFunctions()
        {
            String mCacheName = string.Empty;
            Collection<MSecurityEntityProfile> mSecurityProfiles = SecurityEntityUtility.GetProfiles();
            foreach (MSecurityEntityProfile mProfile in mSecurityProfiles)
            {
                mCacheName = mProfile.Id.ToString() + "_Functions";
                CacheController.RemoveFromCache(mCacheName);
            }
        }

        /// <summary>
        /// Saves the specified profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <param name="saveGroups">if set to <c>true</c> [save groups].</param>
        /// <param name="saveRoles">if set to <c>true</c> [save roles].</param>
        public static void Save(MFunctionProfile profile, bool saveGroups, bool saveRoles)
        {
            BFunctions mBAppFunctions = new BFunctions(SecurityEntityUtility.GetCurrentProfile(), ConfigSettings.CentralManagement);
            try
            {
                mBAppFunctions.Save(profile, saveGroups, saveRoles);
            }
            catch (Exception ex)
            {
                Logger mLog = Logger.Instance();
                mLog.Error(ex);
            }
            finally
            {
                RemoveCachedFunctions();
            }
        }

        /// <summary>
        /// Deletes the specified function given the function seq id.
        /// </summary>
        /// <param name="functionSeqId">The function seq id.</param>
        public static void Delete(int functionSeqId)
        {
            BFunctions mBAppFunctions = new BFunctions(SecurityEntityUtility.GetCurrentProfile(), ConfigSettings.CentralManagement);
            try
            {
                mBAppFunctions.Delete(functionSeqId);
            }
            catch (Exception ex)
            {
                Logger mLog = Logger.Instance();
                mLog.Error(ex);
            }
            finally
            {
                RemoveCachedFunctions();
            }
        }

        /// <summary>
        /// Sets the current function.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public static void SetCurrentFunction(MFunctionProfile profile)
        {
            HttpContext.Current.Items[m_FunctionProfileInfoName] = profile;
        }

        /// <summary>
        /// Searches the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>DataTable.</returns>
        public static DataTable Search(MSearchCriteria searchCriteria)
        {
            BFunctions mBAppFunctions = new BFunctions(SecurityEntityUtility.GetCurrentProfile(), ConfigSettings.CentralManagement);
            return mBAppFunctions.Search(searchCriteria);
        }
    }
}

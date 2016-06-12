using GrowthWare.Framework.BusinessData.BusinessLogicLayer;
using GrowthWare.Framework.BusinessData.DataAccessLayer;
using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace GrowthWare.WebSupport.Utilities
{
    /// <summary>
    /// SecurityEntityUtility serves as the focal point for any web application needing to utiltize the GrowthWare framework.
    /// Web needs such as caching are handeled here
    /// </summary>
    public static class SecurityEntityUtility
    {
        private static String s_CacheName = "Cached_SecurityEntities";

        private static MSecurityEntityProfile s_DefaultProfile = null;

        //private static BSecurityEntity m_BSecurityEntity = null;

        private const string SESSION_PROFILE_NAME = "SessionSecurityEntityProfile";

        /// <summary>
        /// Creates and returns MSecurityEntityProfile populated with information from the
        /// configuration file.
        /// </summary>
        /// <returns>MSecurityEntityProfile</returns>
        public static MSecurityEntityProfile DefaultProfile()
        {
            if (s_DefaultProfile == null)
            {
                MSecurityEntityProfile mDefaultProfile = new MSecurityEntityProfile();
                mDefaultProfile.Id = int.Parse(ConfigSettings.DefaultSecurityEntityId.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                mDefaultProfile.DataAccessLayer = ConfigSettings.DataAccessLayer;
                mDefaultProfile.DataAccessLayerNamespace = ConfigSettings.DataAccessLayerNamespace(mDefaultProfile.DataAccessLayer);
                mDefaultProfile.DataAccessLayerAssemblyName = ConfigSettings.DataAccessLayerAssemblyName(mDefaultProfile.DataAccessLayer);
                mDefaultProfile.ConnectionString = ConfigSettings.ConnectionString(mDefaultProfile.DataAccessLayer);
                s_DefaultProfile = mDefaultProfile;
            }
            return s_DefaultProfile;
        }

        /// <summary>
        /// Returns the current MSecurityEntityProfile from context.  If one is not found in context then 
        /// the default values from the config file will be returned.
        /// </summary>
        /// <returns>MSecurityEntityProfile</returns>
        public static MSecurityEntityProfile CurrentProfile()
        {
            MSecurityEntityProfile mRetProfile = null;
            String mAccount = AccountUtility.HttpContextUserName();
            MClientChoicesState mClientChoicesState = ClientChoicesUtility.GetClientChoicesState(mAccount);
            if (mClientChoicesState != null) 
            {
                int mSecurityEntity = int.Parse(mClientChoicesState[MClientChoices.SecurityEntityId].ToString(), CultureInfo.InvariantCulture);
                mRetProfile = GetProfile(mSecurityEntity);
            }
            if (mRetProfile == null) mRetProfile = DefaultProfile();
            return mRetProfile;
        }

        /// <summary>
        /// Retrieves all security entities from the either the database or cache
        /// </summary>
        /// <returns>A Collection of MSecurityEntityProfile</returns>
        public static Collection<MSecurityEntityProfile> Profiles()
        {
            Collection<MSecurityEntityProfile> mRetVal = null;
            mRetVal = (Collection<MSecurityEntityProfile>)(HttpContext.Current.Cache[s_CacheName]);
            if (mRetVal == null)
            {
                BSecurityEntity mBSecurityEntity = new BSecurityEntity(DefaultProfile(), ConfigSettings.CentralManagement);
                mRetVal = mBSecurityEntity.SecurityEntities();
                CacheController.AddToCacheDependency(s_CacheName, mRetVal);
            }
            return mRetVal;
        }

        /// <summary>
        /// Get a single function given it's action.
        /// </summary>
        /// <param name="name">String</param>
        /// <returns>MSecurityEntityProfile</returns>
        public static MSecurityEntityProfile GetProfile(String name)
        {
            MSecurityEntityProfile mRetVal = new MSecurityEntityProfile();
            var mResult = from mProfile in Profiles()
                          where mProfile.Name.ToLower(CultureInfo.CurrentCulture) == name.ToLower(CultureInfo.CurrentCulture)
                          select mProfile;
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
        /// Get a single function given it's id.
        /// </summary>
        /// <param name="securityEntitySeqId">int or Integer</param>
        /// <returns>MSecurityEntityProfile</returns>
        public static MSecurityEntityProfile GetProfile(int securityEntitySeqId)
        {
            MSecurityEntityProfile mRetVal = new MSecurityEntityProfile();
            var mResult = from mProfile in Profiles()
                          where mProfile.Id == securityEntitySeqId
                          select mProfile;
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
        /// Gets the valid security entities.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="securityEntityId">The security entity id.</param>
        /// <param name="isSystemAdmin">if set to <c>true</c> [is system admin].</param>
        /// <returns>DataView.</returns>
        public static DataView GetValidSecurityEntities(string account, int securityEntityId, bool isSystemAdmin)
        {
            BSecurityEntity mBSecurityEntities = new BSecurityEntity(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            return mBSecurityEntities.GetValidSecurityEntities(account, securityEntityId, isSystemAdmin).DefaultView;
        }

        /// <summary>
        /// Searches Security Entities with the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>DataTable.</returns>
        public static DataTable Search(MSearchCriteria searchCriteria)
        {
            try
            {
                BSecurityEntity mBSecurityEntities = new BSecurityEntity(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
                return mBSecurityEntities.Search(searchCriteria);
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
        }

        /// <summary>
        /// Saves the specified profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public static void Save(MSecurityEntityProfile profile)
        {
            try
            {
                BSecurityEntity mBSecurityEntities = new BSecurityEntity(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
                mBSecurityEntities.Save(profile);
                CacheController.RemoveAllCache();
            }
            catch (DataAccessLayerException ex)
            {
                Logger mLog = Logger.Instance();
                mLog.Error(ex);
            }
        }
    }
}

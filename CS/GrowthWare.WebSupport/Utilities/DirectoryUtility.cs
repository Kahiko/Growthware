using GrowthWare.Framework.BusinessData.BusinessLogicLayer;
using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GrowthWare.WebSupport.Utilities
{
    /// <summary>
    /// Class DirectoryUtility
    /// </summary>
    public static class DirectoryUtility
    {
        /// <summary>
        /// The m_ directory info cached name
        /// </summary>
        public static readonly string m_DirectoryInfoCachedName = "DirectoryInfoCollection";

        /// <summary>
        /// Gets the directories.
        /// </summary>
        /// <returns>Collection{MDirectoryProfile}.</returns>
        public static Collection<MDirectoryProfile> GetDirectories()
        {
            MSecurityEntityProfile mSecurityEntityProfile = SecurityEntityUtility.CurrentProfile();
            BDirectories mBDirectories = new BDirectories(mSecurityEntityProfile, ConfigSettings.CentralManagement);
            String mCacheName = mSecurityEntityProfile.Id.ToString() + "_" + m_DirectoryInfoCachedName;
            Collection<MDirectoryProfile> mRetVal = null;
            mRetVal = (Collection<MDirectoryProfile>)(HttpContext.Current.Cache[mCacheName]);
            if (mRetVal == null)
            {
                mRetVal = mBDirectories.Directories();
                CacheController.AddToCacheDependency(mCacheName, mRetVal);
            }
            return mRetVal;
        }

        /// <summary>
        /// Gets the profile.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>MDirectoryProfile.</returns>
        public static MDirectoryProfile GetProfile(String name)
        {
            var mResult = from mProfile in GetDirectories()
                          where mProfile.Name.ToLower(CultureInfo.CurrentCulture) == name.ToLower(CultureInfo.CurrentCulture)
                          select mProfile;
            MDirectoryProfile mRetVal = null;
            try
            {
                mRetVal = mResult.First();
            }
            catch (NullReferenceException)
            {
                mRetVal = null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            return mRetVal;

        }

        /// <summary>
        /// Gets the profile.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>MDirectoryProfile.</returns>
        public static MDirectoryProfile GetProfile(int id)
        {
            var mResult = from mProfile in GetDirectories()
                          where mProfile.Id == id
                          select mProfile;
            MDirectoryProfile mRetVal = null;
            try
            {
                mRetVal = mResult.First();
            }
            catch (NullReferenceException)
            {
                mRetVal = null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            return mRetVal;

        }

        /// <summary>
        /// Gets the profile by function.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>MDirectoryProfile.</returns>
        public static MDirectoryProfile GetProfileByFunction(int id)
        {
            var mResult = from mProfile in GetDirectories()
                          where mProfile.FunctionSeqId == id
                          select mProfile;
            MDirectoryProfile mRetVal = null;
            try
            {
                mRetVal = mResult.First();
            }
            catch (NullReferenceException)
            {
                mRetVal = null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            return mRetVal;

        }

        /// <summary>
        /// Saves the specified profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public static void Save(MDirectoryProfile profile)
        {
            CacheController.RemoveFromCache(m_DirectoryInfoCachedName);
            MSecurityEntityProfile mSecurityEntityProfile = SecurityEntityUtility.CurrentProfile();
            try
            {
                profile.ImpersonatePassword = CryptoUtility.Decrypt(profile.ImpersonatePassword, mSecurityEntityProfile.EncryptionType);
            }
            catch
            {
                profile.ImpersonatePassword = CryptoUtility.Encrypt(profile.ImpersonatePassword, mSecurityEntityProfile.EncryptionType);
            }
            try
            {
                profile.Directory = CryptoUtility.Decrypt(profile.Directory, mSecurityEntityProfile.EncryptionType);
            }
            catch
            {
                profile.Directory = CryptoUtility.Encrypt(profile.Directory, mSecurityEntityProfile.EncryptionType);
            }
            try
            {
                profile.ImpersonateAccount = CryptoUtility.Decrypt(profile.ImpersonateAccount, mSecurityEntityProfile.EncryptionType);
            }
            catch
            {
                profile.ImpersonateAccount = CryptoUtility.Encrypt(profile.ImpersonateAccount, mSecurityEntityProfile.EncryptionType);
            }
            BDirectories myBLL = new BDirectories(mSecurityEntityProfile, ConfigSettings.CentralManagement);
            myBLL.Save(profile);
            String mCacheName = mSecurityEntityProfile.Id.ToString() + "_" + m_DirectoryInfoCachedName;
            CacheController.RemoveFromCache(mCacheName);
        }
    }
}

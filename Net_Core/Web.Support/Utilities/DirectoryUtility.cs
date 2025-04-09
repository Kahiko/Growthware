using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Web.Support.Helpers;

namespace GrowthWare.Web.Support.Utilities;

public static class DirectoryUtility
{

    private static BDirectories m_BusinessLogic = null;

    private static CacheHelper m_CacheHelper = CacheHelper.Instance();

    /// <summary>
    /// The m_ directory info cached name
    /// </summary>
    private static string s_DirectoryInfoCachedName = "DirectoryInfoCollection";

    /// <summary>
    /// Gets the directories.
    /// </summary>
    /// <returns>Collection{MDirectoryProfile}.</returns>
    public static async Task<Collection<MDirectoryProfile>> Directories()
    {
        MSecurityEntity mSecurityEntityProfile = SecurityEntityUtility.GetCurrentProfile();
        String mCacheName = mSecurityEntityProfile.Id.ToString(CultureInfo.InvariantCulture) + "_" + s_DirectoryInfoCachedName;
        Collection<MDirectoryProfile> mRetVal = m_CacheHelper.GetFromCache<Collection<MDirectoryProfile>>(mCacheName);;
        if(mRetVal == null)
        {
            BDirectories mBDirectories = BusinessLogic;
            mRetVal = await mBDirectories.Directories();
            m_CacheHelper.AddToCache(mCacheName, mRetVal);
        }
        return mRetVal;
    }
    
    /// <summary>
    /// Returns the business logic object used to access the database.
    /// </summary>
    /// <returns></returns>
    private static BDirectories BusinessLogic
    {
        get 
        {
            if(m_BusinessLogic == null || ConfigSettings.CentralManagement == true)
            {
                MSecurityEntity mSecurityEntityCurrentProfile = SecurityEntityUtility.GetCurrentProfile();
                m_BusinessLogic = new(mSecurityEntityCurrentProfile);
            }
            return m_BusinessLogic;
        }
    }

    /// <summary>
    /// Get the desired MDirectoryProfile given the functionSeqId
    /// </summary>
    /// <param name="functionSeqId">int</param>
    /// <returns>A populated MDirectoryProfile or null</returns>
    public static async Task<MDirectoryProfile> GetDirectoryProfile(int functionSeqId)
    {
        Collection<MDirectoryProfile> mDirectories = await Directories();
        var mResult = from mProfile in mDirectories
                        where mProfile.Id == functionSeqId
                        select mProfile;
        MDirectoryProfile mRetVal = null;
        try
        {
            mRetVal = mResult.First();
        }
        catch (NullReferenceException)
        {
            mRetVal = new MDirectoryProfile();
        }
        catch (InvalidOperationException)
        {
            return new MDirectoryProfile();
        }
        return mRetVal;
    }

    public static async Task Save(MDirectoryProfile profile)
    {
        BDirectories mBDirectories = BusinessLogic;
        await mBDirectories.Save(profile);
        MSecurityEntity mSecurityEntityCurrentProfile = SecurityEntityUtility.GetCurrentProfile();
        String mCacheName = mSecurityEntityCurrentProfile.Id.ToString(CultureInfo.InvariantCulture) + "_" + s_DirectoryInfoCachedName;
        m_CacheHelper.RemoveFromCache(mCacheName);
    }
}
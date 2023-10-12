using Microsoft.AspNetCore.Http;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;

namespace GrowthWare.Web.Support.Utilities;

public static class DirectoryUtility
{

    /// <summary>
    /// The m_ directory info cached name
    /// </summary>
    private static string s_DirectoryInfoCachedName = "DirectoryInfoCollection";
    private static IHttpContextAccessor m_HttpContextAccessor = null;

    /// <summary>
    /// Gets the directories.
    /// </summary>
    /// <returns>Collection{MDirectoryProfile}.</returns>
    public static Collection<MDirectoryProfile> Directories()
    {
        MSecurityEntity mSecurityEntityProfile = SecurityEntityUtility.CurrentProfile();
        String mCacheName = mSecurityEntityProfile.Id.ToString(CultureInfo.InvariantCulture) + "_" + s_DirectoryInfoCachedName;
        Collection<MDirectoryProfile> mRetVal = null;
        String mJsonString = m_HttpContextAccessor.HttpContext.Session.GetString(mCacheName);
        if (mJsonString != null && !String.IsNullOrEmpty(mJsonString))
        {
            mRetVal = JsonSerializer.Deserialize<Collection<MDirectoryProfile>>(mJsonString);
        }
        else
        {
            BDirectories mBDirectories = new BDirectories(mSecurityEntityProfile, ConfigSettings.CentralManagement);
            mRetVal = mBDirectories.Directories();
            mJsonString = JsonSerializer.Serialize(mRetVal);
            m_HttpContextAccessor.HttpContext.Session.SetString(mCacheName, mJsonString);
        }
        return mRetVal;
    }

    /// <summary>
    /// Get the desired MDirectoryProfile given the functionSeqId
    /// </summary>
    /// <param name="functionSeqId">int</param>
    /// <returns>A populated MDirectoryProfile or null</returns>
    public static MDirectoryProfile GetDirectoryProfile(int functionSeqId)
    {
            var mResult = from mProfile in Directories()
                          where mProfile.FunctionSeqId == functionSeqId
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

    [CLSCompliant(false)]
    public static void SetHttpContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        m_HttpContextAccessor = httpContextAccessor;
    }
}
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;

namespace GrowthWare.Web.Support.Utilities;

public static class SecurityEntityUtility
{

    private static CacheController m_CacheController = CacheController.Instance();
    private static IHttpContextAccessor m_HttpContextAccessor = null;
    private static String s_CacheName = "Cached_SecurityEntities";

    public static MSecurityEntity CurrentProfile()
    {
        MSecurityEntity mRetProfile = null;
        if(m_HttpContextAccessor != null) 
        {
            string mJsonString = m_HttpContextAccessor.HttpContext.Session.GetString(MClientChoices.SessionName);
            if(mJsonString != null)
            {
                MClientChoicesState mClientChoicesState = new MClientChoicesState(mJsonString);
                if (mClientChoicesState != null) 
                {
                    int mSecurityEntity = int.Parse(mClientChoicesState[MClientChoices.SecurityEntityID].ToString(), CultureInfo.InvariantCulture);
                    mRetProfile = GetProfile(mSecurityEntity);
                }
            }
            if (mRetProfile == null) mRetProfile = DefaultProfile();
        }
        return mRetProfile;
    }

    public static MSecurityEntity DefaultProfile()
    {
        return new MSecurityEntity
        {
            ConnectionString = ConfigSettings.ConnectionString,
            DataAccessLayer = ConfigSettings.DataAccessLayer,
            DataAccessLayerAssemblyName = ConfigSettings.DataAccessLayerAssemblyName,
            DataAccessLayerNamespace = ConfigSettings.DataAccessLayerNamespace,
            EncryptionType = ConfigSettings.EncryptionType,
            Id = ConfigSettings.DefaultSecurityEntityID
        };
    }

    /// <summary>
    /// Get a single function given it's action.
    /// </summary>
    /// <param name="name">String</param>
    /// <returns>MSecurityEntityProfile</returns>
    public static MSecurityEntity GetProfile(string name)
    {
        MSecurityEntity mRetVal = new MSecurityEntity();
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
    public static MSecurityEntity GetProfile(int securityEntitySeqId)
    {
        MSecurityEntity mRetVal = new MSecurityEntity();
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
    public static DataTable GetValidSecurityEntities(string account, int securityEntityId, bool isSystemAdmin)
    {
        BSecurityEntities mBSecurityEntities = new BSecurityEntities(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
        return mBSecurityEntities.GetValidSecurityEntities(account, securityEntityId, isSystemAdmin);
    }

    [CLSCompliant(false)]
    public static void SetHttpContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        m_HttpContextAccessor = httpContextAccessor;
    }

    public static Collection<MSecurityEntity> Profiles()
    {
        Collection<MSecurityEntity> mSecurityEntities = m_CacheController.GetFromCache<Collection<MSecurityEntity>>(s_CacheName);
        if (mSecurityEntities == null)
        {
            mSecurityEntities = new Collection<MSecurityEntity>();
            BSecurityEntities mBSecurityEntities = new BSecurityEntities(DefaultProfile(), ConfigSettings.CentralManagement);
            foreach (MSecurityEntity mSecurityEntity in mBSecurityEntities.SecurityEntities())
            {
                // mSecurityEntity.ConnectionString = CryptoUtility.Decrypt(mSecurityEntity.ConnectionString, ConfigSettings.EncryptionType);
                string mDecryptedPassword;
                CryptoUtility.TryDecrypt(mSecurityEntity.ConnectionString, out mDecryptedPassword, ConfigSettings.EncryptionType);
                mSecurityEntity.ConnectionString = mDecryptedPassword;
                mSecurityEntities.Add(mSecurityEntity);
            }
            m_CacheController.AddToCache(s_CacheName, mSecurityEntities);
        }
        return mSecurityEntities;
    }
}
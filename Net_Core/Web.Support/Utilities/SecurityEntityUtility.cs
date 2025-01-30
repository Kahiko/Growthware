using Microsoft.AspNetCore.Http;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Web.Support.Helpers;

namespace GrowthWare.Web.Support.Utilities;

public static class SecurityEntityUtility
{

    private static BSecurityEntities m_BusinessLogic = null;
    private static BSecurityEntities m_DefaultBusinessLogic = null;
    private static CacheHelper m_CacheHelper = CacheHelper.Instance();
    private static IHttpContextAccessor m_HttpContextAccessor = null;
    private static String s_CacheName = "Cached_SecurityEntities";
    private static string s_CacheRegistrationsName = "Cached_RegistrationInformations";

    public static MSecurityEntity CurrentProfile
    {
        get
        {
            MSecurityEntity mRetProfile = null;
            if(!ConfigSettings.SecurityEntityFromUrl)
            {
                if (m_HttpContextAccessor != null)
                {
                    MClientChoicesState mClientChoicesState = ClientChoicesUtility.CurrentState;
                    if (mClientChoicesState != null)
                    {
                        int mSecurityEntity = int.Parse(mClientChoicesState[MClientChoices.SecurityEntityId].ToString(), CultureInfo.InvariantCulture);
                        mRetProfile = GetProfile(mSecurityEntity);
                    }
                }
            }
            else
            {
                if (m_HttpContextAccessor != null)
                {
                    string mUrl = m_HttpContextAccessor.HttpContext.Request.Scheme + "://" + m_HttpContextAccessor.HttpContext.Request.Host.Host;
                    mRetProfile = GetProfileByUrl(mUrl);
                    // TODO: Unsure if I should attempt to get the selected profile from ClientChoices
                    // for now we'll get the default one I am not and just lettting the
                    // the default profile to be returned
                }
            }
            mRetProfile ??= DefaultProfile();
            return mRetProfile;
        }
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

    public static void DeleteRegistrationInformation(int securityEntitySeqId)
    {
        getBusinessLogic().DeleteRegistrationInformation(securityEntitySeqId);
        m_CacheHelper.RemoveFromCache(s_CacheRegistrationsName);
    }

    /// <summary>
    /// Returns the business logic object used to access the database.
    /// </summary>
    /// <returns></returns>
    private static BSecurityEntities getBusinessLogic(bool useDefault = false)
    {
        if(!useDefault)
        {
            if(m_BusinessLogic == null || ConfigSettings.CentralManagement == true)
            {
                m_BusinessLogic = new(SecurityEntityUtility.CurrentProfile);
            }
            return m_BusinessLogic;
        }
        else
        {
            if(m_DefaultBusinessLogic == null || ConfigSettings.CentralManagement == true)
            {
                m_DefaultBusinessLogic = new(DefaultProfile());
            }
            return m_DefaultBusinessLogic;
        }
    }

    /// <summary>
    /// Get a single function given it's action.
    /// </summary>
    /// <param name="name">String</param>
    /// <returns>MSecurityEntityProfile</returns>
    public static MSecurityEntity GetProfile(string name)
    {
        MSecurityEntity mRetVal = null;
        var mResult = from mProfile in Profiles()
                      where mProfile.Name.ToLower(CultureInfo.CurrentCulture) == name.ToLower(CultureInfo.CurrentCulture)
                      select mProfile;
        if(mResult.Any()) 
        {
            mRetVal = mResult.First();
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
        MSecurityEntity mRetVal = null;
        var mResult = from mProfile in Profiles()
                      where mProfile.Id == securityEntitySeqId
                      select mProfile;
        if(mResult.Any()) 
        {
            mRetVal = mResult.First();
        }
        return mRetVal;
    }

    /// <summary>
    /// Get the first profile that has a partial match on the url and the URL is not "no url".
    /// </summary>
    /// <param name="url">partial URL</param>
    /// <returns>MSecurityEntity or null</returns>
    public static MSecurityEntity GetProfileByUrl(string url)
    {
        MSecurityEntity mRetVal = null;
        var mResult = Profiles()
            .Where(mProfile => !mProfile.Name.Equals("no url", StringComparison.CurrentCultureIgnoreCase))
            .Where(mProfile => mProfile.Url.Replace("http:", "https:").Contains(url.Replace("http:", "https:"), StringComparison.CurrentCultureIgnoreCase))
            .OrderBy(mProfile => mProfile.Id)
            .Select(mProfile => mProfile);
        if(mResult.FirstOrDefault() != null)
        {
           mRetVal = mResult.FirstOrDefault();
        }
        return mRetVal;
    }

    /// <summary>
    /// Gets the registration information.
    /// </summary>
    /// <param name="securityEntityId"></param>
    /// <returns>MRegistrationInformation or null</returns>
    public static MRegistrationInformation GetRegistrationInformation(int securityEntityId)
    {
        MRegistrationInformation mRetVal = null;
        var mResult = RegistrationInformation()
            .Where(mProfile => mProfile.Id == securityEntityId)
            .OrderBy(mProfile => mProfile.Id)
            .Select(mProfile => mProfile);
        if(mResult.FirstOrDefault() != null)
        {
           mRetVal = mResult.FirstOrDefault();
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
        return getBusinessLogic().GetValidSecurityEntities(account, securityEntityId, isSystemAdmin);
    }

    public static int SaveProfile(MSecurityEntity profile)
    {
        string mEcryptedValue = string.Empty;
        CryptoUtility.TryEncrypt(profile.ConnectionString, out mEcryptedValue, profile.EncryptionType);
        profile.ConnectionString = mEcryptedValue;

        int mRetVal = getBusinessLogic().Save(profile);
        m_CacheHelper.RemoveFromCache(s_CacheName);
        return mRetVal;
    }

    public static MRegistrationInformation SaveRegistrationInformation(MRegistrationInformation profile)
    {
        MRegistrationInformation mRetVal = getBusinessLogic().SaveRegistrationInformation(profile);
        m_CacheHelper.RemoveFromCache(s_CacheRegistrationsName);
        return mRetVal;
    }

    [CLSCompliant(false)]
    public static void SetHttpContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        m_HttpContextAccessor = httpContextAccessor;
    }

    public static Collection<MRegistrationInformation> RegistrationInformation()
    {
        Collection<MRegistrationInformation> mRegistrationInformations = m_CacheHelper.GetFromCache<Collection<MRegistrationInformation>>(s_CacheRegistrationsName);
        if (mRegistrationInformations == null)
        {
            mRegistrationInformations = new Collection<MRegistrationInformation>();
            foreach (MRegistrationInformation mRegistrationInformation in getBusinessLogic(true).GetRegistrationInformation())
            {
                mRegistrationInformations.Add(mRegistrationInformation);
            }
            m_CacheHelper.AddToCache(s_CacheRegistrationsName, mRegistrationInformations);
        }
        return mRegistrationInformations;
    }

    public static Collection<MSecurityEntity> Profiles()
    {
        Collection<MSecurityEntity> mSecurityEntities = m_CacheHelper.GetFromCache<Collection<MSecurityEntity>>(s_CacheName);
        if (mSecurityEntities == null)
        {
            mSecurityEntities = new Collection<MSecurityEntity>();
            foreach (MSecurityEntity mSecurityEntity in getBusinessLogic(true).SecurityEntities())
            {
                // mSecurityEntity.ConnectionString = CryptoUtility.Decrypt(mSecurityEntity.ConnectionString, ConfigSettings.EncryptionType);
                string mDecryptedPassword;
                CryptoUtility.TryDecrypt(mSecurityEntity.ConnectionString, out mDecryptedPassword, ConfigSettings.EncryptionType);
                mSecurityEntity.ConnectionString = mDecryptedPassword;
                mSecurityEntities.Add(mSecurityEntity);
            }
            m_CacheHelper.AddToCache(s_CacheName, mSecurityEntities);
        }
        return mSecurityEntities;
    }
}
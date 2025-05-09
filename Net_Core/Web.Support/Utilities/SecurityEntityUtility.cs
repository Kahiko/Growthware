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
using System.Threading.Tasks;

namespace GrowthWare.Web.Support.Utilities;

public static class SecurityEntityUtility
{

    private static BSecurityEntities m_BusinessLogic = null;
    private static BSecurityEntities m_DefaultBusinessLogic = null;
    private static CacheHelper m_CacheHelper = CacheHelper.Instance();
    private static IHttpContextAccessor m_HttpContextAccessor = null;
    private static String s_CacheName = "Cached_SecurityEntities";
    private static string s_CacheRegistrationsName = "Cached_RegistrationInformations";

    /// <summary>
    /// Returns the current security entity based on the SecurityEntityFromUrl config setting.
    /// </summary>
    /// <returns>MSecurityEntity</returns>
    public static async Task<MSecurityEntity> CurrentProfile()
    {
        MSecurityEntity mRetProfile = null;
        if(!ConfigSettings.SecurityEntityFromUrl)
        {
            if (m_HttpContextAccessor != null)
            {
                MClientChoicesState mClientChoicesState = await ClientChoicesUtility.CurrentState();
                if (mClientChoicesState != null)
                {
                    int mSecurityEntity = int.Parse(mClientChoicesState[MClientChoices.SecurityEntityId].ToString(), CultureInfo.InvariantCulture);
                    mRetProfile = await GetProfile(mSecurityEntity);
                }
            }
        }
        else
        {
            if (m_HttpContextAccessor != null)
            {
                string mUrl = m_HttpContextAccessor.HttpContext.Request.Scheme + "://" + m_HttpContextAccessor.HttpContext.Request.Host.Host;
                mRetProfile = await GetProfileByUrl(mUrl);
                // TODO: Unsure if I should attempt to get the selected profile from ClientChoices
                // for now we'll get the default one I am not and just lettting the
                // the default profile to be returned
            }
        }
        mRetProfile ??= DefaultProfile();
        return mRetProfile;
    }

    /// <summary>
    /// Returns a security entity populated with the config settings.
    /// </summary>
    /// <returns>MSecurityEntity</returns>
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
    /// Deletes the registration information from the database and the cache.
    /// </summary>
    /// <param name="securityEntitySeqId"></param>
    /// <returns></returns>
    public static async Task DeleteRegistrationInformation(int securityEntitySeqId)
    {
        BSecurityEntities mBusinessLogic = await getBusinessLogic();
        await mBusinessLogic.DeleteRegistrationInformation(securityEntitySeqId);
        m_CacheHelper.RemoveFromCache(s_CacheRegistrationsName);
    }

    /// <summary>
    /// Returns the business logic object used to access the database.
    /// </summary>
    /// <returns></returns>
    private static async Task<BSecurityEntities> getBusinessLogic(bool useDefault = false)
    {
        if(!useDefault)
        {
            if(m_BusinessLogic == null || ConfigSettings.CentralManagement == true)
            {
                m_BusinessLogic = new(await SecurityEntityUtility.CurrentProfile());
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
    public static async Task<MSecurityEntity> GetProfile(string name)
    {
        Collection<MSecurityEntity> mProfiles = await Profiles();
        MSecurityEntity mRetVal = null;
        var mResult = from mProfile in mProfiles
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
    public static async Task<MSecurityEntity> GetProfile(int securityEntitySeqId)
    {
        MSecurityEntity mRetVal = null;
        Collection<MSecurityEntity> mProfiles = await Profiles();
        var mResult = from mProfile in mProfiles
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
    public static async Task<MSecurityEntity> GetProfileByUrl(string url)
    {
        MSecurityEntity mRetVal = null;
        Collection<MSecurityEntity> mProfiles = await Profiles();
        var mResult = mProfiles
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
    public static async Task<MRegistrationInformation> GetRegistrationInformation(int securityEntityId)
    {
        MRegistrationInformation mRetVal = null;
        Collection<MRegistrationInformation> mRegistrationInformations = await RegistrationInformation();
        var mResult = mRegistrationInformations
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
    public static async Task<DataTable> GetValidSecurityEntities(string account, int securityEntityId, bool isSystemAdmin)
    {
        BSecurityEntities mBusinessLogic = await getBusinessLogic();
        return mBusinessLogic.GetValidSecurityEntities(account, securityEntityId, isSystemAdmin);
    }

    public static async Task<int> SaveProfile(MSecurityEntity profile)
    {
        string mEcryptedValue = string.Empty;
        CryptoUtility.TryEncrypt(profile.ConnectionString, out mEcryptedValue, profile.EncryptionType);
        profile.ConnectionString = mEcryptedValue;
        BSecurityEntities mBusinessLogic = await getBusinessLogic();
        int mRetVal = mBusinessLogic.Save(profile);
        m_CacheHelper.RemoveFromCache(s_CacheName);
        return mRetVal;
    }

    /// <summary>
    /// Saves the registration information to the database and "updates" the cache.
    /// </summary>
    /// <param name="profile"></param>
    /// <returns></returns>
    public static async Task<MRegistrationInformation> SaveRegistrationInformation(MRegistrationInformation profile)
    {
        BSecurityEntities mBusinessLogic = await getBusinessLogic();
        MRegistrationInformation mRetVal = mBusinessLogic.SaveRegistrationInformation(profile);
        m_CacheHelper.RemoveFromCache(s_CacheRegistrationsName);
        return mRetVal;
    }

    /// <summary>
    /// Set the HttpContextAccessor used by the class.
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    [CLSCompliant(false)]
    public static void SetHttpContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        m_HttpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Gets a collection of MRegistrationInformation objects.
    /// </summary>
    /// <returns>Collection of MRegistrationInformation</returns>
    /// <remarks>
    /// The collection is cached. The cache is cleared when the SaveRegistrationInformation method is called.
    /// </remarks>
    public static async Task<Collection<MRegistrationInformation>> RegistrationInformation()
    {
        Collection<MRegistrationInformation> mRegistrationInformations = m_CacheHelper.GetFromCache<Collection<MRegistrationInformation>>(s_CacheRegistrationsName);
        if (mRegistrationInformations == null)
        {
            mRegistrationInformations = new Collection<MRegistrationInformation>();
            BSecurityEntities mBusinessLogic = await getBusinessLogic(true);
            foreach (MRegistrationInformation mRegistrationInformation in await mBusinessLogic.GetRegistrationInformation())
            {
                mRegistrationInformations.Add(mRegistrationInformation);
            }
            m_CacheHelper.AddToCache(s_CacheRegistrationsName, mRegistrationInformations);
        }
        return mRegistrationInformations;
    }

    /// <summary>
    /// Gets the collection of MSecurityEntity objects. The collection is cached. The cache is cleared when the Save method is called.
    /// </summary>
    /// <returns>Collection of MSecurityEntity</returns>
    /// <remarks>
    /// The returned collection contains the decrypted connection string.
    /// </remarks>
    public static async Task<Collection<MSecurityEntity>> Profiles()
    {
        Collection<MSecurityEntity> mSecurityEntities = m_CacheHelper.GetFromCache<Collection<MSecurityEntity>>(s_CacheName);
        if (mSecurityEntities == null)
        {
            mSecurityEntities = new Collection<MSecurityEntity>();
            BSecurityEntities mBusinessLogic = await getBusinessLogic(true);
            foreach (MSecurityEntity mSecurityEntity in mBusinessLogic.SecurityEntities())
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
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using GrowthWare.Framework;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;

namespace GrowthWare.Web.Support.Utilities;

public static class MessageUtility
{
    private static IHttpContextAccessor m_IHttpContextAccessor = null;

    private static string s_MessagesUnitCachedDVName = "dvMessages";

    private static string s_MessagesUnitCachedCollectionName = "MessagesCollection";

    /// <summary>
    /// Messages the name of the unit cached collection.
    /// </summary>
    /// <param name="securityEntityId">The security entity ID.</param>
    /// <returns>System.String.</returns>
    public static string MessagesUnitCachedCollectionName(int securityEntityId)
    {
        return securityEntityId.ToString(CultureInfo.InvariantCulture) + s_MessagesUnitCachedCollectionName;
    }

    /// <summary>
    /// Messages the name of the unit cached DV.
    /// </summary>
    /// <param name="securityEntityId">The security entity ID.</param>
    /// <returns>System.String.</returns>
    public static string MessagesUnitCachedDVName(int securityEntityId)
    {
        return securityEntityId.ToString(CultureInfo.InvariantCulture) + s_MessagesUnitCachedDVName;
    }

    /// <summary>
    /// Saves the specified profile.
    /// </summary>
    /// <param name="profile">The profile.</param>
    public static void Save(MMessage profile)
    {
        BMessages mBMessages = new BMessages(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
        mBMessages.Save(profile);
        RemoveCachedMessagesCollection();
    }

    [CLSCompliant(false)]
    public static void SetHttpContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        m_IHttpContextAccessor = httpContextAccessor;
    }    

    /// <summary>
    /// Gets the name of the message profile by.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>MMessage.</returns>
    public static MMessage GetProfile(string name)
    {
        var mResult = from mProfile in Messages()
                      where mProfile.Name.ToLower(CultureInfo.CurrentCulture) == name.ToLower(CultureInfo.CurrentCulture)
                      select mProfile;
        MMessage mRetVal = new MMessage();
        try
        {
            mRetVal = mResult.First();
        }
        catch (InvalidOperationException)
        {
            String mMSG = "Count not find function: " + name + " in the database";
            Logger mLog = Logger.Instance();
            mLog.Error(mMSG);
            mRetVal = null;
        }
        return mRetVal;
    }

    /// <summary>
    /// Gets the message profile by id.
    /// </summary>
    /// <param name="messageSeqId">The Message Sequence ID.</param>
    /// <returns>MMessage.</returns>
    public static MMessage GetProfile(int messageSeqId)
    {
        var mResult = from mProfile in Messages()
                      where mProfile.Id == messageSeqId
                      select mProfile;
        MMessage mRetVal = null;
        try
        {
            mRetVal = mResult.First();
        }
        catch (InvalidOperationException ex)
        {
            Logger mLog = Logger.Instance();
            mLog.Error(ex);
            mRetVal = null;
        }
        return mRetVal;
    }

    /// <summary>
    /// Gets the messages.
    /// </summary>
    /// <returns>Collection{MMessage}.</returns>
    public static Collection<MMessage> Messages()
    {
        MSecurityEntity mSecurityEntityProfile = SecurityEntityUtility.CurrentProfile();
        string mCacheName = MessagesUnitCachedCollectionName(mSecurityEntityProfile.Id);
        Collection<MMessage> mMessageCollection = null;
        // mMessageCollection = (Collection<MMessage>)HttpContext.Current.Cache[mCacheName];
        if (mMessageCollection == null)
        {
            BMessages mBMessages = new BMessages(mSecurityEntityProfile, ConfigSettings.CentralManagement);
            mMessageCollection = mBMessages.GetMessages(mSecurityEntityProfile.Id);
            CacheController.AddToCacheDependency(mCacheName, mMessageCollection);
        }
        return mMessageCollection;
    }

    /// <summary>
    /// Removes the cached messages DV.
    /// </summary>
    public static void RemoveCachedMessagesDV()
    {
        // int mySecurityEntity = ClientChoicesUtility.SelectedSecurityEntity();
        // CacheController.RemoveFromCache(MessagesUnitCachedDVName(mySecurityEntity));
    }

    /// <summary>
    /// Removes the cached messages collection.
    /// </summary>
    public static void RemoveCachedMessagesCollection()
    {
        // int mySecurityEntity = ClientChoicesUtility.SelectedSecurityEntity();
        // CacheController.RemoveFromCache(MessagesUnitCachedCollectionName(mySecurityEntity));
        RemoveCachedMessagesDV();
    }

    /// <summary>
    /// Searches the specified search criteria.
    /// </summary>
    /// <param name="searchCriteria">The search criteria.</param>
    /// <returns>DataTable.</returns>
    // public static DataTable Search(MSearchCriteria searchCriteria)
    // {
    //     try
    //     {
    //         BMessages mBMessages = new BMessages(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
    //         return mBMessages.Search(searchCriteria);
    //     }
    //     catch (IndexOutOfRangeException ex)
    //     {
    //         Logger mLog = Logger.Instance();
    //         mLog.Debug(ex);
    //         return null;
    //     }
    // }

}
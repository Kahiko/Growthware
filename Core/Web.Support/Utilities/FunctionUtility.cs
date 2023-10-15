using Microsoft.AspNetCore.Http;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using System.Collections.Generic;
using System.Data;

namespace GrowthWare.Web.Support.Utilities;

public static class FunctionUtility
{
    private static CacheController m_CacheController = CacheController.Instance();
    private static IHttpContextAccessor m_IHttpContextAccessor = null;
    private static List<UIKeyValuePair> m_FunctionTypes = null;

    /// <summary>
    /// Deletes a function from the database.
    /// </summary>
    /// <param name="functionSeqId">int</param>
    public static void Delete(int functionSeqId)
    {
        MSecurityEntity mSecurityEntityProfile = SecurityEntityUtility.CurrentProfile();
        BFunctions mBFunctions = new BFunctions(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
        mBFunctions.Delete(functionSeqId);
        String mCacheName = mSecurityEntityProfile.Id.ToString(CultureInfo.InvariantCulture) + "_Functions";        
        m_CacheController.RemoveFromCache(mCacheName);
    }

    /// <summary>
    /// Retrieves all functions from the either the database or cache
    /// </summary>
    /// <returns>A Collection of MFunctionProfiles</returns>
    [CLSCompliant(false)]
    public static Collection<MFunctionProfile> Functions()
    {
        // TODO: Cache has not been implemented
        MSecurityEntity mSecurityEntityProfile = SecurityEntityUtility.CurrentProfile();
        String mCacheName = mSecurityEntityProfile.Id.ToString(CultureInfo.InvariantCulture) + "_Functions";
        Collection<MFunctionProfile> mRetVal = m_CacheController.GetFromCache<Collection<MFunctionProfile>>(mCacheName);
        if (mRetVal == null)
        {
            BFunctions mBFunctions = new BFunctions(mSecurityEntityProfile, ConfigSettings.CentralManagement);
            mRetVal = mBFunctions.GetFunctions(mSecurityEntityProfile.Id);
            m_CacheController.AddToCache(mCacheName, mRetVal);
        }
        return mRetVal;
    }

    /// <summary>
    /// Retrieves all of the avalible parents
    /// </summary>
    /// <returns>List<UIKeyValuePair></returns>
    public static List<UIKeyValuePair> GetAvalibleParents()
    {
        List<UIKeyValuePair> mRetVal = Functions().Where(item => item.IsNavigable).Select(item => new UIKeyValuePair {
            Key = item.Id ,
            Value = item.Name
        }).OrderBy(item => item.Value).ToList();

        return mRetVal;
    }

    /// <summary>
    /// Retrieves the menu order for the given function
    /// </summary>
    /// <param name="functionSeqId"></param>
    /// <returns>List<UIFunctionMenuOrder></returns>
    public static List<UIFunctionMenuOrder> GetFunctionOrder(int functionSeqId)
    {
        MSecurityEntity mSecurityEntityProfile = SecurityEntityUtility.CurrentProfile();
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(functionSeqId);
        BFunctions mBFunctions = new BFunctions(mSecurityEntityProfile, ConfigSettings.CentralManagement);
        DataTable mDataTable = mBFunctions.GetMenuOrder(mFunctionProfile);
        List<UIFunctionMenuOrder> mRetVal = null;
        mRetVal = mDataTable.AsEnumerable().Select(item => new UIFunctionMenuOrder {
            Function_Seq_Id = int.Parse(item["FUNCTION_SEQ_ID"].ToString()) ,
            Action = item["Action"].ToString(),
            Name = item["Name"].ToString()
        }).ToList();

        return mRetVal;
    }

    /// <summary>
    /// Retrieves all function types
    /// </summary>
    /// <returns>List<UIKeyValuePair></returns>
    public static List<UIKeyValuePair> GetFunctionTypes()
    {
        if(m_FunctionTypes == null) 
        {
            MSecurityEntity mSecurityEntityProfile = SecurityEntityUtility.CurrentProfile();
            BFunctions mBFunctions = new BFunctions(mSecurityEntityProfile, ConfigSettings.CentralManagement);
            DataTable mDataTable = mBFunctions.FunctionTypes();
            m_FunctionTypes = mDataTable.AsEnumerable().Select(item => new UIKeyValuePair {
                Key = int.Parse(item["FUNCTION_TYPE_SEQ_ID"].ToString()) ,
                Value = item["Name"].ToString()
            }).ToList() ;

        }
        return m_FunctionTypes;
    }

    /// <summary>
    /// Retrieves a MFunctionProfile given the action
    /// </summary>
    /// <param name="action">string</param>
    /// <returns>MFunctionProfile</returns>
    [CLSCompliant(false)]
    public static MFunctionProfile GetProfile(String action)
    {
        MFunctionProfile mRetVal = null;
        if (!string.IsNullOrEmpty(action))
        {
            var mResult = from mProfile in Functions()
                          where mProfile.Action.ToLower(CultureInfo.CurrentCulture) == action.ToLower(CultureInfo.CurrentCulture)
                          select mProfile;
            mRetVal = new MFunctionProfile();
            try
            {
                mRetVal = mResult.First();
            }
            catch (InvalidOperationException)
            {
                mRetVal = null;
            }
        }
        return mRetVal;
    }

    /// <summary>
    /// Retrieves a MFunctionProfile given the functionSeqId
    /// </summary>
    /// <param name="functionSeqId">int</param>
    /// <returns>MFunctionProfile</returns>
    public static MFunctionProfile GetProfile(int functionSeqId)
    {
        MFunctionProfile mRetVal = null;
        var mResult = from mProfile in Functions()
                        where mProfile.Id == functionSeqId
                        select mProfile;
        mRetVal = new MFunctionProfile();
        try
        {
            mRetVal = mResult.First();
        }
        catch (InvalidOperationException)
        {
            mRetVal = new MFunctionProfile();
        }
        return mRetVal;
    }

    /// <summary>
    /// Saves the given MFunctionProfile to the database
    /// </summary>
    /// <param name="profile">MFunctionProfile</param>
    /// <param name="saveGroups">bool used to indicate if groups should be saved</param>
    /// <param name="saveRoles">bool used to indicate if roles should be saved</param>
    /// <returns>The Id representing the updated/inserted function</returns>
    public static int Save(MFunctionProfile profile, bool saveGroups, bool saveRoles)
    {
        BFunctions mBFunctions = new BFunctions(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
        int mRetVal = mBFunctions.Save(profile, saveGroups, saveRoles);
        String mCacheName = SecurityEntityUtility.CurrentProfile().Id.ToString(CultureInfo.InvariantCulture) + "_Functions";
        m_CacheController.RemoveFromCache(mCacheName);
        return mRetVal;
    }

    /// <summary>
    /// Set the HttpContextAccessor for the application.
    /// </summary>
    /// <param name="httpContextAccessor">An instance of IHttpContextAccessor to be set as the HttpContextAccessor.</param>
    [CLSCompliant(false)]
    public static void SetHttpContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        m_IHttpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Updates the sort order fot the given functions in the commaSeparated_Ids.
    /// </summary>
    /// <param name="commaSeparated_Ids">A comma separated list of ids</param>
    /// <param name="profile">The profile.</param>
    public static void UpdateMenuOrder(string commaseparated_Ids, MFunctionProfile profile)
    {
        BFunctions mBFunctions = new BFunctions(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
        mBFunctions.UpdateMenuOrder(commaseparated_Ids, profile);
    }
}
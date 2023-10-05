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
using Microsoft.AspNetCore.Http.HttpResults;

namespace GrowthWare.Web.Support.Utilities;

public static class FunctionUtility
{
    private static IHttpContextAccessor m_IHttpContextAccessor = null;
    private static List<UIKeyValuePair> m_FunctionTypes = null;

    /// <summary>
    /// Retrieves all functions from the either the database or cache
    /// </summary>
    /// <returns>A Collection of MFunctionProfiles</returns>
    [CLSCompliant(false)]
    public static Collection<MFunctionProfile> Functions()
    {
        MSecurityEntity mSecurityEntityProfile = SecurityEntityUtility.CurrentProfile();
        MClientChoicesState mClientChoicesState =  (MClientChoicesState)m_IHttpContextAccessor.HttpContext.Items["ClientChoicesState"];
        if (mClientChoicesState != null && mClientChoicesState[MClientChoices.SecurityEntityID] != null) 
        {
            int mSecurityEntityID = int.Parse(mClientChoicesState[MClientChoices.SecurityEntityID]);
            mSecurityEntityProfile = SecurityEntityUtility.GetProfile(mSecurityEntityID);
        }
        BFunctions mBFunctions = new BFunctions(mSecurityEntityProfile, ConfigSettings.CentralManagement);
        String mCacheName = mSecurityEntityProfile.Id.ToString(CultureInfo.InvariantCulture) + "_Functions";
        Collection<MFunctionProfile> mRetVal = null;
        // mRetVal = (Collection<MFunctionProfile>)(context.Cache[mCacheName]);
        if (mRetVal == null)
        {
            mRetVal = mBFunctions.GetFunctions(mSecurityEntityProfile.Id);
            // CacheController.AddToCacheDependency(mCacheName, mRetVal);
        }
        return mRetVal;
    }

    public static List<UIKeyValuePair> GetAvalibleParents()
    {
        List<UIKeyValuePair> mRetVal = Functions().Select(item => new UIKeyValuePair {
            Key = item.FunctionTypeSeqId ,
            Value = item.Name
        }).ToList();

        return mRetVal;
    }

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

    [CLSCompliant(false)]
    public static void SetHttpContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        m_IHttpContextAccessor = httpContextAccessor;
    }
}
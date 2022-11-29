using Microsoft.AspNetCore.Http;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;

namespace GrowthWare.WebSupport.Utilities;

public static class FunctionUtility
{

    /// <summary>
    /// Retrieves all functions from the either the database or cache
    /// </summary>
    /// <returns>A Collection of MFunctionProfiles</returns>
    [CLSCompliant(false)]
    public static Collection<MFunctionProfile> Functions(HttpContext httpContext)
    {
        MSecurityEntity mSecurityEntityProfile = SecurityEntityUtility.CurrentProfile();
        MClientChoicesState mClientChoicesState =  (MClientChoicesState)httpContext.Items["ClientChoicesState"];
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

    [CLSCompliant(false)]
    public static MFunctionProfile GetProfile(String action, HttpContext context)
    {
        MFunctionProfile mRetVal = null;
        if (!string.IsNullOrEmpty(action))
        {
            var mResult = from mProfile in Functions(context)
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
}
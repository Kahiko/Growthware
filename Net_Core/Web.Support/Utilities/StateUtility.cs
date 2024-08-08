using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Web.Support.Helpers;

namespace GrowthWare.Web.Support.Utilities;
public static class StateUtility
{
    private static BStates m_BusinessLogic = null;
    private static CacheHelper m_CacheHelper = CacheHelper.Instance();
    private static string m_CacheName = "States";

    /// <summary>
    /// Returns the business logic object used to access the database.
    /// </summary>
    /// <returns></returns>
    private static BStates getBusinessLogic()
    {
        if(m_BusinessLogic == null || ConfigSettings.CentralManagement == true)
        {
            m_BusinessLogic = new(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement);
        }
        return m_BusinessLogic;
    }

    private static Collection<MState> States()
    {
        Collection<MState> mRetVal = m_CacheHelper.GetFromCache<Collection<MState>>(m_CacheName);
        if(mRetVal == null)
        {
            mRetVal = getBusinessLogic().GetStates();
            m_CacheHelper.AddToCache(m_CacheName, mRetVal);
        }
        
        return mRetVal;
    }

    public static MState GetState(string state)
    {
        MState mRetVal = null;
        if (!string.IsNullOrEmpty(state))
        {
            var mResult = from mProfile in States()
                          where mProfile.State.ToLower(CultureInfo.CurrentCulture) == state.ToLower(CultureInfo.CurrentCulture)
                          select mProfile;
            mRetVal = new MState();
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

    public static void Save(MState state)
    {
        getBusinessLogic().Save(state);
        m_CacheHelper.RemoveFromCache(m_CacheName);
    }

}
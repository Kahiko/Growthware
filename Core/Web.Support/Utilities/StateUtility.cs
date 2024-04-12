using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;

namespace GrowthWare.Web.Support.Utilities;
public static class StateUtility
{
    private static CacheController m_CacheController = CacheController.Instance();
    private static string m_CacheName = "States";

    private static Collection<MState> States()
    {
        Collection<MState> mRetVal = m_CacheController.GetFromCache<Collection<MState>>(m_CacheName);
        if(mRetVal == null)
        {
            BStates mBStates = new BStates(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement);
            mRetVal = mBStates.GetStates();
            m_CacheController.AddToCache(m_CacheName, mRetVal);
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
        BStates mBStates = new BStates(SecurityEntityUtility.CurrentProfile, ConfigSettings.CentralManagement);
        mBStates.Save(state);
        m_CacheController.RemoveFromCache(m_CacheName);
    }

}
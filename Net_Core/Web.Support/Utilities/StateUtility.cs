using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
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
    private static async Task<BStates> getBusinessLogic()
    {
        if(m_BusinessLogic == null || ConfigSettings.CentralManagement == true)
        {
            m_BusinessLogic = new(await SecurityEntityUtility.CurrentProfile());
        }
        return m_BusinessLogic;
    }

    private static async Task<Collection<MState>> States()
    {
        Collection<MState> mRetVal = m_CacheHelper.GetFromCache<Collection<MState>>(m_CacheName);
        if(mRetVal == null)
        {
            BStates mBusinessLogic = await getBusinessLogic();
            mRetVal = await mBusinessLogic.GetStates();
            m_CacheHelper.AddToCache(m_CacheName, mRetVal);
        }
        
        return mRetVal;
    }

    public static async Task<MState> GetState(string state)
    {
        MState mRetVal = null;
        if (!string.IsNullOrEmpty(state))
        {
            Collection<MState> mStates = await States();
            var mResult = from mProfile in mStates
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

    public static async Task Save(MState state)
    {
        BStates mBusinessLogic = await getBusinessLogic();
        await mBusinessLogic.Save(state);
        m_CacheHelper.RemoveFromCache(m_CacheName);
    }
}

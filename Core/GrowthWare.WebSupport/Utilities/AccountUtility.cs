using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using System;

namespace GrowthWare.WebSupport.Utilities;
public static class AccountUtility
{
    public static MAccountProfile GetAccount(String account)
    {
            BAccounts mBAccount = new BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            MAccountProfile mRetVal = null;
            try
            {
                mRetVal = mBAccount.GetProfile(account);
            }
            catch (InvalidOperationException)
            {
                String mMSG = "Count not find account: " + account + " in the database";
                // Logger mLog = Logger.Instance();
                // mLog.Error(mMSG);
            }
            catch (IndexOutOfRangeException) 
            {
                String mMSG = "Count not find account: " + account + " in the database";
                // Logger mLog = Logger.Instance();
                // mLog.Error(mMSG);
            }
            return mRetVal;
    }
}

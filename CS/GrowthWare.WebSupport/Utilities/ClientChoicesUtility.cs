using GrowthWare.Framework.BusinessData.BusinessLogicLayer;
using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using System;
using System.Globalization;
using System.Web;

namespace GrowthWare.WebSupport.Utilities
{
    /// <summary>
    /// ClientChoicesUtility serves as the focal point for any web application needing to utiltize the GrowthWare framework
    /// with regards to ClientChoices.
    /// </summary>
    public static class ClientChoicesUtility
    {
        private static String s_CachedAnonymousChoicesState = "AnonymousClientChoicesState";

        /// <summary>
        /// Returns the client choices given the account
        /// </summary>
        /// <param name="account">String</param>
        /// <returns>MClientChoicesState</returns>
        public static MClientChoicesState GetClientChoicesState(String account)
        {
            return GetClientChoicesState(account, false);
        }

        /// <summary>
        /// Gets the state of the client choices.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="fromDB">if set to <c>true</c> [from database].</param>
        /// <returns>MClientChoicesState.</returns>
        public static MClientChoicesState GetClientChoicesState(String account, bool fromDB)
        {
            MClientChoicesState mRetVal = null;
            MSecurityEntityProfile mSecurityEntityProfile = SecurityEntityUtility.GetDefaultProfile();
            BClientChoices mBClientChoices = new BClientChoices(mSecurityEntityProfile, ConfigSettings.CentralManagement);
            if (fromDB)
            {
                return mBClientChoices.GetClientChoicesState(account);
            }
            if (HttpContext.Current.Cache != null)
            {
                mRetVal = (MClientChoicesState)(HttpContext.Current.Cache[MClientChoices.SessionName]);
            }
            if (mRetVal == null)
            {
                if (account.Trim().ToLower(CultureInfo.CurrentCulture) == "anonymous")
                {
                    mRetVal = (MClientChoicesState)HttpContext.Current.Cache[s_CachedAnonymousChoicesState];
                    if (mRetVal == null)
                    {
                        mRetVal = mBClientChoices.GetClientChoicesState(account);
                        CacheController.AddToCacheDependency(s_CachedAnonymousChoicesState, mRetVal);
                    }
                }
                else
                {
                    mRetVal = mBClientChoices.GetClientChoicesState(account);
                }
            }
            else
            {
                if (mRetVal.AccountName != account)
                {
                    mRetVal = mBClientChoices.GetClientChoicesState(account);
                }
            }
            if (HttpContext.Current.Cache != null)
            {
                HttpContext.Current.Cache[MClientChoices.SessionName] = mRetVal;
            }
            return mRetVal;
        }

        /// <summary>
        /// Gets the selected security entity.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public static int GetSelectedSecurityEntity()
        {
            MClientChoicesState myClientChoicesState = (MClientChoicesState)HttpContext.Current.Items[MClientChoices.SessionName];
            if ((myClientChoicesState != null))
            {
                return int.Parse(myClientChoicesState[MClientChoices.SecurityEntityId], CultureInfo.InvariantCulture);
            }
            else
            {
                return ConfigSettings.DefaultSecurityEntityId;
            }
        }

        /// <summary>
        /// Save the client choices to the database.
        /// </summary>
        /// <param name="clientChoicesState">MClientChoicesState</param>
        /// <remarks></remarks>
        public static void Save(MClientChoicesState clientChoicesState)
        {
            MSecurityEntityProfile mSecurityEntityProfile = SecurityEntityUtility.GetDefaultProfile();
            BClientChoices mBClientChoices = new BClientChoices(mSecurityEntityProfile, ConfigSettings.CentralManagement);
            mBClientChoices.Save(clientChoicesState);
            if (HttpContext.Current.Cache != null)
            {
                HttpContext.Current.Cache[MClientChoices.SessionName] = clientChoicesState;
            }
        }
    }
}

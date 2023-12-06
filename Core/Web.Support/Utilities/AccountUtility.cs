using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Enumerations;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;

namespace GrowthWare.Web.Support.Utilities;
public static class AccountUtility
{
    /*
     * TODO: Authenticate should be the only place where the cache/session account is ever touchted.  Logoff should authenticate the Anonymous account so that the session/cached account
     * is only changed and never removed
     */
    private static string s_Anonymous = "Anonymous";
    private static string s_CachedName = "CachedAnonymous";
    private static CacheController m_CacheController = CacheController.Instance();
    private static int[] m_InvalidStatus = { (int)SystemStatus.Disabled, (int)SystemStatus.Inactive };
    private static string s_SessionName = "SessionAccount";

    public static string AnonymousAccount { get { return s_Anonymous; } }

    public static string SessionName { get { return s_SessionName; } }

    /// <summary>
    /// Adds or updates a value in the cache or session.
    /// </summary>
    /// <param name="forAccount"></param>
    /// <param name="value"></param>
    private static void addOrUpdateCacheOrSession(string forAccount, object value, string sessionName = "useDefault")
    {
        string mSessionName = s_SessionName;
        if(sessionName != "useDefault") { mSessionName = sessionName; }
        if (!forAccount.Equals(s_Anonymous, StringComparison.InvariantCultureIgnoreCase))
        {
            SessionController.AddToSession(mSessionName, value);
            return;
        }        
        m_CacheController.AddToCache(s_CachedName, value);
    }

    /// <summary>
    /// Performs the authentication logic
    /// </summary>
    /// <param name="account"></param>
    /// <param name="password"></param>
    /// <param name="ipAddress"></param>
    /// <returns>MAccountProfile or null</returns>
    public static MAccountProfile Authenticate(string account, string password, string ipAddress)
    {
        /*
         *  1.) Don't save the anonymous account
         *  2.) Ensure the account exists
         *  3.) Check account Status
         *  4.) Retrieve the account from the database
         *  5.) Determine authentication method (Password or LDAP)
         *      a.) LDAP or Proprietary authentication
         *  6.) If authentication is successful
         *      a.) Set the tokens on the returned profile
         *      b.) Set LastLogOn
         *      c.) Set FailedAttempts = 0
         *  7.) If authentication is not successful
         *      a.) Set FailedAttempts++
         *          1.) If FailedAttempts >= MaxFailedAttempts then Set Status = (int)SystemStatus.Disabled
         */
        if (string.IsNullOrEmpty(account)) throw new ArgumentNullException("account", "account cannot be a null reference (Nothing in VB) or empty!");
        if (string.IsNullOrEmpty(account)) throw new ArgumentNullException("password", "password cannot be a null reference (Nothing in VB) or empty!");
        string mAccount = account;  // It's good practice to leave parameters unchanged.
        bool mIsAuthenticated = false;
        MAccountProfile mRetVal = null;
        // No need to save the anonymous account
        if (account.Equals(s_Anonymous, StringComparison.InvariantCultureIgnoreCase))
        {
            mRetVal = getAccountProfile(s_Anonymous);
            return mRetVal;
        }
        mRetVal = getAccountProfile(account, true);
        if (mRetVal == null)
        {
            return mRetVal;
        }
        if (!m_InvalidStatus.Contains(mRetVal.Status))
        {
            if (ConfigSettings.AuthenticationType.Equals("internal", StringComparison.InvariantCultureIgnoreCase))
            {
                // Proprietary authentication
                string mProfilePassword = string.Empty;
                CryptoUtility.TryDecrypt(mRetVal.Password, out mProfilePassword, ConfigSettings.EncryptionType);
                mIsAuthenticated = password == mProfilePassword;
                if (mIsAuthenticated)
                {
                    mIsAuthenticated = mRetVal.FailedAttempts < ConfigSettings.FailedAttempts + 1 && mRetVal.Status != (int)SystemStatus.Disabled;
                }
            }
            else
            {
                // TODO: Add LDAP authentication
            }
            if (!mIsAuthenticated)
            {
                mRetVal.FailedAttempts++;
                if (mRetVal.FailedAttempts >= ConfigSettings.FailedAttempts + 1)
                {
                    mRetVal.Status = (int)SystemStatus.Disabled;
                }
                Save(mRetVal, true, false, false);
                return null;
            }
            // setup tokens, claims and what not
            mRetVal = TokenUtility.SetTokens(mRetVal, ipAddress);
            mRetVal.FailedAttempts = 0;
            mRetVal.LastLogOn = DateTime.Now;
            Save(mRetVal, true, false, false);
            ClientChoicesUtility.SynchronizeContext(mRetVal.Account);
            mRetVal.Password = ""; // Don't want to ever send the password out
        }
        return mRetVal;
    }

    /// <summary>
    /// ChangePassword function takes in a UIChangePassword object as a parameter and returns a string.
    /// </summary>
    /// <param name="changePassword">UIChangePassword</param>
    /// <returns></returns>
    public static string ChangePassword(UIChangePassword changePassword)
    {
        string mRetVal = string.Empty;
        MMessage mMessageProfile = new MMessage();
        MAccountProfile mAccountProfile = CurrentProfile;
        MSecurityEntity mSecurityEntity = SecurityEntityUtility.CurrentProfile();
        string mCurrentPassword = mAccountProfile.Password;
        CryptoUtility.TryDecrypt(mAccountProfile.Password, out mCurrentPassword, mSecurityEntity.EncryptionType);
        if (mAccountProfile.Status != (int)SystemStatus.ChangePassword)
        {
            if (changePassword.OldPassword == mCurrentPassword)
            {
                mAccountProfile.PasswordLastSet = System.DateTime.Now;
                mAccountProfile.Status = (int)SystemStatus.Active;
                mAccountProfile.FailedAttempts = 0;
                // mAccountProfile.Password = CryptoUtility.Encrypt(changePassword.NewPassword.Trim(), mSecurityEntity.EncryptionType, ConfigSettings.EncryptionSaltExpression);
                string mEncryptedPassword;
                CryptoUtility.TryEncrypt(changePassword.NewPassword, out mEncryptedPassword, mSecurityEntity.EncryptionType, ConfigSettings.EncryptionSaltExpression);
                mAccountProfile.Password = mEncryptedPassword;
                try
                {
                    Save(mAccountProfile, false, false, false);
                    mMessageProfile = MessageUtility.GetProfile("SuccessChangePassword");
                }
                catch (System.Exception)
                {
                    mMessageProfile = MessageUtility.GetProfile("UnSuccessChangePassword");
                }
            }
            else
            {
                mMessageProfile = MessageUtility.GetProfile("PasswordNotMatched");
            }
        }
        else
        {
            try
            {
                mAccountProfile.PasswordLastSet = System.DateTime.Now;
                mAccountProfile.Status = (int)SystemStatus.Active;
                mAccountProfile.FailedAttempts = 0;
                string mEncryptedPassword;
                CryptoUtility.TryEncrypt(changePassword.NewPassword, out mEncryptedPassword, mSecurityEntity.EncryptionType, ConfigSettings.EncryptionSaltExpression);
                mAccountProfile.Password = mEncryptedPassword;
                try
                {
                    Save(mAccountProfile, false, false, false);
                    mMessageProfile = MessageUtility.GetProfile("SuccessChangePassword");
                }
                catch (System.Exception)
                {
                    mMessageProfile = MessageUtility.GetProfile("UnSuccessChangePassword");
                }
            }
            catch (Exception)
            {
                mMessageProfile = MessageUtility.GetProfile("UnSuccessChangePassword");
            }
        }
        //AccountUtility.RemoveInMemoryInformation(true);
        mRetVal = mMessageProfile.Body;
        return mRetVal;
    }
    
    /// <summary>
    /// Deletes an account with the specified accountSeqId.
    /// </summary>
    /// <param name="accountSeqId"></param>
    public static void Delete(int accountSeqId)
    {
        // TODO: It may be worth being able to get an account from the Id so we can get the name
        // and remove the any in memory information for the account.
        // This is not necessary for now b/c you can't delete the your own account.
        BAccounts mBAccount = new BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
        mBAccount.Delete(accountSeqId);
    }

    /// <summary>
    /// Returns a MAccountProfile given the account.
    /// </summary>
    /// <param name="account"></param>
    /// <returns>MAccountProfile or null</returns>
    public static MAccountProfile GetAccount(string account, bool forceDb = false)
    {
        MAccountProfile mRetVal = CurrentProfile;
        bool mAddToSessionOrCache = mRetVal == null;
        if (mRetVal == null || (!mRetVal.Account.Equals(account, StringComparison.InvariantCultureIgnoreCase)))
        {
            mRetVal = getAccountProfile(account, true);
            if (mAddToSessionOrCache && mRetVal != null)
            {
                // addOrUpdateCacheOrSession(string forAccount, object value, string sessionName = "SessionAccount")
                addOrUpdateCacheOrSession(account, mRetVal);
            }
        }
        return mRetVal;
    }

    /// <summary>
    /// Attempts to return the account from session or cache the stored value account is not equal to the requestd account
    /// the value will be retrieved from the database.
    /// </summary>
    /// <param name="account"></param>
    /// <param name="forceDb"></param>
    /// <returns>MAccountProfile or null</returns>
    private static MAccountProfile getAccountProfile(string account, bool forceDb = false)
    {
        MAccountProfile mRetVal = null;
        BAccounts mBAccount = null;
        bool mAddToSessionOrCache = false;
        if (forceDb)
        {
            mBAccount = new BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            mRetVal = mBAccount.GetProfile(account);
        }
        else
        {
            mRetVal = CurrentProfile;
            mAddToSessionOrCache = mRetVal == null;
            if (mRetVal == null || (!mRetVal.Account.Equals(account, StringComparison.InvariantCultureIgnoreCase)))
            {
                mBAccount = new BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
                mRetVal = mBAccount.GetProfile(account);
            }
        }
        if (mAddToSessionOrCache && mRetVal != null)
        {
            // addOrUpdateCacheOrSession(string forAccount, object value, string sessionName = "SessionAccount")
            addOrUpdateCacheOrSession(account, mRetVal);
        }
        return mRetVal;
    }

    /// <summary>
    /// Retrieves the current account profile.
    /// </summary>
    /// <returns>MAccountProfile</returns>
    public static MAccountProfile CurrentProfile
    {
        get
        {
            /*
             *  1.) Attempt to get account from session
             *  2.) Attempt to get account from cache if the return value is null
             *  3.) If the return value is null the get the Anonymous account from the DB
             *      and add it to cache.
             */
            // getFromCacheOrSession<T>(string forAccount, string sessionName = "SessionAccount")
            MAccountProfile mRetVal = getFromCacheOrSession<MAccountProfile>("not_anonymous") ?? getFromCacheOrSession<MAccountProfile>(s_Anonymous);
            if (mRetVal == null) 
            {
                mRetVal = getAccountProfile(s_Anonymous, true);
                // addOrUpdateCacheOrSession(string forAccount, object value, string sessionName = "SessionAccount")
                addOrUpdateCacheOrSession(s_Anonymous, mRetVal);
            }
            return mRetVal;
        }
    }

    /// <summary>
    /// Retrieves an object of type `T` from either the cache or the session, based on the given `name`.
    /// </summary>
    /// <typeparam name="T">The type of the object being retrieved.</typeparam>
    /// <param name="name">The name of the value to retrieved.</param>
    /// <returns></returns>
    private static T getFromCacheOrSession<T>(string forAccount, string sessionName = "SessionAccount")
    {
        if (!forAccount.Equals(s_Anonymous, StringComparison.InvariantCultureIgnoreCase))
        {
            return SessionController.GetFromSession<T>(sessionName);
        }
        return m_CacheController.GetFromCache<T>(s_CachedName);
    }

    public static string GetMenuData(string account, MenuType menuType)
    {
        if (string.IsNullOrEmpty(account)) throw new ArgumentNullException("account", "account cannot be a null reference (Nothing in VB) or empty!");
        string mMenuName = menuType.ToString() + "_" + account + "_Menu_Data";
        string mRetVal = getFromCacheOrSession<string>(account, mMenuName);
        if (mRetVal != default)
        {
            return mRetVal;
        }
        BAccounts mBAccount = new BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
        DataTable mDataTable = mBAccount.GetMenu(account, menuType);
        if(mDataTable != null) 
        {
            mRetVal = DataHelper.GetJsonStringFromTable(ref mDataTable);
            addOrUpdateCacheOrSession(account, mRetVal, mMenuName);
        }
        return mRetVal;
    }

    /// <summary>
    /// Retrieves the list of menu items for a given account and menu type.
    /// </summary>
    /// <param name="account">The account for which to retrieve the menu items.</param>
    /// <param name="menuType"></param>
    /// <returns>The list of menu items for the specified account and menu type.</returns>
    /// <exception cref="ArgumentNullException">he type of menu (e.g., Hierarchical, Horizontal, or Vertical) to retrieve the menu items for.</exception>
    public static IList<MMenuTree> GetMenuItems(string account, MenuType menuType)
    {
        if (string.IsNullOrEmpty(account)) throw new ArgumentNullException("account", "account cannot be a null reference (Nothing in VB) or empty!");
        IList<MMenuTree> mRetVal = null;
        string mMenuName = menuType.ToString() + "_" + account + "_Menu";
        // getFromCacheOrSession<T>(string forAccount, string sessionName = "SessionAccount")
        mRetVal = getFromCacheOrSession<IList<MMenuTree>>(account, mMenuName);
        if (mRetVal != default)
        {
            return mRetVal;
        }
        mRetVal = new List<MMenuTree>();
        BAccounts mBAccount = new BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
        DataTable mDataTable = null;
        mDataTable = mBAccount.GetMenu(account, menuType);
        if (mDataTable != null && mDataTable.Rows.Count > 0)
        {
            mRetVal = MMenuTree.GetFlatList(mDataTable);
            if (menuType == MenuType.Hierarchical)
            {
                mRetVal = MMenuTree.FillRecursive(MMenuTree.GetFlatList(mDataTable), 0);
            }
        }
        // addOrUpdateCacheOrSession(string forAccount, object value, string sessionName = "SessionAccount")
        addOrUpdateCacheOrSession(account, mRetVal, mMenuName);
        return mRetVal;
    }

    /// <summary>
    /// Removes an object from either the cache or the session, based on the given `forAccount`.
    /// </summary>
    /// <param name="forAccount"></param>
    /// <param name="sessionName">Optional if not specified the default value is "useDefault"</param>
    public static void RemmoveFromCacheOrSession(string forAccount, string sessionName = "useDefault")
    {
        string mSessionName = s_SessionName;
        if(sessionName != "useDefault") { mSessionName = sessionName; }
        if (!forAccount.Equals(s_Anonymous, StringComparison.InvariantCultureIgnoreCase))
        {
            SessionController.RemoveFromSession(mSessionName);
            return;
        }
        m_CacheController.RemoveFromCache(s_CachedName);
    }

    /// <summary>
    /// Removes thhe menu and or account information from the session for the given account.
    /// </summary>
    /// <param name="forAccount"></param>
    /// /// <param name="includeAccount">By default this parameter is true and the account will be removed from the session.</param>
    public static void RemoveInMemoryInformation(string forAccount, bool includeAccount = true) 
    {
        if(includeAccount)
        {
            RemmoveFromCacheOrSession(forAccount);
        }
        foreach (MenuType mMenuType in Enum.GetValues(typeof(MenuType)))
        {
            string mMenuName = mMenuType.ToString() + "_" + forAccount + "_Menu";
            RemmoveFromCacheOrSession(forAccount, mMenuName);
        }        
    }

    /// <summary>
    /// Inserts or updates account information
    /// </summary>
    /// <param name="accountProfile">MAccountProfile</param>
    /// <param name="saveRefreshTokens">Boolean</param>
    /// <param name="saveRoles">Boolean</param>
    /// <param name="saveGroups">Boolean</param>
    /// <remarks>Changes will be reflected in the profile passed as a reference.</remarks>
    public static MAccountProfile Save(MAccountProfile accountProfile, bool saveRefreshTokens, bool saveRoles, bool saveGroups)
    {
        if (accountProfile == null) throw new ArgumentNullException("accountProfile", "accountProfile cannot be a null reference (Nothing in VB) or empty!");
        MSecurityEntity mSecurityEntity = SecurityEntityUtility.CurrentProfile();
        BAccounts mBAccount = new BAccounts(mSecurityEntity, ConfigSettings.CentralManagement);
        mBAccount.Save(accountProfile, saveRefreshTokens, saveRoles, saveGroups);
        if (accountProfile.Account != CurrentProfile.Account) 
        {
            RemoveInMemoryInformation(accountProfile.Account);
        }
        MAccountProfile mAccountProfile =mBAccount.GetProfile(accountProfile.Account);
        // addOrUpdateCacheOrSession(string forAccount, object value, string sessionName = "SessionAccount")
        addOrUpdateCacheOrSession(accountProfile.Account, mAccountProfile);        
        return accountProfile;
    }
}
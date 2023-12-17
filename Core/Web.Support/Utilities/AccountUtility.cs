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
        if (sessionName != "useDefault") { mSessionName = sessionName; }
        if (!forAccount.Equals(s_Anonymous, StringComparison.InvariantCultureIgnoreCase))
        {
            SessionController.AddToSession(mSessionName, value);
            return;
        }
        m_CacheController.AddToCache(s_CachedName, value);
    }

    /// <summary>
    /// Retrieves an object of type `T` from either the cache or the session, based on the given `name`.
    /// </summary>
    /// <typeparam name="T">The type of the object being retrieved.</typeparam>
    /// <param name="name">The name of the value to retrieved.</param>
    /// <returns></returns>
    private static T getFromCacheOrSession<T>(string forAccount, string sessionName = "useDefault")
    {
        string mSessionName = s_SessionName;
        if (sessionName != "useDefault") { mSessionName = sessionName; }
        if (!forAccount.Equals(s_Anonymous, StringComparison.InvariantCultureIgnoreCase))
        {
            var mRetVal = SessionController.GetFromSession<T>(mSessionName);
            return mRetVal;
        }
        return m_CacheController.GetFromCache<T>(s_CachedName);
    }

    /// <summary>
    /// Removes an object from either the cache or the session, based on the given `forAccount`.
    /// </summary>
    /// <param name="forAccount"></param>
    /// <param name="sessionName">Optional if not specified the default value is "useDefault"</param>
    private static void remmoveFromCacheOrSession(string forAccount, string sessionName = "useDefault")
    {
        string mSessionName = s_SessionName;
        if (sessionName != "useDefault") { mSessionName = sessionName; }
        if (!forAccount.Equals(s_Anonymous, StringComparison.InvariantCultureIgnoreCase))
        {
            SessionController.RemoveFromSession(mSessionName);
            return;
        }
        m_CacheController.RemoveFromCache(s_CachedName);
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
        if (string.IsNullOrEmpty(account)) throw new ArgumentNullException("account", "account cannot be a null reference (Nothing in VB) or empty!");
        if (string.IsNullOrEmpty(account)) throw new ArgumentNullException("password", "password cannot be a null reference (Nothing in VB) or empty!");
        string mAccount = account;  // It's good practice to leave parameters unchanged.
        if (account.Equals(s_Anonymous, StringComparison.InvariantCultureIgnoreCase))
        {
            // no need to validate or save
            return GetAccount(account);
        }

        // get account from the DB
        MAccountProfile mRetVal = GetAccount(mAccount, true);
        if (mRetVal == null)
        {
            return mRetVal;
        }

        // validate
        if (m_InvalidStatus.Contains(mRetVal.Status))
        {
            return null;
        }
        bool mIsAuthenticated = false;
        if (ConfigSettings.AuthenticationType.Equals("internal", StringComparison.InvariantCultureIgnoreCase))
        {
            // internal db validation
            CryptoUtility.TryDecrypt(mRetVal.Password, out string mProfilePassword, ConfigSettings.EncryptionType);
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

        // authentication successful so generate jwt and refresh tokens
        mRetVal = TokenUtility.SetTokens(mRetVal, ipAddress);

        // remove old refresh tokens from account
        TokenUtility.RemoveOldRefreshTokens(mRetVal);

        // save changes to db
        mRetVal.FailedAttempts = 0;
        mRetVal.LastLogOn = DateTime.Now;        
        Save(mRetVal, true, false, false);
        RemoveInMemoryInformation(mRetVal.Account);
        addOrUpdateCacheOrSession(mRetVal.Account, mRetVal);
        ClientChoicesUtility.SynchronizeContext(mRetVal.Account); 
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
                CryptoUtility.TryEncrypt(changePassword.NewPassword, out string mEncryptedPassword, mSecurityEntity.EncryptionType, ConfigSettings.EncryptionSaltExpression);
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
        mRetVal = mMessageProfile.Body;
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
            MAccountProfile mRetVal = getFromCacheOrSession<MAccountProfile>("not_anonymous") ?? getFromCacheOrSession<MAccountProfile>(s_Anonymous);
            if (mRetVal == null)
            {
                mRetVal = GetAccount(s_Anonymous, true);
                addOrUpdateCacheOrSession(s_Anonymous, mRetVal);
            }
            return mRetVal;
        }
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
    /// Retrieves menu data for a given account and MenuType
    /// </summary>
    /// <param name="account"></param>
    /// <param name="menuType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
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
        if (mDataTable != null)
        {
            mRetVal = DataHelper.GetJsonStringFromTable(ref mDataTable);
            addOrUpdateCacheOrSession(account, mRetVal, mMenuName);
        }
        return mRetVal;
    }

    /// <summary>
    /// Retrieves the list of menu items for a given account and MenuType
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
        addOrUpdateCacheOrSession(account, mRetVal, mMenuName);
        return mRetVal;
    }

    /// <summary>
    /// Retrieves the account profile for the given account.
    /// </summary>
    /// <param name="account"></param>
    /// <param name="forceDb"></param>
    /// <returns></returns>
    public static MAccountProfile GetAccount(string account, bool forceDb = false)
    {
        MAccountProfile mRetVal = null;
        BAccounts mBAccount = null;
        if (forceDb)
        {
            mBAccount = new BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            mRetVal = mBAccount.GetProfile(account);
            return mRetVal;
        }
        mRetVal = CurrentProfile;
        if (mRetVal == null || (!mRetVal.Account.Equals(account, StringComparison.InvariantCultureIgnoreCase)))
        {
            mBAccount = new BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            mRetVal = mBAccount.GetProfile(account);
        }
        return mRetVal;
    }

    public static void Logoff(string forAccount)
    {
        remmoveFromCacheOrSession(forAccount);
        RemoveInMemoryInformation(forAccount);
    }

    /// <summary>
    /// Removes thhe menu and or other information from the session for the given account.
    /// </summary>
    /// <param name="forAccount"></param>
    /// <notes>Does not remove account information from session use remmoveFromCacheOrSession.</notes>
    public static void RemoveInMemoryInformation(string forAccount)
    {
        foreach (MenuType mMenuType in Enum.GetValues(typeof(MenuType)))
        {
            string mMenuName = mMenuType.ToString() + "_" + forAccount + "_Menu";
            remmoveFromCacheOrSession(forAccount, mMenuName + "_Menu");
            remmoveFromCacheOrSession(forAccount, mMenuName + "_Menu_Data");
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
        /*
         * Roles, groups, and refresh tokens are stored in detail tables and it is not always necessary to save them.
         */
        if (accountProfile == null || string.IsNullOrEmpty(accountProfile.Account)) throw new ArgumentNullException(nameof(accountProfile), "accountProfile cannot be a null reference (Nothing in VB) or empty!");
        MSecurityEntity mSecurityEntity = SecurityEntityUtility.CurrentProfile();
        BAccounts mBAccount = new(mSecurityEntity, ConfigSettings.CentralManagement);
        mBAccount.Save(accountProfile, saveRefreshTokens, saveRoles, saveGroups);
        MAccountProfile mAccountProfile = mBAccount.GetProfile(accountProfile.Account);
        return accountProfile;
    }
}
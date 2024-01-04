using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Enumerations;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.Web.Support.Jwt;

namespace GrowthWare.Web.Support.Utilities;
public static class AccountUtility
{
    private static string s_Anonymous = "Anonymous";
    private static string s_CachedName = "CachedAnonymous";
    private static CacheController m_CacheController = CacheController.Instance();
    private static int[] m_InvalidStatus = { (int)SystemStatus.Disabled, (int)SystemStatus.Inactive };
    private static JwtUtility m_JwtUtils = new JwtUtility();
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
    private static void removeFromCacheOrSession(string forAccount, string sessionName = "useDefault")
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
    /// Performs the authentication logic and session/cache management.
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
        MAccountProfile mRetVal = null;
        if (account.Equals(s_Anonymous, StringComparison.InvariantCultureIgnoreCase))
        {
            // no need to validate or save
            mRetVal = GetAccount(account);
            // generate token for the anonymous account b/c it is needed in auth-guard.guard.ts (canActivate)
            // to be able to check if the user is authenticated or not
            mRetVal.Token = m_JwtUtils.GenerateJwtToken(mRetVal);
            mRetVal.RefreshTokens = [];
            return mRetVal;
        }

        // get account from the DB
        mRetVal = GetAccount(mAccount, true);
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
        mRetVal.Token = m_JwtUtils.GenerateJwtToken(mRetVal);
        mRetVal.RefreshTokens.Add(m_JwtUtils.GenerateRefreshToken(ipAddress, mRetVal.Id));

        // remove old refresh tokens from account
        removeOldRefreshTokens(mRetVal);

        // save changes to db
        mRetVal.FailedAttempts = 0;
        mRetVal.LastLogOn = DateTime.Now;
        Save(mRetVal, true, false, false);
        RemoveInMemoryInformation(mRetVal.Account);
        // Update the cache or session which in turn will update the "CurrentProfile" property.
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

    /// <summary>
    /// Retrieves the account profile based on the provided refresh token.
    /// </summary>
    /// <param name="token">The refresh token used to retrieve the account profile.</param>
    /// <returns>
    /// An instance of MAccountProfile representing the account profile associated with the refresh token.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when the provided token is null or empty.</exception>
    /// <exception cref="System.Exception">Thrown when an error occurs while retrieving the account profile.</exception>
    private static MAccountProfile getAccountByRefreshToken(string token)
    {
        BAccounts mBAccount = new(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
        MAccountProfile mRetVal = null;
        try
        {
            mRetVal = mBAccount.GetProfileByRefreshToken(token);
        }
        catch (System.Exception)
        {
            mRetVal = GetAccount(s_Anonymous);
        }
        return mRetVal;
    }

    public static void Logoff(string forAccount, string token, string ipAddress)
    {
        if(!String.IsNullOrWhiteSpace(token))
        {
            MAccountProfile mAccountProfile = GetAccount(forAccount);
            if(mAccountProfile.RefreshTokens.Count > 0) 
            {
                MRefreshToken mRefreshToken = mAccountProfile.RefreshTokens.Single(x => x.Token == token);            
                revokeRefreshToken(mRefreshToken, ipAddress, "Revoked by Logoff");
                // remove old refresh tokens from account
                removeOldRefreshTokens(mAccountProfile);
                // save changes to db
                AccountUtility.Save(mAccountProfile, true, false, false);
            }
        }
        removeFromCacheOrSession(forAccount);
        RemoveInMemoryInformation(forAccount);
    }

    /// <summary>
    /// Refreshes the access token and generates a new JWT token.
    /// </summary>
    /// <param name="token">The refresh token.</param>
    /// <param name="ipAddress">The IP address of the user.</param>
    /// <returns>AuthenticationResponse or null</returns>
    public static AuthenticationResponse RefreshToken(string token, string ipAddress)
    {
        MAccountProfile mAccountProfile = getAccountByRefreshToken(token);
        if (mAccountProfile.RefreshTokens.Count > 0)
        {
            MRefreshToken mRefreshToken = mAccountProfile.RefreshTokens.Single(x => x.Token == token);

            if (mRefreshToken.IsRevoked())
            {
                // revoke all descendant tokens in case this token has been compromised
                revokeDescendantRefreshTokens(mRefreshToken, mAccountProfile, ipAddress, $"Attempted reuse of revoked ancestor token: {token}");
                AccountUtility.Save(mAccountProfile, true, false, false);
            }

            if (!mRefreshToken.IsActive())
                throw new WebSupportException("Invalid token");

            // replace old refresh token with a new one (rotate token)
            var newRefreshToken = rotateRefreshToken(mAccountProfile.Id, mRefreshToken, ipAddress);
            mAccountProfile.RefreshTokens.Add(newRefreshToken);

            // remove old refresh tokens from account
            removeOldRefreshTokens(mAccountProfile);

            // generate new jwt
            mAccountProfile.Token = m_JwtUtils.GenerateJwtToken(mAccountProfile);

            // save changes to db and update session/cache
            AccountUtility.Save(mAccountProfile, true, false, false);
        }

        AuthenticationResponse mRetVal = new(mAccountProfile);
        ClientChoicesUtility.SynchronizeContext(mRetVal.Account);
        return mRetVal;
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
            removeFromCacheOrSession(forAccount, mMenuName + "_Menu");
            removeFromCacheOrSession(forAccount, mMenuName + "_Menu_Data");
        }
    }

    /// <summary>
    /// Revokes a refresh token.
    /// </summary>
    /// <param name="token">The refresh token to revoke.</param>
    /// <param name="ipAddress">The IP address of the user revoking the token.</param>
    /// <param name="reason">The reason for revoking the token. (optional)</param>
    /// <param name="replacedByToken">The token that replaces the revoked token. (optional)</param>
    private static void revokeRefreshToken(MRefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
    {
        token.Revoked = DateTime.UtcNow;
        token.RevokedByIp = ipAddress;
        token.ReasonRevoked = reason;
        token.ReplacedByToken = replacedByToken;
    }

    public static void RevokeToken(string token, string ipAddress)
    {
        MAccountProfile mAccountProfile = getAccountByRefreshToken(token);
        MRefreshToken mRefreshToken = mAccountProfile.RefreshTokens.Single(x => x.Token == token);

        if (!mRefreshToken.IsActive())
            throw new WebSupportException("Invalid token");

        // revoke token and save
        revokeRefreshToken(mRefreshToken, ipAddress, "Revoked without replacement");
        Save(mAccountProfile, true, false, false);
    }

    /// Recursively traverses the refresh token chain and ensures all descendants are revoked.
    /// </summary>
    /// <param name="refreshToken">The refresh token to start the traversal from.</param>
    /// <param name="account">The account profile associated with the refresh token.</param>
    /// <param name="ipAddress">The IP address of the requester.</param>
    /// <param name="reason">The reason for revoking the tokens.</param>    
    private static void revokeDescendantRefreshTokens(MRefreshToken refreshToken, MAccountProfile account, string ipAddress, string reason)
    {
        // recursively traverse the refresh token chain and ensure all descendants are revoked
        if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
        {
            var childToken = account.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
            if (childToken.IsActive())
                revokeRefreshToken(childToken, ipAddress, reason);
            else
                revokeDescendantRefreshTokens(childToken, account, ipAddress, reason);
        }
    }

    /// <summary>
    /// Rotates the refresh token for the given user.
    /// </summary>
    /// <param name="refreshToken">The current refresh token.</param>
    /// <param name="ipAddress">The IP address of the user.</param>
    /// <returns>The new refresh token.</returns>    
    private static MRefreshToken rotateRefreshToken(int accountSeqId, MRefreshToken refreshToken, string ipAddress)
    {
        var newRefreshToken = m_JwtUtils.GenerateRefreshToken(ipAddress, accountSeqId);
        revokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
        return newRefreshToken;
    }

    /// <summary>
    /// Removes old refresh tokens from the given MAccountProfile.
    /// </summary>
    /// <param name="account"></param>
    private static void removeOldRefreshTokens(MAccountProfile account)
    {
        // TODO: x.Created.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
        account.RefreshTokens.RemoveAll(x =>
            !x.IsActive() &&
            x.Created.AddMinutes(3) <= DateTime.UtcNow);
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
        if(accountProfile.Account.Equals(s_Anonymous, StringComparison.InvariantCultureIgnoreCase) && accountProfile.RefreshTokens.Count > 0)
        {
             return accountProfile;
        }
        MSecurityEntity mSecurityEntity = SecurityEntityUtility.CurrentProfile();
        BAccounts mBAccount = new(mSecurityEntity, ConfigSettings.CentralManagement);
        mBAccount.Save(accountProfile, saveRefreshTokens, saveRoles, saveGroups);
        MAccountProfile mAccountProfile = mBAccount.GetProfile(accountProfile.Account);
        addOrUpdateCacheOrSession(accountProfile.Account, mAccountProfile);
        return accountProfile;
    }
}
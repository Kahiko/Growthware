﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Enumerations;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.Web.Support.Helpers;
using GrowthWare.Web.Support.Jwt;

namespace GrowthWare.Web.Support.Utilities;
public static class AccountUtility
{
    private static string s_CachedName = "CachedAnonymous";
    private static CacheHelper m_CacheHelper = CacheHelper.Instance();
    private static BAccounts m_BAccounts = null;
    private static int[] m_InvalidStatus = { (int)SystemStatus.Disabled, (int)SystemStatus.Inactive };
    private static Logger m_Logger = Logger.Instance();
    private static JwtUtility m_JwtUtils = new JwtUtility();
    private static string s_SessionName = "SessionAccount";

    public static string AnonymousAccount { get { return ConfigSettings.Anonymous; } }

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
        if (!forAccount.Equals(ConfigSettings.Anonymous, StringComparison.InvariantCultureIgnoreCase))
        {
            SessionHelper.AddToSession(mSessionName, value);
            return;
        }
        m_CacheHelper.AddToCache(s_CachedName, value);
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
        if (!forAccount.Equals(ConfigSettings.Anonymous, StringComparison.InvariantCultureIgnoreCase))
        {
            var mRetVal = SessionHelper.GetFromSession<T>(mSessionName);
            return mRetVal;
        }
        return m_CacheHelper.GetFromCache<T>(s_CachedName);
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
        if (!forAccount.Equals(ConfigSettings.Anonymous, StringComparison.InvariantCultureIgnoreCase))
        {
            SessionHelper.RemoveFromSession(mSessionName);
            return;
        }
        m_CacheHelper.RemoveFromCache(s_CachedName);
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
        if (string.IsNullOrEmpty(account)) throw new ArgumentNullException(nameof(account), "account cannot be a null reference (Nothing in VB) or empty!");
        if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password), "password cannot be a null reference (Nothing in VB) or empty!");
        string mAccount = account;  // It's good practice to leave parameters unchanged.
        MAccountProfile mRetVal = null;
        if (account.Equals(ConfigSettings.Anonymous, StringComparison.InvariantCultureIgnoreCase))
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
    public static Tuple<string, MAccountProfile> ChangePassword(UIChangePassword changePassword, string ipAddress)
    {
        MMessage mMessageProfile = new MMessage();
        MAccountProfile mAccountProfile = CurrentProfile;
        MSecurityEntity mSecurityEntity = SecurityEntityUtility.CurrentProfile;
        CryptoUtility.TryDecrypt(mAccountProfile.Password, out string mCurrentPassword, mSecurityEntity.EncryptionType);
        bool mPasswordVerifed = changePassword.OldPassword == mCurrentPassword;
        bool mCheckOldPassword = mAccountProfile.Status != (int)SystemStatus.ChangePassword;
        if(!mCheckOldPassword)
        {
            mPasswordVerifed = true;
        }
        if(mPasswordVerifed)
        {
            setChangePasswordProperties(changePassword, mAccountProfile, mSecurityEntity, ipAddress);
            try
            {
                Save(mAccountProfile, true, false, false);
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
        return new Tuple<string, MAccountProfile>(mMessageProfile.Body, mAccountProfile);
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
            MAccountProfile mRetVal = getFromCacheOrSession<MAccountProfile>("not_anonymous") ?? getFromCacheOrSession<MAccountProfile>(ConfigSettings.Anonymous);
            if (mRetVal == null)
            {
                mRetVal = GetAccount(ConfigSettings.Anonymous, true);
                addOrUpdateCacheOrSession(ConfigSettings.Anonymous, mRetVal);
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
        BAccounts mBAccount = BusinessLogic;
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
        if (string.IsNullOrEmpty(account)) throw new ArgumentNullException(nameof(account), "account cannot be a null reference (Nothing in VB) or empty!");
        string mMenuName = menuType.ToString() + "_" + account + "_Menu_Data";
        string mRetVal = getFromCacheOrSession<string>(account, mMenuName);
        if (mRetVal != default)
        {
            return mRetVal;
        }
        BAccounts mBAccount = BusinessLogic;
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
        if (string.IsNullOrEmpty(account)) throw new ArgumentNullException(nameof(account), "account cannot be a null reference (Nothing in VB) or empty!");
        IList<MMenuTree> mRetVal = null;
        string mMenuName = menuType.ToString() + "_" + account + "_Menu";
        mRetVal = getFromCacheOrSession<IList<MMenuTree>>(account, mMenuName);
        if (mRetVal != default)
        {
            return mRetVal;
        }
        mRetVal = new List<MMenuTree>();
        BAccounts mBAccount = BusinessLogic;
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
    /// Generates a unique reset token for a given account then saves it to the DB.
    /// </summary>
    /// <param name="accountProfile">MAccountProfile for the account to generate a reset token for.</param>
    /// <param name="origin">Used when sending an email.</param>
    public static MAccountProfile ForgotPassword(string account, string origin)
    {
        MAccountProfile mRetVal = GetAccount(account);
        // generate reset token
        mRetVal.ResetToken = JwtUtility.GenerateResetToken();
        mRetVal.ResetTokenExpires = DateTime.UtcNow.AddDays(1);
        mRetVal.UpdatedBy = mRetVal.Id;
        mRetVal.UpdatedDate = DateTime.UtcNow;
        // Save the account
        Save(mRetVal, false, false, false);
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
            mBAccount = BusinessLogic;
            mRetVal = mBAccount.GetProfile(account);
            return mRetVal;
        }
        mRetVal = CurrentProfile;
        if (mRetVal == null || (!mRetVal.Account.Equals(account, StringComparison.InvariantCultureIgnoreCase)))
        {
            mBAccount = BusinessLogic;
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
        BAccounts mBAccount = BusinessLogic;
        MAccountProfile mRetVal = null;
        mRetVal = mBAccount.GetProfileByRefreshToken(token);
        return mRetVal;
    }

    private static MAccountProfile getProfileByVerificationToken(string token)
    {
        BAccounts mBAccount = new(SecurityEntityUtility.CurrentProfile);
        MAccountProfile mRetVal = null;
        try
        {
            mRetVal = mBAccount.GetProfileByVerificationToken(token);
        }
        catch (System.Exception)
        {
            // do
        }
        return mRetVal;
    }

    /// <summary>
    /// Returns the business logic object used to access the database.
    /// </summary>
    /// <returns></returns>
    private static BAccounts BusinessLogic
    {
        get
        {
            if(m_BAccounts == null || ConfigSettings.CentralManagement == true)
            {
                m_BAccounts = new(SecurityEntityUtility.CurrentProfile);
            }
            return m_BAccounts;
        }
    }

    public static MAccountProfile GetProfileByResetToken(string token)
    {
        BAccounts mBAccount = BusinessLogic;
        MAccountProfile mRetVal = null;
        try
        {
            mRetVal = mBAccount.GetProfileByResetToken(token);
        }
        catch (System.Exception)
        {
            mRetVal = GetAccount(ConfigSettings.Anonymous);
        }
        return mRetVal;
    }

    public static void Logoff(string forAccount, string token, string ipAddress)
    {
        if (!String.IsNullOrWhiteSpace(token))
        {
            MAccountProfile mAccountProfile = GetAccount(forAccount);
            if (mAccountProfile.RefreshTokens.Count > 0)
            {
                MRefreshToken mRefreshToken = mAccountProfile.RefreshTokens.Single(x => x.Token == token);
                if (!mRefreshToken.IsActive)
                {
                    removeFromCacheOrSession(forAccount);
                    RemoveInMemoryInformation(forAccount);
                    throw new WebSupportException("Invalid token");
                }
                // revoke token and save
                revokeRefreshToken(mRefreshToken, ipAddress, "Revoked without replacement");
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
        MAccountProfile mAccountProfile = null;
        try
        {
            mAccountProfile = getAccountByRefreshToken(token);
        }
        catch (System.Exception)
        {
            mAccountProfile = GetAccount(AnonymousAccount);
            // throw;
        }
        if (mAccountProfile.RefreshTokens.Count > 0)
        {
            MRefreshToken mRefreshToken = mAccountProfile.RefreshTokens.Single(x => x.Token == token);

            if (mRefreshToken.IsRevoked)
            {
                // revoke all descendant tokens in case this token has been compromised
                revokeDescendantRefreshTokens(mRefreshToken, mAccountProfile, ipAddress, $"Attempted reuse of revoked ancestor token: {token}");
                AccountUtility.Save(mAccountProfile, true, false, false);
            }

            if (!mRefreshToken.IsActive)
                throw new WebSupportException("Invalid token");

            // replace old refresh token with a new one (rotate token)
            var newRefreshToken = rotateRefreshToken(mAccountProfile.Id, mRefreshToken, ipAddress);
            mAccountProfile.RefreshTokens.Add(newRefreshToken);

            // remove old refresh tokens from account
            removeOldRefreshTokens(mAccountProfile);

            // remove replaced by new tokens
            // TODO - This is not quite right as it removes all tokens with the reason "Replaced by new token"
            //        and we need to consider if an account can be logged on with multiple sessions..
            // removeReplacedByNewTokens(mAccountProfile);

            // generate new jwt
            mAccountProfile.Token = m_JwtUtils.GenerateJwtToken(mAccountProfile);

            // save changes to db and update session/cache
            AccountUtility.Save(mAccountProfile, true, false, false);
        }

        AuthenticationResponse mRetVal = new(mAccountProfile);
        ClientChoicesUtility.SynchronizeContext(mRetVal.Account);
        return mRetVal;
    }

    public static MAccountProfile Register(MAccountProfile accountProfile, string origin)
    {
        // Get the security entity via the URL or use the default
        MSecurityEntity mTargetSecurityEntity = SecurityEntityUtility.CurrentProfile;
        if(ConfigSettings.SecurityEntityFromUrl)
        {
            mTargetSecurityEntity = SecurityEntityUtility.GetProfileByUrl(origin);
        }
        // Need to get the roles so need to get the Registration_Information
        MRegistrationInformation mRegistrationInformation = SecurityEntityUtility.GetRegistrationInformation(mTargetSecurityEntity.Id);
        if(mRegistrationInformation == null) 
        {
            m_Logger.Fatal("Unable to get registration information");
            throw new WebSupportException("Unable to get registration information");
        }
        // Validate (ensure email is not in use as an account)
        MAccountProfile mProfileToSave = GetAccount(accountProfile.Email, true);
        if(String.IsNullOrWhiteSpace(mProfileToSave.Account)) 
        {
            mProfileToSave = new MAccountProfile(accountProfile)
            {
                Account = accountProfile.Email
            };
            if (mRegistrationInformation != null) 
            {
                // Populate the roles/groups via the security entity associated 
                // (Uses the [ZGWSecurity].[Registration_Roles] table)
                mProfileToSave.SetGroups(mRegistrationInformation.Groups);
                mProfileToSave.SetRoles(mRegistrationInformation.Roles);
            }
            else 
            {
                mProfileToSave.SetRoles(ConfigSettings.RegistrationDefaultRoles);
                mProfileToSave.SetGroups(ConfigSettings.RegistrationDefaultGroups);
            }
            // When registrating a new account IsSystemAdmin should always be false
            mProfileToSave.IsSystemAdmin = false;
            // Set the password though it won't be used I didn't want to hard code it
            CryptoUtility.TryEncrypt(ConfigSettings.RegistrationPassword, out string mEncryptedPassword, mTargetSecurityEntity.EncryptionType, ConfigSettings.EncryptionSaltExpression);
            mProfileToSave.Password = mEncryptedPassword;
            // Set the AddedBy/AddedDate
            mProfileToSave.AddedBy = GetAccount("System").Id;
            mProfileToSave.AddedDate = DateTime.Now;
            mProfileToSave.PasswordLastSet = System.DateTime.Now;
            // Save the profile
            BAccounts mBAccount = new(mTargetSecurityEntity);
            // Added for clarity
            Boolean mSaveRefreshTokens = false;
            Boolean mSaveRoles = true;
            Boolean mSaveGroups = true;
            JwtUtility mJwtUtility = new();
            // TODO: At this point I think this needs to be in the upgrade downgrade scripts
            // but for now it will be done here, should also be thinking about [ResetToken]
            // as well.
            string mVerificationToken = mJwtUtility.GenerateVerificationToken();
            mProfileToSave.VerificationToken = mVerificationToken;
            mBAccount.Save(mProfileToSave, mSaveRefreshTokens, mSaveRoles, mSaveGroups);
            mProfileToSave = GetAccount(mProfileToSave.Account, true);
            mProfileToSave.VerificationToken = mVerificationToken;
        }
        else 
        {
            mProfileToSave = null;
        }
        return mProfileToSave;
    }

    /// <summary>
    /// Removes menu information from the session for the given account.
    /// </summary>
    /// <param name="forAccount"></param>
    /// <notes>Does not remove account information from session use remmoveFromCacheOrSession.</notes>
    public static void RemoveInMemoryInformation(string forAccount)
    {
        foreach (MenuType mMenuType in Enum.GetValues(typeof(MenuType)))
        {
            string mMenuName = mMenuType.ToString() + "_" + forAccount;
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

        if (!mRefreshToken.IsActive)
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
            if (childToken.IsActive)
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
    /// <param name="accountProfile"></param>
    private static void removeOldRefreshTokens(MAccountProfile accountProfile)
    {
        // TODO - look at this are we sure we need to keep refresh tokens in the db for this long?
        accountProfile.RefreshTokens.RemoveAll(x => 
            !x.IsActive && 
            x.Created.AddDays(ConfigSettings.JWT_Refresh_Token_DB_TTL_Days) <= DateTime.UtcNow
        );
    }

    /// <summary>
    /// Removes all inactive refresh tokens with the reason "Replaced by new token" from the given account profile,
    /// and adds the most recent such token back to the profile.
    /// </summary>
    /// <param name="accountProfile">The account profile from which to remove replaced tokens.</param>
    private static void removeReplacedByNewTokens(MAccountProfile accountProfile)
    {
        MRefreshToken mByNewToken = accountProfile.RefreshTokens.Where(x => x.ReasonRevoked == "Replaced by new token").OrderByDescending(x => x.Created).FirstOrDefault();
        if(mByNewToken != null)
        {
            accountProfile.RefreshTokens.RemoveAll(x => 
                x.ReasonRevoked == "Replaced by new token"
            );
            accountProfile.RefreshTokens.Add(mByNewToken);
        }
    }

    public static void ResetPassword(MAccountProfile forAccount, string password)
    {
        CryptoUtility.TryEncrypt(password, out string mEncryptedPassword, SecurityEntityUtility.CurrentProfile.EncryptionType, ConfigSettings.EncryptionSaltExpression);
        // TODO: find a better way then changing properties on the passed in parameter!
        forAccount.FailedAttempts = 0;
        forAccount.LastLogOn = System.DateTime.Now;
        forAccount.Password = mEncryptedPassword;
        forAccount.PasswordLastSet = System.DateTime.Now;
        forAccount.ResetToken = null;
        forAccount.ResetTokenExpires = null;
        forAccount.Status = (int)SystemStatus.Active;
        forAccount.UpdatedBy = forAccount.Id;
        forAccount.UpdatedDate = System.DateTime.Now;
        Save(forAccount, false, false, false);
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
        if (accountProfile.Account.Equals(ConfigSettings.Anonymous, StringComparison.InvariantCultureIgnoreCase) && accountProfile.RefreshTokens.Count > 0)
        {
            return accountProfile;
        }
        MSecurityEntity mSecurityEntity = SecurityEntityUtility.CurrentProfile;
        BAccounts mBAccount = BusinessLogic;
        mBAccount.Save(accountProfile, saveRefreshTokens, saveRoles, saveGroups);
        MAccountProfile mAccountProfile = mBAccount.GetProfile(accountProfile.Account);
        if ((accountProfile.Id == CurrentProfile.Id) || (CurrentProfile.Account.Equals(ConfigSettings.Anonymous, StringComparison.InvariantCultureIgnoreCase)))
        {
            addOrUpdateCacheOrSession(accountProfile.Account, mAccountProfile);
        }
        return accountProfile;
    }

    /// <summary>
    /// Sets the properties for a password change on a given profile.
    /// </summary>
    /// <param name="changePassword">The UIChangePassword containing the new password to set.</param>
    /// <param name="mAccountProfile">The account profile to update.</param>
    /// <param name="mSecurityEntity">The current security entity.</param>
    /// <param name="ipAddress">The IP address of the user changing the password.</param>
    /// <remarks>
    /// This method encrypts the new password with the security entity's encryption type
    /// and sets the following properties on the account profile:
    /// <list type="bullet">
    ///     <item>PasswordLastSet to DateTime.Now</item>
    ///     <item>Status to Active</item>
    ///     <item>FailedAttempts to 0</item>
    ///     <item>Password to the encrypted new password</item>
    ///     <item>Token to a new JWT token</item>
    ///     <item>Added a new refresh token to RefreshTokens</item>
    /// </list>
    /// In memory information is also updated (removed/added).
    /// </remarks>
    private static void setChangePasswordProperties(UIChangePassword changePassword, MAccountProfile mAccountProfile, MSecurityEntity mSecurityEntity, string ipAddress)
    {
        mAccountProfile.PasswordLastSet = System.DateTime.Now;
        mAccountProfile.Status = (int)SystemStatus.Active;
        mAccountProfile.FailedAttempts = 0;
        string mEncryptedPassword;
        CryptoUtility.TryEncrypt(changePassword.NewPassword, out mEncryptedPassword, mSecurityEntity.EncryptionType, ConfigSettings.EncryptionSaltExpression);
        mAccountProfile.Password = mEncryptedPassword;
        // password change successful so generate jwt and refresh tokens
        mAccountProfile.Token = m_JwtUtils.GenerateJwtToken(mAccountProfile);
        mAccountProfile.RefreshTokens.Add(m_JwtUtils.GenerateRefreshToken(ipAddress, mAccountProfile.Id));
        // remove old refresh tokens from account
        removeOldRefreshTokens(mAccountProfile);
        // update the in-memory information
        RemoveInMemoryInformation(mAccountProfile.Account);
        addOrUpdateCacheOrSession(mAccountProfile.Account, mAccountProfile);
    }
    
    /// <summary>
    /// Verifies an account by checking if the provided verification token exists in the database and matches the email address.
    /// </summary>
    /// <param name="verificationToken">The verification token to check.</param>
    /// <param name="email">The email address to compare against.</param>
    /// <returns>The verified account profile if the verification token and email match, otherwise null.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the verificationToken parameter is null.</exception>
    public static MAccountProfile VerifyAccount(string verificationToken, string email)
    {
        if (verificationToken == null) throw new ArgumentNullException(nameof(verificationToken), "verificationToken cannot be a null reference (Nothing in VB)!");
        MAccountProfile mRetVal = null;
        MAccountProfile mAccountProfile = getProfileByVerificationToken(verificationToken);
        if(mAccountProfile != null && !String.IsNullOrWhiteSpace(mAccountProfile.Email) && mAccountProfile.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase))
        {
            mRetVal = mAccountProfile;
        }
        else
        {
            // ensure that we return null if the account was found by the verification token but the email addresses did not match
            mAccountProfile = null;
            string mMsg = string.Format("Attempted to verify token for account: {0} and verification token: '{1}'", email, verificationToken);
            m_Logger.Error(mMsg);
            mMsg = "The verification token {0}!";
            if(mAccountProfile == null)
            {
                mMsg = string.Format(mMsg, "was not found in the database!");
            } else if(!String.IsNullOrWhiteSpace(mAccountProfile.Email) && !mAccountProfile.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase))
            {
                mMsg = string.Format(mMsg, "email addresses did not match for the found token!");
            }
            m_Logger.Error(mMsg);
        }
        return mRetVal;
    }
}
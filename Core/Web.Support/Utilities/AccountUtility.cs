using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Enumerations;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.Web.Support.Jwt;
using Microsoft.AspNetCore.Mvc.Diagnostics;

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
    private static void addOrUpdateCacheOrSession(string forAccount, object value, string sessionName = "SessionAccount")
    {
        if (forAccount.ToLowerInvariant() != s_Anonymous.ToLowerInvariant())
        {
            SessionController.AddToSession(sessionName, value);
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
        if (account.ToLowerInvariant() == s_Anonymous.ToLowerInvariant())
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
            if (ConfigSettings.AuthenticationType.ToLowerInvariant() == "internal")
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
            mRetVal = setTokens(mRetVal, ipAddress);
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
        MAccountProfile mAccountProfile = SessionController.GetFromSession<MAccountProfile>("AccountProfile");
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
    /// Generates a JWT token using the given MAccountProfile.
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    private static string generateJwtToken(MAccountProfile account)
    {
        var mJwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var mKey = Encoding.ASCII.GetBytes(ConfigSettings.Secret);
        var mSecurityTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {
                new Claim("account", account.Account),
                new Claim("status", account.Status.ToString())
                }),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(mKey), SecurityAlgorithms.HmacSha256Signature)
        };
        var mToken = mJwtSecurityTokenHandler.CreateToken(mSecurityTokenDescriptor);
        return mJwtSecurityTokenHandler.WriteToken(mToken);
    }

    /// <summary>
    /// Generates a new refresh token for the given IP address.
    /// </summary>
    /// <param name="ipAddress">The IP address for which the refresh token is generated.</param>
    private static MRefreshToken generateRefreshToken(string ipAddress)
    {
        var mRetVal = new MRefreshToken
        {
            // token is a cryptographically strong random sequence of values
            Token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64)),
            // token is valid for 7 days
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow,
            CreatedByIp = ipAddress
        };

        // ensure token is unique by checking against db
        // var mTokenIsUnique = !_context.Accounts.Any(a => a.RefreshTokens.Any(t => t.Token == mRetVal.Token));
        var mTokenIsUnique = !RefreshTokenExists(mRetVal.Token);

        if (!mTokenIsUnique)
            return generateRefreshToken(ipAddress);

        return mRetVal;
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
        if (mRetVal == null || (mRetVal.Account.ToLowerInvariant() != account.ToLowerInvariant()))
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
        if (String.IsNullOrEmpty(token))
        {
            throw new ArgumentException("token can not be null or empty", token);
        }
        MAccountProfile mRetVal = null;
        try
        {
            BAccounts mBAccount = new BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            mRetVal = mBAccount.GetProfileByRefreshToken(token);
        }
        catch (System.Exception)
        {
            throw;
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
            if (mRetVal == null || (mRetVal.Account.ToLowerInvariant() != account.ToLowerInvariant()))
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
        if (forAccount.ToLowerInvariant() != s_Anonymous.ToLowerInvariant())
        {
            return SessionController.GetFromSession<T>(sessionName);
        }
        return m_CacheController.GetFromCache<T>(s_CachedName);
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
    /// Refreshes the access token and generates a new JWT token.
    /// </summary>
    /// <param name="token">The refresh token.</param>
    /// <param name="ipAddress">The IP address of the user.</param>
    /// <returns>AuthenticationResponse or null</returns>
    public static AuthenticationResponse RefreshToken(string token, string ipAddress)
    {
        try
        {
            MAccountProfile mAccountProfile = getAccountByRefreshToken(token);
            MRefreshToken refreshToken = mAccountProfile.RefreshTokens.Single(x => x.Token == token);

            if (refreshToken.IsRevoked())
            {
                // revoke all descendant tokens in case this token has been compromised
                revokeDescendantRefreshTokens(refreshToken, mAccountProfile, ipAddress, $"Attempted reuse of revoked ancestor token: {token}");
                Save(mAccountProfile, true, false, false);
                return null;
            }
            if (!refreshToken.IsActive()) throw new WebSupportException("Invalid token");
            // replace old refresh token with a new one (rotate token)
            var newRefreshToken = rotateRefreshToken(refreshToken, ipAddress);
            newRefreshToken.AccountSeqId = mAccountProfile.Id;
            mAccountProfile.RefreshTokens.Add(newRefreshToken);
            // remove old refresh tokens from account
            removeOldRefreshTokens(mAccountProfile);

            // save changes to db
            Save(mAccountProfile, true, false, false);
            ClientChoicesUtility.SynchronizeContext(mAccountProfile.Account);

            // generate new jwt
            JwtUtils mJwtUtils = new JwtUtils();
            var jwtToken = mJwtUtils.GenerateJwtToken(mAccountProfile);

            // return data in authenticate response object
            AuthenticationResponse mResponse = new AuthenticationResponse(mAccountProfile);
            mResponse.JwtToken = jwtToken;
            mResponse.RefreshToken = newRefreshToken.Token;
            return mResponse;
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Checks if a refresh token exists in the system.
    /// </summary>
    /// <param name="refreshToken">The refresh token to check.</param>
    /// <returns>True if the refresh token exists, otherwise false.</returns>
    public static bool RefreshTokenExists(string refreshToken)
    {
        MSecurityEntity mSecurityEntityProfile = SecurityEntityUtility.CurrentProfile();
        BAccounts mBAccount = new BAccounts(mSecurityEntityProfile, ConfigSettings.CentralManagement);
        return mBAccount.RefreshTokenExists(refreshToken);
    }

    /// <summary>
    /// Removes an object from either the cache or the session, based on the given `forAccount`.
    /// </summary>
    /// <param name="forAccount"></param>
    /// <param name="sessionName">Optional if not specified the default value is "SessionAccount"</param>
    public static void RemmoveFromCacheOrSession(string forAccount, string sessionName = "SessionAccount")
    {
        if (forAccount.ToLowerInvariant() != s_Anonymous.ToLowerInvariant())
        {
            SessionController.RemoveFromSession(s_SessionName);
            return;
        }
        m_CacheController.RemoveFromCache(s_CachedName);
    }

    /// <summary>
    /// Removes account and menu information from the session for the given account.
    /// </summary>
    /// <param name="forAccount"></param>
    public static void RemoveInMemoryInformation(string forAccount) 
    {
        RemmoveFromCacheOrSession(forAccount);
        foreach (MenuType mMenuType in Enum.GetValues(typeof(MenuType)))
        {
            string mMenuName = mMenuType.ToString() + "_" + forAccount + "_Menu";
            RemmoveFromCacheOrSession(forAccount, mMenuName);
        }        
    }

    /// <summary>
    /// Removes old refresh tokens from the given MAccountProfile.
    /// </summary>
    /// <param name="account"></param>
    private static void removeOldRefreshTokens(MAccountProfile account)
    {
        account.RefreshTokens.RemoveAll(x =>
            !x.IsActive() &&
            x.Created.AddDays(ConfigSettings.RefreshTokenTTL) <= DateTime.UtcNow);
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

    /// <summary>
    /// Rotates the refresh token for the given user.
    /// </summary>
    /// <param name="refreshToken">The current refresh token.</param>
    /// <param name="ipAddress">The IP address of the user.</param>
    /// <returns>The new refresh token.</returns>
    private static MRefreshToken rotateRefreshToken(MRefreshToken refreshToken, string ipAddress)
    {
        // Generate a new refresh token
        var newRefreshToken = generateRefreshToken(ipAddress);

        // Revoke the old refresh token and replace it with the new token
        revokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);

        // Return the new refresh token
        return newRefreshToken;
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

    /// <summary>
    /// Sets the tokens for the provided account profile and IP address.
    /// </summary>
    /// <param name="accountProfile">The account profile to set the tokens for.</param>
    /// <param name="ipAddress">The IP address associated with the tokens.</param>
    /// <returns>The updated account profile with the tokens set.</returns>
    private static MAccountProfile setTokens(MAccountProfile accountProfile, string ipAddress)
    {
        MAccountProfile mAccountProfile = accountProfile;
        mAccountProfile.FailedAttempts = 0;
        mAccountProfile.LastLogOn = DateTime.Now;
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigSettings.Secret));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
            {
                new Claim("account", mAccountProfile.Account), 
                // new Claim(ClaimTypes.Role, "Manager") 
            };
        // TODO: the issuer and audience should be in a configuration file
        var tokeOptions = new JwtSecurityToken(
            issuer: "https://localhost:5001",
            audience: "https://localhost:5001",
            claims: claims,
            expires: DateTime.Now.AddMinutes(5),
            signingCredentials: signingCredentials
        );
        mAccountProfile.Token = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

        var mJwtToken = generateJwtToken(mAccountProfile);
        var mRefreshToken = generateRefreshToken(ipAddress);
        mRefreshToken.AccountSeqId = mAccountProfile.Id;
        mAccountProfile.Token = mJwtToken;
        mAccountProfile.RefreshTokens.Add(mRefreshToken);
        return mAccountProfile;
    }
}
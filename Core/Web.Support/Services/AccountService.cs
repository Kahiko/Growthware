using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Security.Claims;
using System.Security.Cryptography;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Enumerations;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.Web.Support.Jwt;
using GrowthWare.Web.Support.Utilities;

namespace GrowthWare.Web.Support.Services;
public class AccountService : IAccountService
{
    // private MAccountProfile m_CachedAnonymousAccount = null;
    private int[] m_InvalidStatus = { (int)SystemStatus.Disabled, (int)SystemStatus.Inactive };
    private string s_AnonymousAccount = "Anonymous";

    private CacheController m_CacheController = CacheController.Instance();

    private string s_SessionName = "SessionAccount";

    public string AnonymousAccount { get { return s_AnonymousAccount; } }

    public string SessionName { get { return s_SessionName; } }

    [CLSCompliant(false)]
    public AccountService()
    {
    }

    /// <summary>
    /// Adds or updates a value in the cache or session.
    /// </summary>
    /// <param name="name">The name of the value to add or update.</param>
    /// <param name="value">The value to add or update.</param>
    private void addOrUpdateCacheOrSession(string name, object value, string forAccount)
    {
        if (forAccount.ToLowerInvariant() != s_AnonymousAccount.ToLowerInvariant())
        {
            SessionController.AddToSession(name, value);
            return;
        }
        this.m_CacheController.AddToCache(name, value);
    }

    /// <summary>
    /// Performs the authentication logic
    /// </summary>
    /// <param name="account"></param>
    /// <param name="password"></param>
    /// <param name="ipAddress"></param>
    /// <returns>MAccountProfile if authenticated null if not authenticated</returns>
    public MAccountProfile Authenticate(string account, string password, string ipAddress)
    {
        if (string.IsNullOrEmpty(account)) throw new ArgumentNullException("account", "account cannot be a null reference (Nothing in VB) or empty!");
        if (string.IsNullOrEmpty(account)) throw new ArgumentNullException("password", "password cannot be a null reference (Nothing in VB) or empty!");
        string mRequestedAccount = account;  // It's good practice to leave parameters unchanged.
        bool mAuthenticated = false;
        bool mIsAnonymous = false;
        bool mIsDomainAccount = false;
        int mDomainPos = account.IndexOf(@"\", StringComparison.OrdinalIgnoreCase);
        bool mForceDb = true;
        if (mDomainPos != -1)
        {
            mIsDomainAccount = true;
            mRequestedAccount = account.Substring(mDomainPos + 1, account.Length - mDomainPos - 1);
        }
        mIsAnonymous = mRequestedAccount.ToLowerInvariant() == this.s_AnonymousAccount.ToLowerInvariant();
        if (mIsAnonymous)
        {
            mForceDb = false;
            mAuthenticated = true;
        }
        MAccountProfile mAccountProfile = GetAccount(mRequestedAccount, mForceDb, !mIsAnonymous);
        if (!mIsAnonymous)
        {
            if (!this.m_InvalidStatus.Contains(mAccountProfile.Status))
            {
                // the account is not in an invalid status
                if (!mIsDomainAccount && ConfigSettings.AuthenticationType.ToUpper(CultureInfo.InvariantCulture) == "INTERNAL")
                {
                    // proprietary authentication
                    string mProfilePassword = string.Empty;
                    CryptoUtility.TryDecrypt(mAccountProfile.Password, out mProfilePassword, ConfigSettings.EncryptionType);
                    mAuthenticated = password == mProfilePassword;
                    if (mAuthenticated)
                    {
                        mAuthenticated = mAccountProfile.FailedAttempts < 4 && mAccountProfile.Status != (int)SystemStatus.Disabled;
                    }
                }
                else
                {
                    // TODO: LDAP authentication
                }
            }
        }
        if (mAuthenticated)
        {
            // setup tokens, claims and what not
            mAccountProfile = setTokens(mAccountProfile, ipAddress);
            mAccountProfile.FailedAttempts = 0;
            if (!mIsAnonymous) { mAccountProfile.LastLogOn = DateTime.Now; }
            this.Save(mAccountProfile, true, false, false);
            mAccountProfile.Password = ""; // Don't want to ever send the password out
        }
        else
        {
            // return null
            mAccountProfile.FailedAttempts += 1;
            if (mAccountProfile.FailedAttempts > 3 && mAccountProfile.Status != (int)SystemStatus.Disabled)
            {
                mAccountProfile.Status = (int)SystemStatus.Disabled;
            }
            this.Save(mAccountProfile, true, false, false);
            mAccountProfile = null;
        }
        return mAccountProfile;
    }

    /// <summary>
    /// Sets the tokens for the provided account profile and IP address.
    /// </summary>
    /// <param name="accountProfile">The account profile to set the tokens for.</param>
    /// <param name="ipAddress">The IP address associated with the tokens.</param>
    /// <returns>The updated account profile with the tokens set.</returns>
    private MAccountProfile setTokens(MAccountProfile accountProfile, string ipAddress)
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

    /// <summary>
    /// ChangePassword function takes in a UIChangePassword object as a parameter and returns a string.
    /// </summary>
    /// <param name="changePassword">UIChangePassword</param>
    /// <returns></returns>
    public string ChangePassword(UIChangePassword changePassword)
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
                    addOrUpdateCacheOrSession(s_SessionName, mAccountProfile, mAccountProfile.Account);
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
                    addOrUpdateCacheOrSession(s_SessionName, mAccountProfile, mAccountProfile.Account);
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
    public void Delete(int accountSeqId)
    {
        BAccounts mBAccount = new BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
        mBAccount.Delete(accountSeqId);
    }

    /// <summary>
    /// Generates a JWT token using the given MAccountProfile.
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    private string generateJwtToken(MAccountProfile account)
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
    /// Generates a reset token.
    /// </summary>
    /// <returns></returns>
    private string generateResetToken()
    {
        // token is a cryptographically strong random sequence of values
        var mToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        if (RefreshTokenExists(mToken))
        {
            generateResetToken();
        }
        return mToken;
    }

    private MRefreshToken generateRefreshToken(string ipAddress)
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
    /// Generates a verification token.
    /// </summary>
    /// <returns></returns>
    private string generateVerificationToken()
    {
        // token is a cryptographically strong random sequence of values
        var mToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        if (verificationTokenExists(mToken))
        {
            generateVerificationToken();
        }
        return mToken;
    }

    /// <summary>
    /// Populates and returns a MAccountProfile if found in the DB
    /// </summary>
    /// <param name="account"></param>
    /// <returns>MAccountProfile or null</returns>
    public MAccountProfile GetAccount(String account, bool forceDb = false, bool updateSession = false)
    {
        if (String.IsNullOrEmpty(account)) throw new ArgumentException("account can not be null or empty", account);
        MAccountProfile mRetVal = null;
        BAccounts mBAccount = null;
        string mSessionNameToUse = s_SessionName;
        if (account.ToLowerInvariant() == s_AnonymousAccount.ToLowerInvariant())
        {
            mSessionNameToUse = s_AnonymousAccount;
        }
        try
        {
            if (forceDb)
            {
                mBAccount = new BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
                mRetVal = mBAccount.GetProfile(account);
                if (!String.IsNullOrWhiteSpace(mRetVal.Account))
                {
                    if(updateSession)
                    {
                        addOrUpdateCacheOrSession(mSessionNameToUse, mRetVal, mSessionNameToUse);
                    }
                }
                else
                {
                    mRetVal = null;
                }
            }
            else
            {
                mRetVal = getFromCacheOrSession<MAccountProfile>(mSessionNameToUse, mSessionNameToUse);
                if (mRetVal == default)
                {
                    mBAccount = new BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
                    mRetVal = mBAccount.GetProfile(account);
                    addOrUpdateCacheOrSession(mSessionNameToUse, mRetVal, mSessionNameToUse);
                }
            }
        }
        catch (InvalidOperationException)
        {
            String mMSG = "Account not find account: " + account + " in the database";
            Logger.Instance().Error(mMSG);
        }
        catch (IndexOutOfRangeException)
        {
            String mMSG = "Account not find account: " + account + " in the database";
            Logger.Instance().Error(mMSG);
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
    private MAccountProfile getAccountByRefreshToken(string token)
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
    /// Retrieves an account profile based on the given reset token.
    /// </summary>
    /// <param name="token"></param>
    /// <returns>An instance of MAccountProfile representing the account profile associated with the reset token.</returns>
    /// <exception cref="ArgumentException"></exception>
    private MAccountProfile getAccountByResetToken(string token)
    {
        if (String.IsNullOrEmpty(token))
        {
            throw new ArgumentException("token can not be null or empty", token);
        }
        BAccounts mBAccount = new BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
        MAccountProfile mRetVal = null;
        mRetVal = mBAccount.GetProfileByResetToken(token);
        return mRetVal;
    }

    public MAccountProfile GetCurrentAccount()
    {
        MAccountProfile mRetVal = SessionController.GetFromSession<MAccountProfile>(s_SessionName);
        if(mRetVal == null) 
        {
            mRetVal = GetAccount("Anonymous");
            if(mRetVal == null)
            {
                mRetVal = GetAccount("Anonymous", true);
            }
        }
        return mRetVal;
    }

    /// <summary>
    /// Retrieves an object of type `T` from either the cache or the session, based on the given `name`.
    /// </summary>
    /// <typeparam name="T">The type of the object being retrieved.</typeparam>
    /// <param name="name">The name of the value to retrieved.</param>
    /// <returns></returns>
    private T getFromCacheOrSession<T>(string name, string forAccount)
    {
        if (forAccount.ToLowerInvariant() != s_AnonymousAccount.ToLowerInvariant())
        {
            return SessionController.GetFromSession<T>(name);
        }
        return this.m_CacheController.GetFromCache<T>(name);
    }

    /// <summary>
    /// Retrieves the list of menu items for a given account and menu type.
    /// </summary>
    /// <param name="account">The account for which to retrieve the menu items.</param>
    /// <param name="menuType"></param>
    /// <returns>The list of menu items for the specified account and menu type.</returns>
    /// <exception cref="ArgumentNullException">he type of menu (e.g., Hierarchical, Horizontal, or Vertical) to retrieve the menu items for.</exception>
    public IList<MMenuTree> GetMenuItems(string account, MenuType menuType)
    {
        if (string.IsNullOrEmpty(account)) throw new ArgumentNullException("account", "account cannot be a null reference (Nothing in VB) or empty!");
        IList<MMenuTree> mRetVal = null;
        string mMenuName = menuType.ToString() + "_" + account + "_Menu";
        mRetVal = getFromCacheOrSession<IList<MMenuTree>>(mMenuName, account);
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
        addOrUpdateCacheOrSession(mMenuName, mRetVal, account);
        return mRetVal;
    }

    private MRefreshToken rotateRefreshToken(MRefreshToken refreshToken, string ipAddress)
    {
        var newRefreshToken = generateRefreshToken(ipAddress);
        revokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
        return newRefreshToken;
    }

    /// <summary>
    /// Refreshes the access token and generates a new JWT token.
    /// </summary>
    /// <param name="token">The refresh token.</param>
    /// <param name="ipAddress">The IP address of the user.</param>
    /// <returns></returns>
    public AuthenticationResponse RefreshToken(string token, string ipAddress)
    {
        try
        {
            MAccountProfile mAccountProfile = getAccountByRefreshToken(token);
            MSecurityEntity mSecurityEntityProfile = SecurityEntityUtility.CurrentProfile();
            MRefreshToken refreshToken = mAccountProfile.RefreshTokens.Single(x => x.Token == token);

            if (refreshToken.IsRevoked())
            {
                // revoke all descendant tokens in case this token has been compromised
                revokeDescendantRefreshTokens(refreshToken, mAccountProfile, ipAddress, $"Attempted reuse of revoked ancestor token: {token}");
                this.Save(mAccountProfile, true, false, false, mSecurityEntityProfile);
            }
            if (!refreshToken.IsActive()) throw new WebSupportException("Invalid token");
            // replace old refresh token with a new one (rotate token)
            var newRefreshToken = rotateRefreshToken(refreshToken, ipAddress);
            newRefreshToken.AccountSeqId = mAccountProfile.Id;
            mAccountProfile.RefreshTokens.Add(newRefreshToken);
            // remove old refresh tokens from account
            removeOldRefreshTokens(mAccountProfile);

            // save changes to db
            this.Save(mAccountProfile, true, false, false, mSecurityEntityProfile);
            addOrUpdateCacheOrSession(s_SessionName, mAccountProfile, mAccountProfile.Account);
            ClientChoicesUtility.GetClientChoicesState(mAccountProfile.Account, true);
            
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
    public bool RefreshTokenExists(string refreshToken)
    {
        MSecurityEntity mSecurityEntityProfile = SecurityEntityUtility.CurrentProfile();
        BAccounts mBAccount = new BAccounts(mSecurityEntityProfile, ConfigSettings.CentralManagement);
        return mBAccount.RefreshTokenExists(refreshToken);
    }

    /// <summary>
    /// Removes the specified name from the cache or session.
    /// </summary>
    /// <param name="name">The name to remove from the cache or session.</param>
    public void RemmoveFromCacheOrSession(string name, string forAccount)
    {
        if (forAccount.ToLowerInvariant() != s_AnonymousAccount.ToLowerInvariant())
        {
            SessionController.RemoveFromSession(name);
            return;
        }
        this.m_CacheController.RemoveFromCache(name);
    }

    public void RemoveMenusFromCacheOrSession(string forAccount)
    {
        foreach (MenuType mMenuType in Enum.GetValues(typeof(MenuType)))
        {
            string mMenuName = mMenuType.ToString() + "_" + forAccount + "_Menu";
            this.RemmoveFromCacheOrSession(mMenuName, forAccount);
        }
    }

    /// <summary>
    /// Revokes a refresh token.
    /// </summary>
    /// <param name="token">The refresh token to revoke.</param>
    /// <param name="ipAddress">The IP address of the user revoking the token.</param>
    /// <param name="reason">The reason for revoking the token. (optional)</param>
    /// <param name="replacedByToken">The token that replaces the revoked token. (optional)</param>
    private void revokeRefreshToken(MRefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
    {
        token.Revoked = DateTime.UtcNow;
        token.RevokedByIp = ipAddress;
        token.ReasonRevoked = reason;
        token.ReplacedByToken = replacedByToken;
    }

    /// Recursively traverses the refresh token chain and ensures all descendants are revoked.
    /// </summary>
    /// <param name="refreshToken">The refresh token to start the traversal from.</param>
    /// <param name="account">The account profile associated with the refresh token.</param>
    /// <param name="ipAddress">The IP address of the requester.</param>
    /// <param name="reason">The reason for revoking the tokens.</param>
    private void revokeDescendantRefreshTokens(MRefreshToken refreshToken, MAccountProfile account, string ipAddress, string reason)
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
    /// Removes old refresh tokens from the given MAccountProfile.
    /// </summary>
    /// <param name="account"></param>
    private void removeOldRefreshTokens(MAccountProfile account)
    {
        account.RefreshTokens.RemoveAll(x =>
            !x.IsActive() &&
            x.Created.AddDays(ConfigSettings.RefreshTokenTTL) <= DateTime.UtcNow);
    }

    /// <summary>
    /// Inserts or updates account information
    /// </summary>
    /// <param name="accountProfile">MAccountProfile</param>
    /// <param name="saveRefreshTokens">Boolean</param>
    /// <param name="saveRoles">Boolean</param>
    /// <param name="saveGroups">Boolean</param>
    /// <param name="securityEntityProfile">MSecurityEntityProfile</param>
    /// <remarks>Changes will be reflected in the profile passed as a reference.</remarks>
    public MAccountProfile Save(MAccountProfile accountProfile, bool saveRefreshTokens, bool saveRoles, bool saveGroups, MSecurityEntity securityEntityProfile)
    {
        if (accountProfile == null) throw new ArgumentNullException("accountProfile", "accountProfile cannot be a null reference (Nothing in VB) or empty!");
        if (securityEntityProfile == null) throw new ArgumentNullException("securityEntityProfile", "securityEntityProfile cannot be a null reference (Nothing in VB) or empty!");
        BAccounts mBAccount = new BAccounts(securityEntityProfile, ConfigSettings.CentralManagement);
        mBAccount.Save(accountProfile, saveRefreshTokens, saveRoles, saveGroups);
        return accountProfile;
    }

    /// <summary>
    /// Inserts or updates account information
    /// </summary>
    /// <param name="accountProfile">MAccountProfile</param>
    /// <param name="saveRefreshTokens">Boolean</param>
    /// <param name="saveRoles">Boolean</param>
    /// <param name="saveGroups">Boolean</param>
    /// <remarks>Changes will be reflected in the profile passed as a reference.</remarks>
    public MAccountProfile Save(MAccountProfile accountProfile, bool saveRefreshTokens, bool saveRoles, bool saveGroups)
    {
        MSecurityEntity mSecurityEntityProfile = SecurityEntityUtility.CurrentProfile();
        return Save(accountProfile, saveRefreshTokens, saveRoles, saveGroups, mSecurityEntityProfile);
    }

    /// <summary>
    /// Verifies if a token exists in the system.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    private bool verificationTokenExists(string token)
    {
        MSecurityEntity mSecurityEntityProfile = SecurityEntityUtility.CurrentProfile();
        BAccounts mBAccount = new BAccounts(mSecurityEntityProfile, ConfigSettings.CentralManagement);
        return mBAccount.RefreshTokenExists(token);
    }
}

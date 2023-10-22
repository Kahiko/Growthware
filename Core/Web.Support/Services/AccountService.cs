using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.Json;
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

    // TODO: Cache is now avalible and should be used for the Anonymous account
    private CacheController m_CacheController = CacheController.Instance();

    private string s_SessionName = "SessionAccount";

    private IHttpContextAccessor m_HttpContextAccessor;

    [CLSCompliant(false)]
    public AccountService(IHttpContextAccessor httpContextAccessor)
    {
        this.m_HttpContextAccessor = httpContextAccessor;
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
        MAccountProfile mAccountProfile = GetAccount(mRequestedAccount, mForceDb);
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

    public string ChangePassword(UIChangePassword changePassword)
    {
        string mRetVal = string.Empty;
        MMessage mMessageProfile = new MMessage();
        MAccountProfile mAccountProfile = (MAccountProfile)m_HttpContextAccessor.HttpContext.Items["AccountProfile"];
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
                    string mJsonString = JsonSerializer.Serialize(mAccountProfile);
                    m_HttpContextAccessor.HttpContext.Session.SetString(s_SessionName, mJsonString);
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
                    string mJsonString = JsonSerializer.Serialize(mAccountProfile);
                    m_HttpContextAccessor.HttpContext.Session.SetString(s_SessionName, mJsonString);
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

    public void Delete(int accountSeqId)
    {
        BAccounts mBAccount = new BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
        mBAccount.Delete(accountSeqId);
    }

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
    public MAccountProfile GetAccount(String account, bool forceDb = false)
    {
        if (String.IsNullOrEmpty(account)) throw new ArgumentException("account can not be null or empty", account);
        MAccountProfile mRetVal = null;
        BAccounts mBAccount = null;
        // string mJsonString = string.Empty;
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
                    addToCacheOrSession(mSessionNameToUse, mRetVal);
                }
                else
                {
                    mRetVal = null;
                }
            }
            else
            {
                mRetVal = getFromCacheOrSession<MAccountProfile>(mSessionNameToUse);
                if (mRetVal == default)
                {
                    mBAccount = new BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
                    mRetVal = mBAccount.GetProfile(account);
                    addToCacheOrSession(mSessionNameToUse, mRetVal);
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

    public IList<MMenuTree> GetMenuItems(string account, MenuType menuType)
    {
        if (string.IsNullOrEmpty(account)) throw new ArgumentNullException("account", "account cannot be a null reference (Nothing in VB) or empty!");
        IList<MMenuTree> mRetVal = null;
        string mDataName = menuType.ToString() + "_" + account + "_Menu";
        string mJsonString = this.getStringData(account, mDataName);
        if (mJsonString != null && !String.IsNullOrEmpty(mJsonString))
        {
            mRetVal = JsonSerializer.Deserialize<IList<MMenuTree>>(mJsonString);
            return mRetVal;
        }
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
            this.setStringData(account, mDataName, JsonSerializer.Serialize(mRetVal));
        }
        return mRetVal;
    }

    private string getStringData(string account, string dataName)
    {
        if (account.ToLowerInvariant() != s_AnonymousAccount.ToLowerInvariant())
        {
            // TODO: should attempting to get from cache instead of session but the cache has not been developed yet
            return m_HttpContextAccessor.HttpContext.Session.GetString(dataName);
        }
        else
        {
            return m_HttpContextAccessor.HttpContext.Session.GetString(dataName);
        }
    }

    private void setStringData(string account, string dataName, string data)
    {
        if (account.ToLowerInvariant() != s_AnonymousAccount.ToLowerInvariant())
        {
            // TODO: should attempting to put the string into cache instead of session but the cache has not been developed yet
            m_HttpContextAccessor.HttpContext.Session.SetString(dataName, data);
        }
        else
        {
            m_HttpContextAccessor.HttpContext.Session.SetString(dataName, data);
        }
    }

    private MRefreshToken rotateRefreshToken(MRefreshToken refreshToken, string ipAddress)
    {
        var newRefreshToken = generateRefreshToken(ipAddress);
        revokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
        return newRefreshToken;
    }

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
                // _context.Update(account);
                // _context.SaveChanges();
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

    public bool RefreshTokenExists(string refreshToken)
    {
        MSecurityEntity mSecurityEntityProfile = SecurityEntityUtility.CurrentProfile();
        BAccounts mBAccount = new BAccounts(mSecurityEntityProfile, ConfigSettings.CentralManagement);
        return mBAccount.RefreshTokenExists(refreshToken);
    }

    private void revokeRefreshToken(MRefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
    {
        token.Revoked = DateTime.UtcNow;
        token.RevokedByIp = ipAddress;
        token.ReasonRevoked = reason;
        token.ReplacedByToken = replacedByToken;
    }

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

    private bool verificationTokenExists(string token)
    {
        MSecurityEntity mSecurityEntityProfile = SecurityEntityUtility.CurrentProfile();
        BAccounts mBAccount = new BAccounts(mSecurityEntityProfile, ConfigSettings.CentralManagement);
        return mBAccount.RefreshTokenExists(token);
    }


    private void addToCacheOrSession(string name, object value)
    {
        if (name.ToLowerInvariant() != s_AnonymousAccount.ToLowerInvariant())
        {
            SessionController.AddToSession(name, value);
            return;
        }
        this.m_CacheController.AddToCache(name, value);
    }

    private T getFromCacheOrSession<T>(string name)
    {
        if (name.ToLowerInvariant() != s_AnonymousAccount.ToLowerInvariant())
        {
            return SessionController.GetFromSession<T>(name);
        }
        return this.m_CacheController.GetFromCache<T>(name);
    }

    private void remmoveFromCacheOrSession(string name)
    {
        if (name.ToLowerInvariant() != s_AnonymousAccount.ToLowerInvariant())
        {
            SessionController.RemoveFromSession(name);
            return;
        }
        this.m_CacheController.RemoveFromCache(name);
    }

}

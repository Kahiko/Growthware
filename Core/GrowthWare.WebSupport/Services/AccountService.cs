using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
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
using GrowthWare.WebSupport.Jwt;
using GrowthWare.WebSupport.Utilities;

namespace GrowthWare.WebSupport.Services;
public class AccountService : IAccountService
{
    private MAccountProfile m_CachedAnonymousAccount = null;
    private string s_AnonymousAccount = "Anonymous";

    private string s_SessionName = "SessionAccount";

    private IHttpContextAccessor m_HttpContextAccessor;

    [CLSCompliant(false)]
    public AccountService(IHttpContextAccessor httpContextAccessor)
    {
        this.m_HttpContextAccessor = httpContextAccessor;
    }

    public MAccountProfile Authenticate(string account, string password, string ipAddress)
    {
        if (string.IsNullOrEmpty(account)) throw new ArgumentNullException("account", "account cannot be a null reference (Nothing in VB) or empty!");
        if (string.IsNullOrEmpty(account)) throw new ArgumentNullException("password", "password cannot be a null reference (Nothing in VB) or empty!");
        bool mAuthenticated = false;
        bool mDomainPassed = false;
        if (account.Contains(@"\"))
        {
            mDomainPassed = true;
        }
        MAccountProfile mAccountProfile = GetAccount(account, true);
        if (mDomainPassed && mAccountProfile == null)
        {
            int mDomainPos = account.IndexOf(@"\", StringComparison.OrdinalIgnoreCase);
            account = account.Substring(mDomainPos + 1, account.Length - mDomainPos - 1);
        }
        if (mAccountProfile != null)
        {
            if (ConfigSettings.AuthenticationType.ToUpper(CultureInfo.InvariantCulture) == "INTERNAL")
            {
                string mProfilePassword = string.Empty;
                try
                {
                    mProfilePassword = CryptoUtility.Decrypt(mAccountProfile.Password, SecurityEntityUtility.CurrentProfile().EncryptionType);
                }
                catch (CryptoUtilityException)
                {
                    mProfilePassword = mAccountProfile.Password;
                }
                if ((password == mProfilePassword || account.ToLowerInvariant() == s_AnonymousAccount.ToLowerInvariant()) && (mAccountProfile.Status != Convert.ToInt32(SystemStatus.Disabled, CultureInfo.InvariantCulture) || mAccountProfile.Status != Convert.ToInt32(SystemStatus.Inactive, CultureInfo.InvariantCulture)))
                {
                    mAuthenticated = true;
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

                }
                if (!mAuthenticated)
                {
                    mAccountProfile.FailedAttempts += 1;
                }
                if (mAccountProfile.FailedAttempts == Convert.ToInt32(ConfigSettings.FailedAttempts) && Convert.ToInt32(ConfigSettings.FailedAttempts, CultureInfo.InvariantCulture) != -1)
                {
                    mAccountProfile.Status = Convert.ToInt32(SystemStatus.Disabled, CultureInfo.InvariantCulture);
                }
                // TODO: Need to come up with an exception Anonymous like getting it keeping it in memory
                // and always handing that out and never saving it?
                Save(mAccountProfile, true, false, false);
                // mAccountProfile.PasswordLastSet = new DateTime(1941, 12, 7, 12, 0, 0);
                mAccountProfile.PasswordLastSet = DateTime.Now;
                mAccountProfile.Password = "";
            }
        }
        return mAccountProfile;
    }

    public string ChangePassword(UIChangePassword changePassword)
    {
        string mRetVal = string.Empty;
        MMessage mMessageProfile = new MMessage();
        MAccountProfile mAccountProfile = (MAccountProfile)m_HttpContextAccessor.HttpContext.Items["AccountProfile"];
        MSecurityEntity mSecurityEntity = SecurityEntityUtility.CurrentProfile();
        string mCurrentPassword = "";
        try
        {
            mCurrentPassword = CryptoUtility.Decrypt(mAccountProfile.Password, mSecurityEntity.EncryptionType);
        }
        catch (System.Exception)
        {
            mCurrentPassword = mAccountProfile.Password;
        }
        if(mAccountProfile.Status != (int)SystemStatus.ChangePassword) 
        {
            if(changePassword.OldPassword == mCurrentPassword)
            {
                mAccountProfile.PasswordLastSet = System.DateTime.Now;
                mAccountProfile.Status = (int)SystemStatus.Active;
                mAccountProfile.FailedAttempts = 0;
                mAccountProfile.Password = CryptoUtility.Encrypt(changePassword.NewPassword.Trim(), mSecurityEntity.EncryptionType, ConfigSettings.EncryptionSaltExpression);
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
                mAccountProfile.Password = CryptoUtility.Encrypt(changePassword.NewPassword.Trim(), mSecurityEntity.EncryptionType, ConfigSettings.EncryptionSaltExpression);
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
        try
        {
            BAccounts mBAccount = new BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            if (account != s_AnonymousAccount)
            {
                if (m_HttpContextAccessor.HttpContext != null && m_HttpContextAccessor.HttpContext.Session != null && m_HttpContextAccessor.HttpContext.Session.GetString(s_SessionName) != null && !forceDb)
                {
                    string mJsonString = m_HttpContextAccessor.HttpContext.Session.GetString(s_SessionName);
                    if (mJsonString != null && !String.IsNullOrEmpty(mJsonString)) 
                    {
                        mRetVal = JsonSerializer.Deserialize<MAccountProfile>(mJsonString);
                        if (mRetVal.Account.ToLowerInvariant() != account.ToLowerInvariant())
                        {
                            mRetVal = mBAccount.GetProfile(account);
                            mJsonString = JsonSerializer.Serialize(mRetVal);
                            m_HttpContextAccessor.HttpContext.Session.SetString(s_SessionName, mJsonString);
                        }
                    }
                    else
                    {
                        mRetVal = mBAccount.GetProfile(account);
                        mJsonString = JsonSerializer.Serialize(mRetVal);
                        m_HttpContextAccessor.HttpContext.Session.SetString(s_SessionName, mJsonString);
                    }
                }
                else
                {
                    mRetVal = mBAccount.GetProfile(account);
                    string mJsonString = JsonSerializer.Serialize(mRetVal);
                    m_HttpContextAccessor.HttpContext.Session.SetString(s_SessionName, mJsonString);
                }
            }
            else
            {
                // TODO: Add code to use session
                mRetVal = m_CachedAnonymousAccount;
                if (mRetVal == null)
                {
                    m_CachedAnonymousAccount = mBAccount.GetProfile(account);
                    mRetVal = m_CachedAnonymousAccount;
                }
            }
        }
        catch (InvalidOperationException)
        {
            String mMSG = "Account not find account: " + account + " in the database";
            Logger mLog = Logger.Instance();
            mLog.Error(mMSG);
        }
        catch (IndexOutOfRangeException)
        {
            String mMSG = "Account not find account: " + account + " in the database";
            Logger mLog = Logger.Instance();
            mLog.Error(mMSG);
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
}

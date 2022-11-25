using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Enumerations;
using GrowthWare.Framework.Models;
using GrowthWare.WebSupport.Utilities;

namespace GrowthWare.WebSupport.Services;
public class AccountService : IAccountService
{
    private MAccountProfile m_CachedAnonymousAccount = null;
    private String s_AnonymousAccount = "Anonymous";

    public AccountService() 
    {

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
        MAccountProfile mAccountProfile = GetAccount(account);
        if (mDomainPassed && mAccountProfile == null)
        {
            int mDomainPos = account.IndexOf(@"\", StringComparison.OrdinalIgnoreCase);
            account = account.Substring(mDomainPos + 1, account.Length - mDomainPos - 1);
        }        
        if(mAccountProfile != null)
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
                if (password == mProfilePassword && (mAccountProfile.Status != Convert.ToInt32(SystemStatus.Disabled, CultureInfo.InvariantCulture) || mAccountProfile.Status != Convert.ToInt32(SystemStatus.Inactive, CultureInfo.InvariantCulture)))
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
                Save(mAccountProfile, true, false, false);
                mAccountProfile.PasswordLastSet = new DateTime(1941, 12, 7, 12, 0, 0);
                mAccountProfile.Password = "";
            }
        }
        return mAccountProfile;
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
            Subject = new ClaimsIdentity(new[] { new Claim("account", account.Account) }),
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
        if(RefreshTokenExists(mToken))
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
        if(verificationTokenExists(mToken))
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
    public MAccountProfile GetAccount(String account)
    {
        if(String.IsNullOrEmpty(account)) {
            throw new ArgumentException("account can not be null or empty", account);
        }
        BAccounts mBAccount = new BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
        MAccountProfile mRetVal = null;
        try
        {
            if(account != s_AnonymousAccount)
            {
                mRetVal = mBAccount.GetProfile(account);
            }
            else
            {
                mRetVal = m_CachedAnonymousAccount;
                if(mRetVal == null)
                {
                    m_CachedAnonymousAccount = mBAccount.GetProfile(account);
                    mRetVal = m_CachedAnonymousAccount;
                }
            }
        }
        catch (InvalidOperationException)
        {
            String mMSG = "Count not find account: " + account + " in the database";
            Logger mLog = Logger.Instance();
            mLog.Error(mMSG);
        }
        catch (IndexOutOfRangeException)
        {
            String mMSG = "Count not find account: " + account + " in the database";
            Logger mLog = Logger.Instance();
            mLog.Error(mMSG);
        }
        return mRetVal;
    }

    private MAccountProfile getAccountByRefreshToken(string token)
    {
        if(String.IsNullOrEmpty(token)) 
        {
            throw new ArgumentException("token can not be null or empty", token);
        }
        BAccounts mBAccount = new BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
        MAccountProfile mRetVal = null;
        mRetVal = mBAccount.GetProfileByRefreshToken(token);
        return mRetVal;
    }

    private MAccountProfile getAccountByResetToken(string token)
    {
        if(String.IsNullOrEmpty(token)) 
        {
            throw new ArgumentException("token can not be null or empty", token);
        }
        BAccounts mBAccount = new BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
        MAccountProfile mRetVal = null;
        mRetVal = mBAccount.GetProfileByResetToken(token);
        return mRetVal;
    }

    public bool RefreshTokenExists(string refreshToken) 
    {
        MSecurityEntity mSecurityEntityProfile = SecurityEntityUtility.CurrentProfile();
        BAccounts mBAccount = new BAccounts(mSecurityEntityProfile, ConfigSettings.CentralManagement);
        return mBAccount.RefreshTokenExists(refreshToken);
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

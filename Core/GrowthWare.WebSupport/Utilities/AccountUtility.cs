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

namespace GrowthWare.WebSupport.Utilities;
public static class AccountUtility
{
    private static String s_CachedAnonymousAccount = "AnonymousProfile";
    private static String s_AnonymousAccount = "Anonymous";

    public static MAccountProfile Authenticate(string account, string password, string ipAddress)
    {
        if (string.IsNullOrEmpty(account)) throw new ArgumentNullException("account", "account cannot be a null reference (Nothing in VB) or empty!");
        if (string.IsNullOrEmpty(account)) throw new ArgumentNullException("password", "password cannot be a null reference (Nothing in VB) or empty!");
        bool mAuthenticated = false;
        bool mDomainPassed = false;
        if (account.Contains(@"\"))
        {
            mDomainPassed = true;
        }        
        MAccountProfile mAccountProfile = AccountUtility.GetAccount(account);
        if (mDomainPassed && mAccountProfile == null)
        {
            int mDomainPos = account.IndexOf(@"\", StringComparison.OrdinalIgnoreCase);
            account = account.Substring(mDomainPos + 1, account.Length - mDomainPos - 1);
            mAccountProfile = AccountUtility.GetAccount(account);
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

                    var jwtToken = generateJwtToken(mAccountProfile);
                    var refreshToken = generateRefreshToken(ipAddress);
                    mAccountProfile.Token = jwtToken;
                    mAccountProfile.RefreshTokens.Add(refreshToken);

                }
                if (!mAuthenticated) 
                { 
                    mAccountProfile.FailedAttempts += 1; 
                }
                if (mAccountProfile.FailedAttempts == Convert.ToInt32(ConfigSettings.FailedAttempts) && Convert.ToInt32(ConfigSettings.FailedAttempts, CultureInfo.InvariantCulture) != -1) 
                {
                    mAccountProfile.Status = Convert.ToInt32(SystemStatus.Disabled, CultureInfo.InvariantCulture);
                }
                AccountUtility.Save(mAccountProfile, false, false);
                mAccountProfile.PasswordLastSet = new DateTime(1941, 12, 7, 12, 0, 0);
                mAccountProfile.Password = "";
            }
        }
        return mAccountProfile;
    }

    public static void Delete(int accountSeqId)
    {
        BAccounts mBAccount = new BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
        mBAccount.Delete(accountSeqId);
    }

    private static string generateJwtToken(MAccountProfile account)
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

    public static string generateResetToken()
    {
        // token is a cryptographically strong random sequence of values
        var mToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        if(RefreshTokenExists(mToken))
        {
            generateResetToken();
        }
        return mToken;
    }

    public static RefreshToken generateRefreshToken(string ipAddress)
    {
        var mRetVal = new RefreshToken
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

    public static string generateVerificationToken()
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
    public static MAccountProfile GetAccount(String account)
    {
        if(String.IsNullOrEmpty(account)) {
            throw new ArgumentException("account can not be null or empty", account);
        }
        BAccounts mBAccount = new BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
        MAccountProfile mRetVal = null;
        try
        {
            mRetVal = mBAccount.GetProfile(account);
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

    private static MAccountProfile getAccountByRefreshToken(string token)
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

    private static MAccountProfile getAccountByResetToken(string token)
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

    public static bool RefreshTokenExists(string refreshToken) 
    {
        MSecurityEntity mSecurityEntityProfile = SecurityEntityUtility.CurrentProfile();
        BAccounts mBAccount = new BAccounts(mSecurityEntityProfile, ConfigSettings.CentralManagement);
        return mBAccount.RefreshTokenExists(refreshToken);
    }
    
    /// <summary>
    /// Inserts or updates account information
    /// </summary>
    /// <param name="accountProfile">MAccountProfile</param>
    /// <param name="saveRoles">Boolean</param>
    /// <param name="saveGroups">Boolean</param>
    /// <param name="securityEntityProfile">MSecurityEntityProfile</param>
    /// <remarks>Changes will be reflected in the profile passed as a reference.</remarks>
    public static MAccountProfile Save(MAccountProfile accountProfile, bool saveRoles, bool saveGroups, MSecurityEntity securityEntityProfile)
    {
        if (accountProfile == null) throw new ArgumentNullException("accountProfile", "accountProfile cannot be a null reference (Nothing in VB) or empty!");
        if (securityEntityProfile == null) throw new ArgumentNullException("securityEntityProfile", "securityEntityProfile cannot be a null reference (Nothing in VB) or empty!");
        BAccounts mBAccount = new BAccounts(securityEntityProfile, ConfigSettings.CentralManagement);
        mBAccount.Save(accountProfile, saveRoles, saveGroups);
        return accountProfile;
    }

    /// <summary>
    /// Inserts or updates account information
    /// </summary>
    /// <param name="accountProfile">MAccountProfile</param>
    /// <param name="saveRoles">Boolean</param>
    /// <param name="saveGroups">Boolean</param>
    /// <remarks>Changes will be reflected in the profile passed as a reference.</remarks>
    public static MAccountProfile Save(MAccountProfile accountProfile, bool saveRoles, bool saveGroups)
    {
        MSecurityEntity mSecurityEntityProfile = SecurityEntityUtility.CurrentProfile();
        return Save(accountProfile, saveRoles, saveGroups, mSecurityEntityProfile);
    }

    public static bool verificationTokenExists(string token)
    {
        MSecurityEntity mSecurityEntityProfile = SecurityEntityUtility.CurrentProfile();
        BAccounts mBAccount = new BAccounts(mSecurityEntityProfile, ConfigSettings.CentralManagement);
        return mBAccount.RefreshTokenExists(token);        
    }
}

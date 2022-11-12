using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;

namespace GrowthWare.WebSupport.Utilities;
public static class AccountUtility
{
    private static String s_CachedAnonymousAccount = "AnonymousProfile";
    private static String s_AnonymousAccount = "Anonymous";

    private static string generateJwtToken(MAccountProfile account)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(ConfigSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("account", account.Account) }),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public static string generateResetToken()
    {
        // token is a cryptographically strong random sequence of values
        var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        if(RefreshTokenExists(token))
        {
            generateResetToken();
        }
        return token;
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

    /// <summary>
    /// Inserts or updates account information
    /// </summary>
    /// <param name="profile">MAccountProfile</param>
    /// <param name="saveRoles">Boolean</param>
    /// <param name="saveGroups">Boolean</param>
    /// <param name="securityEntityProfile">MSecurityEntityProfile</param>
    /// <remarks>Changes will be reflected in the profile passed as a reference.</remarks>
    public static MAccountProfile Save(MAccountProfile profile, bool saveRoles, bool saveGroups, MSecurityEntity securityEntityProfile)
    {
        if (profile == null) throw new ArgumentNullException("profile", "profile cannot be a null reference (Nothing in VB) or empty!");
        if (securityEntityProfile == null) throw new ArgumentNullException("securityEntityProfile", "securityEntityProfile cannot be a null reference (Nothing in VB) or empty!");
        BAccounts mBAccount = new BAccounts(securityEntityProfile, ConfigSettings.CentralManagement);
        mBAccount.Save(profile, saveRoles, saveGroups);
        return profile;
    }

    /// <summary>
    /// Inserts or updates account information
    /// </summary>
    /// <param name="profile">MAccountProfile</param>
    /// <param name="saveRoles">Boolean</param>
    /// <param name="saveGroups">Boolean</param>
    /// <remarks>Changes will be reflected in the profile passed as a reference.</remarks>
    public static MAccountProfile Save(MAccountProfile profile, bool saveRoles, bool saveGroups)
    {
        MSecurityEntity mSecurityEntityProfile = SecurityEntityUtility.CurrentProfile();
        return Save(profile, saveRoles, saveGroups, mSecurityEntityProfile);
    }

    public static bool RefreshTokenExists(string refreshToken) 
    {
        MSecurityEntity mSecurityEntityProfile = SecurityEntityUtility.CurrentProfile();
        BAccounts mBAccount = new BAccounts(mSecurityEntityProfile, ConfigSettings.CentralManagement);
        return mBAccount.RefreshTokenExists(refreshToken);
    }

}

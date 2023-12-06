using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Web.Support.Jwt;

namespace GrowthWare.Web.Support.Utilities;

public static class TokenUtility
{

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
                AccountUtility.Save(mAccountProfile, true, false, false);
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
            // TODO: Considering calling AccountUtility.Authenticate instead of AccountUtility.Save and 
            //          ClientChoicesUtility.SynchronizeContext
            AccountUtility.Save(mAccountProfile, true, false, false);
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
    /// Sets the tokens for the provided account profile and IP address.
    /// </summary>
    /// <param name="accountProfile">The account profile to set the tokens for.</param>
    /// <param name="ipAddress">The IP address associated with the tokens.</param>
    /// <returns>The updated account profile with the tokens set.</returns>
    public static MAccountProfile SetTokens(MAccountProfile accountProfile, string ipAddress)
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
            issuer: ConfigSettings.Issuer,
            audience: ConfigSettings.Audience,
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
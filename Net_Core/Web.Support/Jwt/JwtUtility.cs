using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Web.Support.Utilities;

namespace GrowthWare.Web.Support.Jwt;
public class JwtUtility : IJwtUtility
{
    private static BAccounts m_BusinessLogic = null;

    public JwtUtility()
    {
        // nothing atm
    }

    /// <summary>
    /// Returns the business logic object used to access the database.
    /// </summary>
    /// <returns></returns>
    private static BAccounts BusinessLogic
    {
        get
        {
            if(m_BusinessLogic == null || ConfigSettings.CentralManagement == true)
            {
                m_BusinessLogic = new(SecurityEntityUtility.CurrentProfile);
            }
            return m_BusinessLogic;
        }
    }

    /// <summary>
    /// Generate a JWT token for the given account profile.
    /// </summary>
    /// <param name="account">The account profile for which the token is generated.</param>
    /// <returns>The generated JWT token.</returns>
    public string GenerateJwtToken(MAccountProfile account)
    {
        // generate token that is valid for 15 minutes
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(ConfigSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { 
                new Claim("account", account.Account.ToString()),
                new Claim("status", account.Status.ToString()) 
            }),
            Expires = DateTime.UtcNow.AddMinutes(ConfigSettings.JWT_Token_TTL_Minutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Validates a JWT token.
    /// </summary>
    /// <param name="token">The JWT token to be validated.</param>
    /// <returns>
    /// The account from the JWT token if validation is successful.
    /// Null if validation fails or the token is null.
    /// </returns>
    public string ValidateJwtToken(string token)
    {
        if (token == null)
        {
            return null;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(ConfigSettings.Secret);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var mJwtToken = (JwtSecurityToken)validatedToken;
            var mAccount = mJwtToken.Claims.First(x => x.Type == "account").Value;

            // return account id from JWT token if validation successful
            return mAccount;
        }
        catch
        {
            // return null if validation fails
            return null;
        }
    }

    /// <summary>
    /// Generates a refresh token for a given IP address.
    /// </summary>
    /// <param name="ipAddress">The IP address of the client requesting the refresh token.</param>
    /// <returns>An instance of MRefreshToken representing the generated refresh token.</returns>
    public MRefreshToken GenerateRefreshToken(string ipAddress, int accountSeqId)
    {
        var refreshToken = new MRefreshToken
        {
            AccountSeqId = accountSeqId,
            // token is a cryptographically strong random sequence of values
            Token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64)),
            // token is valid for 7 days
            Expires = DateTime.UtcNow.AddDays(ConfigSettings.JWT_Refresh_Token_Expires_Days),
            Created = DateTime.UtcNow,
            CreatedByIp = ipAddress
        };

        // ensure token is unique by checking against db
        // var tokenIsUnique = !_context.Accounts.Any(x => x.ResetToken == token);
        var tokenIsUnique = BusinessLogic.RefreshTokenExists(refreshToken.Token);

        if (!tokenIsUnique)
            return GenerateRefreshToken(ipAddress, accountSeqId);

        return refreshToken;
    }

    /// <summary>
    /// Generates a unique reset token.
    /// </summary>
    /// <returns>string</returns>
    public static string GenerateResetToken()
    {
        string mRetVal = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

        bool mTokenIsUnique = BusinessLogic.ResetTokenExists(mRetVal);

        if (!mTokenIsUnique)
        {
            return GenerateResetToken();
        }
        
        return mRetVal;
    }

    /// <summary>
    /// Generates a unique verification token.
    /// </summary>
    /// <returns>string</returns>
    public string GenerateVerificationToken()
    {
        var mRetVal = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        // ensure token is unique by checking against db
        bool mTokenExists = BusinessLogic.VerificationTokenExists(mRetVal);
        if (mTokenExists)
        {
            return GenerateVerificationToken();
        }
        return mRetVal;
    }
}
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
public class JwtUtils : IJwtUtils
{

    public JwtUtils()
    {
        // nothing atm
    }

    public string GenerateJwtToken(MAccountProfile account)
    {
        // generate token that is valid for 15 minutes
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(ConfigSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("account", account.Account.ToString()) }),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string ValidateJwtToken(string token)
    {
        if (token == null)
            return null;

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
                // set ClockSkew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var mJwtToken = (JwtSecurityToken)validatedToken;
            var mAccount = mJwtToken.Claims.First(x => x.Type == "account").Value;

            // return account id from JWT token if validation successful
            return mAccount;
        }
        catch(SecurityTokenExpiredException) 
        {
            // do nothing ... error is acceptable
            return null;  
        }
        catch(Exception ex)
        {
            Logger mLogger = Logger.Instance();
            mLogger.Debug(ex);
            // return null if validation fails
            return null;
        }
    }

    public MRefreshToken GenerateRefreshToken(string ipAddress)
    {
        var refreshToken = new MRefreshToken
        {
            // token is a cryptographically strong random sequence of values
            Token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64)),
            // token is valid for 7 days
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow,
            CreatedByIp = ipAddress
        };

        // ensure token is unique by checking against db
        MSecurityEntity mSecurityEntityProfile = SecurityEntityUtility.CurrentProfile();
        BAccounts mBAccount = new BAccounts(mSecurityEntityProfile, ConfigSettings.CentralManagement);
        var tokenIsUnique = mBAccount.RefreshTokenExists(refreshToken.Token);

        if (!tokenIsUnique)
            return GenerateRefreshToken(ipAddress);

        return refreshToken;
    }
}
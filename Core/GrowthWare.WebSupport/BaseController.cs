using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using GrowthWare.Framework;
using GrowthWare.Framework.Enumerations;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.WebSupport.Utilities;

namespace GrowthWare.WebSupport;

[Controller]
[CLSCompliant(false)]
public abstract class BaseController : ControllerBase
{
    // returns the current authenticated account (null if not logged in)
    public MAccountProfile Account => (MAccountProfile)HttpContext.Items["AccountProfile"];
    // returns the current authenticated accounts client choices (null if not logged in)
    public MClientChoices ClientChoices => (MClientChoices)HttpContext.Items["ClientChoices"];
    // returns the current security entity (default as defined in GrowthWare.json)
    public MSecurityEntity SecurityEntity => (MSecurityEntity)HttpContext.Items["SecurityEntity"];

    [HttpPost("Authenticate")]
    public ActionResult<MAccountProfile> Authenticate(string account, string password)
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
                        new Claim(ClaimTypes.Name, mAccountProfile.Account), 
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
        } else 
        {
            return StatusCode(403, "Incorrect account or password");
        }
        return Ok(mAccountProfile);
    }

}
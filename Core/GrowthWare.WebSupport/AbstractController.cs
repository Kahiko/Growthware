using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.WebSupport.Utilities;
using GrowthWare.WebSupport.Utilities.Jwt;

namespace GrowthWare.WebSupport;

[CLSCompliant(false)]
public abstract class AbstractController : ControllerBase
{
    // returns the current authenticated account (null if not logged in)
    public MAccountProfile Account => (MAccountProfile)HttpContext.Items["AccountProfile"];
    // returns the current authenticated accounts client choices (null if not logged in)
    public MClientChoices ClientChoices => (MClientChoices)HttpContext.Items["ClientChoices"];
    // returns the current security entity (default as defined in GrowthWare.json)
    public MSecurityEntity SecurityEntity => (MSecurityEntity)HttpContext.Items["SecurityEntity"];

    [AllowAnonymous]
    [HttpPost("Authenticate")]
    public ActionResult<AuthenticationResponse> Authenticate(string account, string password)
    {
        MAccountProfile mAccountProfile = AccountUtility.Authenticate(account, password, ipAddress());
        if (mAccountProfile == null)
        {
            HttpContext.Items["AccountProfile"] = AccountUtility.GetAccount("Anonymous");
            return StatusCode(403, "Incorrect account or password");
        }
        AuthenticationResponse mAuthenticationResponse = new AuthenticationResponse(mAccountProfile);
        setTokenCookie(mAuthenticationResponse.RefreshToken);
        HttpContext.Items["AccountProfile"] = mAccountProfile;
        return Ok(mAuthenticationResponse);
    }

    /// <summary>
    /// Example of how to delete an account
    /// </summary>
    /// <param name="accountSeqId"></param>
    /// <returns>ActionResult</returns>
    private IActionResult DeleteAccount(int accountSeqId)
    {
        // This is here only for example it is this developers view that deleting accounts
        // is extremely risky and should be left to say a backend developer (DBA if you like)
        // it is possible for data to be associated with an account outside the realms of this
        // application and deleting it here could be quite an issue

        // TODO: Finish code for the example
        if (accountSeqId <= 0) throw new ArgumentNullException("accountSeqId", " must be a positive number!");
        string mRetVal = "False";
        Logger mLog = Logger.Instance();
        // if (HttpContext.Items["EditId"] != null)
        // {
        //     int mEditId = int.Parse(HttpContext.Items["EditId"].ToString());
        //     if (mEditId == accountSeqId)
        //     {
        //         MSecurityInfo mSecurityInfo = new MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditOtherAccount", true)), AccountUtility.CurrentProfile());
        //         if (mSecurityInfo != null)
        //         {
        //             if (mSecurityInfo.MayDelete)
        //             {
        //                 try
        //                 {
        //                     AccountUtility.Delete(accountSeqId);
        //                     mRetVal = "True";
        //                 }
        //                 catch (Exception ex)
        //                 {
        //                     mLog.Error(ex);
        //                     throw;
        //                 }
        //             }
        //             else
        //             {
        //                 Exception mError = new Exception("The account (" + AccountUtility.CurrentProfile().Account + ") being used does not have the correct permissions to delete");
        //                 mLog.Error(mError);
        //                 return this.InternalServerError(mError);
        //             }
        //         }
        //         else
        //         {
        //             Exception mError = new Exception("Security Info can not be determined nothing has been deleted!!!!");
        //             mLog.Error(mError);
        //             return this.InternalServerError(mError);
        //         }
        //     }
        //     else
        //     {
        //         Exception mError = new Exception("Identifier you have last looked at does not match the one passed in nothing has been saved!!!!");
        //         mLog.Error(mError);
        //         return this.InternalServerError(mError);
        //     }
        // }
        return Ok(mRetVal);
    }

    private void setTokenCookie(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append("refreshToken", token, cookieOptions);
    }

    private string ipAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
            return Request.Headers["X-Forwarded-For"];
        else
            return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
    }
}
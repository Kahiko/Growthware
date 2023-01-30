using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.WebSupport.Services;
using GrowthWare.WebSupport.Jwt;
using GrowthWare.WebSupport.Utilities;

namespace GrowthWare.WebSupport.BaseControllers;

[CLSCompliant(false)]
public abstract class AbstractAccountController : ControllerBase
{
    protected IAccountService m_AccountService;
    private Logger m_Logger = Logger.Instance();
    private string s_AnonymousAccount = "Anonymous";


    [AllowAnonymous]
    [HttpPost("Authenticate")]
    public ActionResult<AuthenticationResponse> Authenticate(string account, string password)
    {
        MAccountProfile mAccountProfile = m_AccountService.Authenticate(account, password, ipAddress());
        if (mAccountProfile == null)
        {
            HttpContext.Items["AccountProfile"] = m_AccountService.GetAccount("Anonymous");
            return StatusCode(403, "Incorrect account or password");
        }
        AuthenticationResponse mAuthenticationResponse = new AuthenticationResponse(mAccountProfile);
        setTokenCookie(mAuthenticationResponse.RefreshToken);
        return Ok(mAuthenticationResponse);
    }

    [Authorize("ChangePassword")]
    [HttpPost("ChangePassword")]
    public ActionResult ChangePassword(string oldPassword, string newPassword)
    {
        UIChangePassword mChangePassword = new UIChangePassword();
        mChangePassword.OldPassword = oldPassword;
        mChangePassword.NewPassword = newPassword;
        // if (mChangePassword. <= 0) throw new ArgumentNullException("accountSeqId", " must be a positive number!");
        if(mChangePassword.NewPassword.Length == 0) throw new ArgumentNullException("NewPassword", " can not be blank");
        if(mChangePassword.OldPassword.Length == 0) throw new ArgumentNullException("OldPassword", " can not be blank");
        string mRetVal = m_AccountService.ChangePassword(mChangePassword);
        return Ok(mRetVal);
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
        //                     this.m_Logger.Error(ex);
        //                     throw;
        //                 }
        //             }
        //             else
        //             {
        //                 Exception mError = new Exception("The account (" + AccountUtility.CurrentProfile().Account + ") being used does not have the correct permissions to delete");
        //                 this.m_Logger.Error(mError);
        //                 return this.InternalServerError(mError);
        //             }
        //         }
        //         else
        //         {
        //             Exception mError = new Exception("Security Info can not be determined nothing has been deleted!!!!");
        //             this.m_Logger.Error(mError);
        //             return this.InternalServerError(mError);
        //         }
        //     }
        //     else
        //     {
        //         Exception mError = new Exception("Identifier you have last looked at does not match the one passed in nothing has been saved!!!!");
        //         this.m_Logger.Error(mError);
        //         return this.InternalServerError(mError);
        //     }
        // }
        return Ok(mRetVal);
    }

    [HttpGet("EditAccount")]
    public ActionResult<MAccountProfile> EditAccount(string account)
    {
        MAccountProfile mRequestingProfile = (MAccountProfile)HttpContext.Items["AccountProfile"];
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.Actions_EditAccount);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        if(mSecurityInfo.MayEdit)
        {
            MAccountProfile mAccountProfile = this.getAccount(account);
            HttpContext.Session.SetInt32("EditId", mAccountProfile.Id);
            return Ok(mAccountProfile);
        }
        else if(mSecurityInfo.MayView)
        {
            MAccountProfile mAccountProfile = this.getAccount(account);
            HttpContext.Session.Remove("EditId");
            return Ok(mAccountProfile);
        }
        else
        {
            return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
        }
    }

    [HttpGet("NewProfile")]
    public ActionResult<MAccountProfile> NewProfile(string account)
    {
        return new MAccountProfile();
    }


    [HttpGet("EditProfile")]
    public ActionResult<MAccountProfile> EditProfile(string account)
    {
        MAccountProfile mRequestingProfile = (MAccountProfile)HttpContext.Items["AccountProfile"];
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.Actions_EditAccount);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        if(mRequestingProfile.Account.ToLowerInvariant() == account.ToLowerInvariant())
        {
            MAccountProfile mAccountProfile = this.getAccount(account);
            HttpContext.Session.SetInt32("EditId", mAccountProfile.Id);
            return Ok(mAccountProfile);
        }
        else
        {
            return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
        }
    }

    private MAccountProfile getAccount(string account)
    {
        MAccountProfile mRetVal = new MAccountProfile();
        if(!String.IsNullOrWhiteSpace(account) && account != "_")
        {
            mRetVal = m_AccountService.GetAccount(account);
        }
        if(mRetVal == null)
        {
            // mRetVal = AccountUtility.GetAccount("Anonymous");
            mRetVal = new MAccountProfile();
        }
        mRetVal.Password = string.Empty;
        return mRetVal;
    }


    [HttpGet("GetLinks")]
    public List<MNavLink> GetLinks()
    {
        List<MNavLink> mRootNavLinks = new List<MNavLink>();
        MNavLink mNavLink;
        MAccountProfile mAccountProfile = (MAccountProfile)HttpContext.Items["AccountProfile"];
        if(mAccountProfile != null && mAccountProfile.Account.ToLowerInvariant() != this.s_AnonymousAccount.ToLowerInvariant()) 
        {
            mNavLink = new MNavLink("home", "home", "Home");
            mRootNavLinks.Add(mNavLink);
            mNavLink = new MNavLink("dialpad", "counter", "Counter");
            mRootNavLinks.Add(mNavLink);
            mNavLink = new MNavLink("thermostat", "fetch-data", "Fetch Data");
            mRootNavLinks.Add(mNavLink);
            mNavLink = new MNavLink("api", "swagger", "API", false);
            mRootNavLinks.Add(mNavLink);
            // Nested Administration links
            MNavLink mAdminNavLink = new MNavLink("admin_panel_settings", "", "Administration", false);

            MNavLink mAdminChild = new MNavLink("groups", "manage-groups", "Manage Groups");
            mAdminNavLink.Children.Add(mAdminChild);

            mAdminChild = new MNavLink("manage_roles", "manage-roles", "Manage Roles");
            mAdminNavLink.Children.Add(mAdminChild);

            mAdminChild = new MNavLink("manage_accounts", "accounts", "Manage Accounts");
            mAdminNavLink.Children.Add(mAdminChild);

            mAdminChild = new MNavLink("functions", "search-functions", "Manage Functions");
            mAdminNavLink.Children.Add(mAdminChild);

            mRootNavLinks.Add(mAdminNavLink);
        } else 
        {
            mNavLink = new MNavLink("home", "generic_home", "Home");
            mRootNavLinks.Add(mNavLink);
        }
        return mRootNavLinks;
    }

    [HttpGet("GetRoles")]
    public ActionResult<ArrayList> GetRoles()
    {
        ArrayList mRetVal = RoleUtility.GetRolesArrayListBySecurityEntity(SecurityEntityUtility.CurrentProfile().Id);
        return Ok(mRetVal);
    }


    private string ipAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
            return Request.Headers["X-Forwarded-For"];
        else
            return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
    }


    [HttpGet("Logoff")]
    public ActionResult<AuthenticationResponse> Logoff()
    { 
        return this.Authenticate(this.s_AnonymousAccount, "none");
    }

    [HttpPost("RefreshToken")]
    public ActionResult<AuthenticationResponse> RefreshToken()
    {
        try
        {
            var mRefreshToken = Request.Cookies["refreshToken"];
            AuthenticationResponse mAuthenticationResponse = m_AccountService.RefreshToken(mRefreshToken, ipAddress());
            setTokenCookie(mAuthenticationResponse.RefreshToken);
            return Ok(mAuthenticationResponse);
        }
        catch (System.Exception ex)
        {
            if(ex.Message.Contains("token does not exist"))
            {
                return NotFound();
                // throw;
            }
            else
            {
                this.m_Logger.Error(ex);
                throw new Exception("token does not exist, unable to get account");
            }
        }
    }

    public ActionResult<bool> Save(UIAccountProfile uiAccountProfile)
    {
        // requesting profile same as 
        bool mRetVal = false;

        return Ok(mRetVal);
    }

    [Authorize("Search_Accounts")]
    [HttpPost("SearchAccounts")]
    public String SearchAccounts(UISearchCriteria searchCriteria)
    {
        String mRetVal = string.Empty;
        string mColumns = "[AccountSeqId], [Account], [First_Name], [Last_Name], [Email], [Added_Date], [Last_Login]";
        if(searchCriteria.sortColumns.Length > 0)
        {
            Tuple<string, string> mOrderByAndWhere = SearchUtility.GetOrderByAndWhere(mColumns, searchCriteria.searchColumns, searchCriteria.sortColumns, searchCriteria.searchText);
            string mOrderByClause = mOrderByAndWhere.Item1;
            string mWhereClause = mOrderByAndWhere.Item2;
            MSearchCriteria mSearchCriteria = new MSearchCriteria
            {
                Columns = mColumns,
                OrderByClause = mOrderByClause,
                PageSize = searchCriteria.pageSize,
                SelectedPage = searchCriteria.selectedPage,
                TableOrView = "[ZGWSecurity].[Accounts]",
                WhereClause = mWhereClause
            };

            mRetVal = SearchUtility.GetSearchResults(mSearchCriteria);
        }
        return mRetVal;
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
}
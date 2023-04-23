using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.Web.Support.Services;
using GrowthWare.Web.Support.Jwt;
using GrowthWare.Web.Support.Utilities;
using GrowthWare.Framework.Enumerations;

namespace GrowthWare.Web.Support.BaseControllers;

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
        HttpContext.Session.Remove("EditId");
        MAccountProfile mAccountProfile = this.m_AccountService.GetAccount(account, true, false);
        if(mSecurityInfo.MayEdit)
        {
            HttpContext.Session.SetInt32("EditId", mAccountProfile.Id);
        }
        else
        {
            return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
        }
        return Ok(mAccountProfile);
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

    [HttpGet("EncryptDecrypt")]
    public ContentResult EncryptDecrypt(string txtValue, int encryptionType, bool encrypt)
    {
        string mRetVal = string.Empty;
        EncryptionType mEncryptionType = (EncryptionType)encryptionType;
        if(encrypt) 
        {
            CryptoUtility.TryEncrypt(txtValue, out mRetVal, mEncryptionType);
        } 
        else 
        {
            CryptoUtility.TryDecrypt(txtValue, out mRetVal, mEncryptionType);
        }
        return Content(mRetVal);
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
    public List<MNavLink> GetLinks(int menuType)
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
            MNavLink mAdminLinks = new MNavLink("admin_panel_settings", "", "Administration", false);

            MNavLink mChildLink = new MNavLink("groups", "manage-groups", "Manage Groups");
            mAdminLinks.Children.Add(mChildLink);

            mChildLink = new MNavLink("manage_roles", "manage-roles", "Manage Roles");
            mAdminLinks.Children.Add(mChildLink);

            mChildLink = new MNavLink("manage_accounts", "accounts", "Manage Accounts");
            mAdminLinks.Children.Add(mChildLink);

            mChildLink = new MNavLink("functions", "functions", "Manage Functions");
            mAdminLinks.Children.Add(mChildLink);

            mChildLink = new MNavLink("folder_shared", "manage_cache_dependency", "Manage Cache Dependency");
            mAdminLinks.Children.Add(mChildLink);

            mChildLink = new MNavLink("folder_shared", "manage_logs", "Manage Logs");
            mAdminLinks.Children.Add(mChildLink);
            // Nested Administration\Security links
            MNavLink mSecurityLinks = new MNavLink("admin_panel_settings", "", "Security", false);
            mChildLink = new MNavLink("enhanced_encryption", "security", "Encryption Helper");
            mSecurityLinks.Children.Add(mChildLink);

            mChildLink = new MNavLink("admin_panel_settings", "security/guid-helper", "GUID Helper");
            mSecurityLinks.Children.Add(mChildLink);

            mChildLink = new MNavLink("shuffle", "security/random-numbers", "Random number Helper");
            mSecurityLinks.Children.Add(mChildLink);
            // add the security lings to the administration links
            mAdminLinks.Children.Add(mSecurityLinks);

            mRootNavLinks.Add(mAdminLinks);

        } else 
        {
            mNavLink = new MNavLink("home", "generic_home", "Home");
            mRootNavLinks.Add(mNavLink);
        }
        return mRootNavLinks;
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

    [HttpPost("SaveAccount")]
    public ActionResult<bool> SaveAccount(MAccountProfile accountProfile)
    {
        // requesting profile same as 
        bool mRetVal = false;
        MAccountProfile mRequestingProfile = (MAccountProfile)HttpContext.Items["AccountProfile"];
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile("SaveAccount");
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        MSecurityInfo mSecurityInfo_View_Account_Group = new MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.View_Account_Group_Tab), mRequestingProfile);
        MSecurityInfo mSecurityInfo_View_Account_Role = new MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.View_Account_Role_Tab), mRequestingProfile);
        var mEditId =  HttpContext.Session.GetInt32("EditId");
        if(mEditId != null && (mSecurityInfo.MayAdd || mSecurityInfo.MayEdit)) 
        {
            // we don't want to save the of the properties from the UI so we get the profile from the DB
            MAccountProfile mExistingAccount = m_AccountService.GetAccount(accountProfile.Account, true, false);
            if(mExistingAccount == null) 
            {
                mExistingAccount = new MAccountProfile();
                mExistingAccount.Id = -1;
                mExistingAccount.Password = ""; // should be auto generated and 
            }
            mExistingAccount.Account = accountProfile.Account;
            mExistingAccount.AssignedRoles = accountProfile.AssignedRoles;
            mExistingAccount.Email = accountProfile.Email;
            mExistingAccount.EnableNotifications = accountProfile.EnableNotifications;
            mExistingAccount.FirstName = accountProfile.FirstName;
            mExistingAccount.Groups = accountProfile.Groups;
            mExistingAccount.Id = accountProfile.Id;
            mExistingAccount.LastName = accountProfile.LastName;
            mExistingAccount.Location = accountProfile.Location;
            mExistingAccount.MiddleName = accountProfile.MiddleName;
            mExistingAccount.Name = accountProfile.Account;
            mExistingAccount.PreferredName = accountProfile.PreferredName;
            mExistingAccount.Status = accountProfile.Status;
            mExistingAccount.TimeZone = accountProfile.TimeZone;
            mExistingAccount.UpdatedBy = mRequestingProfile.Id;
            mExistingAccount.UpdatedDate = DateTime.Now;
            this.m_AccountService.Save(mExistingAccount, false, mSecurityInfo_View_Account_Role.MayView, mSecurityInfo_View_Account_Role.MayView);
            mRetVal = true;
        }
        else
        {
            this.m_Logger.Error(mRequestingProfile.Account + " does not have permissions to 'SaveAccount'");
        }
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
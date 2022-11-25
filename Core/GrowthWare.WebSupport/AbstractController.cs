using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.WebSupport.Services;
using GrowthWare.WebSupport.Utilities;
using GrowthWare.WebSupport.Utilities.Jwt;

namespace GrowthWare.WebSupport;

[CLSCompliant(false)]
public abstract class AbstractController : ControllerBase
{

    protected IAccountService m_AccountService;
    private string m_ApplicationName = string.Empty;
    private string m_Version = string.Empty;
    private string m_LogPriority = string.Empty;

    // returns the current authenticated account (null if not logged in)
    public MAccountProfile m_AccountProfile => (MAccountProfile)HttpContext.Items["AccountProfile"];
    // returns the current authenticated accounts client choices (null if not logged in)
    public MClientChoices m_ClientChoices => (MClientChoices)HttpContext.Items["ClientChoices"];
    // returns the current security entity (default as defined in GrowthWare.json)
    public MSecurityEntity m_SecurityEntity => (MSecurityEntity)HttpContext.Items["SecurityEntity"];

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

    [HttpGet("GetAccount")]
    public MAccountProfile GetAccount(string account)
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

    [HttpGet("GetAppSettings")]
    public UIAppSettings GetAppSettings()
    {
        UIAppSettings mRetVal = new UIAppSettings();
        if(this.m_LogPriority == string.Empty)
        {
            this.m_LogPriority = ConfigSettings.LogPriority;
        }
        if(this.m_ApplicationName == string.Empty)
        {
            this.m_ApplicationName = ConfigSettings.AppDisplayedName;
        }
        if(this.m_Version == string.Empty)
        {
            this.m_Version = ConfigSettings.Version;
        }
        mRetVal.LogPriority = this.m_LogPriority;
        mRetVal.Name = this.m_ApplicationName;
        mRetVal.Version = this.m_Version;
        return mRetVal;
    }

    [HttpGet("GetLinks")]
    public List<MNavLink> GetLinks()
    {
        List<MNavLink> mRootNavLinks = new List<MNavLink>();
        MNavLink mNavLink;
        if(this.m_AccountProfile != null && this.m_AccountProfile.Account != "Anonymous") 
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

            mAdminChild = new MNavLink("manage_accounts", "search-accounts", "Manage Accounts");
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

    [HttpGet("GetSecurityInfo")]
    public MSecurityInfo GetSecurityInfo(string action) 
    { 
        MSecurityInfo mRetVal = new MSecurityInfo();
        if (action == null || string.IsNullOrEmpty(action)) throw new ArgumentNullException("action", " can not be null or blank!");
        
        return mRetVal;
    }

    private string ipAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
            return Request.Headers["X-Forwarded-For"];
        else
            return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
    }

    [HttpPost("Log")]
    public bool Log(MLoggingProfile profile)
    {
        // MLoggingProfile mProfile = new MLoggingProfile(profile);
        LoggingUtility.Save(profile);
        return true;
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

    [Authorize("Search_Functions")]
    [HttpPost("SearchFunctions")]
    public String SearchFunctions(UISearchCriteria searchCriteria)
    {
        String mRetVal = string.Empty;
        string mColumns = "[FunctionSeqId], [Name], [Description], [Action], [Added_By], [Added_Date], [Updated_By], [Updated_Date]";
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
                TableOrView = "[ZGWSystem].[vwSearchFunctions]",
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
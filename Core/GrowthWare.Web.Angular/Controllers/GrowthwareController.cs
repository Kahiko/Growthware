using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.WebSupport;
using GrowthWare.WebSupport.Utilities;

namespace GrowthWare.Web.Angular.Controllers;

[ApiController]
[Route("[controller]")]
public class GrowthwareAPIController : BaseController
{
    private readonly ILogger<GrowthwareAPIController> _logger;
    private string m_ApplicationName = string.Empty;
    private string m_Version = string.Empty;
    private string m_LogPriority = string.Empty;


    public GrowthwareAPIController(ILogger<GrowthwareAPIController> logger)
    {
        _logger = logger;
    }

    [HttpGet("GetAccount")]
    public MAccountProfile GetAccount(string account)
    {
        MAccountProfile mRetVal = new MAccountProfile();
        if(!String.IsNullOrWhiteSpace(account) && account != "_")
        {
            mRetVal = AccountUtility.GetAccount(account);
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
        List<MNavLink> mRetVal = new List<MNavLink>();
        MNavLink mNavLink;
        if(this.Account != null) 
        {
            mNavLink = new MNavLink("home", "home", "Home");
            mRetVal.Add(mNavLink);
            mNavLink = new MNavLink("dialpad", "counter", "Counter");
            mRetVal.Add(mNavLink);
            mNavLink = new MNavLink("thermostat", "fetch-data", "Fetch Data");
            mRetVal.Add(mNavLink);
            mNavLink = new MNavLink("api", "swagger", "API", false);
            mRetVal.Add(mNavLink);
            // Nested Administration link
            MNavLink mAdminNavLink = new MNavLink("admin_panel_settings", "", "Administration", false);

            MNavLink mAdminChild = new MNavLink("groups", "search-accounts", "Manage Groups");
            mAdminNavLink.Children.Add(mAdminChild);

            mAdminChild = new MNavLink("manage_roles", "search-accounts", "Manage Roles");
            mAdminNavLink.Children.Add(mAdminChild);

            mAdminChild = new MNavLink("manage_accounts", "search-accounts", "Manage Accounts");
            mAdminNavLink.Children.Add(mAdminChild);
            mAdminChild = new MNavLink("functions", "search-functions", "Manage Functions");
            mAdminNavLink.Children.Add(mAdminChild);
            mRetVal.Add(mAdminNavLink);
        } else 
        {
            mNavLink = new MNavLink("generic_home", "home", "Home");
            mRetVal.Add(mNavLink);
        }
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

    [HttpPost("Log")]
    public bool Log(MLoggingProfile profile)
    {
        // MLoggingProfile mProfile = new MLoggingProfile(profile);
        LoggingUtility.Save(profile);
        return true;
    }

    [Authorize]
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

    [Authorize]
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
}

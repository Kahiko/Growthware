using Microsoft.AspNetCore.Mvc;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.WebSupport.Utilities;

namespace GrowthWare.Web.Angular.Controllers;

[ApiController]
[Route("[controller]")]
public class GrowthwareAPIController : ControllerBase
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
        MAccountProfile mRetVal = AccountUtility.GetAccount(account);
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

    [HttpPost("Log")]
    public bool Log(MLoggingProfile profile)
    {
        // MLoggingProfile mProfile = new MLoggingProfile(profile);
        LoggingUtility.Save(profile);
        return true;
    }

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

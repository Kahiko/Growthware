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
using System.Data;
using System.Linq;

namespace GrowthWare.Web.Support.BaseControllers;

[CLSCompliant(false)]
public abstract class AbstractNameValuePairController : ControllerBase
{
    private Logger m_Logger = Logger.Instance();

    [Authorize("search_name_value_pairs")]
    [HttpPost("SearchNameValuePairs")]
    public String SearchNameValuePairs(UISearchCriteria searchCriteria)
    {
        String mRetVal = string.Empty;
        string mColumns = "[NVPSeqId], [Schema_Name], [Static_Name], [Display], [Description], [StatusSeqId], [Added_By], [Added_Date], [Updated_By], [Updated_Date]";
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
                TableOrView = "[ZGWSystem].[Name_Value_Pairs]",
                WhereClause = mWhereClause
            };

            mRetVal = SearchUtility.GetSearchResults(mSearchCriteria);
        }
        return mRetVal;
    }
}
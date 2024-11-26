using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.Web.Support.Utilities;
using GrowthWare.Web.Support.Jwt;
using Microsoft.AspNetCore.StaticFiles;

namespace GrowthWare.Web.Support.BaseControllers;

[CLSCompliant(false)]
public abstract class AbstractFeedbackController : ControllerBase
{

    /// <summary>
    /// Performs a search for feecbacks based on the provided search criteria.
    /// </summary>
    /// <param name="searchCriteria">The criteria used to filter the search</param>
    /// <returns></returns>
    [Authorize("feedbacks")]
    [HttpPost("SearchFeedbacks")]
    public String SearchFeedbacks(UISearchCriteria searchCriteria)
    {
        String mRetVal = string.Empty;
        string mColumns = "[FeedbackId], [Assignee], [Details], [FoundInVersion], [Notes], [Severity], [Status], [TargetVersion], [Type], [VerifiedBy], [Start_Date], [End_Date]";
        if (searchCriteria.sortColumns.Length > 0)
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
                TableOrView = "[ZGWOptional].[vwCurrentFeedbacks]",
                WhereClause = mWhereClause
            };

            mRetVal = SearchUtility.GetSearchResults(mSearchCriteria);
        }
        return mRetVal;
    }
}
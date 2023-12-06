using Microsoft.AspNetCore.Mvc;
using System;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.Web.Support.Jwt;
using GrowthWare.Web.Support.Utilities;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GrowthWare.BusinessLogic;

namespace GrowthWare.Web.Support.BaseControllers;

[CLSCompliant(false)]
public abstract class AbstractNameValuePairController : ControllerBase
{
    private CacheController m_CacheController = CacheController.Instance();
    private Logger m_Logger = Logger.Instance();

    public List<MNameValuePair> GetMNameValuePairs()
    {
        List<MNameValuePair> mRetVal = this.m_CacheController.GetFromCache<List<MNameValuePair>>("NameValuePairs");
        if(mRetVal == null) 
        {
            BNameValuePairs mBNameValuePairs = new BNameValuePairs(SecurityEntityUtility.CurrentProfile());
        }
        return new List<MNameValuePair>();
    }

    [Authorize("search_name_value_pairs")]
    [HttpPost("SearchNameValuePairs")]
    public String SearchNameValuePairs(UISearchCriteria searchCriteria)
    {
        String mRetVal = string.Empty;
        string mColumns = "[NVPSeqId], [Schema_Name], [Static_Name], [Display], [Description], [StatusSeqId]";
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

    /// <summary>
    /// Method to get all children for a name value pair (Security is ignored).  Created to support add/edit name value pair details
    /// </summary>
    /// <param name="searchCriteria"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
     [HttpPost("SearcNVPDetails")]
    public String SearcNVPDetails(UISearchCriteria searchCriteria)
    {
        if (searchCriteria == null) throw new ArgumentNullException(nameof(searchCriteria), "searchCriteria cannot be a null reference (Nothing in VB) or empty!");
        int mNameValuePairSeqId = int.Parse(searchCriteria.searchText);
        String mRetVal = NameValuePairUtility.GetAllChildrenForParent(mNameValuePairSeqId);
        return mRetVal;
    }   
}
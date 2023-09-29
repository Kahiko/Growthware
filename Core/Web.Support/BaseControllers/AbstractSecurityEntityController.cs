using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using GrowthWare.Framework.Enumerations;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.Web.Support.Jwt;
using GrowthWare.Web.Support.Utilities;
using System.Data;

namespace GrowthWare.Web.Support.BaseControllers;

[CLSCompliant(false)]
public abstract class AbstractSecurityEntityController : ControllerBase
{
    [Authorize("Search_Security_Entities")]
    [HttpPost("Security_Entities")]
    public String Security_Entities(UISearchCriteria searchCriteria)
    {
        String mRetVal = string.Empty;
        string mColumns = "[SecurityEntitySeqId], [Name], [Description], [Skin]";
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
                TableOrView = "[ZGWSecurity].[Security_Entities]",
                WhereClause = mWhereClause
            };
            mRetVal = SearchUtility.GetSearchResults(mSearchCriteria);
        }
        return mRetVal;        
    }

    [HttpPost("GetValidSecurityEntities")]
    public ActionResult<List<UIValidSecurityEntity>> GetValidSecurityEntities()
    {
        MAccountProfile mRequestingProfile = (MAccountProfile)HttpContext.Items["AccountProfile"];
        if(mRequestingProfile.Account.ToLower() == "anonymous" || mRequestingProfile.Status == (int) SystemStatus.Active)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
        }
        MSecurityEntity mSecurityEntity = SecurityEntityUtility.CurrentProfile();
        List<UIValidSecurityEntity> mRetVal = new List<UIValidSecurityEntity>();
        
        DataView mDataView = SecurityEntityUtility.GetValidSecurityEntities(mRequestingProfile.Account, mSecurityEntity.Id, mRequestingProfile.IsSystemAdmin);
        foreach (DataRowView mDataRowView in mDataView.Table.Rows)
        {
            UIValidSecurityEntity mItem = new UIValidSecurityEntity(mDataRowView);
            mRetVal.Add(mItem);
        }
        return Ok(mRetVal);
    }
}
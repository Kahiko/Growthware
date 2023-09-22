using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.Web.Support.Jwt;
using GrowthWare.Web.Support.Utilities;

namespace GrowthWare.Web.Support.BaseControllers;

[CLSCompliant(false)]
public abstract class AbstractMessageController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("GetProfile")]
    public ActionResult<MMessage> GetProfile(int id)
    {
        MAccountProfile mRequestingProfile = (MAccountProfile)HttpContext.Items["AccountProfile"];
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.Actions_EditGroups);
        MSecurityEntity mSecurityEntity = SecurityEntityUtility.CurrentProfile();
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        if (mSecurityInfo.MayEdit || mSecurityInfo.MayView)
        {
            HttpContext.Session.SetString("EditId", id.ToString());
            MMessage mRetVal = new MMessage();
            mRetVal = MessageUtility.GetProfile(id);
            if(mRetVal == null)
            {
                mRetVal = new MMessage();
            }
            return Ok(mRetVal);
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");    }

    [Authorize("Search_Messages")]
    [HttpPost("SearchMessages")]
    public String SearchMessages(UISearchCriteria searchCriteria)
    {
        String mRetVal = string.Empty;
        string mColumns = "[MessageSeqId], [Name], [Title], [Description], [Added_By], [Added_Date], [Updated_By], [Updated_Date]";
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
                TableOrView = "[ZGWCoreWeb].[vwSearchMessages]",
                WhereClause = mWhereClause
            };
            mRetVal = SearchUtility.GetSearchResults(mSearchCriteria);
        }
        return mRetVal;        
    }
}
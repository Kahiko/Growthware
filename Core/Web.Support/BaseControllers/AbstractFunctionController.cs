using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.Web.Support.Jwt;
using GrowthWare.Web.Support.Utilities;
using System.Collections.Generic;

namespace GrowthWare.Web.Support.BaseControllers;

[CLSCompliant(false)]
public abstract class AbstractFunctionController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("GetFunction")]
    public ActionResult<MFunctionProfile> GetFunction(int functionSeqId)
    {
        MSecurityInfo mSecurityInfo = this.GetSecurityInfo("FunctionSecurity");
        if(mSecurityInfo != null && mSecurityInfo.MayView)
        {
            MFunctionProfile mRetVal = new MFunctionProfile();
            mRetVal = FunctionUtility.GetProfile(functionSeqId);
            if(mRetVal == null)
            {
                mRetVal = new MFunctionProfile();
            }
            HttpContext.Session.SetInt32("EditId", mRetVal.FunctionTypeSeqId);
            return Ok(mRetVal);
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    [AllowAnonymous]
    [HttpGet("GetFunctionTypes")]
    public ActionResult<List<UIKeyValuePair>> GetFunctionTypes()
    {
        MSecurityInfo mSecurityInfo = this.GetSecurityInfo("FunctionSecurity");
        if (mSecurityInfo != null && mSecurityInfo.MayView)
        {
            List<UIKeyValuePair> mRetVal = FunctionUtility.GetFunctionTypes();
            return Ok(mRetVal);
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    [AllowAnonymous]
    [HttpGet("GetNavigationTypes")]
    public ActionResult<List<UIKeyValuePair>> GetNavigationTypes()
    {
        MSecurityInfo mSecurityInfo = this.GetSecurityInfo("FunctionSecurity");
        if (mSecurityInfo != null && mSecurityInfo.MayView)
        {
            List<UIKeyValuePair> mRetVal = NameValuePairUtility.GetNavigationTypes();
            return Ok(mRetVal);
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    private MSecurityInfo GetSecurityInfo(string action)
    {
        MAccountProfile mRequestingProfile = (MAccountProfile)HttpContext.Items["AccountProfile"];
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(action);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        return mSecurityInfo;
    }

    [Authorize("Functions")]
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

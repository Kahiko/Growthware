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
public abstract class AbstractRoleController : ControllerBase
{
    [HttpGet("GetRoleForEdit")]
    public ActionResult<UIRole> GetRoleForEdit(int roleSeqId)
    {
        MAccountProfile mRequestingProfile = (MAccountProfile)HttpContext.Items["AccountProfile"];
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.Actions_EditRoles);
        MSecurityEntity mSecurityEntity = SecurityEntityUtility.CurrentProfile();
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        if (mSecurityInfo.MayEdit)
        {
            UIRole mRetVal = RoleUtility.GetUIProfile(roleSeqId, SecurityEntityUtility.CurrentProfile());
            HttpContext.Session.SetString("EditId", roleSeqId.ToString());
            return Ok(mRetVal);
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    [HttpGet("GetRoles")]
    public ActionResult<ArrayList> GetRoles()
    {
        ArrayList mRetVal = RoleUtility.GetRolesArrayListBySecurityEntity(SecurityEntityUtility.CurrentProfile().Id);
        return Ok(mRetVal);
    }

    [Authorize("Search_Roles")]
    [HttpPost("SearchRoles")]
    public String SearchRoles(UISearchCriteria searchCriteria)
    {
        String mRetVal = string.Empty;
        string mColumns = "[RoleSeqId], [Name], [Description], [Is_System], [Is_System_Only], [Added_By], [Added_Date], [Updated_By], [Updated_Date]";
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
                TableOrView = "[ZGWSecurity].[vwSearchRoles]",
                WhereClause = mWhereClause
            };
            mRetVal = SearchUtility.GetSearchResults(mSearchCriteria);
        }
        return mRetVal;        
    }
}

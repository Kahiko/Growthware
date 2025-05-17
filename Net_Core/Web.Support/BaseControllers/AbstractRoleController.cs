using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.Web.Support.Jwt;
using GrowthWare.Web.Support.Utilities;
using System.Threading.Tasks;

namespace GrowthWare.Web.Support.BaseControllers;

[CLSCompliant(false)]
public abstract class AbstractRoleController : ControllerBase
{

    [HttpDelete("DeleteRole")]
    public async Task<ActionResult> DeleteRole(int roleSeqId)
    {
        MAccountProfile mRequestingProfile = await AccountUtility.CurrentProfile();
        MFunctionProfile mFunctionProfile = await FunctionUtility.GetProfile(ConfigSettings.Actions_EditRoles);
        MSecurityEntity mSecurityEntity = await SecurityEntityUtility.CurrentProfile();
        MSecurityInfo mSecurityInfo = new(mFunctionProfile, mRequestingProfile);
        if (HttpContext.Session.GetString("EditId") != null)
        {
            if (int.Parse(HttpContext.Session.GetString("EditId")) == roleSeqId)
            {
                UIRole mProfile = await RoleUtility.GetUIProfile(roleSeqId, mSecurityEntity.Id);
                if(mSecurityInfo.MayDelete && !mProfile.IsSystemOnly)
                {
                    await RoleUtility.DeleteRole(roleSeqId, mSecurityEntity.Id);
                    return Ok(true);
                }
                return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
            }
        }
        return StatusCode(StatusCodes.Status304NotModified, "Unable to delete");
    }

    [HttpGet("GetRoleForEdit")]
    public async Task<ActionResult<UIRole>> GetRoleForEdit(int roleSeqId)
    {
        MAccountProfile mRequestingProfile = await AccountUtility.CurrentProfile();
        MFunctionProfile mFunctionProfile = await FunctionUtility.GetProfile(ConfigSettings.Actions_EditRoles);
        MSecurityEntity mSecurityEntity = await SecurityEntityUtility.CurrentProfile();
        MSecurityInfo mSecurityInfo = new(mFunctionProfile, mRequestingProfile);
        if (mSecurityInfo.MayEdit)
        {
            UIRole mRetVal = await RoleUtility.GetUIProfile(roleSeqId, mSecurityEntity.Id);
            HttpContext.Session.SetString("EditId", roleSeqId.ToString());
            return Ok(mRetVal);
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    [HttpGet("GetRoles")]
    public async Task<ActionResult<ArrayList>> GetRoles()
    {
        MSecurityEntity mSecurityEntity = await SecurityEntityUtility.CurrentProfile();
        ArrayList mRetVal = await RoleUtility.GetRolesArrayListBySecurityEntity(mSecurityEntity.Id);
        return Ok(mRetVal);
    }

    [HttpPost("SaveRole")]
    public async Task<ActionResult<UIRole>> SaveRole(UIRole roleProfile)
    {
        MAccountProfile mRequestingProfile = await AccountUtility.CurrentProfile();
        MFunctionProfile mFunctionProfile = await FunctionUtility.GetProfile(ConfigSettings.Actions_EditRoles);
        MSecurityEntity mSecurityEntity = await SecurityEntityUtility.CurrentProfile();
        MSecurityInfo mSecurityInfo = new(mFunctionProfile, mRequestingProfile);
        MRole mProfileToSave = new(roleProfile);
        if (HttpContext.Session.GetString("EditId") != null)
        {
            if (int.Parse(HttpContext.Session.GetString("EditId")) == roleProfile.Id)
            {
                if (roleProfile.Id > -1) 
                {
                    if (mSecurityInfo.MayEdit)
                    {
                        mProfileToSave.UpdatedBy = mRequestingProfile.Id;
                        mProfileToSave.UpdatedDate = DateTime.Now;
                    } 
                    else 
                    {
                        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
                    }
                } 
                else 
                {
                    if (mSecurityInfo.MayAdd)
                    {
                        mProfileToSave.AddedBy = mRequestingProfile.Id;
                        mProfileToSave.AddedDate = DateTime.Now;
                    }
                    else 
                    {
                        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
                    }
                }
            }
            mProfileToSave.SecurityEntityID = mSecurityEntity.Id;
            UIRole mRetVal = await RoleUtility.Save(mProfileToSave, roleProfile.AccountsInRole);
            return Ok(mRetVal);
        }
        return StatusCode(StatusCodes.Status304NotModified, "Unable to save");
    }

    [Authorize("Search_Roles")]
    [HttpPost("SearchRoles")]
    public async Task<String> SearchRoles(UISearchCriteria searchCriteria)
    {
        String mRetVal = string.Empty;
        string mColumns = "[RoleSeqId], [Name], [Description], [Is_System], [Is_System_Only], [Added_By], [Added_Date], [Updated_By], [Updated_Date]";
        if(searchCriteria.sortColumns.Length > 0)
        {
            MSecurityEntity mSecurityEntity = await SecurityEntityUtility.CurrentProfile();
            Tuple<string, string> mOrderByAndWhere = SearchUtility.GetOrderByAndWhere(mColumns, searchCriteria.searchColumns, searchCriteria.sortColumns, searchCriteria.searchText);
            string mOrderByClause = mOrderByAndWhere.Item1;
            string mWhereClause = mOrderByAndWhere.Item2 + " AND SecurityEntitySeqId = " + mSecurityEntity.Id.ToString();
            MSearchCriteria mSearchCriteria = new()
            {
                Columns = mColumns,
                OrderByClause = mOrderByClause,
                PageSize = searchCriteria.pageSize,
                SelectedPage = searchCriteria.selectedPage,
                TableOrView = "[ZGWSecurity].[vwSearchRoles]",
                WhereClause = mWhereClause
            };
            mRetVal = await SearchUtility.GetSearchResults(mSearchCriteria);
        }
        return mRetVal;        
    }
}

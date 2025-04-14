using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.Web.Support.Jwt;
using GrowthWare.Web.Support.Utilities;
using System.Linq;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace GrowthWare.Web.Support.BaseControllers;

[CLSCompliant(false)]
public abstract class AbstractGroupController : ControllerBase
{

    [AllowAnonymous]
    [HttpPost("DeleteGroup")]
    public async Task<ActionResult<bool>> DeleteGroup(int groupSeqId)
    {
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.Actions_EditGroups);
        MSecurityEntity mSecurityEntity = SecurityEntityUtility.CurrentProfile;
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        if (mSecurityInfo.MayDelete)
        {
            if (HttpContext.Session.GetString("EditId") != null)
            {
                if (int.Parse(HttpContext.Session.GetString("EditId")) == groupSeqId)
                {
                    MGroupProfile mProfile = await GroupUtility.GetGroupProfile(groupSeqId);
                    await GroupUtility.Delete(mProfile);
                    return Ok(true);
                }
            }
            return StatusCode(StatusCodes.Status204NoContent, "Could not find the group to delete");
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    [AllowAnonymous]
    [HttpGet("GetGroupForEdit")]
    public async Task<ActionResult<UIGroupProfile>> GetGroupForEdit(int groupSeqId)
    {
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.Actions_EditGroups);
        MSecurityEntity mSecurityEntity = SecurityEntityUtility.CurrentProfile;
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        if (mSecurityInfo.MayEdit || mSecurityInfo.MayView)
        {
            HttpContext.Session.SetString("EditId", groupSeqId.ToString());
            UIGroupProfile mRetVal = await GroupUtility.GetUIGroupProfile(groupSeqId, mSecurityEntity.Id);
            return Ok(mRetVal);
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    [HttpGet("GetGroups")]
    public async Task<ActionResult<ArrayList>> GetGroups()
    {
        ArrayList mRetVal;
        mRetVal = await GroupUtility.GetGroupsArrayListBySecurityEntity(SecurityEntityUtility.CurrentProfile.Id);
        return Ok(mRetVal);
    }

    [AllowAnonymous]
    [HttpPost("SaveGroup")]
    public async Task<ActionResult<UIGroupProfile>> SaveGroup(UIGroupProfile groupProfile)
    {
        if (HttpContext.Session.GetString("EditId") != null)
        {
            MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
            MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.Actions_EditGroups);
            MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
            int mSecurityEntityId = SecurityEntityUtility.CurrentProfile.Id;

            // Get the group profile populated with the parameter values
            MGroupProfile mProfileToSave = new MGroupProfile(groupProfile);
            mProfileToSave.SecurityEntityID = mSecurityEntityId;
            // Get a commaseparated string of roles
            string mRoles = string.Join(",", groupProfile.RolesInGroup);
            // Get a MGroupRoles object
            MGroupRoles mGroupRoles = new MGroupRoles(mRoles, mSecurityEntityId);
            if (int.Parse(HttpContext.Session.GetString("EditId")) == groupProfile.Id)
            {
                // Check if this is an add or an update
                if (groupProfile.Id > -1) 
                {
                    // Check if the requesting account has the correct permissions for an update/edit
                    if (mSecurityInfo.MayEdit)
                    {
                        // update the updated by and updated date for the profiles
                        mProfileToSave.UpdatedBy = mRequestingProfile.Id;
                        mProfileToSave.UpdatedDate = DateTime.Now;
                        mGroupRoles.UpdatedBy = mProfileToSave.UpdatedBy;
                        mGroupRoles.UpdatedDate = mProfileToSave.UpdatedDate;
                    } 
                    else 
                    {
                        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
                    }
                } 
                else 
                {
                    // Check if the requesting account has the correct permissions for an add
                    if (mSecurityInfo.MayAdd)
                    {
                        // update the added by and added date for the profiles
                        mProfileToSave.AddedBy = mRequestingProfile.Id;
                        mProfileToSave.AddedDate = DateTime.Now;
                        mGroupRoles.AddedBy = mProfileToSave.AddedBy;
                        mGroupRoles.AddedDate = mProfileToSave.AddedDate;
                    }
                    else 
                    {
                        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
                    }
                }
            }
            // Save the profiles
            UIGroupProfile mRetVal = await GroupUtility.Save(mProfileToSave, mGroupRoles);
            return Ok(mRetVal);
        }
        return StatusCode(StatusCodes.Status304NotModified, "Unable to save");
    }

    [Authorize("Manage_Groups")]
    [HttpPost("SearchGroups")]
    public String SearchGroups(UISearchCriteria searchCriteria)
    {
        String mRetVal = string.Empty;
        string mColumns = "[GroupSeqId], [Name], [Description], [Added_By], [Added_Date], [Updated_By], [Updated_Date]";
        if (searchCriteria.sortColumns.Length > 0)
        {
            Tuple<string, string> mOrderByAndWhere = SearchUtility.GetOrderByAndWhere(mColumns, searchCriteria.searchColumns, searchCriteria.sortColumns, searchCriteria.searchText);
            string mOrderByClause = mOrderByAndWhere.Item1;
            string mWhereClause = mOrderByAndWhere.Item2 + " AND SecurityEntitySeqId = " + SecurityEntityUtility.CurrentProfile.Id.ToString();
            MSearchCriteria mSearchCriteria = new MSearchCriteria
            {
                Columns = mColumns,
                OrderByClause = mOrderByClause,
                PageSize = searchCriteria.pageSize,
                SelectedPage = searchCriteria.selectedPage,
                TableOrView = "[ZGWSecurity].[vwSearchGroups]",
                WhereClause = mWhereClause
            };
            mRetVal = SearchUtility.GetSearchResults(mSearchCriteria);
        }
        return mRetVal;
    }
}
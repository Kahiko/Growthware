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

namespace GrowthWare.Web.Support.BaseControllers;

[CLSCompliant(false)]
public abstract class AbstractGroupController : ControllerBase
{

    [AllowAnonymous]
    [HttpPost("DeleteGroup")]
    public ActionResult<bool> DeleteGroup(int groupSeqId)
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
                    MGroupProfile mProfile = GroupUtility.GetGroupProfile(groupSeqId);
                    GroupUtility.Delete(mProfile);
                    return Ok(true);
                }
            }
            return StatusCode(StatusCodes.Status204NoContent, "Could not find the group to delete");
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    [AllowAnonymous]
    [HttpGet("GetGroupForEdit")]
    public ActionResult<UIGroupProfile> GetGroupForEdit(int groupSeqId)
    {
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.Actions_EditGroups);
        MSecurityEntity mSecurityEntity = SecurityEntityUtility.CurrentProfile;
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        if (mSecurityInfo.MayEdit || mSecurityInfo.MayView)
        {
            HttpContext.Session.SetString("EditId", groupSeqId.ToString());
            UIGroupProfile mRetVal = GroupUtility.GetUIGroupProfile(groupSeqId, mSecurityEntity.Id);
            return Ok(mRetVal);
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    [HttpGet("GetGroups")]
    public ActionResult<ArrayList> GetGroups()
    {
        ArrayList mRetVal;
        mRetVal = GroupUtility.GetGroupsArrayListBySecurityEntity(SecurityEntityUtility.CurrentProfile.Id);
        return Ok(mRetVal);
    }

    [AllowAnonymous]
    [HttpPost("SaveGroup")]
    public ActionResult<UIGroupProfile> SaveGroup(UIGroupProfile groupProfile)
    {
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.Actions_EditGroups);
        MSecurityEntity mSecurityEntity = SecurityEntityUtility.CurrentProfile;
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        MGroupProfile mProfileToSave = new MGroupProfile();
        MGroupRoles mGroupRoles = new MGroupRoles();
        mProfileToSave.SecurityEntityID = mSecurityEntity.Id;
        mGroupRoles.SecurityEntityID = mProfileToSave.SecurityEntityID;
        mGroupRoles.Roles = string.Join(",", groupProfile.RolesInGroup);
        if (HttpContext.Session.GetString("EditId") != null)
        {
            if (int.Parse(HttpContext.Session.GetString("EditId")) == groupProfile.Id)
            {
                if (groupProfile.Id > -1) 
                {
                    if (mSecurityInfo.MayEdit)
                    {
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
                    if (mSecurityInfo.MayAdd)
                    {
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
            mProfileToSave.Name = groupProfile.Name;
            mProfileToSave.Description = groupProfile.Description;
            mProfileToSave.Id = groupProfile.Id;
            mProfileToSave.SecurityEntityID = SecurityEntityUtility.CurrentProfile.Id;
            UIGroupProfile mRetVal = GroupUtility.Save(mProfileToSave, mGroupRoles);
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
            string mWhereClause = mOrderByAndWhere.Item2;
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
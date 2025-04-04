using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.Web.Support.Jwt;
using GrowthWare.Web.Support.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace GrowthWare.Web.Support.BaseControllers;

[CLSCompliant(false)]
public abstract class AbstractFunctionController : ControllerBase
{
    private Logger m_Logger = Logger.Instance();

    [AllowAnonymous]
    [HttpPost("CopyFunctionSecurity")]
    public ActionResult<bool> CopyFunctionSecurity(int source, int target)
    {
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        // Special case where you must be an account with IsSystemAdmin = true
        // Copying the function security is risky b/c the process will delete all existing security for the target
        if(mRequestingProfile != null && mRequestingProfile.IsSystemAdmin)
        {
            if(source != target && target != 1) {
                FunctionUtility.CopyFunctionSecurity(source, target, mRequestingProfile.Id);
                return Ok(true);
            }
            return StatusCode(StatusCodes.Status400BadRequest, "The function cannot be copied to itself or the target can not be 'System'");
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    [AllowAnonymous]
    [HttpDelete("DeleteFunction")]
    public ActionResult<bool> Delete(int functionSeqId)
    {
        MSecurityInfo mSecurityInfo = this.getSecurityInfo("FunctionSecurity");
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        if(mSecurityInfo.MayDelete)
        {
            var mEditId =  HttpContext.Session.GetInt32("EditId");
            if(mEditId != null && mEditId == functionSeqId)
            {
                FunctionUtility.Delete(functionSeqId);
                return Ok(true);
            }
            this.m_Logger.Error(String.Format("'{0}' attempted to delete a function without having the editid set properly, possible attempt to breach security.", mRequestingProfile.Account));
            return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    [AllowAnonymous]
    [HttpGet("GetAvalibleParents")]
    public ActionResult GetAvalibleParents()
    {
        MSecurityInfo mSecurityInfo = this.getSecurityInfo("FunctionSecurity");
        if(mSecurityInfo.MayView)
        {
            return Ok(FunctionUtility.GetAvalibleParents());
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    [AllowAnonymous]
    [HttpGet("GetFunction")]
    public ActionResult<UIFunctionProfile> GetFunctionForEdit(int functionSeqId)
    {
        MSecurityInfo mSecurityInfo = this.getSecurityInfo("FunctionSecurity");
        if(mSecurityInfo != null && mSecurityInfo.MayView)
        {
            MFunctionProfile mFunctionProfile = new MFunctionProfile();
            mFunctionProfile = FunctionUtility.GetProfile(functionSeqId);
            if(mFunctionProfile == null)
            {
                mFunctionProfile = new MFunctionProfile();
            }
            HttpContext.Session.SetInt32("EditId", mFunctionProfile.Id);
            UIFunctionProfile mRetVal = new UIFunctionProfile(mFunctionProfile);
            mRetVal.CanSaveGroups = this.getSecurityInfo("View_Function_Group_Tab").MayView;
            mRetVal.CanSaveRoles = this.getSecurityInfo("View_Function_Role_Tab").MayView;
            mRetVal.DirectoryData = DirectoryUtility.GetDirectoryProfile(mFunctionProfile.Id);
            mRetVal.DirectoryData.ImpersonatePassword = string.Empty; // We don't want the password to show
            mRetVal.FunctionMenuOrders = FunctionUtility.GetFunctionOrder(mFunctionProfile.Id);
            return Ok(mRetVal);
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");        
    }

    [AllowAnonymous]
    [HttpGet("GetFunctionTypes")]
    public ActionResult<List<UIKeyValuePair>> GetFunctionTypes()
    {
        MSecurityInfo mSecurityInfo = this.getSecurityInfo("FunctionSecurity");
        if (mSecurityInfo != null && mSecurityInfo.MayView)
        {
            List<UIKeyValuePair> mRetVal = FunctionUtility.GetFunctionTypes();
            return Ok(mRetVal);
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    [AllowAnonymous]
    [HttpGet("GetLinkBehaviors")]
    public ActionResult<List<UIKeyValuePair>> GetLinkBehaviors()
    {
        MSecurityInfo mSecurityInfo = this.getSecurityInfo("FunctionSecurity");
        if (mSecurityInfo != null && mSecurityInfo.MayView)
        {
            List<UIKeyValuePair> mRetVal = NameValuePairUtility.LinkBehaviors;
            return Ok(mRetVal);
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    [AllowAnonymous]
    [HttpGet("GetNavigationTypes")]
    public ActionResult<List<UIKeyValuePair>> GetNavigationTypes()
    {
        MSecurityInfo mSecurityInfo = this.getSecurityInfo("FunctionSecurity");
        if (mSecurityInfo != null && mSecurityInfo.MayView)
        {
            List<UIKeyValuePair> mRetVal = NameValuePairUtility.NavigationTypes;
            return Ok(mRetVal);
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    [AllowAnonymous]
    [HttpGet("GetFunctionOrder")]
    public ActionResult<List<UIFunctionMenuOrder>> GetFunctionOrder(int functionSeqId)
    {
        MSecurityInfo mSecurityInfo = this.getSecurityInfo("FunctionSecurity");
        if(mSecurityInfo.MayView)
        {
            return Ok(FunctionUtility.GetFunctionOrder(functionSeqId));
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    private MSecurityInfo getSecurityInfo(string action)
    {
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(action);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        return mSecurityInfo;
    }

    [AllowAnonymous]
    [HttpPost("Save")]
    public ActionResult<bool> Save(UIFunctionProfile functionProfile)
    {
        MSecurityInfo mSecurityInfo = this.getSecurityInfo("FunctionSecurity");
        MSecurityInfo mViewRoleTabSecurityInfo = this.getSecurityInfo("View_Function_Role_Tab");
        MSecurityInfo mViewGroupTabSecurityInfo = this.getSecurityInfo("View_Function_Group_Tab");
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        var mEditId =  HttpContext.Session.GetInt32("EditId");
        string mReturnMsg = string.Empty; // to be used for other security checks so we can pass it back to the client
        if(mEditId != null && (mSecurityInfo.MayAdd || mSecurityInfo.MayEdit))
        {
            // we don't want to save the of the properties from the UI so we get the profile from the DB
            MFunctionProfile mExistingProfile = FunctionUtility.GetProfile(functionProfile.Id);
            if(mExistingProfile == null)
            {
                mExistingProfile = new MFunctionProfile();
            }
            MFunctionProfile mProfileToSave = new MFunctionProfile(functionProfile);
            if (mEditId == functionProfile.Id)
            {
                if (functionProfile.Id > -1) 
                {
                    if (mSecurityInfo.MayEdit)
                    {
                        mProfileToSave.AddedBy = mExistingProfile.AddedBy;
                        mProfileToSave.AddedDate = mExistingProfile.AddedDate;
                        mProfileToSave.UpdatedBy = mRequestingProfile.Id;
                        mProfileToSave.UpdatedDate = DateTime.Now;
                    }else
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
            bool mSaveGroups = this.getSecurityInfo("View_Function_Group_Tab").MayView;
            bool mSaveRoles = this.getSecurityInfo("View_Function_Role_Tab").MayView;

            string ViewRoles = String.Join(",", mProfileToSave.AssignedViewRoles);
            string AddRoles = String.Join(",", mProfileToSave.AssignedAddRoles);
            string EditRoles = String.Join(",", mProfileToSave.AssignedEditRoles);
            string DeleteRoles = String.Join(",", mProfileToSave.AssignedDeleteRoles);

            string ViewGroups =  String.Join(",", mProfileToSave.ViewGroups);
            string AddGroups = String.Join(",", mProfileToSave.AddGroups);
            string EditGroups = String.Join(",", mProfileToSave.EditGroups);
            string DeleteGroups = String.Join(",", mProfileToSave.DeleteGroups);
            
            if(mSaveRoles)
            {
                if(String.Join(",", mExistingProfile.AssignedViewRoles) != ViewRoles) {
                    mSaveRoles = true;
                }
                if(String.Join(",", mExistingProfile.AssignedAddRoles) != AddRoles) {
                    mSaveRoles = true;
                }
                if(String.Join(",", mExistingProfile.AssignedEditRoles) != EditRoles) {
                    mSaveRoles = true;
                }
                if(String.Join(",", mExistingProfile.AssignedDeleteRoles) != DeleteRoles) {
                    mSaveRoles = true;
                }
            }
            if(mSaveGroups)
            {
                if(String.Join(",", mExistingProfile.ViewGroups) != ViewGroups) {
                    mSaveGroups = true;
                }
                if(String.Join(",", mExistingProfile.AddGroups) != AddGroups) {
                    mSaveGroups = true;
                }
                if(String.Join(",", mExistingProfile.EditGroups) != EditGroups) {
                    mSaveGroups = true;
                }
                if(String.Join(",", mExistingProfile.DeleteGroups) != DeleteGroups) {
                    mSaveGroups = true;
                }
            }

            try
            {
                int mFunctionSeqId = FunctionUtility.Save(mProfileToSave, mSaveGroups, mSaveRoles);
                mProfileToSave.Id = mFunctionSeqId;
                if(!string.IsNullOrWhiteSpace(functionProfile.DirectoryData.Directory))
                {
                    MDirectoryProfile mDirectoryProfile = DirectoryUtility.GetDirectoryProfile(mProfileToSave.Id);
                    if(mDirectoryProfile == null)
                    {
                        mDirectoryProfile = new MDirectoryProfile();
                        mDirectoryProfile.Id = mProfileToSave.Id;
                    }
                    mDirectoryProfile.Directory = functionProfile.DirectoryData.Directory;
                    mDirectoryProfile.Impersonate = functionProfile.DirectoryData.Impersonate;
                    if(functionProfile.DirectoryData.Impersonate)
                    {
                        mDirectoryProfile.ImpersonateAccount = functionProfile.DirectoryData.ImpersonateAccount;
                        if(!string.IsNullOrWhiteSpace(functionProfile.DirectoryData.ImpersonatePassword))
                        {
                            mDirectoryProfile.ImpersonatePassword = functionProfile.DirectoryData.ImpersonatePassword;
                        }
                    }
                    mDirectoryProfile.Directory = functionProfile.DirectoryData.Directory;
                    mDirectoryProfile.UpdatedBy = mRequestingProfile.Id;
                    DirectoryUtility.Save(mDirectoryProfile);
                }
                string mCommaSeporatedIds = String.Join(",", functionProfile.FunctionMenuOrders.Select(item => item.Function_Seq_Id.ToString()));
                FunctionUtility.UpdateMenuOrder(mCommaSeporatedIds, mProfileToSave);
            }
            catch (System.Exception ex)
            {
                Logger.Instance().Error(ex);
                return Ok(false);
            }
            return Ok(true);
        }
        if(mEditId!=null) 
        {
            this.m_Logger.Error(String.Format("'{0}' does not have permissions to 'Save' a function. FunctionSeqId: {1}", mRequestingProfile.Account, functionProfile.Id.ToString()));
            return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
        }
        this.m_Logger.Error(String.Format("'{0}' attempted to save a function without having the editid set first possible attempt to breach security.", mRequestingProfile.Account));
        return StatusCode(StatusCodes.Status401Unauthorized, "Unable to save the function.  Please contact your system administrator.");
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

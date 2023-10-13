using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.Web.Support.Jwt;
using GrowthWare.Web.Support.Utilities;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace GrowthWare.Web.Support.BaseControllers;

[CLSCompliant(false)]
public abstract class AbstractFunctionController : ControllerBase
{
    private Logger m_Logger = Logger.Instance();

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
            mRetVal.DirectoryData = DirectoryUtility.GetDirectoryProfile(mFunctionProfile.Id);
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
            List<UIKeyValuePair> mRetVal = NameValuePairUtility.GetLinkBehaviors();
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
            List<UIKeyValuePair> mRetVal = NameValuePairUtility.GetNavigationTypes();
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
        MAccountProfile mRequestingProfile = (MAccountProfile)HttpContext.Items["AccountProfile"];
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
        MAccountProfile mRequestingProfile = (MAccountProfile)HttpContext.Items["AccountProfile"];
        var mEditId =  HttpContext.Session.GetInt32("EditId");
        string mReturnMsg = string.Empty; // to be used for other security checks so we can pass it back to the client
        if(mEditId != null && (mSecurityInfo.MayAdd || mSecurityInfo.MayEdit))
        {
            bool mRetVal = false;
            // we don't want to save the of the properties from the UI so we get the profile from the DB
            MFunctionProfile mExistingFunction = FunctionUtility.GetProfile(functionProfile.Id);
            if(mExistingFunction == null)
            {
                mExistingFunction = new MFunctionProfile();
            }
            MFunctionProfile mProfileToSave = new MFunctionProfile(functionProfile);
            if (mEditId == functionProfile.Id)
            {
                if (functionProfile.Id > -1) 
                {
                    if (mSecurityInfo.MayEdit)
                    {
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
                bool saveGroups = false;
                // bool saveRoles = false;


            }
            mRetVal = true;
            return Ok(mRetVal);
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

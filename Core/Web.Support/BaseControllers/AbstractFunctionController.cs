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
    public ActionResult<MFunctionProfile> GetFunction(int functionSeqId)
    {
        MSecurityInfo mSecurityInfo = this.getSecurityInfo("FunctionSecurity");
        if(mSecurityInfo != null && mSecurityInfo.MayView)
        {
            MFunctionProfile mRetVal = new MFunctionProfile();
            mRetVal = FunctionUtility.GetProfile(functionSeqId);
            if(mRetVal == null)
            {
                mRetVal = new MFunctionProfile();
            }
            mRetVal.DirectoryData = DirectoryUtility.GetDirectoryProfile(mRetVal.Id);
            mRetVal.FunctionMenuOrders = FunctionUtility.GetFunctionOrder(mRetVal.Id);
            HttpContext.Session.SetInt32("EditId", mRetVal.FunctionTypeSeqId);
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
    public ActionResult<bool> Save(MFunctionProfile functionProfile)
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

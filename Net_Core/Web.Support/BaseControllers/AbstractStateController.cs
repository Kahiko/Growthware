using Microsoft.AspNetCore.Mvc;
using System;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.Web.Support.Jwt;
using GrowthWare.Web.Support.Utilities;
using GrowthWare.Framework;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace GrowthWare.Web.Support.BaseControllers;

[CLSCompliant(false)]
public abstract class AbstractStateController : ControllerBase
{

    [AllowAnonymous]
    [HttpGet("GetProfile")]
    public ActionResult<MState> GetProfile(String state)
    {
        if (string.IsNullOrEmpty(state))
        {
            return StatusCode(StatusCodes.Status400BadRequest, "'state' can not be blank"); 
        }
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile("EditState");
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        if(!mSecurityInfo.MayView)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
        }
        MState mState = StateUtility.GetState(state);
        HttpContext.Session.SetString("EditId", mState.State);
        return Ok(mState);}

    [AllowAnonymous]
    [HttpPost("Save")]
    public ActionResult<bool> Save(MState state)
    {
        if (state == null) throw new ArgumentNullException(nameof(state), " can not be null!");
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.Actions_EditAccount);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);        
        if (HttpContext.Session.GetString("EditId") != null)
        {
            string mEditId = HttpContext.Session.GetString("EditId");
            MState mProfileToSave = StateUtility.GetState(state.State);
            if(mEditId == mProfileToSave.State)
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
                mProfileToSave.Description = state.Description;
                mProfileToSave.StatusId = state.StatusId;
                StateUtility.Save(mProfileToSave);
                return Ok(true);
            }
        }
        return StatusCode(StatusCodes.Status304NotModified, "Unable to save");
    }

    [Authorize("Search_States")]
    [HttpPost("Search_States")]
    public async Task<String> Search_States(UISearchCriteria searchCriteria)
    {
        String mRetVal = string.Empty;
        string mColumns = "[State], [Description], [Status], [Added_By], [Added_Date], [Updated_By], [Updated_Date]";
        if(searchCriteria.sortColumns.Length > 0)
        {
            Tuple<string, string> mOrderByAndWhere = SearchUtility.GetOrderByAndWhere(mColumns, searchCriteria.searchColumns, searchCriteria.sortColumns, searchCriteria.searchText);
            string mOrderByClause = mOrderByAndWhere.Item1;
            string mWhereClause = mOrderByAndWhere.Item2;
            MSearchCriteria mSearchCriteria = new()
            {
                Columns = mColumns,
                OrderByClause = mOrderByClause,
                PageSize = searchCriteria.pageSize,
                SelectedPage = searchCriteria.selectedPage,
                TableOrView = "[ZGWOptional].[vwSearchStates]",
                WhereClause = mWhereClause
            };
            mRetVal = await SearchUtility.GetSearchResults(mSearchCriteria);
        }
        return mRetVal;        
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.Web.Support.Utilities;
using GrowthWare.Web.Support.Jwt;

namespace GrowthWare.Web.Support.BaseControllers;

[CLSCompliant(false)]
public abstract class AbstractFeedbackController : ControllerBase
{
    private Logger m_Logger = Logger.Instance();

    public UIFeedbackResult GetFeedbackForEdit(int feedbackId)
    {
        UIFeedbackResult mRetVal = new();
        HttpContext.Session.Remove("EditId");

        HttpContext.Session.SetInt32("EditId", mRetVal.FeedbackId);
        return mRetVal;
    }

    public ActionResult<UIFeedbackResult> SaveFeedback(UIFeedbackResult feedbackResult)
    {
        UIFeedbackResult mRetVal = new();
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.Actions_EditFeedback);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        var mEditId = HttpContext.Session.GetInt32("EditId");
        if (mEditId != null)
        {
            // we don't want to save the properties from the UI so we get the profile from the DB

            if(mRetVal != feedbackResult && mSecurityInfo.MayAdd || mSecurityInfo.MayEdit)
            {
                return Ok(mRetVal);
            }
        }
        this.m_Logger.Error(mRequestingProfile.Account + " does not have permissions to 'Save Feedback'");
        return Ok("The account does not have permissions to 'Save Feedback'");
    }

    /// <summary>
    /// Performs a search for feecbacks based on the provided search criteria.
    /// </summary>
    /// <param name="searchCriteria">The criteria used to filter the search</param>
    /// <returns></returns>
    [Authorize("feedbacks")]
    [HttpPost("SearchFeedbacks")]
    public ActionResult<String> SearchFeedbacks(UISearchCriteria searchCriteria)
    {
        String mRetVal = string.Empty;
        string mColumns = "[FeedbackId], [Assignee], [Details], [FoundInVersion], [Notes], [Severity], [Status], [TargetVersion], [Type], [VerifiedBy], [Start_Date], [End_Date]";
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
                TableOrView = "[ZGWOptional].[vwCurrentFeedbacks]",
                WhereClause = mWhereClause
            };

            mRetVal = SearchUtility.GetSearchResults(mSearchCriteria);
        }
        return Ok(mRetVal);
    }

    public ActionResult<UIFeedbackResult> SubmitFeedback(UIFeedbackResult feedbackResult)
    {
        UIFeedbackResult mRetVal = new();

        return Ok(mRetVal);
    }

}
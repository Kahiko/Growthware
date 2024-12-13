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

    [Authorize("feedbacks")]
    [HttpGet("GetFeedbackForEdit")]
    public UIFeedback GetFeedbackForEdit(int feedbackId)
    {
        UIFeedback mRetVal = new();
        HttpContext.Session.Remove("EditId");

        HttpContext.Session.SetInt32("EditId", mRetVal.FeedbackId);
        mRetVal = FeedbackUtility.GetFeedback(feedbackId);
        return mRetVal;
    }

    [Authorize("feedbacks")]
    [HttpPost("SaveFeedback")]
    public ActionResult<UIFeedback> SaveFeedback(UIFeedback feedback)
    {
        UIFeedback mRetVal = new();
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.Actions_EditFeedback);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        var mEditId = HttpContext.Session.GetInt32("EditId");
        if (mEditId != null)
        {
            // what is saved in the DB isn't the same that is passed in from th UI
            MFeedback mFeedbackToSave = new(feedback);
            mFeedbackToSave.UpdatedById = mRequestingProfile.Id;
            mFeedbackToSave.FunctionSeqId = FunctionUtility.GetProfile(feedback.Action).Id;
            if(mRetVal != feedback && mSecurityInfo.MayAdd || mSecurityInfo.MayEdit)
            {
                mRetVal = FeedbackUtility.SaveFeedback(mFeedbackToSave);
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
        string mColumns = "[FeedbackId], [Assignee], [Details], [Found_In_Version], [Notes], [Severity], [Status], [TargetVersion], [Type], [VerifiedBy]";
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

    [Authorize("feedbacks")]
    [HttpPost("SubmitFeedback")]
    public ActionResult<UIFeedback> SubmitFeedback(UIFeedback feedbackResult)
    {
        UIFeedback mRetVal = new();

        return Ok(mRetVal);
    }

}
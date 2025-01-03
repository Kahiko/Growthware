using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.Web.Support.Utilities;
using GrowthWare.Web.Support.Jwt;
using System.Linq;
using System.Collections.Generic;

namespace GrowthWare.Web.Support.BaseControllers;

[CLSCompliant(false)]
public abstract class AbstractFeedbackController : ControllerBase
{
    private Logger m_Logger = Logger.Instance();

    /// <summary>
    /// Retrieves an array of strings representing the accounts in the Developer and QA roles.
    /// </summary>
    [AllowAnonymous]
    [HttpGet("GetFeedbackAccounts")]
    public ActionResult<Tuple<string[], string[]>> GetFeedbackAccounts()
    {
        MSecurityInfo mSecurityInfo = new(FunctionUtility.GetProfile(ConfigSettings.Actions_EditFeedback), AccountUtility.CurrentProfile);
        if (mSecurityInfo.MayEdit)
        {
            int mSecurityId = SecurityEntityUtility.CurrentProfile.Id;
            // Get all of the roles for the security entity
            List<MRole> mRoles = RoleUtility.GetRolesBySecurityEntity(mSecurityId);
            // Get the Developer and QA roles
            MRole mDeveloper = mRoles.FirstOrDefault<MRole>(x => x.Name.Equals("Developer", StringComparison.InvariantCultureIgnoreCase));
            MRole mQA = mRoles.FirstOrDefault<MRole>(x => x.Name.Equals("QA", StringComparison.InvariantCultureIgnoreCase));
            string[] mAccountsInRole_Developers = [];
            string[] mAccountsInRole_QA = [];
            // Ensure that the roles were found before we attempt to us them
            if (mDeveloper != null)
            {
                // Get the AccountsInRole from the UIProfile
                mAccountsInRole_Developers = RoleUtility.GetUIProfile(mDeveloper.Id, mSecurityId).AccountsInRole;
            }
            if (mQA != null)
            {
                mAccountsInRole_QA = RoleUtility.GetUIProfile(mQA.Id, mSecurityId).AccountsInRole;
            }
            // Add the Anonymous account to the feedback accounts
            List<string> mAccountsList_Developers = new List<string>(mAccountsInRole_Developers);
            List<string> mAccountsList_QA = new List<string>(mAccountsInRole_QA);
            if (!mAccountsList_Developers.Contains("Anonymous"))
            {
                mAccountsList_Developers.Add("Anonymous");
            }
            if (!mAccountsList_QA.Contains("Anonymous"))
            {
                mAccountsList_QA.Add("Anonymous");
            }
            Tuple<string[], string[]> mRetVal = new Tuple<string[], string[]>([.. mAccountsList_Developers.Order()],[.. mAccountsList_QA.Order()] );
            return Ok(mRetVal);
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");       
    }

    [Authorize("feedbacks")]
    [HttpGet("GetFeedbackForEdit")]
    public UIFeedback GetFeedbackForEdit(int feedbackId)
    {
        UIFeedback mRetVal = new();
        // Remove the EditId from the session
        HttpContext.Session.Remove("EditId");
        // Set the EditId in the session
        HttpContext.Session.SetInt32("EditId", mRetVal.FeedbackId);
        // Get the feedback from the data store
        mRetVal = FeedbackUtility.GetFeedback(feedbackId);
        return mRetVal;
    }

    [Authorize("feedbacks")]
    [HttpPost("SaveFeedback")]
    public ActionResult<bool> SaveFeedback(UIFeedback feedback)
    {
        bool mRetVal = false;
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.Actions_EditFeedback);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        var mEditId = HttpContext.Session.GetInt32("EditId");
        if (mEditId != null)
        {
            if(feedback != new UIFeedback()  && mSecurityInfo.MayAdd || mSecurityInfo.MayEdit)
            {
                MFeedback mFeedbackToSave = new(feedback);
                mFeedbackToSave.UpdatedById = mRequestingProfile.Id;
                // set the properties that may not have been set by the UI/Requestor
                if(feedback.FeedbackId == -1)
                {
                    mFeedbackToSave.FunctionSeqId = FunctionUtility.GetProfile(feedback.Action).Id;
                    mFeedbackToSave.DateOpened = DateTime.Now;
                    mFeedbackToSave.DateClosed = mFeedbackToSave.DefaultSystemDateTime;
                    mFeedbackToSave.SubmittedById = mRequestingProfile.Id;
                    mFeedbackToSave.FoundInVersion = ConfigSettings.Version;
                }
                try
                {
                    UIFeedback mSavedFeedback = FeedbackUtility.SaveFeedback(mFeedbackToSave);
                    if (mSavedFeedback != null)
                    {
                        mRetVal = true;
                        return Ok(mRetVal);
                    }
                } catch (Exception ex)
                {
                    m_Logger.Error(ex.Message);
                    return StatusCode(StatusCodes.Status500InternalServerError, "Could not save the feedback!");
                }
            }
        }
        this.m_Logger.Error(mRequestingProfile.Account + " does not have permissions to 'Save Feedback'");
        return StatusCode(StatusCodes.Status401Unauthorized, "The account does not have permissions to 'Save Feedback'");
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
        string mColumns = "[FeedbackId], [Assignee], [SubmittedBy], [Details], [Found_In_Version], [Notes], [Severity], [Status], [TargetVersion], [Type], [VerifiedBy]";
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
}

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
using System.Threading.Tasks;

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
    public async Task<ActionResult<Tuple<string[], string[]>>> GetFeedbackAccounts()
    {
        MFunctionProfile mFunctionProfile = await FunctionUtility.GetProfile(ConfigSettings.Actions_EditFeedback);
        MAccountProfile mAccountProfile = await AccountUtility.CurrentProfile();
        MSecurityInfo mSecurityInfo = new(mFunctionProfile, mAccountProfile);
        MSecurityEntity mSecurityEntity = await SecurityEntityUtility.CurrentProfile();
        if (mSecurityInfo.MayEdit)
        {
            int mSecurityId = mSecurityEntity.Id;
            // Get all of the roles for the security entity
            List<MRole> mRoles = await RoleUtility.GetRolesBySecurityEntity(mSecurityId);
            // Get the Developer and QA roles
            MRole mDeveloper = mRoles.FirstOrDefault<MRole>(x => x.Name.Equals("Developer", StringComparison.InvariantCultureIgnoreCase));
            MRole mQA = mRoles.FirstOrDefault<MRole>(x => x.Name.Equals("QA", StringComparison.InvariantCultureIgnoreCase));
            string[] mAccountsInRole_Developers = [];
            string[] mAccountsInRole_QA = [];
            // Ensure that the roles were found before we attempt to us them
            if (mDeveloper != null)
            {
                // Get the AccountsInRole from the UIProfile
                UIRole mDeveloperProfile = await RoleUtility.GetUIProfile(mDeveloper.Id, mSecurityId);
                mAccountsInRole_Developers = mDeveloperProfile.AccountsInRole;
            }
            if (mQA != null)
            {
                UIRole mQAProfile = await RoleUtility.GetUIProfile(mQA.Id, mSecurityId);
                mAccountsInRole_QA = mQAProfile.AccountsInRole;
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
    public async Task<UIFeedback> GetFeedbackForEdit(int feedbackId)
    {
        UIFeedback mRetVal = new();
        // Remove the EditId from the session
        HttpContext.Session.Remove("EditId");
        // Set the EditId in the session
        HttpContext.Session.SetInt32("EditId", mRetVal.FeedbackId);
        // Get the feedback from the data store
        mRetVal = await FeedbackUtility.GetFeedback(feedbackId);
        return mRetVal;
    }

    [Authorize("feedbacks")]
    [HttpPost("SaveFeedback")]
    public async Task<ActionResult<bool>> SaveFeedback(UIFeedback feedback)
    {
        bool mRetVal = false;
        MAccountProfile mRequestingProfile = await AccountUtility.CurrentProfile();
        MFunctionProfile mFunctionProfile = await FunctionUtility.GetProfile(ConfigSettings.Actions_EditFeedback);
        MSecurityInfo mSecurityInfo = new(mFunctionProfile, mRequestingProfile);
        var mEditId = HttpContext.Session.GetInt32("EditId");
        if (mEditId != null)
        {
            if(feedback != new UIFeedback()  && mSecurityInfo.MayAdd || mSecurityInfo.MayEdit)
            {
                // Populate a MFeedback object with the UI Feedback
                MFeedback mFeedbackToSave = new(feedback);
                // Get the Ids for the Assignee, VerifiedBy and FunctionSeq
                MAccountProfile mAnonymousAccount = await AccountUtility.GetAccount("Anonymous");
                MAccountProfile mAssigneeAccount = await AccountUtility.GetAccount(feedback.Assignee);
                int mAnonymousId = mAnonymousAccount.Id;
                int mAssigneeId = mAssigneeAccount.Id;
                int mFunctionSeqId = FunctionUtility.GetProfile(feedback.Action).Id;
                int mVerifiedById = -1;
                if (!string.IsNullOrEmpty(feedback.VerifiedBy))
                {
                    MAccountProfile mVerifiedByAccount = await AccountUtility.GetAccount(feedback.VerifiedBy);
                    mVerifiedById = mVerifiedByAccount.Id;
                }
                if (mAssigneeId == 0)
                {
                    // this must be set to an existing account id so we'll use the Anonymous account
                    mAssigneeId = mAnonymousId;
                }
                if (mFunctionSeqId == 0)
                {
                    // This is mostly for something other than our frontend calling the API
                    // We can't have a non-existing FunctionSeqId, so it's better to set it to the
                    // current FunctionSeqId
                    mFunctionSeqId = mFunctionProfile.Id;
                }
                if (mVerifiedById == 0)
                {
                    // this must be set to an existing account id so we'll use the Anonymous account
                    mVerifiedById = mAnonymousId;
                }
                // Set the properties that may not have been set by the UI/Requestor
                // In particular the AssigneeId, VerifiedById, FunctionSeqId, and UpdatedById
                mFeedbackToSave.AssigneeId = mAssigneeId;
                mFeedbackToSave.FunctionSeqId = mFunctionSeqId;
                mFeedbackToSave.VerifiedById = mVerifiedById;
                mFeedbackToSave.UpdatedById = mRequestingProfile.Id;
                if(feedback.FeedbackId == -1)
                {
                    // The default values for a new feedback
                    MFunctionProfile mActionFunctionProfile = await FunctionUtility.GetProfile(feedback.Action);
                    mFeedbackToSave.FunctionSeqId = mActionFunctionProfile.Id;
                    mFeedbackToSave.DateOpened = DateTime.Now;
                    mFeedbackToSave.DateClosed = mFeedbackToSave.DefaultSystemDateTime;
                    mFeedbackToSave.SubmittedById = mRequestingProfile.Id;
                    mFeedbackToSave.FoundInVersion = ConfigSettings.Version;
                }
                try
                {
                    UIFeedback mSavedFeedback = await FeedbackUtility.SaveFeedback(mFeedbackToSave);
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
    public async Task<ActionResult<String>> SearchFeedbacks(UISearchCriteria searchCriteria)
    {
        String mRetVal = string.Empty;
        string mColumns = "[FeedbackId], [Assignee], [SubmittedBy], [Details], [Found_In_Version], [Notes], [Severity], [Status], [TargetVersion], [Type], [VerifiedBy]";
        if (searchCriteria.sortColumns.Length > 0)
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
                TableOrView = "[ZGWOptional].[vwCurrentFeedbacks]",
                WhereClause = mWhereClause
            };

            mRetVal = await SearchUtility.GetSearchResults(mSearchCriteria);
        }
        return Ok(mRetVal);
    }
}

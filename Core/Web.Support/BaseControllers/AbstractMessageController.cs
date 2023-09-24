using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Collections;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.Web.Support.Jwt;
using GrowthWare.Web.Support.Utilities;
using System.Data.Common;

namespace GrowthWare.Web.Support.BaseControllers;

[CLSCompliant(false)]
public abstract class AbstractMessageController : ControllerBase
{
    /// <summary>
    /// Check if the requesting account has the correct permissions
    /// </summary>
    /// <param name="editId"></param>
    /// <param name="mSecurityInfo"></param>
    /// <returns></returns>
    private bool canAddOrEdit(int editId, MSecurityInfo mSecurityInfo)
    {
        bool mRetVal = false;
        if (mSecurityInfo.MayEdit || mSecurityInfo.MayAdd)
        {
            if (editId < 0 && mSecurityInfo.MayAdd)
            {
                mRetVal = true;
            }
            if (editId > -1 && mSecurityInfo.MayEdit)
            {
                mRetVal = true;
            }
        }
        return mRetVal;
    }

    [Authorize("Search_Messages")]
    [HttpGet("GetProfile")]
    public ActionResult<UIMessageProfile> GetProfile(int id)
    {
        MAccountProfile mRequestingProfile = (MAccountProfile)HttpContext.Items["AccountProfile"];
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.Actions_EditMessages);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);

        if (mSecurityInfo.MayView)
        {
            HttpContext.Session.SetString("EditId", id.ToString());
            MMessage mProfileFromDb = new MMessage();
            mProfileFromDb = MessageUtility.GetProfile(id);
            UIMessageProfile mRetVal = new UIMessageProfile();
            if (mProfileFromDb != null)
            {
                mRetVal = new UIMessageProfile(mProfileFromDb);
                try
                {
                    string mAssembley = "GrowthWare.Framework";
                    string mNameSpace = "GrowthWare.Framework.Models";
                    MMessage mMessageProfile = (MMessage)ObjectFactory.Create(mAssembley, mNameSpace, "M" + mRetVal.Name);
                    mRetVal.AvalibleTags = mMessageProfile.GetTags(Environment.NewLine); ;
                }
                catch (System.Exception)
                {
                    mRetVal.AvalibleTags = "No tags avalible for this message.";
                }
            }
            HttpContext.Session.SetInt32("EditId", mRetVal.Id);
            return Ok(mRetVal);
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    [Authorize("Search_Messages")]
    [HttpPost("Save")]
    public ActionResult<UIMessageProfile> Save(UIMessageProfile messageProfile)
    {
        if (HttpContext.Session.GetString("EditId") != null && HttpContext.Session.GetInt32("EditId") == messageProfile.Id)
        {
            MAccountProfile mRequestingProfile = (MAccountProfile)HttpContext.Items["AccountProfile"];
            MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.Actions_EditMessages);
            MSecurityEntity mSecurityEntity = SecurityEntityUtility.CurrentProfile();
            MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
            if (canAddOrEdit(messageProfile.Id, mSecurityInfo))
            {
                MMessage mProfileToSave = new MMessage();
                MMessage mProfileFromDb = MessageUtility.GetProfile(messageProfile.Id);
                if (mProfileFromDb != null)
                {
                    mProfileToSave = new MMessage(mProfileFromDb);
                }
                updateProfileWithUIValues(ref mProfileToSave, messageProfile);
                updateAddUpdated(ref mProfileToSave, mRequestingProfile.Id);
                mProfileToSave.Id = MessageUtility.Save(mProfileToSave);
                return Ok(messageProfile);
            }
            return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
        }
        return StatusCode(StatusCodes.Status304NotModified, "Unable to save");
    }

    [Authorize("Search_Messages")]
    [HttpPost("SearchMessages")]
    public String SearchMessages(UISearchCriteria searchCriteria)
    {
        String mRetVal = string.Empty;
        string mColumns = "[MessageSeqId], [Name], [Title], [Description], [Added_By], [Added_Date], [Updated_By], [Updated_Date]";
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
                TableOrView = "[ZGWCoreWeb].[vwSearchMessages]",
                WhereClause = mWhereClause
            };
            mRetVal = SearchUtility.GetSearchResults(mSearchCriteria);
        }
        return mRetVal;
    }

    /// <summary>
    /// Update the profile to be saved with the values passed from the UI
    /// </summary>
    /// <param name="mProfileToSave">MMessage</param>
    /// <param name="messageProfile">UIMessageProfile</param>
    private void updateProfileWithUIValues(ref MMessage mProfileToSave, UIMessageProfile messageProfile)
    {
        mProfileToSave.Body = WebUtility.UrlDecode(messageProfile.Body);
        mProfileToSave.Description = messageProfile.Description;
        mProfileToSave.FormatAsHtml = messageProfile.FormatAsHtml;
        mProfileToSave.Id = messageProfile.Id;
        mProfileToSave.Name = messageProfile.Name;
        mProfileToSave.SecurityEntitySeqId = SecurityEntityUtility.CurrentProfile().Id;
        mProfileToSave.Title = messageProfile.Title;
    }

    /// <summary>
    /// Updates the profile to save's AddedBy/AddedDate or UpdatedBy/UpdatedDate 
    /// </summary>
    /// <param name="mProfileToSave"></param>
    /// <param name="requestedProfileId"></param>
    private void updateAddUpdated(ref MMessage mProfileToSave, int requestedProfileId)
    {
        if (mProfileToSave.Id > -1)
        {
            mProfileToSave.UpdatedBy = requestedProfileId;
            mProfileToSave.UpdatedDate = DateTime.Now;
        }
        else
        {
            mProfileToSave.AddedBy = requestedProfileId;
            mProfileToSave.AddedDate = DateTime.Now;
        }
    }
}
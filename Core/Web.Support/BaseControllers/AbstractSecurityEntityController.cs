using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using GrowthWare.Framework.Enumerations;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.Web.Support.Jwt;
using GrowthWare.Web.Support.Utilities;

namespace GrowthWare.Web.Support.BaseControllers;

[CLSCompliant(false)]
public abstract class AbstractSecurityEntityController : ControllerBase
{

    private MSecurityEntity clone(MSecurityEntity original){
        return JsonSerializer.Deserialize<MSecurityEntity>(JsonSerializer.Serialize(original));
    }

    [AllowAnonymous]
    [HttpGet("GetProfile")]
    public ActionResult<MSecurityEntity> GetProfile(int securityEntitySeqId)
    {
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        MSecurityInfo mSecurityInfo = this.getSecurityInfo("search_security_entities");
        MSecurityEntity mOriginal = new MSecurityEntity();
        if (securityEntitySeqId > -1)
        {
            mOriginal = SecurityEntityUtility.GetProfile(securityEntitySeqId);
        }
        if (mOriginal == null)
        {
            mOriginal = new MSecurityEntity();
        }
        HttpContext.Session.SetInt32("EditId", mOriginal.Id);
        // Serialize the object to a JSON string
        var mJsonString = JsonSerializer.Serialize(mOriginal);
        // Deserialize the JSON string back to an instance of your class
        MSecurityEntity mRetVal = this.clone(mOriginal);
        mRetVal.ConnectionString = ""; // We don't want the password to ever be displayed
        return Ok(mRetVal);
    }

    private MSecurityInfo getSecurityInfo(string action)
    {
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(action);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        return mSecurityInfo;
    }

    [AllowAnonymous]
    [HttpGet("GetValidParents")]
    public ActionResult<List<UIKeyValuePair>> GetValidParents(int securityEntitySeqId)
    {
        MSecurityInfo mSecurityInfo = this.getSecurityInfo("search_security_entities");
        if (mSecurityInfo.MayView)
        {
            List<UIKeyValuePair> mRetVal = new List<UIKeyValuePair>();
            var mSecurityEntities = from mProfile in SecurityEntityUtility.Profiles()
                                    where mProfile.Id != securityEntitySeqId
                                    select mProfile;
            mSecurityEntities.ToList().ForEach(item => mRetVal.Add(new UIKeyValuePair { Key = item.Id, Value = item.Name }));
            return mRetVal;
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    [AllowAnonymous]
    [HttpGet("GetValidSecurityEntities")]
    public ActionResult<List<UIValidSecurityEntity>> GetValidSecurityEntities()
    {
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        if (mRequestingProfile.Account.ToLower() == "anonymous" && mRequestingProfile.Status != (int)SystemStatus.Active)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
        }
        MSecurityEntity mSecurityEntity = SecurityEntityUtility.CurrentProfile();
        List<UIValidSecurityEntity> mRetVal = new List<UIValidSecurityEntity>();

        DataTable mDataView = SecurityEntityUtility.GetValidSecurityEntities(mRequestingProfile.Account, mSecurityEntity.Id, mRequestingProfile.IsSystemAdmin);
        foreach (DataRow mDataRowView in mDataView.Rows)
        {
            UIValidSecurityEntity mItem = new UIValidSecurityEntity(mDataRowView);
            mRetVal.Add(mItem);
        }
        return Ok(mRetVal);
    }

    [Authorize("Search_Security_Entities")]
    [HttpPost("SaveProfile")]
    public ActionResult<bool> SaveProfile(MSecurityEntity securityEntity)
    {
        MSecurityInfo mSecurityInfo = this.getSecurityInfo("search_security_entities");
        MSecurityEntity mProfileToSave = this.clone(securityEntity);
        if (mSecurityInfo.MayAdd || mSecurityInfo.MayEdit)
        {
            MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
            MSecurityEntity mSecurityEntity = SecurityEntityUtility.CurrentProfile();
            if (mProfileToSave.Id == -1)
            {
                if (!mSecurityInfo.MayAdd)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
                }
                mProfileToSave.AddedBy = mRequestingProfile.Id;
                mProfileToSave.AddedDate = DateTime.Now;
            }
            else
            {
                if (!mSecurityInfo.MayEdit)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
                }
                if (securityEntity.Id != -1 && string.IsNullOrWhiteSpace(securityEntity.ConnectionString))
                {
                    mProfileToSave.ConnectionString = SecurityEntityUtility.GetProfile(securityEntity.Id).ConnectionString;
                }
                mProfileToSave.UpdatedBy = mRequestingProfile.Id;
                mProfileToSave.UpdatedDate = DateTime.Now;
            }
            SecurityEntityUtility.SaveProfile(mProfileToSave);
        }
        return Ok(true);
    }

    [Authorize("Search_Security_Entities")]
    [HttpPost("Security_Entities")]
    public String Security_Entities(UISearchCriteria searchCriteria)
    {
        String mRetVal = string.Empty;
        string mColumns = "[SecurityEntitySeqId], [Name], [Description], [Skin]";
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
                TableOrView = "[ZGWSecurity].[Security_Entities]",
                WhereClause = mWhereClause
            };
            mRetVal = SearchUtility.GetSearchResults(mSearchCriteria);
        }
        return mRetVal;
    }

}

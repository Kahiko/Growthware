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
using GrowthWare.Framework;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace GrowthWare.Web.Support.BaseControllers;

[CLSCompliant(false)]
public abstract class AbstractSecurityEntityController : ControllerBase
{

    private MSecurityEntity clone(MSecurityEntity original)
    {
        return JsonSerializer.Deserialize<MSecurityEntity>(JsonSerializer.Serialize(original));
    }

    private MRegistrationInformation clone(MRegistrationInformation original)
    {
        return JsonSerializer.Deserialize<MRegistrationInformation>(JsonSerializer.Serialize(original));
    }

    [AllowAnonymous]
    [HttpGet("GetProfile")]
    public async Task<ActionResult<MSecurityEntity>> GetProfile(int securityEntitySeqId)
    {
        MAccountProfile mRequestingProfile = await AccountUtility.CurrentProfile();
        MSecurityInfo mSecurityInfo = await this.getSecurityInfo(ConfigSettings.Actions_EditSecurityEntities);
        MSecurityEntity mOriginal = new MSecurityEntity();
        if (securityEntitySeqId > -1)
        {
            mOriginal = await SecurityEntityUtility.GetProfile(securityEntitySeqId);
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

    [AllowAnonymous]
    [HttpGet("GetProfileByURL")]
    public async Task<ActionResult<MSecurityEntity>> GetProfileByURL(string url)
    {
        MSecurityEntity mRetVal = await SecurityEntityUtility.GetProfileByUrl(url);
        if (mRetVal == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "No matching profile found");
        }
        return Ok(mRetVal);
    }
    
    [HttpGet("GetRegistrationInformation")]
    public async Task<ActionResult<MRegistrationInformation>> GetRegistrationInformation(int securityEntitySeqId)
    {
        MSecurityInfo mSecurityInfo = await this.getSecurityInfo(ConfigSettings.Actions_EditSecurityEntities);
        if (mSecurityInfo.MayView)
        {
            return Ok(await SecurityEntityUtility.GetRegistrationInformation(securityEntitySeqId));
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }


    private async Task<MSecurityInfo> getSecurityInfo(string action)
    {
        MAccountProfile mRequestingProfile = await AccountUtility.CurrentProfile();
        MFunctionProfile mFunctionProfile = await FunctionUtility.GetProfile(action);
        MSecurityInfo mSecurityInfo = new(mFunctionProfile, mRequestingProfile);
        return mSecurityInfo;
    }

    [AllowAnonymous]
    [HttpGet("GetValidParents")]
    public async Task<ActionResult<List<UIKeyValuePair>>> GetValidParents(int securityEntitySeqId)
    {
        MSecurityInfo mSecurityInfo = await this.getSecurityInfo(ConfigSettings.Actions_EditSecurityEntities);
        if (mSecurityInfo.MayView)
        {
            List<UIKeyValuePair> mRetVal = new List<UIKeyValuePair>();
            Collection<MSecurityEntity> mAllSecurityEntities = await SecurityEntityUtility.Profiles();
            var mSecurityEntities = from mProfile in mAllSecurityEntities
                                    where mProfile.Id != securityEntitySeqId
                                    select mProfile;
            mSecurityEntities.ToList().ForEach(item => mRetVal.Add(new UIKeyValuePair { Key = item.Id, Value = item.Name }));
            return mRetVal;
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    [AllowAnonymous]
    [HttpGet("GetValidSecurityEntities")]
    public async Task<ActionResult<List<UIValidSecurityEntity>>> GetValidSecurityEntities()
    {
        MAccountProfile mRequestingProfile = await AccountUtility.CurrentProfile();
        if (mRequestingProfile.Account.ToLower() == "anonymous" && mRequestingProfile.Status != (int)SystemStatus.Active)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
        }
        MSecurityEntity mSecurityEntity = await SecurityEntityUtility.CurrentProfile();
        List<UIValidSecurityEntity> mRetVal = [];
        DataTable mDataView = await SecurityEntityUtility.GetValidSecurityEntities(mRequestingProfile.Account, mSecurityEntity.Id, mRequestingProfile.IsSystemAdmin);
        foreach (DataRow mDataRowView in mDataView.Rows)
        {
            UIValidSecurityEntity mItem = new UIValidSecurityEntity(mDataRowView);
            mRetVal.Add(mItem);
        }
        return Ok(mRetVal);
    }

    [Authorize("securityEntity")]
    [HttpPost("SaveProfile")]
    public async Task<ActionResult<bool>> SaveProfile(DTO_SecurityEntity_RegistrationInfo securityEntityWithRegistrationInformation)
    {
        MSecurityInfo mSecurityInfo = await this.getSecurityInfo(ConfigSettings.Actions_EditSecurityEntities);
        MSecurityEntity mParamSecurityEntity = securityEntityWithRegistrationInformation.SecurityEntity;
        MRegistrationInformation mParamRegistrationInformation = securityEntityWithRegistrationInformation.RegistrationInformation;
        MSecurityEntity mProfileToSave = this.clone(mParamSecurityEntity);
        MRegistrationInformation mRegistrationToSave = this.clone(mParamRegistrationInformation);
        var mEditId = HttpContext.Session.GetInt32("EditId");
        if (mEditId != null && (mSecurityInfo.MayAdd || mSecurityInfo.MayEdit))
        {
            MAccountProfile mRequestingProfile = await AccountUtility.CurrentProfile();
            MSecurityEntity mSecurityEntity = await SecurityEntityUtility.CurrentProfile();
            if (mProfileToSave.Id == -1)
            {
                if (!mSecurityInfo.MayAdd)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
                }
                mProfileToSave.AddedBy = mRequestingProfile.Id;
                mProfileToSave.AddedDate = DateTime.Now;
                mRegistrationToSave.AddedBy = mRequestingProfile.Id;
                mRegistrationToSave.AddedDate = DateTime.Now;
            }
            else
            {
                if (!mSecurityInfo.MayEdit)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
                }
                if (mParamSecurityEntity.Id != -1 && string.IsNullOrWhiteSpace(mParamSecurityEntity.ConnectionString))
                {
                    MSecurityEntity mDBSecurityEntity = await SecurityEntityUtility.GetProfile(mParamSecurityEntity.Id);
                    mProfileToSave.ConnectionString = mDBSecurityEntity.ConnectionString;
                }
                mProfileToSave.UpdatedBy = mRequestingProfile.Id;
                mProfileToSave.UpdatedDate = DateTime.Now;
                mRegistrationToSave.UpdatedBy = mRequestingProfile.Id;
                mRegistrationToSave.UpdatedDate = DateTime.Now;
            }
            mRegistrationToSave.Id = await SecurityEntityUtility.SaveProfile(mProfileToSave);
            if(mRegistrationToSave.AddAccount > 0){
                await SecurityEntityUtility.SaveRegistrationInformation(mRegistrationToSave);
            }
            else
            {
                if (!mSecurityInfo.MayDelete)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
                }
                await SecurityEntityUtility.DeleteRegistrationInformation(mRegistrationToSave.Id);
            }
        } 
        else 
        {
            return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
        }
        return Ok(true);
    }

    [Authorize("securityEntity")]
    [HttpPost("Security_Entities")]
    public async Task<String> Security_Entities(UISearchCriteria searchCriteria)
    {
        String mRetVal = string.Empty;
        string mColumns = "[SecurityEntitySeqId], [Name], [Description], [Skin]";
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
                TableOrView = "[ZGWSecurity].[Security_Entities]",
                WhereClause = mWhereClause
            };
            mRetVal = await SearchUtility.GetSearchResults(mSearchCriteria);
        }
        return mRetVal;
    }

}

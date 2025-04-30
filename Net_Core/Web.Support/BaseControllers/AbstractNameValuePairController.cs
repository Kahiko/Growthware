using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.Web.Support.Helpers;
using GrowthWare.Web.Support.Jwt;
using GrowthWare.Web.Support.Utilities;
using System.Threading.Tasks;

namespace GrowthWare.Web.Support.BaseControllers;

[CLSCompliant(false)]
public abstract class AbstractNameValuePairController : ControllerBase
{
    private CacheHelper m_CacheHelper = CacheHelper.Instance();
    private Logger m_Logger = Logger.Instance();
    private string s_ParrentCacheName = "NameValuePairs";

    [AllowAnonymous]
    [HttpGet("GetMNameValuePairDetail")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<MNameValuePairDetail>> GetMNameValuePairDetail(int nvpSeqId, int nvpDetailSeqId)
    {
        HttpContext.Session.SetInt32("EditId", nvpDetailSeqId);
        MNameValuePairDetail mRetVal = await NameValuePairUtility.GetNameValuePairDetail(nvpSeqId, nvpDetailSeqId);
        if (mRetVal == null)
        {
            mRetVal = new MNameValuePairDetail();
        }
        return Ok(mRetVal);
    }

    [AllowAnonymous]
    [HttpGet("GetMNameValuePair")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<MNameValuePair>> GetMNameValuePair(int nameValuePairSeqId)
    {
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.Actions_EditNameValueParent);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        if (mSecurityInfo.MayEdit)
        {
            List<MNameValuePair> mNameValuePairs = await this.GetMNameValuePairs();
            MNameValuePair mRetVal = mNameValuePairs.FirstOrDefault(x => x.Id == nameValuePairSeqId);
            if (mRetVal == null)
            {
                mRetVal = new MNameValuePair();
            }
            HttpContext.Session.SetInt32("EditId", mRetVal.Id);
            return Ok(mRetVal);
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    [AllowAnonymous]
    [HttpGet("GetMNameValuePairs")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<List<MNameValuePair>> GetMNameValuePairs()
    {
        List<MNameValuePair> mRetVal = this.m_CacheHelper.GetFromCache<List<MNameValuePair>>(this.s_ParrentCacheName);
        if (mRetVal == null)
        {
            // BNameValuePairs mBNameValuePairs = new BNameValuePairs(SecurityEntityUtility.CurrentProfile);
            mRetVal = await NameValuePairUtility.GetNameValuePairs();
            this.m_CacheHelper.AddToCache(this.s_ParrentCacheName, mRetVal);
        }
        return mRetVal;
    }

    [Authorize("search_name_value_pairs")]
    [HttpPost("SaveNameValuePairDetail")]
    public async Task<ActionResult<MNameValuePairDetail>> SaveNameValuePairDetail(MNameValuePairDetail nameValuePairDetail)
    {
        if (nameValuePairDetail.Id == -1 || nameValuePairDetail.Id == HttpContext.Session.GetInt32("EditId"))
        {
            MNameValuePairDetail mNameValuePairDetail = nameValuePairDetail;
            MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
            MSecurityEntity mSecurityEntity = SecurityEntityUtility.CurrentProfile;
            if (mNameValuePairDetail.Id != -1)
            {
                MNameValuePairDetail mOriginal = await NameValuePairUtility.GetNameValuePairDetail(nameValuePairDetail.NameValuePairSeqId, nameValuePairDetail.Id);
                mNameValuePairDetail.AddedDate = mOriginal.AddedDate;
                mNameValuePairDetail.AddedBy = mOriginal.AddedBy;
                mNameValuePairDetail.UpdatedDate = DateTime.Now;
                mNameValuePairDetail.UpdatedBy = mRequestingProfile.Id;
            }
            else
            {
                mNameValuePairDetail.AddedDate = DateTime.Now;
                mNameValuePairDetail.AddedBy = mRequestingProfile.Id;
            }
            try
            {
                MNameValuePairDetail mRetVal = await NameValuePairUtility.SaveNameValuePairDetail(mNameValuePairDetail);
                HttpContext.Session.SetInt32("EditId", -1);
                return Ok(mRetVal);                
            }
            catch (System.Exception ex)
            {
                Exception mException = new Exception("Error Saving NameValuePairDetail", ex);
                this.m_Logger.Error(mException);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Saving NameValuePairDetail");
            }
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    [Authorize("search_name_value_pairs")]
    [HttpPost("SaveNameValuePairParent")]
    public async Task<ActionResult<MNameValuePair>> SaveNameValuePairParent(MNameValuePair nameValuePair)
    {
        if (nameValuePair.Id == -1 || nameValuePair.Id == HttpContext.Session.GetInt32("EditId"))
        {
            MNameValuePair mNameValuePair = nameValuePair;
            MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
            MSecurityEntity mSecurityEntity = SecurityEntityUtility.CurrentProfile;
            if (mNameValuePair.Id == -1)
            {
                mNameValuePair.AddedDate = DateTime.Now;
                mNameValuePair.AddedBy = mRequestingProfile.Id;
            }
            else
            {
                List<MNameValuePair> mNameValuePairs = await NameValuePairUtility.GetNameValuePairs();
                MNameValuePair mOriginal = mNameValuePairs.FirstOrDefault(x => x.Id == mNameValuePair.Id);
                mNameValuePair.AddedDate = mOriginal.AddedDate;
                mNameValuePair.AddedBy = mOriginal.AddedBy;
                mNameValuePair.UpdatedDate = DateTime.Now;
                mNameValuePair.UpdatedBy = mRequestingProfile.Id;
            }
            HttpContext.Session.SetInt32("EditId", -1);
            MNameValuePair mRetVal = await NameValuePairUtility.SaveNameValuePairParent(mNameValuePair);
            this.m_CacheHelper.RemoveFromCache(this.s_ParrentCacheName);
            return Ok(mRetVal);
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    [Authorize("search_name_value_pairs")]
    [HttpPost("SearchNameValuePairs")]
    public String SearchNameValuePairs(UISearchCriteria searchCriteria)
    {
        String mRetVal = string.Empty;
        string mColumns = "[nvpSeqId] = [NVPSeqId], [schemaName] = [Schema_Name], [staticName] = [Static_Name], [display], [description], [StatusSeqId]";
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
                TableOrView = "[ZGWSystem].[Name_Value_Pairs]",
                WhereClause = mWhereClause
            };

            mRetVal = SearchUtility.GetSearchResults(mSearchCriteria);
        }
        return mRetVal;
    }

    /// <summary>
    /// Method to get all children for a name value pair (Security is ignored).  Created to support add/edit name value pair details
    /// </summary>
    /// <param name="searchCriteria"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost("SearcNVPDetails")]
    public async Task<String> SearcNVPDetails(UISearchCriteria searchCriteria)
    {
        if (searchCriteria == null) throw new ArgumentNullException(nameof(searchCriteria), "searchCriteria cannot be a null reference (Nothing in VB) or empty!");
        int mNameValuePairSeqId = int.Parse(searchCriteria.searchText);
        String mRetVal = await NameValuePairUtility.GetAllChildrenForParent(mNameValuePairSeqId);
        return mRetVal;
    }
}
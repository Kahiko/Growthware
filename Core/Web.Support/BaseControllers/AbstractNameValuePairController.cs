using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.Web.Support.Jwt;
using GrowthWare.Web.Support.Utilities;

namespace GrowthWare.Web.Support.BaseControllers;

[CLSCompliant(false)]
public abstract class AbstractNameValuePairController : ControllerBase
{
    private CacheController m_CacheController = CacheController.Instance();
    private Logger m_Logger = Logger.Instance();

    [AllowAnonymous]
    [HttpGet("GetMNameValuePair")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]  
    public ActionResult<MNameValuePair> GetMNameValuePair(int nameValuePairSeqId) 
    {
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.Actions_EditNameValueParent);
        MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mRequestingProfile);
        if (mSecurityInfo.MayEdit)
        {
            List<MNameValuePair> mNameValuePairs = this.GetMNameValuePairs();
            MNameValuePair mRetVal = mNameValuePairs.FirstOrDefault(x => x.Id == nameValuePairSeqId);
            HttpContext.Session.SetInt32("EditId", mRetVal.Id);
            return Ok(mRetVal);
        }
        return StatusCode(StatusCodes.Status401Unauthorized, "The requesting account does not have the correct permissions");
    }

    [AllowAnonymous]
    [HttpGet("GetMNameValuePairs")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]    
    public List<MNameValuePair> GetMNameValuePairs()
    {
        List<MNameValuePair> mRetVal = this.m_CacheController.GetFromCache<List<MNameValuePair>>("NameValuePairs");
        if(mRetVal == null) 
        {
            // BNameValuePairs mBNameValuePairs = new BNameValuePairs(SecurityEntityUtility.CurrentProfile);
            mRetVal = NameValuePairUtility.GetMNameValuePairs();
            this.m_CacheController.AddToCache("NameValuePairs", mRetVal);
        }
        return mRetVal;
    }

    [Authorize("search_name_value_pairs")]
    [HttpPost("SaveNameValuePairParent")]
    public ActionResult<MNameValuePair> SaveNameValuePairParent(MNameValuePair nameValuePair)
    {
        MNameValuePair mNameValuePair = nameValuePair;
        MAccountProfile mRequestingProfile = AccountUtility.CurrentProfile;
        MSecurityEntity mSecurityEntity = SecurityEntityUtility.CurrentProfile;
        if(mNameValuePair.Id == -1)
        {
            mNameValuePair.AddedDate = DateTime.Now;
            mNameValuePair.AddedBy = mRequestingProfile.Id;
        }
        else
        {
            MNameValuePair mOriginal = NameValuePairUtility.GetMNameValuePairs().FirstOrDefault(x => x.Id == mNameValuePair.Id);
            mNameValuePair.AddedDate = mOriginal.AddedDate;
            mNameValuePair.AddedBy = mOriginal.AddedBy;
            mNameValuePair.UpdatedDate = DateTime.Now;
            mNameValuePair.UpdatedBy = mRequestingProfile.Id;
        }
        HttpContext.Session.SetInt32("EditId", -1);
        return Ok(NameValuePairUtility.SaveNameValuePairParent(mNameValuePair));
    }

    [Authorize("search_name_value_pairs")]
    [HttpPost("SearchNameValuePairs")]
    public String SearchNameValuePairs(UISearchCriteria searchCriteria)
    {
        String mRetVal = string.Empty;
        string mColumns = "[nvpSeqId] = [NVPSeqId], [schemaName] = [Schema_Name], [staticName] = [Static_Name], [display], [description], [StatusSeqId]";
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
    public String SearcNVPDetails(UISearchCriteria searchCriteria)
    {
        if (searchCriteria == null) throw new ArgumentNullException(nameof(searchCriteria), "searchCriteria cannot be a null reference (Nothing in VB) or empty!");
        int mNameValuePairSeqId = int.Parse(searchCriteria.searchText);
        String mRetVal = NameValuePairUtility.GetAllChildrenForParent(mNameValuePairSeqId);
        return mRetVal;
    }   
}
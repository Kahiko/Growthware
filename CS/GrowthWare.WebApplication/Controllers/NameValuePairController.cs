using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebApplication.Models;
using GrowthWare.WebSupport;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace GrowthWare.WebApplication.Controllers
{
    public class NameValuePairController : ApiController
    {
        [HttpPost]
        public IHttpActionResult SaveNameValuePair(UINVPProfile uiProfile)
        {
            string mRetVal = false.ToString();
            MNameValuePair mProfile = new MNameValuePair();
            String mAction = GWWebHelper.GetQueryValue(HttpContext.Current.Request, "Action");
            int mEditId = int.Parse(HttpContext.Current.Items["EditId"].ToString());
            Logger mLog = Logger.Instance();
            if (mEditId != uiProfile.NVP_SEQ_ID)
            {
                Exception mError = new Exception("Identifier you have last looked at does not match the one passed in nothing has been saved!!!!");
                mLog.Error(mError);
                return this.InternalServerError(mError);
            }

            MAccountProfile mUpdatingAccount = AccountUtility.CurrentProfile();
            MSecurityEntityProfile mSecurityEntityProfile = SecurityEntityUtility.CurrentProfile();
            MSecurityInfo mSecurityInfo = new MSecurityInfo(FunctionUtility.GetProfile(mAction), mUpdatingAccount);
            String mGroups = String.Join(",", uiProfile.Groups);
            String mRoles = String.Join(",", uiProfile.Roles);
            String mCommaSepRoles = mUpdatingAccount.GetCommaSeparatedAssignedRoles;
            if (uiProfile.NVP_SEQ_ID != -1)
            {
                if (!mSecurityInfo.MayAdd)
                {
                    Exception mError = new Exception("The account (" + mUpdatingAccount.Account + ") being used does not have the correct permissions to add");
                    mLog.Error(mError);
                    return this.InternalServerError(mError);
                }
            }
            else
            {
                if (!mSecurityInfo.MayAdd)
                {
                    Exception mError = new Exception("The account (" + mUpdatingAccount.Account + ") being used does not have the correct permissions to edit");
                    mLog.Error(mError);
                    return this.InternalServerError(mError);
                }
            }
            if (uiProfile.NVP_SEQ_ID != -1)
            {
                mProfile = NameValuePairUtility.GetNameValuePair(uiProfile.NVP_SEQ_ID);
            }
            else
            {
                mProfile.AddedBy = mUpdatingAccount.Id;
                mProfile.AddedDate = DateTime.Today;
            }

            mProfile.UpdatedBy = mUpdatingAccount.Id;
            mProfile.UpdatedDate = DateTime.Today;
            mProfile.StaticName = uiProfile.STATIC_NAME;
            mProfile.SchemaName = uiProfile.SchemaName;
            mProfile.Display = uiProfile.Display;
            mProfile.Description = uiProfile.Description;
            mProfile.Status = uiProfile.Status;
            int mID = NameValuePairUtility.Save(mProfile);
            int mSecurityId = mSecurityEntityProfile.Id;
            NameValuePairUtility.UpdateRoles(mID, mSecurityId, mRoles, mProfile);
            NameValuePairUtility.UpdateGroups(mID, mSecurityId, mGroups, mProfile);
            return this.Ok(mRetVal);
        }

        [HttpPost]
        public IHttpActionResult SaveNameValuePairDetail(UINVPDetailProfile uiProfile) 
        {
            string mRetVal = false.ToString();
            MAccountProfile mUpdatingAccount = AccountUtility.CurrentProfile();
            String mAction = GWWebHelper.GetQueryValue(HttpContext.Current.Request, "Action");
            int mEditId = int.Parse(HttpContext.Current.Items["EditId"].ToString());
            Logger mLog = Logger.Instance();
            MSecurityInfo mSecurityInfo = new MSecurityInfo(FunctionUtility.GetProfile(mAction), mUpdatingAccount);

            if (mEditId != uiProfile.NVP_SEQ_ID)
            {
                Exception mError = new Exception("Identifier you have last looked at does not match the one passed in nothing has been saved!!!!");
                mLog.Error(mError);
                return this.InternalServerError(mError);
            }
            if (uiProfile.NVP_SEQ_ID != -1)
            {
                if (!mSecurityInfo.MayAdd)
                {
                    Exception mError = new Exception("The account (" + mUpdatingAccount.Account + ") being used does not have the correct permissions to add");
                    mLog.Error(mError);
                    return this.InternalServerError(mError);
                }
            }
            else
            {
                if (!mSecurityInfo.MayAdd)
                {
                    Exception mError = new Exception("The account (" + mUpdatingAccount.Account + ") being used does not have the correct permissions to edit");
                    mLog.Error(mError);
                    return this.InternalServerError(mError);
                }
            }

            try
            {
			    MNameValuePairDetail mProfile = new MNameValuePairDetail();
			    if (uiProfile.NVP_SEQ_DET_ID != -1)
			    {
                    mProfile = NameValuePairUtility.GetNameValuePairDetail(uiProfile.NVP_SEQ_DET_ID, uiProfile.NVP_SEQ_ID);
			    }
			    else 
			    {
                    mProfile.AddedBy = mUpdatingAccount.Id;
				    mProfile.AddedDate = DateTime.Now;
			    }
			    mProfile.NameValuePairSeqId = uiProfile.NVP_SEQ_ID;
                mProfile.UpdatedBy = mUpdatingAccount.Id;
			    mProfile.UpdatedDate = DateTime.Now;
			    mProfile.SortOrder = uiProfile.SortOrder;
			    mProfile.Text = uiProfile.Text;
			    mProfile.Value = uiProfile.Value;
			    mProfile.Status = uiProfile.Status;
			    NameValuePairUtility.SaveDetail(mProfile);
            }
            catch (Exception ex)
            {
                mLog.Error(mUpdatingAccount);
            }

            return this.Ok(mRetVal);
        }
    }
}
using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Enumerations;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace GrowthWare.WebApplication.Controllers
{
    public class MessagesController : ApiController
    {

        [HttpPost()]
        public IHttpActionResult Delete([FromUri] int messageSeqId)
        {
            if (messageSeqId <= 0) throw new ArgumentNullException("messageSeqId", " must be a positive number!");
            string mRetVal = "False";
            Logger mLog = Logger.Instance();
            if (HttpContext.Current.Items["EditId"] != null)
            {
                int mEditId = int.Parse(HttpContext.Current.Items["EditId"].ToString());
                if (mEditId == messageSeqId)
                {
                    MSecurityInfo mSecurityInfo = (MSecurityInfo)HttpContext.Current.Items["SecurityInfo"];
                    if (mSecurityInfo != null)
                    {
                        if (mSecurityInfo.MayDelete)
                        {
                            try
                            {
                                //AccountUtility.Delete(accountSeqId);
                                mRetVal = "True";
                            }
                            catch (Exception ex)
                            {
                                mLog.Error(ex);
                                throw;
                            }
                        }
                        else
                        {
                            Exception mError = new Exception("The account (" + AccountUtility.CurrentProfile().Account + ") being used does not have the correct permissions to delete");
                            mLog.Error(mError);
                            return this.InternalServerError(mError);
                        }
                    }
                    else
                    {
                        Exception mError = new Exception("Security Info is not in context nothing has been saved!!!!");
                        mLog.Error(mError);
                        return this.InternalServerError(mError);
                    }
                }
                else
                {
                    Exception mError = new Exception("Identifier you have last looked at does not match the one passed in nothing has been saved!!!!");
                    mLog.Error(mError);
                    return this.InternalServerError(mError);
                }
            }
            return Ok(mRetVal);
        }

        [HttpPost()]
        public IHttpActionResult Save(UIMessageProfile profile)
        {
            if (profile == null) throw new ArgumentNullException("profile", "uiProfile cannot be a null reference (Nothing in Visual Basic)!");
            string mRetVal = "False";
            MAccountProfile mCurrentAccountProfile = AccountUtility.CurrentProfile();
            Logger mLog = Logger.Instance();
            MMessageProfile mProfileToSave = populateProfile(profile, mCurrentAccountProfile);
            if (HttpContext.Current.Items["EditId"] != null)
            {
                int mEditId = int.Parse(HttpContext.Current.Items["EditId"].ToString());
                if (mEditId == profile.Id)
                {
                    MSecurityInfo mSecurityInfo = (MSecurityInfo)HttpContext.Current.Items["SecurityInfo"];
                    if (mSecurityInfo != null)
                    {
                        if (mEditId != 1)
                        {
                            if (mSecurityInfo.MayEdit)
                            {
                                MessageUtility.Save(mProfileToSave);
                                mLog.Debug("Saved message " + profile.Name + " by " + mCurrentAccountProfile.Account);
                                mRetVal = "true";
                            }
                            else
                            {
                                Exception mError = new Exception("The account (" + AccountUtility.CurrentProfile().Account + ") being used does not have the correct permissions to edit");
                                mLog.Error(mError);
                                return this.InternalServerError(mError);
                            }
                        }
                        else
                        {
                            if (mSecurityInfo.MayAdd)
                            {
                                MessageUtility.Save(mProfileToSave);
                                mLog.Debug("Added message " + profile.Name + " by " + mCurrentAccountProfile.Account);
                                mRetVal = "true";
                            }
                            else
                            {
                                Exception mError = new Exception("The account (" + AccountUtility.CurrentProfile().Account + ") being used does not have the correct permissions to add");
                                mLog.Error(mError);
                                return this.InternalServerError(mError);
                            }
                        }
                    }
                    else
                    {
                        Exception mError = new Exception("Security Info is not in context nothing has been saved!!!!");
                        mLog.Error(mError);
                        return this.InternalServerError(mError);
                    }
                }
                else
                {
                    Exception mError = new Exception("Identifier you have last looked at does not match the one passed in nothing has been saved!!!!");
                    mLog.Error(mError);
                    return this.InternalServerError(mError);
                }
            } 
            return Ok(mRetVal);
        }

        private MMessageProfile populateProfile(UIMessageProfile uiProfile, MAccountProfile accountProfile)
        {
            MMessageProfile mProfile = new MMessageProfile();
            if (accountProfile == null) accountProfile = new MAccountProfile();
            if( uiProfile.Id > 0){
                mProfile = MessageUtility.GetProfile(uiProfile.Id);
                mProfile.UpdatedBy = accountProfile.Id;
                mProfile.UpdatedDate = DateTime.Now;
            }else{
                mProfile.AddedBy = accountProfile.Id;
                mProfile.AddedDate = DateTime.Now;
            }
            mProfile.Body = HttpContext.Current.Server.UrlDecode(uiProfile.Body);
            mProfile.Description = uiProfile.Description;
            mProfile.FormatAsHtml = uiProfile.FormatAsHtml;
            mProfile.Id = uiProfile.Id;
            mProfile.Name = uiProfile.Name;
            mProfile.SecurityEntitySeqId = SecurityEntityUtility.CurrentProfile().Id;
            mProfile.Title = uiProfile.Title;
            return mProfile;
        }
    }

    public class UIMessageProfile
    {
        public string Body { get; set; }
        public string Description { get; set; }
        public bool FormatAsHtml { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }

    }
}
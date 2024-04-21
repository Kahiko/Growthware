using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
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
    public class GroupsController : ApiController
    {
        [HttpPost()]
        public IHttpActionResult Delete([FromUri] int groupSeqId)
        {
            string mRetVal = "false";
            MSecurityInfo mSecurityInfo = new MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditGroups", true)), AccountUtility.CurrentProfile());
            if (!mSecurityInfo.MayDelete)
            {
                Exception mError = new Exception("The account (" + AccountUtility.CurrentProfile().Account + ") being used does not have the correct permissions to delete");
                Logger mLog = Logger.Instance();
                mLog.Error(mError);
                return this.InternalServerError(mError);
            }
            else
            {
                if (HttpContext.Current.Items["EditId"] != null)
                {
                    int mEditId = int.Parse(HttpContext.Current.Items["EditId"].ToString());
                    if (mEditId == groupSeqId)
                    {
                        MGroupProfile mProfile = GroupUtility.GetProfile(groupSeqId);
                        GroupUtility.Delete(mProfile);
                    }
                    else
                    {
                        Exception mError = new Exception("Identifier you have last looked at does not match the one passed in nothing has been saved!!!!");
                        Logger mLog = Logger.Instance();
                        mLog.Error(mError);
                        return this.InternalServerError(mError);
                    }
                }
                else
                {
                    Exception mError = new Exception("The identifier unknown and nothing has been saved!!!!");
                    Logger mLog = Logger.Instance();
                    mLog.Error(mError);
                    return this.InternalServerError(mError);
                }
            }

            return Ok(mRetVal);
        }

        [HttpPost()]
        public IHttpActionResult Save(MUIGroupProfile profile)
        {
            if (profile == null) throw new ArgumentNullException("uiProfile", "uiProfile cannot be a null reference (Nothing in Visual Basic)!");
            string mRetVal = "false";
            MGroupProfile mProfileToSave = new MGroupProfile();
            MSecurityInfo mSecurityInfo = new MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditGroups", true)), AccountUtility.CurrentProfile());
            Logger mLog = Logger.Instance();
            if (HttpContext.Current.Items["EditId"] != null)
            {
                if (profile.Id == -1)
                {
                    if (!mSecurityInfo.MayAdd)
                    {
                        Exception mError = new Exception("The account (" + AccountUtility.CurrentProfile().Account + ") being used does not have the correct permissions to add");
                        mLog.Error(mError);
                        return this.InternalServerError(mError);
                    }
                    mProfileToSave.AddedBy = AccountUtility.CurrentProfile().Id;
                    mProfileToSave.AddedDate = DateTime.Now;
                }
                else
                {
                    if (!mSecurityInfo.MayAdd)
                    {
                        Exception mError = new Exception("The account (" + AccountUtility.CurrentProfile().Account + ") being used does not have the correct permissions to edit");
                        mLog.Error(mError);
                        return this.InternalServerError(mError);
                    }
                    //mProfileToSave = RoleUtility.GetProfile(profile.Id);

                    mProfileToSave.UpdatedBy = AccountUtility.CurrentProfile().Id;
                    mProfileToSave.UpdatedDate = DateTime.Now;
                }
            }
            mProfileToSave = populateProfile(profile);
            GroupUtility.Save(mProfileToSave);
            return Ok(mRetVal);
        }

        [HttpPost()]
        public IHttpActionResult SaveMembers(UIAccounts groupAccounts)
        {
            string mRetVal = "false";
            Logger mLog = Logger.Instance();
            MSecurityInfo mSecurityInfo = new MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditGroups", true)), AccountUtility.CurrentProfile());
            if (!mSecurityInfo.MayEdit)
            {
                Exception mError = new Exception("The account (" + AccountUtility.CurrentProfile().Account + ") being used does not have the correct permissions to add");
                mLog.Error(mError);
                return this.InternalServerError(mError);
            }
            if (HttpContext.Current.Items["EditId"] == null || HttpContext.Current.Items["EditId"].ToString().ToLowerInvariant() != groupAccounts.SeqId.ToString().ToLowerInvariant())
            {
                Exception mError = new Exception("Identifier you have last looked at does not match the one passed in nothing has been saved!!!!");
                mLog.Error(mError);
                return this.InternalServerError(mError);
            }
            MAccountProfile accountProfile = AccountUtility.CurrentProfile();
            MClientChoicesState mClientChoicesState = ClientChoicesUtility.GetClientChoicesState(accountProfile.Account);
            MGroupRoles mProfile = new MGroupRoles();
            mProfile.SecurityEntityId = SecurityEntityUtility.CurrentProfile().Id;
            mProfile.GroupSeqId = groupAccounts.SeqId;
            mProfile.Roles = String.Join(",", groupAccounts.Accounts);
            mProfile.AddedUpdatedBy = accountProfile.Id;
            GroupUtility.UpdateGroupRoles(mProfile);
            return Ok(mRetVal);
        }

        MGroupProfile populateProfile(MUIGroupProfile profile)
        {
            MGroupProfile mRetVal = new MGroupProfile();
            mRetVal.Name = profile.Name;
            mRetVal.Description = profile.Description;
            mRetVal.Id = profile.Id;
            mRetVal.SecurityEntityId = SecurityEntityUtility.CurrentProfile().Id;
            return mRetVal;
        }
    }

    public class MUIGroupProfile
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}

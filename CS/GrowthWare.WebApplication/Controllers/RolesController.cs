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
    public class RolesController : ApiController
    {

        [HttpPost()]
        public IHttpActionResult Delete([FromUri] int roleSeqId)
        {
            string mRetVal = "false";
            MSecurityInfo mSecurityInfo = new MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditRoles", true)), AccountUtility.CurrentProfile());
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
                    if (mEditId == roleSeqId)
                    {
                        MRoleProfile mProfile = RoleUtility.GetProfile(roleSeqId);
                        RoleUtility.DeleteRole(mProfile);
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
        public IHttpActionResult Save(MUIRoleProfile profile) 
        {
            if (profile == null) throw new ArgumentNullException("uiProfile", "uiProfile cannot be a null reference (Nothing in Visual Basic)!");
            string mRetVal = "false";
            MRoleProfile mProfileToSave = new MRoleProfile();
            MSecurityInfo mSecurityInfo = new MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditRoles", true)), AccountUtility.CurrentProfile());
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
                    if (profile.IsSystem)
                    {
                        mProfileToSave.IsSystem = true;
                    }
                    if (profile.IsSystemOnly)
                    {
                        mProfileToSave.IsSystemOnly = true;
                    }
                    mProfileToSave = RoleUtility.GetProfile(profile.Id);

                    mProfileToSave.UpdatedBy = AccountUtility.CurrentProfile().Id;
                    mProfileToSave.UpdatedDate = DateTime.Now;
                }
            }
            mProfileToSave = populateProfile(profile);
            RoleUtility.Save(mProfileToSave);
            return Ok(mRetVal);
        }

        [HttpPost()]
        public IHttpActionResult SaveMembers(UIAccounts roleAccounts) 
        {
            string mRetVal = "false";
            Logger mLog = Logger.Instance();
            MSecurityInfo mSecurityInfo = new MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditRoles", true)), AccountUtility.CurrentProfile());
            if (!mSecurityInfo.MayEdit) 
            {
                Exception mError = new Exception("The account (" + AccountUtility.CurrentProfile().Account + ") being used does not have the correct permissions to add");
                mLog.Error(mError);
                return this.InternalServerError(mError);            
            }
            if (HttpContext.Current.Items["EditId"] == null)
            {
                Exception mError = new Exception("Identifier you have last looked at does not match the one passed in nothing has been saved!!!!");
                mLog.Error(mError);
                return this.InternalServerError(mError);
            }
            MAccountProfile accountProfile = AccountUtility.CurrentProfile();
            MClientChoicesState mClientChoicesState = ClientChoicesUtility.GetClientChoicesState(accountProfile.Account);
            bool success = RoleUtility.UpdateAllAccountsForRole(roleAccounts.SeqId, int.Parse(mClientChoicesState[MClientChoices.SecurityEntityId]), roleAccounts.Accounts, accountProfile.Id);
            return Ok(mRetVal);
        }

        MRoleProfile populateProfile(MUIRoleProfile profile) 
        { 
            MRoleProfile mRetVal = new MRoleProfile();
            mRetVal.Name = profile.Name;
            mRetVal.Description = profile.Description;
            mRetVal.Id = profile.Id;
            mRetVal.IsSystem = profile.IsSystem;
            mRetVal.IsSystemOnly = profile.IsSystemOnly;
            mRetVal.SecurityEntityId = SecurityEntityUtility.CurrentProfile().Id;
            return mRetVal;
        }
    }

    public class MUIRoleProfile
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsSystem { get; set; }

        public bool IsSystemOnly { get; set; }
    }

    public class UIAccounts
    {
        public int SeqId;
        public string[] Accounts;
    }
}

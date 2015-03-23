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
        public IHttpActionResult Save(MRoleProfile profile) 
        {
            if (profile == null) throw new ArgumentNullException("uiProfile", "uiProfile cannot be a null reference (Nothing in Visual Basic)!");
            string mRetVal = "false";
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
                    profile.AddedBy = AccountUtility.CurrentProfile().Id;
                    profile.AddedDate = DateTime.Now;
                }
                else 
                {
                    if (!mSecurityInfo.MayAdd)
                    {
                        Exception mError = new Exception("The account (" + AccountUtility.CurrentProfile().Account + ") being used does not have the correct permissions to edit");
                        mLog.Error(mError);
                        return this.InternalServerError(mError);
                    }
                    profile.UpdatedBy = AccountUtility.CurrentProfile().Id;
                    profile.UpdatedDate = DateTime.Now;
                }
            }
            if (profile.IsSystem)
            {
                profile.IsSystem = true;
            }
            if (profile.IsSystemOnly)
            {
                profile.IsSystemOnly = true;
            }
            RoleUtility.Save(profile);
            return Ok(mRetVal);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Net.Http.Formatting;
using GrowthWare.WebSupport.Utilities;
using System.Web.SessionState;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Enumerations;

namespace GrowthWare.WebApplication.Controllers
{
    public class SecurityEntitiesController : ApiController
    {
        [HttpPost()]
        public IHttpActionResult Save(MUISecurityEntityProfile uiProfile) 
        { 
            if(uiProfile == null) new ArgumentNullException("uiProfile", "uiProfile cannot be a null reference (Nothing in Visual Basic)!");
            bool mRetVal = false;
            Logger mLog = Logger.Instance();
            var session = SessionStateUtility.GetHttpSessionStateFromContext(HttpContext.Current);
            MSecurityInfo mSecurityInfo = new MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditSecurityEntity", true)), AccountUtility.CurrentProfile());
            if (mSecurityInfo != null)
            {
                if (HttpContext.Current.Session["EditId"] != null)
                {
                    int mEditId = int.Parse(HttpContext.Current.Items["EditId"].ToString());
                    if (mEditId == int.Parse(uiProfile.Id)) 
                    {
                        if (mEditId != -1)
                        {
                            if (mSecurityInfo.MayEdit)
                            {
                                MSecurityEntityProfile mSecurityEntityProfile = SecurityEntityUtility.GetProfile(mEditId);
                                mSecurityEntityProfile = populateSecurityEntity(uiProfile);
                                mSecurityEntityProfile.Id = int.Parse(uiProfile.Id);
                                SecurityEntityUtility.Save(mSecurityEntityProfile);
                                mRetVal = true;
                            }
                            else
                            {
                                Exception mError = new Exception("The account (" + AccountUtility.CurrentProfile().Account + ") being used does not have the correct permissions to add");
                                mLog.Error(mError);
                                return this.InternalServerError(mError);
                            }
                        }
                        else
                        {
                            if (mSecurityInfo.MayAdd)
                            {
                                MSecurityEntityProfile mSecurityEntityProfile = populateSecurityEntity(uiProfile);
                                mSecurityEntityProfile.Id = -1;
                                mSecurityEntityProfile.AddedBy = AccountUtility.CurrentProfile().Id;
                                mSecurityEntityProfile.AddedDate = DateTime.Now;
                                mSecurityEntityProfile.UpdatedBy = mSecurityEntityProfile.AddedBy;
                                mSecurityEntityProfile.UpdatedDate = mSecurityEntityProfile.AddedDate;
                                SecurityEntityUtility.Save(mSecurityEntityProfile);
                                mRetVal = true;
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
                        Exception mError = new Exception("Identifier you have last looked at does not match the one passed in nothing has been saved!!!!");
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
            return this.Ok(mRetVal);
        }

        private MSecurityEntityProfile populateSecurityEntity(MUISecurityEntityProfile uiProfile) 
        {
            MSecurityEntityProfile mSecurityEntityProfile = new MSecurityEntityProfile();
            mSecurityEntityProfile.UpdatedBy = AccountUtility.CurrentProfile().Id;
            mSecurityEntityProfile.UpdatedDate = DateTime.Now;
            mSecurityEntityProfile.ConnectionString = uiProfile.ConnectionString;
            mSecurityEntityProfile.DataAccessLayer = uiProfile.DAL;
            mSecurityEntityProfile.DataAccessLayerAssemblyName = uiProfile.DALAssemblyName;
            mSecurityEntityProfile.DataAccessLayerNamespace = uiProfile.DALNamespace;
            mSecurityEntityProfile.Description = uiProfile.Description;
            mSecurityEntityProfile.EncryptionType = (EncryptionType)uiProfile.EncryptionType;
            mSecurityEntityProfile.Name = uiProfile.Name;
            mSecurityEntityProfile.ParentSeqId = uiProfile.ParentSeqId;
            mSecurityEntityProfile.Skin = uiProfile.Skin;
            mSecurityEntityProfile.StatusSeqId = uiProfile.StatusSeqId;
            mSecurityEntityProfile.Style = uiProfile.Style;
            mSecurityEntityProfile.Url = uiProfile.Url;
            return mSecurityEntityProfile;
        }
    }

    public class MUISecurityEntityProfile
    {
        public string ConnectionString { get; set; }
        public string DAL { get; set; }
        public string DALAssemblyName { get; set; }
        public string DALNamespace { get; set; }
        public string Description { get; set; }
        public int EncryptionType { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public int ParentSeqId { get; set; }
        public string Skin { get; set; }
        public int StatusSeqId { get; set; }
        public string Style { get; set; }
        public string Url { get; set; }

    }
}
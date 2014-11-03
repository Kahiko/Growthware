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

namespace GrowthWare.WebApplication.Controllers
{
    public class SecurityEntitiesController : ApiController
    {
        [HttpPost()]
        public IHttpActionResult Save(MUISecurityEntityProfile uiProfile) 
        { 
            if(uiProfile == null) new ArgumentNullException("uiProfile", "uiProfile cannot be a null reference (Nothing in Visual Basic)!");
            bool mRetVal = false;
            var session = SessionStateUtility.GetHttpSessionStateFromContext(HttpContext.Current);
            if (HttpContext.Current.Session["EditId"] != null) 
            {
                if (!string.IsNullOrEmpty(uiProfile.Name))
                {
                    mRetVal = true;
                    //SecurityEntityUtility.Save(uiProfile);

                }
                else 
                {
                    ArgumentNullException mError = new ArgumentNullException("uiProfile", "uiProfile.Name  cannot be a null reference (Nothing in Visual Basic)!");
                    return this.InternalServerError(mError);
                }

            }
            return this.Ok(mRetVal);
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
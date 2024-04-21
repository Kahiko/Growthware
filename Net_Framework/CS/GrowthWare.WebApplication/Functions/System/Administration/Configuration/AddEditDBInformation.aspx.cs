using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport.Base;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GrowthWare.WebApplication.Functions.System.Administration.Configuration
{
    public partial class AddEditDBInformation : BaseWebpage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MDBInformation mProfile = DBInformationUtility.DBInformation();
            lblVersion.Text = mProfile.Version;
            NameValuePairUtility.SetDropSelection(dropEnableInheritance, mProfile.EnableInheritance.ToString());
        }

        [WebMethod(EnableSession = false)]
        public static void InvokeSave(int enableInheritance)
        {
            MDBInformation mProfile = DBInformationUtility.DBInformation();
            mProfile.EnableInheritance = enableInheritance;
            if (DBInformationUtility.UpdateProfile(mProfile)) FunctionUtility.RemoveCachedFunctions();
        }
    }
}
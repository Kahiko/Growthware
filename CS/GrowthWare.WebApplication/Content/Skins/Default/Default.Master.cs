using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GrowthWare.WebApplication.Content.Skins.Default
{
    public partial class Default : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int mSecurityEntityID = int.Parse(ClientChoicesUtility.GetClientChoicesState(AccountUtility.CurrentProfile().Account)[MClientChoices.SecurityEntityId].ToString());
            MSecurityEntityProfile mSecurityEntityProfile = SecurityEntityUtility.GetProfile(mSecurityEntityID);
            formStyles.Attributes["href"] = GWWebHelper.RootSite + "/Content/FormStyles/" + mSecurityEntityProfile.Style + ".css";

        }
    }
}
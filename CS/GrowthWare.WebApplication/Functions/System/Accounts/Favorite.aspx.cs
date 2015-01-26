using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport.Base;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GrowthWare.WebApplication.Functions.System.Accounts
{
    public partial class Favorite : ClientChoicesPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String mAction = ClientChoicesState[MClientChoices.Action];
            MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(mAction);
            MAccountProfile mAccountProfile = AccountUtility.CurrentProfile();
            MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mAccountProfile);
            if (mSecurityInfo.MayView)
            {
                String mScript = "<script type='text/javascript' language='javascript'>window.location.hash = '?Action=" + mAction + "'</script>";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", mScript);
            }
            else
            {
                Response.Write("Your favorite is not available.  Please ensure that you have chosen the correct " + ConfigSettings.SecurityEntityTranslation);
            }
        }
    }
}
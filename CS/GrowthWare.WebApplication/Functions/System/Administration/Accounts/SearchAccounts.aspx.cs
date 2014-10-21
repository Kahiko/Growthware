using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GrowthWare.WebApplication.Functions.System.Administration.Accounts
{
    public partial class SearchAccounts : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string mAction = GWWebHelper.GetQueryValue(Request, "action");
            if (!String.IsNullOrEmpty(mAction))
            {
                MAccountProfile mAccountProfile = AccountUtility.CurrentProfile();
                MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(mAction);
                //MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, mAccountProfile);
                //SearchControl.ShowAddLink = mSecurityInfo.MayAdd;
            }
        }
    }
}
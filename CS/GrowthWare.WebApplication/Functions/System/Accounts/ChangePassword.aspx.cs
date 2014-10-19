using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GrowthWare.WebApplication.Functions.System.Accounts
{
    public partial class ChangePassword : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MAccountProfile mAccountProfile = AccountUtility.CurrentProfile();
            bool flag = mAccountProfile.Status == 4;
            if (flag)
            {
                this.trForceChange.Visible = true;
                this.trNormalChange.Visible = false;
                this.trOldPassword.Visible = false;
                this.NewPassword.Focus();
            }
            else
            {
                this.trForceChange.Visible = false;
                this.trNormalChange.Visible = true;
                this.trOldPassword.Visible = true;
                this.OldPassword.Focus();
            }
        }
    }
}
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport;
using GrowthWare.WebSupport.Base;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GrowthWare.WebApplication.Functions.System.Administration.Roles
{
    public partial class EditRoleMembers : ClientChoicesPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MRoleProfile myProfile = new MRoleProfile();
            MSecurityInfo accountSecurityInfo = new MSecurityInfo(FunctionUtility.CurrentProfile(), AccountUtility.CurrentProfile());
            txtEditID.Text = Request.QueryString[GWWebHelper.RoleDataKeyField].ToString();
            //HttpContext.Current.Session.Remove(AppConstants.ROLE_DATA_KEY_FIELD);
            myProfile.Id = int.Parse(txtEditID.Text);
            myProfile = RoleUtility.GetProfile(myProfile.Id);
            litRole.Text = myProfile.Name;
            myProfile.SecurityEntityId = int.Parse(ClientChoicesState[MClientChoices.SecurityEntityId].ToString());
            ctlMembers.DataSource = RoleUtility.GetAccountsNotInRole(myProfile).ToArray(Type.GetType("System.String"));
            ctlMembers.SelectedItems = (string[])RoleUtility.GetAccountsInRole(myProfile).ToArray(Type.GetType("System.String"));
            ctlMembers.DataBind();
        }
    }
}
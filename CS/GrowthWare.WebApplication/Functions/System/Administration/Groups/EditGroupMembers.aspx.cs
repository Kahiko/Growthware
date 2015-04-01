using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport.Base;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GrowthWare.WebApplication.Functions.System.Administration.Groups
{
    public partial class EditGroupMembers : ClientChoicesPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MAccountProfile updatingAccount = null;
            MGroupProfile mGroupProfile = GroupUtility.GetProfile(int.Parse(HttpContext.Current.Request["GroupSeqId"]));
            txtEditID.Text = mGroupProfile.Id.ToString();
            litGroup.Text = mGroupProfile.Name;
            HttpContext.Current.Session.Add("EditId", mGroupProfile.Id);
            updatingAccount = AccountUtility.CurrentProfile();

            DataView myDataView = RoleUtility.GetAllRolesBySecurityEntity(int.Parse(ClientChoicesState[MClientChoices.SecurityEntityId].ToString())).DefaultView;
            if (!updatingAccount.IsSystemAdmin)
            {
                String rowFilter = "IS_SYSTEM <> 1 AND IS_SYSTEM_ONLY <> 1";
                myDataView.RowFilter = rowFilter;
            }
            try
            {
                MGroupProfile myGroupProfile = new MGroupProfile();
                myGroupProfile = GroupUtility.GetProfile(int.Parse(txtEditID.Text.ToString()));
                litGroup.Text = myGroupProfile.Name;
                MGroupRoles mProfile = new MGroupRoles();
                mProfile.SecurityEntityId = int.Parse(ClientChoicesState[MClientChoices.SecurityEntityId].ToString());
                mProfile.GroupSeqId = int.Parse(txtEditID.Text.ToString());
                ctlMembers.SelectedItems = GroupUtility.GetSelectedRoles(mProfile);
            }
            catch (Exception ex)
            {
                Logger log = Logger.Instance();
                log.Debug(ex);
            }
            ctlMembers.DataSource = myDataView;
            ctlMembers.DataField = "Name";
            ctlMembers.DataBind();
        }
    }
}
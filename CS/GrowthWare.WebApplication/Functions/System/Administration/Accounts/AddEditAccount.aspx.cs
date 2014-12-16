using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport;
using GrowthWare.WebSupport.BasePages;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace GrowthWare.WebApplication.Functions.System.Administration.Accounts
{
    /// <summary>
    /// Class SearchAccountResults
    /// </summary>
    public partial class AddEditAccount : BaseWebpage
    {
        MAccountProfile m_Profile = null;

        string m_Action = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSave.Visible = false;
            m_Action = GWWebHelper.GetQueryValue(Request, "Action");
            if (!String.IsNullOrEmpty(Request.QueryString["AccountSeqID"]))
            {
                int mAccountSeqID = int.Parse(Request.QueryString["AccountSeqID"].ToString());
                if (mAccountSeqID != -1)
                {
                    m_Profile = AccountUtility.GetProfile(mAccountSeqID);
                }
                else
                {
                    m_Profile = new MAccountProfile();
                }
            }
            else
            {
                m_Profile = AccountUtility.CurrentProfile();
                btnSave.Visible = true;
                hdnCanSaveRoles.Value = false.ToString();
                hdnCanSaveGroups.Value = false.ToString();
                hdnCanSaveStatus.Value = false.ToString();
                tdStatus.Style.Add("display", "none");
                dropStatus.Style.Add("display", "none");
                if (m_Action.ToUpper(CultureInfo.InvariantCulture) == "REGISTER") 
                { 
                    m_Profile = new MAccountProfile();
                    trAccount.Visible = false;
                    tabsDerivedRoles.Visible = false;
                    derivedRolesTab.Visible = false;
                }
            }
            HttpContext.Current.Session.Add("EditId", m_Profile.Id);
            populatePage();
        }

        private void populatePage()
        {
            populateGeneral();
            populateRoles();
            populateGroups();
            if (String.IsNullOrEmpty(hdnCanSaveStatus.Value.ToString()))
            {
                hdnCanSaveStatus.Value = true.ToString();
            }

            MSecurityInfo mRoleTabSecurity = new MSecurityInfo(FunctionUtility.GetProfile("ViewAccountRoleTab"), AccountUtility.CurrentProfile());
            MSecurityInfo mGroupTabSecurity = new MSecurityInfo(FunctionUtility.GetProfile("ViewAccountGroupTab"), AccountUtility.CurrentProfile());
            if (String.IsNullOrEmpty(hdnCanSaveRoles.Value.ToString()))
            {
                hdnCanSaveRoles.Value = mRoleTabSecurity.MayView.ToString();
                tabsRoles.Visible = mRoleTabSecurity.MayView;
                rolesTab.Visible = mRoleTabSecurity.MayView;
            }
            else
            {
                hdnCanSaveRoles.Value = "False";
                tabsRoles.Visible = false;
                rolesTab.Visible = false;
            }
            if (String.IsNullOrEmpty(hdnCanSaveGroups.Value.ToString()))
            {
                hdnCanSaveGroups.Value = mGroupTabSecurity.MayView.ToString();
                tabsGroups.Visible = mGroupTabSecurity.MayView;
                groupsTab.Visible = mGroupTabSecurity.MayView;
            }
            else
            {
                hdnCanSaveGroups.Value = "False";
                tabsGroups.Visible = false;
                groupsTab.Visible = false;
            }
            trSysAdmin.Visible = AccountUtility.CurrentProfile().IsSystemAdmin;
        }

        private void populateGeneral()
        {
            txtAccount_seq_id.Value = m_Profile.Id.ToString();
            txtAccount_seq_id.Style.Add("display", "none");
            txtAccount.Text = m_Profile.Account;
            chkSysAdmin.Checked = m_Profile.IsSystemAdmin;
            litFailedAttempts.Text = m_Profile.FailedAttempts.ToString();
            txtFailedAttempts.Text = m_Profile.FailedAttempts.ToString();
            txtFirstName.Text = m_Profile.FirstName;
            txtLastName.Text = m_Profile.LastName;
            txtMiddleName.Text = m_Profile.MiddleName;
            txtPreferredName.Text = m_Profile.PreferredName;
            txtEmail.Text = m_Profile.Email;
            txtLocation.Text = m_Profile.Location;
            chkEnableNotifications.Checked = m_Profile.EnableNotifications;
            NameValuePairUtility.SetDropSelection(dropStatus, m_Profile.Status.ToString());
            NameValuePairUtility.SetDropSelection(dropTimezone, m_Profile.TimeZone.ToString());
        }

        private void populateRoles()
        {
            ctlRoles.DataSource = RoleUtility.GetRolesArrayListBySecurityEntity(SecurityEntityUtility.CurrentProfile().Id);
            ctlRoles.SelectedItems = m_Profile.AssignedRoles.ToArray();
            ctlRoles.DataBind();
            foreach (String role in m_Profile.DerivedRoles)
            {
                lstBoxRoles.Items.Add(role);
            }
        }

        private void populateGroups()
        {
            ctlGroups.DataSource = GroupUtility.GetGroupsArrayListBySecurityEntity(SecurityEntityUtility.CurrentProfile().Id);
            ctlGroups.SelectedItems = m_Profile.Groups.ToArray();
            ctlGroups.DataBind();
        }
    }
}
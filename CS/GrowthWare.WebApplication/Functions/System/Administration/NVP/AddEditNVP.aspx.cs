using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport.Base;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Data;
using System.Web;
using System.Web.UI;

namespace GrowthWare.WebApplication.Functions.System.Administration.NVP
{
    public partial class AddEditNVP : ClientChoicesPage
    {
        protected MNameValuePair m_NVPToUpdate = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["NVP_SEQ_ID"]))
            {
                int mNameValuePairSeqID = int.Parse((HttpContext.Current.Request.QueryString["NVP_SEQ_ID"]).ToString());
                if (mNameValuePairSeqID != -1)
                {
                    m_NVPToUpdate = NameValuePairUtility.GetNameValuePair(mNameValuePairSeqID);
                }
                else
                {
                    m_NVPToUpdate = new MNameValuePair();
                }
                HttpContext.Current.Session.Add("EditId", m_NVPToUpdate.Id);
            }
            else
            {
                tdStatus.Style.Add("display", "none");
                dropStatus.Style.Add("display", "none");
            }
            populatePage(m_NVPToUpdate);
        }

        private void populatePage(MNameValuePair profile)
        {
            populateGeneral(profile);
            populateRolesAndGroups(profile);
        }

        private void populateGeneral(MNameValuePair profile)
        {
            txtNVP_SEQ_ID.Text = profile.Id.ToString();
            txtSchemaName.Text = profile.SchemaName;
            litSchemaName.Text = profile.SchemaName;
            txtSTATIC_NAME.Text = profile.StaticName;
            litStaticName.Text = profile.StaticName;
            txtDisplay.Text = profile.Display;
            txtDescription.Text = profile.Description;
            if (!String.IsNullOrEmpty(profile.SchemaName))
            {
                lblTableName.InnerHtml = profile.SchemaName + "." + profile.StaticName;
            }
            if (profile.Id > -1)
            {
                txtSchemaName.Style.Add("display", "none");
                RequiredFieldValidator1.Enabled = false;
                Alphanumeric.Enabled = false;
                txtSTATIC_NAME.Style.Add("display", "none");
                RequiredFieldValidator2.Enabled = false;
                Alphanumeric2.Enabled = false;
                litSchemaName.Visible = true;
                litStaticName.Visible = true;
            }
            else
            {
                txtSchemaName.Style.Add("display", "");
                txtSTATIC_NAME.Style.Add("display", "");
                litSchemaName.Visible = false;
                litStaticName.Visible = false;
            }
        }

        private void populateRolesAndGroups(MNameValuePair profile)
        {
            int mSecurityEntityId = int.Parse(ClientChoicesState[MClientChoices.SecurityEntityId].ToString());
            DataView mDVRoles = RoleUtility.GetAllRolesBySecurityEntity(mSecurityEntityId).DefaultView;
            DataView mDVGroups = GroupUtility.GetAllGroupsBySecurityEntity(mSecurityEntityId).DefaultView;
            try
            {
                ctlGroups.SelectedItems = NameValuePairUtility.GetSelectedGroups(m_NVPToUpdate.Id);
                ctlRoles.SelectedItems = NameValuePairUtility.GetSelectedRoles(m_NVPToUpdate.Id);
            }
            catch (Exception ex)
            {
                Logger mLog = Logger.Instance();
                mLog.Debug(ex);
            }
            ctlGroups.DataSource = mDVGroups;
            ctlGroups.DataField = "Name";
            ctlGroups.DataBind();
            ctlRoles.DataSource = mDVRoles;
            ctlRoles.DataField = "Name";
            ctlRoles.DataBind();
        }

    }
}
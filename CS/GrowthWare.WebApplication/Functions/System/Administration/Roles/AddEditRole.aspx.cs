using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport;
using GrowthWare.WebSupport.Base;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Web;

namespace GrowthWare.WebApplication.Functions.System.Administration.Roles
{
    public partial class AddEditRole : BaseWebpage
    {
        protected MRoleProfile m_Profile = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            string mRoleSeqId = GWWebHelper.GetQueryValue(Request, "RoleSeqID");
            if (!String.IsNullOrEmpty(mRoleSeqId))
            {
                int mRoleID = int.Parse(mRoleSeqId);
                if (mRoleID != -1)
                {
                    m_Profile = RoleUtility.GetProfile(mRoleID);
                }
                else
                {
                    m_Profile = new MRoleProfile();
                }
                HttpContext.Current.Session.Add("EditId", m_Profile.Id);
                populatePage();
            }
        }

        private void populatePage()
        {
            txtRoleSeqId.Value = m_Profile.Id.ToString();
            txtRole.Text = m_Profile.Name;
            txtDescription.Text = m_Profile.Description;
            chkIsSystem.Checked = m_Profile.IsSystem;
            chkIsSystemOnly.Checked = m_Profile.IsSystemOnly;

            if (m_Profile.IsSystem || m_Profile.IsSystemOnly)
            {
                txtRole.Enabled = false;
                chkIsSystem.Enabled = false;
                chkIsSystemOnly.Enabled = false;
            }
        }
    }
}
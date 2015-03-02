using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport;
using GrowthWare.WebSupport.Base;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Data;
using System.Linq;
using System.Web;

namespace GrowthWare.WebApplication.Functions.System.Administration.Functions
{
    public partial class AddEditFunction : BaseWebpage
    {
        protected MFunctionProfile m_Profile = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["FunctionSeqID"]))
            {
                int mFunctionSeqID = int.Parse((HttpContext.Current.Request.QueryString["FunctionSeqID"]).ToString());
                if (mFunctionSeqID != -1)
                {
                    m_Profile = FunctionUtility.GetProfile(mFunctionSeqID);
                }
                else
                {
                    m_Profile = new MFunctionProfile();
                }
                HttpContext.Current.Session.Add("EditId", m_Profile.Id);
                populatePage();
            }
        }

        private void populatePage()
        {
            if (m_Profile.Id == -1)
            {
                divAction.Visible = false;
                txtAction.Visible = true;
            }
            else
            {
                divAction.Visible = true;
                txtAction.Visible = false;
            }
            populateGeneral();
            populateFucntionDrop();
            populateFunctionTypes();
            populateNavigationTypes();
            populateMenuOrder();
            populateDerivedRoles();
            populateLinkBehaviors();
            populateParent();
            populateDirectoryInformation();
        }

        private void populateGeneral()
        {
            //litFunctionSeqId.Text = m_Profile.Id.ToString();
            divFunctionSeqId.InnerHtml = m_Profile.Id.ToString();
            txtName.Value = m_Profile.Name;
            txtDescription.Value = m_Profile.Description;
            txtNotes.Text = m_Profile.Notes;
            txtKeyWords.Text = m_Profile.MetaKeywords;

            divAction.InnerHtml = m_Profile.Action;
            divAction.Visible = true;
            txtAction.Text = m_Profile.Action;
            //txtAction.Visible = false;

            txtSource.Text = m_Profile.Source;

            chkEnableViewState.Checked = m_Profile.EnableViewState;
            chkEnableNotifications.Checked = m_Profile.EnableNotifications;
            chkRedirectOnTimeout.Checked = m_Profile.RedirectOnTimeout;
            chkNoUI.Checked = m_Profile.NoUI;
            chkIsNav.Checked = m_Profile.IsNavigable;

            RolesControl.AllRoles = RoleUtility.GetRolesArrayListBySecurityEntity(SecurityEntityUtility.CurrentProfile().Id);
            RolesControl.SelectedAddRoles = m_Profile.AssignedAddRoles.ToArray();
            RolesControl.SelectedDeleteRoles = m_Profile.AssignedDeleteRoles.ToArray();
            RolesControl.SelectedEditRoles = m_Profile.AssignedEditRoles.ToArray();
            RolesControl.SelectedViewRoles = m_Profile.AssignedViewRoles.ToArray();

            GroupsControl.AllGroups = GroupUtility.GetGroupsArrayListBySecurityEntity(SecurityEntityUtility.CurrentProfile().Id);
            GroupsControl.SelectedAddGroups = m_Profile.AddGroups.ToArray();
            GroupsControl.SelectedDeleteGroups = m_Profile.DeleteGroups.ToArray();
            GroupsControl.SelectedEditGroups = m_Profile.EditGroups.ToArray();
            GroupsControl.SelectedViewGroups = m_Profile.ViewGroups.ToArray();
        }

        private void populateFucntionDrop()
        {
            if (m_Profile.Id > 0) 
            {
                DataView mDataView = FunctionUtility.GetFunctionMenuOrder(m_Profile).DefaultView;
                if (mDataView.Count > 0)
                {
                    mDataView.Sort = "[Name] ASC";
                    dropFunctions.DataSource = mDataView;
                    dropFunctions.DataValueField = "FUNCTION_SEQ_ID";
                    dropFunctions.DataTextField = "NAME";
                    dropFunctions.DataBind();
                    if (m_Profile.Id != -1)
                    {
                        NameValuePairUtility.SetDropSelection(dropFunctions, m_Profile.Id.ToString());
                    }
                }
            }
        }

        private void populateFunctionTypes()
        {
            dropFunctionType.DataSource = FunctionTypeUtility.FunctionTypes();
            dropFunctionType.DataTextField = "NAME";
            dropFunctionType.DataValueField = "FUNCTION_TYPE_SEQ_ID";
            dropFunctionType.DataBind();
            if (m_Profile.Id != -1)
            {
                NameValuePairUtility.SetDropSelection(dropFunctionType, m_Profile.FunctionTypeSeqId.ToString());
            }
        }

        private void populateMenuOrder()
        {
            //DataView myDataView = FunctionUtility.GetFunctionMenuOrder(ref m_Profile).DefaultView;
            //dgFunctionOrder.DataSource = myDataView;
            //dgFunctionOrder.DataKeyField = GWWebHelper.FUNCTION_DATA_KEY_FIELD;
            //dgFunctionOrder.DataBind();
        }

        private void populateNavigationTypes()
        {
            DataTable mDataTable = new DataTable();
            int mNavType = GWWebHelper.LinkBehaviorNavigationTypesSequenceId;
            NameValuePairUtility.GetNameValuePairDetails(ref mDataTable, mNavType);
            dropNavType.DataSource = mDataTable;
            dropNavType.DataTextField = "NVP_DET_TEXT";
            dropNavType.DataValueField = "NVP_SEQ_DET_ID";
            dropNavType.DataBind();
            if (m_Profile.Id != -1)
            {
                NameValuePairUtility.SetDropSelection(dropNavType, m_Profile.NavigationTypeSeqId.ToString());
            }
        }

        private void populateLinkBehaviors()
        {
            DataTable mDataTable = new DataTable();
            int mNavType = GWWebHelper.LinkBehaviorNameValuePairSequenceId;
            NameValuePairUtility.GetNameValuePairDetails(ref mDataTable, mNavType);
            dropLinkBehavior.DataSource = mDataTable;
            dropLinkBehavior.DataTextField = "NVP_DET_TEXT";
            dropLinkBehavior.DataValueField = "NVP_SEQ_DET_ID";
            dropLinkBehavior.DataBind();
            if (m_Profile.Id != -1)
            {
                NameValuePairUtility.SetDropSelection(dropLinkBehavior, m_Profile.LinkBehavior.ToString());
            }
        }

        private void populateDerivedRoles()
        {
            foreach (string item in m_Profile.DerivedAddRoles)
            {
                lstBoxAddRoles.Items.Add(item);
            }
            foreach (string item in m_Profile.DerivedDeleteRoles)
            {
                lstBoxDeleteRoles.Items.Add(item);
            }
            foreach (string item in m_Profile.DerivedEditRoles)
            {
                lstBoxEditRoles.Items.Add(item);
            }
            foreach (string item in m_Profile.DerivedViewRoles)
            {
                lstBoxViewRoles.Items.Add(item);
            }
        }

        private void populateParent()
        {
            //dvFunctions.RowFilter = "PARENT_FUNCTION_SEQ_ID <> " + CurrentProfile.ID;
            var mResult = from mProfile in FunctionUtility.Functions()
                          where mProfile.ParentId != m_Profile.Id
                          select mProfile;

            dropNavParent.DataValueField = "Id";
            dropNavParent.DataTextField = "Name";
            dropNavParent.DataSource = mResult;
            dropNavParent.DataBind();
            if (m_Profile.Id != -1)
            {
                NameValuePairUtility.SetDropSelection(dropNavParent, m_Profile.ParentId.ToString());
            }
        }

        private void populateDirectoryInformation()
        {
            MDirectoryProfile mProfile = DirectoryUtility.GetProfile(m_Profile.Id);
            if (mProfile == null)
            {
                mProfile = new MDirectoryProfile();
            }
            txtDirectory.Text = mProfile.Directory;
            chkImpersonation.Checked = mProfile.Impersonate;
            txtAccount.Text = mProfile.ImpersonateAccount;
            txtPassword.Text = mProfile.ImpersonatePassword;
            txtHidPwd.Text = mProfile.ImpersonatePassword;
        }
    }
}
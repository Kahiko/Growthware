using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport;
using GrowthWare.WebSupport.Base;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace GrowthWare.WebApplication.Functions.System.Administration.SecurityEntities
{
    public partial class AddEditSecurityEntities : BaseWebpage
    {
        protected MSecurityEntityProfile m_Profile = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["SecurityEntitySeqId"]))
            {
                int mAccountSeqID = int.Parse(Request.QueryString["SecurityEntitySeqId"]);
                if (mAccountSeqID != -1)
                {
                    m_Profile = SecurityEntityUtility.GetProfile(mAccountSeqID);
                }
                else
                {
                    m_Profile = new MSecurityEntityProfile();
                    m_Profile.DataAccessLayerAssemblyName = "GrowthWareFramework";
                    m_Profile.DataAccessLayerNamespace = "GrowthWare.Framework.DataAccessLayer.SQLServer.V2008";
                }
            }
            HttpContext.Current.Session.Add("EditId", m_Profile.Id);
            populatePage();
        }

        private void populatePage()
        {
            litSecurityEntity.Text = m_Profile.Name;
            txtSecurityEntity.Text = m_Profile.Name;
            if (m_Profile.Id == -1)
            {
                litSecurityEntity.Visible = false;
                txtSecurityEntity.Style.Add("display", "");
            }
            else
            {
                litSecurityEntity.Visible = true;
                txtSecurityEntity.Style.Add("display", "none");
            }
            txtSeqID.Text = m_Profile.Id.ToString();
            txtDescription.Text = m_Profile.Description;
            txtURL.Text = m_Profile.Url;
            try
            {
                txtConnectionstring.Text = CryptoUtility.Decrypt(m_Profile.ConnectionString, SecurityEntityUtility.CurrentProfile().EncryptionType);
            }
            catch (Exception)
            {
                txtConnectionstring.Text = m_Profile.ConnectionString;
            }
            litSecurityEntityTranslation.Text = ConfigSettings.SecurityEntityTranslation;
            txtAssembly_Name.Text = m_Profile.DataAccessLayerAssemblyName;
            txtName_Space.Text = m_Profile.DataAccessLayerNamespace;
            MDirectoryProfile myDirectoryInfo = new MDirectoryProfile();
            DataView dvSkin = new DataView();
            dvSkin = FileUtility.GetDirectoryTableData(GWWebHelper.SkinPath, myDirectoryInfo, false).DefaultView;
            dvSkin.RowFilter = "Type = 'folder'";
            dropSkin.DataSource = dvSkin;
            dropSkin.DataTextField = "Name";
            dropSkin.DataValueField = "Name";
            dropSkin.DataBind();

            DataView dvStyles = new DataView();
            dvStyles = FileUtility.GetDirectoryTableData(Server.MapPath(@"~\Content\FormStyles"), myDirectoryInfo, true).DefaultView;
            dvStyles.RowFilter = "[Name] like '%.css'";
            dropStyles.DataSource = dvStyles;
            dropStyles.DataTextField = "ShortFileName";
            dropStyles.DataValueField = "ShortFileName";
            dropStyles.DataBind();
            Collection<MSecurityEntityProfile> mProfiles = SecurityEntityUtility.Profiles();
            dropParent.DataSource = mProfiles;
            MSecurityEntityProfile mm = new MSecurityEntityProfile();
            dropParent.DataTextField = "Name";
            dropParent.DataValueField = "Id";
            dropParent.DataBind();
            ListItem lstItem = new ListItem();
            lstItem.Text = "None";
            lstItem.Value = "-1";
            dropParent.Items.Add(lstItem);
            NameValuePairUtility.SetDropSelection(dropParent, m_Profile.ParentSeqId.ToString());
            NameValuePairUtility.SetDropSelection(dropSkin, m_Profile.Skin);
            NameValuePairUtility.SetDropSelection(dropStyles, m_Profile.Style);
            NameValuePairUtility.SetDropSelection(dropStatus, m_Profile.StatusSeqId.ToString());
            NameValuePairUtility.SetDropSelection(dropDAL, m_Profile.DataAccessLayer);
            NameValuePairUtility.SetDropSelection(dropEncryptionType, m_Profile.EncryptionType.ToString());
        }
    }
}
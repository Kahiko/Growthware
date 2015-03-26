using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport;
using GrowthWare.WebSupport.Base;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Web;

namespace GrowthWare.WebApplication.Functions.System.Administration.Groups
{
    public partial class AddEditGroup : BaseWebpage
    {
        protected MGroupProfile m_Profile = null;


        protected void Page_Load(object sender, EventArgs e)
        {
            String mGroupSeqId = GWWebHelper.GetQueryValue(Request, "GroupSeqID");
            if (!String.IsNullOrEmpty(mGroupSeqId))
            {
                int mGroupID = int.Parse(mGroupSeqId);
                if (mGroupID != -1)
                {
                    m_Profile = GroupUtility.GetProfile(mGroupID);
                }
                else
                {
                    m_Profile = new MGroupProfile();
                }
                HttpContext.Current.Session.Add("EditId", m_Profile.Id);
                populatePage();
            }
        }

        private void populatePage()
        {
            txtGroupSeqId.Value = m_Profile.Id.ToString();
            txtGroup.Text = m_Profile.Name;
            txtDescription.Text = m_Profile.Description;
        }
    }
}
using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.Framework.Model.Profiles.Interfaces;
using GrowthWare.WebSupport.Base;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GrowthWare.WebApplication.Functions.System.Administration.Messages
{
    public partial class AddEditMessage : BaseWebpage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["messageSeqId"]))
            {
                int mMessageSeqId = int.Parse(Request.QueryString["messageSeqId"].ToString());
                MMessageProfile mProfile = new MMessageProfile();
                if (mMessageSeqId > -1) mProfile = MessageUtility.GetProfile(mMessageSeqId);
                HttpContext.Current.Session.Add("EditId", mProfile.Id);
                populatePage(mProfile);
            }
        }

        private void populatePage(MMessageProfile profile)
        {
            txtMessageSeqID.Value = profile.Id.ToString();
            lblName.Text = profile.Name;
            txtName.Text = profile.Name;
            txtDescription.Text = profile.Description;
            txtMessageTitle.Text = profile.Title;
            txtMessageBody.Text = profile.Body;
            chkFormatAsHTML.Checked = profile.FormatAsHtml;

            if (profile.Id == -1)
            {
                txtName.Style.Add("display", "inline");
                lblName.Visible = false;
            }

            IMessageProfile mProfile = default(IMessageProfile);
            try
            {
                mProfile = (IMessageProfile)ObjectFactory.Create("GrowthWare.Framework", "GrowthWare.Framework.Model.Profiles", "M" + profile.Name);
            }
            catch (Exception ex)
            {
                Logger mLog = Logger.Instance();
                mLog.Debug(ex);
            }
            finally
            {
                if (mProfile == null)
                {
                    txtTags.Text = profile.GetTags(Environment.NewLine);
                }
                else
                {
                    txtTags.Text = mProfile.GetTags(Environment.NewLine);
                }
            }
        }
    }
}
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport.BasePages;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GrowthWare.WebApplication.Functions.System.SecurityEntities
{
    public partial class SelectSecurityEntity : ClientChoicesPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MAccountProfile mAccountProfile = AccountUtility.CurrentProfile();
            int mSecurityId = int.Parse(ClientChoicesState[MClientChoices.SecurityEntityId]);
            dropSecurityEntities.DataSource = SecurityEntityUtility.GetValidSecurityEntities(mAccountProfile.Account, mSecurityId, mAccountProfile.IsSystemAdmin);
            dropSecurityEntities.DataValueField = "SE_SEQ_ID";
            dropSecurityEntities.DataTextField = "NAME";
            dropSecurityEntities.DataBind();
            NameValuePairUtility.SetDropSelection(dropSecurityEntities, mSecurityId.ToString());
        }
    }
}
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GrowthWare.WebApplication.Functions.System.Administration.NVP
{
    public partial class AddEditNVPDetails : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string NVP_SEQ_ID = GWWebHelper.GetQueryValue(Request, "NVP_SEQ_ID");
            string NVP_Detail_SeqID = GWWebHelper.GetQueryValue(Request, "NVP_Detail_SeqID");
            MNameValuePairDetail mProfile = new MNameValuePairDetail();
            if (!String.IsNullOrEmpty(NVP_SEQ_ID) && !String.IsNullOrEmpty(NVP_Detail_SeqID))
            {
                int mSeqId = int.Parse(NVP_SEQ_ID);
                int mSeqDetId = int.Parse(NVP_Detail_SeqID);
                if (mSeqDetId != -1)
                {
                    mProfile = NameValuePairUtility.GetNameValuePairDetail(mSeqDetId, mSeqId);
                }
                hdnNVP_SEQ_ID.Value = mSeqId.ToString();
                hdnNVP_SEQ_DET_ID.Value = mProfile.Id.ToString();
                txtValue.Value = mProfile.Value;
                txtText.Value = mProfile.Text;
                txtSortOrder.Value = mProfile.SortOrder.ToString();
                HttpContext.Current.Session.Add("EditId", mProfile.Id);
            }
            HttpContext.Current.Session.Add("EditId", mProfile.Id);
        }
    }
}
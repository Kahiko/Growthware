using GrowthWare.WebSupport;
using GrowthWare.WebSupport.Base;
using System;

namespace GrowthWare.WebApplication.Functions.System.Administration.NVP
{
    public partial class SearchNVPChildren : BaseWebpage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String mNVP_SEQ_ID = GWWebHelper.GetQueryValue(Request, "NVP_SEQ_ID");
            if (!String.IsNullOrEmpty(mNVP_SEQ_ID))
            {
                NVP_SEQ_ID.Value = mNVP_SEQ_ID.ToString();
            }
        }
    }
}
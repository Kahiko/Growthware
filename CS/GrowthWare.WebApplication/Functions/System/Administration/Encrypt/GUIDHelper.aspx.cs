using GrowthWare.WebSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GrowthWare.WebApplication.Functions.System.Administration.Encrypt
{
    public partial class GUIDHelper : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod(CacheDuration = 0, EnableSession = false)]
        public static string GetGUID()
        {
            string mRetVal = string.Empty;
            mRetVal = GWWebHelper.GetNewGuid();
            return mRetVal;
        }
    }
}
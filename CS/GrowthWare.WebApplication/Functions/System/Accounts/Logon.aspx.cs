using GrowthWare.WebSupport;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GrowthWare.WebApplication.Functions.System.ExternalAuth
{
    public partial class Logon : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Exception mException = GWWebHelper.ExceptionError;
            if (mException != null)
            {
                clientMessage.Style.Add("display", "");
                clientMessage.InnerHtml = mException.Message.ToString();
                GWWebHelper.ExceptionError = null;
            }
            else 
            { 
                if(AccountUtility.CurrentProfile().Account.ToString().ToUpper(CultureInfo.InvariantCulture) != "ANONYMOUS")
                {
                    clientMessage.Style.Add("display", "");
                    clientMessage.InnerHtml = "You are currently logged on as " + AccountUtility.CurrentProfile().Account.ToString();
                }
            }
        }
    }
}
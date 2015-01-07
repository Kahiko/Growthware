using GrowthWare.WebSupport.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GrowthWare.WebApplication.Functions.System.ExternalAuth
{
    public partial class Logoff : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AccountUtility.LogOff();
        }
    }
}
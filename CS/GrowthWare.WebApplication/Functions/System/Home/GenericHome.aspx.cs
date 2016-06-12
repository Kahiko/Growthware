using GrowthWare.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GrowthWare.WebApplication.Functions.System.Home
{
    public partial class GenericHome : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblAppName.Text = ConfigSettings.AppDisplayedName;
            SideImage.ImageUrl = ResolveUrl("~/Content/GrowthWare/Images/Misc/sidebar_blue.gif");
        }
    }
}
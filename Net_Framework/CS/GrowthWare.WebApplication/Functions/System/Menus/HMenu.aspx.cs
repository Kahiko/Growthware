using GrowthWare.Framework.Model.Enumerations;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GrowthWare.WebApplication.Functions.System.Menus
{
    public partial class HMenu : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String mAccount = AccountUtility.HttpContextUserName();
            DataTable myDataTable = AccountUtility.GetMenu(mAccount, MenuType.Horizontal);
            NavigationTrail.DataSource = myDataTable.DefaultView;
            NavigationTrail.DataBind();
        }
    }
}
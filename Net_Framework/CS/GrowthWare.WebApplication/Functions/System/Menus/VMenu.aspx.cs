﻿using GrowthWare.Framework.Model.Enumerations;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Data;
using System.Web.UI;

namespace GrowthWare.WebApplication.Functions.System.Menus
{
    public partial class VMenu : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String mAccount = AccountUtility.HttpContextUserName();
            DataTable myDataTable = AccountUtility.GetMenu(mAccount, MenuType.Vertical);
            NavigationTrail.DataSource = myDataTable.DefaultView;
            NavigationTrail.DataBind();
        }
    }
}
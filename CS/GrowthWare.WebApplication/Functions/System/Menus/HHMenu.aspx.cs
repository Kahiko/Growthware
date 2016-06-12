using GrowthWare.Framework.Model.Enumerations;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Data;
using System.Text;
using System.Web.UI;

namespace GrowthWare.WebApplication.Functions.System.Menus
{
    public partial class HHMenu : Page
    {
        private String s_MenuRelationName = "MenuRelation";

        protected void Page_Load(object sender, EventArgs e)
        {
            //String mAccount = AccountUtility.HttpContextUserName();
            //DataTable mDataTable = AccountUtility.GetMenu(mAccount, MenuType.Hierarchical);
            //String mMenuString = String.Empty;
            //if (mDataTable != null && mDataTable.Rows.Count > 0)
            //{
            //    DataSet mDataset = new DataSet();
            //    mDataset.Tables.Add(mDataTable.Copy());
            //    String mMenu = String.Empty;
            //    DataRelation mRelation = new DataRelation(s_MenuRelationName, mDataset.Tables[0].Columns["MenuID"], mDataset.Tables[0].Columns["ParentID"]);
            //    StringBuilder mStringBuiler = new StringBuilder();
            //    mDataset.EnforceConstraints = false;
            //    mDataset.Relations.Add(mRelation);
            //    mMenuString = MenuUtility.GenerateUnorderedList(mDataTable, mStringBuiler);
            //}
            //cssMenu.InnerHtml = mMenuString;
        }
    }
}
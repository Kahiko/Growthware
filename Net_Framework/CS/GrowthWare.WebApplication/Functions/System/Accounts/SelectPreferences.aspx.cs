using GrowthWare.Framework.Model.Enumerations;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport.Base;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Data;
using System.Globalization;
using System.Web.UI.HtmlControls;

namespace GrowthWare.WebApplication.Functions.System.Accounts
{
    public partial class SelectPreferences : ClientChoicesPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string rowFilter = "FUNCTION_TYPE_SEQ_ID <> 2 AND FUNCTION_TYPE_SEQ_ID <> 3";
            DataView myDataView = AccountUtility.GetMenu(AccountUtility.CurrentProfile().Account, MenuType.Hierarchical).DefaultView;
            myDataView.Sort = "Title asc";
            myDataView.RowFilter = rowFilter;
            dropFavorite.DataSource = myDataView;
            dropFavorite.DataValueField = "url";
            dropFavorite.DataTextField = "Title";
            dropFavorite.DataBind();
            txtPreferedRecordsPerPage.Text = ClientChoicesState[MClientChoices.RecordsPerPage].ToString();
            NameValuePairUtility.SetDropSelection(dropFavorite, ClientChoicesState[MClientChoices.Action].ToString());
            int X = 0;
            for (X = 1; X <= 5; X++)
            {
                HtmlInputRadioButton button = (HtmlInputRadioButton)this.FindControl("Radio" + X);
                if (button.Value.Substring(0, ClientChoicesState[MClientChoices.ColorScheme].Length).ToLower(new CultureInfo("en-US", false)) == ClientChoicesState[MClientChoices.ColorScheme].ToLower(new CultureInfo("en-US", false)))
                {
                    button.Checked = true;
                    break; // TODO: might not be correct. Was : Exit For
                }
            }
        }
    }
}
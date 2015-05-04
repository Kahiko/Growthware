using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport;
using GrowthWare.WebSupport.Base;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace GrowthWare.WebApplication.Functions.System.Administration.NVP
{
    public partial class SearchNVPResults : ClientChoicesPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            string mAction = GWWebHelper.GetQueryValue(Request, "action");
            if (!String.IsNullOrEmpty(mAction))
            {
                MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(mAction);
                MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunctionProfile, AccountUtility.CurrentProfile());
                if (!mSecurityInfo.MayDelete)
                {
                    searchResults.Columns.RemoveAt(1);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            noResults.Visible = false;
            searchResults.HeaderStyle.ForeColor = ColorTranslator.FromHtml(ClientChoicesState[MClientChoices.HeaderForeColor]);
            searchResults.HeaderStyle.BackColor = ColorTranslator.FromHtml(ClientChoicesState[MClientChoices.HeadColor]);
            searchResults.AlternatingRowStyle.BackColor = ColorTranslator.FromHtml(ClientChoicesState[MClientChoices.AlternatingRowBackColor]);
            searchResults.RowStyle.BackColor = ColorTranslator.FromHtml(ClientChoicesState[MClientChoices.RowBackColor]);
            if (!String.IsNullOrEmpty(GWWebHelper.GetQueryValue(Request, "Columns")))
            {
                MSearchCriteria mSearchCriteria = new MSearchCriteria();
                mSearchCriteria.Columns = GWWebHelper.GetQueryValue(Request, "Columns");
                mSearchCriteria.OrderByColumn = Server.UrlDecode(GWWebHelper.GetQueryValue(Request, "OrderByColumn"));
                mSearchCriteria.OrderByDirection = GWWebHelper.GetQueryValue(Request, "OrderByDirection");
                int mTryParse = 0;
                if (int.TryParse(GWWebHelper.GetQueryValue(Request, "PageSize"), out mTryParse))
                {
                    mSearchCriteria.PageSize = int.Parse(GWWebHelper.GetQueryValue(Request, "PageSize"));
                }
                else
                {
                    mSearchCriteria.PageSize = 10;
                }

                if (int.TryParse(GWWebHelper.GetQueryValue(Request, "SelectedPage"), out mTryParse))
                {
                    mSearchCriteria.SelectedPage = int.Parse(GWWebHelper.GetQueryValue(Request, "SelectedPage"));
                }
                else
                {
                    mSearchCriteria.SelectedPage = 1;
                }
                mSearchCriteria.WhereClause = Server.UrlDecode(GWWebHelper.GetQueryValue(Request, "WhereClause"));
                mSearchCriteria.WhereClause = mSearchCriteria.WhereClause.Replace("\"", String.Empty);
                bindData(mSearchCriteria);
            }

        }

        protected void searchResults_RowDatabound(object sender, GridViewRowEventArgs e)
        {
            DataControlRowType rowType = e.Row.RowType;
            if (rowType == DataControlRowType.DataRow)
            {
                String mEditOnClick = "javascript:" + string.Format("edit('{0}')", DataBinder.Eval(e.Row.DataItem, "NVP_SeqID").ToString());
                String mDeleteOnClick = "javascript:" + string.Format("deleteNVP('{0}','{1}')", DataBinder.Eval(e.Row.DataItem, "NVP_SeqID").ToString(), DataBinder.Eval(e.Row.DataItem, "Name").ToString()).ToString();
                String mEditChildrenOnClick = "javascript:" + String.Format("manageChildren('{0}')", DataBinder.Eval(e.Row.DataItem, "NVP_SeqID").ToString());
                HtmlImage btnDetails = (HtmlImage)(e.Row.FindControl("btnDetails"));
                e.Row.Attributes.Add("ondblclick", mEditOnClick);
                btnDetails.Attributes.Add("onclick", mEditOnClick);
                HtmlImage btnDelete = (HtmlImage)(e.Row.FindControl("btnDelete"));
                //' Add confirmation to delete button
                if (btnDelete != null)
                {
                    btnDelete.Attributes.Add("onclick", mDeleteOnClick);
                }

                HtmlImage btnEditChildren = (HtmlImage)(e.Row.FindControl("btnEditChildren"));
                //' Add confirmation to delete button
                if (btnEditChildren != null)
                {
                    btnEditChildren.Attributes.Add("onclick", mEditChildrenOnClick);
                }

                //' add the hover behavior
                if (e.Row.RowState == DataControlRowState.Normal)
                {
                    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='Beige'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='" + ClientChoicesState[MClientChoices.RowBackColor] + "'");
                }
                else // the alternate row.
                {
                    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='Beige'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='" + ClientChoicesState[MClientChoices.AlternatingRowBackColor] + "'");
                }
            }
        }

        private void bindData(MSearchCriteria searchCriteria)
        {
            DataTable mDataTable = NameValuePairUtility.Search(searchCriteria);
            if (mDataTable != null && mDataTable.Rows.Count > 0)
            {
                DataView mDataView = mDataTable.DefaultView;
                recordsReturned.Value = mDataTable.Rows[0][0].ToString();
                searchResults.DataSource = mDataTable;
                searchResults.DataBind();
            }
            else
            {
                noResults.Visible = true;
            }
        }
    }
}
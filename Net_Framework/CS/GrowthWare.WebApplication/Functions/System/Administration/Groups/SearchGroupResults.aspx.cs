﻿using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport;
using GrowthWare.WebSupport.Base;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace GrowthWare.WebApplication.Functions.System.Administration.Groups
{
    public partial class SearchGroupResults : ClientChoicesPage
    {
        protected MAccountProfile m_AccountProfile = AccountUtility.CurrentProfile();

        private MSecurityInfo m_SecurityInfo = null ;

        /// <summary>
        /// Page_s the init.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Init(Object sender, EventArgs e)
        {
            string mAction = GWWebHelper.GetQueryValue(Request, "action");
            if (!String.IsNullOrEmpty(mAction))
            {
                m_SecurityInfo = new MSecurityInfo(FunctionUtility.CurrentProfile(), AccountUtility.CurrentProfile());
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
                mSearchCriteria.WhereClause += " AND Security_Entity_SeqID = " + SecurityEntityUtility.CurrentProfile().Id.ToString();
                mSearchCriteria.WhereClause = mSearchCriteria.WhereClause.Replace("\"", String.Empty);
                bindData(mSearchCriteria);
            }
        }

        /// <summary>
        /// Handles the Row Data bound event of the searchResults control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs" /> instance containing the event data.</param>
        protected void searchResults_RowDatabound(object sender, GridViewRowEventArgs e)
        {
            DataControlRowType rowType = e.Row.RowType;
            if (rowType == DataControlRowType.DataRow)
            {
                String mEditOnClick = "javascript:" + String.Format("edit('{0}',{1},{2})", DataBinder.Eval(e.Row.DataItem, "GROUP_SEQ_ID").ToString(), m_SecurityInfo.MayEdit.ToString().ToLowerInvariant(), m_SecurityInfo.MayDelete.ToString().ToLowerInvariant());
                String mEditMembersOnClick = "javascript:" + String.Format("editMembers('{0}')", DataBinder.Eval(e.Row.DataItem, "GROUP_SEQ_ID").ToString());
                HtmlImage btnDetails = (HtmlImage)(e.Row.FindControl("btnDetails"));
                e.Row.Attributes.Add("ondblclick", mEditOnClick);
                btnDetails.Attributes.Add("onclick", mEditOnClick);

                HtmlImage btnMembers = (HtmlImage)(e.Row.FindControl("btnMembers"));
                btnMembers.Attributes.Add("onclick", mEditMembersOnClick);
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

        /// <summary>
        /// Binds the data.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        private void bindData(MSearchCriteria searchCriteria)
        {
            DataTable mDataTable = GroupUtility.Search(searchCriteria);
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
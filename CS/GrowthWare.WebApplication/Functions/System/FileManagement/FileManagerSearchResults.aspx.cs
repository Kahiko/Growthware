using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport;
using GrowthWare.WebSupport.Base;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace GrowthWare.WebApplication.Functions.System.FileManagement
{
    public partial class FileManagerSearchResults : ClientChoicesPage
    {
        private bool m_ShowDeleteLink = false;

        public bool ShowDeleteLink
        {
            get { return m_ShowDeleteLink; }
            set { m_ShowDeleteLink = value; }
        }

        private MDirectoryProfile m_DirectoryProfile = null;

        private String m_CurrentDirectory = "/";

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
                MSecurityInfo mSecurityInfo = new MSecurityInfo(FunctionUtility.GetProfile(mAction), AccountUtility.CurrentProfile());
                if (!mSecurityInfo.MayDelete)
                {
                    this.searchResults.Columns.RemoveAt(1);
                }
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
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
                string mDesiredPath = Server.UrlDecode(GWWebHelper.GetQueryValue(Request, "desiredPath"));
                if (mDesiredPath.Length != 0)
                {
                    m_CurrentDirectory = mDesiredPath;
                }
                string mTestFunctionSeqID = GWWebHelper.GetQueryValue(Request, "functionSeqID");
                if (!String.IsNullOrEmpty(mTestFunctionSeqID))
                {
                    int mFunctionSeqId = int.Parse(mTestFunctionSeqID);
                }
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
                String mDeleteData = "{";
                mDeleteData += string.Format(
                        "\"FileName\" : \"{0}\",\"FileType\" : \"{1}\",\"CurrentDirectory\" : \"{2}\",\"FunctionSeqId\" : \"{3}\"",
                        Server.UrlEncode(DataBinder.Eval(e.Row.DataItem, "Name").ToString()),
                        DataBinder.Eval(e.Row.DataItem, "Type").ToString(),
                        Server.UrlEncode(m_CurrentDirectory),
                        m_DirectoryProfile.FunctionSeqId.ToString()).ToString();
                mDeleteData += "}";

                HtmlInputCheckBox chkDelete = (HtmlInputCheckBox)(e.Row.FindControl("DeleteCheckBox"));
                //' Add confirmation to delete button
                if (chkDelete != null)
                {
                    chkDelete.Attributes.Add("Data", mDeleteData);
                }

                global::System.Web.UI.WebControls.Image imgType = (global::System.Web.UI.WebControls.Image)e.Row.FindControl("imgType");
                string type = string.Empty;
                type = DataBinder.Eval(e.Row.DataItem, "type", "");
                if ((imgType != null))
                {
                    string mFileName = DataBinder.Eval(e.Row.DataItem, "Name").ToString();
                    switch (type.ToLower())
                    {
                        case "folder":
                            imgType.ImageUrl = "Public/GrowthWare/Images/Folder.gif";
                            HtmlAnchor changeDirectoryLink = ((HtmlAnchor)e.Row.FindControl("lnkName"));
                            if (changeDirectoryLink != null)
                            {
                                string mCurrentDirectory = m_CurrentDirectory;

                                string mPath = mCurrentDirectory + "/" + mFileName;
                                changeDirectoryLink.InnerText = mFileName + "\\";
                                changeDirectoryLink.HRef = string.Format("javascript:GW.FileManager.changeDirectory('{0}','{1}')", mPath, m_DirectoryProfile.FunctionSeqId.ToString());
                            }

                            break;
                        case "file":
                            imgType.ImageUrl = "Public/GrowthWare/Images/File.gif";
                            HtmlAnchor downloadLink = ((HtmlAnchor)e.Row.FindControl("lnkName"));
                            if (downloadLink != null)
                            {
                                string mCurrentDirectory = m_CurrentDirectory;
                                downloadLink.InnerText = mFileName;
                                downloadLink.HRef = string.Format("javascript:GW.FileManager.downLoad('{0}','{1}','{2}')", mCurrentDirectory, mFileName, m_DirectoryProfile.FunctionSeqId.ToString());
                            }

                            break;
                        default:
                            imgType.Visible = false;
                            break;
                    }
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

        /// <summary>
        /// Binds the data.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        private void bindData(MSearchCriteria searchCriteria)
        {
            try
            {
                HttpServerUtility mServer = Server;
                string mAction = GWWebHelper.GetQueryValue(HttpContext.Current.Request, "Action");
                MFunctionProfile mFunctionProfile = FunctionUtility.GetProfile(mAction);
                m_DirectoryProfile = DirectoryUtility.GetProfileByFunction(mFunctionProfile.Id);
                string mDirectoryPath = m_DirectoryProfile.Directory + m_CurrentDirectory;
                DataTable mDataTable = FileUtility.GetDirectoryTableData(mDirectoryPath, m_DirectoryProfile, false);
                SortTable mSorter = new SortTable();
                string mColName = searchCriteria.OrderByColumn;
                mSorter.Sort(mDataTable, mColName, searchCriteria.OrderByDirection);

                DataView mView = mDataTable.DefaultView;
                mView.Sort = "type desc";
                mDataTable = DataHelper.GetTable(ref mView);
                //mDataTable = DataHelper.GetPageOfData(ref mDataTable, ref searchCriteria);
                string mSort = "type desc, " + searchCriteria.OrderByColumn + " " + searchCriteria.OrderByDirection;
                mDataTable = DataHelper.GetPageOfData(ref mDataTable, mSort, ref searchCriteria);
                if (mDataTable != null && mDataTable.Rows.Count > 0)
                {
                    DataView mDataView = mDataTable.DefaultView;
                    recordsReturned.Value = mDataTable.Rows[0][DataHelper.TotalRowColumnName].ToString();
                    searchResults.DataSource = mDataTable;
                    searchResults.DataBind();
                }
                else
                {
                    noResults.Visible = true;
                }
            }
            catch (DirectoryNotFoundException)
            {
                litErrorMSG.Visible = true;
                litErrorMSG.Text = "The Directory has not been setup or is unavalible.";
            }
        }
    }
}
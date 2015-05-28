﻿Imports GrowthWare.WebSupport.Base
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.WebSupport
Imports System.Drawing

Public Class SearchGroupResults
    Inherits ClientChoicesPage

    Protected m_AccountProfile As MAccountProfile = AccountUtility.CurrentProfile()

    Protected m_SecurityInfo As MSecurityInfo = Nothing

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreInit
        m_SecurityInfo = New MSecurityInfo(FunctionUtility.CurrentProfile(), AccountUtility.CurrentProfile())
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        noResults.Visible = False
        searchResults.HeaderStyle.ForeColor = ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.HeaderForeColor))
        searchResults.HeaderStyle.BackColor = ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.HeadColor))
        searchResults.AlternatingRowStyle.BackColor = ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.AlternatingRowBackColor))
        searchResults.RowStyle.BackColor = ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.RowBackColor))
        If Not String.IsNullOrEmpty(GWWebHelper.GetQueryValue(Request, "Columns")) Then
            Dim mSearchCriteria As New MSearchCriteria()
            mSearchCriteria.Columns = GWWebHelper.GetQueryValue(Request, "Columns")
            mSearchCriteria.OrderByColumn = Server.UrlDecode(GWWebHelper.GetQueryValue(Request, "OrderByColumn"))
            mSearchCriteria.OrderByDirection = GWWebHelper.GetQueryValue(Request, "OrderByDirection")
            Dim mTryParse As Integer = 0
            If Integer.TryParse(GWWebHelper.GetQueryValue(Request, "PageSize"), mTryParse) Then
                mSearchCriteria.PageSize = Integer.Parse(GWWebHelper.GetQueryValue(Request, "PageSize"))
            Else
                mSearchCriteria.PageSize = 10
            End If
            If Integer.TryParse(GWWebHelper.GetQueryValue(Request, "SelectedPage"), mTryParse) Then
                mSearchCriteria.SelectedPage = Integer.Parse(GWWebHelper.GetQueryValue(Request, "SelectedPage"))
            Else
                mSearchCriteria.SelectedPage = 1
            End If
            mSearchCriteria.WhereClause = Server.UrlDecode(GWWebHelper.GetQueryValue(Request, "WhereClause"))
            mSearchCriteria.WhereClause += " AND Security_Entity_SeqID = " + SecurityEntityUtility.CurrentProfile().Id.ToString()
            mSearchCriteria.WhereClause = mSearchCriteria.WhereClause.Replace("""", String.Empty)
            bindData(mSearchCriteria)
        End If
    End Sub

    Private Sub bindData(ByVal searchCriteria As MSearchCriteria)
        Dim mDataTable As DataTable = GroupUtility.Search(searchCriteria)
        If Not mDataTable Is Nothing And mDataTable.Rows.Count > 0 Then
            Dim mDataView As DataView = mDataTable.DefaultView()
            recordsReturned.Value = mDataTable.Rows(0)(0)
            searchResults.DataSource = mDataTable
            searchResults.DataBind()
        Else
            noResults.Visible = True
        End If
    End Sub

    Private Sub searchResults_DataBound(sender As Object, e As GridViewRowEventArgs) Handles searchResults.RowDataBound
        Dim rowType As DataControlRowType = e.Row.RowType
        If rowType = DataControlRowType.DataRow Then
            Dim mEditOnClick As String = "javascript:" + String.Format("edit('{0}',{1},{2})", DataBinder.Eval(e.Row.DataItem, "GROUP_SEQ_ID").ToString(), m_SecurityInfo.MayEdit.ToString().ToLowerInvariant(), m_SecurityInfo.MayDelete.ToString().ToLowerInvariant())
            Dim mEditMembersOnClick As String = "javascript:" + String.Format("editMembers('{0}')", DataBinder.Eval(e.Row.DataItem, "GROUP_SEQ_ID").ToString())
            Dim btnDetails As HtmlImage = CType(e.Row.FindControl("btnDetails"), HtmlImage)
            e.Row.Attributes.Add("ondblclick", mEditOnClick)
            btnDetails.Attributes.Add("onclick", mEditOnClick)
            Dim btnMembers As HtmlImage = CType(e.Row.FindControl("btnMembers"), HtmlImage)
            If Not btnMembers Is Nothing Then btnMembers.Attributes.Add("onclick", mEditMembersOnClick)
            ' add the hover behavior
            If e.Row.RowState = DataControlRowState.Normal Then
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='Beige'")
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='" & ClientChoicesState(MClientChoices.RowBackColor) & "'")
            Else ' the alternate row.
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='Beige'")
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='" & ClientChoicesState(MClientChoices.AlternatingRowBackColor) & "'")
            End If
        End If
    End Sub

End Class
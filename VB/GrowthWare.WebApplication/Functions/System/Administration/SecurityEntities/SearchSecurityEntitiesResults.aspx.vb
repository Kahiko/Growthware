Imports GrowthWare.WebSupport
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.Framework.Model.Profiles
Imports System.Drawing
Imports GrowthWare.WebSupport.Base

Public Class SearchSecurityEntitiesResults
	Inherits ClientChoicesPage

    Protected m_SecurityInfo As MSecurityInfo = Nothing

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_SecurityInfo = New MSecurityInfo(FunctionUtility.CurrentProfile(), AccountUtility.CurrentProfile())
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
            mSearchCriteria.WhereClause = mSearchCriteria.WhereClause.Replace("""", String.Empty)
            bindData(mSearchCriteria)
        End If

    End Sub

    Private Sub bindData(ByVal searchCriteria As MSearchCriteria)
        Dim mDataTable As DataTable = SecurityEntityUtility.Search(searchCriteria)
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
            Dim mEditOnClick As String = "javascript:" + String.Format("edit('{0}','{1}')", DataBinder.Eval(e.Row.DataItem, "Security_Entity_SeqID").ToString(), m_SecurityInfo.MayEdit)
            'Dim mDeleteOnClick As String = "javascript:" + String.Format("delete('{0}','{1}')", DataBinder.Eval(e.Row.DataItem, "Security_Entity_SeqID").ToString(), CStr(DataBinder.Eval(e.Row.DataItem, "Name")))
            Dim btnDetails As HtmlImage = CType(e.Row.FindControl("btnDetails"), HtmlImage)
            e.Row.Attributes.Add("ondblclick", mEditOnClick)
            btnDetails.Attributes.Add("onclick", mEditOnClick)
            'Dim btnDelete As HtmlImage = CType(e.Row.FindControl("btnDelete"), HtmlImage)
            '' Add confirmation to delete button
            'btnDelete.Attributes.Add("onclick", mDeleteOnClick)
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
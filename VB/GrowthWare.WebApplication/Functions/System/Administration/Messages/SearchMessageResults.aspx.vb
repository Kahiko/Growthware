Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport
Imports GrowthWare.WebSupport.Base
Imports GrowthWare.WebSupport.Utilities
Imports System.Drawing

Public Class SearchMessageResults
    Inherits ClientChoicesPage

    Private m_ShowDeleteLink As Boolean = False

    Public Property ShowDeleteLink() As Boolean
        Get
            Return m_ShowDeleteLink
        End Get
        Set(ByVal value As Boolean)
            m_ShowDeleteLink = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
        Dim mDataTable As DataTable = MessageUtility.Search(searchCriteria)
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
            Dim mEditOnClick As String = "javascript:" + String.Format("edit('{0}')", DataBinder.Eval(e.Row.DataItem, "Message_SeqID").ToString())
            Dim mDeleteOnClick As String = "javascript:" + String.Format("deleteMessage('{0}','{1}')", DataBinder.Eval(e.Row.DataItem, "Message_SeqID").ToString(), DataBinder.Eval(e.Row.DataItem, "Name").ToString())
            Dim btnDetails As HtmlImage = CType(e.Row.FindControl("btnDetails"), HtmlImage)
            e.Row.Attributes.Add("ondblclick", mEditOnClick)
            btnDetails.Attributes.Add("onclick", mEditOnClick)
            Dim btnDelete As HtmlImage = CType(e.Row.FindControl("btnDelete"), HtmlImage)
            If Not btnDelete Is Nothing Then
                ' Add confirmation to delete button
                btnDelete.Attributes.Add("onclick", mDeleteOnClick)
            End If
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
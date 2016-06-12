Imports ApplicationBase.Common.CustomWebControls
Imports ApplicationBase.Common.Globals
Imports ApplicationBase.BusinessLogic
Imports ApplicationBase.Model.Accounts
Imports ApplicationBase.Model.Accounts.Security
Imports ApplicationBase.Model.Modules
Imports ApplicationBase.Model.Special.ClientChoices
Imports System.Data
Imports System.Drawing

Partial Class SelectAccounts
	Inherits ClientChoices.ClientChoicesUserControl

    Private myAccountUtility As New AccountUtility(HttpContext.Current)
    Protected WithEvents ClientSecurityInfo As MAccountSecurityInfo

    'The Page-level properties that write to ViewState
    Private Property SortExpression() As String
        Get
            Dim o As Object = viewstate("SortExpression")
            If o Is Nothing Then
                Return String.Empty
            Else
                Return o.ToString.Trim
            End If
        End Get
        Set(ByVal Value As String)
            viewstate("SortExpression") = Value
        End Set
    End Property

    Private Property SortAscending() As Boolean
        Get
            Dim o As Object = viewstate("SortAscending")
            If o Is Nothing Then
                Return True
            Else
                Return Convert.ToBoolean(o)
            End If
        End Get
        Set(ByVal Value As Boolean)
            viewstate("SortAscending") = Value
        End Set
    End Property

    Private Property SearchColumn() As String
        Get
            Dim o As Object = ViewState("SearchColumn")
            If o Is Nothing Then
                Return String.Empty
            Else
                Return o.ToString.Trim
            End If
        End Get
        Set(ByVal value As String)
            ViewState("SearchColumn") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            AlphaPicker.SelectedLetter = "All"
            SearchColumn = "Account"
            SortExpression = "Account"
            txtRecordsPerPage.Text = ClientChoicesState(MClientChoices.RecordsPerPage)
            dgResults.PageSize = ClientChoicesState(MClientChoices.RecordsPerPage)
            dgResults.HeaderStyle.BackColor = ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.HeadColor))
            dgResults.HeaderStyle.ForeColor = Color.WhiteSmoke
            dgResults.PagerStyle.BackColor = ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.HeadColor))
            dgResults.AlternatingItemStyle.BackColor = ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.HeadColor))
            dgResults.ItemStyle.BackColor = ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.SubheadColor))
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        bindData()
    End Sub

    Public Sub txtRecordsPerPage_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRecordsPerPage.TextChanged
        dgResults.CurrentPageIndex = 0
    End Sub 'txtRecordsPerPage_TextChanged

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        dgResults.CurrentPageIndex = 0
    End Sub

    Private Sub bindData()
        Dim searchText As String = txtSearch.Text.Trim
        Dim sortDirection As String = "asc"
        Dim dvResults As DataView
        Dim AccountType As Integer
        Dim myAccountUtility As New AccountUtility(HttpContext.Current)
        Dim BusinessUnitID As String = ClientChoicesState(MClientChoices.BusinessUnitID)
        If Not Me.SortAscending Then
            sortDirection = "desc"
        End If
        If HttpContext.Current.User.IsInRole("SysAdmin") Then
            AccountType = MAccountTypes.value.SysAdmin
        Else
            AccountType = MAccountTypes.value.AllOthers
        End If
        dvResults = myAccountUtility.GetAccountsByLetter(AccountType, BusinessUnitID)

        If searchText.Trim.Length = 0 Then
            If AlphaPicker.SelectedLetter = "All" Then   ' filter out the sortBy column
                dvResults.RowFilter = SearchColumn & " like '%'"
            Else
                dvResults.RowFilter = SearchColumn & " like '" & AlphaPicker.SelectedLetter & "%'"
            End If
        Else
            dvResults.RowFilter = SearchColumn & " like '%" & searchText & "%'"
        End If

        Try
            dvResults.Sort = SortExpression & " " & sortDirection
        Catch ex As Exception
            ' sort by the first colmn in the grid asending
            dvResults.Sort = SortExpression
        End Try

        If dvResults.Count > 0 Then
            dgResults.PageSize = Integer.Parse(txtRecordsPerPage.Text.Trim)
            dgResults.Visible = True
            UpdateColumnHeaders(dgResults)
            dgResults.DataSource = dvResults
            dgResults.DataBind()
            litNoData.Visible = False
        Else
            litNoData.Text = "No information found."
            litNoData.Visible = True
            dgResults.Visible = False
        End If
    End Sub

    Private Sub dgResults_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgResults.ItemCreated
        Dim itemType As ListItemType = e.Item.ItemType
        If itemType = ListItemType.Item Or itemType = ListItemType.AlternatingItem Then
            If itemType = ListItemType.Item Then
                e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor='beige';")
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='" & ClientChoicesState(MClientChoices.HeadColor) & "';")
                'e.Item.Attributes.Add("onclick", "javascript:__doPostBack" + "('_ctl0$DataGrid1$_ctl" + ((Convert.ToInt32(e.Item.ItemIndex.ToString)) + 2) + "$_ctl0','')")
            Else
                e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor='beige';")
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='" & ClientChoicesState(MClientChoices.SubheadColor) & "';")
                'e.Item.Attributes.Add("onclick", "javascript:__doPostBack" + "('_ctl0$DataGrid1$_ctl" + ((Convert.ToInt32(e.Item.ItemIndex.ToString)) + 2) + "$_ctl0','')")
            End If

        End If
        If itemType = ListItemType.Pager Then
            If ClientSecurityInfo Is Nothing Then
                myAccountUtility.GetAccountSecurityInfo(ClientSecurityInfo)
            End If
            Dim pagerCell As TableCell = CType(e.Item.Controls(0), TableCell)
            Dim newcell As TableCell = New TableCell
            Dim colSpan As Integer = pagerCell.ColumnSpan - 1
            newcell.HorizontalAlign = HorizontalAlign.Right
            newcell.Style("border-color") = pagerCell.Style("border-color")
            Dim lblNumRecords As Label = New Label
            lblNumRecords.ID = "lblNumRecords"
            newcell.Controls.Add(lblNumRecords)
            newcell.Wrap = False
            e.Item.Controls.AddAt(e.Item.Controls.Count, newcell)
            pagerCell.ColumnSpan = colSpan
        End If
    End Sub

    Private Sub dgResults_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgResults.ItemDataBound
        Dim itemType As ListItemType = e.Item.ItemType
        If itemType = ListItemType.Item OrElse itemType = ListItemType.AlternatingItem Then
            Dim HyperEdit As HyperLink = CType(e.Item.FindControl("hyperEdit"), HyperLink)
            HyperEdit.ImageUrl = BaseSettings.ImagePath & "AdminEdit.gif"
            HyperEdit.NavigateUrl = BaseSettings.FQDNPage & String.Format("?action=EditProfile&Account={0}", e.Item.DataItem("Account"))
            If ClientSecurityInfo Is Nothing Then
                myAccountUtility.GetAccountSecurityInfo(ClientSecurityInfo)
            End If
            If itemType = ListItemType.Item Then
                e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor='Beige'")
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='" & ClientChoicesState(MClientChoices.SubheadColor) & "'")
            Else
                e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor='Beige'")
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='" & ClientChoicesState(MClientChoices.HeadColor) & "'")
            End If
        End If
    End Sub

    Private Sub dgResults_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgResults.PageIndexChanged
        dgResults.CurrentPageIndex = e.NewPageIndex
    End Sub 'dgModules_PageIndexChanged

    Private Sub dgResults_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgResults.PreRender
        On Error Resume Next
        'top
        CType(dgResults.Controls(0).Controls(0).FindControl("lblNumRecords"), Label).Text = CType(dgResults.DataSource, DataView).Count.ToString + " records found " & dgResults.PageCount & " Page(s)"
        'bottom
        CType(dgResults.Controls(0).Controls(dgResults.Controls(0).Controls.Count - 1).FindControl("lblNumRecords"), Label).Text = CType(dgResults.DataSource, DataView).Count.ToString + " records found" & dgResults.PageCount & " Page(s)"
    End Sub

    Sub OrderChanged(ByVal sender As [Object], ByVal e As EventArgs) Handles AlphaPicker.LetterChanged
        dgResults.CurrentPageIndex = 0
    End Sub 'OrderChanged

    Private Sub dgResults_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgResults.SortCommand
        'Toggle SortAscending if the column that the data was sorted by has
        'been clicked again...
        If e.SortExpression = Me.SortExpression Then
            SortAscending = Not SortAscending
        Else
            SortAscending = True
        End If

        'Set the SortExpression property to the SortExpression passed in
        Me.SortExpression = e.SortExpression
    End Sub

    Sub UpdateColumnHeaders(ByVal dg As DataGrid)
        Dim c As DataGridColumn
        For Each c In dg.Columns
            'Clear any <img> tags that might be present
            c.HeaderText = Regex.Replace(c.HeaderText, "\s<.*>", String.Empty)
            If c.SortExpression = SortExpression Then
                If SortAscending Then
                    c.HeaderText &= " <img src=""" & BaseSettings.imagePath & "up.gif"" border=""0"">"
                Else
                    c.HeaderText &= " <img src=""" & BaseSettings.imagePath & "down.gif"" border=""0"">"
                End If
            End If
        Next
    End Sub

    Private Sub dropSearchBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dropSearchBy.SelectedIndexChanged
        SearchColumn = dropSearchBy.SelectedValue
    End Sub
End Class
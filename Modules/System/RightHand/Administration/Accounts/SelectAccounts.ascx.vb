Imports BLL.Base.ClientChoices
Imports Common.CustomWebControls
Imports DALModel.Base.Accounts
Imports DALModel.Special.ClientChoices

Public Class SelectAccounts
	Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
	Protected WithEvents AlphaPicker As AlphaPicker
	Protected WithEvents txtRecordsPerPage As System.Web.UI.WebControls.TextBox
	Protected WithEvents dropSortBy As System.Web.UI.WebControls.DropDownList
	Protected WithEvents dropOrderBy As System.Web.UI.WebControls.DropDownList
	Protected WithEvents dgResults As System.Web.UI.WebControls.DataGrid
	Protected WithEvents anchorReturnToAdministrationTop As System.Web.UI.HtmlControls.HtmlAnchor
	Protected WithEvents anchorReturnToAdministrationBottom As System.Web.UI.HtmlControls.HtmlAnchor
	Protected WithEvents NavTrail1 As NavTrail
	Protected WithEvents Navtrail2 As NavTrail
	Protected WithEvents txtSearch As System.Web.UI.WebControls.TextBox
	Protected WithEvents btnSearch As System.Web.UI.WebControls.Button
    Protected WithEvents txtFirstName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtLastName As System.Web.UI.WebControls.TextBox

	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object

	Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Not IsPostBack Then
            AlphaPicker.SelectedLetter = "All"
            txtRecordsPerPage.Text = ClientChoicesState(MClientChoices.RecordsPerPage)
            dgResults.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.HeadColor))
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Dim recsPerPage As Integer
        Try
            recsPerPage = CInt(txtRecordsPerPage.Text)
            If recsPerPage <= 0 Then
                recsPerPage = CInt(ClientChoicesState(MClientChoices.RecordsPerPage))
                txtRecordsPerPage.Text = recsPerPage
            End If
        Catch ex As Exception
            recsPerPage = 10
            txtRecordsPerPage.Text = CInt(ClientChoicesState(MClientChoices.RecordsPerPage))
        Finally
            dgResults.PageSize = recsPerPage
        End Try
        bindData()
    End Sub
    Private Sub bindData()
        Dim sortBy As String = dropSortBy.SelectedItem.Value
        Dim dvAccounts As DataView
        Dim AccountType As Integer
        Dim myAccountUtility As New AccountUtility(HttpContext.Current)
		Dim BusinessUnitID As String = ClientChoicesState(MClientChoices.BusinessUnitID)
        If HttpContext.Current.User.IsInRole("SysAdmin") Then
            AccountType = MAccountTypes.value.SysAdmin
        Else
            AccountType = MAccountTypes.value.AllOthers
        End If
		dvAccounts = myAccountUtility.GetAccountsByLetter(AccountType, BusinessUnitID)
        If sortBy = "0" Then   ' determin what column to filter and sort
            sortBy = "ACCOUNT"
        Else
            sortBy = "FullName"
        End If
        If txtSearch.Text.Trim.Length = 0 Then
            If AlphaPicker.SelectedLetter = "All" Then   ' filter out the sortBy column
                dvAccounts.RowFilter = sortBy & " like '%'"
            Else
                dvAccounts.RowFilter = sortBy & " like '" & AlphaPicker.SelectedLetter & "%'"
            End If
        Else
            dvAccounts.RowFilter = sortBy & " like '%" & txtSearch.Text & "%'"
        End If
        ' sort asc or desc
        If dropOrderBy.SelectedItem.Value = "0" Then
            dvAccounts.Sort = sortBy & " ASC"
        Else
            dvAccounts.Sort = sortBy & " DESC"
        End If
        dgResults.DataSource = dvAccounts
        dgResults.DataKeyField = "Account"
        dgResults.DataBind()
    End Sub ' bindData

    Private Sub dgResults_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgResults.ItemDataBound
        ' skip if no dataitem
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim HyperEdit As HyperLink = CType(e.Item.FindControl("hyperEdit"), HyperLink)
            HyperEdit.ImageUrl = BaseHelper.ImagePath & "AdminEdit.gif"
            HyperEdit.NavigateUrl = BaseHelper.FQDNBasePage & String.Format("?Action=EditProfile&Account={0}", e.Item.DataItem("Account"))
            Dim litEMail As Literal = CType(e.Item.FindControl("EMail"), Literal)
            litEMail.Text = e.Item.DataItem("EMAIL")

            ' Add confirmation to delete button
            'Dim btnDelete As ImageButton = CType(e.Item.FindControl("btnDelete"), ImageButton)
            'btnDelete.ImageUrl = BaseHelper.ImagePath & "delete.gif"
            'btnDelete.Attributes.Add("onclick", String.Format("return confirm('Are you sure you want to delete the ""{0}"" section?')", CStr(DataBinder.Eval(e.Item.DataItem, "Name"))))
        End If
    End Sub ' dgAccounts_ItemDataBound

    Private Sub dgResults_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgResults.PageIndexChanged
        dgResults.CurrentPageIndex = e.NewPageIndex
    End Sub ' dgAccounts_PageIndexChanged

    Public Sub txtRecordsPerPage_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRecordsPerPage.TextChanged
        dgResults.CurrentPageIndex = 0
    End Sub ' txtRecordsPerPage_TextChanged

    Sub OrderChanged(ByVal sender As [Object], ByVal e As EventArgs) Handles AlphaPicker.LetterChanged, dropOrderBy.SelectedIndexChanged, dropSortBy.SelectedIndexChanged
        dgResults.CurrentPageIndex = 0
    End Sub 'OrderChanged
End Class
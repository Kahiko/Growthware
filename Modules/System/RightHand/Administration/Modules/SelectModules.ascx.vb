Imports Common.CustomWebControls
Imports BLL.Base.ClientChoices
Imports BLL.Base.SQLServer
Imports DALModel.Base.Accounts.Security
Imports DALModel.Base.Modules
Imports DALModel.Special.ClientChoices

Public Class SelectModules
	Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents dgResults As System.Web.UI.WebControls.DataGrid
    Protected WithEvents RecordsPerPage As System.Web.UI.WebControls.TextBox
	Protected WithEvents AlphaPicker As AlphaPicker
	Protected WithEvents dropSortBy As System.Web.UI.WebControls.DropDownList
	Protected WithEvents dropOrderBy As System.Web.UI.WebControls.DropDownList
	Protected WithEvents txtRecordsPerPage As System.Web.UI.WebControls.TextBox
	Protected WithEvents anchorUpdateTop As System.Web.UI.HtmlControls.HtmlAnchor
	Protected WithEvents anchorUpdateBottom As System.Web.UI.HtmlControls.HtmlAnchor
	Protected WithEvents NavTrail1 As NavTrail
	Protected WithEvents Navtrail2 As NavTrail
	Protected WithEvents txtSearch As System.Web.UI.WebControls.TextBox
	Protected WithEvents btnSearch As System.Web.UI.WebControls.Button
    Protected WithEvents litNoData As System.Web.UI.WebControls.Literal

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

	Private myAccountUtility As New AccountUtility(HttpContext.Current)
	Private ClientSecurityInfo As MAccountSecurityInfo = myAccountUtility.GetAccountSecurityInfo(ClientSecurityInfo)

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		If Not IsPostBack Then
            txtRecordsPerPage.Text = ClientChoicesState(MClientChoices.RecordsPerPage)
            AlphaPicker.SelectedLetter = "All"
			dgResults.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.HeadColor))
		End If
	End Sub	'Page_Load

	'*********************************************************************
	'
	' Page_PreRender Method
	'
	' Performs any action needed before the page is rendered.
	' In most cases we will do any type of redirect here to avoid.
	' header errors.
	'
	'*********************************************************************
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
        If Not ClientSecurityInfo.MayDelete Then
            dgResults.Columns(1).Visible = False
        End If
        If dgResults.Items.Count > 0 Then
            litNoData.Visible = False
            dgResults.Visible = True
        Else
            litNoData.Visible = True
            dgResults.Visible = False
        End If
    End Sub ' Page_PreRender
    Private Sub bindData()
        Dim dvModules As New DataView
        Dim sortBy As String = dropSortBy.SelectedItem.Value
        Dim orderBy As String = dropOrderBy.SelectedItem.Value
        dvModules = AppModulesUtility.GetModulesDataView()
        If sortBy = "0" Then   ' determin what column to filter and sort
            sortBy = "Name"
        End If
        If txtSearch.Text.Trim.Length = 0 Then
            If AlphaPicker.SelectedLetter = "All" Then   ' filter out the sortBy column
                dvModules.RowFilter = sortBy & " like '%'"
            Else
                dvModules.RowFilter = sortBy & " like '" & AlphaPicker.SelectedLetter & "%'"
            End If
        Else
            dvModules.RowFilter = sortBy & " like '%" & txtSearch.Text & "%'"
        End If
        ' sort asc or desc
        If orderBy = "0" Then
            dvModules.Sort = sortBy & " ASC"
        Else
            dvModules.Sort = sortBy & " DESC"
        End If
        dgResults.DataSource = dvModules
        dgResults.DataKeyField = "MODULE_SEQ_ID"
        dgResults.DataBind()
    End Sub ' bindData
    Private Sub dgResults_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgResults.ItemDataBound
        ' skip if no dataitem
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim liName As Literal = CType(e.Item.FindControl("Name"), Literal)
            liName.Text = e.Item.DataItem("NAME")
            Dim HyperEdit As HyperLink = CType(e.Item.FindControl("hyperEdit"), HyperLink)
            HyperEdit.ImageUrl = BaseHelper.ImagePath & "AdminEdit.gif"
            HyperEdit.NavigateUrl = BaseHelper.FQDNBasePage & String.Format("?action=AddEditModules&id={0}", e.Item.DataItem("MODULE_SEQ_ID"))
            Dim btnDelete As ImageButton = CType(e.Item.FindControl("btnDelete"), ImageButton)
            ' Add confirmation to delete button
            btnDelete.ImageUrl = BaseHelper.ImagePath & "delete.gif"
            btnDelete.Attributes.Add("onclick", String.Format("return confirm('Are you sure you want to delete the ""{0}"" Module?')", CStr(DataBinder.Eval(e.Item.DataItem, "Name"))))
        End If
    End Sub ' dgModules_ItemDataBound

    '*******************************************************
    '
    ' User clicked the Delete link, display delete
    ' warning.
    '
    '*******************************************************
    Sub Item_Click(ByVal s As [Object], ByVal e As DataGridCommandEventArgs) Handles dgResults.ItemCommand
        Select Case e.CommandName
            Case "Delete"
                Dim MODULE_SEQ_ID As Integer = Fix(dgResults.DataKeys(e.Item.ItemIndex))
                Dim deleteOK As Boolean = BAppModules.DeleteModule(MODULE_SEQ_ID)
                Dim myAccountUtility As New AccountUtility(HttpContext.Current)
                AppModulesUtility.RemoveCachedModules()
                ' rebuild the cache
                Dim moduleProfileInfo As MModuleProfileInfo
                moduleProfileInfo = AppModulesUtility.GetModuleInfoByAction("Home")
                dgResults.CurrentPageIndex = 0
        End Select
    End Sub 'Item_Click

    Private Sub dgResults_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgResults.PageIndexChanged
        dgResults.CurrentPageIndex = e.NewPageIndex
    End Sub 'dgModules_PageIndexChanged

    Sub OrderChanged(ByVal sender As [Object], ByVal e As EventArgs) Handles AlphaPicker.LetterChanged, dropOrderBy.SelectedIndexChanged, dropSortBy.SelectedIndexChanged
        dgResults.CurrentPageIndex = 0
    End Sub 'OrderChanged

    Public Sub txtRecordsPerPage_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRecordsPerPage.TextChanged
        dgResults.CurrentPageIndex = 0
    End Sub 'txtRecordsPerPage_TextChanged

	Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
		dgResults.CurrentPageIndex = 0
	End Sub
End Class
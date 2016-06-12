Imports Common.CustomWebControls
Imports BLL.Base.ClientChoices
Imports BLL.Base.SQLServer
Imports DALModel.Base.Accounts.Security
Imports DALModel.Special.ClientChoices
Imports DALModel.Special.Accounts

Public Class SelectDropDowns
	Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents AlphaPicker As Common.CustomWebControls.AlphaPicker
	Protected WithEvents txtRecordsPerPage As System.Web.UI.WebControls.TextBox
	Protected WithEvents dropSortBy As System.Web.UI.WebControls.DropDownList
	Protected WithEvents dropOrderBy As System.Web.UI.WebControls.DropDownList
	Protected WithEvents txtSearch As System.Web.UI.WebControls.TextBox
	Protected WithEvents btnSearch As System.Web.UI.WebControls.Button
	Protected WithEvents litNoData As System.Web.UI.WebControls.Literal
	Protected WithEvents dgResults As System.Web.UI.WebControls.DataGrid

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
	Private AccountSecurityInfo As MAccountSecurityInfo = myAccountUtility.GetAccountSecurityInfo(AccountSecurityInfo)

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		If Not IsPostBack Then
			txtRecordsPerPage.Text = ClientChoicesState(MClientChoices.RecordsPerPage)
			AlphaPicker.SelectedLetter = "All"
			dgResults.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.HeadColor))
		End If
	End Sub
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
		If Not AccountSecurityInfo.MayDelete Then
			dgResults.Columns(1).Visible = False
		End If
		If dgResults.Items.Count > 0 Then
			litNoData.Visible = False
			dgResults.Visible = True
		Else
			litNoData.Visible = True
			dgResults.Visible = False
		End If
	End Sub	' Page_PreRender
	Private Sub bindData()
		Dim dvDropDowns As New DataView
		Dim sortBy As String = dropSortBy.SelectedItem.Value
		Dim orderBy As String = dropOrderBy.SelectedItem.Value
		Dim myAccountUtility As New AccountUtility(HttpContext.Current)
		Dim myAccountProfileInfo As MAccountProfileInfo = myAccountUtility.GetAccountProfileInfo(HttpContext.Current.User.Identity.Name)
		DropBoxUtility.GET_DROP_BOX_NAME(myAccountProfileInfo.ACCOUNT_SEQ_ID, dvDropDowns)
		If sortBy = "0" Then		 ' determin what column to filter and sort
			sortBy = "DESCRIPTION"
		End If
		If txtSearch.Text.Trim.Length = 0 Then
			If AlphaPicker.SelectedLetter = "All" Then			' filter out the sortBy column
				dvDropDowns.RowFilter = sortBy & " like '%'"
			Else
				dvDropDowns.RowFilter = sortBy & " like '" & AlphaPicker.SelectedLetter & "%'"
			End If
		Else
			dvDropDowns.RowFilter = sortBy & " like '%" & txtSearch.Text & "%'"
		End If
		' sort asc or desc
		If orderBy = "0" Then
			dvDropDowns.Sort = sortBy & " ASC"
		Else
			dvDropDowns.Sort = sortBy & " DESC"
		End If
		dgResults.DataSource = dvDropDowns
		dgResults.DataKeyField = "DROP_BOX_SEQ_ID"
		dgResults.DataBind()
	End Sub	' bindData
	Private Sub dgResults_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgResults.ItemDataBound
		' skip if no dataitem
		If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
			Dim HyperEdit As HyperLink = CType(e.Item.FindControl("hyperEdit"), HyperLink)
			HyperEdit.ImageUrl = BaseHelper.ImagePath & "AdminEdit.gif"
			HyperEdit.NavigateUrl = BaseHelper.FQDNBasePage & String.Format("?action=EditDropDown&id={0}", e.Item.DataItem("DROP_BOX_SEQ_ID"))
			'Dim btnDelete As ImageButton = CType(e.Item.FindControl("btnDelete"), ImageButton)
			' Add confirmation to delete button
			'btnDelete.ImageUrl = BaseHelper.ImagePath & "delete.gif"
			'btnDelete.Attributes.Add("onclick", String.Format("return confirm('Are you sure you want to delete the ""{0}"" Module?')", CStr(DataBinder.Eval(e.Item.DataItem, "DESCRIPTION"))))
		End If
	End Sub	' dgModules_ItemDataBound

	'*******************************************************
	'
	' User clicked the Delete link, display delete
	' warning.
	'
	'*******************************************************
	Sub Item_Click(ByVal s As [Object], ByVal e As DataGridCommandEventArgs) Handles dgResults.ItemCommand
		Select Case e.CommandName
			Case "Delete"
				Dim DROP_BOX_SEQ_ID As Integer = Fix(dgResults.DataKeys(e.Item.ItemIndex))
				'Dim deleteOK As Boolean = BAppModules.DeleteModule(DROP_BOX_SEQ_ID)
				Dim myAccountUtility As New AccountUtility(HttpContext.Current)
				' rebuild the cache
				dgResults.CurrentPageIndex = 0
		End Select
	End Sub	'Item_Click

	Private Sub dgResults_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgResults.PageIndexChanged
		dgResults.CurrentPageIndex = e.NewPageIndex
	End Sub	'dgModules_PageIndexChanged

	Sub OrderChanged(ByVal sender As [Object], ByVal e As EventArgs) Handles AlphaPicker.LetterChanged, dropOrderBy.SelectedIndexChanged, dropSortBy.SelectedIndexChanged
		dgResults.CurrentPageIndex = 0
	End Sub	'OrderChanged

	Public Sub txtRecordsPerPage_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRecordsPerPage.TextChanged
		dgResults.CurrentPageIndex = 0
	End Sub	'txtRecordsPerPage_TextChanged
End Class

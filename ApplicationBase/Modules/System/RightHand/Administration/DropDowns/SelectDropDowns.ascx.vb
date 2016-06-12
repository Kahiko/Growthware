Imports ApplicationBase.Common.CustomWebControls
Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Model.Accounts.Security
Imports ApplicationBase.Model.Special.ClientChoices
Imports ApplicationBase.Model.Accounts
Imports System.Data

Partial Class SelectDropDowns
	Inherits ClientChoices.ClientChoicesUserControl

	Private myAccountUtility As New AccountUtility(HttpContext.Current)
	Dim accountSecurityInfo As New MAccountSecurityInfo

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		myAccountUtility.GetAccountSecurityInfo(accountSecurityInfo)
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
            HyperEdit.ImageUrl = BaseSettings.ImagePath & "AdminEdit.gif"
            HyperEdit.NavigateUrl = BaseSettings.FQDNPage & String.Format("?action=EditDropDown&id={0}", e.Item.DataItem("DROP_BOX_SEQ_ID"))
			'Dim btnDelete As ImageButton = CType(e.Item.FindControl("btnDelete"), ImageButton)
			' Add confirmation to delete button
            'btnDelete.ImageUrl = BaseSettings.ImagePath & "delete.gif"
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

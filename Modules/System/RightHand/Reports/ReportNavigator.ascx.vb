Imports CrystalDecisions.Enterprise
'Imports CrystalDecisions.Enterprise.Desktop
Imports System.Text
Imports BLL.Base.ClientChoices
Imports Common.Cache
Imports Common.CustomWebControls
Imports Common.Security.BaseSecurity
Imports DALModel.Special.ClientChoices
Imports DALModel.Special.Accounts

Public Class ReportNavigator
	Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents btnRefresh As System.Web.UI.WebControls.Button
	Protected WithEvents AlphaPicker As AlphaPicker
	Protected WithEvents txtRecordsPerPage As System.Web.UI.WebControls.TextBox
	Protected WithEvents dropSortBy As System.Web.UI.WebControls.DropDownList
	Protected WithEvents dropOrderBy As System.Web.UI.WebControls.DropDownList
	Protected WithEvents txtReportName As System.Web.UI.WebControls.TextBox
	Protected WithEvents txtReportID As System.Web.UI.WebControls.TextBox
	Protected WithEvents btnSearch As System.Web.UI.WebControls.Button
	Protected WithEvents dgReportResults As System.Web.UI.WebControls.DataGrid
	Protected WithEvents plcReportsList As System.Web.UI.WebControls.PlaceHolder
	Protected WithEvents LnkButton As System.Web.UI.WebControls.LinkButton
	Protected WithEvents btnGoUp As System.Web.UI.WebControls.ImageButton

	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object

	Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

	Private defaultRecsPerPage As Integer = CInt(ClientChoicesState(MClientChoices.RecordsPerPage))
	Private Shared FolderID As String = "0"
	Private Shared oldFolderID As String = "0"
	Private Shared imgUpImage As String = BaseHelper.ImagePath & "FolderUp.gif"
	Private Shared dvReports As DataView

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		If Not IsPostBack Then
			Dim myAccountUtility As New AccountUtility(HttpContext.Current)
			Dim myAccountProfilInfo As MAccountProfileInfo = myAccountUtility.GetAccountProfileInfo(HttpContext.Current.User.Identity.Name)
			Dim myCryptoUtil As New CryptoUtil
			Dim thePassword As String = myCryptoUtil.DecryptTripleDES(myAccountProfilInfo.PWD)
			Dim ceSession As EnterpriseSession
			Dim ceSessionMgr As New SessionMgr
			btnGoUp.ImageUrl = imgUpImage
			btnGoUp.Visible = False
			AlphaPicker.SelectedLetter = "All"
			txtRecordsPerPage.Text = defaultRecsPerPage
			dgReportResults.HeaderStyle.BackColor = ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.SubheadColor))
			dgReportResults.HeaderStyle.ForeColor = Color.WhiteSmoke
			dgReportResults.PagerStyle.BackColor = ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.SubheadColor))
			' logon to Business Objects
			ceSession = ceSessionMgr.Logon(myAccountProfilInfo.ACCOUNT, thePassword, BaseHelper.BOServer, BaseHelper.BOAuthenticationType)
			'propagate the Enterprise Session
			' Sence the crystal objects are not serializable we need to put these
			' into cache and not the session because we need to keep
			' session information in the SQL database.
			CacheControler.AddToCacheDependency(HttpContext.Current.User.Identity.Name & "ceSession", ceSession)
		End If
	End Sub

	'*********************************************************************
	'
	' bindData Sub Routine
	'
	' Binds filtered data to the datagrid.
	'
	'*********************************************************************
	Private Sub bindData()
		Dim sortBy As String = dropSortBy.SelectedItem.Value
		Dim recsPerPage As Integer
		Try
			recsPerPage = CInt(txtRecordsPerPage.Text)
			If recsPerPage <= 0 Then
				recsPerPage = defaultRecsPerPage
				txtRecordsPerPage.Text = defaultRecsPerPage
			End If
		Catch ex As Exception
			recsPerPage = defaultRecsPerPage
			txtRecordsPerPage.Text = defaultRecsPerPage
		Finally
			dgReportResults.PageSize = recsPerPage
		End Try
		' get the data
		GetReportsTable()

		If txtReportName.Text.Trim.Length = 0 And txtReportID.Text.Trim.Length = 0 Then
			If sortBy = "0" Then			' determin what column to filter and sort
				sortBy = "Title"
			Else
				sortBy = "ID"
			End If
			If AlphaPicker.SelectedLetter = "All" Then			' filter out the sortBy column
				dvReports.RowFilter = sortBy & " like '%'"
			Else
				dvReports.RowFilter = sortBy & " like '" & AlphaPicker.SelectedLetter & "%'"
			End If
		Else
			If Not txtReportName.Text.Equals(String.Empty) Then
				sortBy = "Title"
				txtReportName.Text.Replace("'", "''")
				dvReports.RowFilter = sortBy & " like '%" & txtReportName.Text & "%'"
			Else
				sortBy = "ID"
				dvReports.RowFilter = sortBy & " like '%" & txtReportID.Text & "%'"
			End If
		End If
		' sort asc or desc
		If dropOrderBy.SelectedItem.Value = "0" Then
			dvReports.Sort = sortBy & " ASC"
		Else
			dvReports.Sort = sortBy & " DESC"
		End If
		dgReportResults.DataSource = dvReports
		dgReportResults.DataKeyField = "type"
		dgReportResults.DataBind()
	End Sub	'bindData

	'*********************************************************************
	'
	' GetReportsTable Sub Routine
	'
	' Retrieves the data from Business Objects Enterprise server
	' for the client and places the information in a dataview
	' the dataview is then placed into a session variable
	'
	'*********************************************************************
	Private Sub GetReportsTable()
		Dim strBuilder As New StringBuilder
		Dim ceSessionMgr As New SessionMgr
		Dim ceSession As EnterpriseSession
		Dim ceEnterpriseService As EnterpriseService
		Dim ceInfoStore As InfoStore
		Dim ceFolderObjects As InfoObjects
		Dim ceFolderObject As InfoObject
		Dim ceReportObjects As InfoObjects
		Dim ceReportObject As InfoObject
		Dim sQueryReports As String
		Dim sQueryFolders As String
		Try
			If TypeOf Cache.Item(HttpContext.Current.User.Identity.Name & "ceSession") Is Object Then

				'grab the Enterprise session
				ceSession = Cache.Item(HttpContext.Current.User.Identity.Name & "ceSession")

				'Create the infostore object
				ceEnterpriseService = ceSession.GetService("", "InfoStore")
				ceInfoStore = New InfoStore(ceEnterpriseService)

				'Create query to all folders
				If FolderID = 0 Then
					sQueryFolders = "SELECT SI_ID, SI_NAME, SI_DESCRIPTION FROM CI_INFOOBJECTS WHERE SI_PARENTID = 0"
				Else
					sQueryFolders = "SELECT SI_ID, SI_NAME, SI_DESCRIPTION FROM CI_INFOOBJECTS WHERE SI_PARENTID = " & FolderID
				End If

				'Create query to all reports that are not instances
				sQueryReports = "Select SI_ID, SI_DESCRIPTION FROM CI_INFOOBJECTS WHERE SI_PROGID = 'CRYSTALENTERPRISE.REPORT' AND SI_INSTANCE = 0 AND SI_PARENTID = " & FolderID
				' execute the queries
				ceFolderObjects = ceInfoStore.Query(sQueryFolders)
				ceReportObjects = ceInfoStore.Query(sQueryReports)
				Dim oTable As New DataTable("ReportTable")
				Dim oRow As DataRow = oTable.NewRow()
				oTable.Columns.Add("Title", System.Type.GetType("System.String"))
				oTable.Columns.Add("ID", System.Type.GetType("System.String"))
				oTable.Columns.Add("Description", System.Type.GetType("System.String"))
				oTable.Columns.Add("Type", System.Type.GetType("System.String"))
				'check for returned folders
				If ceFolderObjects.Count > 0 Then
					' populate the table with the folders
					For Each ceFolderObject In ceFolderObjects
						Dim type As String = ceFolderObject.GetType.Name.Trim
						If type.ToLower <> "report" Then
							oRow = oTable.NewRow()
							oRow("Title") = ceFolderObject.Title.ToString.Trim
							oRow("ID") = ceFolderObject.ID.ToString.Trim
							If ceFolderObject.Description.ToString.Trim.Length > 0 Then
								oRow("Description") = ceFolderObject.Description.ToString.Trim
							Else
								oRow("Description") = "None Given"
							End If
							oRow("Type") = ceFolderObject.GetType.Name.Trim
							oTable.Rows.Add(oRow)

						End If
					Next
					dvReports = oTable.DefaultView
				End If

				'check for returned reports
				If ceReportObjects.Count > 0 Then
					' populate the table with the folders
					For Each ceReportObject In ceReportObjects
						oRow = oTable.NewRow()
						oRow("Title") = ceReportObject.Title.ToString.Trim.Replace(".rpt", "")
						oRow("ID") = ceReportObject.ID.ToString.ToLower
						If ceReportObject.Description.ToString.Trim.Length > 0 Then
							oRow("Description") = ceReportObject.Description.ToString.Trim
						Else
							oRow("Description") = "None Given"
						End If
						oRow("Type") = ceReportObject.GetType.Name.Trim
						oTable.Rows.Add(oRow)
					Next
					dvReports = oTable.DefaultView
				End If
			Else
				'no Enterprise session available
				strBuilder.Append("No Valid Enterprise Session Found!<br>")
			End If

		Catch err As Exception
			strBuilder.Append("There was an error: <br>")
			strBuilder.Append(err.Message.ToString + "<br>")
		Finally
			If strBuilder.Length > 0 Then
				plcReportsList.Controls.Add(New LiteralControl(strBuilder.ToString))
			End If
		End Try
	End Sub	'GetReportsTable

	Private Sub dgReportResults_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgReportResults.PageIndexChanged
		dgReportResults.CurrentPageIndex = e.NewPageIndex
	End Sub

	Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
		bindData()
	End Sub

	Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
		FolderID = 0
		GetReportsTable()
		dgReportResults.CurrentPageIndex = 0
	End Sub

	Private Sub txtRecordsPerPage_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
		dgReportResults.CurrentPageIndex = 0
	End Sub

	Private Sub OrderChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AlphaPicker.LetterChanged
		txtReportName.Text = String.Empty
		txtReportID.Text = String.Empty
		dgReportResults.CurrentPageIndex = 0
	End Sub

	Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
		dgReportResults.CurrentPageIndex = 0
	End Sub

	Private Sub dropSortBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dropSortBy.SelectedIndexChanged
		txtReportName.Text = String.Empty
		txtReportID.Text = String.Empty
	End Sub

	Private Sub dgReportResults_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgReportResults.ItemDataBound
		If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.EditItem Then
			Dim imgType As System.Web.UI.WebControls.Image = e.Item.FindControl("imgType")
			Dim hyperReport As System.Web.UI.WebControls.HyperLink = e.Item.FindControl("hyperReport")
			Dim lnkReport As System.Web.UI.WebControls.LinkButton = e.Item.FindControl("lnkReport")
			Dim type As String
			Dim reportTitle As String
			reportTitle = DataBinder.Eval(e.Item.DataItem, "Title", "")
			type = DataBinder.Eval(e.Item.DataItem, "Type", "")
			If Not imgType Is Nothing Then
				Select Case type.ToLower
					Case "folder", "favoritesfolder"
						imgType.ImageUrl = BaseHelper.ImagePath & "Folder.gif"
						hyperReport.Visible = False
					Case "report"
						imgType.ImageUrl = BaseHelper.ImagePath & "File.gif"
						hyperReport.Text = reportTitle
						hyperReport.Target = "_Default"
						hyperReport.NavigateUrl = BaseHelper.FQDNBasePage & "?Action=ViewReport&server=page&id=" & DataBinder.Eval(e.Item.DataItem, "ID", "")
						lnkReport.Visible = False
					Case Else
						imgType.Visible = False
				End Select
			End If
		End If
	End Sub

	Private Sub dgReportResults_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgReportResults.ItemCommand
		If e.CommandName.ToLower = "itemclicked" Then
			dgReportResults.EditItemIndex = -1
			Dim lnkReport As LinkButton = e.Item.FindControl("lnkReport")
			Dim type As String = dgReportResults.DataKeys(e.Item.ItemIndex)
			Select Case type.ToLower
				Case "folder", "favoritesfolder"
					If FolderID <> lnkReport.CommandArgument Then
						oldFolderID = FolderID
						FolderID = lnkReport.CommandArgument
						dgReportResults.CurrentPageIndex = 0
						btnGoUp.Visible = True
						btnGoUp.CommandArgument = FolderID
						Exit Sub
					End If
				Case "report"
					Dim myHref As String
					myHref = ViewState("curDir") & "\" & lnkReport.Text.ToString()
					Dim filename As String = myHref
					'Response.AddHeader("Content-Disposition", "attachment; filename=" + lnkName.Text.Trim())
					'Response.WriteFile(myHref)
					Response.End()
			End Select
		End If
	End Sub

	Private Sub btnGoUp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnGoUp.Click
		Dim ceSessionMgr As New SessionMgr
		Dim ceSession As EnterpriseSession
		Dim ceEnterpriseService As EnterpriseService
		Dim ceInfoStore As InfoStore
		Dim ceFolderObjects As InfoObjects
		Dim ceFolderObject As InfoObject
		Dim sQueryFolders As String
		Try
			'grab the Enterprise session
			ceSession = Cache.Item(HttpContext.Current.User.Identity.Name & "ceSession")
			If TypeOf Cache.Item(HttpContext.Current.User.Identity.Name & "ceSession") Is Object Then
				'Create the infostore object
				ceEnterpriseService = ceSession.GetService("", "InfoStore")
				ceInfoStore = New InfoStore(ceEnterpriseService)
				'Create query to all folders
				If FolderID = 0 Then
					Exit Try
				Else
					sQueryFolders = "SELECT * FROM CI_INFOOBJECTS WHERE SI_PARENTID = " & FolderID
				End If
				' execute the queries
				ceFolderObjects = ceInfoStore.Query(sQueryFolders)
				If ceFolderObjects.Count > 0 Then
					For Each ceFolderObject In ceFolderObjects
						Try
							FolderID = ceFolderObject.Parent.ParentID
							If FolderID = 0 Then btnGoUp.Visible = False
							Exit For
						Catch ex As Exception
							FolderID = 0
							btnGoUp.Visible = False
						End Try
					Next
				End If
			End If
		Catch ex As Exception
			FolderID = 0
			btnGoUp.Visible = False
		End Try
		GetReportsTable()
	End Sub
End Class
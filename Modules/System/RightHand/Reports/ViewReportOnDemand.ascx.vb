Imports CrystalDecisions.Enterprise
'Imports CrystalDecisions.Enterprise.Desktop
Imports System.Text
Imports BLL.Base.ClientChoices
Imports Common.CustomWebControls
Imports Common.Security.BaseSecurity
Imports DALModel.Special.Accounts
Imports DALModel.Special.ClientChoices

Public Class ViewReportOnDemand
	Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents plcReportsList As System.Web.UI.WebControls.PlaceHolder
	Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
	Protected WithEvents dgReportResults As System.Web.UI.WebControls.DataGrid
	Protected WithEvents btnRefresh As System.Web.UI.WebControls.Button
	Protected WithEvents AlphaPicker As AlphaPicker
	Protected WithEvents txtRecordsPerPage As System.Web.UI.WebControls.TextBox
	Protected WithEvents dropSortBy As System.Web.UI.WebControls.DropDownList
	Protected WithEvents dropOrderBy As System.Web.UI.WebControls.DropDownList
	Protected WithEvents txtReportName As System.Web.UI.WebControls.TextBox
	Protected WithEvents txtReportID As System.Web.UI.WebControls.TextBox
	Protected WithEvents btnSearch As System.Web.UI.WebControls.Button

	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object

	Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

	Private defaultRecsPerPage As Integer = 10

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		If Not IsPostBack Then
			AlphaPicker.SelectedLetter = "All"
			txtRecordsPerPage.Text = defaultRecsPerPage
			dgReportResults.HeaderStyle.BackColor = ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.SubheadColor))
			dgReportResults.HeaderStyle.ForeColor = Color.WhiteSmoke
			dgReportResults.PagerStyle.BackColor = ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.SubheadColor))
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

		Dim dvReports As DataView
		dvReports = CType(Session.Item("dvReport"), DataView)
		If dvReports Is Nothing Then
			GetReportsTable()
			dvReports = CType(Session.Item("dvReport"), DataView)
			If dvReports Is Nothing Then
				'plcReportsList.Controls.Add(New LiteralControl("No Data found"))
				Exit Sub
			End If
		End If
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
		Dim ceReportObjects As InfoObjects
		Dim ceReportObject As InfoObject
		Dim sQuery As String
		Dim myAccountUtility As New AccountUtility(HttpContext.Current)
		Try
			Dim myAccountProfilInfo As MAccountProfileInfo = myAccountUtility.GetAccountProfileInfo(HttpContext.Current.User.Identity.Name)
			Dim myCryptoUtil As New CryptoUtil
			Dim thePassword As String = myCryptoUtil.DecryptTripleDES(myAccountProfilInfo.PWD)

			ceSession = ceSessionMgr.Logon(myAccountProfilInfo.ACCOUNT, thePassword, BaseHelper.BOServer, BaseHelper.BOAuthenticationType)

			'propagate the Enterprise Session
			Session.Add("ceSession", ceSession)

			If TypeOf Session.Item("ceSession") Is Object Then

				'grab the Enterprise session
				ceSession = Session.Item("ceSession")

				'Create the infostore object
				ceEnterpriseService = ceSession.GetService("", "InfoStore")
				ceInfoStore = New InfoStore(ceEnterpriseService)

				'Create query to get first 50 reports that are not instances
				'change query to return more or fewer reports
				sQuery = "Select SI_ID, SI_DESCRIPTION FROM CI_INFOOBJECTS WHERE SI_PROGID = 'CRYSTALENTERPRISE.REPORT' AND SI_INSTANCE = 0 AND SI_PARENTID = 960"
				sQuery = "Select SI_ID, SI_DESCRIPTION From CI_INFOOBJECTS Where SI_PROGID = 'CrystalEnterprise.Report' AND SI_INSTANCE = 0"
				ceReportObjects = ceInfoStore.Query(sQuery)
				Dim oTable As New DataTable("ReportTable")
				Dim oRow As DataRow = oTable.NewRow()
				oTable.Columns.Add("Title", System.Type.GetType("System.String"))
				oTable.Columns.Add("ID", System.Type.GetType("System.String"))
				oTable.Columns.Add("Description", System.Type.GetType("System.String"))

				'check for returned reports
				If ceReportObjects.Count > 0 Then
					For Each ceReportObject In ceReportObjects
						oRow = oTable.NewRow()
						oRow("Title") = ceReportObject.Title.ToString.Trim
						oRow("ID") = ceReportObject.ID.ToString.Trim
						If ceReportObject.Description.ToString.Trim.Length > 0 Then
							'oRow("Description") = ceReportObject.Description.ToString.Trim
						Else
							oRow("Description") = "None Given"
						End If
						oTable.Rows.Add(oRow)
					Next
					Dim myDataView As New DataView
					myDataView = oTable.DefaultView
					Session.Add("dvReport", myDataView)
				Else
					'no objects returned by query
					strBuilder.Append("No report objects found by query <br>")
					'strBuilder.Append("Please click <a href='Index.aspx'>here</a> to return to the logon page.<br>")
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
End Class
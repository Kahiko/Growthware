Imports BLL.Base.SQLServer
Imports BLL.Base.ClientChoices
Imports DALModel.Base.WorkFlows
Imports DALModel.Special.ClientChoices

Public Class SelectWorkFlowItem
	Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents AlphaPicker As Common.CustomWebControls.AlphaPicker
	Protected WithEvents txtRecordsPerPage As System.Web.UI.WebControls.TextBox
	Protected WithEvents dropSortBy As System.Web.UI.WebControls.DropDownList
	Protected WithEvents dropOrderBy As System.Web.UI.WebControls.DropDownList
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

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		If Not IsPostBack Then
			txtRecordsPerPage.Text = ClientChoicesState(MClientChoices.RecordsPerPage)
			AlphaPicker.SelectedLetter = "All"
			dgResults.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.HeadColor))
		End If
	End Sub	'Page_Load
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
	End Sub	'Page_PreRender

	Private Sub bindData()
		Dim dvWorkFlows As New DataView
		Dim sortBy As String = dropSortBy.SelectedItem.Value
		Dim WorkFlowName As String = Request.QueryString("ID")
		dvWorkFlows = WorkFlowUtility.GetWorkFlowDataView(WorkFlowName)
		If dvWorkFlows.Count = 0 Then
			WorkFlowUtility.RemoveWorkFlowDataView(WorkFlowName)
			NavControler.NavTo("selectworkflow")
		End If
		If sortBy = "0" Then		 ' determin what column to filter and sort
			sortBy = "WORK_FLOW_NAME"
		End If
		If AlphaPicker.SelectedLetter = "All" Then		 ' filter out the sortBy column
			dvWorkFlows.RowFilter = sortBy & " like '%'"
		Else
			dvWorkFlows.RowFilter = sortBy & " like '" & AlphaPicker.SelectedLetter & "%'"
		End If
		' sort asc or desc
		If dropOrderBy.SelectedItem.Value = "0" Then
			dvWorkFlows.Sort = sortBy & " ASC"
		Else
			dvWorkFlows.Sort = sortBy & " DESC"
		End If
		dgResults.DataSource = dvWorkFlows
		dgResults.DataKeyField = "WORK_FLOW_NAME"
		dgResults.DataKeyField = "WORK_FLOW_SEQ_ID"
		dgResults.DataBind()
	End Sub	'bindData
	Private Sub dgResults_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgResults.ItemDataBound
		' skip if no dataitem
		If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
			Dim HyperEdit As HyperLink = CType(e.Item.FindControl("hyperEdit"), HyperLink)
			HyperEdit.ImageUrl = BaseHelper.ImagePath & "AdminEdit.gif"
			HyperEdit.NavigateUrl = BaseHelper.FQDNBasePage & String.Format("?Action=EditWorkFlow&ID={0}&WorkFlowName={1}", e.Item.DataItem("WORK_FLOW_SEQ_ID"), e.Item.DataItem("WORK_FLOW_NAME"))
			Dim btnDelete As ImageButton = CType(e.Item.FindControl("btnDelete"), ImageButton)
			' Add confirmation to delete button
			btnDelete.ImageUrl = BaseHelper.ImagePath & "delete.gif"
			btnDelete.Attributes.Add("onclick", String.Format("return confirm('Are you sure you want to delete the ""{0}"" Work flow?')", CStr(DataBinder.Eval(e.Item.DataItem, "WORK_FLOW_NAME"))))
		End If
	End Sub	'dgStates_ItemDataBound

	'*******************************************************
	'
	' User clicked the Delete link, display delete
	' warning.
	'
	'*******************************************************
	Sub Item_Click(ByVal s As [Object], ByVal e As DataGridCommandEventArgs) Handles dgResults.ItemCommand
		Select Case e.CommandName
			Case "Delete"
				Dim myWorkProfileInfo As MWorkFlowProfileInfo
				Dim myWorkFlowName As String = Request.QueryString("ID")
				Dim myWorkFlowProfileInfoCollection As MWorkFlowProfileInfoCollection
				Dim WORK_FLOW_SEQ_ID As Integer = Fix(dgResults.DataKeys(e.Item.ItemIndex))
				WorkFlowUtility.GetWorkFlowCollection(myWorkFlowName, myWorkFlowProfileInfoCollection)
				myWorkProfileInfo = myWorkFlowProfileInfoCollection.GetWorkFlowByID(WORK_FLOW_SEQ_ID)

                Dim deleteOK As Boolean = BWorkFlows.DeleteProfile(myWorkProfileInfo)
				If deleteOK Then
					WorkFlowUtility.RemoveCachedWorkFlowProfile(myWorkFlowName)
					Dim myUrl As String = ""
					myUrl = String.Format("SelectWorkFlowItem&id={0}", myWorkProfileInfo.WorkFlowName)
					NavControler.NavTo(myUrl)

				End If
				dgResults.CurrentPageIndex = 0
		End Select
	End Sub	'Item_Click


	Private Sub dgResults_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgResults.PageIndexChanged
		dgResults.CurrentPageIndex = e.NewPageIndex
	End Sub	'dgStates_PageIndexChanged

	Sub OrderChanged(ByVal sender As [Object], ByVal e As EventArgs) Handles AlphaPicker.LetterChanged, dropOrderBy.SelectedIndexChanged, dropSortBy.SelectedIndexChanged
		dgResults.CurrentPageIndex = 0
	End Sub	'OrderChanged

	Public Sub txtRecordsPerPage_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRecordsPerPage.TextChanged
		dgResults.CurrentPageIndex = 0
	End Sub	'txtRecordsPerPage_TextChanged
End Class

Imports BaseApplication.ClientChoices
Imports DALModel.Base
Imports DALModel.Special.Accounts
Imports DALModel.Special.ClientChoices

Public Class AddEditDropDownDetails
	Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents dropDropDowns As System.Web.UI.WebControls.DropDownList
	Protected WithEvents dgResults As System.Web.UI.WebControls.DataGrid
	Protected WithEvents txtCode As System.Web.UI.HtmlControls.HtmlInputText
	Protected WithEvents txtValue As System.Web.UI.HtmlControls.HtmlInputText
	Protected WithEvents Submit1 As System.Web.UI.HtmlControls.HtmlInputButton
	Protected WithEvents Message As System.Web.UI.HtmlControls.HtmlGenericControl
	Protected WithEvents btnAddDropDownValue As System.Web.UI.WebControls.Button

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
			Dim myAccountUtility As New AccountUtility(HttpContext.Current)
			Dim myAccountProfileInfo As MAccountProfileInfo = myAccountUtility.GetAccountProfileInfo(HttpContext.Current.User.Identity.Name)
			Dim myDataView As New DataView
			DropBoxUtility.GET_DROP_BOX_NAME(myAccountProfileInfo.ACCOUNT_SEQ_ID, myDataView)
			If Not myDataView Is Nothing Then
				If myDataView.Count > 0 Then
					dropDropDowns.DataSource = myDataView
					dropDropDowns.DataValueField = "DROP_BOX_SEQ_ID"
					dropDropDowns.DataTextField = "DESCRIPTION"
					dropDropDowns.DataBind()
				End If
			End If
			dgResults.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.HeadColor))
		End If
	End Sub

	Public Function GetStatusIndex(ByVal StatusValue As Integer) As Integer
		Select Case StatusValue
			Case MSystemStatus.value.Active
				Return 0
			Case Else
				Return 1
		End Select
	End Function

	'*******************************************************
	' Perform binding to the DataGrid.
	'*******************************************************
	Sub BindData()
		Dim myDataView As New DataView
		myDataView = DropBoxUtility.GET_DROP_BOX_DETAILS(CInt(dropDropDowns.SelectedValue))
		If Not myDataView Is Nothing Then
			If myDataView.Count > 0 Then
				dgResults.DataSource = DropBoxUtility.GET_DROP_BOX_DETAILS(CInt(dropDropDowns.SelectedValue))
				dgResults.DataKeyField = "DROP_BOX_DET_ID"
				dgResults.DataBind()
			End If
		End If
	End Sub	'BindRoles


	Private Sub dgResults_ItemDataBound(ByVal s As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgResults.ItemDataBound
		If e.Item.DataItem Is Nothing Then
			Return
		End If
		Dim lblStatus As Label = CType(e.Item.FindControl("lblStatus"), Label)
		If Not lblStatus Is Nothing Then
			Select Case CInt(e.Item.DataItem("DROP_BOX_DET_STATUS"))
				Case MSystemStatus.value.Active
					lblStatus.Text = "Active"
				Case Else
					lblStatus.Text = "Inactive"
			End Select
		End If
	End Sub	'dgResults_ItemDataBound

	'*******************************************************
	' A Drop Down entry has been selected for editing.
	'*******************************************************
	Private Sub dgResults_EditCommand(ByVal s As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgResults.EditCommand
		' Select row for editing
		dgResults.EditItemIndex = e.Item.ItemIndex
	End Sub	'dgResults_EditCommand

	'*******************************************************
	' Edit has been canceled for a drop down entry.
	'*******************************************************
	Private Sub dgResults_CancelCommand(ByVal s As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgResults.CancelCommand
		' UnSelect row for editing
		dgResults.EditItemIndex = -1
	End Sub	'dgResults_CancelCommand

	'*******************************************************
	'
	' Update the role in the database.
	'
	'*******************************************************
	Private Sub dgResults_UpdateCommand(ByVal s As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgResults.UpdateCommand
		Dim myAccountUtility As New AccountUtility(HttpContext.Current)
		Dim myAccountProfileInfo As MAccountProfileInfo = myAccountUtility.GetAccountProfileInfo(HttpContext.Current.User.Identity.Name)
		Dim DROP_BOX_DET_ID As Integer = CInt(dgResults.DataKeys(e.Item.ItemIndex))
		Dim DROP_BOX_DET_code As Integer = CInt(CType(e.Item.Cells(1).Controls(0), TextBox).Text)
		Dim DROP_BOX_DET_value As String = CType(e.Item.Cells(2).Controls(0), TextBox).Text
		Dim DROP_BOX_DET_status As Integer = CInt(CType(e.Item.Cells(3).Controls(1), DropDownList).SelectedValue)
		Dim DROP_BOX_ID As Integer = CInt(dropDropDowns.SelectedValue)
		Dim ACCOUNT_SEQ_ID As Integer = myAccountProfileInfo.ACCOUNT_SEQ_ID
		Dim success As Boolean = False
		' Update values
		success = DropBoxUtility.EDIT_DROP_BOX(DROP_BOX_DET_ID, DROP_BOX_DET_code, DROP_BOX_DET_value, DROP_BOX_DET_status, DROP_BOX_ID, ACCOUNT_SEQ_ID)
		' UnSelect row for editing
		dgResults.EditItemIndex = -1
		'BindRoles()

	End Sub	'RoleList_UpdateCommand


	Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
		On Error Resume Next
		Dim SelectedIndex = dropDropDowns.SelectedIndex
		If SelectedIndex = -1 Then dropDropDowns.SelectedIndex = 0
		BindData()
	End Sub

	Private Sub btnAddDropDownValue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddDropDownValue.Click
		Dim myAccountUtility As New AccountUtility(HttpContext.Current)
		Dim myAccountProfileInfo As MAccountProfileInfo = myAccountUtility.GetAccountProfileInfo(HttpContext.Current.User.Identity.Name)
		Dim DROP_BOX_DET_code As Integer = CInt(txtCode.Value)
		Dim DROP_BOX_DET_value As String = txtValue.Value
		Dim DROP_BOX_DET_status As Integer = 0
		Dim DROP_BOX_ID As Integer = CInt(dropDropDowns.SelectedValue)
		Dim ACCOUNT_SEQ_ID As Integer = myAccountProfileInfo.ACCOUNT_SEQ_ID
		Dim success As Boolean = False
		' Add values
		success = DropBoxUtility.ADD_DROP_BOX_DETAIL(DROP_BOX_DET_code, DROP_BOX_DET_value, DROP_BOX_DET_status, DROP_BOX_ID, ACCOUNT_SEQ_ID)

	End Sub
End Class
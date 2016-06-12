Imports BLL.Base.ClientChoices
Imports DALModel.Special.Accounts
Imports DALModel.Special.ClientChoices

Public Class SelectPreferences
	Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents dropFavorite As System.Web.UI.WebControls.DropDownList
	Protected WithEvents btnSubmit As System.Web.UI.WebControls.Button
	Protected WithEvents txtPreferedRecordsPerPage As System.Web.UI.WebControls.TextBox
	Protected WithEvents Requiredfieldvalidator3 As System.Web.UI.WebControls.RequiredFieldValidator
	Protected WithEvents RangeValidator1 As System.Web.UI.WebControls.RangeValidator

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
			PopulatePage()
		End If
	End Sub

	Private Sub PopulatePage()
		Dim myAccountProfileInfo As MAccountProfileInfo
		Dim myAccountUtility As New AccountUtility(HttpContext.Current)
		Dim dsNavLinksLeft As New DataSet
		Dim X As Integer
		Dim oTable As New DataTable
		Dim oRow As DataRow = oTable.NewRow()
		oTable.Columns.Add("Name", System.Type.GetType("System.String"))
		oTable.Columns.Add("Action", System.Type.GetType("System.String"))
		myAccountProfileInfo = myAccountUtility.GetAccountProfileInfo(HttpContext.Current.User.Identity.Name)
		NavMenuUtility.GetRootLinks(dsNavLinksLeft, myAccountProfileInfo.ACCOUNT_SEQ_ID, 1)
		For X = 0 To dsNavLinksLeft.Tables(0).Rows.Count - 1
			oRow("Name") = dsNavLinksLeft.Tables(0).Rows(X).Item(0)
			Dim strAction As String = dsNavLinksLeft.Tables(0).Rows(X).Item(1)
			oRow("Action") = strAction.Trim
			oTable.Rows.Add(oRow)
			oRow = oTable.NewRow
		Next
		dropFavorite.DataSource = oTable
		dropFavorite.DataTextField = "Name"
		dropFavorite.DataValueField = "Action"
		dropFavorite.DataBind()
		BaseHelper.SetDropSelection(dropFavorite, ClientChoicesState(MClientChoices.Action))
		txtPreferedRecordsPerPage.Text = ClientChoicesState(MClientChoices.RecordsPerPage)
	End Sub

	Private Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
		ClientChoicesState(MClientChoices.Action) = dropFavorite.SelectedValue
		ClientChoicesState(MClientChoices.RecordsPerPage) = txtPreferedRecordsPerPage.Text
	End Sub
End Class
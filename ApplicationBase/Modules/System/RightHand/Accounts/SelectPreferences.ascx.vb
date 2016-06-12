Imports ApplicationBase.Model.Accounts
Imports ApplicationBase.Model.Special.ClientChoices
Imports System.Data

Partial Class SelectPreferences
	Inherits ClientChoices.ClientChoicesUserControl

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
        BaseHelperOld.SetDropSelection(dropFavorite, ClientChoicesState(MClientChoices.Action))
		txtPreferedRecordsPerPage.Text = ClientChoicesState(MClientChoices.RecordsPerPage)
	End Sub

	Private Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
		ClientChoicesState(MClientChoices.Action) = dropFavorite.SelectedValue
		ClientChoicesState(MClientChoices.RecordsPerPage) = txtPreferedRecordsPerPage.Text
	End Sub
End Class
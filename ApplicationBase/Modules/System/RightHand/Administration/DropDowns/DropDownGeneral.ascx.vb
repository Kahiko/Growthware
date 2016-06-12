Imports ApplicationBase.BusinessLogic
Imports ApplicationBase.Model.Accounts
Imports ApplicationBase.Model.Special.ClientChoices
Imports System.Data

Partial Class DropDownGeneral
	Inherits ClientChoices.ClientChoicesUserControl

	Private _DROP_BOX_SEQ_ID As Integer

	Public ReadOnly Property DROP_BOX_SEQ_ID() As Integer
		Get
			Return _DROP_BOX_SEQ_ID
		End Get
	End Property

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		If Not Request.QueryString("ID") Is Nothing Then
			Dim DROP_BOX_SEQ_ID As Integer = CInt(Request.QueryString("ID"))
			Dim BUSINESS_SEQ_ID As Integer = ClientChoicesState(MClientChoices.BusinessUnitID)
			Dim myAccountUtility As New AccountUtility(HttpContext.Current)
			Dim myAccountProfileInfo As MAccountProfileInfo = myAccountUtility.GetAccountProfileInfo(HttpContext.Current.User.Identity.Name)
			Dim myDataView As New DataView
			DropBoxUtility.GET_DROP_BOX_NAME(myAccountProfileInfo.ACCOUNT_SEQ_ID, myDataView)
			myDataView.RowFilter = "DROP_BOX_SEQ_ID=" & DROP_BOX_SEQ_ID
			txtName.Text = myDataView.Item(0).Item(1)
		End If
	End Sub

	Public Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) ' Handles btnSave.Click
		If Page.IsValid Then
			Dim myAccountUtility As New AccountUtility(HttpContext.Current)
			Dim myAccountProfileInfo As MAccountProfileInfo = myAccountUtility.GetAccountProfileInfo(HttpContext.Current.User.Identity.Name)
			If Request.QueryString("ID") Is Nothing Then
				_DROP_BOX_SEQ_ID = BDropBox.ADD_DROP_BOX(myAccountProfileInfo.ACCOUNT_SEQ_ID, txtName.Text.Trim)
			Else
				Dim success As Boolean = False
				success = BDropBox.UPDATE_DROP_BOX(CInt(Request.QueryString("ID")), txtName.Text.Trim)
				If Not success Then
					Dim myEx As New ApplicationException("Could not update the drop box")
					Throw myEx
				End If
				_DROP_BOX_SEQ_ID = CInt(Request.QueryString("ID"))
			End If
		End If
	End Sub
End Class

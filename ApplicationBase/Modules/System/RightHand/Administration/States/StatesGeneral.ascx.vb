Imports ApplicationBase.Common.Security
Imports ApplicationBase.Model.States

Partial Class StatesGeneral
	Inherits System.Web.UI.UserControl

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
		If Not IsPostBack Then
			Dim stateProfileInfo As New MStateProfileInfo
			stateProfileInfo = StatesUtility.GetInfoByState(Request.QueryString("id"))
			If Not stateProfileInfo Is Nothing Then
				litState.Text = stateProfileInfo.State
				txtDescription.Text = stateProfileInfo.LongName
                BaseHelperOld.SetDropSelection(STATUS_SEQ_ID, stateProfileInfo.STATUS_SEQ_ID)
			End If
		End If
	End Sub

	Public Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) ' Handles btnSave.Click
		Dim statesProfileInfo As New MStateProfileInfo
		Dim wasSuccess As Boolean
		statesProfileInfo.State = litState.Text.Trim.ToUpper
		statesProfileInfo.LongName = txtDescription.Text.Trim
		statesProfileInfo.STATUS_SEQ_ID = STATUS_SEQ_ID.SelectedValue
		Try
			wasSuccess = StatesUtility.UpdateStateProfileInfo(statesProfileInfo)
			If Not wasSuccess Then
				Dim ex As New Exception("Could not update state information")
				Throw ex
			End If
		Catch ex As Exception
			Throw New Exception("Could not update state information" & vbCrLf & ex.Message)
		End Try
	End Sub
End Class
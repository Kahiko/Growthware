Imports ApplicationBase.Common.CustomWebControls
Imports ApplicationBase.Model
Imports ApplicationBase.Model.States
Imports ApplicationBase.Model.Special.ClientChoices

Partial Class EditStates
	Inherits ClientChoices.ClientChoicesUserControl

	'Protected WithEvents statesGeneral As statesGeneral

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
		If Not IsPostBack Then
			bottomTabStrip.BgColor = ClientChoicesState(MClientChoices.HeadColor)
		End If
		Dim statesProfileInfo As MStateProfileInfo
		statesProfileInfo = StatesUtility.GetInfoByState(Request.QueryString("id"))
		litState.Text = statesProfileInfo.LongName.Trim
	End Sub

	Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
		statesGeneral.btnSave_Click(sender, e)
		StatesUtility.RemoveCachedStatesDV()
	End Sub
End Class

Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Model.Special.ClientChoices

Partial Class Home
	Inherits ClientChoices.ClientChoicesUserControl

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		On Error Resume Next
		If Not IsPostBack Then
            lblAppName.Text = BaseSettings.appDisplayedName
            SideImage.ImageUrl = BaseSettings.ImagePath & SideImage.ImageUrl & ClientChoicesState(MClientChoices.ColorScheme) & ".gif"
		End If
	End Sub	'Page_Load
End Class
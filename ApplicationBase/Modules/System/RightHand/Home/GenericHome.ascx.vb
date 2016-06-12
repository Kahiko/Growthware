Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Model.Special.ClientChoices

Partial Class GenericHome
	Inherits ClientChoices.ClientChoicesUserControl

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		On Error Resume Next
        lblAppName.Text = BaseSettings.appDisplayedName
        SideImage.ImageUrl = BaseSettings.ImagePath & "sidebar_" & ClientChoicesState(MClientChoices.ColorScheme) & ".gif"
	End Sub	'Page_Load
End Class

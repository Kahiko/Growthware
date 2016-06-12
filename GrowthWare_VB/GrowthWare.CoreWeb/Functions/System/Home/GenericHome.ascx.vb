Imports GrowthWare.Framework.Web

Public Class GenericHome
	Inherits System.Web.UI.UserControl

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		lblAppName.Text = WebConfigSettings.AppDisplayedName
		SideImage.ImageUrl = ResolveUrl("~/Public/Images/GrowthWare/Misc/sidebar_blue.gif")
		'SideImage.ImageUrl = WebConfigSettings.ImagePath & "sidebar_" & ClientChoicesState(MClientChoices.ColorScheme) & ".gif"
	End Sub

End Class
Imports GrowthWare.Framework.Common
Public Class Home
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblAppName.Text = ConfigSettings.AppDisplayedName
        SideImage.ImageUrl = ResolveUrl("~/Content/GrowthWare/Images/Misc/sidebar_blue.gif")
    End Sub

End Class
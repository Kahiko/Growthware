Imports GrowthWare.Framework.Common

Public Class GenericHome
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblAppName.Text = ConfigSettings.AppDisplayedName
        SideImage.ImageUrl = ResolveUrl("~/Public/GrowthWare/Images/Misc/sidebar_blue.gif")
    End Sub

End Class
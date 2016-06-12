Imports ApplicationBase.Common.Globals

Partial Class UpdateSession
    Inherits System.Web.UI.UserControl

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim myAccountUtility As New AccountUtility(HttpContext.Current)
        myAccountUtility.RemoveAccountInMemoryInformation()
        NavControler.NavTo("Home" & BaseSettings.GetURL)
    End Sub
End Class
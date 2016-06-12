Imports ApplicationBase.Common.Globals

Partial Class SetForceHttps
    Inherits System.Web.UI.UserControl

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        BaseHelperOld.SetDropSelection(dropForceHTTPS, BaseSettings.ForceHTTPS)
    End Sub

    Private Sub btnGo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGo.Click
        BaseSettings.ForceHTTPS = CBool(dropForceHTTPS.SelectedValue)
    End Sub
End Class

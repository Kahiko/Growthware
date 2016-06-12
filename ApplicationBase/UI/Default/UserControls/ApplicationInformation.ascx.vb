Imports ApplicationBase.Common.Globals

Partial Class ApplicationInformation
    Inherits System.Web.UI.UserControl

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lblAppName.Text = BaseSettings.appDisplayedName
        lblEnvironment.Text = BaseSettings.environment
        lblVersion.Text = BaseSettings.verison
    End Sub 'Page_Load
End Class

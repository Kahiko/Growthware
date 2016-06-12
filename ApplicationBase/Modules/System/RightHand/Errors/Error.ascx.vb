Imports ApplicationBase.Common.Globals

Partial Class _Error
    Inherits System.Web.UI.UserControl

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            Dim Ex As Exception = BaseHelperOld.ExceptionError
            litCurrentTime.Text = Format(Now, "HH:mm - MM/dd/yyyy")
            litRequestedPageError.Text = Request.QueryString("ReturnURL")
            litErrorSource.Text = Ex.Source
            litErrorMessage.Text = Ex.Message
            BaseHelperOld.ExceptionError = Nothing
        End If
    End Sub 'Page_Load

    Private Sub btnHome_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHome.Click
        If HttpContext.Current.User.Identity.IsAuthenticated Then
            NavControler.NavTo("Home")
        Else
            NavControler.NavTo("GenericHome")
        End If
    End Sub

    Private Sub btnReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReturn.Click
        NavControler.NavTo(Request.QueryString("ReturnURL") & BaseSettings.GetURL)
    End Sub
End Class
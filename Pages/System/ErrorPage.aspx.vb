Public Class ErrorPage
	Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents litRequestedPage As System.Web.UI.WebControls.Literal
	Protected WithEvents litCurrentTime As System.Web.UI.WebControls.Literal
	Protected WithEvents litRequestedPageError As System.Web.UI.WebControls.Literal
	Protected WithEvents litErrorSource As System.Web.UI.WebControls.Literal
	Protected WithEvents litErrorNumber As System.Web.UI.WebControls.Literal
	Protected WithEvents litErrorMessage As System.Web.UI.WebControls.Literal
	Protected WithEvents btnReturn As System.Web.UI.WebControls.Button
	Protected WithEvents btnHome As System.Web.UI.WebControls.Button

	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object

	Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim Ex As Exception = BaseHelper.ExceptionError
		litCurrentTime.Text = Format(Now, "HH:mm - MM/dd/yyyy")
		litRequestedPageError.Text = Request.QueryString("Action")
		litErrorSource.Text = Ex.Source
		litErrorNumber.Text = Err.Number
		litErrorMessage.Text = Ex.Message
	End Sub	'Page_Load

	Private Sub btnReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReturn.Click
		NavControler.NavTo(Request.QueryString("ReturnURL"))
	End Sub	'btnReturn_Click

	Private Sub btnHome_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHome.Click
		If HttpContext.Current.User.Identity.IsAuthenticated Then
			NavControler.NavTo("Home")
		Else
			NavControler.NavTo("GenericHome")
		End If
	End Sub	'btnHome_Click
End Class
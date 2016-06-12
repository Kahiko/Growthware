Public Class StatesYears
    Inherits System.Web.UI.UserControl

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents litState As System.Web.UI.WebControls.Literal
	Protected WithEvents txtCurrentYear As System.Web.UI.WebControls.TextBox
	Protected WithEvents txtYear As System.Web.UI.WebControls.TextBox
	Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
	Protected WithEvents btnAdd As System.Web.UI.WebControls.Button
	Protected WithEvents txtNewYear As System.Web.UI.WebControls.TextBox
	Protected WithEvents txtNewConnectionString As System.Web.UI.WebControls.TextBox
	Protected WithEvents Requiredfieldvalidator2 As System.Web.UI.WebControls.RequiredFieldValidator
	Protected WithEvents pnlAddYear As System.Web.UI.WebControls.Panel

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
        'Put user code to initialize the page here
		'Response.Write(BStates.GetYearInfoJavaScriptArray(Request.QueryString("id")))
	End Sub	' Page_Load

End Class

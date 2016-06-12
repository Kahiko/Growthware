Imports BLL.Base.ClientChoices

Public Class RandomNumbers
	Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents txtMaxNumber As System.Web.UI.WebControls.TextBox
	Protected WithEvents txtMinNumber As System.Web.UI.WebControls.TextBox
	Protected WithEvents txtAmountOfNumbers As System.Web.UI.WebControls.TextBox
	Protected WithEvents litResults As System.Web.UI.WebControls.Literal
	Protected WithEvents btnSubmit As System.Web.UI.WebControls.Button

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
	End Sub

	Private Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
		Dim Numbers As String = String.Empty
		Dim X As Integer = 1
		For X = 1 To CInt(txtAmountOfNumbers.Text)
			Numbers &= BaseHelper.GetRandomNumber(CInt(txtMaxNumber.Text), CInt(txtMinNumber.Text)) & ", "
		Next
		litResults.Text &= Left(Numbers, Len(Numbers) - 2) & "<br>"
	End Sub
End Class

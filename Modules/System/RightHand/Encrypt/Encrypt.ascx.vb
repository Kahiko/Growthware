Imports Common.Security.BaseSecurity

Public Class Encrypt
	Inherits System.Web.UI.UserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents txtToEncrypt As System.Web.UI.WebControls.TextBox
	Protected WithEvents EncryptedResults As System.Web.UI.WebControls.Literal
	Protected WithEvents cmdEncrypt As System.Web.UI.WebControls.Button
	Protected WithEvents cmdDecrypt As System.Web.UI.WebControls.Button

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

	Private Sub cmdEncrypt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEncrypt.Click
        If Trim(txtToEncrypt.Text).Length > 0 Then
            Dim myCryptoUtil As New CryptoUtil
            EncryptedResults.Text = myCryptoUtil.EncryptTripleDES(txtToEncrypt.Text)
        End If
	End Sub

	Private Sub cmdDecrypt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDecrypt.Click
        If Trim(txtToEncrypt.Text).Length > 0 Then
            Dim myCryptoUtil As New CryptoUtil
            EncryptedResults.Text = myCryptoUtil.DecryptTripleDES(txtToEncrypt.Text)
        End If
	End Sub
End Class

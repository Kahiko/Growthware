Imports ApplicationBase.Common.Security

Partial Class Encrypt
	Inherits System.Web.UI.UserControl

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

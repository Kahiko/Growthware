Partial Class GUIDHelper
	Inherits System.Web.UI.UserControl

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
	End Sub

	Private Sub btnGUID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGUID.Click
		Dim MyGuid As Guid = Guid.NewGuid()
		litOutput.Text = MyGuid.ToString.ToUpper
	End Sub
End Class

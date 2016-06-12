Partial Class UnknownRequest
	Inherits System.Web.UI.UserControl

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
		If Not IsPostBack Then
			Try
				requesedAction.InnerHtml = Request.QueryString("Action").ToString
			Catch ex As Exception

			End Try
		End If
	End Sub

End Class

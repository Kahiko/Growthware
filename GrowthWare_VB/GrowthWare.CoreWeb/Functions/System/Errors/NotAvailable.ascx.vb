Public Class NotAvailable
	Inherits System.Web.UI.UserControl

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		requesedAction.InnerText = Chr(34) & Request.QueryString("Action") & Chr(34)
	End Sub

End Class
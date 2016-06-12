Imports ApplicationBase.Model.Modules

Partial Class NotAvailable
	Inherits ClientChoices.ClientChoicesUserControl

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim ModuleProfileInfo As MModuleProfileInfo
		ModuleProfileInfo = AppModulesUtility.GetModuleInfoByAction(Request.QueryString("Action"))
		requesedAction.InnerText = Chr(34) & ModuleProfileInfo.Description & Chr(34)
	End Sub
End Class
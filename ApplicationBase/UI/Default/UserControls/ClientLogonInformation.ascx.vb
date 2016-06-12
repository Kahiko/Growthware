Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Model.Special.ClientChoices

Partial Class ClientLogonInformation
	Inherits ClientChoices.ClientChoicesUserControl

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		If Context.User.Identity.IsAuthenticated Then
			lblAccount.Text = ClientChoicesState(MClientChoices.AccountName)
            Label3.Text = Label3.Text.Replace("BusinessUnitTranslation", BaseSettings.BusinessUnitTranslation)
			lblSelectedBusinessUnitName.Text = ClientChoicesState(MClientChoices.BusinessUnitName)
		Else
			trClientSecurityInformation.Visible = False
		End If
	End Sub
End Class
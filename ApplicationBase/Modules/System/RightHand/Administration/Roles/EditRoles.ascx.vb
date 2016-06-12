Imports ApplicationBase.BusinessLogic
Imports ApplicationBase.Model.Accounts.Security
Imports ApplicationBase.Model.Special.ClientChoices
Imports ApplicationBase.Common.CustomWebControls

Partial Class EditRoles
	Inherits ClientChoices.ClientChoicesUserControl

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
		Dim myAccountUtility As New AccountUtility(HttpContext.Current)
		Dim accountSecurityInfo As New MAccountSecurityInfo
		myAccountUtility.GetAccountSecurityInfo(accountSecurityInfo)
		If Not accountSecurityInfo.MayEdit Then btnSave.Visible = False
		litRole.Text = BRoles.GetRoleNameByID(Request.QueryString("ID"))
		ctlMembers.DataSource = BRoles.GetAllAccountsNotInRoleByBusinessUnit(Request.QueryString("ID"), ClientChoicesState(MClientChoices.BusinessUnitID))
		ctlMembers.SelectedItems = BRoles.GetAllAccountsForRoleByBusinessUnit(Request.QueryString("ID"), ClientChoicesState(MClientChoices.BusinessUnitID)).ToArray(Type.GetType("System.String"))
		ctlMembers.DataBind()
	End Sub

	Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
		Dim update As Boolean = False
		Dim myAccountUtility As New AccountUtility(HttpContext.Current)
		update = BRoles.UpdateAllAccountsForRoleByBusinessUnit(Request.QueryString("ID"), ClientChoicesState(MClientChoices.BusinessUnitID), ctlMembers.SelectedItems)
		If Not update Then
			Throw New Exception("Error Updating Role")
		End If
		myAccountUtility.RemoveCachedAccounts(ClientChoicesState(MClientChoices.BusinessUnitID))
	End Sub
End Class
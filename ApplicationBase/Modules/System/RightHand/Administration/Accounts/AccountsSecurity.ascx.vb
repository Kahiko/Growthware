Imports ApplicationBase.BusinessLogic
Imports ApplicationBase.Common.CustomWebControls
Imports ApplicationBase.Model.Accounts.Security
Imports ApplicationBase.Model.Modules
Imports ApplicationBase.Model.Accounts
Imports ApplicationBase.Model.Special.ClientChoices

Partial Class AccountsSecurity
	Inherits ClientChoices.ClientChoicesUserControl

	Public ReadOnly Property Roles() As String()
		Get
			Return ctlRoles.SelectedItems
		End Get
	End Property

	Public ReadOnly Property RolesChanged() As Boolean
		Get
			Return ctlRoles.Changed
		End Get
	End Property

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
		If Not IsPostBack Then
			Dim myAccountUtility As New AccountUtility(HttpContext.Current)
			Dim currentAccountName As String = Context.User.Identity.Name
			Dim accountProfileInfo As MAccountProfileInfo
			Dim moduleProfileInfo As MModuleProfileInfo = AppModulesUtility.GetCurrentModule
			Dim accountSecurityInfo As New MAccountSecurityInfo
			myAccountUtility.GetAccountSecurityInfo(accountSecurityInfo)
			Dim AccountType As Integer = 0
			' Get roles
			If Request.QueryString("Account") Is Nothing Then
				accountProfileInfo = myAccountUtility.GetAccountProfileInfo(currentAccountName)
			Else
				If Not Request.QueryString("Account").ToLower = "new" Then
					accountProfileInfo = myAccountUtility.GetAccountProfileInfo(Request.QueryString("Account"), True)
				Else
					accountProfileInfo = myAccountUtility.GetAccountProfileInfo(currentAccountName, True)
				End If
			End If
			ctlRoles.SelectedItems = BAccount.GetRolesFromDBByBusinessUnitID(accountProfileInfo.ACCOUNT_SEQ_ID, ClientChoicesState(MClientChoices.BusinessUnitID))
			If accountSecurityInfo.IsSystemAdministrator Then AccountType = 1
			ctlRoles.DataSource = BusinessUnitUtility.GetAllRolesForBusinessUnit(ClientChoicesState(MClientChoices.BusinessUnitID)).Tables(0).DefaultView
			ctlRoles.DataField = "Role_Name"
			ctlRoles.DataBind()
			' need to reset the cached client profile information back to the currently logged on
			' account
			accountProfileInfo = myAccountUtility.GetAccountProfileInfo(currentAccountName, True)
		End If
	End Sub
End Class
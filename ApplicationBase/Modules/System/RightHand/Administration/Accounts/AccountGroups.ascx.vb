Imports ApplicationBase.BusinessLogic
Imports ApplicationBase.Common.CustomWebControls
Imports ApplicationBase.Model.Accounts.Security
Imports ApplicationBase.Model.Modules
Imports ApplicationBase.Model.Accounts
Imports ApplicationBase.Model.Special.ClientChoices
Imports System.Data

Partial Class AccountGroups
	Inherits ClientChoices.ClientChoicesUserControl

	Public ReadOnly Property Groups() As String()
		Get
			Return ctlGroups.SelectedItems
		End Get
	End Property

	Public ReadOnly Property GroupsChanged() As Boolean
		Get
			Return ctlGroups.Changed
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
			' Get Groups
			If Request.QueryString("Account") Is Nothing Then
				accountProfileInfo = myAccountUtility.GetAccountProfileInfo(currentAccountName)
			Else
				If Not Request.QueryString("Account").ToLower = "new" Then
					accountProfileInfo = myAccountUtility.GetAccountProfileInfo(Request.QueryString("Account"), True)
				Else
					accountProfileInfo = myAccountUtility.GetAccountProfileInfo(currentAccountName, True)
				End If
			End If
			ctlGroups.SelectedItems = BAccount.GetGroupsFromDBByBusinessUnitID(accountProfileInfo.ACCOUNT_SEQ_ID, ClientChoicesState(MClientChoices.BusinessUnitID))
			If accountSecurityInfo.IsSystemAdministrator Then AccountType = 1
			Dim myDataView As DataView = BusinessUnitUtility.GetAllGroupsForBusinessUnit(ClientChoicesState(MClientChoices.BusinessUnitID)).Tables(0).DefaultView
			ctlGroups.DataSource = myDataView
			ctlGroups.DataField = "GROUP_NAME"
			ctlGroups.DataBind()
			' need to reset the cached client profile information back to the currently logged on
			' account
			accountProfileInfo = myAccountUtility.GetAccountProfileInfo(currentAccountName, True)
		End If
	End Sub
End Class

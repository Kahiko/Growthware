Imports BLL.Base.ClientChoices
Imports BLL.Special
Imports Common.CustomWebControls
Imports DALModel.Base.Accounts.Security
Imports DALModel.Base.Modules
Imports DALModel.Special.Accounts
Imports DALModel.Special.ClientChoices

Public Class AccountRoles
	Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents ctlRoles As Common.CustomWebControls.ListPicker

	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object

	Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

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
			Dim currentAccountName As String = context.User.Identity.Name
			Dim accountProfileInfo As MAccountProfileInfo
			Dim moduleProfileInfo As MModuleProfileInfo = AppModulesUtility.GetCurrentModule
			Dim accountSecurityInfo As MAccountSecurityInfo = myAccountUtility.GetAccountSecurityInfo(accountSecurityInfo)
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

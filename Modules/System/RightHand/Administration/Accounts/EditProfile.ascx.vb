Imports BLL.Special
Imports BLL.Base.ClientChoices
Imports Common.CustomWebControls
Imports DALModel.Base.Accounts.Security
Imports DALModel.Base.Modules
Imports DALModel.Special.Accounts
Imports DALModel.Special.ClientChoices
Imports System.Web.Security


Public Class EditProfile
    Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents chkActive As System.Web.UI.WebControls.CheckBox
	Protected WithEvents Tabstrip1 As TabStrip
	Protected WithEvents pnlGeneral As System.Web.UI.WebControls.Panel
	Protected WithEvents bottomTabStrip As System.Web.UI.HtmlControls.HtmlTableCell
	Protected WithEvents btnSave As System.Web.UI.WebControls.Button
	Protected WithEvents litAccountName As System.Web.UI.WebControls.Literal
	Protected WithEvents pnlRoles As System.Web.UI.WebControls.Panel
	Protected WithEvents pnlGroups As System.Web.UI.WebControls.Panel

	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object

	Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

	Protected WithEvents AccountsGeneral As AccountsGeneral
	Protected WithEvents AccountRoles As AccountRoles
	Protected WithEvents AccountGroups As AccountGroups

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim myAccountUtility As New AccountUtility(HttpContext.Current)
		Dim accountSecurityInfo As MAccountSecurityInfo = myAccountUtility.GetAccountSecurityInfo(accountSecurityInfo)
		Dim targetaccountProfileInfo As MAccountProfileInfo
		If Not IsPostBack Then
			' get the client security information
			If Not accountSecurityInfo.MayEdit And Not accountSecurityInfo.MayAdd Then btnSave.Visible = False
			bottomTabStrip.BgColor = ClientChoicesState(MClientChoices.HeadColor)
			Dim rolesModuleProfileInfo As MModuleProfileInfo = AppModulesUtility.GetModuleInfoByAction("RoleTab")
			Dim groupsModuleProfileInfo As MModuleProfileInfo = AppModulesUtility.GetModuleInfoByAction("GroupTab")
			Dim myTab As Common.CustomWebControls.Tab
			myTab = Tabstrip1.Tabs("RolesTab")
			Dim newRole As String = String.Empty
			Dim newRoles As String = String.Empty
			If Not rolesModuleProfileInfo Is Nothing Then
				For Each newRole In rolesModuleProfileInfo.ViewRoles
					newRoles += newRole & ";"
				Next
			End If
			newRoles += newRole & "SysAdmin;"
			If Not newRoles Is Nothing Then
				If Right(newRoles, 1) = ";" Then
					newRoles = Left(newRoles, Len(newRoles) - 1)
				End If
			End If
			myTab.Roles = newRoles
			newRoles = ""
			myTab = Tabstrip1.Tabs("GroupsTab")
			If Not groupsModuleProfileInfo Is Nothing Then
				For Each newRole In groupsModuleProfileInfo.ViewRoles
					newRoles += newRole & ";"
				Next
			End If
			newRoles += newRole & "SysAdmin;"
			If Not newRoles Is Nothing Then
				If Right(newRoles, 1) = ";" Then
					newRoles = Left(newRoles, Len(newRoles) - 1)
				End If
			End If
			myTab.Roles = newRoles
		End If
		If Request.QueryString("Action").ToLower = "editprofile" Then
			If Not Request.QueryString("Account") Is Nothing Then
				targetaccountProfileInfo = myAccountUtility.GetAccountProfileInfo(Request.QueryString("Account"), True)
			Else
				targetaccountProfileInfo = myAccountUtility.GetAccountProfileInfo(context.User.Identity.Name)
			End If
		Else
			targetaccountProfileInfo = New MAccountProfileInfo
		End If
		If targetaccountProfileInfo.ACCOUNT.Trim.Length > 0 Then
			litAccountName.Text = targetaccountProfileInfo.ACCOUNT
		Else
			litAccountName.Text = "New"
		End If
	End Sub

	Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
		Page.Validate()
		If Page.IsValid Then
			Dim myAccountUtility As New AccountUtility(HttpContext.Current)
			Dim myAccountProfileInfo As MAccountProfileInfo
			' Update the general information
			AccountsGeneral.btnSave_Click(sender, e)
			Dim mySeq_id As Integer = AccountsGeneral.UpdatedACCOUNT_SEQ_ID
			' Update Roles
			If AccountRoles.RolesChanged OrElse Request.QueryString("Action").ToLower = "addaccount" Then
				Try
					BAccount.UpdateRoles(AccountsGeneral.UpdatedACCOUNT_SEQ_ID, ClientChoicesState(MClientChoices.BusinessUnitID), AccountRoles.Roles)
				Catch ex As Exception
					Throw New ApplicationException("Error Writing to the DATABASE", ex)
				End Try
			End If
			' Update Groups
			If AccountGroups.GroupsChanged OrElse Request.QueryString("Action").ToLower = "addaccount" Then
				Try
					BAccount.UpdateGroups(AccountsGeneral.UpdatedACCOUNT_SEQ_ID, ClientChoicesState(MClientChoices.BusinessUnitID), AccountGroups.Groups)
				Catch ex As Exception
					Throw New ApplicationException("Error Writing to the DATABASE", ex)
				End Try
			End If
			If Not Request.QueryString("Action").ToLower = "addaccount" Then
				myAccountUtility.RemoveAccountInMemoryInformation()
			End If
			myAccountProfileInfo = myAccountUtility.GetAccountProfileInfo(context.User.Identity.Name, True)
			If myAccountProfileInfo Is Nothing Then
				myAccountUtility.SignOut()
			End If
			myAccountUtility.RemoveAccountInMemoryInformation()
			NavControler.NavTo("EditProfile&Account=" & AccountsGeneral.UpdateAccountName)
		End If
	End Sub
End Class
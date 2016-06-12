Imports BLL.Base.SQLServer
Imports BLL.Base.ClientChoices
Imports DALModel.Base.Accounts.Security
Imports DALModel.Special.ClientChoices
Imports Common.CustomWebControls

Public Class EditRoles
	Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents ctlMembers As ListPicker
	Protected WithEvents btnSave As System.Web.UI.WebControls.Button
	Protected WithEvents litRole As System.Web.UI.WebControls.Literal
	Protected WithEvents NavTrail1 As NavTrail
	Protected WithEvents Navtrail2 As NavTrail

	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object

	Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
		Dim myAccountUtility As New AccountUtility(HttpContext.Current)
		Dim accountSecurityInfo As MAccountSecurityInfo = myAccountUtility.GetAccountSecurityInfo(accountSecurityInfo)
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
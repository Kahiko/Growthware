Imports BLL.Base.SQLServer
Imports BLL.Base.ClientChoices
Imports DALModel.Base.Accounts.Security
Imports DALModel.Special.ClientChoices

Public Class AddEditRoles
	Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents anchorReturnToAdministration As System.Web.UI.HtmlControls.HtmlAnchor
	Protected WithEvents anchorReturnToAdministration2 As System.Web.UI.HtmlControls.HtmlAnchor
	Protected WithEvents dgResults As System.Web.UI.WebControls.DataGrid
	Protected WithEvents txtNewRole As System.Web.UI.WebControls.TextBox
	Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
	Protected WithEvents txtDescription As System.Web.UI.WebControls.TextBox
	Protected WithEvents RequiredFieldValidator2 As System.Web.UI.WebControls.RequiredFieldValidator
	Protected WithEvents btnAdd As System.Web.UI.WebControls.Button
	Protected WithEvents pnlAddRole As System.Web.UI.WebControls.Panel

	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object

	Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

	Private myAccountUtility As New AccountUtility(HttpContext.Current)
	Private accountSecurityInfo As MAccountSecurityInfo = myAccountUtility.GetAccountSecurityInfo(accountSecurityInfo)

	Sub Page_Load(ByVal s As Object, ByVal e As EventArgs) Handles MyBase.Load
		If Not Page.IsPostBack Then
			dgResults.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.HeadColor))
		End If
	End Sub	'Page_Load

	'*******************************************************
	'
	' Perform binding to the DataGrid.
	'
	'*******************************************************
	Sub BindRoles()
        dgResults.DataSource = BusinessUnitUtility.GetAllRolesForBusinessUnit(ClientChoicesState(MClientChoices.BusinessUnitID))
		dgResults.DataKeyField = "ROLE_NAME"
		dgResults.DataBind()
	End Sub	'BindRoles

	'*******************************************************
	'
	' We don't want the delete link to appear
	' next to system roles, so we clear the cell
	' containing the delete link for system roles.
	'
	'*******************************************************
	Private Sub dgResults_ItemDataBound(ByVal s As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgResults.ItemDataBound
		'editmodules
		If e.Item.DataItem Is Nothing Then
			Return
		End If
		Dim isSystem As Boolean = CBool(DataBinder.Eval(e.Item.DataItem, "Is_System"))
		Dim HyperMembers As HyperLink = CType(e.Item.FindControl("hyperMembers"), HyperLink)
		HyperMembers.ImageUrl = BaseHelper.ImagePath & "AdminEdit.gif"
		HyperMembers.NavigateUrl = BaseHelper.FQDNBasePage & String.Format("?action=editroles&id={0}", e.Item.DataItem("ROLE_SEQ_ID"))
        Dim btnDelete As ImageButton = CType(e.Item.FindControl("btnDelete"), ImageButton)
        ' Add confirmation to delete button
        btnDelete.ImageUrl = BaseHelper.ImagePath & "delete.gif"
        btnDelete.Attributes.Add("onclick", String.Format("return confirm('Are you sure you want to delete the ""{0}"" Role?');", CStr(DataBinder.Eval(e.Item.DataItem, "role_name"))))
        If isSystem Then
            e.Item.Cells(3).Controls.Clear()
            Return
        End If
    End Sub 'RoleList_ItemDataBound

    '*******************************************************
    '
    ' A Role has been selected for editing.
    '
    '*******************************************************
    Private Sub dgResults_EditCommand(ByVal s As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgResults.EditCommand
        ' Select row for editing
        dgResults.EditItemIndex = e.Item.ItemIndex
        BindRoles()

        ' Hide add role panel
        pnlAddRole.Visible = False
    End Sub 'RoleList_EditCommand

    '*******************************************************
    '
    ' Editing has been canceled on a role.
    '
    '*******************************************************
    Private Sub dgResults_CancelCommand(ByVal s As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgResults.CancelCommand
        ' UnSelect row for editing
        dgResults.EditItemIndex = -1
        BindRoles()
        ' Show add role panel
        pnlAddRole.Visible = True
    End Sub 'RoleList_CancelCommand

    '*******************************************************
    '
    ' Update the role in the database.
    '
    '*******************************************************
    Private Sub RoleList_UpdateCommand(ByVal s As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgResults.UpdateCommand
        Dim originalRoleName As String = CStr(dgResults.DataKeys(e.Item.ItemIndex))
        Dim newRoleName As String = e.Item.Cells(0).Text
        Dim roleDescription As String = CType(e.Item.Cells(1).Controls(0), TextBox).Text
        ' Update Roles
        If newRoleName.Length > 0 Then
            BRoles.UpdateRole(originalRoleName, newRoleName, roleDescription)
            BusinessUnitUtility.RemoveRoleCache(ClientChoicesState(MClientChoices.BusinessUnitID))
        End If
        ' UnSelect row for editing
        dgResults.EditItemIndex = -1
        BindRoles()

        ' Show add role panel
        pnlAddRole.Visible = True
    End Sub 'RoleList_UpdateCommand

    '*******************************************************
    '
    ' Delete a role from the database.
    '
    '*******************************************************
    Private Sub dgResults_DeleteCommand(ByVal s As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgResults.DeleteCommand
        Dim roleName As String = CStr(dgResults.DataKeys(e.Item.ItemIndex))
        ' Delete Role
        BRoles.DeleteRole(roleName, ClientChoicesState(MClientChoices.BusinessUnitID))
        BusinessUnitUtility.RemoveRoleCache(ClientChoicesState(MClientChoices.BusinessUnitID))
        ' UnSelect row for editing
        dgResults.EditItemIndex = -1
        BindRoles()

        ' Show add role panel
        pnlAddRole.Visible = True
    End Sub 'RoleList_DeleteCommand

    '*******************************************************
    '
    ' Add a new role to the database.
    '
    '*******************************************************
    Public Sub btnAdd_Click(ByVal s As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        BRoles.AddRole(txtNewRole.Text, txtDescription.Text, ClientChoicesState(MClientChoices.BusinessUnitID))
        BusinessUnitUtility.RemoveRoleCache(ClientChoicesState(MClientChoices.BusinessUnitID))
        txtNewRole.Text = ""
        txtDescription.Text = ""
        BindRoles()
    End Sub 'btnAdd_Click

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Dim myAccountUtility As New AccountUtility(HttpContext.Current)
		Dim myAccountSecurityInfo As MAccountSecurityInfo = myAccountUtility.GetAccountSecurityInfo(myAccountSecurityInfo)
        BindRoles()
		If Not myAccountSecurityInfo.MayEdit Then
			dgResults.Columns(3).Visible = False
			dgResults.Columns(5).Visible = False
		End If
		If (Not myAccountSecurityInfo.MayDelete) Then
			dgResults.Columns(4).Visible = False
		End If
		If Not myAccountSecurityInfo.MayAdd Then
			pnlAddRole.Visible = False
		End If
    End Sub
End Class
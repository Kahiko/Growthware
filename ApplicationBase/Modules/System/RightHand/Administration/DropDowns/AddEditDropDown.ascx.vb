Imports ApplicationBase.BusinessLogic
Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Model
Imports ApplicationBase.Model.Special.ClientChoices
Imports ApplicationBase.Model.Accounts

Partial Class AddEditDropDown
	Inherits ClientChoices.ClientChoicesUserControl

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
		If Not IsPostBack Then
			' todo forced business unit id
            Dim colAllRoles As ArrayList = BRoles.GetAllRolesForBusinessUnit(BaseSettings.DefaultBusinessUnitID)
			RolesControl.AllRoles = colAllRoles
			If Not Request.QueryString("ID") Is Nothing Then
				Dim DROP_BOX_SEQ_ID As Integer = CInt(Request.QueryString("ID"))
				Dim BUSINESS_SEQ_ID As Integer = ClientChoicesState(MClientChoices.BusinessUnitID)
				RolesControl.SelectedViewRoles = BRoles.GetDropBoxBusinessUnitSelectedRoles(MRoleType.value.ViewRole, DROP_BOX_SEQ_ID, BUSINESS_SEQ_ID).ToArray(Type.GetType("System.String"))
				RolesControl.SelectedAddRoles = BRoles.GetDropBoxBusinessUnitSelectedRoles(MRoleType.value.AddRole, DROP_BOX_SEQ_ID, BUSINESS_SEQ_ID).ToArray(Type.GetType("System.String"))
				RolesControl.SelectedEditRoles = BRoles.GetDropBoxBusinessUnitSelectedRoles(MRoleType.value.EditRole, DROP_BOX_SEQ_ID, BUSINESS_SEQ_ID).ToArray(Type.GetType("System.String"))
				RolesControl.SelectedDeleteRoles = BRoles.GetDropBoxBusinessUnitSelectedRoles(MRoleType.value.DeleteRole, DROP_BOX_SEQ_ID, BUSINESS_SEQ_ID).ToArray(Type.GetType("System.String"))
			End If
			bottomTabStrip.BgColor = ClientChoicesState(MClientChoices.HeadColor)
		End If
	End Sub

	Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) ' Handles btnSave.Click
		Dim needModuleRebuild As Boolean = False
        Dim SelectedBusinessUnitID As Integer = BaseSettings.defaultBusinessUnitID
		DropDownGeneral.btnSave_Click(sender, e)
		Dim DROP_BOX_SEQ_ID As Integer = DropDownGeneral.DROP_BOX_SEQ_ID
		' Update View Roles
		If RolesControl.ViewRolesChanged Then
			BDropBox.AddDropBoxRoles(DROP_BOX_SEQ_ID, SelectedBusinessUnitID, MRoleType.value.ViewRole, RolesControl.ViewRoles)
			needModuleRebuild = True
		End If
		' Update Add Roles
		If RolesControl.AddRolesChanged Then
			BDropBox.AddDropBoxRoles(DROP_BOX_SEQ_ID, SelectedBusinessUnitID, MRoleType.value.AddRole, RolesControl.AddRoles)
			needModuleRebuild = True
		End If
		' Update Edit Roles
		If RolesControl.EditRolesChanged Then
			BDropBox.AddDropBoxRoles(DROP_BOX_SEQ_ID, SelectedBusinessUnitID, MRoleType.value.EditRole, RolesControl.EditRoles)
			needModuleRebuild = True
		End If
		' Update Delete Roles
		If RolesControl.DeleteRolesChanged Then
			BDropBox.AddDropBoxRoles(DROP_BOX_SEQ_ID, SelectedBusinessUnitID, MRoleType.value.DeleteRole, RolesControl.DeleteRoles)
			needModuleRebuild = True
		End If
	End Sub
End Class

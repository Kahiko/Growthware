Imports ApplicationBase.Common.CustomWebControls
Imports ApplicationBase.BusinessLogic
Imports ApplicationBase.Model
Imports ApplicationBase.Model.Accounts.Security
Imports ApplicationBase.Model.Modules
Imports ApplicationBase.Model.Special.ClientChoices

Partial Class AddEditModules
	Inherits ClientChoices.ClientChoicesUserControl

	Public Module_Seq_Id As Integer
	Public State As String

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		If Not IsPostBack Then
			If Not Request.QueryString("id") Is Nothing Then
				Dim myAccountUtility As New AccountUtility(HttpContext.Current)
				' Assign Module Sequence ID
				Module_Seq_Id = Int32.Parse(Request.QueryString("id"))
				' Get a module profile
				Dim objModuleProfileInfo As MModuleProfileInfo
				' populate the module profile with specific infromation
				objModuleProfileInfo = AppModulesUtility.GetModulesByID(Module_Seq_Id)
				' Assign the Module values
				ModulesGeneral.moduleProfileInfo = objModuleProfileInfo
				RolesControl.SelectedViewRoles = BRoles.GetModuleBusinessUnitSelectedRoles(MRoleType.value.ViewRole, objModuleProfileInfo.MODULE_SEQ_ID, ClientChoicesState(MClientChoices.BusinessUnitID)).ToArray(Type.GetType("System.String"))
				RolesControl.SelectedAddRoles = BRoles.GetModuleBusinessUnitSelectedRoles(MRoleType.value.AddRole, objModuleProfileInfo.MODULE_SEQ_ID, ClientChoicesState(MClientChoices.BusinessUnitID)).ToArray(Type.GetType("System.String"))
				RolesControl.SelectedEditRoles = BRoles.GetModuleBusinessUnitSelectedRoles(MRoleType.value.EditRole, objModuleProfileInfo.MODULE_SEQ_ID, ClientChoicesState(MClientChoices.BusinessUnitID)).ToArray(Type.GetType("System.String"))
				RolesControl.SelectedDeleteRoles = BRoles.GetModuleBusinessUnitSelectedRoles(MRoleType.value.DeleteRole, objModuleProfileInfo.MODULE_SEQ_ID, ClientChoicesState(MClientChoices.BusinessUnitID)).ToArray(Type.GetType("System.String"))

				GroupsControl.SelectedViewGroups = BRoles.GetModuleBusinessUnitSelectedgroups(MRoleType.value.ViewRole, objModuleProfileInfo.MODULE_SEQ_ID, ClientChoicesState(MClientChoices.BusinessUnitID)).ToArray(Type.GetType("System.String"))
				GroupsControl.SelectedAddGroups = BRoles.GetModuleBusinessUnitSelectedGroups(MRoleType.value.AddRole, objModuleProfileInfo.MODULE_SEQ_ID, ClientChoicesState(MClientChoices.BusinessUnitID)).ToArray(Type.GetType("System.String"))
				GroupsControl.SelectedEditGroups = BRoles.GetModuleBusinessUnitSelectedGroups(MRoleType.value.EditRole, objModuleProfileInfo.MODULE_SEQ_ID, ClientChoicesState(MClientChoices.BusinessUnitID)).ToArray(Type.GetType("System.String"))
				GroupsControl.SelectedDeleteGroups = BRoles.GetModuleBusinessUnitSelectedGroups(MRoleType.value.DeleteRole, objModuleProfileInfo.MODULE_SEQ_ID, ClientChoicesState(MClientChoices.BusinessUnitID)).ToArray(Type.GetType("System.String"))

				litModuleName.Text = objModuleProfileInfo.Name
				' Initialize roles
				myAccountUtility.GetAllAccountRoles()
				Dim accountSecurityInfo As New MAccountSecurityInfo
				myAccountUtility.GetAccountSecurityInfo(accountSecurityInfo)
				Dim colAllRoles As ArrayList = BRoles.GetAllRolesForBusinessUnit(ClientChoicesState(MClientChoices.BusinessUnitID))
				' get all roles
				RolesControl.AllRoles = colAllRoles
				'get all groups
				colAllRoles = BRoles.GetAllGroupsForBusinessUnit(ClientChoicesState(MClientChoices.BusinessUnitID))
				GroupsControl.AllGroups = colAllRoles

				Dim rolesModuleProfileInfo As MModuleProfileInfo = AppModulesUtility.GetModuleInfoByAction("RoleTab")
				Dim groupsModuleProfileInfo As MModuleProfileInfo = AppModulesUtility.GetModuleInfoByAction("GroupTab")
				Dim myTab As ApplicationBase.Common.CustomWebControls.Tab
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
			Else
				' ensure that the correct business unit has been selected
				If CInt(ClientChoicesState(MClientChoices.BusinessUnitID)) <> 1 Then
                    BaseHelperOld.SelectBusinessUnit()
				End If
				' Initialize roles
				Dim colAllRoles As ArrayList = BRoles.GetAllRoles
				RolesControl.AllRoles = colAllRoles
			End If
			bottomTabStrip.BgColor = ClientChoicesState(MClientChoices.HeadColor)
		End If
	End Sub	' Page_Load

	'*********************************************************************
	'
	' UpdateModules Method
	'
	' Updates the module in question as well as
	' updating them module cache collection.
	'
    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Page.Validate()
        If Page.IsValid Then
            ' Update the general information
            ModulesGeneral.btnSave_Click(sender, e)
            Dim moduleProfileInfo As New MModuleProfileInfo
            Dim mySeq_id As Integer = ModulesGeneral.UpdatedMODULE_SEQ_ID
            moduleProfileInfo = AppModulesUtility.GetModulesByID(mySeq_id)
            Dim SelectedBusinessUnitID As String = ClientChoicesState(MClientChoices.BusinessUnitID)
            Dim Module_Seq_Id As Integer = moduleProfileInfo.MODULE_SEQ_ID
            ' Update View Roles
            If RolesControl.ViewRolesChanged Then
                BAppModules.AddModuleRoles(Module_Seq_Id, SelectedBusinessUnitID, MRoleType.value.ViewRole, RolesControl.ViewRoles)
            End If
            ' Update Add Roles
            If RolesControl.AddRolesChanged Then
                BAppModules.AddModuleRoles(Module_Seq_Id, SelectedBusinessUnitID, MRoleType.value.AddRole, RolesControl.AddRoles)
            End If
            ' Update Edit Roles
            If RolesControl.EditRolesChanged Then
                BAppModules.AddModuleRoles(Module_Seq_Id, SelectedBusinessUnitID, MRoleType.value.EditRole, RolesControl.EditRoles)
            End If
            ' Update Delete Roles
            If RolesControl.DeleteRolesChanged Then
                BAppModules.AddModuleRoles(Module_Seq_Id, SelectedBusinessUnitID, MRoleType.value.DeleteRole, RolesControl.DeleteRoles)
            End If

            ' Update View Groups
            If GroupsControl.ViewGroupsChanged Then
                BAppModules.AddModuleGroups(Module_Seq_Id, SelectedBusinessUnitID, MGroupType.value.ViewRole, GroupsControl.ViewGroups)
            End If
            ' Update Add Groups
            If GroupsControl.AddGroupsChanged Then
                BAppModules.AddModuleGroups(Module_Seq_Id, SelectedBusinessUnitID, MGroupType.value.AddRole, GroupsControl.AddGroups)
            End If
            ' Update Edit Groups
            If GroupsControl.EditGroupsChanged Then
                BAppModules.AddModuleGroups(Module_Seq_Id, SelectedBusinessUnitID, MGroupType.value.EditRole, GroupsControl.EditGroups)
            End If
            ' Update Delete Groups
            If GroupsControl.DeleteGroupsChanged Then
                BAppModules.AddModuleGroups(Module_Seq_Id, SelectedBusinessUnitID, MGroupType.value.DeleteRole, GroupsControl.DeleteGroups)
            End If

            AppModulesUtility.ReBuildModuleCollection()
            Module_Seq_Id = moduleProfileInfo.MODULE_SEQ_ID
            NavControler.NavTo("AddEditModules&id=" & moduleProfileInfo.MODULE_SEQ_ID)
        End If
    End Sub
End Class
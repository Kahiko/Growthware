Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Factory
Imports ApplicationBase.Interfaces
Imports System.Runtime.InteropServices

Public Class BRoles
	'Private Shared iBaseDAL As IRoles = FRoles.Create(Configuration.ConfigurationManager.AppSettings("BaseDAL"))
    Private Shared iBaseDAL As IRoles = FactoryObject.Create(BaseSettings.applicationBaseDAL, "DRoles")

	Public Shared Sub AddRole(ByVal roleName As String, ByVal description As String, ByVal BUSINESS_UNIT_SEQ_ID As Integer, Optional ByVal Account_seq_Id As Integer = 1)
		iBaseDAL.AddRole(roleName, description, BUSINESS_UNIT_SEQ_ID, Account_seq_Id)
	End Sub

	Public Shared Sub DeleteRole(ByVal roleName As String, ByVal BUSINESS_UNIT_SEQ_ID As Integer, Optional ByVal Account_seq_Id As Integer = 1)
		iBaseDAL.DeleteRole(roleName, BUSINESS_UNIT_SEQ_ID, Account_seq_Id)
	End Sub

	Public Shared Sub UpdateRole(ByVal originalRoleName As String, ByVal newRoleName As String, ByVal newRoleDescription As String, Optional ByVal Account_seq_Id As Integer = 1)
		iBaseDAL.UpdateRole(originalRoleName, newRoleName, newRoleDescription, Account_seq_Id)
	End Sub

	Public Shared Function GetAllRoles() As ArrayList
		Return iBaseDAL.GetAllRoles()
	End Function

	Public Shared Function GetAllRolesForBusinessUnit(ByVal BUSINESS_UNIT_SEQ_ID As Integer) As ArrayList
		' todo this needs to change some code is getting role information form the 
		' BusinessUnit interface all access to roles should be done here
		Return iBaseDAL.GetRolesForBusinessUnit(BUSINESS_UNIT_SEQ_ID)
	End Function

	Public Shared Function GetAllGroupsForBusinessUnit(ByVal BUSINESS_UNIT_SEQ_ID As Integer) As ArrayList
		' todo this needs to change some code is getting role information form the 
		' BusinessUnit interface all access to roles should be done here
		Return iBaseDAL.GetGroupsForBusinessUnit(BUSINESS_UNIT_SEQ_ID)
	End Function


	Public Shared Function GetModuleBusinessUnitSelectedRoles(ByVal RoleType As Integer, ByVal MODULE_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As ArrayList
		Return iBaseDAL.GetModuleBusinessUnitSelectedRoles(RoleType, MODULE_SEQ_ID, BUSINESS_UNIT_SEQ_ID)
	End Function

	Public Shared Function GetModuleBusinessUnitSelectedGroups(ByVal GroupType As Integer, ByVal MODULE_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As ArrayList
		Return iBaseDAL.GetModuleBusinessUnitSelectedGroups(GroupType, MODULE_SEQ_ID, BUSINESS_UNIT_SEQ_ID)
	End Function


	Public Shared Function GetDropBoxBusinessUnitSelectedRoles(ByVal RoleType As Integer, ByVal DROP_BOX_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As ArrayList
		Return iBaseDAL.GetDropBoxBusinessUnitSelectedRoles(RoleType, DROP_BOX_SEQ_ID, BUSINESS_UNIT_SEQ_ID)
	End Function

	Public Shared Function GetAllAccountsForRoleByBusinessUnit(ByVal ROLE_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As ArrayList
		Return iBaseDAL.GetAllAccountsForRoleByBusinessUnit(ROLE_SEQ_ID, BUSINESS_UNIT_SEQ_ID)
	End Function

	Public Shared Function GetAllAccountsNotInRoleByBusinessUnit(ByVal ROLE_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As ArrayList
		Return iBaseDAL.GetAllAccountsNotInRoleByBusinessUnit(ROLE_SEQ_ID, BUSINESS_UNIT_SEQ_ID)
	End Function

	Public Shared Function UpdateAllAccountsForRoleByBusinessUnit(ByVal ROLE_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer, ByVal Accounts As String()) As Boolean
		Return iBaseDAL.UpdateAllAccountsForRoleByBusinessUnit(ROLE_SEQ_ID, BUSINESS_UNIT_SEQ_ID, Accounts)
	End Function

	Public Shared Function GetRoleNameByID(ByVal ROLE_SEQ_ID As Integer) As String
		Return iBaseDAL.GetRoleNameByID(ROLE_SEQ_ID)
	End Function
End Class
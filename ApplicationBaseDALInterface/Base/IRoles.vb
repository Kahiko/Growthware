Public Interface IRoles
	Sub AddRole(ByVal roleName As String, ByVal description As String, ByVal BUSINESS_UNIT_SEQ_ID As Integer, Optional ByVal Account_Seq_id As Integer = 1)
	Function GetAllRoles() As ArrayList
	Function GetRolesForBusinessUnit(ByVal BUSINESS_UNIT_SEQ_ID As Integer) As ArrayList

	Function GetGroupsForBusinessUnit(ByVal BUSINESS_UNIT_SEQ_ID As Integer) As ArrayList

	Function GetModuleBusinessUnitSelectedRoles(ByVal RoleType As Integer, ByVal MODULE_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As ArrayList

	Function GetModuleBusinessUnitSelectedGroups(ByVal GroupType As Integer, ByVal MODULE_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As ArrayList

	Function GetDropBoxBusinessUnitSelectedRoles(ByVal RoleType As Integer, ByVal DROP_BOX_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As ArrayList
	Function GetAllAccountsForRoleByBusinessUnit(ByVal ROLE_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As ArrayList
	Function GetAllAccountsNotInRoleByBusinessUnit(ByVal ROLE_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As ArrayList
	Function UpdateAllAccountsForRoleByBusinessUnit(ByVal ROLE_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer, ByVal Accounts As String(), Optional ByVal Account_Seq_id As Integer = 1) As Boolean
	Function GetRoleNameByID(ByVal ROLE_SEQ_ID As Integer) As String
	Sub DeleteRole(ByVal roleName As String, ByVal BUSINESS_UNIT_SEQ_ID As Integer, Optional ByVal Account_seq_Id As Integer = 1)
	Sub UpdateRole(ByVal originalRoleName As String, ByVal newRoleName As String, ByVal newRoleDescription As String, Optional ByVal Account_seq_Id As Integer = 1)
End Interface
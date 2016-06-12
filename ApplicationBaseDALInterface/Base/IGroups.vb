Imports ApplicationBase.Model.Group

Public Interface IGroups
	Function AddGroup(ByRef GroupInfo As MGroupInfo, Optional ByVal Account_Seq_id As Integer = 1) As Integer
	Function SearchGroups(ByVal GroupName As String, ByVal BusinessUnitSeqId As Integer) As DataSet
	Function GetGroupInfo(ByVal GroupId As Integer) As MGroupInfo
	Function GetRolesForGroup(ByVal GroupId As Integer, Optional ByVal BusinessUnitSeqId As Integer = 1) As String()
	Function GetRolesByBusinessUnit(ByVal GroupId As Integer, ByVal BusinessUnitSeqId As Integer) As String()
	Sub UpdateRoles(ByVal GroupId As Integer, ByVal BusinessUnitSeqId As Integer, ByVal roles() As String, Optional ByVal Account_Seq_id As Integer = 1)
	Function UpdateAGroup(ByVal GroupInfo As MGroupInfo, Optional ByVal Account_Seq_id As Integer = 1) As Boolean
	Sub DeleteGroup(ByVal GroupSeqId As String, ByVal BusinessUnitSeqId As Integer, Optional ByVal Account_Seq_Id As Integer = 1)
End Interface
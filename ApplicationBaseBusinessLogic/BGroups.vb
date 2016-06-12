Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Factory
Imports ApplicationBase.Interfaces
Imports ApplicationBase.Model.Group
Imports System.Runtime.InteropServices

Public Class BGroups
	'Private Shared iBaseDAL As IGroups = FGroups.Create(Configuration.ConfigurationManager.AppSettings("BaseDAL"))
    Private Shared iBaseDAL As IGroups = FactoryObject.Create(BaseSettings.applicationBaseDAL, "DGroups")

	Public Shared Function AddGroup(ByRef GroupInfo As MGroupInfo, Optional ByVal Account_seq_Id As Integer = 1) As Integer
		Return iBaseDAL.AddGroup(GroupInfo, Account_seq_Id)
	End Function
	Public Shared Function SearchGroups(ByVal GroupName As String, ByVal BusinessUnitSeqId As Integer) As DataSet
		Return iBaseDAL.SearchGroups(GroupName, BusinessUnitSeqId)
	End Function
	Public Shared Function GetGroupInfo(ByVal GroupId As Integer) As MGroupInfo
		Return iBaseDAL.GetGroupInfo(GroupId)
	End Function
	Public Shared Function GetRolesForGroup(ByVal GroupId As Integer, Optional ByVal BusinessUnitSeqId As Integer = 1) As String()
		Return iBaseDAL.GetRolesForGroup(GroupId)
	End Function
	Public Shared Function GetRolesByBusinessUnit(ByVal GroupId As Integer, ByVal BusinessUnitSeqId As Integer) As String()
		Return iBaseDAL.GetRolesByBusinessUnit(GroupId, BusinessUnitSeqId)
	End Function
	Public Shared Sub UpdateRoles(ByVal GroupId As Integer, ByVal BusinessUnitSeqId As Integer, ByVal roles() As String, Optional ByVal Account_seq_Id As Integer = 1)
		iBaseDAL.UpdateRoles(GroupId, BusinessUnitSeqId, roles, Account_seq_Id)
	End Sub
	Public Shared Function UpdateAGroup(ByVal GroupInfo As MGroupInfo, Optional ByVal Account_seq_Id As Integer = 1) As Boolean
		Return iBaseDAL.UpdateAGroup(GroupInfo, Account_seq_Id)
	End Function
	Public Shared Sub DeleteGroup(ByVal GroupSeqId As String, ByVal BusinessUnitSeqId As Integer, Optional ByVal Account_Seq_Id As Integer = 1)
		iBaseDAL.DeleteGroup(GroupSeqId, BusinessUnitSeqId, Account_Seq_Id)
	End Sub

End Class
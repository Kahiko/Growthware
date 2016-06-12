Imports GrowthWare.Framework.DataAccessLayer.Interfaces.Base
Imports GrowthWare.Framework.ModelObjects

Namespace DataAccessLayer
	Public Interface IGroups
		Inherits IDBInteraction

		Property Profile() As MGroupProfile
		Property GroupRolesProfile() As MGroupRoles

		Function GetGroupRoles() As DataTable
		Function UpdateGroupRoles() As Boolean

		Function GetGroupsByBU() As DataTable
		Function AddGroup() As Boolean
		Function GetProfileData() As DataRow
		Function DeleteGroup() As Boolean
		Function UpdateGroup() As Boolean
	End Interface
End Namespace
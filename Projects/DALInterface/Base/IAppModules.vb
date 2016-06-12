Imports DALModel.Base
Imports DALModel.Base.Modules

Namespace Base.Interfaces
	Public Interface IAppModules
		Sub AddModuleRoles(ByVal MODULE_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer, ByVal moduleRoleType As MRoleType.value, ByVal roles() As String, Optional ByVal Account_Seq_id As Integer = 1)

		Sub AddModuleGroups(ByVal MODULE_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer, ByVal moduleGroupType As MGroupType.value, ByVal groups() As String, Optional ByVal Account_Seq_id As Integer = 1)

		Function AddModule(ByVal profile As MModuleProfileInfo, Optional ByVal Account_Seq_id As Integer = 1) As Integer
		Function DeleteModule(ByVal MODULE_SEQ_ID As Integer, Optional ByVal Account_Seq_id As Integer = 1) As Boolean
		Function GetModuleCollectionFromDB(ByVal BUSINESS_UNIT_SEQ_ID As Integer) As MModuleCollection
		Function UpdateProfile(ByVal profile As MModuleProfileInfo, Optional ByVal Account_Seq_id As Integer = 1) As Integer
	End Interface
End Namespace
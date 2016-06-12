Imports DALFactory.Base.Application
Imports DALInterface.Base.Interfaces
Imports DALModel.Base
Imports DALModel.Base.Modules

'Imports System.EnterpriseServices
'Imports System.Runtime.InteropServices

Namespace Base.SQLServer
    '<Transaction(System.EnterpriseServices.TransactionOption.Required), _
    'ClassInterface(ClassInterfaceType.AutoDispatch), _
    'ObjectPooling(4, 4), _
    'Guid("14E3573D-78C8-4220-9649-BA490DB7B78D")> _
    Public Class BAppModules

		'Private Shared iBaseDAL = CType(AbstractFactory.Create(Configuration.ConfigurationSettings.AppSettings("BaseDAL"), "DAppModules"), IAppModules)
		Private Shared iBaseDAL As IAppModules = AbstractFactory.Create(Configuration.ConfigurationSettings.AppSettings("BaseDAL"), "DAppModules")

        Public Shared Function AddModule(ByVal profile As MModuleProfileInfo, Optional ByVal Account_seq_id As Integer = 1) As Integer
			Return iBaseDAL.AddModule(profile, Account_seq_id)
        End Function

        Public Shared Sub AddModuleRoles(ByVal MODULE_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer, ByVal moduleRoleType As MRoleType.value, ByVal roles() As String, Optional ByVal Account_seq_id As Integer = 1)
			iBaseDAL.AddModuleRoles(MODULE_SEQ_ID, BUSINESS_UNIT_SEQ_ID, moduleRoleType, roles, Account_seq_id)
        End Sub

        Public Shared Sub AddModuleGroups(ByVal MODULE_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer, ByVal moduleGroupType As MGroupType.value, ByVal roles() As String, Optional ByVal Account_seq_id As Integer = 1)
			iBaseDAL.AddModuleGroups(MODULE_SEQ_ID, BUSINESS_UNIT_SEQ_ID, moduleGroupType, roles, Account_seq_id)
        End Sub

        Public Shared Function DeleteModule(ByVal MODULE_SEQ_ID As Integer, Optional ByVal Account_seq_id As Integer = 1) As Boolean
			Return iBaseDAL.DeleteModule(MODULE_SEQ_ID, Account_seq_id)
        End Function

        Public Shared Function GetModuleInfoFromAction(ByVal Action As String, ByVal State As String) As MModuleProfileInfo
			Dim moduleCollection As MModuleCollection = GetAllEnabledModules(State)
            Return moduleCollection(Action)
        End Function

        Public Shared Function GetAllEnabledModules(ByVal State As String) As MModuleCollection
			Return iBaseDAL.GetModuleCollectionFromDB(State)
        End Function    'GetAllEnabledSections

        Public Shared Function GetModuleCollectionFromDB(ByVal State As String) As MModuleCollection
			Return iBaseDAL.GetModuleCollectionFromDB(State)
        End Function

        Public Shared Function UpdateProfile(ByVal profile As MModuleProfileInfo, Optional ByVal Account_seq_id As Integer = 1) As Integer
			Return iBaseDAL.UpdateProfile(profile, Account_seq_id)
        End Function
    End Class
End Namespace
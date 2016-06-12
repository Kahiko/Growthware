Imports DALFactory.Base.Application
Imports DALInterface.Base.Interfaces

Namespace Base.SQLServer
	Public Class BDropBox
		'Private Shared iBaseDAL As IDropBox = FDropBox.Create(Configuration.ConfigurationSettings.AppSettings("BaseDAL"))
		Private Shared iBaseDAL As IDropBox = AbstractFactory.Create(Configuration.ConfigurationSettings.AppSettings("BaseDAL"), "DDropBox")

		Public Shared Sub GET_DROP_BOX_NAME(ByVal ACCOUNT_SEQ_ID As Integer, ByRef yourDataView As DataView)
			iBaseDAL.GET_DROP_BOX_NAME(ACCOUNT_SEQ_ID, yourDataView)
		End Sub

		Public Shared Sub GET_DROP_BOX_DETAIL(ByRef yourDataView As DataView)
			iBaseDAL.GET_DROP_BOX_DETAIL(yourDataView)
		End Sub

        Public Shared Function UPDATE_DROP_BOX(ByVal DROP_BOX_DET_ID As Integer, ByVal DROP_BOX_DET_code As Integer, ByVal DROP_BOX_DET_value As String, ByVal DROP_BOX_DET_status As Integer, ByVal DROP_BOX_ID As Integer, Optional ByVal ACCOUNT_SEQ_ID As Integer = 1) As Boolean
			Return iBaseDAL.UPDATE_DROP_BOX_DETAIL(DROP_BOX_DET_ID, DROP_BOX_DET_code, DROP_BOX_DET_value, DROP_BOX_DET_status, DROP_BOX_ID, ACCOUNT_SEQ_ID)
        End Function

        Public Shared Function ADD_DROP_BOX_DETAIL(ByVal DROP_BOX_DET_code As Integer, ByVal DROP_BOX_DET_value As String, ByVal DROP_BOX_DET_status As Integer, ByVal DROP_BOX_ID As Integer, Optional ByVal ACCOUNT_SEQ_ID As Integer = 1) As Boolean
			Return iBaseDAL.ADD_DROP_BOX_DETAIL(DROP_BOX_DET_code, DROP_BOX_DET_value, DROP_BOX_DET_status, DROP_BOX_ID, ACCOUNT_SEQ_ID)
        End Function

        Public Shared Sub AddDropBoxRoles(ByVal DROP_BOX_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer, ByVal moduleRoleType As DALModel.Base.MRoleType.value, ByVal roles() As String, Optional ByVal ACCOUNT_SEQ_ID As Integer = 1)
			iBaseDAL.AddDropBoxRoles(DROP_BOX_SEQ_ID, BUSINESS_UNIT_SEQ_ID, moduleRoleType, roles, ACCOUNT_SEQ_ID)
        End Sub

        Public Shared Function ADD_DROP_BOX(ByVal ACCOUNT_SEQ_ID As Integer, ByVal DESCRIPTION As String, Optional ByVal ACCNT_SEQ_ID As Integer = 1) As Integer
			Return iBaseDAL.ADD_DROP_BOX(ACCOUNT_SEQ_ID, DESCRIPTION, ACCNT_SEQ_ID)
        End Function

        Public Shared Function UPDATE_DROP_BOX(ByVal DROP_BOX_SEQ_ID As Integer, ByVal DESCRIPTION As String, Optional ByVal ACCOUNT_SEQ_ID As Integer = 1) As Boolean
			Return iBaseDAL.UPDATE_DROP_BOX(DROP_BOX_SEQ_ID, DESCRIPTION, ACCOUNT_SEQ_ID)
        End Function
    End Class
End Namespace

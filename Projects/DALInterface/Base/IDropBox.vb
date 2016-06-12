Imports DALModel.Base

Namespace Base.Interfaces
	Public Interface IDropBox
		Function ADD_DROP_BOX(ByVal ACCOUNT_SEQ_ID As Integer, ByVal DESCRIPTION As String, Optional ByVal ACCNT_SEQ_ID As Integer = 1) As Integer
		Function UPDATE_DROP_BOX(ByVal DROP_BOX_SEQ_ID As Integer, ByVal DESCRIPTION As String, Optional ByVal ACCOUNT_SEQ_ID As Integer = 1) As Boolean
		Sub AddDropBoxRoles(ByVal DROP_BOX_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer, ByVal moduleRoleType As MRoleType.value, ByVal roles() As String, Optional ByVal ACCOUNT_SEQ_ID As Integer = 1)
		Sub GET_DROP_BOX_NAME(ByVal ACCOUNT_SEQ_ID As Integer, ByRef theirDataView As DataView)
		Function ADD_DROP_BOX_DETAIL(ByVal DROP_BOX_DET_code As Integer, ByVal DROP_BOX_DET_value As String, ByVal DROP_BOX_DET_status As Integer, ByVal DROP_BOX_ID As Integer, Optional ByVal ACCOUNT_SEQ_ID As Integer = 1) As Boolean
		Function UPDATE_DROP_BOX_DETAIL(ByVal DROP_BOX_DET_ID As Integer, ByVal DROP_BOX_DET_code As Integer, ByVal DROP_BOX_DET_value As String, ByVal DROP_BOX_DET_status As Integer, ByVal DROP_BOX_ID As Integer, Optional ByVal ACCOUNT_SEQ_ID As Integer = 1) As Boolean
		Sub GET_DROP_BOX_DETAIL(ByRef theirDataView As DataView)
	End Interface
End Namespace
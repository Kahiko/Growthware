Imports ApplicationBase.Model.States

Public Interface IStates
	'Function GetStateArray(ByVal ACCOUNT_SEQ_ID As Integer) As String
	Function GetAdminStateArray(ByVal ACCOUNT_SEQ_ID As Integer) As String
	Function UpdateStateProfileInfo(ByVal stateProfileInfo As MStateProfileInfo, Optional ByVal Account_seq_Id As Integer = 1) As Boolean
	Function GetAllStates() As MStateProfileInfoCollection
End Interface
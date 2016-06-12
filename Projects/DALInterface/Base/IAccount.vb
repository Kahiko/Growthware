Imports DALModel.Special.Accounts

Namespace Base.Interfaces
	Public Interface IAccount
		Function AddAccount(ByVal profile As MAccountProfileInfo, ByVal ClientChoicesAccount As String, Optional ByVal Account_Seq_id As Integer = 1) As Integer
		Function GetAccountsByLetter(ByVal dsAccounts As DataSet, ByVal AccountType As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As DataSet
		Function GetProfile(ByVal Account As String) As MAccountProfileInfo
		Function GetRolesFromDB(ByVal ACCOUNT_SEQ_ID As Integer, Optional ByVal BUSINESS_UNIT_SEQ_ID As Integer = 1) As String()
		Function GetRolesFromDBByBusinessUnit(ByVal ACCOUNT_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As String()
		Function GetGroupsFromDB(ByVal ACCOUNT_SEQ_ID As Integer, Optional ByVal BUSINESS_UNIT_SEQ_ID As Integer = 1) As String()
		Function GetGroupsFromDBByBusinessUnit(ByVal ACCOUNT_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As String()
		Function LoginClient(ByVal AccountName As String, ByVal Password As String) As Boolean
		Sub UpdateRoles(ByVal ACCOUNT_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer, ByVal roles() As String, Optional ByVal Accnt_seq_id As Integer = 1)
		Function UpdateProfile(ByVal profile As MAccountProfileInfo, Optional ByVal Account_Seq_ID As Integer = 1) As Boolean
		Sub UpdateGroups(ByVal ACCOUNT_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer, ByVal groups() As String, Optional ByVal Accnt_seq_id As Integer = 1)
	End Interface
End Namespace
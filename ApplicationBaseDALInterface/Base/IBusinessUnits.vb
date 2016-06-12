Imports ApplicationBase.Model.BusinessUnits

Public Interface IBusinessUnits
	Function GetAllRolesForBusinessUnit(ByVal dstRoles As DataSet, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As DataSet
	Function GetAllGroupsForBusinessUnit(ByVal dstGroups As DataSet, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As DataSet
	'Function GetBusinessUnitArray(ByVal ACCOUNT_SEQ_ID As Integer) As String
	'Function GetAdminBusinessUnitArray(ByVal ACCOUNT_SEQ_ID As Integer) As String
	Function GetConnectionString(ByVal BUSINESS_UNIT_SEQ_ID As Integer) As String
	Function UpdateBusinessUnitProfileInfo(ByRef businessUnitProfileInfo As MBusinessUnitProfileInfo, Optional ByVal Account_Seq_id As Integer = 1) As Boolean
	Function AddBusinessUnitProfileInfo(ByRef businessUnitProfileInfo As MBusinessUnitProfileInfo, Optional ByVal Account_Seq_id As Integer = 1) As Boolean
	Function GetAllBusinessUnits() As MBusinessUnitProfileInfoCollection
	Sub GetValidBusinessUnits(ByRef yourDataSet As DataSet, ByVal Account_Seq_Id As Integer, ByVal isSysAdmin As Integer)
End Interface
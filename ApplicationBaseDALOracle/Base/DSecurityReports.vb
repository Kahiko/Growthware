Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports ApplicationBase.Common
Imports ApplicationBase.Interfaces
Imports ApplicationBaseDALOracle.SharedOracle

Public Class DSecurityReports
	Implements ISecurityReports

	Private _ConnectionString As String = String.Empty

	Public Function SecurityByRole(ByVal Business_Unit_SEQ_ID As Integer, ByVal ENVIRONMENT As String) As DataSet Implements ISecurityReports.SecurityByRole
		Dim myDS As New DataSet
		Dim reader As OracleDataReader = Nothing
		Try
			reader = OracleHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, _
			   "ZB_SECURITY_PKG.ZBP_GET_SECURITY_4_ROLES", New OracleParameter() _
			   {New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", Business_Unit_SEQ_ID), _
			   New OracleParameter("@P_ENVIRONMENT", ENVIRONMENT)})
			OracleHelperExtension.Fill(reader, myDS, "SecurityInfo", 0, 0)
			Return myDS
		Catch ex As Exception
			Throw ex
		End Try
	End Function

	Function Security4Module(ByVal Business_Unit_SEQ_ID As Integer, ByVal ENVIRONMENT As String) As DataSet Implements ISecurityReports.Security4Module
		Dim myDS As New DataSet
		Dim reader As OracleDataReader = Nothing
		Try
			reader = OracleHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, _
			   "ZB_SECURITY_PKG.ZBP_GET_SECURITY_4_MODULES", New OracleParameter() _
			   {New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", Business_Unit_SEQ_ID), _
			   New OracleParameter("@P_ENVIRONMENT", ENVIRONMENT)})
			OracleHelperExtension.Fill(reader, myDS, "SecurityInfo", 0, 0)
			Return myDS
		Catch ex As Exception
			Throw ex
		End Try
	End Function

	Private Function ConnectionString() As String
		If _ConnectionString = String.Empty Then
			' try to decrypt the connection string
            _ConnectionString = ConnectionHelper.GetConnectionString("ApplicationBaseDALOracle")
		End If
		Return _ConnectionString
	End Function
End Class
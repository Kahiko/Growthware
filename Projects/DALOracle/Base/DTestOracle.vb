Imports Common
Imports Common.Oracle
Imports System.Configuration
Imports DALInterface.Base.Interfaces
Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types

Public Class DTestOracle
    Implements ITestOracle

    Private _ConnectionString As String = String.Empty

    Public Function GetAddresses(ByRef dstAddresses As System.Data.DataSet) As System.Data.DataSet Implements ITestOracle.GetAddresses
        Try
            Dim reader As OracleDataReader = Nothing

			reader = OracleHelper.ExecuteReader( _
			ConnectionString, _
			CommandType.StoredProcedure, _
			"ZB_SECURITY_PKG.ZBP_GET_ACCOUNT_CHOICESINFO", _
			New OracleParameter() {New OracleParameter("@P_ACCOUNT", "Default")})

			If Not reader Is Nothing Then
				OracleHelperExtension.Fill(reader, dstAddresses, "Addresses")
			End If
		Catch ex As Exception
            Throw ex
        End Try
        Return dstAddresses
    End Function

    '*********************************************************************
    '
    ' ConnectionString Method
    '
    ' Get the connection string from the web.config file.
    '
    '*********************************************************************
    Private Function ConnectionString() As String
        If _ConnectionString = String.Empty Then
            ' try to decrypt the connection string
            _ConnectionString = ConnectionHelper.GetConnectionString("OracleDAL")
        End If
        Return _ConnectionString
    End Function
End Class
Imports ApplicationBase.Common
Imports ApplicationBase.Common.Security
Imports ApplicationBase.Interfaces
Imports ApplicationBaseDALOracle.SharedOracle
Imports Oracle.DataAccess.Client

Public Class ODALTestOracle
    Implements ITestOracle

	Private _ConnectionString As String = String.Empty

    Public Function GetAddresses(ByRef dstAddresses As System.Data.DataSet) As System.Data.DataSet Implements ITestOracle.GetAddresses
        Try
            Dim reader As OracleDataReader = Nothing
            Dim myTable As New DataTable
            Dim myConnectionString As String = ConnectionString()
            Dim commandText As String = String.Empty
            'commandText = "SELECT DISTINCT mau_case.active, mau_beneficiary.fname, mau_beneficiary.mname, mau_beneficiary.lname, mau_beneficiary.medicaid, mau_beneficiary.medicare, mau_provider.medicaid, mau_provider.provider_medicare FROM mau_case, mau_beneficiary, mau_provider WHERE ((mau_case.medicare = mau_beneficiary.medicare) AND (mau_case.provider_medicare = mau_provider.provider_medicare))"
            commandText = "select DISTINCT STREET_1 AS Street, CITY, STATE_TYPE_ID as State, ZIP from ADDRESS ORDER BY STATE_TYPE_ID,ZIP,CITY"
            'commandText = "select * from emp"

            reader = OracleHelper.ExecuteReader(myConnectionString, CommandType.Text, commandText)
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
            _ConnectionString = ConnectionHelper.GetConnectionString("ApplicationBaseDALOracle")
		End If
		Return _ConnectionString
	End Function
End Class

Imports ApplicationBase.Common
Imports ApplicationBaseDALSQLServer.SharedSQLServer
Imports ApplicationBase.Common.Security
Imports ApplicationBase.Interfaces
Imports System.Data.SqlClient

Public Class DSecurityReports
    Implements ISecurityReports

    Private _ConnectionString As String = String.Empty

    Public Function SecurityByRole(ByVal Business_Unit_SEQ_ID As Integer, ByVal ENVIRONMENT As String) As DataSet Implements ISecurityReports.SecurityByRole
        Dim myDS As New DataSet
        Dim reader As SqlDataReader = Nothing
        Try
            reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, _
               "ZBP_GET_SECURITY_4_ROLES", New SqlParameter() { _
               New SqlParameter("@P_BUSINESS_UNIT_SEQ_ID", Business_Unit_SEQ_ID), _
               New SqlParameter("@P_ENVIRONMENT", ENVIRONMENT)})
            SqlHelperExtension.Fill(reader, myDS, "SecurityInfo", 0, 0)
            Return myDS
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function Security4Module(ByVal Business_Unit_SEQ_ID As Integer, ByVal ENVIRONMENT As String) As DataSet Implements ISecurityReports.Security4Module
        Dim myDS As New DataSet
        Dim reader As SqlDataReader = Nothing
        Try
            reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, _
               "ZBP_GET_SECURITY_4_MODULES", New SqlParameter() { _
               New SqlParameter("@P_BUSINESS_UNIT_SEQ_ID", Business_Unit_SEQ_ID), _
               New SqlParameter("@P_ENVIRONMENT", ENVIRONMENT) _
               })
            SqlHelperExtension.Fill(reader, myDS, "SecurityInfo", 0, 0)
            Return myDS
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ConnectionString() As String
        If _ConnectionString = String.Empty Then
            ' try to decrypt the connection string
            _ConnectionString = ConnectionHelper.GetConnectionString("ApplicationBaseDALSQLServer")
        End If
        Return _ConnectionString
    End Function
End Class
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web
Imports Common
Imports Common.SQLServer
Imports DALInterface.Base.Interfaces

Public Class DCalendarData
    Implements ICalendarData

    Private _ConnectionString As String = String.Empty

    Public Function GetCalendarData(ByVal BUSINESS_UNIT_SEQ_ID As Integer, ByVal Calendar_Name As String, ByRef dsCalendarData As System.Data.DataSet) As Boolean Implements ICalendarData.GetCalendarData
        Dim retVal As Boolean = False
        Try
            dsCalendarData = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZBP_get_Calendar_Data", New SqlParameter() {New SqlParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID), New SqlParameter("@P_Calendar_Name", Calendar_Name)})
            retVal = True
        Catch ex As Exception
            Throw ex
        End Try
        Return retVal
    End Function

    Public Function SaveCalendarData(ByVal BUSINESS_UNIT_SEQ_ID As Integer, ByVal Calendar_Name As String, ByVal Entry As String, ByVal EntryDate As Date, Optional ByVal Account_Seq_id As Integer = 1) As Boolean Implements ICalendarData.SaveCalendarData
        Dim retVal As Boolean = False
        Try
            Dim SQLReturn As String
            Dim returnParam As New SqlParameter("@P_RETURN_VALUE", SqlDbType.NVarChar)
            returnParam.Direction = ParameterDirection.ReturnValue
            SqlHelper.ExecuteNonQuery(ConnectionString, _
            CommandType.StoredProcedure, _
            "ZBP_add_Calendar_Data", _
            New SqlParameter() { _
            New SqlParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID), _
            New SqlParameter("@P_Calendar_Name", Calendar_Name), _
            New SqlParameter("@P_Entry", Entry), _
            New SqlParameter("@P_EntryDate", EntryDate), returnParam, _
            New SqlParameter("@P_ADDUPD_BY", Account_Seq_id) _
             })
            SQLReturn = Fix(returnParam.Value)
            If SQLReturn = "1" Then retVal = True
        Catch ex As Exception
            Throw ex
        End Try
        Return retVal
    End Function

    Public Function DeleteCalendarData(ByVal BUSINESS_UNIT_SEQ_ID As Integer, ByVal Calendar_Name As String, ByVal Entry As String, ByVal EntryDate As Date, Optional ByVal Account_Seq_id As Integer = 1) As Boolean Implements ICalendarData.DeleteCalendarData
        Dim retVal As Boolean = False
        Try
            SqlHelper.ExecuteNonQuery(ConnectionString, _
            CommandType.StoredProcedure, _
            "ZBP_DEL_CALENDAR_DATA", _
            New SqlParameter() { _
            New SqlParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID), _
            New SqlParameter("@P_Calendar_Name", Calendar_Name), _
            New SqlParameter("@P_Entry", Entry), _
            New SqlParameter("@P_EntryDate", EntryDate), _
            New SqlParameter("@P_ADDUPD_BY", Account_Seq_id) _
            })
            retVal = True
        Catch ex As Exception
            Throw ex
        End Try
        Return retVal
    End Function

#Region " Private  "

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
            _ConnectionString = ConnectionHelper.GetConnectionString("SQLServerAppDB")
        End If
        Return _ConnectionString
    End Function
#End Region
End Class
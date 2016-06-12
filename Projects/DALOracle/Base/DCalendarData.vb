Imports System.Configuration
Imports System.Data
Imports System.Web
Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports Common
Imports Common.Oracle
Imports DALInterface.Base.Interfaces

Public Class DCalendarData
	Implements ICalendarData

	Private _ConnectionString As String = String.Empty

	Public Function GetCalendarData(ByVal BUSINESS_UNIT_SEQ_ID As Integer, ByVal Calendar_Name As String, ByRef dsCalendarData As System.Data.DataSet) As Boolean Implements ICalendarData.GetCalendarData
		Dim retVal As Boolean = False
		Try
			dsCalendarData = OracleHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_get_Calendar_Data", New OracleParameter() {New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID), New OracleParameter("@P_Calendar_Name", Calendar_Name)})
			retVal = True
		Catch ex As Exception
			Throw ex
		End Try
		Return retVal
	End Function

    Public Function SaveCalendarData(ByVal BUSINESS_UNIT_SEQ_ID As Integer, ByVal Calendar_Name As String, ByVal Entry As String, ByVal EntryDate As Date, Optional ByVal Account_Seq_Id As Integer = 1) As Boolean Implements ICalendarData.SaveCalendarData
        Dim retVal As Boolean = False
        Try
            Dim OracleReturn As String
            Dim returnParam As New OracleParameter("@P_RETURN_VALUE", OracleDbType.Varchar2)
            returnParam.Direction = ParameterDirection.ReturnValue
            OracleHelper.ExecuteNonQuery(ConnectionString, _
            CommandType.StoredProcedure, _
            "ZB_SECURITY_PKG.ZBP_add_Calendar_Data", _
            New OracleParameter() { _
            New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID), _
            New OracleParameter("@P_Calendar_Name", Calendar_Name), _
            New OracleParameter("@P_Entry", Entry), _
            New OracleParameter("@P_EntryDate", EntryDate), _
New OracleParameter("@P_ADDUPD_BY", Account_Seq_Id), _
returnParam})
            OracleReturn = Fix(returnParam.Value)
            If OracleReturn = "1" Then retVal = True
        Catch ex As Exception
            Throw ex
        End Try
        Return retVal
    End Function

    Public Function DeleteCalendarData(ByVal BUSINESS_UNIT_SEQ_ID As Integer, ByVal Calendar_Name As String, ByVal Entry As String, ByVal EntryDate As Date, Optional ByVal Account_Seq_Id As Integer = 1) As Boolean Implements ICalendarData.DeleteCalendarData
        Dim retVal As Boolean = False
        Try
            OracleHelper.ExecuteNonQuery(ConnectionString, _
            CommandType.StoredProcedure, _
            "ZB_SECURITY_PKG.ZBP_DEL_CALENDAR_DATA", _
            New OracleParameter() { _
            New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID), _
            New OracleParameter("@P_Calendar_Name", Calendar_Name), _
            New OracleParameter("@P_Entry", Entry), _
            New OracleParameter("@P_EntryDate", EntryDate), _
New OracleParameter("@P_ADDUPD_BY", Account_Seq_Id) _
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
            _ConnectionString = ConnectionHelper.GetConnectionString("OracleDAL")
        End If
        Return _ConnectionString
    End Function
#End Region
End Class
Imports System.Configuration
Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports Common
Imports Common.Oracle
Imports Common.Security.BaseSecurity
Imports DALInterface.Base.Interfaces
Imports DALModel.Base.States

Public Class DStates
	Implements IStates

#Region " Private Objects "
	Private _ConnectionString As String = String.Empty
#End Region

#Region " Private Methods "
	'*********************************************************************
	'
	' CalculateDirectories Method
	'
	' Calculate directory information such as directy paths and 
	' inherited properties by iterating through each 
	' row in a DataSet containing directory rows.
	'
	'*********************************************************************
	Private Shared Function CalculateStates(ByVal dstStates As DataSet) As MStateProfileInfoCollection
		Dim stateInfoCollection As New MStateProfileInfoCollection
		Dim dtblStates As DataTable = dstStates.Tables(0)

		' Calculated inherited properties for each state
		Dim drowState As DataRow
		For Each drowState In dtblStates.Rows
			If Not drowState("State") Is Nothing Then
				' Add the directoryInfo to the collection
				stateInfoCollection.Add(Trim(CStr(drowState("State"))), New MStateProfileInfo(drowState))
			End If
		Next drowState

		Return stateInfoCollection
	End Function	'CalculateDirectories

	'*********************************************************************
	'
	' ConnectionString Method
	' Gets the connection string from the web.config file.
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

	Function GetAdminStateArray(ByVal ACCOUNT_SEQ_ID As Integer) As String Implements IStates.GetAdminStateArray
		Dim ds As DataSet = New DataSet
		Dim strWriteScript As String = ""
		Dim x As Integer
		Dim myState As String
		Dim myYear As String
		strWriteScript &= "<script>" & vbCrLf
		Try
			ds = OracleHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_get_All_Active_States")
			strWriteScript &= "     var yearArray = new Array();" & vbCrLf
			For x = 0 To ds.Tables.Item(0).Rows.Count - 1
				myYear = ""
				' for value
				myState &= "'" & ds.Tables.Item(0).Rows(x).Item(0) & "',"
				' for text
				myState &= "'" & ds.Tables.Item(0).Rows(x).Item(0) & "',"
				'myYear = getYearArray(ds.Tables.Item(0).Rows(x).Item(0))
				'strWriteScript &= "     yearArray[" & x & "] = new Array(" & myYear & ");" & vbCrLf
			Next
			myState = myState.Substring(0, myState.Length - 1)
			strWriteScript &= "     var stateArray = new Array();" & vbCrLf
			strWriteScript &= "     stateArray[0] = new Array(" & myState & ");" & vbCrLf
			strWriteScript &= "</script>" & vbCrLf
		Catch ex As Exception
			Throw ex
			'Throw New ApplicationException(String.Format(ResourceManager.GetString("RES_ExceptionCantGetProduct"), productId), e)
		Finally
			If Not ds Is Nothing Then
				ds.Dispose()
				ds = Nothing
			End If
		End Try
		Return strWriteScript
	End Function

	'*********************************************************************
	'
	' GetAllStates Method
	' Gets all the states and returns them as a State Profile Information Collection.
	'
	'*********************************************************************
	Function GetAllStates() As MStateProfileInfoCollection Implements IStates.GetAllStates
		Dim myDataSet As New DataSet
		Dim myStateProfileInfoCollection As MStateProfileInfoCollection
		Try
			myDataSet = OracleHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_get_All_States_All_Info")
			myStateProfileInfoCollection = CalculateStates(myDataSet)
		Catch ex As Exception
			Throw ex
		Finally
			If Not myDataSet Is Nothing Then
				myDataSet = Nothing
			End If
		End Try
		Return myStateProfileInfoCollection
	End Function	'GetAllStates

	Private Function PopulateProfileFromOracleDataReader(ByVal dr As OracleDataReader) As MStateProfileInfo
		Dim profile As New MStateProfileInfo
		profile.State = CStr(dr("State"))
		profile.LongName = CStr(dr("Long"))
		profile.STATUS_SEQ_ID = CInt(dr("STATUS_SEQ_ID"))
		Return profile
	End Function	'PopulateProfileFromOracleDataReader

    Public Function UpdateStateProfileInfo(ByVal stateProfileInfo As MStateProfileInfo, Optional ByVal Account_Seq_Id As Integer = 1) As Boolean Implements IStates.UpdateStateProfileInfo
        Dim retVal As Boolean = False
        Try
            OracleHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_update_State_Profile", _
            New OracleParameter() { _
            New OracleParameter("@P_State", stateProfileInfo.State), _
            New OracleParameter("@P_LongName", stateProfileInfo.LongName), _
            New OracleParameter("@P_STATUS_SEQ_ID", stateProfileInfo.STATUS_SEQ_ID), _
            New OracleParameter("@P_ADDUPD_BY", Account_Seq_Id) _
               })
            retVal = True
        Catch ex As Exception
            Throw ex
        End Try
        Return retVal
    End Function

    Public Function GetStateArray(ByVal ACCOUNT_SEQ_ID As Integer) As String Implements IStates.GetStateArray
        ' does nothing at the moment but may need to be for future use
    End Function
End Class
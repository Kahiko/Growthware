Imports System.Configuration
Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports Common
Imports Common.Oracle
Imports Common.Security.BaseSecurity
Imports DALInterface.Base.Interfaces
Imports DALModel.Base.Messages

Public Class DMessages
	Implements IMessages

#Region " Private Objects "
	Private _ConnectionString As String = String.Empty
#End Region

	'*********************************************************************
	'
	' GetMessage Method
	' Retrieves message information from the database.
	'
	'*********************************************************************
	Public Function GetMessage(ByVal messageName As String, Optional ByVal BUSINESS_UNIT_SEQ_ID As Integer = 1) As MMessageInfo Implements IMessages.GetMessage
		Dim messageInfo As New MMessageInfo
		Dim reader As OracleDataReader = Nothing
		Try
			reader = OracleHelper.ExecuteReader( _
			   ConnectionString, _
			   CommandType.StoredProcedure, _
			   "ZB_SECURITY_PKG.ZBP_get_Message", _
			   New OracleParameter() { _
			 New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID), _
			 New OracleParameter("@P_messageName", messageName) _
			   } _
			   )
			If reader.Read() Then
				messageInfo.MESSAGE_SEQ_ID = CInt(reader("MESSAGE_SEQ_ID"))
				messageInfo.BUSINESS_UNIT_SEQ_ID = CStr(reader("BUSINESS_UNIT_SEQ_ID"))
				messageInfo.Name = CStr(reader("Name"))
				messageInfo.Description = CStr(reader("Description"))
				messageInfo.Title = CStr(reader("Title"))
				messageInfo.Body = CStr(reader("Body"))
			End If
		Catch ex As Exception
			Throw ex
		Finally
			If Not (reader Is Nothing) Then
				CType(reader, IDisposable).Dispose()
			End If
		End Try
		Return messageInfo
	End Function	'GetMessage

#Region " Private "
	'*********************************************************************
	'
	' ConnectionString Method
	' Get the connection string from the web.config file.
	'
	'*********************************************************************
	Private Function ConnectionString() As String
		If _ConnectionString = String.Empty Then
			' try to decrypt the connection string
			_ConnectionString = ConnectionHelper.GetConnectionString("OracleDAL")
		End If
		Return _ConnectionString
	End Function	'ConnectionString
#End Region

	Public Sub GetMessageNames(ByRef yourDataSet As System.Data.DataSet, ByVal BUSINESS_UNIT_SEQ_ID As Integer) Implements IMessages.GetMessageNames
		Try
            yourDataSet = OracleHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_GET_ALL_MESSAGES_FOR_BU", New OracleParameter() {New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID)})
		Catch ex As Exception
			Throw ex
		End Try
	End Sub

    Public Function UpdateMessage(ByVal yourMessageInfo As MMessageInfo, Optional ByVal Account_Seq_Id As Integer = 1) As Boolean Implements IMessages.UpdateMessage
        Dim retVal As Boolean = False
        Try
            OracleHelper.ExecuteNonQuery( _
            ConnectionString, _
            CommandType.StoredProcedure, _
            "ZB_SECURITY_PKG.ZBP_update_Message_Profile", _
            New OracleParameter() { _
            New OracleParameter("@P_MESSAGE_SEQ_ID", yourMessageInfo.MESSAGE_SEQ_ID), _
            New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", yourMessageInfo.BUSINESS_UNIT_SEQ_ID), _
            New OracleParameter("@P_Name", yourMessageInfo.Name), _
            New OracleParameter("@P_Title", yourMessageInfo.Title), _
            New OracleParameter("@P_Description", yourMessageInfo.Description), _
            New OracleParameter("@P_Body", yourMessageInfo.Body), _
            New OracleParameter("@P_ADDUPD_BY", Account_Seq_Id) _
            })

            retVal = True
        Catch ex As Exception
            Throw ex
        End Try
        Return retVal
    End Function
End Class
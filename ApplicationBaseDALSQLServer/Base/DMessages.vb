Imports System.Configuration
Imports System.Data.SqlClient
Imports ApplicationBase.Common
Imports ApplicationBaseDALSQLServer.SharedSQLServer
Imports ApplicationBase.Common.Security
Imports ApplicationBase.Interfaces
Imports ApplicationBase.Model.Messages

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
        Dim reader As SqlDataReader = Nothing
        Try
            reader = SqlHelper.ExecuteReader( _
               ConnectionString, _
               CommandType.StoredProcedure, _
               "ZBP_get_Message", _
               New SqlParameter() { _
             New SqlParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID), _
             New SqlParameter("@P_messageName", messageName) _
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
    End Function 'GetMessage

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
            _ConnectionString = ConnectionHelper.GetConnectionString("ApplicationBaseDALSQLServer")
        End If
        Return _ConnectionString
    End Function 'ConnectionString
#End Region

    Public Sub GetMessageNames(ByRef yourDataSet As System.Data.DataSet, ByVal BUSINESS_UNIT_SEQ_ID As Integer) Implements IMessages.GetMessageNames
        Try
            yourDataSet = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZBP_GET_ALL_MESSAGES_FOR_BU", New SqlParameter() {New SqlParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID)})
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function UpdateMessage(ByVal yourMessageInfo As MMessageInfo, Optional ByVal Account_Seq_id As Integer = 1) As Boolean Implements IMessages.UpdateMessage
        Dim retVal As Boolean = False
        Try
            SqlHelper.ExecuteNonQuery( _
            ConnectionString, _
            CommandType.StoredProcedure, _
            "ZBP_update_Message_Profile", _
            New SqlParameter() { _
            New SqlParameter("@P_MESSAGE_SEQ_ID", yourMessageInfo.MESSAGE_SEQ_ID), _
            New SqlParameter("@P_BUSINESS_UNIT_SEQ_ID", yourMessageInfo.BUSINESS_UNIT_SEQ_ID), _
            New SqlParameter("@P_Name", yourMessageInfo.Name), _
            New SqlParameter("@P_Title", yourMessageInfo.Title), _
            New SqlParameter("@P_Description", yourMessageInfo.Description), _
            New SqlParameter("@P_Body", yourMessageInfo.Body), _
            New SqlParameter("@P_ADDUPD_BY", Account_Seq_id) _
            })
            retVal = True
        Catch ex As Exception
            Throw ex
        End Try
        Return retVal
    End Function
End Class
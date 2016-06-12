Imports System.Configuration
Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports System.Data
Imports System.Web
Imports Common
Imports Common.Oracle
Imports Common.Security.BaseSecurity
Imports DALInterface.Base.Interfaces

Public Class DClientChoices
	Implements IClientChoices

#Region " Private Objects "
	Private _ConnectionString As String = String.Empty
#End Region

    Public Function CreateClientChoicesAccount(ByVal Account As String, Optional ByVal Account_Seq_Id As Integer = 1) As Boolean Implements IClientChoices.CreateClientChoicesAccount
        Dim retVal As Boolean = False
        Try
            OracleHelper.ExecuteNonQuery(ConnectionString, _
            CommandType.StoredProcedure, _
            "ZB_SECURITY_PKG.ZBP_add_Account_Choices", _
            New OracleParameter() { _
            New OracleParameter("@P_ACCOUNT", Account), _
            New OracleParameter("@P_ADDUPD_BY", Account_Seq_Id) _
            })
            retVal = True
        Catch ex As Exception
            Throw ex
        End Try
        Return retVal
    End Function 'CreateClientChoicesAccount

    Public Function GetClientChoicesData(ByVal AccountName As String) As System.Data.DataSet Implements IClientChoices.GetClientChoicesData
        Dim dsResult As New DataSet
        Dim reader As OracleDataReader = Nothing
        Try
            reader = OracleHelper.ExecuteReader( _
               ConnectionString, _
               CommandType.StoredProcedure, _
               "ZB_SECURITY_PKG.ZBP_get_Account_ChoicesInfo", _
               New OracleParameter() { _
             New OracleParameter("@P_Account", AccountName) _
               })
            If Not reader Is Nothing Then
                OracleHelperExtension.Fill(reader, dsResult, "dsResult", 0, 0)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            If Not (reader Is Nothing) Then
                CType(reader, IDisposable).Dispose()
            End If
        End Try
        Return dsResult
    End Function

    Public Function Save(ByVal theChoices As System.Collections.Hashtable) As Boolean Implements IClientChoices.Save
        Dim isDirty As Boolean = True
        Try
            Dim HashKeyEnum As IEnumerator = CType(theChoices.Keys, IEnumerable).GetEnumerator()
            Dim HashValEnum As IEnumerator = CType(theChoices.Values, IEnumerable).GetEnumerator()
            Dim commandParameters(theChoices.Count - 1) As OracleParameter
            Dim x As Integer = 0
            Do While (HashKeyEnum.MoveNext() And HashValEnum.MoveNext())
                Dim myParameter As OracleParameter = New OracleParameter("@P_" + HashKeyEnum.Current.ToString(), OracleDbType.Varchar2, 1000)
                myParameter.Value = HashValEnum.Current.ToString()
                commandParameters.SetValue(myParameter, x)
                x = x + 1
            Loop
            OracleHelper.ExecuteNonQuery( _
             ConnectionString, _
             CommandType.StoredProcedure, _
             "ZB_SECURITY_PKG.ZBP_update_Account_Choices", commandParameters)
            isDirty = False    ' Added to prevent excessive saves
        Catch ex As Exception
            Throw ex
        End Try
        Return isDirty
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
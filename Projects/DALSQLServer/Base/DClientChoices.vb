Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web
Imports Common
Imports Common.SQLServer
Imports Common.Security.BaseSecurity
Imports DALInterface.Base.Interfaces

Public Class DClientChoices
    Implements IClientChoices

#Region " Private Objects "
    Private _ConnectionString As String = String.Empty
#End Region

    Public Function CreateClientChoicesAccount(ByVal Account As String, Optional ByVal Account_Seq_id As Integer = 1) As Boolean Implements IClientChoices.CreateClientChoicesAccount
        Dim retVal As Boolean = False
        Try
            SqlHelper.ExecuteNonQuery(ConnectionString, _
            CommandType.StoredProcedure, _
            "ZBP_add_Account_Choices", _
            New SqlParameter() { _
            New SqlParameter("@P_ACCOUNT", Account), _
            New SqlParameter("@P_ADDUPD_BY", Account_Seq_id) _
            })
            retVal = True
        Catch ex As Exception
            Throw ex
        End Try
        Return retVal
    End Function 'CreateClientChoicesAccount

    Public Function GetClientChoicesData(ByVal AccountName As String) As System.Data.DataSet Implements IClientChoices.GetClientChoicesData
        Dim dsResult As New DataSet
        Dim reader As SqlDataReader = Nothing
        Try
            reader = SqlHelper.ExecuteReader( _
               ConnectionString, _
               CommandType.StoredProcedure, _
               "ZBP_get_Account_ChoicesInfo", _
               New SqlParameter() { _
             New SqlParameter("@P_Account", AccountName) _
               })
            If Not reader Is Nothing Then
                SqlHelperExtension.Fill(reader, dsResult, "dsResult", 0, 0)
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
            Dim commandParameters(theChoices.Count - 1) As SqlParameter
            Dim x As Integer = 0
            Do While (HashKeyEnum.MoveNext() And HashValEnum.MoveNext())
                Dim myParameter As SqlParameter = New SqlParameter("@P_" + HashKeyEnum.Current.ToString(), SqlDbType.NVarChar, 1000)
                myParameter.Value = HashValEnum.Current.ToString()
                commandParameters.SetValue(myParameter, x)
                x = x + 1
            Loop
            SqlHelper.ExecuteNonQuery( _
             ConnectionString, _
             CommandType.StoredProcedure, _
             "ZBP_update_Account_Choices", commandParameters)
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
            _ConnectionString = ConnectionHelper.GetConnectionString("SQLServerAppDB")
        End If
        Return _ConnectionString
    End Function
End Class
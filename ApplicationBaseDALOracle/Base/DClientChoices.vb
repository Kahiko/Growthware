Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports ApplicationBase.Common
Imports ApplicationBase.Interfaces
Imports ApplicationBaseDALOracle.SharedOracle

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
		Dim isDirty As Boolean = False
		Try
			OracleHelper.ExecuteNonQuery( _
			 ConnectionString, _
			 CommandType.StoredProcedure, _
			 "ZB_SECURITY_PKG.ZBP_update_Account_Choices", New OracleParameter() { _
			 New OracleParameter("@P_ACCOUNT", GetParameterValue("ACCOUNT", theChoices)), _
			 New OracleParameter("@P_COLOR_SCHEME", GetParameterValue("COLOR_SCHEME", theChoices)), _
			 New OracleParameter("@P_MODULE_ACTION", GetParameterValue("MODULE_ACTION", theChoices)), _
			 New OracleParameter("@P_LEFT_COLOR", GetParameterValue("LEFT_COLOR", theChoices)), _
			 New OracleParameter("@P_SUB_HEAD_COLOR", GetParameterValue("SUB_HEAD_COLOR", theChoices)), _
			 New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", GetParameterValue("BUSINESS_UNIT_SEQ_ID", theChoices)), _
			 New OracleParameter("@P_RECORDS_PER_PAGE", GetParameterValue("RECORDS_PER_PAGE", theChoices)), _
			 New OracleParameter("@P_BUSINESS_UNIT_NAME", GetParameterValue("BUSINESS_UNIT_NAME", theChoices)), _
			 New OracleParameter("@P_BACK_COLOR", GetParameterValue("BACK_COLOR", theChoices)), _
			 New OracleParameter("@P_HEAD_COLOR", GetParameterValue("HEAD_COLOR", theChoices)) _
			   })
		Catch ex As Exception
			Throw ex
		End Try
		Return isDirty
	End Function

	Private Function GetParameterValue(ByVal parameterName As String, ByVal theChoices As System.Collections.Hashtable) As String
		Dim HashKeyEnum As IEnumerator = CType(theChoices.Keys, IEnumerable).GetEnumerator()
		Dim HashValEnum As IEnumerator = CType(theChoices.Values, IEnumerable).GetEnumerator()
		Dim retVal As String = String.Empty
		Do While (HashKeyEnum.MoveNext() And HashValEnum.MoveNext())
			If HashKeyEnum.Current.ToString().Trim.ToLower = parameterName.Trim.ToLower Then
				retVal = HashValEnum.Current.ToString()
				Exit Do
			End If
		Loop
		Return retVal
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
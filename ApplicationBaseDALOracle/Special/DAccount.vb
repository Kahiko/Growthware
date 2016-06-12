Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports ApplicationBase.Common
Imports ApplicationBase.Model.Accounts
Imports ApplicationBase.Interfaces
Imports ApplicationBaseDALOracle.SharedOracle

Public Class DAccount
	Implements IAccount

#Region "Private Objects"
	Private _ConnectionString As String = String.Empty
#End Region

#Region "Private Methods"
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

	'*********************************************************************
	'
	' PopulateProfileInfoFromOracleDataReader Method
	'
	' Populates a ProfileInfo object from a OracleDataReader.
	'
	'*********************************************************************
	Private Function PopulateProfileFromOracleDataReader(ByVal dr As OracleDataReader) As MAccountProfileInfo
		On Error Resume Next		  ' give what we can
		Dim profile As New MAccountProfileInfo
		profile.ACCOUNT_SEQ_ID = Fix(dr("ACCOUNT_SEQ_ID"))
		profile.CREATED_BY = Fix(dr("ADDED_BY"))
		profile.UPDATED_BY = Fix(dr("UPDATED_BY"))
		profile.UPDATED_DATE = CType(dr("UPDATED_DATE"), DateTime)
		profile.ACCOUNT = CStr(dr("ACCOUNT"))
		profile.SYSTEM_STATUS_ID = CInt(dr("SYSTEM_STATUS_ID"))
		profile.First_Name = CStr(dr("FIRST_NAME"))
		profile.Middle_Name = CStr(dr("MIDDLE_NAME"))
		profile.Last_Name = CStr(dr("LAST_NAME"))
		profile.Prefered_Name = CStr(dr("PREFERED_NAME"))
		profile.EMAIL = CStr(dr("EMAIL"))
		profile.PWD = CStr(dr("PWD"))
		profile.FailedAttempts = CInt(dr("FAILED_ATTEMPTS"))
		profile.TIME_ZONE = Fix(dr("TIME_ZONE"))
		profile.Location = CStr(dr("LOCATION"))
		profile.DATE_CREATED = CType(dr("ADDED_DATE"), DateTime)
		profile.LAST_LOGIN = CType(dr("LAST_LOGIN"), DateTime)
		profile.EnableNotifications = CBool(dr("EnableNotifications"))
		Return profile
	End Function	'PopulateProfileFromOracleDataReader
#End Region

	'*********************************************************************
	'
	' GetAccountsByLetter Method
	'
	' Retrieves accounts from the database.
	'
	'*********************************************************************
	Public Function GetAccountsByLetter(ByVal dsAccounts As DataSet, ByVal AccountType As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As System.Data.DataSet Implements IAccount.GetAccountsByLetter
		Try
            dsAccounts = OracleHelper.ExecuteDataset( _
             ConnectionString, _
             CommandType.StoredProcedure, _
             "ZB_SECURITY_PKG.ZBP_GET_ACCTS_BY_LETTER", New OracleParameter() _
             {New OracleParameter("@P_Type", AccountType), _
             New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID)} _
             )
		Catch ex As Exception
			Throw ex
		End Try
		Return dsAccounts
	End Function

	'*********************************************************************
	'
	' GetProfile Method
	'
	' Retrieves a user profile from the database.
	'
	'*********************************************************************
	Public Function GetProfile(ByVal Account As String) As MAccountProfileInfo Implements IAccount.GetProfile
		Dim profile As MAccountProfileInfo = Nothing
		Dim reader As OracleDataReader = Nothing
		Try
			reader = OracleHelper.ExecuteReader( _
			   ConnectionString, _
			   CommandType.StoredProcedure, _
			   "ZB_SECURITY_PKG.ZBP_get_Account_Profile", _
			   New OracleParameter() {New OracleParameter("@P_Account", Account)} _
			   )
			If reader.Read() Then
				profile = PopulateProfileFromOracleDataReader(reader)
			End If
		Catch ex As Exception
			Throw ex
		Finally
			If Not (reader Is Nothing) Then
				CType(reader, IDisposable).Dispose()
			End If
		End Try
		Return profile
	End Function

	'*********************************************************************
	'
	' GetRolesFromDB Method
	' Retrieves Roles from the database.
	'
	'*********************************************************************
	Function GetRolesFromDB(ByVal ACCOUNT_SEQ_ID As Integer, Optional ByVal BUSINESS_UNIT_SEQ_ID As Integer = 1) As String() Implements IAccount.GetRolesFromDB
		Dim ClientRoles As New ArrayList
		Dim reader As OracleDataReader = Nothing
		Try
            reader = OracleHelper.ExecuteReader( _
               ConnectionString, _
               CommandType.StoredProcedure, _
               "ZB_SECURITY_PKG.ZBP_get_Rls_For_Account", _
               New OracleParameter() { _
             New OracleParameter("@P_ACCOUNT_SEQ_ID", ACCOUNT_SEQ_ID), _
             New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID) _
               } _
               )
			While reader.Read()
				ClientRoles.Add(reader("RLS"))
			End While
		Catch ex As Exception
			Throw ex
		Finally
			If Not (reader Is Nothing) Then
				CType(reader, IDisposable).Dispose()
			End If
		End Try
		' Return the String array of roles
		Return CType(ClientRoles.ToArray(GetType(String)), String())
	End Function	'GetRolesFromDB

	'*********************************************************************
	'
	' GetRolesFromDBByBusinessUnit Method
	' Retrieves Roles from the database by business unit.
	'
	'*********************************************************************
	Public Function GetRolesFromDBByBusinessUnit(ByVal ACCOUNT_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As String() Implements IAccount.GetRolesFromDBByBusinessUnit
		Dim ClientRoles As New ArrayList
		Dim reader As OracleDataReader = Nothing
		Try
            reader = OracleHelper.ExecuteReader( _
               ConnectionString, _
               CommandType.StoredProcedure, _
               "ZB_SECURITY_PKG.ZBP_GET_RLS_FOR_ACCOUNT_BY_BU", _
               New OracleParameter() { _
             New OracleParameter("@P_ACCOUNT_SEQ_ID", ACCOUNT_SEQ_ID), _
             New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID) _
               } _
               )
            While reader.Read()
                ClientRoles.Add(reader("ROLE_NAME"))
            End While
        Catch ex As Exception
            Throw ex
        Finally
            If Not (reader Is Nothing) Then
                CType(reader, IDisposable).Dispose()
            End If
        End Try
        ' Return the String array of roles
        Return CType(ClientRoles.ToArray(GetType(String)), String())
    End Function 'GetRolesFromDBByBusinessUnit

    '*********************************************************************
    '
    ' AddAccount Method
    '
    ' Calls SP_AddAccountProfile to create an account.
    '
    '*********************************************************************
    Public Function AddAccount(ByVal profile As MAccountProfileInfo, ByVal ClientChoicesAccount As String, Optional ByVal Account_Seq_id As Integer = 1) As Integer Implements IAccount.AddAccount
		Dim retVal As Integer = 1
        Try
			Dim returnParam As New OracleParameter("@P_RETURN_VALUE", OracleDbType.NVarchar2)
			Dim EnableNotifications As Integer = CInt(profile.EnableNotifications)
            returnParam.Direction = ParameterDirection.ReturnValue
			OracleHelper.ExecuteNonQuery(ConnectionString, _
			CommandType.StoredProcedure, _
			"ZB_SECURITY_PKG.ZBP_add_Account_Profile", _
			 New OracleParameter() { _
			 New OracleParameter("@P_SYSTEM_STATUS_ID", profile.SYSTEM_STATUS_ID), _
			 New OracleParameter("@P_ACCOUNT", profile.ACCOUNT), _
			 New OracleParameter("@P_FIRST_NAME", profile.First_Name), _
			 New OracleParameter("@P_LAST_NAME", profile.Last_Name), _
			 New OracleParameter("@P_MIDDLE_NAME", profile.Middle_Name), _
			 New OracleParameter("@P_PREFERED_NAME", profile.Prefered_Name), _
			 New OracleParameter("@P_EMAIL", profile.EMAIL), _
			 New OracleParameter("@P_PWD", profile.PWD), _
			 New OracleParameter("@P_FAILED_ATTEMPTS", profile.FailedAttempts), _
			 New OracleParameter("@P_LAST_LOGIN", ""), _
			 New OracleParameter("@P_TIME_ZONE", profile.TIME_ZONE), _
			 New OracleParameter("@P_LOCATION", profile.Location), _
			 New OracleParameter("@P_ENABLE_NOTIFICATIONS", EnableNotifications), _
			 New OracleParameter("@P_DEFAULT_CLIENTCHOICES_ACCOUNT", ClientChoicesAccount), _
			 New OracleParameter("@P_ADDUPD_BY", Account_Seq_id) _
			 })
			'retVal = Fix(returnParam.Value)
        Catch ex As Exception
            Throw ex
            'Throw New ApplicationException(ResourceManager.GetString("RES_ExceptionCantCreateOrder"), e)
        End Try
        Return retVal
    End Function

    '*********************************************************************
    '
    ' UpdateClientsRoles Method
    '
    ' Updates roles for a client in the database.
    '
    '*********************************************************************
    Public Sub UpdateRoles(ByVal ACCOUNT_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer, ByVal roles() As String, Optional ByVal Accnt_Seq_Id As Integer = 1) Implements IAccount.UpdateRoles
        Dim role As String
        Dim dbConn As New OracleConnection(ConnectionString)
        dbConn.Open()
        Dim trans As OracleTransaction = dbConn.BeginTransaction
        Dim newRoles As String = String.Empty
        Try
            ' delete the roles
			'OracleHelper.ExecuteNonQuery(trans, _
			' CommandType.StoredProcedure, _
			' "ZB_SECURITY_PKG.ZB_SECURITY_PKG.ZBP_DEL_ACCOUNT_RLS", _
			' New OracleParameter() { _
			' New OracleParameter("@P_ACCOUNT_SEQ_ID", ACCOUNT_SEQ_ID), _
			' New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID) _
			'	})
            ' Comma seporate the roles
            For Each role In roles
                newRoles += role & ","
            Next role
            If Not newRoles = String.Empty Then
                newRoles = Left(newRoles, Len(newRoles) - 1)
            Else
                newRoles = " "
            End If
            ' Add the selected roles
			OracleHelper.ExecuteNonQuery(trans, _
			 CommandType.StoredProcedure, _
			 "ZB_SECURITY_PKG.ZBP_UPDATE_ACCOUNT_RLS", _
			 New OracleParameter() { _
			 New OracleParameter("@P_ACCOUNT_SEQ_ID", ACCOUNT_SEQ_ID), _
			 New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID), _
			 New OracleParameter("@P_ROLES", newRoles), _
			 New OracleParameter("@P_ADDUPD_BY", Accnt_Seq_Id) _
			 })
            trans.Commit()   ' commit the changes
        Catch ex As Exception
            trans.Rollback()   ' rollback anything
            Throw ex
        Finally   ' clean up
            ' close and dispose of the db connection object
            If Not dbConn Is Nothing Then
                dbConn.Close()
                dbConn.Dispose()
            End If
            ' dispose of the transaction object
            If Not trans Is Nothing Then
                trans.Dispose()
            End If
        End Try
    End Sub 'UpdateClientsRoles

    '*********************************************************************
    '
    ' UpdateProfile Method
    '
    ' Updates a clients profile information in the database.
    '
    '*********************************************************************
    Public Function UpdateProfile(ByVal profile As MAccountProfileInfo, Optional ByVal Account_Seq_Id As Integer = 1) As Boolean Implements IAccount.UpdateProfile
		Dim retVal As Boolean = True
		Dim returnParam As New OracleParameter("@P_RETURN_VALUE", OracleDbType.Int32)
		returnParam.Direction = ParameterDirection.ReturnValue
		Dim LAST_LOGIN As Oracle.DataAccess.Types.OracleDate
		Dim DATE_CREATED As Oracle.DataAccess.Types.OracleDate
		Dim UPDATED_DATE As Oracle.DataAccess.Types.OracleDate
		'LAST_LOGIN = Format(profile.LAST_LOGIN, "M/dd/yyyy")
		Try
			LAST_LOGIN = New Oracle.DataAccess.Types.OracleDate(profile.LAST_LOGIN)
		Catch ex As Exception
			LAST_LOGIN = New Oracle.DataAccess.Types.OracleDate(DateTime.Now)
		End Try
		Try
			DATE_CREATED = New Oracle.DataAccess.Types.OracleDate(profile.DATE_CREATED)
		Catch ex As Exception
			DATE_CREATED = New Oracle.DataAccess.Types.OracleDate(DateTime.Now)
		End Try
		Try
			UPDATED_DATE = New Oracle.DataAccess.Types.OracleDate(profile.UPDATED_DATE)
		Catch ex As Exception
			UPDATED_DATE = New Oracle.DataAccess.Types.OracleDate(DateTime.Now)
		End Try
		Dim EnableNotifications As Integer = CInt(profile.EnableNotifications)
		If EnableNotifications > 0 Then
			EnableNotifications = 1
		End If
		Try
			OracleHelper.ExecuteNonQuery(ConnectionString, _
			 CommandType.StoredProcedure, _
			 "ZB_SECURITY_PKG.ZBP_update_Account_Profile", _
			 New OracleParameter() { _
			  New OracleParameter("@P_ACCOUNT_SEQ_ID", profile.ACCOUNT_SEQ_ID), _
			  New OracleParameter("@P_ACCOUNT", profile.ACCOUNT), _
			  New OracleParameter("@P_UPDATED_BY", profile.UPDATED_BY), _
			  New OracleParameter("@P_CREATED_BY", profile.CREATED_BY), _
			  New OracleParameter("@P_SYSTEM_STATUS_ID", profile.SYSTEM_STATUS_ID), _
			  New OracleParameter("@P_FIRST_NAME", profile.First_Name), _
			  New OracleParameter("@P_LAST_NAME", profile.Last_Name), _
			  New OracleParameter("@P_MIDDLE_NAME", profile.Middle_Name), _
			  New OracleParameter("@P_PREFERED_NAME", profile.Prefered_Name), _
			  New OracleParameter("@P_EMAIL", profile.EMAIL), _
			  New OracleParameter("@P_PWD", profile.PWD), _
			  New OracleParameter("@P_FAILED_ATTEMPTS", profile.FailedAttempts), _
			  New OracleParameter("@P_DATE_CREATED", DATE_CREATED), _
			  New OracleParameter("@P_UPDATED_DATE", UPDATED_DATE), _
			  New OracleParameter("@P_LAST_LOGIN", LAST_LOGIN), _
			  New OracleParameter("@P_TIME_ZONE", profile.TIME_ZONE), _
			  New OracleParameter("@P_LOCATION", profile.Location), _
			  New OracleParameter("@P_ENABLENOTIFICATIONS", EnableNotifications), _
			  New OracleParameter("@P_ADDUPD_BY", Account_Seq_Id) _
			 })
		Catch ex As Exception
			Throw ex
		End Try
		Return retVal
	End Function	'UpdateProfile

	'*********************************************************************
	'
	' LoginClient Method
	'
	' Verifies an account
	'
	'*********************************************************************
	Public Function LoginClient(ByVal AccountName As String, ByVal Password As String) As Boolean Implements IAccount.LoginClient
		Dim retVal As Boolean = False
		Try
			Dim returnParam As New OracleParameter("@P_IsValid", OracleDbType.Int32)
			returnParam.Direction = ParameterDirection.Output
			OracleHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_get_Is_Valid_Account", _
			 New OracleParameter() { _
			 New OracleParameter("@P_Account", AccountName.Trim()), _
			 New OracleParameter("@P_Password", Password.Trim()), returnParam _
			 } _
			 )
			If (CType(returnParam.Value, Integer) = 1) Then retVal = True
		Catch e As Exception
			Throw e
		End Try
		Return retVal
	End Function	'LoginClient

	Public Sub UpdateGroups(ByVal ACCOUNT_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer, ByVal groups() As String, Optional ByVal Accnt_Seq_Id As Integer = 1) Implements IAccount.UpdateGroups
		Dim group As String
		Dim dbConn As New OracleConnection(ConnectionString)
		dbConn.Open()
		Dim trans As OracleTransaction = dbConn.BeginTransaction
		Dim newGroups As String = String.Empty
		Try
			' delete the groups
			''''OracleHelper.ExecuteNonQuery(trans, _
			'''' CommandType.StoredProcedure, _
			'''' "ZB_SECURITY_PKG.ZBP_DEL_ACCOUNT_GRPS", _
			'''' New OracleParameter() { _
			'''' New OracleParameter("@P_ACCOUNT_SEQ_ID", ACCOUNT_SEQ_ID), _
			'''' New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID) _
			' })
			' Comma seporate the roles

			For Each group In groups
				newGroups += group & ","
			Next group
			If Not newGroups = String.Empty Then
				newGroups = Left(newGroups, Len(newGroups) - 1)
			End If

			' Add the selected roles
			OracleHelper.ExecuteNonQuery(trans, _
			 CommandType.StoredProcedure, _
			 "ZB_SECURITY_PKG.ZBP_UPDATE_ACCOUNT_GRPS", _
			 New OracleParameter() { _
			 New OracleParameter("@P_ACCOUNT_SEQ_ID", ACCOUNT_SEQ_ID), _
			 New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID), _
			 New OracleParameter("@P_GRPS", newGroups), _
			 New OracleParameter("@P_ADDUPD_BY", ACCOUNT_SEQ_ID) _
			 })
			trans.Commit()			' commit the changes
		Catch ex As Exception
			trans.Rollback()			' rollback anything
			Throw ex
		Finally		 ' clean up
			' close and dispose of the db connection object
			If Not dbConn Is Nothing Then
				dbConn.Close()
				dbConn.Dispose()
			End If
			' dispose of the transaction object
			If Not trans Is Nothing Then
				trans.Dispose()
			End If
		End Try
	End Sub

	Public Function GetGroupsFromDB(ByVal ACCOUNT_SEQ_ID As Integer, Optional ByVal BUSINESS_UNIT_SEQ_ID As Integer = 1) As String() Implements IAccount.GetGroupsFromDB
		Dim ClientRoles As New ArrayList
		Dim reader As OracleDataReader = Nothing
		Try
			reader = OracleHelper.ExecuteReader( _
			   ConnectionString, _
			   CommandType.StoredProcedure, _
			   "ZB_SECURITY_PKG.ZBP_GET_GRPS_FOR_ACCOUNT", _
			   New OracleParameter() { _
			 New OracleParameter("@P_Account_seq_id", ACCOUNT_SEQ_ID), _
			 New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID) _
			   } _
			   )
			While reader.Read()
				ClientRoles.Add(reader("RLS"))
			End While
		Catch ex As Exception
			Throw ex
		Finally
			If Not (reader Is Nothing) Then
				CType(reader, IDisposable).Dispose()
			End If
		End Try
		' Return the String array of roles
		Return CType(ClientRoles.ToArray(GetType(String)), String())

	End Function

	Public Function GetGroupsFromDBByBusinessUnit(ByVal ACCOUNT_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As String() Implements IAccount.GetGroupsFromDBByBusinessUnit
		Dim ClientGroups As New ArrayList
		Dim reader As OracleDataReader = Nothing
		Try
			reader = OracleHelper.ExecuteReader( _
			   ConnectionString, _
			   CommandType.StoredProcedure, _
			   "ZB_SECURITY_PKG.ZBP_GET_GRPS_FOR_ACCOUNT_BY_BU", _
			   New OracleParameter() { _
			 New OracleParameter("@P_Account_seq_id", ACCOUNT_SEQ_ID), _
			 New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID) _
			   } _
			   )
			While reader.Read()
				ClientGroups.Add(reader("GRPS"))
			End While
		Catch ex As Exception
			Throw ex
		Finally
			If Not (reader Is Nothing) Then
				CType(reader, IDisposable).Dispose()
			End If
		End Try
		' Return the String array of groups
		Return CType(ClientGroups.ToArray(GetType(String)), String())
	End Function
End Class
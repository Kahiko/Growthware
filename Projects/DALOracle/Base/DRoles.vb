Imports System.Configuration
Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports Common
Imports Common.Oracle
Imports Common.Security.BaseSecurity
Imports DALInterface.Base.Interfaces

Public Class DRoles
	Implements IRoles

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
			_ConnectionString = ConnectionHelper.GetConnectionString("OracleDAL")
		End If
		Return _ConnectionString
	End Function
#End Region

    Public Sub AddRole(ByVal roleName As String, ByVal description As String, ByVal BUSINESS_UNIT_SEQ_ID As Integer, Optional ByVal Account_Seq_Id As Integer = 1) Implements IRoles.AddRole
        Try
			OracleHelper.ExecuteNonQuery(ConnectionString, _
			  CommandType.StoredProcedure, _
			 "ZB_SECURITY_PKG.ZBP_add_Role", _
			 New OracleParameter() { _
			  New OracleParameter("@P_ROLE_NAME", roleName), _
			  New OracleParameter("@P_DESCRIPTION", description), _
			  New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID), _
			  New OracleParameter("@P_ADDUPD_BY", Account_Seq_Id) _
			 })
        Catch ex As Exception
            Throw ex
            'Throw New ApplicationException(ResourceManager.GetString("RES_ExceptionCantCreateOrder"), e)
        End Try
    End Sub

    Public Sub DeleteRole(ByVal roleName As String, ByVal BUSINESS_UNIT_SEQ_ID As Integer, Optional ByVal Account_Seq_Id As Integer = 1) Implements IRoles.DeleteRole
		Dim retVal As Integer = 1
		Dim returnParam As New OracleParameter("@P_ISVALID", OracleDbType.Int32)
        Try
            returnParam.Direction = ParameterDirection.Output
			OracleHelper.ExecuteNonQuery(ConnectionString, _
			 CommandType.StoredProcedure, _
			 "ZB_SECURITY_PKG.ZBP_DEL_ROLE", _
			 New OracleParameter() { _
			  New OracleParameter("@P_ROLE_NAME", roleName), _
			  New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID), _
			  returnParam, _
			  New OracleParameter("@P_ADDUPD_BY", Account_Seq_Id) _
			 })
			Try
				retVal = Fix(returnParam.Value)
			Catch ex As Exception

			End Try
		Catch ex As Exception
			Throw ex
			'Throw New ApplicationException(ResourceManager.GetString("RES_ExceptionCantCreateOrder"), e)
		End Try
    End Sub

    Public Sub UpdateRole(ByVal originalRoleName As String, ByVal newRoleName As String, ByVal newRoleDescription As String, Optional ByVal Account_Seq_Id As Integer = 1) Implements IRoles.UpdateRole
        Try

            OracleHelper.ExecuteNonQuery(ConnectionString, _
             CommandType.StoredProcedure, _
             "ZB_SECURITY_PKG.ZBP_update_Role", _
             New OracleParameter() { _
              New OracleParameter("@P_originalRoleName", originalRoleName), _
              New OracleParameter("@P_ROLE_NAME", newRoleName), _
              New OracleParameter("@P_DESCRIPTION", newRoleDescription), _
              New OracleParameter("@P_ADDUPD_BY", Account_Seq_Id) _
             })
        Catch ex As Exception
            Throw ex
            'Throw New ApplicationException(ResourceManager.GetString("RES_ExceptionCantCreateOrder"), e)
        End Try
    End Sub
    Public Function GetRolesForBusinessUnit(ByVal BUSINESS_UNIT_SEQ_ID As Integer) As ArrayList Implements IRoles.GetRolesForBusinessUnit
        Dim colRoles As New ArrayList
        Dim roleRow As DataRow
        Dim dtblRoles As DataTable
        Dim dsRoles As New DataSet
        Try
            dtblRoles = GetRolesForBusinessUnitFromDB(dsRoles, BUSINESS_UNIT_SEQ_ID).Tables(0)
            For Each roleRow In dtblRoles.Rows
                colRoles.Add(CStr(roleRow("ROLE_NAME")))
            Next roleRow
        Catch ex As Exception
            Throw ex
        Finally
            If Not dtblRoles Is Nothing Then
                dtblRoles.Dispose()
                dtblRoles = Nothing
            End If
            If Not dsRoles Is Nothing Then
                dsRoles.Dispose()
                dsRoles = Nothing
            End If
        End Try
        Return colRoles
    End Function

    Public Function GetAllRoles() As ArrayList Implements IRoles.GetAllRoles
        Dim colRoles As New ArrayList
        Dim roleRow As DataRow
        Dim dtblRoles As DataTable
        Dim dsRoles As New DataSet
        Try
            dtblRoles = GetAllRolesFromDB(dsRoles).Tables(0)
            For Each roleRow In dtblRoles.Rows
                colRoles.Add(CStr(roleRow("ROLE_NAME")))
            Next roleRow
        Catch ex As Exception
            Throw ex
        Finally
            If Not dtblRoles Is Nothing Then
                dtblRoles.Dispose()
                dtblRoles = Nothing
            End If
            If Not dsRoles Is Nothing Then
                dsRoles.Dispose()
                dsRoles = Nothing
            End If
        End Try
        Return colRoles
    End Function

    Private Function GetAllRolesFromDB(ByRef dstRoles As DataSet) As DataSet
        Try
            dstRoles = OracleHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_get_All_Rls")
        Catch ex As Exception
            Throw ex
        End Try
        Return dstRoles
    End Function

    Private Function GetRolesForBusinessUnitFromDB(ByRef dstRoles As DataSet, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As DataSet
        Try
            dstRoles = OracleHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_GET_ALL_RLS_FOR_BU", New OracleParameter() { _
             New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID) _
            })
        Catch ex As Exception
            Throw ex
        End Try
        Return dstRoles
    End Function

    Private Function GetGroupsForBusinessUnitFromDB(ByRef dstGroups As DataSet, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As DataSet
        Try
            dstGroups = OracleHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_GET_ALL_GRPS_FOR_BU", New OracleParameter() { _
             New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID) _
            })
        Catch ex As Exception
            Throw ex
        End Try
        Return dstGroups
    End Function


    Private Function GetModuleBusinessUnitSelectedRoles(ByRef retDS As DataSet, ByVal RoleType As Integer, ByVal MODULE_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As DataSet
        Try
            retDS = OracleHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_GET_MODULE_BU_SELECTED_RLS", New OracleParameter() { _
             New OracleParameter("@P_PERMISSIONS_SEQ_ID", RoleType), _
             New OracleParameter("@P_MODULE_SEQ_ID", MODULE_SEQ_ID), _
             New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID) _
            })
        Catch ex As Exception
            Throw ex
        End Try
        Return retDS
    End Function

    Private Function GetModuleBusinessUnitSelectedGroups(ByRef retDS As DataSet, ByVal GroupType As Integer, ByVal MODULE_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As DataSet
        Try
            retDS = OracleHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_GET_MODULE_BU_SELTD_GRPS", New OracleParameter() { _
             New OracleParameter("@P_PERMISSIONS_SEQ_ID", GroupType), _
             New OracleParameter("@P_MODULE_SEQ_ID", MODULE_SEQ_ID), _
             New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID) _
            })
        Catch ex As Exception
            Throw ex
        End Try
        Return retDS
    End Function


    Private Function GetDropBoxBusinessUnitSelectedRoles(ByRef retDS As DataSet, ByVal RoleType As Integer, ByVal MODULE_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As DataSet
        Try
			retDS = OracleHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_GET_DROP_BOX_BU_SELTD_RLS", New OracleParameter() { _
			 New OracleParameter("@P_PERMISSIONS_SEQ_ID", RoleType), _
			 New OracleParameter("@P_DROP_BOX_SEQ_ID", MODULE_SEQ_ID), _
			 New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID) _
			})
        Catch ex As Exception
            Throw ex
        End Try
        Return retDS
    End Function

    Public Function GetModuleBusinessUnitSelectedRoles(ByVal RoleType As Integer, ByVal MODULE_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As System.Collections.ArrayList Implements IRoles.GetModuleBusinessUnitSelectedRoles
        Dim colRoles As New ArrayList
        Dim roleRow As DataRow
        Dim dtblRoles As DataTable
        Dim dsRoles As New DataSet
        Try
            dtblRoles = GetModuleBusinessUnitSelectedRoles(dsRoles, RoleType, MODULE_SEQ_ID, BUSINESS_UNIT_SEQ_ID).Tables(0)
            For Each roleRow In dtblRoles.Rows
                colRoles.Add(CStr(roleRow("ROLE_NAME")))
            Next roleRow
        Catch ex As Exception
            Throw ex
        Finally
            If Not dtblRoles Is Nothing Then
                dtblRoles.Dispose()
                dtblRoles = Nothing
            End If
            If Not dsRoles Is Nothing Then
                dsRoles.Dispose()
                dsRoles = Nothing
            End If
        End Try
        Return colRoles
    End Function

    Public Function GetAllAccountsForRoleByBusinessUnit(ByVal ROLE_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As System.Collections.ArrayList Implements IRoles.GetAllAccountsForRoleByBusinessUnit
        Dim colAccounts As New ArrayList
        Dim accountsRow As DataRow
        Dim dsAccounts As DataSet
        Dim dtAccounts As DataTable
        Try
            dsAccounts = OracleHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_GET_ALL_ACCTS_ROLE_BY_BU", New OracleParameter() {New OracleParameter("@P_ROLE_SEQ_ID", ROLE_SEQ_ID), New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID)})
            dtAccounts = dsAccounts.Tables(0)
            For Each accountsRow In dtAccounts.Rows
                colAccounts.Add(CStr(accountsRow("ACCOUNT")))
            Next accountsRow
        Catch ex As Exception
            Throw ex
        End Try
        Return colAccounts
    End Function

    Public Function GetAllAccountsNotInRoleByBusinessUnit(ByVal ROLE_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As System.Collections.ArrayList Implements IRoles.GetAllAccountsNotInRoleByBusinessUnit
        Dim colAccounts As New ArrayList
        Dim accountsRow As DataRow
        Dim dsAccounts As DataSet
        Dim dtAccounts As DataTable
        Try
            dsAccounts = OracleHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_GET_ALL_ACCTS_N_ROLE_BY_BU", New OracleParameter() {New OracleParameter("@P_ROLE_SEQ_ID", ROLE_SEQ_ID), New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID)})
            dtAccounts = dsAccounts.Tables(0)
            For Each accountsRow In dtAccounts.Rows
                colAccounts.Add(CStr(accountsRow("ACCOUNT")))
            Next accountsRow
        Catch ex As Exception
            Throw ex
        End Try
        Return colAccounts
    End Function

    Public Function UpdateAllAccountsForRoleByBusinessUnit(ByVal ROLE_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer, ByVal Accounts() As String, Optional ByVal Account_Seq_Id As Integer = 1) As Boolean Implements IRoles.UpdateAllAccountsForRoleByBusinessUnit
        Dim success As Boolean = False
        Dim dbConn As New OracleConnection(ConnectionString)
        dbConn.Open()
        Dim trans As OracleTransaction = dbConn.BeginTransaction
        Dim account As String
        Try
            ' delete all the accounts for this role/BusinessUnit
			OracleHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_DEL_ALL_ACCTS_ROLE_BY_BU", New OracleParameter() {New OracleParameter("@P_ROLE_SEQ_ID", ROLE_SEQ_ID), New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID), New OracleParameter("@P_ADDUPD_BY", Account_Seq_Id)})
            For Each account In Accounts
                OracleHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_UPD_ALL_ACCTS_ROLE_BY_BU", _
                New OracleParameter() { _
                New OracleParameter("@P_ROLE_SEQ_ID", ROLE_SEQ_ID), _
                New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID), _
                New OracleParameter("@P_Account", account), _
                New OracleParameter("@P_ADDUPD_BY", Account_Seq_Id) _
                })
            Next
            trans.Commit()
            success = True
        Catch ex As Exception
            Throw ex
        Finally
            If Not trans Is Nothing Then
                trans.Dispose()
                trans = Nothing
            End If
            If Not dbConn Is Nothing Then
                dbConn.Close()
                dbConn.Dispose()
                dbConn = Nothing
            End If
        End Try
        Return success
    End Function

    Public Function GetRoleNameByID(ByVal ROLE_SEQ_ID As Integer) As String Implements IRoles.GetRoleNameByID
        Dim dsRole As DataSet
        Try
            dsRole = OracleHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_get_Role_Name_By_ID", New OracleParameter() {New OracleParameter("@P_ROLE_SEQ_ID", ROLE_SEQ_ID)})
            Return CStr(dsRole.Tables(0).Rows(0).Item(0))
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetDropBoxBusinessUnitSelectedRoles1(ByVal RoleType As Integer, ByVal DROP_BOX_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As System.Collections.ArrayList Implements IRoles.GetDropBoxBusinessUnitSelectedRoles
        Dim colRoles As New ArrayList
        Dim roleRow As DataRow
        Dim dtblRoles As DataTable
        Dim dsRoles As New DataSet
        Try
            dtblRoles = GetDropBoxBusinessUnitSelectedRoles(dsRoles, RoleType, DROP_BOX_SEQ_ID, BUSINESS_UNIT_SEQ_ID).Tables(0)
            For Each roleRow In dtblRoles.Rows
                colRoles.Add(CStr(roleRow("ROLE_NAME")))
            Next roleRow
        Catch ex As Exception
            Throw ex
        Finally
            If Not dtblRoles Is Nothing Then
                dtblRoles.Dispose()
                dtblRoles = Nothing
            End If
            If Not dsRoles Is Nothing Then
                dsRoles.Dispose()
                dsRoles = Nothing
            End If
        End Try
        Return colRoles
    End Function

    Public Function GetModuleBusinessUnitSelectedGroups(ByVal GroupType As Integer, ByVal MODULE_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As System.Collections.ArrayList Implements IRoles.GetModuleBusinessUnitSelectedGroups
        Dim colGroups As New ArrayList
        Dim GroupRow As DataRow
        Dim dtblGroups As DataTable
        Dim dsGroups As New DataSet
        Try
            dtblGroups = GetModuleBusinessUnitSelectedGroups(dsGroups, GroupType, MODULE_SEQ_ID, BUSINESS_UNIT_SEQ_ID).Tables(0)
            For Each GroupRow In dtblGroups.Rows
                colGroups.Add(CStr(GroupRow("GROUP_NAME")))
            Next GroupRow
        Catch ex As Exception
            Throw ex
        Finally
            If Not dtblGroups Is Nothing Then
                dtblGroups.Dispose()
                dtblGroups = Nothing
            End If
            If Not dsGroups Is Nothing Then
                dsGroups.Dispose()
                dsGroups = Nothing
            End If
        End Try
        Return colGroups
    End Function

    Public Function GetGroupsForBusinessUnit(ByVal BUSINESS_UNIT_SEQ_ID As Integer) As System.Collections.ArrayList Implements IRoles.GetGroupsForBusinessUnit
        Dim colGroups As New ArrayList
        Dim GroupRow As DataRow
        Dim dtblGroups As DataTable
        Dim dsGroups As New DataSet
        Try
            dtblGroups = GetGroupsForBusinessUnitFromDB(dsGroups, BUSINESS_UNIT_SEQ_ID).Tables(0)
            For Each GroupRow In dtblGroups.Rows
                colGroups.Add(CStr(GroupRow("GROUP_NAME")))
            Next GroupRow
        Catch ex As Exception
            Throw ex
        Finally
            If Not dtblGroups Is Nothing Then
                dtblGroups.Dispose()
                dtblGroups = Nothing
            End If
            If Not dsGroups Is Nothing Then
                dsGroups.Dispose()
                dsGroups = Nothing
            End If
        End Try
        Return colGroups
    End Function
End Class
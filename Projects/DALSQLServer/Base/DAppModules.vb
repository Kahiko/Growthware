Imports System.Configuration
Imports System.Data.SqlClient
Imports Common
Imports Common.SQLServer
Imports Common.Security.BaseSecurity
Imports DALInterface.Base.Interfaces
Imports DALModel.Base
Imports DALModel.Base.Modules

Public Class DAppModules
	Implements IAppModules

#Region " Private Objects "
    Private _ConnectionString As String = String.Empty
#End Region

    '*********************************************************************
    '
    ' GetAllModulesFromDB Method
    '
    ' Returns all modules from the database.
    '
    '*********************************************************************
	Public Function GetModuleCollectionFromDB(ByVal BUSINESS_UNIT_SEQ_ID As Integer) As MModuleCollection Implements IAppModules.GetModuleCollectionFromDB
		Dim dstModules As New DataSet
		Dim reader As SqlDataReader = Nothing
		Dim hasSecurity As Boolean = False
		Try
			reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, _
			   "ZBP_get_All_Modules")
			SqlHelperExtension.Fill(reader, dstModules, "Modules", 0, 0)
            reader = SqlHelper.ExecuteReader( _
            ConnectionString, _
            CommandType.StoredProcedure, _
            "ZBP_GET_MODULE_RLS_BY_BU", New SqlParameter() {New SqlParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID)})

			If reader.HasRows Then
				hasSecurity = True
			End If
			SqlHelperExtension.Fill(reader, dstModules, "ModuleSecurity", 0, 0)
			reader.Close()
		Catch ex As Exception
			Throw ex
		Finally
			If Not (reader Is Nothing) Then
				CType(reader, IDisposable).Dispose()
			End If
		End Try
		If hasSecurity Then
			Return CalculateModules(dstModules)
		Else
			Return Nothing
		End If
	End Function	'GetAllModulesFromDB

	'*********************************************************************
	'
	' AddModule Method
	'
	' Adds a module to the database.
	'
	'*********************************************************************
    Public Function AddModule(ByVal profile As MModuleProfileInfo, Optional ByVal Account_Seq_id As Integer = 1) As Integer Implements IAppModules.AddModule
        Dim retVal As Integer
        Try
            Dim returnParam As New SqlParameter("@P_RETURN_VALUE", SqlDbType.Int)
            returnParam.Direction = ParameterDirection.ReturnValue

            SqlHelper.ExecuteNonQuery( _
             ConnectionString, _
             CommandType.StoredProcedure, _
             "ZBP_add_Module", _
             New SqlParameter() { _
             New SqlParameter("@P_Name", profile.Name), _
             New SqlParameter("@P_Description", profile.Description), _
             New SqlParameter("@P_Source", profile.Source), _
             New SqlParameter("@P_Enable_View_State", profile.EnableViewState), _
             New SqlParameter("@P_ParentID", profile.ParentID), _
             New SqlParameter("@P_IS_NAV", profile.IS_NAV), _
             New SqlParameter("@P_NAV_TYPE_SEQ_ID", profile.NAV_TYPE_SEQ_ID), _
             New SqlParameter("@P_Action", profile.Action), returnParam, _
             New SqlParameter("@P_ADDUPD_BY", Account_Seq_id) _
             })
            retVal = Fix(returnParam.Value)
        Catch ex As Exception
            Throw ex
            'Throw New ApplicationException(ResourceManager.GetString("RES_ExceptionCantCreateOrder"), e)
        End Try
        Return retVal
    End Function   'AddModule

    '*********************************************************************
    '
    ' UpdateProfile Method
    '
    ' Updates a module profile information in the database.
    '
    '*********************************************************************
    Public Function UpdateProfile(ByVal profile As MModuleProfileInfo, Optional ByVal Account_Seq_id As Integer = 1) As Integer Implements IAppModules.UpdateProfile
        Dim retVal As Integer
        Try
            Dim returnParam As New SqlParameter("@P_RETURN_VALUE", SqlDbType.Int)
            returnParam.Direction = ParameterDirection.ReturnValue

            SqlHelper.ExecuteNonQuery( _
             ConnectionString, _
             CommandType.StoredProcedure, _
             "ZBP_update_Module_Profile", _
             New SqlParameter() { _
             New SqlParameter("@P_MODULE_SEQ_ID", profile.MODULE_SEQ_ID), _
             New SqlParameter("@P_Name", profile.Name), _
             New SqlParameter("@P_Description", profile.Description), _
             New SqlParameter("@P_Source", profile.Source), _
             New SqlParameter("@P_Enable_View_State", profile.EnableViewState), _
             New SqlParameter("@P_IS_NAV", profile.IS_NAV), _
             New SqlParameter("@P_NAV_TYPE_SEQ_ID", profile.NAV_TYPE_SEQ_ID), _
             New SqlParameter("@P_ParentID", profile.ParentID), _
             New SqlParameter("@P_Action", profile.Action), _
New SqlParameter("@P_ADDUPD_BY", Account_Seq_id), _
            returnParam _
             })
            retVal = Fix(returnParam.Value)
        Catch ex As Exception
            Throw ex
            'Throw New ApplicationException(ResourceManager.GetString("RES_ExceptionCantCreateOrder"), e)
        End Try
        Return retVal
    End Function 'UpdateProfile

    '*********************************************************************
    '
    ' DeleteModule Method
    '
    ' Deletes a module from the database.
    '
    '*********************************************************************
    Public Function DeleteModule(ByVal MODULE_SEQ_ID As Integer, Optional ByVal Account_Seq_id As Integer = 1) As Boolean Implements IAppModules.DeleteModule
        Dim retVal As Boolean = False
        Try
            SqlHelper.ExecuteNonQuery(ConnectionString, _
            CommandType.StoredProcedure, _
            "ZBP_DEL_MODULE", _
            New SqlParameter() { _
            New SqlParameter("@P_MODULE_SEQ_ID", MODULE_SEQ_ID), _
            New SqlParameter("@P_ADDUPD_BY", Account_Seq_id) _
            })
        Catch ex As Exception
            Throw ex
        End Try
        If Err.Number = 0 Then retVal = True
        Return retVal
    End Function ' DeleteModule
    '*********************************************************************
    ' AddModuleRoles Method
    '
    ' Adds new roles to the TBL_MODULES_SECURITY table.
    '
    '*********************************************************************
    Public Sub AddModuleRoles(ByVal MODULE_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer, ByVal moduleRoleType As MRoleType.value, ByVal roles() As String, Optional ByVal Account_Seq_id As Integer = 1) Implements IAppModules.AddModuleRoles
        Dim role As String
        Dim newRoles As String = String.Empty
        Dim dbConn As New SqlConnection(ConnectionString)
        dbConn.Open()
        Dim trans As SqlTransaction = dbConn.BeginTransaction
        Try
            '****NOTE : COMMEENTED ON 26TH SEPT 2006. THIS STORED PROC IS EXECUTED 
            ' IN  ZBP_UPDATE_MODULE_RLS STORED PROC - ARAVIND

            ' delete the roles
            '''SqlHelper.ExecuteNonQuery(trans, _
            '''CommandType.StoredProcedure, _
            '''"ZBP_DEL_MODULE_SECTY_BY_ROLE", _
            '''New SqlParameter() { _
            '''New SqlParameter("@P_MODULE_SEQ_ID", MODULE_SEQ_ID), _
            '''New SqlParameter("@P_PERMISSIONS_SEQ_ID", moduleRoleType), _
            '''New SqlParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID), _
            '''New SqlParameter("@P_ADDUPD_BY", Account_Seq_id) _
            '})
            ' Comma seporate the roles
            For Each role In roles
                newRoles += role & ","
            Next role
            If Not newRoles.Trim = String.Empty Then
                newRoles = Left(newRoles, Len(newRoles) - 1)
            Else
                newRoles = " "
            End If
            'P_ROLES
            SqlHelper.ExecuteNonQuery(trans, _
            CommandType.StoredProcedure, _
            "ZBP_UPDATE_MODULE_RLS", _
            New SqlParameter() { _
            New SqlParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID), _
            New SqlParameter("@P_MODULE_SEQ_ID", MODULE_SEQ_ID), _
            New SqlParameter("@P_PERMISSIONS_SEQ_ID", moduleRoleType), _
            New SqlParameter("@P_ROLES", newRoles), _
            New SqlParameter("@P_ADDUPD_BY", Account_Seq_id) _
            })
            trans.Commit()
        Catch ex As Exception
            If Not trans Is Nothing Then trans.Rollback()
            Throw ex
        Finally
            If Not dbConn Is Nothing Then
                dbConn.Close()
                dbConn.Dispose()
            End If
            If Not trans Is Nothing Then
                trans.Dispose()
            End If
        End Try
    End Sub

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

    '*********************************************************************
    '
    ' PopulateProfileInfoFromSqlDataReader Method
    '
    ' Creates a ModuleProfileInfo object from a SqlDataReader.
    '
    '*********************************************************************
    Private Shared Function PopulateProfileFromSqlDataReader(ByVal dr As SqlDataReader) As MModuleProfileInfo
        Dim profile As New MModuleProfileInfo

        If Not dr("Action") Is Nothing Then
            profile.Action = CStr(dr("Action"))
        End If
        profile.MODULE_SEQ_ID = Fix(dr("MODULE_SEQ_ID"))
        profile.Name = CStr(dr("Name"))
        profile.Description = CStr(dr("Description"))
        profile.Source = CStr(dr("Source"))
        profile.EnableViewState = CBool(dr("EnableViewState"))
        profile.IS_NAV = CBool(dr("IS_NAV"))
        profile.NAV_TYPE_SEQ_ID = CInt(dr("NAV_TYPE_SEQ_ID"))
        profile.ParentID = CInt(dr("ParentID"))
        profile.Action = CStr(dr("Action"))
        Return profile
    End Function 'PopulateProfileFromSqlDataReader

    '*********************************************************************
    '
    ' CalculateSections Method
    '
    ' Calculate section information such as section paths and 
    ' inherited properties by iterating through each 
    ' row in a DataSet containing section rows.
    '
    '*********************************************************************
    Private Shared Function CalculateModules(ByVal dstModules As DataSet) As MModuleCollection
        Dim moduleCollection As New MModuleCollection
        Dim dtblModules As DataTable = dstModules.Tables("Modules")
        Dim dtblModuleSecurity As DataTable = dstModules.Tables("ModuleSecurity")
        Dim drowRoles() As DataRow

        ' Calculated inherited properties for each module
        Dim drowModule As DataRow
        For Each drowModule In dtblModules.Rows
            If Not drowModule("MODULE_SEQ_ID") Is Nothing Then
                ' Add to section collection
                drowRoles = dtblModuleSecurity.Select(String.Format("MODULE_SEQ_ID={0}", drowModule("MODULE_SEQ_ID")))
                ' Retrieve roles
                moduleCollection.Add(Trim(CStr(drowModule("module_action"))), New MModuleProfileInfo(drowModule, drowRoles))
            End If
        Next drowModule

        Return moduleCollection
    End Function 'CalculateModules

	Public Sub AddModuleGroups(ByVal MODULE_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer, ByVal moduleGroupType As DALModel.Base.MGroupType.value, ByVal groups() As String, Optional ByVal Account_Seq_id As Integer = 1) Implements IAppModules.AddModuleGroups
		Dim group As String
		Dim newGroups As String = String.Empty
		Dim dbConn As New SqlConnection(ConnectionString)
		dbConn.Open()
		Dim trans As SqlTransaction = dbConn.BeginTransaction
		Try
            ' delete the GROUPS
			''''SqlHelper.ExecuteNonQuery(trans, _
			''''CommandType.StoredProcedure, _
			''''"ZBP_DEL_MODULE_SECURITY_BY_GRP", _
			''''New SqlParameter() { _
			''''New SqlParameter("@P_MODULE_SEQ_ID", MODULE_SEQ_ID), _
			''''New SqlParameter("@P_PERMISSIONS_SEQ_ID", moduleGroupType), _
			''''   New SqlParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID) _
			''})
			' Comma seporate the groups
			For Each group In groups
				newGroups += group & ","
			Next group
			If Not newGroups.Trim = String.Empty Then
				newGroups = Left(newGroups, Len(newGroups) - 1)
			Else
				newGroups = " "
			End If
			'P_ROLES
			SqlHelper.ExecuteNonQuery(trans, _
			CommandType.StoredProcedure, _
			"ZBP_UPDATE_MODULE_GRPS", _
			New SqlParameter() { _
			New SqlParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID), _
			New SqlParameter("@P_MODULE_SEQ_ID", MODULE_SEQ_ID), _
			New SqlParameter("@P_PERMISSIONS_SEQ_ID", moduleGroupType), _
			New SqlParameter("@P_GRPS", newGroups), _
			New SqlParameter("@P_ADDUPD_BY", Account_Seq_id) _
			})
			trans.Commit()
		Catch ex As Exception
			If Not trans Is Nothing Then trans.Rollback()
			Throw ex
		Finally
			If Not dbConn Is Nothing Then
				dbConn.Close()
				dbConn.Dispose()
			End If
			If Not trans Is Nothing Then
				trans.Dispose()
			End If
		End Try
	End Sub
End Class
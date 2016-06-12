Imports Common
Imports Common.Oracle
Imports DALInterface.Base.Interfaces
Imports DALModel.Base
Imports DALModel.Base.Modules
Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports System.Configuration

Public Class DAppModules
	Implements IAppModules

#Region " Private Objects "
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
	'*********************************************************************
	'
	' PopulateProfileInfoFromSqlDataReader Method
	'
	' Creates a ModuleProfileInfo object from a SqlDataReader.
	'
	'*********************************************************************
	Private Shared Function PopulateProfileFromSqlDataReader(ByVal dr As OracleDataReader) As MModuleProfileInfo
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
	End Function	'PopulateProfileFromSqlDataReader

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
	End Function	'CalculateModules

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
		Dim reader As OracleDataReader = Nothing
		Dim hasSecurity As Boolean = False
		Try
			reader = OracleHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, _
			   "ZB_SECURITY_PKG.ZBP_get_All_Modules")
			OracleHelperExtension.Fill(reader, dstModules, "Modules", 0, 0)
            reader = OracleHelper.ExecuteReader( _
            ConnectionString, _
            CommandType.StoredProcedure, _
            "ZB_SECURITY_PKG.ZBP_GET_MODULE_RLS_BY_BU", New OracleParameter() {New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID)})

			If reader.HasRows Then
				hasSecurity = True
			End If
			OracleHelperExtension.Fill(reader, dstModules, "ModuleSecurity", 0, 0)
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
    Public Function AddModule(ByVal profile As MModuleProfileInfo, Optional ByVal Account_Seq_Id As Integer = 1) As Integer Implements IAppModules.AddModule
		Dim retVal As Integer = 0
        Try
			Dim isNav As Integer = CInt(profile.IS_NAV)
			If isNav < 0 Then
				isNav = 1
			End If
			Dim EnableViewState As Integer = CInt(profile.EnableViewState)
			If EnableViewState < 0 Then
				EnableViewState = 1
			End If

			OracleHelper.ExecuteNonQuery( _
			 ConnectionString, _
			 CommandType.StoredProcedure, _
			 "ZB_SECURITY_PKG.ZBP_add_Module", _
			 New OracleParameter() { _
			 New OracleParameter("@P_Name", profile.Name), _
			 New OracleParameter("@P_Description", profile.Description), _
			 New OracleParameter("@P_Source", profile.Source), _
			 New OracleParameter("@P_Enable_View_State", EnableViewState), _
			 New OracleParameter("@P_IS_NAV", isNav), _
			 New OracleParameter("@P_NAV_TYPE_SEQ_ID", profile.NAV_TYPE_SEQ_ID), _
			 New OracleParameter("@P_ParentID", profile.ParentID), _
			 New OracleParameter("@P_Action", profile.Action), _
			 New OracleParameter("@P_ADDUPD_BY", Account_Seq_Id) _
			 })
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
    Public Function UpdateProfile(ByVal profile As MModuleProfileInfo, Optional ByVal Account_Seq_Id As Integer = 1) As Integer Implements IAppModules.UpdateProfile
		Dim retVal As Integer = 1
		Dim isNav As Integer = CInt(profile.IS_NAV)
		If isNav < 0 Then
			isNav = 1
		End If
		Dim EnableViewState As Integer = CInt(profile.EnableViewState)
		If EnableViewState < 0 Then
			EnableViewState = 1
		End If
		Try
			OracleHelper.ExecuteNonQuery( _
			 ConnectionString, _
			 CommandType.StoredProcedure, _
			 "ZB_SECURITY_PKG.ZBP_update_Module_Profile", _
			 New OracleParameter() { _
			 New OracleParameter("@P_MODULE_SEQ_ID", profile.MODULE_SEQ_ID), _
			 New OracleParameter("@P_Name", profile.Name), _
			 New OracleParameter("@P_Description", profile.Description), _
			 New OracleParameter("@P_Source", profile.Source), _
			 New OracleParameter("@P_Enable_View_State", EnableViewState), _
			 New OracleParameter("@P_IS_NAV", isNav), _
			 New OracleParameter("@P_NAV_TYPE_SEQ_ID", profile.NAV_TYPE_SEQ_ID), _
			 New OracleParameter("@P_ParentID", profile.ParentID), _
			 New OracleParameter("@P_Action", profile.Action), _
			 New OracleParameter("@P_ADDUPD_BY", Account_Seq_Id) _
			 })
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
    Public Function DeleteModule(ByVal MODULE_SEQ_ID As Integer, Optional ByVal Account_Seq_Id As Integer = 1) As Boolean Implements IAppModules.DeleteModule
        Dim retVal As Boolean = False
        Try
            OracleHelper.ExecuteNonQuery(ConnectionString, _
            CommandType.StoredProcedure, _
            "ZB_SECURITY_PKG.ZBP_DEL_MODULE", _
            New OracleParameter() { _
            New OracleParameter("@P_MODULE_SEQ_ID", MODULE_SEQ_ID), _
            New OracleParameter("@P_ADDUPD_BY", Account_Seq_Id) _
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
	Public Sub AddModuleRoles(ByVal MODULE_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer, ByVal moduleRoleType As MRoleType.value, ByVal roles() As String, Optional ByVal Account_Seq_Id As Integer = 1) Implements IAppModules.AddModuleRoles
		Dim role As String
		Dim newRoles As String = String.Empty
		Dim roleType As Integer = moduleRoleType
		Dim dbConn As New OracleConnection(ConnectionString)
		dbConn.Open()
		Dim trans As OracleTransaction = dbConn.BeginTransaction

		Try
			' delete the roles
            ''OracleHelper.ExecuteNonQuery(trans, _
            ''CommandType.StoredProcedure, _
            ''"ZB_SECURITY_PKG.ZBP_DEL_MODULE_SECTY_BY_ROLE", _
            ''New OracleParameter() { _
            ''New OracleParameter("@P_MODULE_SEQ_ID", MODULE_SEQ_ID), _
            ''New OracleParameter("@P_PERMISSIONS_SEQ_ID", roleType), _
            ''New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID), _
            ''New OracleParameter("@P_ADDUPD_BY", Account_Seq_Id) _
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
            OracleHelper.ExecuteNonQuery(trans, _
            CommandType.StoredProcedure, _
            "ZB_SECURITY_PKG.ZBP_UPDATE_MODULE_RLS", _
            New OracleParameter() { _
            New OracleParameter("@P_ROLES", newRoles), _
            New OracleParameter("@P_MODULE_SEQ_ID", MODULE_SEQ_ID), _
            New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID), _
            New OracleParameter("@P_PERMISSIONS_SEQ_ID", roleType), _
            New OracleParameter("@P_ADDUPD_BY", Account_Seq_Id) _
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

	Public Sub AddModuleGroups(ByVal MODULE_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer, ByVal moduleGroupType As DALModel.Base.MGroupType.value, ByVal groups() As String, Optional ByVal Account_Seq_Id As Integer = 1) Implements IAppModules.AddModuleGroups
		Dim group As String
		Dim newGroups As String = String.Empty
		Dim dbConn As New OracleConnection(ConnectionString)
		dbConn.Open()
		Dim trans As OracleTransaction = dbConn.BeginTransaction
		Try
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
			OracleHelper.ExecuteNonQuery(trans, _
			CommandType.StoredProcedure, _
			"ZB_SECURITY_PKG.ZBP_UPDATE_MODULE_GRPS", _
			New OracleParameter() { _
			New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID), _
			New OracleParameter("@P_MODULE_SEQ_ID", MODULE_SEQ_ID), _
			New OracleParameter("@P_PERMISSIONS_SEQ_ID", moduleGroupType), _
			New OracleParameter("@P_GRPS", newGroups), _
			New OracleParameter("@P_ADDUPD_BY", Account_Seq_Id) _
			})
			trans.Commit()
		Catch ex As Exception
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
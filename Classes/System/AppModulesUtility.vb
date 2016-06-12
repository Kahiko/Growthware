Imports BLL.Base.SQLServer
Imports Common.Cache
Imports BLL.Base.ClientChoices
Imports DALModel.Base.BusinessUnits
Imports DALModel.Base.Modules
Imports System
Imports System.Web
#Region " Notes "
' The AppModulesUtility class aids in managing pages/modules
#End Region
Public Class AppModulesUtility
	Private Shared _AppLogger As AppLogger = AppLogger.GetInstance

	Public Shared ReadOnly AppModulesCachedCollectionName As String = "ModuleCollection"
	Public Shared ReadOnly AppModulesCachedDVModules As String = "dvModules"

	' GetModulesDataView returns modues in the form of a dataview
	Public Shared Function GetModulesDataView() As DataView
		_AppLogger.Debug("Start GetModulesDataView " & Now)
		Dim retDV As New DataView
		Dim moduleCollection As MModuleCollection = GetModulesCollection(moduleCollection)
		Dim Action As String
		Dim moduleProfileInfo As MModuleProfileInfo
		Dim oTable As New DataTable("MyTable")
		Dim x As Integer
		oTable.Columns.Add("MODULE_SEQ_ID", System.Type.GetType("System.Int32"))
		oTable.Columns.Add("Name", System.Type.GetType("System.String"))
		oTable.Columns.Add("Description", System.Type.GetType("System.String"))
		oTable.Columns.Add("Source", System.Type.GetType("System.String"))
		oTable.Columns.Add("EnableViewState", System.Type.GetType("System.Boolean"))
		oTable.Columns.Add("IS_NAV", System.Type.GetType("System.Boolean"))
		oTable.Columns.Add("NAV_TYPE_SEQ_ID", System.Type.GetType("System.Int32"))
		oTable.Columns.Add("Action", System.Type.GetType("System.String"))
		oTable.Columns.Add("InheritTransformations", System.Type.GetType("System.Boolean"))
		oTable.Columns.Add("ViewRoles", System.Type.GetType("System.String"))
		oTable.Columns.Add("AddRoles", System.Type.GetType("System.String"))
		oTable.Columns.Add("EditRoles", System.Type.GetType("System.String"))
		oTable.Columns.Add("DeleteRoles", System.Type.GetType("System.String"))
		oTable.Columns.Add("Transformations", System.Type.GetType("System.String"))
		Try
			For Each Action In moduleCollection.Keys
				Dim oRow As DataRow = oTable.NewRow()
				moduleProfileInfo = moduleCollection.GetModulesByAction(Action)
				oRow("MODULE_SEQ_ID") = moduleProfileInfo.MODULE_SEQ_ID
				oRow("Name") = moduleProfileInfo.Name
				oRow("Description") = moduleProfileInfo.Description
				oRow("Source") = moduleProfileInfo.Source
				oRow("EnableViewState") = moduleProfileInfo.EnableViewState
				oRow("IS_NAV") = moduleProfileInfo.IS_NAV
				oRow("NAV_TYPE_SEQ_ID") = moduleProfileInfo.NAV_TYPE_SEQ_ID
				oRow("Action") = moduleProfileInfo.Action
				oRow("InheritTransformations") = moduleProfileInfo.InheritTransformations
				oRow("ViewRoles") = moduleProfileInfo.ViewRoles
				oRow("AddRoles") = moduleProfileInfo.AddRoles
				oRow("EditRoles") = moduleProfileInfo.EditRoles
				oRow("DeleteRoles") = moduleProfileInfo.DeleteRoles
				oRow("Transformations") = moduleProfileInfo.Transformations
				oTable.Rows.Add(oRow)
			Next
		Catch ex As Exception
			Throw ex
		End Try
		retDV = oTable.DefaultView
		_AppLogger.Debug("End GetModulesDataView " & Now)
		Return retDV
	End Function	'GetModulesDataView

	'*********************************************************************
	' GetModuleInfoByAction Method -
	' Returns module profile information from the module profile collection
	' given the modules action.
	'*********************************************************************
	Public Shared Function GetModuleInfoByAction(ByVal Action As String) As MModuleProfileInfo
		'Action = Action.ToLower
		Dim retVal As MModuleProfileInfo
		Dim moduleCollection As MModuleCollection = GetModulesCollection(moduleCollection)
		retVal = moduleCollection.GetModulesByAction(Action.ToLower)
		If retVal Is Nothing Then
			retVal = moduleCollection.GetModulesByAction(Action)
		End If
		Return retVal
	End Function	'GetModuleInfoFromAction

	'*********************************************************************
	' GetModulesByID Method -
	' Returns module profile information from the module profile collection
	' given the modules ID.
	'*********************************************************************
	Public Shared Function GetModulesByID(ByVal Module_Seq_Id As Integer) As MModuleProfileInfo
		Dim moduleCollection As MModuleCollection = GetModulesCollection(moduleCollection)
		Return moduleCollection.GetModulesByID(Module_Seq_Id)
	End Function	'GetModuleInfoFromAction

	'*********************************************************************
	' GetModulesCollection - 
	' Attempts to retrieve the module collection from cache.
	' If that fails the collection is obtained through the BLL from
	' the data store and then placed into cache.
	' Since the application needs to have the roles information
	' by Business Unit the first attempt should be to retrieve
	' the role information for the selected Business Unit.
	' Should that fail then use the role information for
	' the 1 business_unit_seq_id or All business units
	'*********************************************************************
	Public Shared Function GetModulesCollection(ByRef yourModulesCollection As MModuleCollection) As MModuleCollection
		Dim myBusinessUnit As String = ClientChoicesUtility.GetSelectedBusinessUnit
		yourModulesCollection = CType(HttpContext.Current.Cache.Item(myBusinessUnit & AppModulesCachedCollectionName), MModuleCollection)
		If yourModulesCollection Is Nothing Then
			yourModulesCollection = BAppModules.GetAllEnabledModules(myBusinessUnit)
			Try
				CacheControler.AddToCacheDependency(myBusinessUnit & AppModulesCachedCollectionName, yourModulesCollection)
			Catch ex As Exception
				Dim myAppEx As New ApplicationException("Could not add to cache for Business Unit '" & myBusinessUnit & "'")
				Throw myAppEx
			End Try
		End If
		Return yourModulesCollection
	End Function	'GetModulesCollection

	'*********************************************************************
	' RemoveCachedModules Method
	' Removes both the modulecollection and as well as the dataview
	'*********************************************************************
	Public Shared Sub RemoveCachedModules()
		'Dim myBusinessUnitSeqID As String = ClientChoicesUtility.GetSelectedBusinessUnit
		'If myBusinessUnitSeqID = 1 Then
		'	Dim businessUnitID As Integer
		'	Dim businessUnitProfileInfoCollection As MBusinessUnitProfileInfoCollection = CType(HttpContext.Current.Cache.Item(BusinessUnitUtility.BusinessUnitCachedCollection), MBusinessUnitProfileInfoCollection)
		'	For Each businessUnitID In businessUnitProfileInfoCollection.Keys
		'		CacheControler.RemoveFromCache(businessUnitID & AppModulesCachedCollectionName)
		'	Next
		'Else
		'	CacheControler.RemoveFromCache(myBusinessUnitSeqID & AppModulesCachedCollectionName)
		'End If
		Dim businessUnitID As Integer
		Dim businessUnitProfileInfoCollection As MBusinessUnitProfileInfoCollection = CType(HttpContext.Current.Cache.Item(BusinessUnitUtility.BusinessUnitCachedCollection), MBusinessUnitProfileInfoCollection)
		For Each businessUnitID In businessUnitProfileInfoCollection.Keys
			CacheControler.RemoveFromCache(businessUnitID & AppModulesCachedCollectionName)
		Next
		CacheControler.RemoveFromCache(AppModulesCachedDVModules)
	End Sub	'RemoveCachedModules

	'*********************************************************************
	' ReBuildModuleCollection Method
	' ReBuilds the modulecollection
	'*********************************************************************
	Public Shared Sub ReBuildModuleCollection()
		RemoveCachedModules()
		Dim myModuleProfileInfo As New MModuleProfileInfo
		myModuleProfileInfo = GetModulesByID(1)
	End Sub	'ReBuildModuleCollection

	'*********************************************************************
	' GetCurrentModule Method
	' Returns the current module profile from the selected Business unit
	' cached module profile collection
	'*********************************************************************
	Public Shared Function GetCurrentModule() As MModuleProfileInfo
		Try
			Return GetModuleInfoByAction(System.Web.HttpContext.Current.Items("ModuleProfileInfo").Action)
		Catch ex As Exception
			Return Nothing
		End Try
    End Function 'GetCurrentModule
End Class
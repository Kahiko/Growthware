Imports BLL.Base.SQLServer
Imports Common.Cache
Imports DALModel.Base.Accounts
Imports DALModel.Base.BusinessUnits
#Region " Notes "
' The BusinessUnitUtility class aids in the management of Business Units.
#End Region
Public Class BusinessUnitUtility
    Public Shared ReadOnly BusinessUnitCachedDV As String = "dvBusinessUnit"
    Public Shared BusinessUnitCachedCollection As String = "BusinessUnitCollection"

    Public Shared Function BusinessUnitsRolesCacheName(ByVal BUSINESS_UNIT_SEQ_ID As Integer) As String
        Dim retVal As String = BUSINESS_UNIT_SEQ_ID & "BusinessUnitRoles"
        Return retVal
    End Function

	Public Shared Function BusinessUnitsGroupsCacheName(ByVal BUSINESS_UNIT_SEQ_ID As Integer) As String
		Dim retVal As String = BUSINESS_UNIT_SEQ_ID & "BusinessUnitGroupss"
		Return retVal
	End Function

	'*********************************************************************
	' GetAllRolesForBusinessUnit Method
	' Retrieves all the roles for a business unit and then caches the information
	' for latter use.
	'*********************************************************************
	Public Shared Function GetAllRolesForBusinessUnit(ByVal Business_Unit_Seq_ID As Integer) As DataSet
		Dim myBusinessUnitRoles As DataSet
		' attempt to retrieve the information from cache
		myBusinessUnitRoles = HttpContext.Current.Cache.Item(BusinessUnitsRolesCacheName(Business_Unit_Seq_ID))
		' if the information was not avalible in cache
		' then retieve the information from the DB and put it into
		' cache for subsequent use.
		If myBusinessUnitRoles Is Nothing Then
			myBusinessUnitRoles = BBusinessUnits.GetAllRolesForBusinessUnit(myBusinessUnitRoles, Business_Unit_Seq_ID)
			CacheControler.AddToCacheDependency(Business_Unit_Seq_ID & "BusinessUnitRoles", myBusinessUnitRoles)
		End If
		Return myBusinessUnitRoles
	End Function	' GetAllRolesForBusinessUnit

	Public Shared Function GetAllGroupsForBusinessUnit(ByVal Business_Unit_Seq_ID As Integer) As DataSet
		Dim myBusinessUnitGroups As DataSet
		' attempt to retrieve the information from cache
		myBusinessUnitGroups = HttpContext.Current.Cache.Item(BusinessUnitsGroupsCacheName(Business_Unit_Seq_ID))
		' if the information was not avalible in cache
		' then retieve the information from the DB and put it into
		' cache for subsequent use.
		If myBusinessUnitGroups Is Nothing Then
			myBusinessUnitGroups = BBusinessUnits.GetAllGroupsForBusinessUnit(myBusinessUnitGroups, Business_Unit_Seq_ID)
			CacheControler.AddToCacheDependency(BusinessUnitsGroupsCacheName(Business_Unit_Seq_ID), myBusinessUnitGroups)
		End If
		Return myBusinessUnitGroups
	End Function	' GetAllRolesForBusinessUnit

	'*********************************************************************
	' GetProfileByName Method
	' Attempts to retrieve a modules information given the desired action
	' from cache.  If not found in cache the the information is retrieved
	' from the DB and is placed into cache for furture use.
	'*********************************************************************
	Public Shared Function GetProfileByName(ByVal Name As String) As MBusinessUnitProfileInfo
		Dim businessUnitCollection As MBusinessUnitProfileInfoCollection
		GetBusinessProfileCollection(businessUnitCollection)
		Return businessUnitCollection.GetBusinessUnitByName(Name)
	End Function	'GetProfileByName

	'*********************************************************************
	' GetInfoByID Method
	' Attempts to retrieve a modules information given the desired action
	' from cache.  If not found in cache the the information is retrieved
	' from the DB and is placed into cache for furture use.
	'*********************************************************************
	Public Shared Function GetProfileByID(ByVal BUSINESS_UNIT_SEQ_ID As Integer) As MBusinessUnitProfileInfo
		Dim businessUnitCollection As MBusinessUnitProfileInfoCollection
		GetBusinessProfileCollection(businessUnitCollection)
		Return businessUnitCollection.GetBusinessUnitByID(BUSINESS_UNIT_SEQ_ID)
	End Function	'GetInfoByID

	Public Shared Sub GetBusinessProfileCollection(ByRef yourBusinessUnitProfileCollection As MBusinessUnitProfileInfoCollection)
		yourBusinessUnitProfileCollection = CType(HttpContext.Current.Cache.Item(BusinessUnitCachedCollection), MBusinessUnitProfileInfoCollection)
		If yourBusinessUnitProfileCollection Is Nothing Then
			yourBusinessUnitProfileCollection = BBusinessUnits.GetAllBusinessUnits()
			CacheControler.AddToCacheDependency(BusinessUnitCachedCollection, yourBusinessUnitProfileCollection)
		End If
	End Sub

	Public Shared Sub GetBusinessProfileInfoByName(ByRef yourBusinessUnitProfileInfo As MBusinessUnitProfileInfo, ByVal Name As String)
		Dim myBusinessUnitProfileInfoCollection As MBusinessUnitProfileInfoCollection
		GetBusinessProfileCollection(myBusinessUnitProfileInfoCollection)
		yourBusinessUnitProfileInfo = myBusinessUnitProfileInfoCollection.GetBusinessUnitByName(Name)
	End Sub

	Public Shared Sub GetBusinessProfileInfoById(ByRef yourBusinessUnitProfileInfo As MBusinessUnitProfileInfo, ByVal BUSINESS_UNIT_SEQ_ID As Integer)
		Dim myBusinessUnitProfileInfoCollection As MBusinessUnitProfileInfoCollection
		GetBusinessProfileCollection(myBusinessUnitProfileInfoCollection)
		yourBusinessUnitProfileInfo = myBusinessUnitProfileInfoCollection.GetBusinessUnitByID(BUSINESS_UNIT_SEQ_ID)
	End Sub

	'*********************************************************************
	' GetBusinessUnitsDataView Method 
	' retieves all of the information for the business unit and returns
	' it as a dataview
	'*********************************************************************
	Public Shared Function GetBusinessUnitsDataView() As DataView
		Dim retDV As New DataView
		'Dim dsBusinessUnit As New DataSet
		' Attempt to retrive the information from the caceh
		retDV = HttpContext.Current.Cache(BusinessUnitCachedDV)
		' if the information was not avalible in cache
		' then retieve the information from the DB, create a dataview,
		' and put it into cache for subsequent use.
		If retDV Is Nothing Then
			Dim BUSINESS_SEQ_ID As Integer
			Dim businessUnitProfileInfoCollection As MBusinessUnitProfileInfoCollection = CType(HttpContext.Current.Cache.Item(BusinessUnitCachedCollection), MBusinessUnitProfileInfoCollection)
			Dim businessUnitProfileInfo As MBusinessUnitProfileInfo
			Dim oTable As New DataTable("MyTable")
			oTable.Columns.Add("BUSINESS_UNIT_SEQ_ID", System.Type.GetType("System.String"))
			oTable.Columns.Add("Name", System.Type.GetType("System.String"))
			oTable.Columns.Add("Description", System.Type.GetType("System.String"))
			oTable.Columns.Add("STATUS_SEQ_ID", System.Type.GetType("System.String"))
			Try
				For Each BUSINESS_SEQ_ID In businessUnitProfileInfoCollection.Keys
					Dim oRow As DataRow = oTable.NewRow()
					businessUnitProfileInfo = businessUnitProfileInfoCollection.GetBusinessUnitByID(BUSINESS_SEQ_ID)
					oRow("BUSINESS_UNIT_SEQ_ID") = businessUnitProfileInfo.BUSINESS_UNIT_SEQ_ID
					oRow("Name") = businessUnitProfileInfo.Name
					oRow("Description") = businessUnitProfileInfo.Description
					oRow("STATUS_SEQ_ID") = businessUnitProfileInfo.STATUS_SEQ_ID
					oTable.Rows.Add(oRow)
				Next
			Catch ex As Exception
				Throw ex
			End Try
			retDV = oTable.DefaultView
			CacheControler.AddToCacheDependency(BusinessUnitCachedDV, retDV)
		End If
		Return retDV
	End Function	  'GetBusinessUnitsDataView

	'*********************************************************************
	'
	' UpdateBusinessUnitProfileInfo Method saves state profile info information
	' to the database then removes the state collection from the cache.
	' The next request for the state profile information will cause
	' the business unit profile information collection to be rebuilt.
	'
	'*********************************************************************
	Public Shared Function UpdateBusinessUnitProfileInfo(ByVal businessUnitProfileInfo As MBusinessUnitProfileInfo) As Boolean
		Dim retVal As Boolean = False
		retVal = BBusinessUnits.UpdateBusinessUnitsProfileInfo(businessUnitProfileInfo)
		BusinessUnitUtility.RemoveCachedBusinessUnitCollection()
		Return retVal
	End Function	'UpdateBusinessUnitProfileInfo

	'*********************************************************************
	'
	' UpdateBusinessUnitProfileInfo Method saves state profile info information
	' to the database then removes the state collection from the cache.
	' The next request for the state profile information will cause
	' the business unit profile information collection to be rebuilt.
	'
	'*********************************************************************
	Public Shared Function AddBusinessUnitProfileInfo(ByVal businessUnitProfileInfo As MBusinessUnitProfileInfo) As Boolean
		Dim retVal As Boolean = False
		retVal = BBusinessUnits.AddBusinessUnitsProfileInfo(businessUnitProfileInfo)
		BusinessUnitUtility.RemoveCachedBusinessUnitCollection()
		RemoveCachedBusinessUnitsDV()
		Return retVal
	End Function	'AddBusinessUnitProfileInfo

	Public Shared Sub GetValidBusinessUnits(ByRef yourDataSet As DataSet, ByVal ACCOUNT_SEQ_ID As Integer, ByVal isSysAdmin As Integer)
		BBusinessUnits.GetValidBusinessUnits(yourDataSet, ACCOUNT_SEQ_ID, isSysAdmin)
	End Sub

	Public Shared Sub RemoveRoleCache(ByVal BUSINESS_UNIT_SEQ_ID As Integer)
		Dim AccountType As Integer
		Dim AccountTypes As Integer() = System.Enum.GetValues(GetType(MAccountTypes.value))
		If BUSINESS_UNIT_SEQ_ID = 1 Then
			' todo add code to remove all business units cached roles!
		End If
		For Each AccountType In AccountTypes
			CacheControler.RemoveFromCache(BusinessUnitsRolesCacheName(BUSINESS_UNIT_SEQ_ID))
		Next
	End Sub

	Public Shared Sub RemoveCachedBusinessUnitsDV()
		CacheControler.RemoveFromCache(BusinessUnitCachedDV)
	End Sub

	Public Shared Sub RemoveCachedBusinessUnitCollection()
		CacheControler.RemoveFromCache(BusinessUnitCachedCollection)
		RemoveCachedBusinessUnitsDV()
	End Sub
End Class
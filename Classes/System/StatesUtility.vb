Imports BLL.Base.SQLServer
Imports Common.Cache
Imports DALModel.Base.States
#Region " Notes "
' The StatesUtility class aids in the management of states.
#End Region
Public Class StatesUtility
	Public Shared ReadOnly StatesCachedDV As String = "dvStates"
	Public Shared StatesCachedCollection As String = "StateCollection"

	'*********************************************************************
	' GetInfoByState Method
	' Attempts to retrieve a modules information given the desired action
	' from cache.  If not found in cache the the information is retrieved
	' from the DB and is placed into cache for furture use.
	'*********************************************************************
	Public Shared Function GetInfoByState(ByVal State As String) As MStateProfileInfo
		Dim stateCollection As MStateProfileInfoCollection = GetStateProfileCollection(stateCollection)
		Return stateCollection.GetInformationByState(State)
	End Function	'GetModuleInfoFromAction

	Public Shared Function GetStateProfileCollection(ByVal yourStateProfileCollection As MStateProfileInfoCollection) As MStateProfileInfoCollection
		yourStateProfileCollection = CType(HttpContext.Current.Cache.Item(StatesCachedCollection), MStateProfileInfoCollection)
		If yourStateProfileCollection Is Nothing Then
			yourStateProfileCollection = BStates.GetAllStates()
			CacheControler.AddToCacheDependency(StatesCachedCollection, yourStateProfileCollection)
		End If
		Return yourStateProfileCollection
	End Function

	'*********************************************************************
	' GetStatesDataView Method 
	' retieves all of the information for the states and returns
	' it as a dataview
	'*********************************************************************
	Public Shared Function GetStatesDataView() As DataView
		Dim retDV As New DataView
		Dim dsStates As New DataSet
		' Attempt to retrive the information from the caceh
		retDV = HttpContext.Current.Cache(StatesCachedDV)
		' if the information was not avalible in cache
		' then retieve the information from the DB, create a dataview,
		' and put it into cache for subsequent use.
		If retDV Is Nothing Then
			Dim State As String
			Dim stateCollection As MStateProfileInfoCollection = CType(HttpContext.Current.Cache.Item(StatesCachedCollection), MStateProfileInfoCollection)
			Dim stateProfileInfo As MStateProfileInfo
			Dim oTable As New DataTable("MyTable")
			oTable.Columns.Add("STATE", System.Type.GetType("System.String"))
			oTable.Columns.Add("LongName", System.Type.GetType("System.String"))
			oTable.Columns.Add("Status", System.Type.GetType("System.String"))
			If stateCollection Is Nothing Then
				stateCollection = StatesUtility.GetStateProfileCollection(stateCollection)
			End If
			Try
				For Each State In stateCollection.Keys
					Dim oRow As DataRow = oTable.NewRow()
					stateProfileInfo = stateCollection.GetInformationByState(State)
					oRow("STATE") = stateProfileInfo.State
					oRow("LongName") = stateProfileInfo.LongName
					oRow("Status") = stateProfileInfo.STATUS_SEQ_ID
					oTable.Rows.Add(oRow)
				Next
			Catch ex As Exception
				Throw ex
			End Try
			retDV = oTable.DefaultView
			CacheControler.AddToCacheDependency(StatesCachedDV, retDV)
		End If
		Return retDV
	End Function	  'GetStatesDataView

	'*********************************************************************
	'
	' UpdateStateProfileInfo Method saves state profile info information
	' to the database then removes the state collection from the cache.
	' The next request for the state profile information will cause
	' the state profile information collection to be rebuilt.
	'
	'*********************************************************************
	Public Shared Function UpdateStateProfileInfo(ByVal stateProfileInfo As MStateProfileInfo) As Boolean
		Dim retVal As Boolean = False
		retVal = BStates.UpdateStateProfileInfo(stateProfileInfo)
		StatesUtility.RemoveCachedStateCollection()
		Return retVal
	End Function

	Public Shared Sub RemoveCachedStatesDV()
		CacheControler.RemoveFromCache(StatesCachedDV)
	End Sub

	Public Shared Sub RemoveCachedStateCollection()
		CacheControler.RemoveFromCache(StatesCachedCollection)
	End Sub
End Class
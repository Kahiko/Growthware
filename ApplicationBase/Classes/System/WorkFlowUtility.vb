Imports ApplicationBase.BusinessLogic
Imports ApplicationBase.Common.Cache
Imports ApplicationBase.Model.WorkFlows
Imports System.Data

Public Class WorkFlowUtility
	Public Shared ReadOnly WorkFlowCachedDV As String = "dvWorkFlow"
	Public Shared ReadOnly WorkFlowCachedCollectionName As String = "WorkFlowCollection"
	Public Shared SessionWFPName As String = "WFP"

	'*********************************************************************
	' GetWorkFlowDataView Method 
	' retieves all of the information for the workflow and returns
	' it as a dataview
	'*********************************************************************
	Public Shared Function GetWorkFlowDataView() As DataView
		Dim retDV As New DataView
		'Dim dsBusinessUnit As New DataSet
		' Attempt to retrive the information from the caceh
		retDV = HttpContext.Current.Cache(WorkFlowCachedDV)
		' if the information was not avalible in cache
		' then retieve the information from the DB, create a dataview,
		' and put it into cache for subsequent use.
		If retDV Is Nothing Then
			Dim myDataSet As New DataSet
            BWorkFlows.GetWorkFlowsFromDB(myDataSet)
			retDV = myDataSet.Tables(0).DefaultView
			CacheControler.AddToCacheDependency(WorkFlowCachedDV, retDV)
		End If
		Return retDV
	End Function	  'GetWorkFlowDataView

	'*********************************************************************
	' GetWorkFlowDataView Method 
	' retieves all of the information for the work flow and returns
	' it as a dataview
	'*********************************************************************
	Public Shared Function GetWorkFlowDataView(ByVal WORK_FLOW_NAME As String) As DataView
		Dim retDV As New DataView
		Dim myWorkFlowName As String = WORK_FLOW_NAME + WorkFlowCachedDV
		'Dim dsBusinessUnit As New DataSet
		' Attempt to retrive the information from the caceh
		retDV = HttpContext.Current.Cache(myWorkFlowName)
		' if the information was not avalible in cache
		' then retieve the information from the DB, create a dataview,
		' and put it into cache for subsequent use.
		If retDV Is Nothing Then
			Dim myDataSet As New DataSet
            BWorkFlows.GetWorkFlowsFromDB(WORK_FLOW_NAME, myDataSet)
			retDV = myDataSet.Tables(0).DefaultView
			CacheControler.AddToCacheDependency(myWorkFlowName, retDV)
		End If
		Return retDV
	End Function	  'GetWorkFlowDataView

	'*********************************************************************
	' GetWorkFlowCollection - 
	' Attempts to retrieve the work flwo collection from cache.
	' If that fails the collection is obtained through the BLL from
	' the data store and then placed into cache.
	' Since the application needs to have the roles information
	' by Business Unit the first attempt should be to retrieve
	' the role information for the selected Business Unit.
	' Should that fail then use the role information for
	' the 1 business_unit_seq_id or All business units
	'*********************************************************************
	Public Shared Function GetWorkFlowCollection(ByVal WorkFlowName As String, ByRef yourWorkFlowCollection As MWorkFlowProfileInfoCollection) As MWorkFlowProfileInfoCollection
		yourWorkFlowCollection = CType(HttpContext.Current.Cache.Item(WorkFlowName & WorkFlowCachedCollectionName), MWorkFlowProfileInfoCollection)
		If yourWorkFlowCollection Is Nothing Then
			yourWorkFlowCollection = BWorkFlows.GetCollectionFromDB(WorkFlowName)
			Try
				CacheControler.AddToCacheDependency(WorkFlowName & WorkFlowCachedCollectionName, yourWorkFlowCollection)
			Catch ex As Exception
				Dim myAppEx As New ApplicationException("Could not add to cache for Work Flow '" & WorkFlowName & "'")
				Throw myAppEx
			End Try
		End If
		Return yourWorkFlowCollection
	End Function	'GetWorkFlowCollection

	'*********************************************************************
	' GetWorkFlowByOrder Method -
	' Returns work flow profile information from the work flow profile collection
	' given the work flows name and order.
	'*********************************************************************
	Public Shared Function GetWorkFlowByOrder(ByVal WorkFlowName As String, ByVal Order As Integer) As MWorkFlowProfileInfo
		Dim workFlowCollection As New MWorkFlowProfileInfoCollection
		GetWorkFlowCollection(WorkFlowName, workFlowCollection)
		Return workFlowCollection.GetWorkFlowByOrder(Order)
	End Function	'GetWorkFlowByOrder


	'*********************************************************************
	' RemoveCachedWorkFlowProfile Method
	' Removes both the WorkFlowCollection and as well as the dataview
	'*********************************************************************
	Public Shared Sub RemoveCachedWorkFlowProfile(ByVal WorkFlowName As String)
		CacheControler.RemoveFromCache(WorkFlowName & WorkFlowCachedCollectionName)
		CacheControler.RemoveFromCache(WorkFlowName & WorkFlowCachedDV)
	End Sub	'RemoveCachedWorkFlowProfile

	Public Shared Sub RemoveWorkFlowDataView(ByVal WorkFlowName As String)
		CacheControler.RemoveFromCache(WorkFlowCachedDV)
	End Sub

    '*********************************************************************
    ' GetCurrentWFP Function
    ' Returns the current work flow profile.
    '*********************************************************************
    Public Shared Function GetCurrentWFP() As MWorkFlowProfileInfo
        Dim myWorkFlowProfileInfo As MWorkFlowProfileInfo
        myWorkFlowProfileInfo = HttpContext.Current.Session(WorkFlowUtility.SessionWFPName)
        Return myWorkFlowProfileInfo
    End Function

    '*********************************************************************
    ' isFirst Function
    ' Returns true if the current work flow profile order is 1.
    '*********************************************************************
    Public Shared Function isFirst() As Boolean
        Dim retVal As Boolean = False
        Dim myWorkFlowProfile As MWorkFlowProfileInfo
        myWorkFlowProfile = GetCurrentWFP()
        If Not myWorkFlowProfile Is Nothing Then
            If myWorkFlowProfile.Order = 1 Then
                retVal = True
            End If
        End If
        Return retVal
	End Function

	Public Shared Sub SetWorkFlow(ByVal workFlowProfile As MWorkFlowProfileInfo)
		HttpContext.Current.Session.Add(WorkFlowUtility.SessionWFPName, workFlowProfile)
	End Sub
End Class
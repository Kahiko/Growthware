Imports System
Imports System.Collections
Imports System.Collections.Specialized

Namespace WorkFlows
	'*********************************************************************
	' MWorkFlowProfileInfoCollection Class
	' Is a collection of WorkFlowProfileInfo objects representing 
	' work flow information.
	'*********************************************************************
	<Serializable(), CLSCompliant(True)> _
	Public Class MWorkFlowProfileInfoCollection
		Inherits Hashtable

		Private workFlowByOrder As New Hashtable
		Private workFlowByID As New Hashtable

		'*********************************************************************
		' Add Method
		' Adds a new WorkFlowProfileInfo object to the collection.
		'*********************************************************************
		Public Overrides Sub Add(ByVal key As [Object], ByVal value As [Object])
			Dim _workFlowProfileInfo As MWorkFlowProfileInfo = CType(value, MWorkFlowProfileInfo)
			' Don't allow duplicate values
			If Contains(key) Then Return
			workFlowByOrder.Add(_workFlowProfileInfo.Order, value)
			workFlowByID.Add(_workFlowProfileInfo.WORK_FLOW_SEQ_ID, value)
			MyBase.Add(key, value)
		End Sub	'Add

		'*********************************************************************
		' this Indexer
		' Adds a WorkFlowProfileInfo object. 
		'*********************************************************************
		Default Public Overloads Overrides Property Item(ByVal key As [Object]) As [Object]
			Set(ByVal Value As [Object])
				' Don't allow duplicate values
				If Contains(key) Then Return
				workFlowByOrder.Add(CType(Value, MWorkFlowProfileInfo).Order, Value)
				'workFlowByID.Add(CType(Value, MWorkFlowProfileInfo).WORK_FLOW_SEQ_ID, Value)
				MyBase.Item(key) = Value
			End Set
			Get
				Return MyBase.Item(key)
			End Get
		End Property

		'*********************************************************************
		' GetWorkFlowByOrder Method
		' Returns WorkFlowProfileInfo object given it's order. 
		'*********************************************************************
		Public Function GetWorkFlowByOrder(ByVal Order As Integer) As MWorkFlowProfileInfo
			Dim myWorkFlowProfileInfo As New MWorkFlowProfileInfo
			myWorkFlowProfileInfo = workFlowByOrder(Order)
			Return CType(workFlowByOrder(Order), MWorkFlowProfileInfo)
		End Function 'GetWorkFlowByOrder

		'*********************************************************************
		' GetWorkFlowByID Method
		' Returns WorkFlowProfileInfo object given it's seq id. 
		'*********************************************************************
		Public Function GetWorkFlowByID(ByVal Work_Flow_Seq_Id As Integer) As MWorkFlowProfileInfo
			Dim myWorkFlowProfileInfo As New MWorkFlowProfileInfo
			myWorkFlowProfileInfo = workFlowByID(Work_Flow_Seq_Id)
			Return CType(workFlowByID(Work_Flow_Seq_Id), MWorkFlowProfileInfo)
		End Function 'GetWorkFlowByOrder


		'*********************************************************************
		' WorkFlowProfileInfoCollection Constructor
		' Initializes WorkFlowProfileInfoCollection as a case-insensitive Hashtable. 
		'*********************************************************************
		Public Sub New()
			'MyBase.New(New StringComparison, New CaseInsensitiveComparer)
			MyBase.New(New StringComparison())
		End Sub	'New 
	End Class
End Namespace
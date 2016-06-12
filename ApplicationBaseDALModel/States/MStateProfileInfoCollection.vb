Imports System
Imports System.Collections
Imports System.Collections.Specialized

Namespace States
	'*********************************************************************
	'
	' StateProfileInfoCollection Class
	'
	' Represents a collection of StateProfileInfo objects representing 
	' Directory information.
	'
	'*********************************************************************

	<CLSCompliant(True)> _
	  Public Class MStateProfileInfoCollection
		Inherits Hashtable

		Private orderedStates As New ArrayList
		Private informationByState As New Hashtable

		'*********************************************************************
		'
		' Add Method
		'
		' Adds a new StateProfileInfo object to the collection.
		'
		'*********************************************************************
		Public Overrides Sub Add(ByVal key As [Object], ByVal value As [Object])
			Dim _StateProfileInfo As MStateProfileInfo = CType(value, MStateProfileInfo)

			' Don't allow duplicate values
			If Contains(key) Then
				Return
			End If

			orderedStates.Add(_StateProfileInfo)
			informationByState.Add(_StateProfileInfo.State, value)
			MyBase.Add(key, value)
		End Sub	   'Add

		'*********************************************************************
		'
		' this Indexer
		'
		' Adds a StateProfileInfo object. 
		'
		'*********************************************************************
		Default Public Overloads Overrides Property Item(ByVal key As [Object]) As [Object]
			Set(ByVal Value As [Object])
				' Don't allow duplicate values
				If Contains(key) Then
					Return
				End If
				informationByState.Add(CType(Value, MStateProfileInfo).State, Value)
				MyBase.Item(key) = Value
			End Set
			Get
				Return MyBase.Item(key)
			End Get
		End Property

		'*********************************************************************
		'
		' this Indexer
		'
		' Returns a StateProfileInfo object based on path. 
		'
		'*********************************************************************
		Default Public Overloads ReadOnly Property Item(ByVal State As String) As MStateProfileInfo
			Get
				Return CType(MyBase.Item(State), MStateProfileInfo)
			End Get
		End Property

		'*********************************************************************
		'
		' GetOrderedSections Method
		'
		' Returns an ordered list of sections. 
		'
		'*********************************************************************
		Public Function GetOrderedStates() As ArrayList
			Return orderedStates
		End Function	'GetOrderedDirectories

		'*********************************************************************
		'
		' GetDirectoryByState Method
		'
		' Returns StateProfileInfo object from a State. 
		'
		'*********************************************************************
		Public Function GetInformationByState(ByVal State As String) As MStateProfileInfo
			Return CType(informationByState(State), MStateProfileInfo)
		End Function	'GetDirectoryByState

		'*********************************************************************
		'
		' StateProfileInfoCollection Constructor
		'
		' Initializes StateProfileInfoCollection as a case-insensitive Hashtable. 
		'
		'*********************************************************************
		Public Sub New()
			'MyBase.New(New StringComparison, New CaseInsensitiveComparer)
			MyBase.New(New StringComparison())
		End Sub	   'New 
	End Class ' StateProfileInfoCollection
End Namespace
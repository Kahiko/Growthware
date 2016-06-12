Imports System
Imports System.Collections
Imports System.Collections.Specialized
Namespace Base.BusinessUnits
    '*********************************************************************
    ' MBusinessUnitProfileInfoCollection Class
    ' Is a collection of BusinessUnitProfileInfo objects representing 
    ' Business unit information.
    '*********************************************************************

    Public Class MBusinessUnitProfileInfoCollection
        Inherits Hashtable

        Private businessUnitByID As New Hashtable
        Private businessUnitByName As New Hashtable

        '*********************************************************************
        ' Add Method
        ' Adds a new BusinessUnitProfileInfo object to the collection.
        '*********************************************************************
        Public Overrides Sub Add(ByVal key As [Object], ByVal value As [Object])
            Dim _businessUnitInfo As MBusinessUnitProfileInfo = CType(value, MBusinessUnitProfileInfo)
            ' Don't allow duplicate values
            If Contains(key) Then Return
            businessUnitByID.Add(_businessUnitInfo.BUSINESS_UNIT_SEQ_ID, value)
            businessUnitByName.Add(_businessUnitInfo.Name, value)
            MyBase.Add(key, value)
        End Sub 'Add

        '*********************************************************************
        ' this Indexer
        ' Adds a BusinessUnitProfileInfo object. 
        '*********************************************************************
        Default Public Overloads Overrides Property Item(ByVal key As [Object]) As [Object]
            Set(ByVal Value As [Object])
                ' Don't allow duplicate values
                If Contains(key) Then Return
                businessUnitByID.Add(CType(Value, MBusinessUnitProfileInfo).BUSINESS_UNIT_SEQ_ID, Value)
                businessUnitByName.Add(CType(Value, MBusinessUnitProfileInfo).Name, Value)
                MyBase.Item(key) = Value
            End Set
            Get
                Return MyBase.Item(key)
            End Get
        End Property

        '*********************************************************************
        ' GetBusinessUnitByID Method
        ' Returns BusinessUnitProfileInfo object given it's sequence id. 
        '*********************************************************************
        Public Function GetBusinessUnitByID(ByVal BUSINESS_UNIT_SEQ_ID As Integer) As MBusinessUnitProfileInfo
            Dim myBusinessUnitProfileInfo As New MBusinessUnitProfileInfo
            myBusinessUnitProfileInfo = businessUnitByID(BUSINESS_UNIT_SEQ_ID)
            Return CType(businessUnitByID(BUSINESS_UNIT_SEQ_ID), MBusinessUnitProfileInfo)
        End Function 'GetBusinessUnitByID

        '*********************************************************************
        ' GetBusinessUnitByName Method
        ' Returns BusinessUnitProfileInfo object given it's name. 
        '*********************************************************************
        Public Function GetBusinessUnitByName(ByVal Name As String) As MBusinessUnitProfileInfo
			Dim myBusinessUnitProfileInfo As New MBusinessUnitProfileInfo
			myBusinessUnitProfileInfo = businessUnitByName(Name)
			Return myBusinessUnitProfileInfo
        End Function 'GetBusinessUnitByName

        '*********************************************************************
        ' BusinessUnitProfileInfoCollection Constructor
        ' Initializes BusinessUnitProfileInfoCollection as a case-insensitive Hashtable. 
        '*********************************************************************
        Public Sub New()
            MyBase.New(New CaseInsensitiveHashCodeProvider, New CaseInsensitiveComparer)
        End Sub 'New 
    End Class ' MBusinessUnitProfileInfoCollection
End Namespace
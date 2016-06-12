Imports System
Imports System.Collections
Imports System.Collections.Specialized
Namespace Base.Directories
    '*********************************************************************
    '
    ' DirectoryInfoCollection Class
    ' Represents a collection of DirectoryInfo objects representing 
    ' Directory information.
    '
    '*********************************************************************
    Public Class MDirectoryProfileInfoCollection
        Inherits Hashtable

        Private orderedDirectories As New ArrayList
        Private directoryByBusinessUnitID As New Hashtable

        '*********************************************************************
        '
        ' Add Method
        ' Adds a new DirectoryInfo object to the collection.
        '
        '*********************************************************************
        Public Overrides Sub Add(ByVal key As [Object], ByVal value As [Object])
            Dim _directoryInfo As MDirectoryProfileInformation = CType(value, MDirectoryProfileInformation)

            ' Don't allow duplicate values
            If Contains(key) Then
                Return
            End If

            orderedDirectories.Add(_directoryInfo)
            directoryByBusinessUnitID.Add(_directoryInfo.BUSINESS_UNIT_SEQ_ID, value)
            MyBase.Add(key, value)
        End Sub 'Add

        '*********************************************************************
        '
        ' this Indexer
        ' Adds a DirectoryInfo object. 
        '
        '*********************************************************************
        Default Public Overloads Overrides Property Item(ByVal key As [Object]) As [Object]
            Set(ByVal Value As [Object])
                ' Don't allow duplicate values
                If Contains(key) Then
                    Return
                End If
                directoryByBusinessUnitID.Add(CType(Value, MDirectoryProfileInformation).BUSINESS_UNIT_SEQ_ID, Value)
                MyBase.Item(key) = Value
            End Set
            Get
                Return MyBase.Item(key)
            End Get
        End Property

        '*********************************************************************
        '
        ' this Indexer
        ' Returns a DirectoryInfo object based on path. 
        '
        '*********************************************************************
        Default Public Overloads ReadOnly Property Item(ByVal State As String) As MDirectoryProfileInformation
            Get
                Return CType(MyBase.Item(State), MDirectoryProfileInformation)
            End Get
        End Property

        '*********************************************************************
        '
        ' GetOrderedSections Method
        ' Returns an ordered list of sections. 
        '
        '*********************************************************************
        Public Function GetOrderedDirectories() As ArrayList
            Return orderedDirectories
        End Function 'GetOrderedDirectories

        '*********************************************************************
        '
        ' GetDirectoryByState Method
        ' Returns DirectoryInfo object from a State. 
        '
        '*********************************************************************
        Public Function GetDirectoryByBusinessUnitID(ByVal BUSINESS_UNIT_SEQ_ID As Integer) As MDirectoryProfileInformation
            Return CType(directoryByBusinessUnitID(BUSINESS_UNIT_SEQ_ID), MDirectoryProfileInformation)
        End Function 'GetDirectoryByState

        '*********************************************************************
        '
        ' DirectoryInfoCollection Constructor
        ' Initializes DirectoryInfoCollection as a case-insensitive Hashtable. 
        '
        '*********************************************************************
        Public Sub New()
            MyBase.New(New CaseInsensitiveHashCodeProvider, New CaseInsensitiveComparer)
        End Sub 'New 
    End Class ' DirectoryInfoCollection
End Namespace
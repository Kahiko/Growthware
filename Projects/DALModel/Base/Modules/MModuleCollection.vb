Imports System
Imports System.Collections
Imports System.Collections.Specialized

Namespace Base.Modules
    '*********************************************************************
    '
    ' ModuleCollection Class
    '
    ' Represents a collection of ModuleInfo objects representing 
    ' application modules.
    '
    '*********************************************************************
    Public Class MModuleCollection
        Inherits Hashtable

        Private modulesByID As New Hashtable
        Private modulesByAction As New Hashtable

        '*********************************************************************
        '
        ' Add Method
        ' Adds a new ModuleInfo object to the collection.
        '
        '*********************************************************************
        Public Overrides Sub Add(ByVal key As [Object], ByVal value As [Object])
            Dim _moduleInfo As MModuleProfileInfo = CType(value, MModuleProfileInfo)
            ' Don't allow duplicate values
            If Contains(key) Then
                Return
            End If
            modulesByID.Add(_moduleInfo.MODULE_SEQ_ID, value)
            modulesByAction.Add(_moduleInfo.Action, value)
            MyBase.Add(key, value)
        End Sub 'Add

        '*********************************************************************
        '
        ' this Indexer
        ' Adds a ModuleInfo object. 
        '
        '*********************************************************************
        Default Public Overloads Overrides Property Item(ByVal key As [Object]) As [Object]
            Set(ByVal Value As [Object])
                ' Don't allow duplicate values
                If Contains(key) Then
                    Return
                End If
                modulesByID.Add(CType(Value, MModuleProfileInfo).MODULE_SEQ_ID, Value)
                modulesByAction.Add(CType(Value, MModuleProfileInfo).Action, Value)
                MyBase.Item(key) = Value
            End Set
            Get
                Return MyBase.Item(key)
            End Get
        End Property

        '*********************************************************************
        '
        ' GetSectionByID Method
        ' Returns ModuleInfo object from a section ID. 
        '
        '*********************************************************************
        Public Function GetModulesByID(ByVal MODULE_SEQ_ID As Integer) As MModuleProfileInfo
            Return CType(modulesByID(MODULE_SEQ_ID), MModuleProfileInfo)
        End Function 'GetModulesByID

        '*********************************************************************
        '
        ' GetModulesByAction Method
        ' Returns ModuleInfo object given it's action. 
        '
        '*********************************************************************
        Public Function GetModulesByAction(ByVal Action As String) As MModuleProfileInfo
            Return CType(modulesByAction(Action), MModuleProfileInfo)
        End Function 'GetModulesByAction

        '*********************************************************************
        '
        ' ModuleCollection Constructor
        ' Initializes ModuleCollection as a case-insensitive Hashtable. 
        '
        '*********************************************************************
        Public Sub New()
            MyBase.New(New CaseInsensitiveHashCodeProvider, New CaseInsensitiveComparer)
        End Sub 'New 
    End Class ' ModuleCollection
End Namespace
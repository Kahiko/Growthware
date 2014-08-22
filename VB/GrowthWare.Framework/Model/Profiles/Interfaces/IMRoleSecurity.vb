Imports System.Collections.ObjectModel

Namespace Model.Profiles.Interfaces
    <CLSCompliant(True)> _
    Public Interface IMRoleSecurity
        ReadOnly Property AssignedRoles() As Collection(Of String)
        ReadOnly Property DerivedRoles() As Collection(Of String)

    End Interface
End Namespace

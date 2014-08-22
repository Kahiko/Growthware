Imports System.Collections.ObjectModel

Namespace Model.Profiles.Interfaces
    <CLSCompliant(True)> _
    Public Interface IMGroupRoleSecurity
        ReadOnly Property AssignedRoles() As Collection(Of String)
        ReadOnly Property DerivedRoles() As Collection(Of String)
        ReadOnly Property Groups As Collection(Of String)

    End Interface
End Namespace

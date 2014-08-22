Imports System.Collections.ObjectModel

Namespace Model.Profiles.Interfaces
    <CLSCompliant(True)> _
    Public Interface IMGroupRoleSecurity
        ReadOnly Property AddGroups As Collection(Of String)

        ReadOnly Property EditGroups As Collection(Of String)

        ReadOnly Property DeleteGroups As Collection(Of String)

        ReadOnly Property ViewGroups As Collection(Of String)

        ReadOnly Property AssignedAddRoles As Collection(Of String)

        ReadOnly Property AssignedEditRoles As Collection(Of String)

        ReadOnly Property AssignedDeleteRoles As Collection(Of String)

        ReadOnly Property AssignedViewRoles As Collection(Of String)

        ReadOnly Property DerivedAddRoles As Collection(Of String)

        ReadOnly Property DerivedEditRoles As Collection(Of String)

        ReadOnly Property DerivedDeleteRoles As Collection(Of String)

        ReadOnly Property DerivedViewRoles As Collection(Of String)
    End Interface
End Namespace

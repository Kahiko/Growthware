Imports System.Collections.ObjectModel

Namespace Model.Profiles.Interfaces
    <CLSCompliant(True)> _
    Public Interface IMGroupRolePermissionSecurity

        ''' <summary>
        ''' Add groups that are directly assigned
        ''' </summary>
        ReadOnly Property AddGroups As Collection(Of String)

        ''' <summary>
        ''' Edit groups that are directly assigned
        ''' </summary>
        ReadOnly Property EditGroups As Collection(Of String)

        ''' <summary>
        ''' Delete groups that are directly assigned
        ''' </summary>
        ReadOnly Property DeleteGroups As Collection(Of String)

        ''' <summary>
        ''' View groups that are directly assigned
        ''' </summary>
        ReadOnly Property ViewGroups As Collection(Of String)

        ''' <summary>
        ''' Add roles that are directly assigned
        ''' </summary>
        ReadOnly Property AssignedAddRoles As Collection(Of String)

        ''' <summary>
        ''' Edit roles that are directly assigned
        ''' </summary>
        ReadOnly Property AssignedEditRoles As Collection(Of String)

        ''' <summary>
        ''' Delete roles that are directly assigned
        ''' </summary>
        ReadOnly Property AssignedDeleteRoles As Collection(Of String)

        ''' <summary>
        ''' View roles that are directly assigned
        ''' </summary>
        ReadOnly Property AssignedViewRoles As Collection(Of String)

        ''' <summary>
        ''' Add roles that are derived from roles assigned to groups
        ''' </summary>
        ReadOnly Property DerivedAddRoles As Collection(Of String)

        ''' <summary>
        ''' Edit roles that are derived from roles assigned to groups
        ''' </summary>
        ReadOnly Property DerivedEditRoles As Collection(Of String)

        ''' <summary>
        ''' Delete roles that are derived from roles assigned to groups
        ''' </summary>
        ReadOnly Property DerivedDeleteRoles As Collection(Of String)

        ''' <summary>
        ''' View roles that are derived from roles assigned to groups
        ''' </summary>
        ReadOnly Property DerivedViewRoles As Collection(Of String)
    End Interface
End Namespace

Imports System.Collections.ObjectModel

Namespace Model.Profiles.Interfaces
    ''' <summary>
    ''' Interface for anything needed group and role security
    ''' </summary>
    <CLSCompliant(True)> _
    Public Interface IMGroupRoleSecurity
        ''' <summary>
        ''' Roles that are directly assigned
        ''' </summary>
        ReadOnly Property AssignedRoles() As Collection(Of String)

        ''' <summary>
        ''' Roles that are derived from groups
        ''' </summary>
        ReadOnly Property DerivedRoles() As Collection(Of String)

        ''' <summary>
        ''' Groups that are directly assigned
        ''' </summary>
        ReadOnly Property Groups As Collection(Of String)

    End Interface
End Namespace

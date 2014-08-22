Imports GrowthWare.Framework.Model.Enumerations
Imports GrowthWare.Framework.Model.Profiles.Interfaces
Imports System.Collections.ObjectModel

Namespace Model.Profiles.Base
    Public MustInherit Class MGroupRolePermissionSecurity
        Inherits MProfile
        Implements IMGroupRolePermissionSecurity

        Private m_DerivedAddRoles As Collection(Of String) = New Collection(Of String)
        Private m_DerivedDeleteRoles As Collection(Of String) = New Collection(Of String)
        Private m_DerivedEditRoles As Collection(Of String) = New Collection(Of String)
        Private m_DerivedViewRoles As Collection(Of String) = New Collection(Of String)

        Private m_AssignedAddRoles As Collection(Of String) = New Collection(Of String)
        Private m_AssignedDeleteRoles As Collection(Of String) = New Collection(Of String)
        Private m_AssignedEditRoles As Collection(Of String) = New Collection(Of String)
        Private m_AssignedViewRoles As Collection(Of String) = New Collection(Of String)

        Private m_AddGroups As Collection(Of String) = New Collection(Of String)
        Private m_DeleteGroups As Collection(Of String) = New Collection(Of String)
        Private m_EditGroups As Collection(Of String) = New Collection(Of String)
        Private m_ViewGroups As Collection(Of String) = New Collection(Of String)
        Private m_PermissionColumn As String = "PERMISSIONS_SEQ_ID"
        Private m_RoleColumn As String = "Role"
        Private m_GroupColumn As String = "Group"

        ''' <summary>
        ''' Initialize orverloads and calles mybase.init to will populate the Add, Delete, Edit, and View role properties.
        ''' </summary>
        ''' <param name="detailDatarow">A data row that contains base information</param>
        ''' <param name="derivedRoles">An array of data rows that must contain two columns ("PERMISSIONS_SEQ_ID","ROLE")</param>
        ''' <param name="assignedRoles">An array of data rows that must contain two columns ("PERMISSIONS_SEQ_ID","ROLE")</param>
        ''' <param name="groups">An array of data rows that must contain two columns ("PERMISSIONS_SEQ_ID","ROLE")</param>
        ''' <remarks></remarks>
        Protected Overridable Overloads Sub Initialize(ByRef detailDatarow As DataRow, ByVal derivedRoles() As DataRow, ByVal assignedRoles() As DataRow, ByVal groups() As DataRow)
            MyBase.Initialize(detailDatarow)
            setRoleOrGroup(m_DerivedAddRoles, derivedRoles, RoleType.AddRole, m_RoleColumn)
            setRoleOrGroup(m_DerivedDeleteRoles, derivedRoles, RoleType.DeleteRole, m_RoleColumn)
            setRoleOrGroup(m_DerivedEditRoles, derivedRoles, RoleType.EditRole, m_RoleColumn)
            setRoleOrGroup(m_DerivedViewRoles, derivedRoles, RoleType.ViewRole, m_RoleColumn)
            If Not assignedRoles Is Nothing Then
                setRoleOrGroup(m_AssignedAddRoles, assignedRoles, RoleType.AddRole, m_RoleColumn)
                setRoleOrGroup(m_AssignedDeleteRoles, assignedRoles, RoleType.DeleteRole, m_RoleColumn)
                setRoleOrGroup(m_AssignedEditRoles, assignedRoles, RoleType.EditRole, m_RoleColumn)
                setRoleOrGroup(m_AssignedViewRoles, assignedRoles, RoleType.ViewRole, m_RoleColumn)
            End If
            If Not groups Is Nothing Then
                setRoleOrGroup(m_AddGroups, groups, RoleType.AddRole, m_GroupColumn)
                setRoleOrGroup(m_DeleteGroups, groups, RoleType.DeleteRole, m_GroupColumn)
                setRoleOrGroup(m_EditGroups, groups, RoleType.EditRole, m_GroupColumn)
                setRoleOrGroup(m_ViewGroups, groups, RoleType.ViewRole, m_GroupColumn)
            End If

        End Sub

        ''' <summary>
        ''' Return roles associated with the "Add" permission.
        ''' </summary>
        Public ReadOnly Property AssignedAddRoles() As Collection(Of String) Implements IMGroupRolePermissionSecurity.AssignedAddRoles
            Get
                Return m_AssignedAddRoles
            End Get
        End Property

        ''' <summary>
        ''' Return roles associated with the "Add" permission.
        ''' </summary>
        Public ReadOnly Property DerivedAddRoles() As Collection(Of String) Implements IMGroupRolePermissionSecurity.DerivedAddRoles
            Get
                Return m_DerivedAddRoles
            End Get
        End Property

        ''' <summary>
        ''' Return assigned roles associated with the "Delete" permission.
        ''' </summary>
        Public ReadOnly Property AssignedDeleteRoles() As Collection(Of String) Implements IMGroupRolePermissionSecurity.AssignedDeleteRoles
            Get
                Return m_AssignedDeleteRoles
            End Get
        End Property

        ''' <summary>
        ''' Return roles associated with the "Delete" permission.
        ''' </summary>
        Public ReadOnly Property DerivedDeleteRoles() As Collection(Of String) Implements IMGroupRolePermissionSecurity.DerivedDeleteRoles
            Get
                Return m_DerivedDeleteRoles
            End Get
        End Property

        ''' <summary>
        ''' Return assigned roles associated with the "Edit" permission.
        ''' </summary>
        Public ReadOnly Property AssignedEditRoles() As Collection(Of String) Implements IMGroupRolePermissionSecurity.AssignedEditRoles
            Get
                Return m_AssignedEditRoles
            End Get
        End Property

        ''' <summary>
        ''' Return roles associated with the "Edit" permission.
        ''' </summary>
        Public ReadOnly Property DerivedEditRoles() As Collection(Of String) Implements IMGroupRolePermissionSecurity.DerivedEditRoles
            Get
                Return m_DerivedEditRoles
            End Get
        End Property

        ''' <summary>
        ''' Return assigned roles associated with the "View" permission.
        ''' </summary>
        Public ReadOnly Property AssignedViewRoles() As Collection(Of String) Implements IMGroupRolePermissionSecurity.AssignedViewRoles
            Get
                Return m_AssignedViewRoles
            End Get
        End Property

        ''' <summary>
        ''' Return roles associated with the "View" permission.
        ''' </summary>
        Public ReadOnly Property DerivedViewRoles() As Collection(Of String) Implements IMGroupRolePermissionSecurity.DerivedViewRoles
            Get
                Return m_DerivedViewRoles
            End Get
        End Property

        Public ReadOnly Property AddGroups As Collection(Of String) Implements IMGroupRolePermissionSecurity.AddGroups
            Get
                Return m_AddGroups
            End Get
        End Property

        Public ReadOnly Property DeleteGroups As Collection(Of String) Implements IMGroupRolePermissionSecurity.DeleteGroups
            Get
                Return m_DeleteGroups
            End Get
        End Property

        Public ReadOnly Property EditGroups As Collection(Of String) Implements IMGroupRolePermissionSecurity.EditGroups
            Get
                Return m_EditGroups
            End Get
        End Property

        Public ReadOnly Property ViewGroups As Collection(Of String) Implements IMGroupRolePermissionSecurity.ViewGroups
            Get
                Return m_ViewGroups
            End Get
        End Property

        ''' <summary>
        ''' Represents the permission column name.
        ''' </summary>
        Public Property PermissionColumn() As String
            Get
                Return m_PermissionColumn
            End Get
            Set(ByVal value As String)
                m_PermissionColumn = value.Trim
            End Set
        End Property

        ''' <summary>
        ''' Represents the role column name.
        ''' </summary>
        Public Property RoleColumn() As String
            Get
                Return m_RoleColumn
            End Get
            Set(ByVal value As String)
                m_RoleColumn = value.Trim
            End Set
        End Property

        ''' <summary>
        ''' Populates the given permissions roles.
        ''' </summary>
        ''' <param name="refCollection">reference to the role or group colletion</param>
        ''' <param name="roleOrGroups">An array of rows for the role or group</param>
        ''' <param name="permissionType">the type of role or group (View, Add, Edit, Delete)</param>
        ''' <param name="dataColumnName">Name of the column containg the data... will be different for roles and groups.</param>
        ''' <remarks></remarks>
        Private Sub setRoleOrGroup(ByRef refCollection As Collection(Of String), ByVal roleOrGroups() As DataRow, ByVal permissionType As PermissionType, ByVal dataColumnName As String)
            refCollection = New Collection(Of String)
            Dim row As DataRow
            For Each row In roleOrGroups
                If Not IsDBNull(row(m_PermissionColumn)) Then
                    If (Not IsDBNull(row(dataColumnName))) Then
                        If Integer.Parse(row(m_PermissionColumn).ToString()) = CType(permissionType, Integer) Then
                            refCollection.Add(row(dataColumnName).ToString())
                        End If
                    End If
                End If
            Next row
        End Sub

        ''' <summary>
        ''' Will set the collection of roles given a comma seporated string of roles.
        ''' </summary>
        ''' <param name="CommaSeporatedRoles">String of comma seporated roles</param>
        Public Sub SetAssignedRoles(ByVal commaSeporatedRoles As String, ByVal permission As PermissionType)
            Select Case permission
                Case PermissionType.Add
                    setRolesOrGroups(m_AssignedAddRoles, commaSeporatedRoles)
                Case PermissionType.Delete
                    setRolesOrGroups(m_AssignedDeleteRoles, commaSeporatedRoles)
                Case PermissionType.Edit
                    setRolesOrGroups(m_AssignedEditRoles, commaSeporatedRoles)
                Case PermissionType.View
                    setRolesOrGroups(m_AssignedViewRoles, commaSeporatedRoles)
                Case Else
            End Select
        End Sub

        ''' <summary>
        ''' Will set the collection of groups given a comma seporated string of groups.
        ''' </summary>
        ''' <param name="CommaSeporatedGroups">String of comma seporated groups</param>
        Public Sub SetGroups(ByVal commaSeporatedGroups As String, ByVal permission As PermissionType)
            Select Case permission
                Case PermissionType.Add
                    setRolesOrGroups(m_AddGroups, commaSeporatedGroups)
                Case PermissionType.Delete
                    setRolesOrGroups(m_DeleteGroups, commaSeporatedGroups)
                Case PermissionType.Edit
                    setRolesOrGroups(m_EditGroups, commaSeporatedGroups)
                Case PermissionType.View
                    setRolesOrGroups(m_ViewGroups, commaSeporatedGroups)
                Case Else
                    'Do nothing
            End Select
        End Sub

        ''' <summary>
        ''' Converts the collection of AssignedRoles to a comma seporated string.
        ''' </summary>
        ''' <returns>String</returns>
        Public Function GetCommaSeporatedAssingedRoles(ByVal permission As PermissionType) As String
            Select Case permission
                Case PermissionType.Add
                    Return Me.getCommaSeportatedString(m_AssignedAddRoles)
                Case PermissionType.Delete
                    Return Me.getCommaSeportatedString(m_AssignedDeleteRoles)
                Case PermissionType.Edit
                    Return Me.getCommaSeportatedString(m_AssignedEditRoles)
                Case PermissionType.View
                    Return Me.getCommaSeportatedString(m_AssignedViewRoles)
                Case Else
                    Return Me.getCommaSeportatedString(m_AssignedViewRoles)
            End Select
        End Function

        ''' <summary>
        ''' Converts the collection of AssignedGroups to a comma seporated string.
        ''' </summary>
        ''' <returns>String</returns>
        Public Function GetCommaSeporatedGroups(ByVal permission As PermissionType) As String
            Select Case permission
                Case PermissionType.Add
                    Return Me.getCommaSeportatedString(m_AddGroups)
                Case PermissionType.Delete
                    Return Me.getCommaSeportatedString(m_DeleteGroups)
                Case PermissionType.Edit
                    Return Me.getCommaSeportatedString(m_EditGroups)
                Case PermissionType.View
                    Return Me.getCommaSeportatedString(m_ViewGroups)
                Case Else
                    Return Me.getCommaSeportatedString(m_ViewGroups)
            End Select
        End Function

        ' ''' <summary>
        ' ''' Converts the collection of DerivedRoles to a comma seporated string.
        ' ''' </summary>
        ' ''' <returns>String</returns>
        'Public Function GetCommaSeporatedDerivedRoles() As String
        '	Return Me.getCommaSeportatedString(m_DerivedRoles)
        'End Function


#Region "Private Methods"
        ''' <summary>
        ''' Sets the assigned roles or groups.
        ''' </summary>
        ''' <param name="StringCollectionObject">The collection of roles or groups that need to be set</param>
        ''' <param name="GroupsOrRoles">The DataRowCollection that represents either roles or groups</param>
        ''' <param name="ColumnName">The column name to retrieve the data from</param>
        Private Sub setRolesOrGroups(ByRef stringCollectionObject As Collection(Of String), ByVal groupsOrRoles As DataRowCollection, ByVal columnName As String, ByVal permission As PermissionType)
            For Each mRow In groupsOrRoles
                If Not IsDBNull(mRow(columnName)) Then
                    If Integer.Parse(mRow(m_PermissionColumn).ToString()) = permission Then
                        stringCollectionObject.Add(mRow(columnName).ToString())
                    End If
                End If
            Next
        End Sub

        ''' <summary>
        ''' Sets the assigned roles or groups.
        ''' </summary>
        ''' <param name="StringCollectionObject">The collection of roles or groups that need to be set</param>
        ''' <param name="CommaSeporatedString">A comma seporated list of roles or groups 'you, me' as an example</param>
        Private Sub setRolesOrGroups(ByRef stringCollectionObject As Collection(Of String), ByRef commaSeporatedString As String)
            Dim mRoles() As String = commaSeporatedString.Split(",")
            stringCollectionObject = New Collection(Of String)
            For Each mRole In mRoles
                stringCollectionObject.Add(mRole.ToString())
            Next
        End Sub

        Private Function getCommaSeportatedString(ByVal collectionOfStrings As Collection(Of String)) As String
            Dim mRetValue As String = String.Empty
            If Not collectionOfStrings Is Nothing Then
                If collectionOfStrings.Count > 0 Then
                    For Each item In collectionOfStrings
                        mRetValue += item.ToString() + ","
                    Next
                End If
            End If
            If mRetValue.Length > 0 Then
                mRetValue = mRetValue.Substring(0, mRetValue.Length - 1)
            End If
            Return mRetValue
        End Function

#End Region
    End Class
End Namespace

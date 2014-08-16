Imports GrowthWare.Framework.Model.Enumerations
Imports GrowthWare.Framework.Model.Profiles.Interfaces

Namespace Model.Profiles.Base
    Public MustInherit Class MBaseSecurity
        Inherits MProfile
        Implements IMSecurityInfo

        Private m_AddRoles() As String
        Private m_DeleteRoles() As String
        Private m_EditRoles() As String
        Private m_ViewRoles() As String
        Private m_PermissionColumn As String = "PERMISSIONS_SEQ_ID"
        Private m_RoleColumn As String = "ROLE"

        ''' <summary>
        ''' Init orverloads and calles mybase.init to will populate the Add, Delete, Edit, and View role properties.
        ''' </summary>
        ''' <param name="drowSecurity">A data row that must contain two columns ("PERMISSIONS_SEQ_ID","ROLE")</param>
        ''' <remarks></remarks>
        Protected Overloads Sub Initialize(ByVal dataRow As DataRow, ByRef drowSecurity() As DataRow)
            MyBase.Initialize(dataRow)
            m_AddRoles = SplitRoles(drowSecurity, RoleType.AddRole)
            m_DeleteRoles = SplitRoles(drowSecurity, RoleType.DeleteRole)
            m_EditRoles = SplitRoles(drowSecurity, RoleType.EditRole)
            m_ViewRoles = SplitRoles(drowSecurity, RoleType.ViewRole)
        End Sub

        Public ReadOnly Property AddRoles() As String() Implements IMSecurityInfo.AddRoles
            Get
                Return m_AddRoles
            End Get
        End Property

        Public ReadOnly Property DeleteRoles() As String() Implements IMSecurityInfo.DeleteRoles
            Get
                Return m_DeleteRoles
            End Get
        End Property

        Public ReadOnly Property EditRoles() As String() Implements IMSecurityInfo.EditRoles
            Get
                Return m_EditRoles
            End Get
        End Property

        Public ReadOnly Property ViewRoles() As String() Implements IMSecurityInfo.ViewRoles
            Get
                Return m_ViewRoles
            End Get
        End Property

        Public Property PermissionColumn() As String
            Get
                Return m_PermissionColumn
            End Get
            Set(ByVal value As String)
                m_PermissionColumn = value.Trim
            End Set
        End Property

        Public Property RoleColumn() As String
            Get
                Return m_RoleColumn
            End Get
            Set(ByVal value As String)
                m_RoleColumn = value.Trim
            End Set
        End Property

        Protected Function SplitRoles(ByVal allRoles() As DataRow, ByVal moduleRoleType As RoleType) As String()
            Dim colRoles As New ArrayList
            Dim row As DataRow
            For Each row In allRoles
                If Not IsDBNull(row(m_PermissionColumn)) Then
                    If CType(row(m_PermissionColumn), RoleType) = moduleRoleType Then
                        colRoles.Add(row(m_RoleColumn))
                    End If
                End If
            Next row
            Return CType(colRoles.ToArray(GetType(String)), String())
        End Function
    End Class
End Namespace

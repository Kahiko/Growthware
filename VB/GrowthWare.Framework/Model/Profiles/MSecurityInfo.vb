Imports System.Collections.ObjectModel
Imports GrowthWare.Framework.Model.Profiles.Interfaces

Namespace Model.Profiles
    ''' <summary>
    ''' IMSecurityInfo sets the contract for all
    ''' classing inheriting fromm MSecurity.vb
    ''' </summary>
    <Serializable(), CLSCompliant(True)> _
    Public Class MSecurityInfo
        Private m_MayView As Boolean = False
        Private m_MayAdd As Boolean = False
        Private m_MayEdit As Boolean = False
        Private m_MayDelete As Boolean = False

        ''' <summary>
        ''' MayView()--
        ''' This property is calculated relative to the current object that 
        ''' implements ISecurityInfo.  
        ''' When true, user can view the module.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MayView() As Boolean
            Get
                Return m_MayView
            End Get
        End Property

        ''' <summary>
        ''' MayAdd()--
        ''' This property is calculated relative to the current object that 
        ''' implements ISecurityInfo.  
        ''' When true, user can view the module.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MayAdd() As Boolean
            Get
                Return m_MayAdd
            End Get
        End Property

        ''' <summary>
        ''' MayEdit()--
        ''' This property is calculated relative to the current object that 
        ''' implements ISecurityInfo.  
        ''' When true, user can view the module.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MayEdit() As Boolean
            Get
                Return m_MayEdit
            End Get
        End Property

        ''' <summary>
        ''' MayDelete()--
        ''' This property is calculated relative to the current object that 
        ''' implements ISecurityInfo.  
        ''' When true, user can view the module.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MayDelete() As Boolean
            Get
                Return m_MayDelete
            End Get
        End Property

        ''' <summary>
        ''' Creates a new instance of MSecurityInfo
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Initializes a new MSecurityInfo object given an object that implements ISecurityInfo.
        '''  All client permissions are calculated relative to the object and the client roles.
        ''' </summary>
        ''' <param name="groupRolePermissionSecurity">IMGroupRolePermissionSecurity</param>
        ''' <param name="profileWithDerivedRoles">A profile that implements IMGroupRoleSecurity.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal groupRolePermissionSecurity As IMGroupRolePermissionSecurity, ByVal profileWithDerivedRoles As IMGroupRoleSecurity)
            If groupRolePermissionSecurity Is Nothing Then Throw New ArgumentNullException("groupRolePermissionSecurity", "groupRolePermissionSecurity cannot be a null reference (Nothing in Visual Basic)")
            If profileWithDerivedRoles Is Nothing Then Throw New ArgumentNullException("profileWithDerivedRoles", "profileWithDerivedRoles cannot be a null reference (Nothing in Visual Basic)")
            ' Check View Permissions
            m_MayView = CheckAuthenticatedPermission(groupRolePermissionSecurity.DerivedViewRoles, profileWithDerivedRoles.DerivedRoles)
            ' Check Add Permissions
            m_MayAdd = CheckAuthenticatedPermission(groupRolePermissionSecurity.DerivedAddRoles, profileWithDerivedRoles.DerivedRoles)
            ' Check Edit Permissions
            m_MayEdit = CheckAuthenticatedPermission(groupRolePermissionSecurity.DerivedEditRoles, profileWithDerivedRoles.DerivedRoles)
            ' Check Delete Permissions
            m_MayDelete = CheckAuthenticatedPermission(groupRolePermissionSecurity.DerivedDeleteRoles, profileWithDerivedRoles.DerivedRoles)
        End Sub 'New

        ''' <summary>
        ''' Checks whether an account is in the necessary role for the 4 permissions given an objects roles
        ''' </summary>
        ''' <param name="objRoles">Collection(Of String)</param>
        ''' <param name="profileDerivedRoles">The derived roles.</param>
        ''' <returns>True/False</returns>
        ''' <remarks></remarks>
        Protected Shared Function CheckAuthenticatedPermission(ByVal objRoles As Collection(Of String), ByVal profileDerivedRoles As Collection(Of String)) As Boolean
            If objRoles Is Nothing Then Throw New ArgumentNullException("objRoles", "objRoles cannot be a null reference (Nothing in Visual Basic)")
            If profileDerivedRoles Is Nothing Then Throw New ArgumentNullException("profileDerivedRoles", "profileDerivedRoles cannot be a null reference (Nothing in Visual Basic)")
            Dim role As String
            'If objRoles contains the role "Anonymous" the don't bother running the rest of code just return true
            If objRoles.Contains("Anonymous") Then Return True
            If profileDerivedRoles.Contains("SysAdmin") Then Return True
            For Each role In objRoles
                If profileDerivedRoles.Contains(role) Then
                    Return True
                End If
            Next role    ' Nope, doesn't have permissions
            Return False
        End Function
    End Class

End Namespace

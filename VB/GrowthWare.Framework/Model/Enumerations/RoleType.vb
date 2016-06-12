Namespace Model.Enumerations
    ''' <summary>
    ''' Enumerates all role types.
    ''' </summary>
    ''' <remarks>
    ''' Closely coupled with table ZF_PERMISSIONS.
    ''' </remarks>
    <Serializable(), CLSCompliant(True)> _
    Public Enum RoleType
        AddRole = 3
        DeleteRole = 4
        EditRole = 2
        ViewRole = 1
        None = 0
    End Enum 'RoleType 
End Namespace

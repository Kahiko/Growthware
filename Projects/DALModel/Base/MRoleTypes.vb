'*********************************************************************
'
' RoleType Enum
'
' This enumeration represents all the role types that can be used
' with the Application.
'
' NOTE:
'   These values match TBL_PERMISSIONS in the AppDB database
'*********************************************************************
Namespace Base.MRoleType
    Public Enum value
        ViewRole = 1
        EditRole = 2
        AddRole = 3
        DeleteRole = 4
    End Enum 'RoleType 
End Namespace
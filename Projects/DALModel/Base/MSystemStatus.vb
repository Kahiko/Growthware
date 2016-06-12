#Region " Notes "
'*********************************************************************
'
' StatusType Enum
'
' This enumeration represents all the system status types that can be
' used with the Application.
'
' NOTE:
'   These values match TBL_SYSTEM_STATUS in the AppDB database
'*********************************************************************
#End Region
Namespace Base.MSystemStatus
    Public Enum value
        Active = 0
        ChangePassword = 1
        Disabled = 2
        Inactive = 3
    End Enum 'MSystemStatus.value
End Namespace
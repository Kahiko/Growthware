#Region " Notes "
'*********************************************************************
'
' NavTypes Enum
'
' This enumeration represents all the navigation types that can be used
' with the Application.
'
' NOTE:
'   These values match TBL_NAVIGATION_TYPE in the AppHelper.AppDB database
'*********************************************************************
#End Region
Namespace NavTypes
    Public Enum value
        RightHand = 1
        LeftHand = 2
        TopRight = 3
        Horizontal = 4
    End Enum 'NavTypes.value
End Namespace
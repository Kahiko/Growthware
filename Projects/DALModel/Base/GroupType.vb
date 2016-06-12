'*********************************************************************
'
' RoleType Enum
'
' This enumeration represents all the group types that can be used
' with the Application.
'
' NOTE:
'   These values match TBL_PERMISSIONS in the AppDB database
'*********************************************************************
Namespace Base.MGroupType
	Public Enum value
		ViewRole = 1
		EditRole = 2
		AddRole = 3
		DeleteRole = 4
	End Enum	'GroupType 
End Namespace
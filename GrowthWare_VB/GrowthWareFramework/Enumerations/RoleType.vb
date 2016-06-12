Namespace Enumerations
	''' <summary>
	''' Enumerates all role types.
	''' </summary>
	''' <remarks>
	''' Closely coupled with table ZF_PERMISSIONS or ZGWSecurity.Permissions.
	''' </remarks>
	<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")> <Serializable(), CLSCompliant(True)> _
	Public Enum RoleType
		''' <summary>
		''' Represents add permission
		''' </summary>
		AddRole = 3

		''' <summary>
		''' Represents delete permission
		''' </summary>
		DeleteRole = 4

		''' <summary>
		''' Represents edit permission
		''' </summary>
		EditRole = 2

		''' <summary>
		''' Represents view permission
		''' </summary>
		ViewRole = 1
	End Enum 'RoleType 
End Namespace
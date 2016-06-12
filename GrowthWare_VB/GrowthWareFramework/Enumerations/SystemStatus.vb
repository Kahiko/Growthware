Namespace Enumerations
	''' <summary>
	''' Enumeration of system status
	''' </summary>
	''' <remarks>
	''' Values match ZF_SYSTEM_STATUS in the database
	''' </remarks>
	<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")>
	Public Enum SystemStatus
		Active = 1
		ChangePassword = 4
		Disabled = 3
		Inactive = 2
	End Enum
End Namespace
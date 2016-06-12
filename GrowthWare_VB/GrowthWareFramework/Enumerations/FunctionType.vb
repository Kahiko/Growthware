Namespace Enumerations
    ''' <summary>
    ''' Enumeration of menu types
    ''' </summary>
    ''' <remarks>
    ''' Values match ZF_FUNCTION_TYPES in the database
    ''' </remarks>
	<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")> Public Enum FunctionType
		Modules = 1
		Security = 2
		Menu_Item = 3
		HTML_Content = 4
		Content = 5
	End Enum
End Namespace
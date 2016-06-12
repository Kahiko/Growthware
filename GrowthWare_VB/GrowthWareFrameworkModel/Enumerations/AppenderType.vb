Namespace Enumerations
    ''' <summary>
    ''' Enumeration of Log4Net appenders
    ''' </summary>
    ''' <remarks>
    ''' 
    ''' </remarks>
	<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId:="Appender")> Public Enum AppenderType
		''' <summary>
		''' File type
		''' </summary>
		''' <remarks></remarks>
		File = 0
		''' <summary>
		''' Database type
		''' </summary>
		''' <remarks></remarks>
		Database = 1
		''' <summary>
		''' EMail
		''' </summary>
		''' <remarks></remarks>
		Email = 2
	End Enum
End Namespace
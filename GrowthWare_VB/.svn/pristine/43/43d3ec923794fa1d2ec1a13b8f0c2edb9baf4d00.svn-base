Namespace DataAccessLayer
	''' <summary>
	''' Created to distinguish errors created in the data access layer.
	''' </summary>
	''' <remarks></remarks>
	<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors")>
	<Serializable()>
	Public NotInheritable Class DataAccessLayerException
		Inherits Exception

		<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors")>
		Private Sub New()

		End Sub

		''' <summary>
		''' Calls base method
		''' </summary>
		''' <param name="message">String</param>
		''' <remarks></remarks>
		Public Sub New(ByVal message As String)
			MyBase.New(message)
		End Sub

		''' <summary>
		''' Calls base method
		''' </summary>
		''' <param name="message">String</param>
		''' <param name="innerException">Exception</param>
		''' <remarks></remarks>
		Public Sub New(ByVal message As String, ByVal innerException As Exception)
			MyBase.New(message, innerException)
		End Sub

	End Class
End Namespace
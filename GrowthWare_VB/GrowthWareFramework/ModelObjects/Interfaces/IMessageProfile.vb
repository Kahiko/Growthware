Namespace ModelObjects
	Public Interface IMessageProfile
		Property Body() As String
		Property Title() As String
		Property FormatAsHTML() As Boolean
		Sub FormatBody()
	End Interface
End Namespace
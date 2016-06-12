Namespace ModelObjects
    ''' <summary>
    ''' All propertyies represent the column names in the ZF_ACCT_CHOICES table.
    ''' </summary>
    ''' <remarks></remarks>
	<Serializable(), CLSCompliant(True)> _
	Public NotInheritable Class MClientChoices
		Private Sub New()

		End Sub

		Shared ReadOnly Property RecordsPerPage() As String
			Get
				Return "RECORDS_PER_PAGE"
			End Get
		End Property

		Shared ReadOnly Property AnonymousClientChoicesState() As String
			Get
				Return "AnonymousClientChoicesState"
			End Get
		End Property

		Shared ReadOnly Property SessionName() As String
			Get
				Return "ClientChoicesState"
			End Get
		End Property

		Shared ReadOnly Property AccountName() As String
			Get
				Return "ACCT"
			End Get
		End Property

		Shared ReadOnly Property SecurityEntityID() As String
			Get
				Return "SE_SEQ_ID"
			End Get
		End Property

		Shared ReadOnly Property SecurityEntityName() As String
			Get
				Return "SE_NAME"
			End Get
		End Property

		Shared ReadOnly Property BackColor() As String
			Get
				Return "BACK_COLOR"
			End Get
		End Property

		Shared ReadOnly Property LeftColor() As String
			Get
				Return "LEFT_COLOR"
			End Get
		End Property

		Shared ReadOnly Property HeadColor() As String
			Get
				Return "HEAD_COLOR"
			End Get
		End Property

		Shared ReadOnly Property SubheadColor() As String
			Get
				Return "SUB_HEAD_COLOR"
			End Get
		End Property

		Shared ReadOnly Property ColorScheme() As String
			Get
				Return "COLOR_SCHEME"
			End Get
		End Property

		Shared ReadOnly Property Action() As String
			Get
				Return "FAVORIATE_ACTION"
			End Get
		End Property
	End Class
End Namespace
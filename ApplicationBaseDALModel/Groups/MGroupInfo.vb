
Namespace Group
	<Serializable(), CLSCompliant(True)> _
	Public Class MGroupInfo
		Private _groupSeqId As Integer = 0
		Private _groupName As String = String.Empty
		Private _groupDescription As String = String.Empty
		Private _businessUnitId As Integer = 0

		Public Property BusinessUnitId() As Integer
			Get
				Return _businessUnitId
			End Get
			Set(ByVal Value As Integer)
				_businessUnitId = Value
			End Set
		End Property
		Public Property GroupId() As Integer
			Get
				Return _groupSeqId
			End Get
			Set(ByVal Value As Integer)
				_groupSeqId = Value
			End Set
		End Property
		Public Property GroupName() As String
			Get
				Return _groupName
			End Get
			Set(ByVal Value As String)
				_groupName = Value.Trim
			End Set
		End Property
		Public Property GroupDescription() As String
			Get
				Return _groupDescription
			End Get
			Set(ByVal Value As String)
				_groupDescription = Value
			End Set
		End Property
	End Class
End Namespace
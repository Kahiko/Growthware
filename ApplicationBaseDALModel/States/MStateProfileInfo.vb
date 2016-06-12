Namespace States
	'*******************************************
	' StateProfileInfo class represents
	' all the profile information for a given state
	'*******************************************

	<CLSCompliant(True)> _
	   Public Class MStateProfileInfo
		Private _State As String = String.Empty
		Private _LongName As String = String.Empty
		Private _STATUS_SEQ_ID As Integer = 3

		Public Sub New()

		End Sub

		Public Sub New(ByVal drowDirectory As DataRow)
			_State = CStr(drowDirectory("State"))
			_LongName = CStr(drowDirectory("LongName"))
			_STATUS_SEQ_ID = CInt(drowDirectory("STATUS_SEQ_ID"))
		End Sub	'New

		'*********************************************************************
		'
		' State Property
		'
		' Specifies the id of the State.
		'
		'*********************************************************************
		Public Property State() As String
			Get
				Return _State
			End Get
			Set(ByVal Value As String)
				_State = Value
			End Set
		End Property ' State

		'*********************************************************************
		'
		' LongName Property
		'
		' Specifies long name of the state.
		'
		'*********************************************************************
		Public Property LongName() As String
			Get
				Return _LongName
			End Get
			Set(ByVal Value As String)
				_LongName = Value
			End Set
		End Property ' LongName

		Public Property STATUS_SEQ_ID() As Integer
			Get
				Return _STATUS_SEQ_ID
			End Get
			Set(ByVal Value As Integer)
				_STATUS_SEQ_ID = Value
			End Set
		End Property
	End Class
End Namespace
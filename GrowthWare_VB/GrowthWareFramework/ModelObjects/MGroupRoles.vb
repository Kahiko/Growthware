Namespace ModelObjects
	<Serializable(), CLSCompliant(True)> _
	Public Class MGroupRoles
		Private mADD_UP_BY As Integer
		Private mSE_SEQ_ID As Integer = -1
		Private mGROUP_SEQ_ID As Integer = -1
		Private mRoles As String

		Public Property ADD_UP_BY() As Integer
			Get
				Return mADD_UP_BY
			End Get
			Set(ByVal value As Integer)
				mADD_UP_BY = value
			End Set
		End Property

		Public Property SE_SEQ_ID() As Integer
			Get
				Return mSE_SEQ_ID
			End Get
			Set(ByVal value As Integer)
				mSE_SEQ_ID = value
			End Set
		End Property

		Public Property GROUP_SEQ_ID() As Integer
			Get
				Return mGROUP_SEQ_ID
			End Get
			Set(ByVal value As Integer)
				mGROUP_SEQ_ID = value
			End Set
		End Property

		Public Property Roles() As String
			Get
				Return mRoles
			End Get
			Set(ByVal value As String)
				mRoles = value.Trim
			End Set
		End Property
	End Class
End Namespace
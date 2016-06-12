Namespace Profiles
	<Serializable(), CLSCompliant(True)> _
	Public Class MClientChoicesState
		Dim mClientChoices As System.Collections.Hashtable = New System.Collections.Hashtable
		Dim mAccountName As String
		Dim mIsDirty As Boolean = False

		Public Sub New(ByVal clientChoicesData As DataRow)
			MyBase.New()
			mAccountName = AccountName
			Try
				Dim myTable As DataTable = clientChoicesData.Table.Copy
				Dim Row As DataRow = myTable.Rows(0)
				Dim i As Integer
				For i = 0 To myTable.Columns.Count - 1
					Dim Value As Object = Row.Item(myTable.Columns(i))
					mClientChoices(myTable.Columns(i).ToString()) = Value.ToString()
				Next i
			Catch
				Throw
			Finally
			End Try
		End Sub

		Public Property AccountName() As String
			Get
				Return mAccountName
			End Get
			Set(ByVal Value As String)
				mAccountName = Value
			End Set
		End Property

		Public ReadOnly Property ChoicesHashtable() As Hashtable
			Get
				Return mClientChoices
			End Get
		End Property

		Default Public Property Item(ByVal key As String) As String
			Get
				Return CType(mClientChoices(key), String)
			End Get
			Set(ByVal Value As String)
				mClientChoices(key) = Value
				mIsDirty = True
			End Set
		End Property

		Public Property isDirty() As Boolean
			Get
				Return mIsDirty
			End Get
			Set(ByVal value As Boolean)
				mIsDirty = value
			End Set
		End Property
	End Class
End Namespace
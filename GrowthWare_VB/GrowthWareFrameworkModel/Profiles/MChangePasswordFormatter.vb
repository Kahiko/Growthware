Imports GrowthWare.Framework.ModelObjects
Imports GrowthWare.Framework.ModelObjects.Base

Namespace ModelObjects
	<Serializable(), CLSCompliant(True)> _
	Public Class MChangePasswordFormatter
		Inherits MMessageProfile

#Region "Member Properties"
		Private _AccountName As String = String.Empty
		Private _FullName As String = String.Empty
		Private _Password As String = String.Empty
		Private _Server As String = String.Empty
#End Region

#Region "Public Properties"
		Public Property AccountName() As String
			Get
				Return _AccountName
			End Get
			Set(ByVal value As String)
				_AccountName = value.Trim
			End Set
		End Property

		Public Property FullName() As String
			Get
				Return _FullName
			End Get
			Set(ByVal value As String)
				_FullName = value.Trim
			End Set
		End Property

		Public Property Password() As String
			Get
				Return _Password
			End Get
			Set(ByVal value As String)
				_Password = value.Trim
			End Set
		End Property

		Public Property Server() As String
			Get
				Return _Server
			End Get
			Set(ByVal value As String)
				_Server = value.Trim
			End Set
		End Property
#End Region
	End Class
End Namespace
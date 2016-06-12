Namespace Directories
	<CLSCompliant(True)> _
	   Public Class MDirectoryProfileInformation
		Private _BUSINESS_UNIT_SEQ_ID As Integer
		Private _Directory As String = String.Empty
		Private _Impersonate As Boolean = False
		Private _Impersonate_Account As String = String.Empty
		Private _Impersonate_PWD As String = String.Empty

		Public Property BUSINESS_UNIT_SEQ_ID() As Integer
			Get
				Return _BUSINESS_UNIT_SEQ_ID
			End Get
			Set(ByVal Value As Integer)
				_BUSINESS_UNIT_SEQ_ID = Value
			End Set
		End Property

		Public Property Directory() As String
			Get
				Return _Directory
			End Get
			Set(ByVal Value As String)
				_Directory = Value.Trim
			End Set
		End Property

		Public Property Impersonate() As Boolean
			Get
				Return _Impersonate
			End Get
			Set(ByVal Value As Boolean)
				_Impersonate = Value
			End Set
		End Property

		Public Property Impersonate_Account() As String
			Get
				Return _Impersonate_Account
			End Get
			Set(ByVal Value As String)
				_Impersonate_Account = Value.Trim
			End Set
		End Property

		Public Property Impersonate_PWD() As String
			Get
				Return _Impersonate_PWD
			End Get
			Set(ByVal Value As String)
				_Impersonate_PWD = Value.Trim
			End Set
		End Property

		'*********************************************************************
		'
		' DirectoryInfo Constructor
		' Creates the DirectoryInfo object with default information. 
		'
		'*********************************************************************
		Public Sub New()

		End Sub

		'*********************************************************************
		'
		' DirectoryInfo Constructor
		' Initializes the DirectoryInfo object with a DataRow. 
		'
		'*********************************************************************
		Public Sub New(ByVal drowDirectory As DataRow)
			On Error Resume Next
			_BUSINESS_UNIT_SEQ_ID = CStr(drowDirectory("BUSINESS_UNIT_SEQ_ID"))
			_Directory = CStr(drowDirectory("Directory"))
			_Impersonate = CBool(drowDirectory("Impersonate"))
			_Impersonate_Account = CStr(drowDirectory("Impersonate_Account"))
			_Impersonate_PWD = CStr(drowDirectory("Impersonate_PWD"))
		End Sub	   'New
	End Class
End Namespace
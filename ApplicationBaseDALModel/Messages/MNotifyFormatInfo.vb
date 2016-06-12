Imports System
Namespace Messages.Notify
	'*********************************************************************
	'
	' NotifyFormatInfo Class
	'
	' Represents all the elements that will be replaced in 
	' a notification email.
	'
	'*********************************************************************
	<CLSCompliant(True)> _
	  Public Class MNotifyFormatInfo
		Private _username As String = String.Empty
		Private _Server As String = String.Empty
		Private _fullUsername As String = String.Empty
		Private _password As String = String.Empty
		Private _editProfileLink As String = String.Empty
		Private _contentTitle As String = String.Empty
		Private _contentLink As String = String.Empty
		Private _sectionName As String = String.Empty
		Private _formName As String = String.Empty

		'*********************************************************************
		'
		' Username Property
		'
		' Represents the username replaced in the email. 
		'
		'*********************************************************************

		Public Property Username() As String
			Get
				Return _username
			End Get
			Set(ByVal Value As String)
				_username = Value
			End Set
		End Property

		'*********************************************************************
		'
		' Server Property
		'
		' Represents the server replaced in the email. 
		'
		'*********************************************************************
		Public Property Server() As String
			Get
				Return _Server
			End Get
			Set(ByVal Value As String)
				_Server = Value
			End Set
		End Property

		'*********************************************************************
		'
		' Password Property
		'
		' Represents the password replaced in the email. 
		'
		'*********************************************************************
		Public Property Password() As String
			Get
				Return _password
			End Get
			Set(ByVal Value As String)
				_password = Value
			End Set
		End Property

		'*********************************************************************
		'
		' FullUsername Property
		'
		' Represents the full username replaced in the email.
		' Includes first and last name. 
		'
		'*********************************************************************

		Public Property FullUsername() As String
			Get
				Return _fullUsername
			End Get
			Set(ByVal Value As String)
				_fullUsername = Value
			End Set
		End Property

		'*********************************************************************
		'
		' EditProfileLink Property
		'
		' Represents the link to the user profile replaced in the email. 
		'
		'*********************************************************************

		Public Property EditProfileLink() As String
			Get
				Return _editProfileLink
			End Get
			Set(ByVal Value As String)
				_editProfileLink = Value
			End Set
		End Property

		'*********************************************************************
		'
		' ContentTitle Property
		'
		' Represents the title of the content item replaced in the email. 
		'
		'*********************************************************************

		Public Property ContentTitle() As String
			Get
				Return _contentTitle
			End Get
			Set(ByVal Value As String)
				_contentTitle = Value
			End Set
		End Property

		'*********************************************************************
		'
		' ContentLink Property
		'
		' Represents the link to the content page replaced in the email. 
		'
		'*********************************************************************

		Public Property ContentLink() As String
			Get
				Return _contentLink
			End Get
			Set(ByVal Value As String)
				_contentLink = Value
			End Set
		End Property

		'*********************************************************************
		'
		' SectionName Property
		'
		' Represents the name of the section replaced in the email. 
		'
		'*********************************************************************

		Public Property SectionName() As String
			Get
				Return _sectionName
			End Get
			Set(ByVal Value As String)
				_sectionName = Value
			End Set
		End Property

		Public Property FormName() As String
			Get
				Return _formName
			End Get
			Set(ByVal Value As String)
				_formName = Value
			End Set
		End Property

		'*********************************************************************
		'
		' NotfifyFormatInfo Constructor
		'
		' Initializes the NotifyFormatInfo class. 
		'
		'*********************************************************************
		Public Sub New()
		End Sub	'New
	End Class 'NotifyFormatInfo
End Namespace
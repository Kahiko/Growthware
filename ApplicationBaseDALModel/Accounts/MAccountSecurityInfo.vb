Imports System
Imports System.Web
Imports ApplicationBase.Model.Modules

Namespace Accounts.Security
	'*********************************************************************
	'
	' MAccountSecurityInfo Class
	' Represents all the security information about a client.
	' Given the roles in a cookie that was set in
	' ClientUtility.GetAllClientRoles()
	'
	'*********************************************************************
	<CLSCompliant(True)> _
	Public Class MAccountSecurityInfo
		Private _isAuthenticated As Boolean = False
		Private _isSystemAdministrator As Boolean = False
		Private _accountName As String = "Anonymous"
		Private _mayView As Boolean = False
		Private _mayAdd As Boolean = False
		Private _mayEdit As Boolean = False
		Private _mayDelete As Boolean = False

		'*********************************************************************
		'
		' IsAuthenticated Property
		' Indicates whether the user is anonymous or authenticated.
		'
		'*********************************************************************
		Public ReadOnly Property IsAuthenticated() As Boolean
			Get
				Return _isAuthenticated
			End Get
		End Property

		'*********************************************************************
		'
		' IsAdministrator Property
		' Indicates whether the current user is a member of the 
		' SysAdmin role.
		'
		'*********************************************************************
		Public ReadOnly Property IsSystemAdministrator() As Boolean
			Get
				Return _isSystemAdministrator
			End Get
		End Property

		'*********************************************************************
		'
		' AccountName Property
		' The AccountName of the current user, aids in debug
		' by letting the developer see that the client security information
		' object is for who they are expecting.
		'
		'*********************************************************************
		Public ReadOnly Property AccountName() As String
			Get
				Return _accountName
			End Get
		End Property

		'*********************************************************************
		'
		' MayView Property
		' This property is calculated relative to the current module.
		' When true, user can view the module.
		'
		'*********************************************************************
		Public ReadOnly Property MayView() As Boolean
			Get
				Return _mayView
			End Get
		End Property

		'*********************************************************************
		'
		' MayAdd Property
		' This property is calculated relative to the current module.
		' When true, user can add information for this module.
		'
		'*********************************************************************
		Public ReadOnly Property MayAdd() As Boolean
			Get
				Return _mayAdd
			End Get
		End Property

		'*********************************************************************
		'
		' MayEdit Property
		' This property is calculated relative to the current module.
		' When true, user can edit information for this module.
		'
		'*********************************************************************
		Public ReadOnly Property MayEdit() As Boolean
			Get
				Return _mayEdit
			End Get
		End Property

		'*********************************************************************
		'
		' MayDelete Property
		' This property is calculated relative to the current module.
		' When true, user can delete information for this module.
		'
		'*********************************************************************
		Public ReadOnly Property MayDelete() As Boolean
			Get
				Return _mayDelete
			End Get
		End Property

		Public Sub New()

		End Sub
		'*********************************************************************
		'
		' MAccountSecurityInfo Constructor
		' Initializes a new MAccountSecurityInfo object given a module. All
		' client permissions are calculated relative to the module.
		'
		'*********************************************************************
		Public Sub New(ByVal objModuleProfileInfo As MModuleProfileInfo)
			' Set the account name if possible
			If Not HttpContext.Current.User Is Nothing Then _accountName = HttpContext.Current.User.Identity.Name
			' Check whether authenticated
			_isAuthenticated = HttpContext.Current.Request.IsAuthenticated
			If _isAuthenticated Then
				' Check if System Administrator
				If HttpContext.Current.User.IsInRole("SysAdmin") Then _isSystemAdministrator = True
			End If
			' Check View Permissions
			_mayView = CheckAuthenticatedPermission(objModuleProfileInfo.ViewRoles)
			' Check Add Permissions
			_mayAdd = CheckAuthenticatedPermission(objModuleProfileInfo.AddRoles)
			' Check Edit Permissions
			_mayEdit = CheckAuthenticatedPermission(objModuleProfileInfo.EditRoles)
			' Check Delete Permissions
			_mayDelete = CheckAuthenticatedPermission(objModuleProfileInfo.DeleteRoles)
		End Sub	'New

		'*********************************************************************
		'
		' CheckAuthenticatedPermission Method
		' Checks whether an account is in the necessary role.
		'
		'*********************************************************************
		Private Function CheckAuthenticatedPermission(ByVal PageRoles() As String) As Boolean
			' If page/module contains the role "Anonymous" the don't bother running the rest of code just return true
			If Array.IndexOf(PageRoles, "Anonymous") <> -1 Then Return True
			' SysAdmin is special and will always have rights to everything all states
			If _isSystemAdministrator Then Return True
			' Otherwise, check each role
			Dim role As String
			For Each role In PageRoles
				If HttpContext.Current.User.IsInRole(role) Then
					Return True
				End If
			Next role	 ' Nope, doesn't have permissions
			Return False
		End Function 'CheckAuthenticatedPermission
	End Class ' MAccountSecurityInfo
End Namespace
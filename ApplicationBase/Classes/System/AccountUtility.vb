Imports ApplicationBase.BusinessLogic
Imports ApplicationBase.Common.Cache
Imports ApplicationBase.Model.Accounts
Imports ApplicationBase.Model.Accounts.Security
Imports ApplicationBase.Model.Special.ClientChoices
Imports ApplicationBase.Model.WorkFlows
Imports ApplicationBase.Model.Modules
Imports System.Web
Imports System.Web.Security
Imports System.Security.Principal
Imports System.Data

#Region " Notes "
' The AccountUtility class aids in managing the account information.
#End Region
Public Class AccountUtility

    Private _context As HttpContext
	Public ReadOnly CachedDvAccounts As String = "dvAccounts"
	Public ReadOnly AnonymousAccountProfileInfo As String = "AnonymousAccountProfileInfo"
	Public ReadOnly SessionAccountProfileInfo As String = "AccountProfileInfo"
	Public ReadOnly SessionAccountSecurityInfo As String = "AccountSecurityInfo"


    Public Sub New(ByRef yourCurrentContext As HttpContext)
        _context = yourCurrentContext
    End Sub

	Public Sub GetAccountSecurityInfo(ByRef accountSecurityInfo As MAccountSecurityInfo)
		Dim mySecurityInfo As New MAccountSecurityInfo()
		accountSecurityInfo = CType(_context.Items(SessionAccountSecurityInfo), MAccountSecurityInfo)
		If accountSecurityInfo Is Nothing Then
			accountSecurityInfo = New MAccountSecurityInfo(AppModulesUtility.GetCurrentModule)
		End If
	End Sub

	Public Function GetAccountsByLetter(ByVal AccountType As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As DataView
		Dim dsAccounts As New DataSet
		Dim dvAccounts As DataView
		dvAccounts = _context.Cache.Item(CachedDvAccounts & AccountType & BUSINESS_UNIT_SEQ_ID)
		If dvAccounts Is Nothing Then
			dsAccounts = BAccount.GetAccountsByLetter(dsAccounts, AccountType, BUSINESS_UNIT_SEQ_ID)
			dvAccounts = dsAccounts.Tables(0).DefaultView
			CacheControler.AddToCacheDependency(CachedDvAccounts & AccountType & BUSINESS_UNIT_SEQ_ID, dvAccounts)
		End If
		Return dvAccounts
	End Function

	'*********************************************************************
	'
	' GetProfile Method
	'
	' Attempts to retrive the account profile information from
	' the session.  If unsuccessfull retrieves the information
	' from the database
	'
	'*********************************************************************
	Public Function GetAccountProfileInfo(ByVal Account As String, Optional ByVal force As Boolean = False) As MAccountProfileInfo
		' There are several seniros to deal with three of which require datastore access
		' 1.)	There is no session - get the data from the datastore
		' 2.)	The account is Anonymous
		' 3.)	The account is Anonymous and not in cache - get the data from the datastore
		' 4.)	The account is in the session
		' 5.)	The session information is not for the requested account - get the data from the data store
		' 6.)	Requester is asking to force the issue and get the data from the datastore
		Dim accountProfileInfo As MAccountProfileInfo
		If _context.Session Is Nothing Then
			accountProfileInfo = BAccount.GetProfile(Account)
		Else
			If Account.Trim.ToLower = "anonymous" Then
				accountProfileInfo = CType(_context.Cache.Item(AnonymousAccountProfileInfo), MAccountProfileInfo)
				If accountProfileInfo Is Nothing OrElse force Then
					accountProfileInfo = BAccount.GetProfile(Account)
					CacheControler.AddToCacheDependency(AnonymousAccountProfileInfo, accountProfileInfo)
				End If
			Else
				accountProfileInfo = CType(_context.Session(SessionAccountProfileInfo), MAccountProfileInfo)
				If (accountProfileInfo Is Nothing) OrElse Not (Account.ToLower = _context.User.Identity.Name.ToLower) OrElse _context.User.Identity.Name.Trim.Length = 0 OrElse force Then
					If Account.Trim.Length > 0 Then
						accountProfileInfo = BAccount.GetProfile(Account)
						HttpContext.Current.Session(SessionAccountProfileInfo) = accountProfileInfo
					End If
				End If
			End If
		End If
		Return accountProfileInfo
	End Function

	'*********************************************************************
	'
	' GetAllAccountRoles Method
	'
	' Retrieves a list of all account roles excluding system roles
	' such as the Everyone and Authenticated roles.
	'
	'*********************************************************************
	Public Sub GetAllAccountRoles(Optional ByVal Force As Boolean = False)
		Dim accountRoles As String() = Nothing
		Dim formattedAccountRoles As String
		Dim clientChoicesUtility As New ClientChoicesUtility
		Dim BusinessUnit As String = ""
		Dim Account As String = _context.User.Identity.Name
		' Get the roles this user is in
		Dim rolesCookie As String = Account & "Roles"
		If Not (_context.Request.Cookies(rolesCookie) Is Nothing OrElse _context.Request.Cookies(rolesCookie).Value = "" OrElse Force) Then
			' Get roles from roles cookie
			Dim ticket As FormsAuthenticationTicket = FormsAuthentication.Decrypt(_context.Request.Cookies(rolesCookie).Value)
			'convert the string representation of the role data into a string array
			Dim rolesArrayList As New ArrayList
			Dim role As String
			For Each role In ticket.UserData.Split(New Char() {";"c})
				If role <> "" Then
					rolesArrayList.Add(role)
				End If
			Next role
			accountRoles = CType(rolesArrayList.ToArray(GetType(String)), String())
		Else
			Dim myAccountUtility As New AccountUtility(HttpContext.Current)
			Dim accountProfileInfo As MAccountProfileInfo
			BusinessUnit = clientChoicesUtility.GetSelectedBusinessUnit
			accountProfileInfo = myAccountUtility.GetAccountProfileInfo(Account)
			accountRoles = BAccount.GetRolesFromDB(accountProfileInfo.ACCOUNT_SEQ_ID, BusinessUnit)
			' Format string array
			formattedAccountRoles = ""
			Dim role As String
			For Each role In accountRoles
				formattedAccountRoles &= role & ";"
			Next role
			' Create authentication ticket
			' ticket params: version, user name, issue time, expires every hour, don't persist cookie, roles
			Dim ticket As New FormsAuthenticationTicket(1, Account, DateTime.Now, DateTime.Now.AddHours(1), False, formattedAccountRoles)
			' Encrypt the ticket
			Dim cookieStr As String = FormsAuthentication.Encrypt(ticket)
			' Send the cookie to the client
			_context.Response.Cookies(rolesCookie).Value = cookieStr
			'Context.Response.Cookies([rolesCookie]).Path = BaseHelper.AppPath ' having issues
			_context.Response.Cookies(rolesCookie).Expires = DateTime.Now.AddMinutes(5)
		End If
		' Add our own custom principal to the request containing the roles in the auth ticket
		_context.User = New GenericPrincipal(_context.User.Identity, accountRoles)
	End Sub	  'GetAllAccountRoles

	'*********************************************************************
	'
	' SignOut Method
	'
	' Logout a client by destroying the cookies.
	'
	'*********************************************************************
	Public Sub SignOut()
		' Sign Out
		RemoveAccountInMemoryInformation(False, True)
		FormsAuthentication.SignOut()
		NavControler.NavTo("generichome")
	End Sub	'SignOut

	'*********************************************************************
	'
	' RemoveCachedModules Method
	'
	' Removes in memory account information
	'
	'*********************************************************************
	Public Sub RemoveAccountInMemoryInformation(Optional ByVal GetRoles As Boolean = True, Optional ByVal RemoveWorkFlow As Boolean = False)
		Dim x As Integer
		On Error Resume Next
		' remove all cookies except ".aspxauth" and "ASP.NET_SessionId"
		For x = 0 To _context.Request.Cookies.Count - 1
			Dim myCookieName As String = _context.Request.Cookies.GetKey(x).ToString
			If InStr(".aspxauth", myCookieName, CompareMethod.Text) = 0 And InStr("ASP.NET_SessionId", myCookieName, CompareMethod.Text) = 0 Then
				_context.Response.Cookies(myCookieName).Value = Nothing
				_context.Response.Cookies(myCookieName).Expires = New System.DateTime(1999, 10, 12)
			End If
		Next
		' get new roles from the data store
		If GetRoles Then
			GetAllAccountRoles(True)
		End If
		' check for a work flow object
		Dim inMemoryWorkFlow As MWorkFlowProfileInfo = WorkFlowUtility.GetCurrentWFP
		'remove all sesssion information
		_context.Session.RemoveAll()
		' re-create the context item for the account info
		_context.Items.Remove(SessionAccountSecurityInfo)
		Dim accountSecurityInfo As New MAccountSecurityInfo(AppModulesUtility.GetCurrentModule)
		_context.Items(SessionAccountSecurityInfo) = accountSecurityInfo		  ' place into context
		' if there was a work flow put it back into the session
		If Not RemoveWorkFlow Then
			If Not inMemoryWorkFlow Is Nothing Then
				WorkFlowUtility.SetWorkFlow(inMemoryWorkFlow)
				NavControler.StartWorkFlow(inMemoryWorkFlow.WorkFlowName)
			End If
		End If
	End Sub	'RemoveAccountStateCache

	Public Sub RemoveCachedAccounts(ByVal BUSINESS_UNIT_SEQ_ID As Integer)
		Dim AccountType As Integer
		Dim AccountTypes As Integer() = System.Enum.GetValues(GetType(MAccountTypes.value))
		CacheControler.RemoveFromCache(BUSINESS_UNIT_SEQ_ID & "Roles" & 0)
		For Each AccountType In AccountTypes
			CacheControler.RemoveFromCache(CachedDvAccounts & AccountType & BUSINESS_UNIT_SEQ_ID)
		Next
	End Sub
End Class
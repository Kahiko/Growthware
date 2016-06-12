Imports System.Globalization
Imports System.Runtime.InteropServices
Imports System.Security.Principal
Imports GrowthWare.Framework.Web.Utilities

Public NotInheritable Class WebImpersonate

	''' <summary>
	''' Public constructor
	''' </summary>
	''' <remarks></remarks>
	Private Sub New()

	End Sub

	''' <summary>
	''' Begins the impersonation process
	''' </summary>
	''' <param name="account">String including domain ie domain\useraccount</param>
	''' <param name="password">String</param>
	''' <returns>WindowsImpersonationContext</returns>
	''' <remarks></remarks>
	Public Shared Function ImpersonateNow(ByVal account As String, ByVal password As String) As WindowsImpersonationContext
		If account Is Nothing Or password Is Nothing Then
			Throw New ArgumentNullException("account", "account or pasword can not be null.")
		End If
		Dim tokenHandle As New IntPtr(0)
		Dim dupeTokenHandle As New IntPtr(0)
		'Dim myImpersonate As New WebImpersonate

		Dim userName As String = String.Empty
		Dim domainName As String = String.Empty
		Dim posSlash As Integer = InStr(account, "\")
		If posSlash <> 0 Then
			userName = account
			domainName = account
			userName = userName.Remove(0, posSlash)
			domainName = domainName.Remove((posSlash - 1), (account.Length - posSlash) + 1)
		Else
			userName = account
		End If
		Dim returnValue As Boolean = NativeMethods.LogonUser(userName, domainName, password, NativeMethods.Logon32LogonInteractive, NativeMethods.Long32ProviderDefault, tokenHandle)
		If Not returnValue Then
			Dim log As LogUtility = LogUtility.GetInstance
			Dim mExceptionMsg As String = "Could not impersonate user: " & userName & " for domain " & domainName
			Dim mException As WebImpersonateException = New Exception(mExceptionMsg)
			log.Fatal(mException)
			Throw mException
		End If
		Dim retVal As Boolean = NativeMethods.DuplicateToken(tokenHandle, NativeMethods.SecurityImpersonation, dupeTokenHandle)
		If Not retVal Then
			NativeMethods.CloseHandle(tokenHandle)
			Dim mLog As LogUtility = LogUtility.GetInstance()
			Dim mExceptionMsg As String = "Exception thrown when trying to duplicate token." + Environment.NewLine + "Account: " + account + Environment.NewLine + "Domain: " + domainName
			Dim mException As WebImpersonateException = New Exception(mExceptionMsg)
			mLog.Error(mException)
			Throw mException
		End If
		' The token that is passed to the following constructor must 
		' be a primary token in order to use it for impersonation.
		Dim newId As New WindowsIdentity(dupeTokenHandle)
		Dim impersonatedUser As WindowsImpersonationContext = Nothing
		Try
			impersonatedUser = newId.Impersonate()
		Catch ex As Exception
			Throw
		Finally
			newId.Dispose()
		End Try
		Return impersonatedUser
	End Function

End Class

Friend Module NativeMethods
	''' <summary>
	''' Represents a 32 bit logon int for the defaut provider
	''' </summary>
	Public ReadOnly Property Long32ProviderDefault() As Integer
		Get
			Return 0
		End Get
	End Property

	''' <summary>
	''' Represents a 32 bit logon int for interactive
	''' </summary>
	Public ReadOnly Property Logon32LogonInteractive() As Integer
		Get
			Return 2
		End Get
	End Property

	''' <summary>
	''' Represents the securty impersionation level of 2
	''' </summary>
	Public ReadOnly Property SecurityImpersonation() As Integer
		Get
			Return 2
		End Get
	End Property

	''' <summary>
	''' Performs logon by envoking the LogonUser function from advapi32.dll
	''' </summary>
	''' <param name="lpszUsername">The user name or acount</param>
	''' <param name="lpszDomain">The domain the user name or acount is found in.</param>
	''' <param name="lpszPassword">The password for the user name or account.</param>
	''' <param name="dwLogonType">The logon type</param>
	''' <param name="dwLogonProvider">The logon type</param>
	''' <param name="phToken">A platform-specific that is used to represent a pointer to a handle</param>
	''' <returns></returns>
	Public Declare Auto Function LogonUser Lib "advapi32.dll" ( _
	 ByVal lpszUsername As [String], _
	 ByVal lpszDomain As [String], ByVal lpszPassword As [String], _
	 ByVal dwLogonType As Integer, ByVal dwLogonProvider As Integer, _
	 ByRef phToken As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

	' ''' <summary>
	' ''' Formats system error message
	' ''' </summary>
	' ''' <param name="dwFlags">int</param>
	' ''' <param name="lpSource">IntPtr</param>
	' ''' <param name="dwMessageId">int</param>
	' ''' <param name="dwLanguageId">int</param>
	' ''' <param name="lpBuffer">string as reference</param>
	' ''' <param name="nSize">int</param>
	' ''' <param name="Arguments">IntPtr as reference</param>
	' ''' <returns></returns>
	'<DllImport("kernel32.dll")> _
	'Public Function FormatMessage( _
	' ByVal dwFlags As Integer, _
	' ByRef lpSource As IntPtr, _
	' ByVal dwMessageId As Integer, ByVal dwLanguageId As Integer, ByRef lpBuffer As [String], _
	' ByVal nSize As Integer, ByRef Arguments As IntPtr) As Integer
	'End Function

	Public Declare Auto Function CloseHandle Lib "kernel32.dll" (ByVal handle As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

	Public Declare Auto Function DuplicateToken Lib "advapi32.dll" (ByVal existingTokenHandle As IntPtr, ByVal securityImpersonationLevel As Integer, ByRef duplicateTokenHandle As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

	Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Long)

	' ''' <summary>
	' '''     GetErrorMessage formats and returns an error message
	' '''     corresponding to the input errorCode.
	' ''' </summary>
	' ''' <param name="errorCode">int</param>
	' ''' <returns>string</returns>
	'Public Function GetErrorMessage(ByVal errorCode As Integer) As String
	'	Dim FormatMessageAllocateBuffer As Integer = &H100
	'	Dim FormatMessageIgnoreInserts As Integer = &H200
	'	Dim FormatMessageFromSystem As Integer = &H1000

	'	Dim messageSize As Integer = 255
	'	Dim lpMsgBuf As String = String.Empty
	'	Dim dwFlags As Integer = FormatMessageAllocateBuffer Or FormatMessageFromSystem Or FormatMessageIgnoreInserts

	'	Dim ptrlpSource As IntPtr = IntPtr.Zero
	'	Dim prtArguments As IntPtr = IntPtr.Zero

	'	Dim retVal As Integer = FormatMessage(dwFlags, ptrlpSource, errorCode, 0, lpMsgBuf, _
	'	 messageSize, prtArguments)
	'	If 0 = retVal Then
	'		Throw New Exception("Failed to format message for error code " + errorCode.ToString(CultureInfo.CurrentCulture) + ". ")
	'	End If

	'	Return lpMsgBuf
	'End Function
End Module

Imports System
Imports System.Runtime.InteropServices
Imports System.Security.Principal
Imports System.Security.Permissions
#Region " Notes "
' The WebImpersonate class performs a windows impersonate.
' Generally used by the FileManager class to access
' file structures on other servers than the IIS
#End Region
<Assembly: SecurityPermissionAttribute(SecurityAction.RequestMinimum, UnmanagedCode:=True), _
 Assembly: PermissionSetAttribute(SecurityAction.RequestMinimum, Name:="FullTrust")> 
Public Class WebImpersonate
	Public ReadOnly Property LOGON32_PROVIDER_DEFAULT() As Integer
		Get
			Return 0
		End Get
	End Property

	Public ReadOnly Property LOGON32_LOGON_INTERACTIVE() As Integer
		Get
			Return 2
		End Get
	End Property

	Public ReadOnly Property SecurityImpersonation() As Integer
		Get
			Return 2
		End Get
	End Property

	Public Declare Auto Function LogonUser Lib "advapi32.dll" ( _
	 ByVal lpszUsername As [String], _
	 ByVal lpszDomain As [String], ByVal lpszPassword As [String], _
	 ByVal dwLogonType As Integer, ByVal dwLogonProvider As Integer, _
	 ByRef phToken As IntPtr) As Boolean

	<DllImport("kernel32.dll")> _
	Public Shared Function FormatMessage( _
	 ByVal dwFlags As Integer, _
	 ByRef lpSource As IntPtr, _
	 ByVal dwMessageId As Integer, ByVal dwLanguageId As Integer, ByRef lpBuffer As [String], _
	 ByVal nSize As Integer, ByRef Arguments As IntPtr) As Integer

	End Function

	Public Declare Auto Function CloseHandle Lib "kernel32.dll" (ByVal handle As IntPtr) As Boolean

	Public Declare Auto Function DuplicateToken Lib "advapi32.dll" ( _
	 ByVal ExistingTokenHandle As IntPtr, _
	 ByVal SECURITY_IMPERSONATION_LEVEL As Integer, _
	 ByRef DuplicateTokenHandle As IntPtr) As Boolean

	'GetErrorMessage formats and returns an error message
	'corresponding to the input errorCode.
	Public Shared Function GetErrorMessage(ByVal errorCode As Integer) As String
		Dim FORMAT_MESSAGE_ALLOCATE_BUFFER As Integer = &H100
		Dim FORMAT_MESSAGE_IGNORE_INSERTS As Integer = &H200
		Dim FORMAT_MESSAGE_FROM_SYSTEM As Integer = &H1000

		Dim messageSize As Integer = 255
		Dim lpMsgBuf As String
		Dim dwFlags As Integer = FORMAT_MESSAGE_ALLOCATE_BUFFER Or FORMAT_MESSAGE_FROM_SYSTEM Or FORMAT_MESSAGE_IGNORE_INSERTS

		Dim ptrlpSource As IntPtr = IntPtr.Zero
		Dim prtArguments As IntPtr = IntPtr.Zero

		Dim retVal As Integer = FormatMessage(dwFlags, ptrlpSource, errorCode, 0, lpMsgBuf, _
		 messageSize, prtArguments)
		If 0 = retVal Then
			Throw New Exception("Failed to format message for error code " + errorCode.ToString() + ". ")
		End If

		Return lpMsgBuf
	End Function	'GetErrorMessage
End Class
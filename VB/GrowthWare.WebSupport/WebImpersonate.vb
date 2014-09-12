Imports System.Runtime.InteropServices
Imports System.Security.Principal
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.Framework.Common

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
            Dim log As Logger = Logger.Instance
            Dim mExceptionMsg As String = "Could not impersonate user: " & userName & " for domain " & domainName
            Dim mException As WebImpersonateException = New WebImpersonateException(mExceptionMsg)
            log.Fatal(mException)
            Throw mException
        End If
        Dim retVal As Boolean = NativeMethods.DuplicateToken(tokenHandle, NativeMethods.SecurityImpersonation, dupeTokenHandle)
        If Not retVal Then
            NativeMethods.CloseHandle(tokenHandle)
            Dim mLog As Logger = Logger.Instance()
            Dim mExceptionMsg As String = "Exception thrown when trying to duplicate token." + System.Environment.NewLine + "Account: " + account + System.Environment.NewLine + "Domain: " + domainName
            Dim mException As WebImpersonateException = New WebImpersonateException(mExceptionMsg)
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

    Public Declare Auto Function CloseHandle Lib "kernel32.dll" (ByVal handle As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

    Public Declare Auto Function DuplicateToken Lib "advapi32.dll" (ByVal existingTokenHandle As IntPtr, ByVal securityImpersonationLevel As Integer, ByRef duplicateTokenHandle As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

    'Public Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Integer)
End Module

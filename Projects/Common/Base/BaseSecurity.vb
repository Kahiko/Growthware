Imports System.Collections
Imports System.Collections.Specialized
Imports System.Diagnostics
Imports System.DirectoryServices
Imports System.Security.Cryptography
Imports System.Text
Imports System.IO
Imports System.Web

Namespace Security
    Public Class BaseSecurity
        Private _filterAttribute As String

        Public Sub New()

        End Sub

        Public Class LDAPUtility
            Private retVal As Boolean = False
            Private _path As String
            Private _filterAttribute As String

            Public Sub New()

            End Sub

            Public Function IsAuthenticated(ByVal path As String, ByVal Domain As String, ByVal Account As String, ByVal pwd As String) As Boolean
                Dim domainAndUsername As String = Domain & "\" & Account
                domainAndUsername = domainAndUsername.Trim
                Dim retVal As Boolean = False
                Dim entry As DirectoryEntry = New DirectoryEntry(path, domainAndUsername, pwd)
                Dim obj As Object
                Try
                    'Bind to the native AdsObject to force authentication
                    'if this does not work it will throw an exception.
                    obj = entry.NativeObject
                    retVal = True
                Catch ex As Exception
                    Throw New Exception("Error authenticating account. " & ex.Message, ex)
                Finally
                    If Not obj Is Nothing Then obj = Nothing
                    If Not entry Is Nothing Then entry.Dispose()
                End Try
                Return retVal
            End Function

            Public Function SetPasswordLdap(ByVal path As String, ByVal Domain As String, ByVal Account As String, ByVal oldPWD As String, ByVal newPWD As String) As Boolean
                Dim domainAndUsername As String = String.Empty
                Dim search As DirectorySearcher
                Dim results As SearchResultCollection
                Dim result As SearchResult
                Dim obj As Object
                path &= ":636"
                Dim retVal As Boolean = False
                If Domain.Trim.Length = 0 Then
                    domainAndUsername = Account
                Else
                    domainAndUsername = Domain & "\" & Account
                End If
                Dim entry As DirectoryEntry = New DirectoryEntry(path, domainAndUsername, oldPWD)
                Try
                    'Bind to the native AdsObject to force authentication
                    'if this does not work it will throw an exception.
                    obj = entry.NativeObject
                    search = New DirectorySearcher(entry)
                    search.Filter = "(SAMAccountName=" & Account & ")"
                    search.PropertiesToLoad.Add("cn")
                    results = search.FindAll()    ' findone causes a memory leak
                    If (Not results Is Nothing) Then
                        ' due to the leak with find one need to go through all results
                        ' good news the SAMAccountName is unique so there is only
                        For Each result In results
                            entry = result.GetDirectoryEntry
                        Next
                        If Not entry Is Nothing Then
                            ' now that we have a direct referance to the account we can change
                            ' the password by invoking "ChangePassword"
                            entry.Invoke("ChangePassword", New Object() {oldPWD, newPWD})
                            entry.CommitChanges()
                            retVal = True
                        Else
                            Throw New ApplicationException("Unknown account or bad password.")
                        End If
                    Else
                        Throw New ApplicationException("Unknown account or bad password.")
                    End If
                Catch ex As Reflection.TargetInvocationException
                    Throw ex.InnerException
                Catch ex As Exception
                    Throw New Exception("Error authenticating account. " & ex.Message, ex)
                Finally
                    If Not obj Is Nothing Then obj = Nothing
                    If Not entry Is Nothing Then entry.Dispose()
                    If Not results Is Nothing Then results.Dispose()
                    If Not search Is Nothing Then search.Dispose()
                End Try
                Return retVal
            End Function
        End Class
        Public Class CryptoUtil
            '8 bytes randomly selected for both the Key and the Initialization Vector
            'the IV is used to encrypt the first block of text so that any repetitive 
            'patterns are not apparent
            Private Shared KEY_64() As Byte = {83, 68, 91, 37, 128, 64, 92, 197}
            Private Shared IV_64() As Byte = {6, 55, 118, 219, 92, 197, 78, 69}

            '24 byte or 192 bit key for TripleDES
            Private Shared KEY_192() As Byte = {83, 68, 91, 37, 128, 64, 92, 197, _
              87, 215, 61, 243, 148, 20, 252, 34, _
              38, 69, 83, 201, 74, 211, 6, 98}
            '24 byte or 192 bit Initialization Vector for TripleDES
            Private Shared IV_192() As Byte = {6, 55, 118, 219, 92, 197, 78, 69, _
              247, 110, 61, 189, 247, 110, 61, 189, _
              243, 148, 20, 252, 34, 133, 174, 189}

            Public Sub New()

            End Sub
            'Standard DES encryption
            Public Function Encrypt(ByVal value As String) As String
                If value <> "" Then
                    Dim cryptoProvider As DESCryptoServiceProvider = _
                     New DESCryptoServiceProvider
                    Dim ms As MemoryStream = New MemoryStream
                    Dim cs As CryptoStream = _
                     New CryptoStream(ms, cryptoProvider.CreateEncryptor(KEY_64, IV_64), _
                      CryptoStreamMode.Write)
                    Dim sw As StreamWriter = New StreamWriter(cs)
                    sw.Write(value)
                    sw.Flush()
                    cs.FlushFinalBlock()
                    ms.Flush()
                    'convert back to a string
                    Return Convert.ToBase64String(ms.GetBuffer(), 0, ms.Length)
                End If
            End Function

            'Standard DES decryption
            Public Function Decrypt(ByVal value As String) As String
                If value <> "" Then
                    Dim cryptoProvider As DESCryptoServiceProvider = _
                     New DESCryptoServiceProvider
                    'convert from string to byte array
                    Dim buffer As Byte() = Convert.FromBase64String(value)
                    Dim ms As MemoryStream = New MemoryStream(buffer)
                    Dim cs As CryptoStream = _
                     New CryptoStream(ms, cryptoProvider.CreateDecryptor(KEY_64, IV_64), _
                      CryptoStreamMode.Read)
                    Dim sr As StreamReader = New StreamReader(cs)
                    Return sr.ReadToEnd()
                End If
            End Function

            'TRIPLE DES encryption
            Public Function EncryptTripleDES(ByVal value As String) As String
                If value <> "" Then
                    Dim cryptoProvider As TripleDESCryptoServiceProvider = _
                     New TripleDESCryptoServiceProvider
                    Dim ms As MemoryStream = New MemoryStream
                    Dim cs As CryptoStream = _
                     New CryptoStream(ms, cryptoProvider.CreateEncryptor(KEY_192, IV_192), _
                      CryptoStreamMode.Write)
                    Dim sw As StreamWriter = New StreamWriter(cs)
                    sw.Write(value)
                    sw.Flush()
                    cs.FlushFinalBlock()
                    ms.Flush()
                    'convert back to a string
                    Return Convert.ToBase64String(ms.GetBuffer(), 0, ms.Length)
                End If
            End Function

            'TRIPLE DES decryption
            Public Function DecryptTripleDES(ByVal value As String) As String
                If value <> "" Then
                    Dim cryptoProvider As TripleDESCryptoServiceProvider = _
                     New TripleDESCryptoServiceProvider
                    'convert from string to byte array
                    Dim buffer As Byte() = Convert.FromBase64String(value)
                    Dim ms As MemoryStream = New MemoryStream(buffer)
                    Dim cs As CryptoStream = _
                     New CryptoStream(ms, cryptoProvider.CreateDecryptor(KEY_192, IV_192), _
                      CryptoStreamMode.Read)
                    Dim sr As StreamReader = New StreamReader(cs)
                    Return sr.ReadToEnd()
                End If
            End Function
        End Class
    End Class
End Namespace
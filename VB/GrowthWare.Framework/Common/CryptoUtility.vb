Imports System.Text.RegularExpressions
Imports GrowthWare.Framework.Model.Enumerations
Imports System.Security.Cryptography
Imports System.Text
Imports System.IO

Namespace Common
    ''' <summary>
    ''' The CryptoUtility is a utility to provide encryption/decryption based on either DES or 
    ''' triple DES methods
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CryptoUtility

#Region "Member Fields"
        ''' <summary> 
        ''' SetKeys will create a 192 bit key and 64 bit IV based on 
        ''' two MD5 methods found in another article (http://www.aspalliance.com/535) 
        ''' </summary> 
        Private Shared WriteOnly Property setKeys() As String
            Set(ByVal value As String)
                ' create the byte arrays needed to create the key and iv. 
                Dim md5key As Byte()
                Dim hashedkey As Byte()

                ' for ease of preparation, we'll utilize code found in a different 
                ' article. http://www.aspalliance.com/535 
                md5key = md5Encryption(value)
                hashedkey = md5SaltedHashEncryption(value)

                ' loop to transfer the keys. 
                For i As Integer = 0 To hashedkey.Length - 1
                    s_KEY_192(i) = hashedkey(i)
                Next

                ' create the start and mid portion of the hashed key 
                Dim startcount As Integer = hashedkey.Length
                ' always 128 
                Dim midcount As Integer = md5key.Length / 2
                ' always 64 

                ' loop to fill in the rest of the key, and create 
                ' the IV with the remaining. 
                For i As Integer = midcount To md5key.Length - 1
                    s_KEY_192(startcount + (i - midcount)) = md5key(i)
                    s_IV_192(i - midcount) = md5key(i - midcount)
                Next

                ' clean up resources. 
                md5key = Nothing
                hashedkey = Nothing
            End Set
        End Property

        '8 bytes randomly selected for both the Key and the Initialization Vector
        'the IV is used to encrypt the first block of text so that any repetitive 
        'patterns are not apparent
        Private Shared s_KEY_64() As Byte = {83, 68, 91, 37, 128, 64, 92, 197}

        Private Shared s_IV_64() As Byte = {6, 55, 118, 219, 92, 197, 78, 69}

        '24 byte or 192 bit key for TripleDES
        Private Shared s_KEY_192() As Byte = {83, 68, 91, 37, 128, 64, 92, 197,
          87, 215, 61, 243, 148, 20, 252, 34,
          38, 69, 83, 201, 74, 211, 6, 98}
        '24 byte or 192 bit Initialization Vector for TripleDES
        Private Shared s_IV_192() As Byte = {6, 55, 118, 219, 92, 197, 78, 69,
          247, 110, 61, 189, 247, 110, 61, 189,
          243, 148, 20, 252, 34, 133, 174, 189}

#End Region

#Region "Public Methods"
        ''' <summary>
        ''' Performs encryption given the desired encryption type.
        ''' </summary>
        ''' <param name="ValueToEncrypt">String to encrypt</param>
        ''' <param name="EncryptionType">If "TripleDES" is not specified the DES is returned.</param>
        ''' <returns>Encrypted string</returns>
        ''' <remarks>EncryptionType is case sensitive.</remarks>
        Public Shared Function Encrypt(ByVal valueToEncrypt As String, ByVal encryptionType As EncryptionType) As String
            Select Case encryptionType
                Case encryptionType.TripleDES
                    '24 byte or 192 bit key for TripleDES
                    Dim mKEY_192() As Byte = {83, 68, 91, 37, 128, 64, 92, 197, _
                      87, 215, 61, 243, 148, 20, 252, 34, _
                      38, 69, 83, 201, 74, 211, 6, 98}
                    '24 byte or 192 bit Initialization Vector for TripleDES
                    Dim mIV_192() As Byte = _
                     {6, 55, 118, 219, 92, 197, 78, 69, _
                      247, 110, 61, 189, 247, 110, 61, 189, _
                      243, 148, 20, 252, 34, 133, 174, 189}

                    s_KEY_192 = mKEY_192
                    s_IV_192 = mIV_192
                    Return encryptTripleDES(valueToEncrypt)
                Case encryptionType.DES
                    Return Encrypt(valueToEncrypt)
                Case Else
                    Return valueToEncrypt
            End Select
        End Function

        ''' <summary>
        ''' Performs encryption given the desired encryption type.
        ''' </summary>
        ''' <param name="ValueToEncrypt">String</param>
        ''' <param name="EncryptionType">EncryptionType</param>
        ''' <param name="SaltExpression">String</param>
        ''' <returns>Encrypted string</returns>
        ''' <remarks>EncryptionType is case sensitive.</remarks>
        Public Shared Function Encrypt(ByVal valueToEncrypt As String, ByVal encryptionType As EncryptionType, ByVal saltExpression As String) As String
            setKeys = saltExpression
            Select Case encryptionType
                Case encryptionType.TripleDES
                    Return encryptTripleDES(valueToEncrypt)
                Case encryptionType.DES
                    Return Encrypt(valueToEncrypt)
                Case Else
                    Return valueToEncrypt
            End Select
        End Function

        ''' <summary>
        ''' Performs dencryption.
        ''' </summary>
        ''' <param name="ValueToDecrypt">Encrypted string</param>
        ''' <param name="EncryptionType">If "TripleDES" is not specified the DES is returned.</param>
        ''' <returns>Decrypted string</returns>
        ''' <remarks>EncryptionType is case sensitive</remarks>
        Public Shared Function Decrypt(ByVal valueToDecrypt As String, ByVal encryptionType As EncryptionType) As String
            Select Case encryptionType
                Case encryptionType.TripleDES
                    Return decryptTripleDES(valueToDecrypt)
                Case encryptionType.DES
                    Return decrypt(valueToDecrypt)
                Case Else
                    Try
                        Return decryptTripleDES(valueToDecrypt)
                    Catch ex As CryptoUtilityException
                        ' do nothing
                    End Try
                    Try
                        Return decrypt(valueToDecrypt)
                    Catch ex As CryptoUtilityException
                        ' do nothing
                    End Try
                    Return valueToDecrypt
            End Select
        End Function

        ''' <summary>
        ''' Performs dencryption.
        ''' </summary>
        ''' <param name="ValueToDecrypt">String</param>
        ''' <param name="EncryptionType">EncryptionType</param>
        ''' <param name="SaltExpression">SaltExpression</param>
        ''' <returns>Decrypted string</returns>
        ''' <remarks></remarks>
        Public Shared Function Decrypt(ByVal valueToDecrypt As String, ByVal encryptionType As EncryptionType, ByVal saltExpression As String) As String
            setKeys = saltExpression
            Select Case encryptionType
                Case encryptionType.TripleDES
                    Return decryptTripleDES(valueToDecrypt)
                Case encryptionType.DES
                    Return decrypt(valueToDecrypt)
                Case Else
                    Try
                        Return decryptTripleDES(valueToDecrypt)
                    Catch ex As CryptoUtilityException
                        ' do nothing
                    End Try
                    Try
                        Return decrypt(valueToDecrypt)
                    Catch ex As CryptoUtilityException
                        ' do nothing
                    End Try
                    Return valueToDecrypt
            End Select
        End Function
#End Region

#Region "Private Methods"
        Private Sub New()

        End Sub

        ''' <summary>
        ''' Encrypts the string to a byte array using the MD5 Encryption Algorithm.
        ''' <see cref="System.Security.Cryptography.MD5CryptoServiceProvider"/>
        ''' </summary>
        ''' <param name="toEncrypt">System.String.  Usually a password.</param>
        ''' <returns>System.Byte[]</returns>
        Private Shared Function md5Encryption(ByVal toEncrypt As String) As Byte()
            ' Create instance of the crypto provider.
            Dim mMD5CryptoServiceProvider As MD5CryptoServiceProvider = Nothing
            Try
                mMD5CryptoServiceProvider = New MD5CryptoServiceProvider()
                ' Create a Byte array to store the encryption to return.
                Dim mHashedbytes As Byte()
                ' Required UTF8 Encoding used to encode the input value to a usable state.
                Dim mTextencoder As New UTF8Encoding()

                ' let the show begin.
                mHashedbytes = mMD5CryptoServiceProvider.ComputeHash(mTextencoder.GetBytes(toEncrypt))

                ' return the hased bytes to the calling method.
                Return mHashedbytes

            Catch ex As Exception
                Throw
            Finally
                ' Destroy objects that aren't needed.
                If Not mMD5CryptoServiceProvider Is Nothing Then
                    mMD5CryptoServiceProvider.Dispose()
                End If
            End Try

        End Function

        ''' <summary> 
        ''' Encrypts the string to a byte array using the MD5 Encryption 
        ''' Algorithm with an additional Salted Hash. 
        ''' <see cref="System.Security.Cryptography.MD5CryptoServiceProvider"/> 
        ''' </summary> 
        ''' <param name="toEncrypt">System.String. Usually a password.</param> 
        ''' <returns>System.Byte[]</returns> 
        Private Shared Function md5SaltedHashEncryption(ByVal toEncrypt As String) As Byte()
            ' Create instance of the crypto provider. 
            Dim mMD5CryptoServiceProvider As New MD5CryptoServiceProvider()
            ' Create a Byte array to store the encryption to return. 
            Dim mHashedbytes As Byte()

            ' Create a Byte array to store the salted hash. 
            Dim mSaltedhash As Byte()

            Try
                ' Required UTF8 Encoding used to encode the input value to a usable state. 
                Dim textencoder As New UTF8Encoding()

                ' let the show begin. 
                mHashedbytes = mMD5CryptoServiceProvider.ComputeHash(textencoder.GetBytes(toEncrypt))

                ' Let's add the salt. 
                toEncrypt += textencoder.GetString(mHashedbytes)
                ' Get the new byte array after adding the salt. 
                mSaltedhash = mMD5CryptoServiceProvider.ComputeHash(textencoder.GetBytes(toEncrypt))

            Catch ex As Exception
                Throw
            Finally
                ' Destroy objects that aren't needed. 
                mMD5CryptoServiceProvider.Dispose()
            End Try
            ' return the hased bytes to the calling method. 
            Return mSaltedhash
        End Function

        ''' <summary>
        ''' Private method to return DES encryption.
        ''' </summary>
        ''' <param name="unEncryptedValue">String to be encrypted</param>
        ''' <returns>Encrypted string</returns>
        ''' <remarks></remarks>
        Private Shared Function encrypt(ByVal unEncryptedValue As String) As String
            Dim retVal As String = unEncryptedValue
            If Not String.IsNullOrEmpty(unEncryptedValue) Then
                Dim mCryptoProvider As DESCryptoServiceProvider = Nothing
                Dim mMemoryStream As MemoryStream = Nothing

                Try
                    mCryptoProvider = New DESCryptoServiceProvider
                    mMemoryStream = New MemoryStream
                    Dim mCryptoStream As CryptoStream = New CryptoStream(mMemoryStream, mCryptoProvider.CreateEncryptor(s_KEY_64, s_IV_64), CryptoStreamMode.Write)
                    Dim mStreamWriter As StreamWriter = New StreamWriter(mCryptoStream)
                    mStreamWriter.Write(unEncryptedValue)
                    mStreamWriter.Flush()
                    mCryptoStream.FlushFinalBlock()
                    mMemoryStream.Flush()
                    'convert back to a string
                    retVal = Convert.ToBase64String(mMemoryStream.GetBuffer(), 0, mMemoryStream.Length)
                Catch ex As Exception
                    Throw
                Finally
                    If Not mCryptoProvider Is Nothing Then
                        mCryptoProvider.Dispose()
                    End If
                    If Not mMemoryStream Is Nothing Then
                        mMemoryStream.Dispose()
                    End If
                End Try

            End If
            Return retVal
        End Function

        ''' <summary>
        ''' Private method to perform DES decryption
        ''' </summary>
        ''' <param name="encryptedValue">DES encrypted string</param>
        ''' <returns>Decrypted DES string</returns>
        ''' <remarks></remarks>
        Private Shared Function decrypt(ByVal encryptedValue As String) As String
            Dim mRetVal As String = String.Empty

            If Not String.IsNullOrEmpty(encryptedValue) Then
                Dim mCryptoProvider As DESCryptoServiceProvider = Nothing
                Dim mMemoryStream As MemoryStream = Nothing
                Try
                    mCryptoProvider = New DESCryptoServiceProvider
                    'convert from string to byte array
                    If IsBase64String(encryptedValue) Then
                        Dim buffer As Byte() = Convert.FromBase64String(encryptedValue)
                        mMemoryStream = New MemoryStream(buffer)
                        Dim cs As CryptoStream = _
                         New CryptoStream(mMemoryStream, mCryptoProvider.CreateDecryptor(s_KEY_64, s_IV_64), _
                          CryptoStreamMode.Read)
                        Dim sr As StreamReader = New StreamReader(cs)
                        mRetVal = sr.ReadToEnd
                    Else
                        mRetVal = encryptedValue
                    End If
                Catch ex As Exception
                    Throw
                Finally
                    If Not mCryptoProvider Is Nothing Then
                        mCryptoProvider.Dispose()
                    End If
                    If Not mMemoryStream Is Nothing Then
                        mMemoryStream.Dispose()
                    End If
                End Try
            End If
            Return mRetVal
        End Function

        ''' <summary>
        ''' Private method to perform DES3 encryption
        ''' </summary>
        ''' <param name="EncryptedValue">String to be DES3 encrypted</param>
        ''' <returns>Encrypted string</returns>
        ''' <remarks></remarks>
        Private Shared Function encryptTripleDES(ByVal encryptedValue As String) As String
            Dim mRetVal As String = encryptedValue
            If Not String.IsNullOrEmpty(encryptedValue) Then
                Dim mCryptoProvider As TripleDESCryptoServiceProvider = Nothing
                Dim mMemoryStream As MemoryStream = Nothing
                Try
                    mCryptoProvider = New TripleDESCryptoServiceProvider
                    mMemoryStream = New MemoryStream
                    Dim mCryptoStream As CryptoStream = New CryptoStream(mMemoryStream, mCryptoProvider.CreateEncryptor(s_KEY_192, s_IV_192), CryptoStreamMode.Write)
                    Dim mStreamWriter As StreamWriter = New StreamWriter(mCryptoStream)
                    mStreamWriter.Write(encryptedValue)
                    mStreamWriter.Flush()
                    mCryptoStream.FlushFinalBlock()
                    mMemoryStream.Flush()
                    'convert back to a string
                    mRetVal = Convert.ToBase64String(mMemoryStream.GetBuffer(), 0, mMemoryStream.Length)
                Catch ex As Exception
                    Throw New CryptoUtilityException("error using encrypt Triple DES", ex)
                Finally
                    If Not mCryptoProvider Is Nothing Then
                        mCryptoProvider.Dispose()
                    End If
                    If Not mMemoryStream Is Nothing Then
                        mMemoryStream.Dispose()
                    End If
                End Try
            End If
            Return mRetVal
        End Function

        ''' <summary>
        ''' Private method to DES3 decryption.
        ''' </summary>
        ''' <param name="encryptedValue">DES3 encrypted string</param>
        ''' <returns>clear text string</returns>
        ''' <remarks></remarks>
        Private Shared Function decryptTripleDES(ByVal encryptedValue As String) As String
            Dim mRetVal As String = String.Empty
            If encryptedValue <> "" Then
                Dim mCryptoProvider As TripleDESCryptoServiceProvider = Nothing
                Dim mMemoryStream As MemoryStream = Nothing
                Try
                    mCryptoProvider = New TripleDESCryptoServiceProvider
                    'convert from string to byte array
                    If IsBase64String(encryptedValue) Then
                        Dim buffer As Byte() = Convert.FromBase64String(encryptedValue)
                        mMemoryStream = New MemoryStream(buffer)

                        Dim mCryptoStream As CryptoStream = New CryptoStream(mMemoryStream, mCryptoProvider.CreateDecryptor(s_KEY_192, s_IV_192), CryptoStreamMode.Read)
                        Dim mStreamReader As StreamReader = New StreamReader(mCryptoStream)
                        mRetVal = mStreamReader.ReadToEnd
                    Else
                        mRetVal = encryptedValue
                    End If
                Catch ex As Exception
                    Throw New CryptoUtilityException("error using Decrypt Triple DES", ex)
                Finally
                    If Not mCryptoProvider Is Nothing Then
                        mCryptoProvider.Dispose()
                    End If
                    If Not mMemoryStream Is Nothing Then
                        mMemoryStream.Dispose()
                    End If
                End Try

            End If
            Return mRetVal
        End Function

        ''' <summary>
        ''' Checks to see if a string value is a base64 string.  Reduces the need for try catch and exceptions.
        ''' </summary>
        ''' <param name="Value">String to be tested</param>
        ''' <returns>Boolean</returns>
        Private Shared Function IsBase64String(ByVal Value) As Boolean
            Value = Value.Trim()
            Return (Value.Length Mod 4 = 0) And Regex.IsMatch(Value, "^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None)
        End Function
#End Region
    End Class
End Namespace

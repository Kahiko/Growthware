using System.IO;
using System.Security.Cryptography;
using GrowthWare.Framework.Enumerations;
using System.Text;
using System;
using System.Text.RegularExpressions;
using System.Globalization;

namespace GrowthWare.Framework
{
	/// <summary>
	/// The CryptoUtility is a utility to provide encryption/decryption based on either DES or 
	/// triple DES methods
	/// </summary>
	/// <remarks></remarks>
    public sealed class CryptoUtility
    {
#region Member Fields
        //8 bytes randomly selected for the Key
        private static byte[] s_KEY_8_BYTE = { 83, 68, 91, 37, 128, 64, 92, 197 };

        //24 bytes randomly selected for the Key
        private static byte[] s_KEY_24_BYTE = {
            83,  68, 91,  37, 128,  64,  92, 197,
		    87, 215, 61, 243, 148,  20, 252,  34,
		    38,  69, 83, 201,  74, 211,   6,  98
        };

        //32 bytes randomly selected for the Key
        private static byte[] s_KEY_32_BYTE = { 
            156, 174, 234, 86, 123,  61,  10, 101, 
             18, 109,   4, 95, 150, 106, 152,  90, 
             55, 122, 113, 55,  14, 227, 171,  37, 
             81, 236,   2, 52, 252, 155,  43, 159 
        };
#endregion

#region Public Methods
        /// <summary>
        /// Attempts to encrypt the valueToEncrypt. A return value indicates whether the encryption succeeded.
        /// </summary>
        /// <param name="valueToEncrypt"></param>
        /// <param name="value"></param>
        /// <param name="encryptionType"></param>
        /// <param name="saltExpression"></param>
        /// <returns></returns>
        /// <remarks>value is either the encrypted value or the unchanged value of valueToEncrypt</remarks>
        public static bool TryEncrypt(string valueToEncrypt, out string value, EncryptionType encryptionType, string saltExpression)
        {
            bool mRetVal = false;
            string mOutValue = valueToEncrypt;
            try
            {
                switch (encryptionType)
                {
                    case EncryptionType.Aes:
                        mOutValue = encryptAes(valueToEncrypt);
                        break;
                    case EncryptionType.TripleDes:
                        mOutValue = encryptTripleDES(valueToEncrypt);
                        break;
                    case EncryptionType.Des:
                        mOutValue = encryptDES(valueToEncrypt);
                        break;
                    case EncryptionType.None:
                        mOutValue = valueToEncrypt;
                        break;
                    default:
                        mOutValue = encryptTripleDES(valueToEncrypt);
                        break;
                }
                mRetVal = true;
            }
            catch (System.Exception)
            {
                // do nothing
            }
            value = mOutValue;
            return mRetVal;
        }

        /// <summary>
        /// Attempts to encrypt the valueToEncrypt. A return value indicates whether the encryption succeeded.
        /// </summary>
        /// <param name="valueToEncrypt"></param>
        /// <param name="value"></param>
        /// <param name="encryptionType"></param>
        /// <returns></returns>
        /// <remarks>
        /// returns TryEncrypt(string valueToEncrypt, out string value, EncryptionType encryptionType, string saltExpression)
        /// uses ConfigSettings.EncryptionSaltExpression for the saltExpression
        /// </remarks>
        public static bool TryEncrypt(string valueToEncrypt, out string value, EncryptionType encryptionType)
        {
            return TryEncrypt(valueToEncrypt, out value, encryptionType, ConfigSettings.EncryptionSaltExpression);
        }

        /// <summary>
        /// Attempts to decrypt the valueToDecrypt. A return value indicates whether the decryption succeeded.
        /// </summary>
        /// <param name="valueToDecrypt"></param>
        /// <param name="value"></param>
        /// <param name="encryptionType"></param>
        /// <param name="saltExpression"></param>
        /// <returns></returns>
        /// <remarks>value is either the decrypted value or the unchanged value of valueToDecrypt</remarks>
        public static bool TryDecrypt(string valueToDecrypt, out string value, EncryptionType encryptionType, string saltExpression)
        {
            bool mRetVal = false;
            string mOutValue = valueToDecrypt;
            try
            {
                switch (encryptionType)
                {
                    case EncryptionType.Aes:
                        mOutValue = decryptAes(valueToDecrypt);
                        break;
                    case EncryptionType.TripleDes:
                        mOutValue = decryptTripleDES(valueToDecrypt);
                        break;
                    case EncryptionType.Des:
                        mOutValue = decryptDES(valueToDecrypt);
                        break;
                    case EncryptionType.None:
                        mOutValue = valueToDecrypt;
                        break;
                    default:
                        mOutValue = decryptTripleDES(valueToDecrypt);
                        break;
                }
                mRetVal = true;
            }
            catch (System.Exception)
            {
                // do nothing
            }
            value = mOutValue;
            return mRetVal;
        }

        /// <summary>
        /// Attempts to decrypt the valueToDecrypt. A return value indicates whether the decryption succeeded.
        /// </summary>
        /// <param name="valueToDecrypt"></param>
        /// <param name="value"></param>
        /// <param name="encryptionType"></param>
        /// <returns></returns>
        /// <remarks>
        /// returns TryDecrypt(string valueToEncrypt, out string value, EncryptionType encryptionType, string saltExpression)
        /// uses ConfigSettings.EncryptionSaltExpression for the saltExpression
        /// </remarks>
        public static bool TryDecrypt(string valueToDecrypt, out string value, EncryptionType encryptionType)
        {
            return TryDecrypt(valueToDecrypt, out value, encryptionType, ConfigSettings.EncryptionSaltExpression);
        }

#endregion

#region Private Methods
        private CryptoUtility()
        {
        }

        /// <summary> 
        /// Encrypts the string to a byte array using the SHA512 Encryption 
        /// Algorithm with an additional Salted Hash. 
        /// <see cref="System.Security.Cryptography.MD5CryptoServiceProvider"/> 
        /// </summary> 
        /// <param name="toEncrypt">System.String. Usually a password.</param> 
        /// <returns>System.Byte[]</returns> 
        private static byte[] saltedHashEncryptionSHA512(string toEncrypt)
        {
            try
            {
                // Create an instance of SHA512
                SHA512 mSHA512 = SHA512.Create();
                // Required UTF8 Encoding used to encode the input value to a usable state. 
                UTF8Encoding mTextEncoder = new UTF8Encoding();
                // Create a Byte array to store the salted hash. 
                byte[] mSaltedByteArray = null;
                // Create a Byte array to store the encryption to return. 
                byte[] mSaltedHash = null;
                // Store/use toEncrypt so we are not changing the parameters value
                string mValueToEncrypt = toEncrypt;

                // let the show begin. 
                mSaltedByteArray = mSHA512.ComputeHash(mTextEncoder.GetBytes(ConfigSettings.EncryptionSaltExpression));

                // Let's add the salt. 
                mValueToEncrypt += mTextEncoder.GetString(mSaltedByteArray);
                // Get the new byte array after adding the salt. 
                mSaltedHash = mSHA512.ComputeHash(mTextEncoder.GetBytes(mValueToEncrypt));

                // return the hased bytes to the calling method. 
                return mSaltedHash;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Private method to return DES encryption.
        /// </summary>
        /// <param name="valueToEncrypt">String to be encrypted</param>
        /// <returns>Encrypted string</returns>
        /// <remarks></remarks>
        private static string encryptDES(string valueToEncrypt)
        {
            string mRetVal = valueToEncrypt;
            if (!string.IsNullOrEmpty(valueToEncrypt))
            {
                MemoryStream mMemoryStream = null;
                try
                {
                    SymmetricAlgorithm mSymmetricAlgorithm = DES.Create();
                    byte[] mIV = mSymmetricAlgorithm.IV;
                    using (mMemoryStream = new MemoryStream()) {
                        mMemoryStream.Write(mIV, 0, mIV.Length);  // Add the IV to the first 16 bytes of the encrypted value
                        using (CryptoStream mCryptoStream = new CryptoStream(mMemoryStream, mSymmetricAlgorithm.CreateEncryptor(s_KEY_8_BYTE, mSymmetricAlgorithm.IV), CryptoStreamMode.Write)) {
                            using (StreamWriter mStreamWriter = new StreamWriter(mCryptoStream)) {
                                mStreamWriter.Write(valueToEncrypt);
                            }
                        }
                        byte[] mByteArray = mMemoryStream.ToArray();
                        mRetVal = Convert.ToBase64String(mByteArray, 0, mByteArray.Length);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (mMemoryStream != null)
                    {
                        mMemoryStream.Dispose();
                    }
                }
            }
            return mRetVal;
        }

        /// <summary>
        /// Private method to perform DES decryption
        /// </summary>
        /// <param name="encryptedValue">DES encrypted string</param>
        /// <returns>Decrypted DES string</returns>
        /// <remarks></remarks>
        private static string decryptDES(string encryptedValue)
        {
            string mRetVal = string.Empty;

            if (!string.IsNullOrEmpty(encryptedValue))
            {
                MemoryStream mMemoryStream = null;
                try
                {
                    if (isBase64String(encryptedValue))
                    {
                        SymmetricAlgorithm mSymmetricAlgorithm = DES.Create();
                        byte[] mByteArray = Convert.FromBase64String(encryptedValue);
                        using (mMemoryStream = new MemoryStream(mByteArray)) {
                            byte[] mIV = new byte[16];
                            mMemoryStream.Read(mIV, 0, mIV.Length);  // Pull the IV from the first 16 bytes of the encrypted value
                            using (var cryptStream = new CryptoStream(mMemoryStream, mSymmetricAlgorithm.CreateDecryptor(s_KEY_8_BYTE, mIV), CryptoStreamMode.Read)) {
                                using (var reader = new System.IO.StreamReader(cryptStream)) {
                                    mRetVal = reader.ReadToEnd();
                                }
                            }
                        } 
                    }
                    else
                    {
                        mRetVal = encryptedValue;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (mMemoryStream != null)
                    {
                        mMemoryStream.Dispose();
                    }
                }
            }
            return mRetVal;
        }

        private static string encryptAes(string valueToEncrypt)
        {
            string mRetVal = valueToEncrypt;
            if (!string.IsNullOrEmpty(valueToEncrypt))
            {
                MemoryStream mMemoryStream = null;
                try
                {
                    SymmetricAlgorithm mSymmetricAlgorithm = Aes.Create();
                    byte[] mIV = mSymmetricAlgorithm.IV;
                    using (mMemoryStream = new MemoryStream()) {
                        mMemoryStream.Write(mIV, 0, mIV.Length);  // Add the IV to the first 16 bytes of the encrypted value
                        using (CryptoStream mCryptoStream = new CryptoStream(mMemoryStream, mSymmetricAlgorithm.CreateEncryptor(s_KEY_32_BYTE, mSymmetricAlgorithm.IV), CryptoStreamMode.Write)) {
                            using (StreamWriter mStreamWriter = new StreamWriter(mCryptoStream)) {
                                mStreamWriter.Write(valueToEncrypt);
                            }
                        }
                        byte[] mByteArray = mMemoryStream.ToArray();
                        mRetVal = Convert.ToBase64String(mByteArray, 0, mByteArray.Length);
                    }                    
                }
                catch (Exception ex)
                {
                    throw new CryptoUtilityException("error using encrypt Triple DES", ex);
                }
                finally
                {
                    if (mMemoryStream != null)
                    {
                        mMemoryStream.Dispose();
                    }
                }
            }
            return mRetVal;
        }

        /// <summary>
        /// Private method to perform DES3 encryption
        /// </summary>
        /// <param name="valueToEncrypt">String to be DES3 encrypted</param>
        /// <returns>Encrypted string</returns>
        /// <remarks></remarks>
        private static string encryptTripleDES(string valueToEncrypt)
        {
            string mRetVal = valueToEncrypt;
            if (!string.IsNullOrEmpty(valueToEncrypt))
            {
                MemoryStream mMemoryStream = null;
                try
                {
                    SymmetricAlgorithm mSymmetricAlgorithm = TripleDES.Create();
                    byte[] mIV = mSymmetricAlgorithm.IV;
                    using (mMemoryStream = new MemoryStream()) {
                        mMemoryStream.Write(mIV, 0, mIV.Length);  // Add the IV to the first 16 bytes of the encrypted value
                        using (CryptoStream mCryptoStream = new CryptoStream(mMemoryStream, mSymmetricAlgorithm.CreateEncryptor(s_KEY_24_BYTE, mSymmetricAlgorithm.IV), CryptoStreamMode.Write)) {
                            using (StreamWriter mStreamWriter = new StreamWriter(mCryptoStream)) {
                                mStreamWriter.Write(valueToEncrypt);
                            }
                        }
                        byte[] mByteArray = mMemoryStream.ToArray();
                        mRetVal = Convert.ToBase64String(mByteArray, 0, mByteArray.Length);
                    }  
                }
                catch (Exception ex)
                {
                    throw new CryptoUtilityException("error using encrypt Triple DES", ex);
                }
                finally
                {
                    if (mMemoryStream != null)
                    {
                        mMemoryStream.Dispose();
                    }
                }
            }
            return mRetVal;
        }

        private static string decryptAes(string encryptedValue)
        {
            string mRetVal = string.Empty;
            MemoryStream mMemoryStream = null;
            if (!string.IsNullOrEmpty(encryptedValue))
            {
                try
                {
                    if (isBase64String(encryptedValue))
                    {
                        SymmetricAlgorithm mSymmetricAlgorithm = Aes.Create();
                        byte[] mByteArray = Convert.FromBase64String(encryptedValue);
                        using (mMemoryStream = new MemoryStream(mByteArray)) {
                            byte[] mIV = new byte[16];
                            mMemoryStream.Read(mIV, 0, mIV.Length);  // Pull the IV from the first 16 bytes of the encrypted value
                            using (var cryptStream = new CryptoStream(mMemoryStream, mSymmetricAlgorithm.CreateDecryptor(s_KEY_32_BYTE, mIV), CryptoStreamMode.Read)) {
                                using (var reader = new System.IO.StreamReader(cryptStream)) {
                                    mRetVal = reader.ReadToEnd();
                                }
                            }
                        } 
                    }
                    else
                    {
                        mRetVal = encryptedValue;
                    }

                }
                catch (Exception ex)
                {
                    throw new CryptoUtilityException("error using Decrypt Triple DES", ex);
                }
                finally
                {
                    if (mMemoryStream != null)
                    {
                        mMemoryStream.Dispose();
                    }
                }

            }
            return mRetVal;
        }

        /// <summary>
        /// Private method to DES3 decryption.
        /// </summary>
        /// <param name="encryptedValue">DES3 encrypted string</param>
        /// <returns>clear text string</returns>
        /// <remarks></remarks>
        private static string decryptTripleDES(string encryptedValue)
        {
            string mRetVal = string.Empty;
            MemoryStream mMemoryStream = null;
            if (!string.IsNullOrEmpty(encryptedValue))
            {
                try
                {
                    if (isBase64String(encryptedValue))
                    {
                        SymmetricAlgorithm mSymmetricAlgorithm = TripleDES.Create();
                        byte[] mByteArray = Convert.FromBase64String(encryptedValue);
                        using (mMemoryStream = new MemoryStream(mByteArray)) {
                            byte[] mIV = new byte[16];
                            mMemoryStream.Read(mIV, 0, mIV.Length);  // Pull the IV from the first 16 bytes of the encrypted value
                            using (var cryptStream = new CryptoStream(mMemoryStream, mSymmetricAlgorithm.CreateDecryptor(s_KEY_24_BYTE, mIV), CryptoStreamMode.Read)) {
                                using (var reader = new System.IO.StreamReader(cryptStream)) {
                                    mRetVal = reader.ReadToEnd();
                                }
                            }
                        } 
                    }
                    else
                    {
                        mRetVal = encryptedValue;
                    }

                }
                catch (Exception ex)
                {
                    throw new CryptoUtilityException("error using Decrypt Triple DES", ex);
                }
                finally
                {
                    if (mMemoryStream != null)
                    {
                        mMemoryStream.Dispose();
                    }
                }

            }
            return mRetVal;
        }

        /// <summary>
        /// Checks to see if a string value is a base64 string.  Reduces the need for try catch and exceptions.
        /// </summary>
        /// <param name="value">String to be tested</param>
        /// <returns>Boolean</returns>
        private static bool isBase64String(string value)
        {
            value = value.Trim();
            return (value.Length % 4 == 0) && Regex.IsMatch(value, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

        }
#endregion
    }
}

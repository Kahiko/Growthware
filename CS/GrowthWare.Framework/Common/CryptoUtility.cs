﻿using System.IO;
using System.Security.Cryptography;
using GrowthWare.Framework.Model.Enumerations;
using System.Text;
using System;
using System.Text.RegularExpressions;
using System.Globalization;

namespace GrowthWare.Framework.Common
{
	/// <summary>
	/// The CryptoUtility is a utility to provide encryption/decryption based on either DES or 
	/// triple DES methods
	/// </summary>
	/// <remarks></remarks>
    public sealed class CryptoUtility
    {
        #region Member Fields
        /// <summary> 
        /// SetKeys will create a 192 bit key and 64 bit IV based on 
        /// two MD5 methods found in another article (http://www.aspalliance.com/535) 
        /// </summary> 
        private static string setKeys
        {
            set
            {
                // create the byte arrays needed to create the key and iv. 
                byte[] md5key = null;
                byte[] hashedkey = null;

                // for ease of preparation, we'll utilize code found in a different 
                // article. http://www.aspalliance.com/535 
                md5key = MD5Encryption(value);
                hashedkey = MD5SaltedHashEncryption(value);

                // loop to transfer the keys. 
                for (int i = 0; i <= hashedkey.Length - 1; i++)
                {
                    s_KEY_192[i] = hashedkey[i];
                }

                // create the start and mid portion of the hashed key 
                int startcount = hashedkey.Length;
                // always 128 
                int midcount = md5key.Length / 2;
                // always 64 

                // loop to fill in the rest of the key, and create 
                // the IV with the remaining. 
                for (int i = midcount; i <= md5key.Length - 1; i++)
                {
                    s_KEY_192[startcount + (i - midcount)] = md5key[i];
                    s_IV_192[i - midcount] = md5key[i - midcount];
                }

                // clean up resources. 
                md5key = null;
                hashedkey = null;
            }
        }

        //8 bytes randomly selected for both the Key and the Initialization Vector
        //the IV is used to encrypt the first block of text so that any repetitive 
        //patterns are not apparent
        private static byte[] s_KEY_64 = { 83, 68, 91, 37, 128, 64, 92, 197 };

        private static byte[] s_IV_64 = { 6, 55, 118, 219, 92, 197, 78, 69 };
        //24 byte or 192 bit key for TripleDES
        private static byte[] s_KEY_192 = {83, 68, 91, 37, 128, 64, 92, 197,
		  87, 215, 61, 243, 148, 20, 252, 34,
		  38, 69, 83, 201, 74, 211, 6, 98};
        //24 byte or 192 bit Initialization Vector for TripleDES
        private static byte[] s_IV_192 = {6, 55, 118, 219, 92, 197, 78, 69,
		  247, 110, 61, 189, 247, 110, 61, 189,
		  243, 148, 20, 252, 34, 133, 174, 189};
        #endregion

        #region Public Methods
        /// <summary>
        /// Performs encryption given the desired encryption type.
        /// </summary>
        /// <param name="valueToEncrypt">String to encrypt</param>
        /// <param name="encryptionType">If "TripleDES" is not specified the DES is returned.</param>
        /// <returns>Encrypted string</returns>
        /// <remarks>encryptionType is case sensitive.</remarks>
        public static string Encrypt(string valueToEncrypt, EncryptionType encryptionType)
        {
            return Encrypt(valueToEncrypt, encryptionType, ConfigSettings.EncryptionSaltExpression);
        }

        /// <summary>
        /// Performs encryption given the desired encryption type.
        /// </summary>
        /// <param name="valueToEncrypt">String</param>
        /// <param name="encryptionType">EncryptionType</param>
        /// <param name="saltExpression">String</param>
        /// <returns>Encrypted string</returns>
        /// <remarks>EncryptionType is case sensitive.</remarks>
        public static string Encrypt(string valueToEncrypt, EncryptionType encryptionType, string saltExpression)
        {
            setKeys = saltExpression;
            switch (encryptionType)
            {
                case EncryptionType.TripleDes:
                    return encryptTripleDES(valueToEncrypt);
                case EncryptionType.Des:
                    return encrypt(valueToEncrypt);
                default:
                    return valueToEncrypt;
            }
        }

        /// <summary>
        /// Performs dencryption.
        /// </summary>
        /// <param name="valueToDecrypt">Encrypted string</param>
        /// <param name="encryptionType">If "TripleDES" is not specified the DES is returned.</param>
        /// <returns>Decrypted string</returns>
        /// <remarks></remarks>
        public static string Decrypt(string valueToDecrypt, EncryptionType encryptionType)
        {
            return Decrypt(valueToDecrypt, encryptionType, ConfigSettings.EncryptionSaltExpression);
        }

        /// <summary>
        /// Performs dencryption.
        /// </summary>
        /// <param name="valueToDecrypt">String</param>
        /// <param name="encryptionType">EncryptionType</param>
        /// <param name="saltExpression">SaltExpression</param>
        /// <returns>Decrypted string</returns>
        /// <remarks></remarks>
        public static string Decrypt(string valueToDecrypt, EncryptionType encryptionType, string saltExpression)
        {
            setKeys = saltExpression;
            switch (encryptionType)
            {
                case EncryptionType.TripleDes:
                    return decryptTripleDES(valueToDecrypt);
                case EncryptionType.Des:
                    return decrypt(valueToDecrypt);
                default:
                    try
                    {
                        return decryptTripleDES(valueToDecrypt);
                    }
                    catch (CryptoUtilityException)
                    {
                        // do nothing
                    }

                    try
                    {
                        return decrypt(valueToDecrypt);
                    }
                    catch (CryptoUtilityException)
                    {
                        // do nothing
                    }

                    return valueToDecrypt;
            }
        }
        #endregion

        #region Private Methods
        private CryptoUtility()
        {
        }

        /// <summary>
        /// Encrypts the string to a byte array using the MD5 Encryption Algorithm.
        /// <see cref="System.Security.Cryptography.MD5CryptoServiceProvider"/>
        /// </summary>
        /// <param name="toEncrypt">System.String.  Usually a password.</param>
        /// <returns>System.Byte[]</returns>
        private static byte[] MD5Encryption(string toEncrypt)
        {
            // Create instance of the crypto provider. 
            MD5CryptoServiceProvider mMD5CryptoServiceProvider = null;
            try
            {
                mMD5CryptoServiceProvider = new MD5CryptoServiceProvider();
                // Create a Byte array to store the encryption to return. 
                byte[] mRetBytes = null;

                // Required UTF8 Encoding used to encode the input value to a usable state. 
                UTF8Encoding mTextEncoder = new UTF8Encoding();

                // let the show begin. 
                mRetBytes = mMD5CryptoServiceProvider.ComputeHash(mTextEncoder.GetBytes(toEncrypt));

                // return the hased bytes to the calling method. 
                return mRetBytes;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (mMD5CryptoServiceProvider != null)
                {
                    mMD5CryptoServiceProvider.Dispose();
                }
            }
        }

        /// <summary> 
        /// Encrypts the string to a byte array using the MD5 Encryption 
        /// Algorithm with an additional Salted Hash. 
        /// <see cref="System.Security.Cryptography.MD5CryptoServiceProvider"/> 
        /// </summary> 
        /// <param name="toEncrypt">System.String. Usually a password.</param> 
        /// <returns>System.Byte[]</returns> 
        private static byte[] MD5SaltedHashEncryption(string toEncrypt)
        {
            // Create instance of the crypto provider. 
            MD5CryptoServiceProvider mMD5CryptoServiceProvider = null;
            try
            {
                mMD5CryptoServiceProvider = new MD5CryptoServiceProvider();
                // Create a Byte array to store the encryption to return. 
                byte[] mRetBytes = null;
                // Create a Byte array to store the salted hash. 
                byte[] mSaltedHash = null;

                // Required UTF8 Encoding used to encode the input value to a usable state. 
                UTF8Encoding mTextEncoder = new UTF8Encoding();

                // let the show begin. 
                mRetBytes = mMD5CryptoServiceProvider.ComputeHash(mTextEncoder.GetBytes(toEncrypt));

                // Let's add the salt. 
                toEncrypt += mTextEncoder.GetString(mRetBytes);
                // Get the new byte array after adding the salt. 
                mSaltedHash = mMD5CryptoServiceProvider.ComputeHash(mTextEncoder.GetBytes(toEncrypt));

                // return the hased bytes to the calling method. 
                return mSaltedHash;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (mMD5CryptoServiceProvider != null)
                {
                    mMD5CryptoServiceProvider.Dispose();
                }
            }
        }

        /// <summary>
        /// Private method to return DES encryption.
        /// </summary>
        /// <param name="valueToEncrypt">String to be encrypted</param>
        /// <returns>Encrypted string</returns>
        /// <remarks></remarks>
        private static string encrypt(string valueToEncrypt)
        {
            string mRetVal = valueToEncrypt;
            if (!string.IsNullOrEmpty(valueToEncrypt))
            {
                DESCryptoServiceProvider mCryptoProvider = null;
                MemoryStream mMemoryStream = null;
                try
                {
                    mCryptoProvider = new DESCryptoServiceProvider();
                    mMemoryStream = new MemoryStream();
                    CryptoStream mCryptoStream = new CryptoStream(mMemoryStream, mCryptoProvider.CreateEncryptor(s_KEY_64, s_IV_64), CryptoStreamMode.Write);
                    StreamWriter mStreamWriter = new StreamWriter(mCryptoStream);
                    mStreamWriter.Write(valueToEncrypt);
                    mStreamWriter.Flush();
                    mCryptoStream.FlushFinalBlock();
                    mMemoryStream.Flush();
                    //convert back to a string
                    mRetVal = Convert.ToBase64String(mMemoryStream.GetBuffer(), 0, int.Parse(mMemoryStream.Length.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture));
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (mCryptoProvider != null)
                    {
                        mCryptoProvider.Dispose();
                    }
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
        /// <param name="EncryptedValue">DES encrypted string</param>
        /// <returns>Decrypted DES string</returns>
        /// <remarks></remarks>
        private static string decrypt(string EncryptedValue)
        {
            string mRetVal = string.Empty;

            if (!string.IsNullOrEmpty(EncryptedValue))
            {
                DESCryptoServiceProvider mCryptoProvider = null;
                MemoryStream mMemoryStream = null;
                try
                {
                    if (isBase64String(EncryptedValue))
                    {
                        mCryptoProvider = new DESCryptoServiceProvider();
                        //convert from string to byte array
                        byte[] buffer = Convert.FromBase64String(EncryptedValue);
                        mMemoryStream = new MemoryStream(buffer);
                        CryptoStream cs = new CryptoStream(mMemoryStream, mCryptoProvider.CreateDecryptor(s_KEY_64, s_IV_64), CryptoStreamMode.Read);
                        StreamReader sr = new StreamReader(cs);
                        mRetVal = sr.ReadToEnd();
                    }
                    else
                    {
                        mRetVal = EncryptedValue;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (mCryptoProvider != null)
                    {
                        mCryptoProvider.Dispose();
                    }
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
            string retVal = valueToEncrypt;
            if (!string.IsNullOrEmpty(valueToEncrypt))
            {
                TripleDESCryptoServiceProvider mCryptoProvider = null;
                MemoryStream mMemoryStream = null;
                try
                {
                    mCryptoProvider = new TripleDESCryptoServiceProvider();
                    mMemoryStream = new MemoryStream();
                    CryptoStream mCryptoStream = new CryptoStream(mMemoryStream, mCryptoProvider.CreateEncryptor(s_KEY_192, s_IV_192), CryptoStreamMode.Write);
                    StreamWriter mStreamWriter = new StreamWriter(mCryptoStream);
                    mStreamWriter.Write(valueToEncrypt);
                    mStreamWriter.Flush();
                    mCryptoStream.FlushFinalBlock();
                    mMemoryStream.Flush();
                    //convert back to a string
                    retVal = Convert.ToBase64String(mMemoryStream.GetBuffer(), 0, int.Parse(mMemoryStream.Length.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture));
                }
                catch (Exception ex)
                {
                    throw new CryptoUtilityException("error using encrypt Triple DES", ex);
                }
                finally
                {
                    if (mCryptoProvider != null)
                    {
                        mCryptoProvider.Dispose();
                    }
                    if (mMemoryStream != null)
                    {
                        mMemoryStream.Dispose();
                    }
                }
            }
            return retVal;
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
            TripleDESCryptoServiceProvider mCryptoProvider = null;
            MemoryStream mMemoryStream = null;
            if (!string.IsNullOrEmpty(encryptedValue))
            {
                try
                {
                    mCryptoProvider = new TripleDESCryptoServiceProvider();
                    if (isBase64String(encryptedValue))
                    {
                        //convert from string to byte array
                        byte[] buffer = Convert.FromBase64String(encryptedValue);
                        mMemoryStream = new MemoryStream(buffer);
                        CryptoStream mCryptoStream = new CryptoStream(mMemoryStream, mCryptoProvider.CreateDecryptor(s_KEY_192, s_IV_192), CryptoStreamMode.Read);
                        StreamReader mStreamReader = new StreamReader(mCryptoStream);
                        mRetVal = mStreamReader.ReadToEnd();
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
                    if (mCryptoProvider != null)
                    {
                        mCryptoProvider.Dispose();
                    }
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

using GrowthWare.Framework.Common;
using System;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace GrowthWare.WebSupport
{
    /// <summary>
    /// Use to initiate a web impersonation
    /// </summary>
    public sealed class WebImpersonate
    {

        /// <summary>
        /// Begins the impersonation process
        /// </summary>
        /// <param name="account">String including domain ie domain\useraccount</param>
        /// <param name="password">String</param>
        /// <returns>WindowsImpersonationContext</returns>
        /// <remarks></remarks>
        public static WindowsImpersonationContext ImpersonateNow(string account, string password)
        {
            if (account == null || password == null) throw new ArgumentNullException("account", "Account or password can not be null reference (Nothing in Visual Basic)");
            IntPtr tokenHandle = new IntPtr(0);
            IntPtr dupeTokenHandle = new IntPtr(0);
            //WebImpersonate myImpersonate = new WebImpersonate();

            string userName = string.Empty;
            string domainName = string.Empty;
            int posSlash = account.IndexOf("\\", StringComparison.CurrentCultureIgnoreCase);
            if (posSlash != 0)
            {
                userName = account;
                domainName = account;
                userName = userName.Remove(0, posSlash);
                domainName = domainName.Remove((posSlash - 1), (account.Length - posSlash) + 1);
            }
            else
            {
                userName = account;
            }
            bool returnValue = NativeMethods.LogonUser(userName, domainName, password, NativeMethods.Logon32LogonInteractive, NativeMethods.Long32ProviderDefault, ref tokenHandle);
            if (!returnValue)
            {
                Logger mLog = Logger.Instance();
                string mExceptionMsg = "Could not impersonate user: " + userName + " for domain " + domainName;
                WebImpersonateException mException = new WebImpersonateException(mExceptionMsg);
                mLog.Error(mException);
                throw mException;
            }
            bool retVal = NativeMethods.DuplicateToken(tokenHandle, NativeMethods.SecurityImpersonation, ref dupeTokenHandle);
            if (!retVal)
            {
                NativeMethods.CloseHandle(tokenHandle);
                Logger mLog = Logger.Instance();
                string mExceptionMsg = "Exception thrown when trying to duplicate token." + Environment.NewLine + "Account: " + account + Environment.NewLine + "Domain: " + domainName;
                WebImpersonateException mException = new WebImpersonateException(mExceptionMsg);
                mLog.Error(mException);
                throw mException;
            }
            // The token that is passed to the following constructor must 
            // be a primary token in order to use it for impersonation.
            WindowsIdentity newId = new WindowsIdentity(dupeTokenHandle);
            WindowsImpersonationContext impersonatedUser = null;
            try
            {
                impersonatedUser = newId.Impersonate();
            }
            catch (Exception)
            {
                throw;
            }
            finally 
            {
                newId.Dispose();
            }
            return impersonatedUser;
        }
    }

    static class NativeMethods
    {
        /// <summary>
        /// Represents a 32 bit logon int for the defaut provider
        /// </summary>
        public static int Long32ProviderDefault
        {
            get { return 0; }
        }

        /// <summary>
        /// Represents a 32 bit logon int for interactive
        /// </summary>
        public static int Logon32LogonInteractive
        {
            get { return 2; }
        }

        /// <summary>
        /// Represents the securty impersionation level of 2
        /// </summary>
        public static int SecurityImpersonation
        {
            get { return 2; }
        }

        /// <summary>
        /// Performs logon by envoking the LogonUser function from advapi32.dll
        /// </summary>
        /// <param name="lpszUsername">The user name or acount</param>
        /// <param name="lpszDomain">The domain the user name or acount is found in.</param>
        /// <param name="lpszPassword">The password for the user name or account.</param>
        /// <param name="dwLogonType">The logon type</param>
        /// <param name="dwLogonProvider">The logon type</param>
        /// <param name="phToken">A platform-specific that is used to represent a pointer to a handle</param>
        /// <returns></returns>
        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        ///// <summary>
        ///// Formats system error message
        ///// </summary>
        ///// <param name="dwFlags">int</param>
        ///// <param name="lpSource">IntPtr</param>
        ///// <param name="dwMessageId">int</param>
        ///// <param name="dwLanguageId">int</param>
        ///// <param name="lpBuffer">string as reference</param>
        ///// <param name="nSize">int</param>
        ///// <param name="Arguments">IntPtr as reference</param>
        ///// <returns></returns>
        //[DllImport("kernel32.dll")]
        //public static extern int FormatMessage(int dwFlags, ref IntPtr lpSource, int dwMessageId, int dwLanguageId, ref String lpBuffer, int nSize, ref IntPtr Arguments);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DuplicateToken(IntPtr existingTokenHandle, int SECURITY_IMPERSONATION_LEVEL, ref IntPtr duplicateTokenHandle);

        ///// <summary>
        /////     GetErrorMessage formats and returns an error message
        /////     corresponding to the input errorCode.
        ///// </summary>
        ///// <param name="errorCode">int</param>
        ///// <returns>string</returns>
        //public static string GetErrorMessage(int errorCode)
        //{
        //    int FormatMessageAllocateBuffer = 0x100;
        //    int FormatMessageIgnoreInserts = 0x200;
        //    int FormatMessageFromSystem = 0x1000;

        //    int messageSize = 255;
        //    string lpMsgBuf = string.Empty;
        //    int dwFlags = FormatMessageAllocateBuffer | FormatMessageFromSystem | FormatMessageIgnoreInserts;

        //    IntPtr ptrlpSource = IntPtr.Zero;
        //    IntPtr prtArguments = IntPtr.Zero;

        //    int retVal = FormatMessage(dwFlags, ref ptrlpSource, errorCode, 0, ref lpMsgBuf, messageSize, ref prtArguments);
        //    if (0 == retVal)
        //    {
        //        throw new Exception("Failed to format message for error code " + errorCode.ToString(CultureInfo.CurrentCulture) + ". ");
        //    }

        //    return lpMsgBuf;
        //}
    }
}

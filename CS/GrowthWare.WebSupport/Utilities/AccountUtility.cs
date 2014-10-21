﻿using GrowthWare.Framework.BusinessData.BusinessLogicLayer;
using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Enumerations;
using GrowthWare.Framework.Model.Profiles;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.DirectoryServices;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace GrowthWare.WebSupport.Utilities
{
    /// <summary>
    /// AccountUtility serves as the focal point for any web application needing to utilize the GrowthWare framework.
    /// Web needs such as caching are handled here
    /// </summary>
    public static class AccountUtility
    {
        private static String s_CachedAnonymousAccount = "AnonymousProfile";
        private static String s_AnonymousAccount = "Anonymous";
        /// <summary>
        /// The anonymous account profile
        /// </summary>
        public static readonly String AnonymousAccountProfile = s_CachedAnonymousAccount;

        /// <summary>
        /// Performs authentication give an account and password
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns>Boolean</returns>
        /// <remarks>
        /// Handles authentication methodology
        /// </remarks>
        public static Boolean Authenticated(String account, String password)
        {
            if (string.IsNullOrEmpty(account)) throw new ArgumentNullException("account", "account cannot be a null reference (Nothing in Visual Basic)! (Nothing in VB) or empty!");
            if (string.IsNullOrEmpty(account)) throw new ArgumentNullException("password", "password cannot be a null reference (Nothing in Visual Basic)! (Nothing in VB) or empty!");
            bool retVal = false;
            bool mDomainPassed = false;
            if (account.Contains(@"\"))
            {
                mDomainPassed = true;
            }
            MAccountProfile mAccountProfile = GetProfile(account);
            if (mDomainPassed && mAccountProfile == null)
            {
                int mDomainPos = account.IndexOf(@"\", StringComparison.OrdinalIgnoreCase);
                account = account.Substring(mDomainPos + 1, account.Length - mDomainPos - 1);
                mAccountProfile = GetProfile(account);
            }
            if (mAccountProfile != null)
            {
                if (ConfigSettings.AuthenticationType.ToUpper(CultureInfo.InvariantCulture) == "INTERNAL")
                {
                    string profilePassword = string.Empty;
                    if ((mAccountProfile != null))
                    {
                        try
                        {
                            profilePassword = CryptoUtility.Decrypt(mAccountProfile.Password, SecurityEntityUtility.CurrentProfile().EncryptionType);
                        }
                        catch (CryptoUtilityException)
                        {
                            profilePassword = mAccountProfile.Password;
                        }
                        if (password == profilePassword)
                        {
                            retVal = true;
                        }
                    }
                }
                else // LDAP authentication
                {
                    string domainAndUsername = ConfigSettings.LdapDomain + "\\" + account;
                    if (mDomainPassed) domainAndUsername = account;
                    domainAndUsername = domainAndUsername.Trim();
                    DirectoryEntry entry = null;
                    object obj = new object();
                    try
                    {
                        entry = new DirectoryEntry(ConfigSettings.LdapServer, domainAndUsername, password);
                        //Bind to the native AdsObject to force authentication
                        //if this does not work it will throw an exception.
                        obj = entry.NativeObject;
                        retVal = true;
                    }
                    catch (Exception ex)
                    {
                        string mMessage = "Error Authenticating account " + domainAndUsername + " through LDAP.";
                        WebSupportException mEx = new WebSupportException(mMessage,ex);
                        Logger mLog = Logger.Instance();
                        mLog.Error(mEx);
                        throw mEx;
                    }
                    finally
                    {
                        if ((obj != null)) obj = null;
                        if ((entry != null)) entry.Dispose();
                    }
                }

            }
            return retVal;
        }

        /// <summary>
        /// Deletes the specified account seq id.
        /// </summary>
        /// <param name="accountSeqId">The account seq id.</param>
        public static void Delete(int accountSeqId)
        {
            BAccounts mBAccount = new BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            mBAccount.Delete(accountSeqId);
        }

        /// <summary>
        /// Retruns a collection of MAccountProfiles given an MAccountProfile and the current SecurityEntitySeqID
        /// </summary>
        /// <param name="profile">MAccountProfile</param>
        /// <remarks>If the Profiles.IsSysAdmin is true then all accounts will be returned</remarks>
        public static Collection<MAccountProfile> GetAccounts(MAccountProfile profile)
        {
            // was thinking of adding cache here but
            // when you think of it it's not needed
            // account information needs to come from
            // the db to help ensure passwords are correct and what not.
            // besides which a list of accounts is only necessary
            // when editing an account and it that case
            // what accounts that are returned are dependend on the requesting account.IsSysAdmin bit.
            BAccounts mBAccount = new BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            return mBAccount.GetAccounts(profile);
        }

        /// <summary>
        /// Retrieves the current profile.
        /// </summary>
        /// <returns>MAccountProfile</returns>
        /// <remarks>If context does not contain a referance to an account anonymous will be returned</remarks>
        public static MAccountProfile CurrentProfile()
        {
            Logger mLog = Logger.Instance();
            mLog.Debug("AccountUtility::GetCurrentProfile() Started");
            MAccountProfile mRetProfile = null;
            String mAccountName = HttpContext.Current.User.Identity.Name;
            if (string.IsNullOrEmpty(mAccountName)) mAccountName = s_AnonymousAccount;
            if (mAccountName.Trim() == s_AnonymousAccount)
            {
                if (HttpContext.Current.Cache != null)
                {
                    mRetProfile = (MAccountProfile)(HttpContext.Current.Cache[s_CachedAnonymousAccount]);
                    if (mRetProfile == null)
                    {
                        mRetProfile = GetProfile(mAccountName);
                        CacheController.AddToCacheDependency(s_CachedAnonymousAccount, mRetProfile);
                    }
                }
                else
                {
                    mLog.Debug("AccountUtility::GetCurrentProfile() No cache available");
                }
            }
            if (mRetProfile == null)
            {
                mRetProfile = (MAccountProfile)HttpContext.Current.Cache[mAccountName + "_Session"];
                if (mRetProfile == null)
                {
                    mRetProfile = GetProfile(mAccountName);
                    if (mRetProfile != null)
                    {
                        HttpContext.Current.Cache[mAccountName + "_Session"] = mRetProfile;
                    }
                    else
                    {
                        mLog.Debug("AccountUtility::GetCurrentProfile() No cache available");
                    }
                }
            }
            mLog.Debug("AccountUtility::GetCurrentProfile() Done");
            return mRetProfile;
        }

        /// <summary>
        /// Gets the name of the HTTP context user.
        /// </summary>
        /// <returns>String.</returns>
        public static String HttpContextUserName()
        {
            String mRetVal = "Anonymous";
            if (HttpContext.Current != null && HttpContext.Current.User != null && HttpContext.Current.User.Identity != null)
            {
                if (HttpContext.Current.User.Identity.Name.Length > 0)
                {
                    if (!ConfigSettings.StripDomainFromHttpContextUserName())
                    {
                        mRetVal = HttpContext.Current.User.Identity.Name;
                    }
                    else
                    {
                        int mPos = HttpContext.Current.User.Identity.Name.IndexOf(@"\", StringComparison.OrdinalIgnoreCase);
                        mRetVal = HttpContext.Current.User.Identity.Name.Substring(mPos, HttpContext.Current.User.Identity.Name.Length - mPos);
                    }

                }
            }
            return mRetVal;
        }

        /// <summary>
        /// Gets the menu.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="menuType">Type of the menu.</param>
        /// <returns>DataTable.</returns>
        public static DataTable GetMenu(String account, MenuType menuType)
        {
            if (string.IsNullOrEmpty(account)) throw new ArgumentNullException("account", "account cannot be a null reference (Nothing in Visual Basic)! (Nothing in VB) or empty!");
            BAccounts mBAccount = new BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            DataTable mRetVal = null;
            if (account.ToUpper(CultureInfo.InvariantCulture) == "ANONYMOUS")
            {
                String mAnonMenu = menuType.ToString() + "Anonymous";
                mRetVal = (DataTable)HttpContext.Current.Cache[mAnonMenu];
                if (mRetVal == null)
                {
                    mRetVal = mBAccount.GetMenu(account, menuType);
                    CacheController.AddToCacheDependency(mAnonMenu, mRetVal);
                }
            }
            else
            {
                mRetVal = mBAccount.GetMenu(account, menuType);
            }
            return mRetVal;
        }

        /// <summary>
        /// Retrieves an account profile given the account
        /// </summary>
        /// <param name="account">String</param>
        /// <returns>a populated MAccountProfile</returns>
        public static MAccountProfile GetProfile(String account)
        {
            BAccounts mBAccount = new BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            MAccountProfile mRetVal = null;
            try
            {
                mRetVal = mBAccount.GetProfile(account);
            }
            catch (IndexOutOfRangeException)
            {
                String mMSG = "Count not find account: " + account + " in the database";
                Logger mLog = Logger.Instance();
                mLog.Error(mMSG);
            }
            return mRetVal;
        }

        /// <summary>
        /// Get a single account given it's id.
        /// </summary>
        /// <param name="id">int or Integer</param>
        /// <returns>MAccountProfile</returns>
        /// <remarks>Returns null object if not found</remarks>
        public static MAccountProfile GetProfile(int id)
        {
            var mResult = from mProfile in GetAccounts(CurrentProfile()) where mProfile.Id == id select mProfile;
            MAccountProfile mRetVal = null;
            try
            {
                mRetVal = mResult.First();
            }
            catch (InvalidOperationException ex)
            {
                Logger mLog = Logger.Instance();
                mLog.Error(ex);
                mRetVal = null;
            }
            if (mRetVal != null)
            {
                BAccounts mBAccount = new BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
                mRetVal = mBAccount.GetProfile(mRetVal.Account);
            }
            return mRetVal;
        }

        /// <summary>
        /// Performs all logoff function
        /// </summary>
        /// <remarks></remarks>
        public static void Logoff()
        {
            RemoveInMemoryInformation(true);
            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// Removes any session or cache information about the account
        /// </summary>
        /// <param name="removeWorkflow"></param>
        /// <remarks></remarks>
        public static void RemoveInMemoryInformation(Boolean removeWorkflow)
        {
            String mAccountName = HttpContext.Current.User.Identity.Name;
            HttpContext.Current.Session.Remove(mAccountName + "_Session");
            HttpContext.Current.Cache.Remove(mAccountName + "_Session");
            HttpContext.Current.Cache.Remove(MClientChoices.SessionName);
            if (removeWorkflow) 
            { 
            
            }
        }

        /// <summary>
        /// Sets the current profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public static void SetCurrentProfile(MAccountProfile profile)
        {
            HttpContext.Current.Items.Add("CurrentAccount", profile);
        }

        /// <summary>
        /// Sets the principal by either retrieving roles from the db or by cookie
        /// </summary>
        /// <param name="accountProfile"></param>
        /// <remarks></remarks>
        public static void SetPrincipal(MAccountProfile accountProfile)
        {
            if (accountProfile == null) throw new ArgumentNullException("accountProfile", "accountProfile cannot be a null reference (Nothing in Visual Basic)!!");
            HttpContext mCurrentConext = HttpContext.Current;
            String mAccountRoles = accountProfile.AssignedRoles.ToString().Replace(",", ";");
            // generate authentication ticket
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, accountProfile.Account, DateTime.Now, DateTime.Now.AddHours(1), false, mAccountRoles);
            // Encrypt the ticket.
            String encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            // Create a cookie and add the encrypted ticket to the cookie
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            mCurrentConext.Response.Cookies.Add(authCookie);
            String[] mRoles = new String[accountProfile.DerivedRoles.Count];
            int i = 0;
            foreach (string item in accountProfile.DerivedRoles)
            {
                mRoles[i] = item;
                i++;
            }
            mCurrentConext.User = new GenericPrincipal(mCurrentConext.User.Identity, mRoles);
        }

        /// <summary>
        /// Searches the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>DataTable.</returns>
        public static DataTable Search(MSearchCriteria searchCriteria)
        {
            BAccounts mBAccount = new BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            return mBAccount.Search(searchCriteria);
        }

        /// <summary>
        /// Inserts or updates account information
        /// </summary>
        /// <param name="profile">MAccountProfile</param>
        /// <param name="saveRoles">Boolean</param>
        /// <param name="saveGroups">Boolean</param>
        /// <remarks>Changes will be reflected in the profile passed as a reference.</remarks>
        public static MAccountProfile Save(MAccountProfile profile, bool saveRoles, bool saveGroups)
        {
            if (profile == null) throw new ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)! (Nothing in VB) or empty!");
            BAccounts mBAccount = new BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            mBAccount.Save(profile, saveRoles, saveGroups);
            if (profile.Id == CurrentProfile().Id)
            {
                RemoveInMemoryInformation(true);
            }
            return profile;
        }
    }
}

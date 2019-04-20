using GrowthWare.Framework.BusinessData.BusinessLogicLayer;
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
            if (string.IsNullOrEmpty(account)) throw new ArgumentNullException("account", "account cannot be a null reference (Nothing in VB) or empty!");
            if (string.IsNullOrEmpty(account)) throw new ArgumentNullException("password", "password cannot be a null reference (Nothing in VB) or empty!");
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
                        if (password == profilePassword && (mAccountProfile.Status != Convert.ToInt32(SystemStatus.Disabled, CultureInfo.InvariantCulture) || mAccountProfile.Status != Convert.ToInt32(SystemStatus.Inactive, CultureInfo.InvariantCulture)))
                        {
                            retVal = true;
                        }
                        if (!retVal) mAccountProfile.FailedAttempts += 1;
                        if (mAccountProfile.FailedAttempts == Convert.ToInt32(ConfigSettings.FailedAttempts) && Convert.ToInt32(ConfigSettings.FailedAttempts, CultureInfo.InvariantCulture) != -1) 
                        {
                            mAccountProfile.Status = Convert.ToInt32(SystemStatus.Disabled, CultureInfo.InvariantCulture);
                        }
                        AccountUtility.Save(mAccountProfile, false, false);
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
                        mAccountProfile.LastLogOn = DateTime.Now;
                        AccountUtility.Save(mAccountProfile, false, false);
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
        /// AutoCreateAccount will automatically create an account based on infomration found both in the web.config file
        /// and the database.
        /// </summary>
        /// <returns>MAccountProfile</returns>
        public static MAccountProfile AutoCreateAccount()
        {
            MAccountProfile mCurrentAccountProfile = AccountUtility.GetProfile("System");
            MAccountProfile mAccountProfileToSave = new MAccountProfile();
            Logger mLog = Logger.Instance();
            mAccountProfileToSave.Id = -1;
            bool mSaveGroups = true;
            bool mSaveRoles = true;
            string mGroups = ConfigSettings.RegistrationGroups;
            string mRoles = ConfigSettings.RegistrationRoles;
            if (string.IsNullOrEmpty(mGroups))
                mSaveGroups = false;
            if (string.IsNullOrEmpty(mRoles))
                mSaveRoles = false;
            mAccountProfileToSave.Account = AccountUtility.HttpContextUserName();
            mAccountProfileToSave.FirstName = "Auto created";
            mAccountProfileToSave.MiddleName = "";
            mAccountProfileToSave.LastName = "Auto created";
            mAccountProfileToSave.PreferredName = "Auto created";
            mAccountProfileToSave.Email = "change@me.com";
            mAccountProfileToSave.Location = "Hawaii";
            mAccountProfileToSave.TimeZone = -8;
            mAccountProfileToSave.AddedBy = mCurrentAccountProfile.Id;
            mAccountProfileToSave.AddedDate = DateTime.Now;
            mAccountProfileToSave.SetGroups(mGroups);
            mAccountProfileToSave.SetRoles(mRoles);
            mAccountProfileToSave.PasswordLastSet = DateTime.Now;
            mAccountProfileToSave.LastLogOn = DateTime.Now;
            mAccountProfileToSave.Password = CryptoUtility.Encrypt(ConfigSettings.RegistrationPassword, ConfigSettings.EncryptionType);
            mAccountProfileToSave.Status = (int)SystemStatus.SetAccountDetails;
            MClientChoicesState mClientChoiceState = ClientChoicesUtility.GetClientChoicesState(ConfigSettings.RegistrationAccountChoicesAccount, true);
            MSecurityEntityProfile mSecurityEntityProfile = SecurityEntityUtility.GetProfile(ConfigSettings.RegistrationSecurityEntityId);

            mClientChoiceState.IsDirty = false;
            mClientChoiceState[MClientChoices.AccountName] = mAccountProfileToSave.Account;
            mClientChoiceState[MClientChoices.SecurityEntityId] = mSecurityEntityProfile.Id.ToString(CultureInfo.InvariantCulture);
            mClientChoiceState[MClientChoices.SecurityEntityName] = mSecurityEntityProfile.Name;
            try
            {
                AccountUtility.Save(mAccountProfileToSave, mSaveRoles, mSaveGroups, mSecurityEntityProfile);
                ClientChoicesUtility.Save(mClientChoiceState, false);
                AccountUtility.SetPrincipal(mAccountProfileToSave);
            }
            catch (Exception ex)
            {
                mLog.Error(ex);
                throw;
            }
            return mAccountProfileToSave;
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
            String mAccountName = HttpContextUserName();
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
                if (HttpContext.Current.Session != null)
                {
                    mRetProfile = (MAccountProfile)HttpContext.Current.Session[mAccountName + "_Session"];
                    if (mRetProfile == null)
                    {
                        mRetProfile = GetProfile(mAccountName);
                        if (mRetProfile != null)
                        {
                            HttpContext.Current.Session[mAccountName + "_Session"] = mRetProfile;
                        }
                    }
                }
                else 
                {
                    mLog.Debug("AccountUtility::GetCurrentProfile() No Session available");
                    mRetProfile = GetProfile(mAccountName);
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
            else
            {
                if (HttpContext.Current != null)
                {
                    HttpCookie mCookie = HttpContext.Current.Request.Cookies.Get(".ASPXAUTH");
                    if (mCookie != null)
                    {
                        FormsAuthenticationTicket mAuthTicket = FormsAuthentication.Decrypt(mCookie.Value);
                        if (mAuthTicket != null)
                        {
                            GenericIdentity mIdentity = new GenericIdentity(mAuthTicket.Name);
                            if (mIdentity != null)
                            {
                                mRetVal = mIdentity.Name;
                            }
                        }
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
            if (string.IsNullOrEmpty(account)) throw new ArgumentNullException("account", "account cannot be a null reference (Nothing in VB) or empty!");
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
                String mMenuName = account + "_" + menuType.ToString() + "_MenuData";
                if (HttpContext.Current.Session != null) 
                {
                    mRetVal = (DataTable)HttpContext.Current.Session[mMenuName];
                    if (mRetVal == null)
                    {
                        mRetVal = mBAccount.GetMenu(account, menuType);
                        foreach (DataRow item in mRetVal.Rows)
                        {
                            item["URL"] = "?Action=" + item["URL"].ToString();
                        }
                        HttpContext.Current.Session[mMenuName] = mRetVal;
                    }
                }
                else 
                {
                    mRetVal = mBAccount.GetMenu(account, menuType);
                    foreach (DataRow item in mRetVal.Rows)
                    {
                        item["URL"] = "?Action=" + item["URL"].ToString();
                    }
                }
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
            catch (InvalidOperationException)
            {
                String mMSG = "Count not find account: " + account + " in the database";
                Logger mLog = Logger.Instance();
                mLog.Error(mMSG);
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
            catch (InvalidOperationException)
            {
                String mMSG = "Count not find account: " + id + " in the database";
                Logger mLog = Logger.Instance();
                mLog.Error(mMSG);
            }
            catch (IndexOutOfRangeException)
            {
                String mMSG = "Count not find account: " + id + " in the database";
                Logger mLog = Logger.Instance();
                mLog.Error(mMSG);
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
        public static void LogOff()
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
            if (HttpContext.Current.Session != null) 
            {
                HttpContext.Current.Session.Clear();
                if (removeWorkflow)
                {

                }
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
        /// <param name="securityEntityProfile">MSecurityEntityProfile</param>
        /// <remarks>Changes will be reflected in the profile passed as a reference.</remarks>
        public static MAccountProfile Save(MAccountProfile profile, bool saveRoles, bool saveGroups, MSecurityEntityProfile securityEntityProfile)
        {
            if (profile == null) throw new ArgumentNullException("profile", "profile cannot be a null reference (Nothing in VB) or empty!");
            if (securityEntityProfile == null) throw new ArgumentNullException("securityEntityProfile", "securityEntityProfile cannot be a null reference (Nothing in VB) or empty!");
            BAccounts mBAccount = new BAccounts(securityEntityProfile, ConfigSettings.CentralManagement);
            mBAccount.Save(profile, saveRoles, saveGroups);
            if (profile.Id == CurrentProfile().Id)
            {
                RemoveInMemoryInformation(true);
            }
            return profile;
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
            MSecurityEntityProfile mSecurityEntityProfile = SecurityEntityUtility.CurrentProfile();
            return Save(profile, saveRoles, saveGroups, mSecurityEntityProfile);
        }
    }
}

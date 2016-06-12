Imports GrowthWare.Framework.Model.Profiles
Imports System.Web
Imports System.Collections.ObjectModel
Imports GrowthWare.Framework.BusinessData.BusinessLogicLayer
Imports GrowthWare.Framework.Common
Imports System.Security.Principal
Imports System.Web.Security
Imports System.Globalization
Imports GrowthWare.Framework.Model.Enumerations
Imports System.DirectoryServices
Imports System.Security.Claims

Namespace Utilities
    Public Class AccountUtility
        Private Shared s_CachedAnonymousAccount As String = "AnonymousProfile"
        Private Shared s_AnonymousAccount As String = "Anonymous"

        ''' <summary>
        ''' The name of the anonymous account profile
        ''' </summary>
        Public Shared ReadOnly AnonymousAccountProfile As String = s_CachedAnonymousAccount

        ''' <summary>
        ''' Performs authentication give an account and password
        ''' </summary>
        ''' <param name="account"></param>
        ''' <param name="password"></param>
        ''' <returns>Boolean</returns>
        ''' <remarks>
        ''' Handles authentication methodology
        ''' </remarks>
        Public Shared Function Authenticated(ByVal account As String, ByVal password As String) As Boolean
            If String.IsNullOrEmpty(account) Then Throw New ArgumentNullException("account", "account can not be null (Nothing in Visual Basic) or empty!")
            If String.IsNullOrEmpty(password) Then Throw New ArgumentNullException("password", "password can not be null (Nothing in Visual Basic) or empty!")
            Dim retVal As Boolean = False
            Dim mySEProfile As MSecurityEntityProfile = SecurityEntityUtility.CurrentProfile
            Dim mDomainPassed As Boolean = False
            If account.Contains("\") Then
                mDomainPassed = True
            End If
            Dim mAccountProfile = GetProfile(account)
            If mDomainPassed And mAccountProfile Is Nothing Then
                Dim mDomainPos As Integer = account.IndexOf("\", StringComparison.OrdinalIgnoreCase)
                account = account.Substring(mDomainPos + 1, account.Length - mDomainPos - 1)
                mAccountProfile = GetProfile(account)
            End If
            If Not mAccountProfile Is Nothing Then
                If ConfigSettings.AuthenticationType.ToUpper(CultureInfo.InvariantCulture) = "INTERNAL" Then
                    Dim profilePassword As String = String.Empty
                    If Not mAccountProfile Is Nothing Then
                        Try
                            profilePassword = CryptoUtility.Decrypt(mAccountProfile.Password, mySEProfile.EncryptionType)
                        Catch ex As CryptoUtilityException
                            profilePassword = mAccountProfile.Password
                        End Try
                        If password = profilePassword And Not (mAccountProfile.Status = Convert.ToInt32(SystemStatus.Disabled, CultureInfo.InvariantCulture) Or mAccountProfile.Status = Convert.ToInt32(SystemStatus.Inactive, CultureInfo.InvariantCulture)) Then
                            retVal = True
                        End If
                        If Not retVal Then mAccountProfile.FailedAttempts += 1
                        If mAccountProfile.FailedAttempts >= Convert.ToInt32(ConfigSettings.FailedAttempts, CultureInfo.InvariantCulture) And Not Convert.ToInt32(ConfigSettings.FailedAttempts, CultureInfo.InvariantCulture) = -1 Then
                            mAccountProfile.Status = Convert.ToInt32(SystemStatus.Disabled, CultureInfo.InvariantCulture)
                        End If
                        AccountUtility.Save(mAccountProfile, False, False)
                    Else
                        Dim mLog As Logger = Logger.Instance
                        mLog.Error("Invalid account.")
                    End If
                Else ' LDAP authentication
                    Dim domainAndUsername As String = ConfigSettings.LdapDomain + "\" + account
                    If mDomainPassed Then domainAndUsername = account
                    domainAndUsername = domainAndUsername.Trim
                    Dim entry As DirectoryEntry = Nothing
                    Dim obj As New Object
                    Try
                        entry = New DirectoryEntry(ConfigSettings.LdapServer, domainAndUsername, password)
                        'Bind to the native AdsObject to force authentication
                        'if this does not work it will throw an exception.
                        obj = entry.NativeObject
                        retVal = True
                    Catch ex As Exception
                        Dim mMessage As String = "Error Authenticating account " + domainAndUsername + " through LDAP."
                        Dim mEx As WebSupportException = New WebSupportException(mMessage, ex)
                        Dim mLogger As Logger = Logger.Instance()
                        mLogger.Error(mEx)
                        Throw
                    Finally
                        If Not obj Is Nothing Then obj = Nothing
                        If Not entry Is Nothing Then entry.Dispose()
                    End Try
                End If
            End If
            Return retVal
        End Function

        ''' <summary>
        ''' AutoCreateAccount will automatically create an account based on infomration found both in the web.config file
        ''' and the database.
        ''' </summary>
        ''' <returns>MAccountProfile</returns>
        Public Shared Function AutoCreateAccount() As MAccountProfile
            Dim mCurrentAccountProfile As MAccountProfile = AccountUtility.GetProfile("System")
            Dim mAccountProfileToSave As New MAccountProfile()
            Dim mLog As Logger = Logger.Instance()
            mAccountProfileToSave.Id = -1
            Dim mSaveGroups As Boolean = True
            Dim mSaveRoles As Boolean = True
            Dim mGroups As String = ConfigSettings.RegistrationGroups
            Dim mRoles As String = ConfigSettings.RegistrationRoles
            If String.IsNullOrEmpty(mGroups) Then
                mSaveGroups = False
            End If
            If String.IsNullOrEmpty(mRoles) Then
                mSaveRoles = False
            End If
            mAccountProfileToSave.Account = AccountUtility.HttpContextUserName()
            mAccountProfileToSave.FirstName = "Auto created"
            mAccountProfileToSave.MiddleName = ""
            mAccountProfileToSave.LastName = "Auto created"
            mAccountProfileToSave.PreferredName = "Auto created"
            mAccountProfileToSave.Email = "change@me.com"
            mAccountProfileToSave.Location = "Hawaii"
            mAccountProfileToSave.TimeZone = -8
            mAccountProfileToSave.AddedBy = mCurrentAccountProfile.Id
            mAccountProfileToSave.AddedDate = DateTime.Now
            mAccountProfileToSave.SetGroups(mGroups)
            mAccountProfileToSave.SetRoles(mRoles)
            mAccountProfileToSave.PasswordLastSet = DateTime.Now
            mAccountProfileToSave.LastLogOn = DateTime.Now
            mAccountProfileToSave.Password = CryptoUtility.Encrypt(ConfigSettings.RegistrationPassword, ConfigSettings.EncryptionType)
            mAccountProfileToSave.Status = CInt(SystemStatus.SetAccountDetails)
            Dim mClientChoiceState As MClientChoicesState = ClientChoicesUtility.GetClientChoicesState(ConfigSettings.RegistrationAccountChoicesAccount, True)
            Dim mSecurityEntityProfile As MSecurityEntityProfile = SecurityEntityUtility.GetProfile(ConfigSettings.RegistrationSecurityEntityId)

            mClientChoiceState.IsDirty = False
            mClientChoiceState(MClientChoices.AccountName) = mAccountProfileToSave.Account
            mClientChoiceState(MClientChoices.SecurityEntityId) = mSecurityEntityProfile.Id.ToString(CultureInfo.InvariantCulture)
            mClientChoiceState(MClientChoices.SecurityEntityName) = mSecurityEntityProfile.Name
            Try
                AccountUtility.Save(mAccountProfileToSave, mSaveRoles, mSaveGroups, mSecurityEntityProfile)
                ClientChoicesUtility.Save(mClientChoiceState, False)
                AccountUtility.SetPrincipal(mAccountProfileToSave)
            Catch ex As Exception
                mLog.Error(ex)
                Throw
            End Try
            Return mAccountProfileToSave
        End Function

        ''' <summary>
        ''' Deletes the specified account seq id.
        ''' </summary>
        ''' <param name="accountSeqId">The account seq id.</param>
        Public Shared Sub Delete(ByVal accountSeqId As Integer)
            Dim mBAccount As BAccounts = New BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement)
            mBAccount.Delete(accountSeqId)
        End Sub

        ''' <summary>
        ''' Reruns a collection of MAccountProfiles given an MAccountProfile and the current SecurityEntitySeqID
        ''' </summary>
        ''' <param name="profile">MAccountProfile</param>
        ''' <remarks>If the Profiles.IsSysAdmin is true then all accounts will be returned</remarks>
        Public Shared Function GetAccounts(ByVal profile As MAccountProfile) As Collection(Of MAccountProfile)
            ' was thinking of adding cache here but
            ' when you think of it it's not needed
            ' account information needs to come from
            ' the db to help ensure passwords are correct and what not.
            ' besides which a list of accounts is only necessary
            ' when editing an account and it that case
            ' what accounts that are returned are depended on the requesting account.IsSysAdmin bit.
            Dim mBAccount As BAccounts = New BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement)
            Return mBAccount.GetAccounts(profile)
        End Function

        ''' <summary>
        ''' Retrieves the current profile.
        ''' </summary>
        ''' <returns>MAccountProfile</returns>
        ''' <remarks>If context does not contain a reference to an account anonymous will be returned</remarks>
        Public Shared Function CurrentProfile() As MAccountProfile
            Dim mLog As Logger = Logger.Instance()
            mLog.Debug("AccountUtility::GetCurrentProfile() Started")
            Dim mRetProfile As MAccountProfile = Nothing
            Dim mAccountName As String = HttpContext.Current.User.Identity.Name
            If String.IsNullOrEmpty(mAccountName) Then mAccountName = s_AnonymousAccount
            If mAccountName.Trim() = s_AnonymousAccount Then
                If Not HttpContext.Current.Cache Is Nothing Then
                    mRetProfile = CType(HttpContext.Current.Cache(s_CachedAnonymousAccount), MAccountProfile)
                    If mRetProfile Is Nothing Then
                        mRetProfile = GetProfile(mAccountName)
                        CacheController.AddToCacheDependency(s_CachedAnonymousAccount, mRetProfile)
                    End If
                Else
                    mLog.Debug("AccountUtility::GetCurrentProfile() No cache available")
                End If
            End If
            If mRetProfile Is Nothing Then
                If Not HttpContext.Current.Session Is Nothing Then
                    mRetProfile = CType(HttpContext.Current.Session(mAccountName + "_Session"), MAccountProfile)
                    If mRetProfile Is Nothing Then
                        mRetProfile = GetProfile(mAccountName)
                        If Not mRetProfile Is Nothing Then
                            HttpContext.Current.Session(mAccountName + "_Session") = mRetProfile
                        End If
                    End If
                Else
                    mLog.Debug("AccountUtility::GetCurrentProfile() No Session available")
                    mRetProfile = GetProfile(mAccountName)
                End If
            End If
            mLog.Debug("AccountUtility::GetCurrentProfile() Done")
            Return mRetProfile
        End Function

        ''' <summary>
        ''' Gets the name of the HTTP context user.
        ''' </summary>
        ''' <returns>System.String.</returns>
        Public Shared Function HttpContextUserName() As String
            Dim mRetVal As String = "Anonymous"
            If Not HttpContext.Current Is Nothing AndAlso Not HttpContext.Current.User Is Nothing AndAlso Not HttpContext.Current.User.Identity Is Nothing Then
                If HttpContext.Current.User.Identity.Name.Length > 0 Then
                    If Not ConfigSettings.StripDomainFromHttpContextUserName Then
                        mRetVal = HttpContext.Current.User.Identity.Name
                    Else
                        Dim mPos As Integer = HttpContext.Current.User.Identity.Name.IndexOf("\", StringComparison.OrdinalIgnoreCase)
                        mRetVal = HttpContext.Current.User.Identity.Name.Substring(mPos, HttpContext.Current.User.Identity.Name.Length - mPos)
                    End If
                End If
            End If
            Return mRetVal
        End Function

        ''' <summary>
        ''' Gets the menu information for the desired menu type.
        ''' </summary>
        ''' <param name="account">String</param>
        ''' <param name="menuType">MenuType</param>
        ''' <returns>DataTable</returns>
        ''' <remarks></remarks>
        Public Shared Function GetMenu(ByVal account As String, ByVal menuType As MenuType) As DataTable
            If String.IsNullOrEmpty(account) Then Throw New ArgumentNullException("account", "account cannot be a null reference (Nothing in Visual Basic)!")
            Dim mBAccount As BAccounts = New BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement)
            Dim mRetVal As DataTable = Nothing
            If account.ToUpper(CultureInfo.InvariantCulture) = "ANONYMOUS" Then
                Dim mAnonMenu As String = menuType.ToString() + "Anonymous"
                mRetVal = CType(HttpContext.Current.Cache(mAnonMenu), DataTable)
                If mRetVal Is Nothing Then
                    mRetVal = mBAccount.GetMenu(account, menuType)
                    For Each item As DataRow In mRetVal.Rows
                        item("URL") = "?Action=" + item("URL").ToString()
                    Next
                    CacheController.AddToCacheDependency(mAnonMenu, mRetVal)
                End If
            Else
                Dim mMenuName As String = account + "_" + menuType.ToString() + "_MenuData"
                If Not HttpContext.Current.Session Is Nothing Then
                    mRetVal = CType(HttpContext.Current.Session(mMenuName), DataTable)
                    If mRetVal Is Nothing Then mRetVal = mBAccount.GetMenu(account, menuType)
                    HttpContext.Current.Session(mMenuName) = mRetVal
                Else
                    mRetVal = mBAccount.GetMenu(account, menuType)
                    For Each item As DataRow In mRetVal.Rows
                        item("URL") = "?Action=" + item("URL").ToString()
                    Next
                End If
            End If
            Return mRetVal
        End Function

        ''' <summary>
        ''' Retrieves an account profile given the account
        ''' </summary>
        ''' <param name="account">String</param>
        ''' <returns>a populated MAccountProfile</returns>
        Public Shared Function GetProfile(ByVal account As String) As MAccountProfile
            Dim mRetVal As MAccountProfile = Nothing
            Try
                Dim mBAccount As BAccounts = New BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement)
                mRetVal = mBAccount.GetProfile(account)
            Catch ex As InvalidOperationException
                Dim mMSG As String = "Count not find account: " + account + " in the database"
                Dim mLog As Logger = Logger.Instance
                mLog.Error(mMSG)
            Catch ex As IndexOutOfRangeException
                Dim mMSG As String = "Count not find account: " + account + " in the database"
                Dim mLog As Logger = Logger.Instance
                mLog.Error(mMSG)
            End Try
            Return mRetVal
        End Function

        ''' <summary>
        ''' Retrieves an account profile given the account
        ''' </summary>
        ''' <param name="accountSeqId">Integer</param>
        ''' <returns>a populated MAccountProfile</returns>
        Public Shared Function GetProfile(ByVal accountSeqId As Integer) As MAccountProfile
            Dim mResult = From mProfile In GetAccounts(CurrentProfile()) Where mProfile.Id = accountSeqId Select mProfile
            Dim mRetVal As MAccountProfile = New MAccountProfile()
            Try
                mRetVal = mResult.First
            Catch ex As InvalidOperationException
                Dim mLog As Logger = Logger.Instance()
                mLog.Error(ex)
                mRetVal = Nothing
            End Try
            If Not mRetVal Is Nothing Then
                Dim mBAccount As BAccounts = New BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement)
                mRetVal = mBAccount.GetProfile(mRetVal.Account)
            End If
            Return mRetVal
        End Function

        ''' <summary>
        ''' Performs all logoff function
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub LogOff()
            RemoveInMemoryInformation(True)
            FormsAuthentication.SignOut()
        End Sub

        ''' <summary>
        ''' Returns a data table of the search data
        ''' </summary>
        ''' <param name="searchCriteria">MSearchCriteria</param>
        ''' <returns>NULL/Nothing if no records are returned.</returns>
        ''' <remarks></remarks>
        Public Shared Function Search(ByVal searchCriteria As MSearchCriteria) As DataTable
            Try
                Dim mBAccount As BAccounts = New BAccounts(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement)
                Return mBAccount.Search(searchCriteria)
            Catch ex As IndexOutOfRangeException
                'no data is not a problem
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Removes any session or cache information about the account
        ''' </summary>
        ''' <param name="removeWorkflow"></param>
        ''' <remarks></remarks>
        Public Shared Sub RemoveInMemoryInformation(ByVal removeWorkflow As Boolean)
            HttpContext.Current.Session.Clear()
            If removeWorkflow Then

            End If
        End Sub

        ''' <summary>
        ''' Inserts or updates account information
        ''' </summary>
        ''' <param name="profile">MAccountProfile</param>
        ''' <param name="saveRoles">Boolean</param>
        ''' <param name="saveGroups">Boolean</param>
        ''' <param name="securityEntityProfile">MSecurityEntityProfile</param>
        ''' <remarks>Changes will be reflected in the profile passed as a reference.</remarks>
        Public Shared Function Save(ByVal profile As MAccountProfile, ByVal saveRoles As Boolean, ByVal saveGroups As Boolean, ByVal securityEntityProfile As MSecurityEntityProfile)
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!")
            If securityEntityProfile Is Nothing Then Throw New ArgumentNullException("securityEntityProfile", "securityEntityProfile cannot be a null reference (Nothing in Visual Basic)!")
            Dim mBAccount As BAccounts = New BAccounts(securityEntityProfile, ConfigSettings.CentralManagement)
            mBAccount.Save(profile, saveRoles, saveGroups)
            If profile.Id = CurrentProfile().Id Then
                RemoveInMemoryInformation(True)
            End If
            Return profile
        End Function

        ''' <summary>
        ''' Inserts or updates account information
        ''' </summary>
        ''' <param name="profile">MAccountProfile</param>
        ''' <param name="saveRoles">Boolean</param>
        ''' <param name="saveGroups">Boolean</param>
        ''' <remarks>Changes will be reflected in the profile passed as a reference.</remarks>
        Public Shared Function Save(ByVal profile As MAccountProfile, ByVal saveRoles As Boolean, ByVal saveGroups As Boolean)
            If profile Is Nothing Then Throw New ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!")
            Dim mSecurityEntityProfile As MSecurityEntityProfile = SecurityEntityUtility.CurrentProfile()
            Return Save(profile, saveRoles, saveGroups, mSecurityEntityProfile)
        End Function

        ''' <summary>
        ''' Sets the principal by either retrieving roles from the db or by cookie
        ''' </summary>
        ''' <param name="accountProfile">MAccountProfile</param>
        ''' <remarks></remarks>
        Public Shared Sub SetPrincipal(ByVal accountProfile As MAccountProfile)
            If Not accountProfile Is Nothing Then
                Dim mCurrentContext As HttpContext = HttpContext.Current
                Dim mAccountRoles As String = accountProfile.AssignedRoles.ToString().Replace(",", ";")
                ' generate authentication ticket
                Dim authTicket As FormsAuthenticationTicket = New FormsAuthenticationTicket(1, accountProfile.Account, DateTime.Now, DateTime.Now.AddHours(1), False, mAccountRoles)
                Dim mFormsIdentity As FormsIdentity = New FormsIdentity(authTicket)
                ' Encrypt the ticket.
                Dim encryptedTicket As String = FormsAuthentication.Encrypt(authTicket)
                ' Create a cookie and add the encrypted ticket to the cookie
                Dim authCookie As HttpCookie = New HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
                mCurrentContext.Response.Cookies.Add(authCookie)
                Dim mGenericPrincipal As GenericPrincipal = New GenericPrincipal(mCurrentContext.User.Identity, accountProfile.DerivedRoles.ToArray)
                Dim mClaimsIdentity = New ClaimsIdentity(mFormsIdentity)
                Dim mClaimsPrincipal As ClaimsPrincipal = New ClaimsPrincipal(mClaimsIdentity)
                'mCurrentContext.User = New GenericPrincipal(mCurrentContext.User.Identity, accountProfile.DerivedRoles.ToArray)
                mCurrentContext.User = mGenericPrincipal
                'mCurrentContext.User = mClaimsPrincipal
            Else
                Throw New ArgumentNullException("accountProfile", "accountProfile cannot be a null reference (Nothing in Visual Basic)!")
            End If
        End Sub

        ''' <summary>
        ''' Sets the current profile.
        ''' </summary>
        ''' <param name="profile">The profile.</param>
        Public Shared Sub SetCurrentProfile(ByVal profile As MAccountProfile)
            HttpContext.Current.Items.Add("CurrentAccount", profile)
        End Sub
    End Class
End Namespace


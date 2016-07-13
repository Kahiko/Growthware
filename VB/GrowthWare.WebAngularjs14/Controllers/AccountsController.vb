Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Enumerations
Imports GrowthWare.Framework.Model
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport
Imports GrowthWare.WebSupport.Utilities
Imports System.Globalization
Imports System.Net
Imports System.Web.Http
Imports System.Web.UI
Imports System.Net.Http

Namespace Controllers
    Public Class AccountsController
        Inherits ApiController

        <HttpPost>
        Public Function ChangePassword(ByVal mChangePassword As MUIChangePassword) As IHttpActionResult
            If mChangePassword Is Nothing Then Throw New ArgumentNullException("mChangePassword", "mChangePassword cannot be a null reference (Nothing in Visual Basic)!")
            Dim mMessageProfile As New MMessageProfile
            Dim mSecurityEntityProfile As MSecurityEntityProfile = SecurityEntityUtility.CurrentProfile()
            Dim mAccountProfile As MAccountProfile = AccountUtility.CurrentProfile()
            Dim mCurrentPassword = ""
            mMessageProfile = MessageUtility.GetProfile("SuccessChangePassword")
            Try
                mCurrentPassword = CryptoUtility.Decrypt(mAccountProfile.Password, mSecurityEntityProfile.EncryptionType)
            Catch ex As Exception
                mCurrentPassword = mAccountProfile.Password
            End Try
            If mAccountProfile.Status <> SystemStatus.ChangePassword Then
                If mChangePassword.OldPassword <> mCurrentPassword Then
                    mMessageProfile = MessageUtility.GetProfile("PasswordNotMatched")
                Else
                    With mAccountProfile
                        .PasswordLastSet = Date.Now
                        .Status = SystemStatus.Active
                        .FailedAttempts = 0
                        .Password = CryptoUtility.Encrypt(mChangePassword.NewPassword.Trim, mSecurityEntityProfile.EncryptionType)
                    End With
                    Try
                        AccountUtility.Save(mAccountProfile, False, False)
                    Catch ex As Exception
                        mMessageProfile = MessageUtility.GetProfile("UnSuccessChangePassword")
                    End Try
                End If
            Else
                Try
                    With mAccountProfile
                        .PasswordLastSet = Date.Now
                        .Status = SystemStatus.Active
                        .FailedAttempts = 0
                        .Password = CryptoUtility.Encrypt(mChangePassword.NewPassword.Trim, mSecurityEntityProfile.EncryptionType)
                    End With
                    AccountUtility.Save(mAccountProfile, False, False)
                Catch ex As Exception
                    mMessageProfile = MessageUtility.GetProfile("UnSuccessChangePassword")
                End Try
            End If
            AccountUtility.RemoveInMemoryInformation(True)
            Return Ok(mMessageProfile.Body)
        End Function

        <HttpPost>
        Public Function Delete(<FromUri()> ByVal accountSeqId As Integer) As IHttpActionResult
            If accountSeqId < 1 Then Throw New ArgumentNullException("accountSeqId", "accountSeqId must be a positive number!")
            Dim mRetVal As String = False
            Dim mLog As Logger = Logger.Instance()
            If Not HttpContext.Current.Items("EditId") Is Nothing Then
                Dim mEditId = Integer.Parse(HttpContext.Current.Items("EditId").ToString())
                If mEditId = accountSeqId Then
                    Dim mSecurityInfo As MSecurityInfo = New MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditOtherAccount", True)), AccountUtility.CurrentProfile())
                    If Not mSecurityInfo Is Nothing Then
                        If mSecurityInfo.MayDelete Then
                            Try
                                AccountUtility.Delete(accountSeqId)
                                mRetVal = True
                            Catch ex As Exception
                                mLog.Error(ex)
                            End Try
                        Else
                            Dim mError As Exception = New Exception("The account (" + AccountUtility.CurrentProfile.Account + ") being used does not have the correct permissions to delete")
                            mLog.Error(mError)
                            Return Me.InternalServerError(mError)
                        End If
                    Else
                        Dim mError As Exception = New Exception("Security Info can not be determined nothing has been saved!!!!")
                        mLog.Error(mError)
                        Return Me.InternalServerError(mError)
                    End If
                Else
                    Dim mError As Exception = New Exception("Identifier you have last looked at does not match the one passed in nothing has been saved!!!!")
                    mLog.Error(mError)
                    Return Me.InternalServerError(mError)
                End If
            Else
                Dim mError As Exception = New Exception("Can not verify the identifier you are trying to work with!")
                mLog.Error(mError)
                Return Me.InternalServerError(mError)
            End If
            Return Me.Ok(mRetVal)
        End Function

        <HttpPost>
        Public Function GetMenuData(<FromUri()> ByVal menuType As Integer) As IHttpActionResult
            Dim mAccount As String = AccountUtility.HttpContextUserName()
            Dim mMenuType As MenuType = DirectCast(menuType, MenuType)
            Dim mDataTable As DataTable = AccountUtility.GetMenu(mAccount, mMenuType)
            Return Ok(mDataTable)
        End Function

        <HttpPost>
        Public Function GetSearchResults(<FromBody> searchCriteria As MSearchCriteria) As IHttpActionResult
            Dim mDataTable As DataTable = Nothing
            Try
                'searchCriteria.WhereClause = HttpUtility.UrlDecode(searchCriteria.WhereClause)
                mDataTable = AccountUtility.Search(searchCriteria)
            Catch ex As Exception
                ' Only going to log message
                Dim mLog As Logger = Logger.Instance()
                mLog.Error(ex)
            End Try
            Return Ok(mDataTable)
        End Function

        <HttpPost>
        Public Function GetSecurityInfo(<FromUri> action As String) As MUISecurityInfo
            If String.IsNullOrEmpty(action) Then Throw New ArgumentNullException("action", "action cannot be a null reference (Nothing in VB) or empty!")
            Dim mSecurityInfo As MSecurityInfo = New MSecurityInfo(FunctionUtility.GetProfile(action), AccountUtility.CurrentProfile())
            Dim mRetVal As MUISecurityInfo = New MUISecurityInfo
            mRetVal.MayAdd = mSecurityInfo.MayAdd
            mRetVal.MayDelete = mSecurityInfo.MayDelete
            mRetVal.MayEdit = mSecurityInfo.MayEdit
            mRetVal.MayView = mSecurityInfo.MayView
            Return mRetVal
        End Function

        <HttpGet>
        Public Function GetPreferences() As MUIAccountChoices
            Dim mAccountProfile As MAccountProfile = AccountUtility.CurrentProfile()
            Dim mRetVal As MUIAccountChoices = Nothing
            If Not mAccountProfile Is Nothing Then
                Dim mClientChoicesState As MClientChoicesState = ClientChoicesUtility.GetClientChoicesState(AccountUtility.CurrentProfile().Account)
                mRetVal = New MUIAccountChoices(mClientChoicesState)
                mRetVal.Environment = GWWebHelper.DisplayEnvironment
                mRetVal.Version = GWWebHelper.Version
                mRetVal.FrameworkVersion = GWWebHelper.FrameworkVersion
            End If
            Return mRetVal
        End Function


        <HttpGet>
        Public Function GetProfile(<FromUri()> ByVal accountSeqID As Integer) As MUIAccountProfile
            Dim mProfile As MAccountProfile = Nothing
            Dim mRetVal As MUIAccountProfile = New MUIAccountProfile()
            If Not accountSeqID = -1 Then
                mProfile = AccountUtility.GetProfile(accountSeqID)
                mRetVal.Account = mProfile.Account
                Dim mUIGroups As MUIAccountGroups = New MUIAccountGroups
                Dim mUIRoles As MUIAccountRoles = New MUIAccountRoles
                mUIGroups.Groups = mProfile.GetCommaSeparatedAssignedGroups().Split(",")
                mUIRoles.Roles = mProfile.GetCommaSeparatedAssignedRoles().Split(",")
                mRetVal.AccountGroups = mUIGroups
                mRetVal.AccountRoles = mUIRoles
                mRetVal.DerivedRoles = mProfile.DerivedRoles.Cast(Of String)().ToList()
                mRetVal.EMail = mProfile.Email
                mRetVal.EnableNotifications = mProfile.EnableNotifications
                mRetVal.FirstName = mProfile.FirstName
                mRetVal.Id = mProfile.Id
                mRetVal.IsSystemAdmin = mProfile.IsSystemAdmin
                mRetVal.LastName = mProfile.LastName
                mRetVal.Location = mProfile.Location
                mRetVal.MiddleName = mProfile.MiddleName
                mRetVal.PreferredName = mProfile.PreferredName
                mRetVal.Status = mProfile.Status
                mRetVal.TimeZone = mProfile.TimeZone
            Else
                mRetVal.Id = -1
            End If
            Return mRetVal
        End Function

        <HttpPost>
        Public Function Logon(ByVal jsonData As MUILogonInfo) As IHttpActionResult
            If jsonData Is Nothing Then Throw New ArgumentNullException("jsonData", "jsonData cannot be a null reference (Nothing in Visual Basic)!")
            If String.IsNullOrEmpty(jsonData.Account) Then Throw New NullReferenceException("jsonData.Account cannot be a null reference (Nothing in Visual Basic)!")
            If String.IsNullOrEmpty(jsonData.Password) Then Throw New NullReferenceException("jsonData.Password cannot be a null reference (Nothing in Visual Basic)!")
            Dim mRetVal As String = "false"
            Dim mDomainPassed As Boolean = False
            If jsonData.Account.Contains("\") Then
                mDomainPassed = True
            End If
            If ConfigSettings.AuthenticationType.ToUpper() = "LDAP" And Not mDomainPassed Then
                jsonData.Account = ConfigSettings.LdapDomain + "\" + jsonData.Account
            End If
            If AccountUtility.Authenticated(jsonData.Account, jsonData.Password) Then
                Dim mAccountProfile As MAccountProfile = AccountUtility.GetProfile(jsonData.Account)
                With mAccountProfile
                    .LastLogOn = DateTime.Now
                    .FailedAttempts = 0
                End With
                If mAccountProfile.Status = Convert.ToInt32(SystemStatus.Disabled) Then mAccountProfile.Status = Convert.ToInt32(SystemStatus.Active)

                AccountUtility.Save(mAccountProfile, False, False)
                AccountUtility.SetPrincipal(mAccountProfile)
                mRetVal = "true"
            Else
                Dim mAccountProfile As MAccountProfile = AccountUtility.GetProfile(jsonData.Account)
                If mAccountProfile IsNot Nothing Then
                    If mAccountProfile.Account.ToUpper(New CultureInfo("en-US", False)) = jsonData.Account.ToUpper(New CultureInfo("en-US", False)) Then
                        If ConfigSettings.AuthenticationType.ToUpper() = "INTERNAL" Then
                            If Not mAccountProfile.Status = Convert.ToInt32(SystemStatus.Disabled) Or Not mAccountProfile.Status = Convert.ToInt32(SystemStatus.Inactive) Then
                                mRetVal = "Request"
                            Else
                                Dim mMessageProfile As MMessageProfile = MessageUtility.GetProfile("DisabledAccount")
                                If mMessageProfile IsNot Nothing Then
                                    mRetVal = mMessageProfile.Body
                                End If
                            End If
                        Else
                            Dim mMessageProfile As MMessageProfile = MessageUtility.GetProfile("Logon Error")
                            If mMessageProfile IsNot Nothing Then
                                mRetVal = mMessageProfile.Body
                            End If
                        End If
                    End If
                Else
                    Dim mMessageProfile As MMessageProfile = MessageUtility.GetProfile("Logon Error")
                    If mMessageProfile IsNot Nothing Then
                        mRetVal = mMessageProfile.Body
                    End If
                End If
            End If

            Return Ok(mRetVal)
        End Function

        <HttpGet>
        Public Function Logoff() As IHttpActionResult
            Dim mRetVal As String = "false"
            AccountUtility.LogOff()
            Return Ok(mRetVal)
        End Function

        <HttpPost>
        Public Function RequestChange(<FromUri()> ByVal account As String) As IHttpActionResult
            Dim mRetVal As String = String.Empty
            Dim mProfile As MAccountProfile = AccountUtility.GetProfile(account)
            Dim mSecurityEntityProfile As MSecurityEntityProfile = SecurityEntityUtility.CurrentProfile()
            Dim mMessageProfile As MMessageProfile = MessageUtility.GetProfile("ErrorAccountDetails")
            Dim mRequestNewPassword As MRequestNewPassword = Nothing
            Dim isEncryptedPassword As Boolean = False
            Dim clearTextAccount As String = String.Empty
            Dim mLog As Logger = Logger.Instance()
            If Not mProfile Is Nothing Then
                With mProfile
                    .FailedAttempts = 0
                    .Status = SystemStatus.ChangePassword
                    .Password = CryptoUtility.Encrypt(GWWebHelper.GetNewGuid, mSecurityEntityProfile.EncryptionType)
                    .UpdatedBy = AccountUtility.GetProfile("anonymous").Id
                    .UpdatedDate = Date.Now
                End With
                clearTextAccount = CryptoUtility.Decrypt(mProfile.Password, mSecurityEntityProfile.EncryptionType)
                Try
                    mMessageProfile = MessageUtility.GetProfile("Request Password Reset UI")
                    mRetVal = mMessageProfile.Body
                    mMessageProfile = MessageUtility.GetProfile("RequestNewPassword")
                    mRequestNewPassword = New MRequestNewPassword(mMessageProfile)
                    With mRequestNewPassword
                        .AccountName = HttpUtility.UrlEncode(CryptoUtility.Encrypt(mProfile.Account, mSecurityEntityProfile.EncryptionType))
                        .FullName = mProfile.FirstName + " " + mProfile.LastName
                        .Password = HttpUtility.UrlEncode(mProfile.Password)
                        .Server = GWWebHelper.RootSite
                    End With
                    mProfile = AccountUtility.Save(mProfile, False, False)
                    NotifyUtility.SendMail(mRequestNewPassword, mProfile)
                    'btnRequestPasswordReset.Visible = False
                    mLog.Debug("Reset password for account " & clearTextAccount)
                Catch ex As Net.Mail.SmtpException
                    Dim myException As New Exception("Could not send e-mail." & ex.Message)
                    mLog.Error(myException)
                    mMessageProfile = MessageUtility.GetProfile("PasswordSendMailError")
                    mRetVal = mMessageProfile.Body
                Catch ex As Exception
                    Dim myException As New Exception("Could not set account details." & ex.Message)
                    mLog.Error(myException)
                    mMessageProfile = MessageUtility.GetProfile("ErrorAccountDetails")
                    mRetVal = mMessageProfile.Body
                End Try
            End If
            Return Ok(mRetVal)
        End Function

        <HttpPost>
        Public Function Save(ByVal uiProfile As MUIAccountProfile) As IHttpActionResult
            If uiProfile Is Nothing Then Throw New ArgumentNullException("uiProfile", "uiProfile cannot be a null reference (Nothing in Visual Basic)!")
            Dim mRetVal As String = False
            Dim mSaveGroups As Boolean = False
            Dim mSaveRoles As Boolean = False
            Dim mCurrentAccountProfile As MAccountProfile = AccountUtility.CurrentProfile()
            Dim mAccountProfileToSave As MAccountProfile = New MAccountProfile()
            Dim mLog As Logger = Logger.Instance()
            If HttpContext.Current.Request.QueryString("Action").ToString.ToUpper(CultureInfo.InvariantCulture).IndexOf("REGISTER") > -1 Then
                Dim mExistingAccount As MAccountProfile = AccountUtility.GetProfile(uiProfile.Account)
                If mExistingAccount Is Nothing Then
                    mAccountProfileToSave = populateAccountProfile(uiProfile, mAccountProfileToSave)
                    mAccountProfileToSave.Id = uiProfile.Id
                    Dim mGroups As String = ConfigSettings.RegistrationGroups
                    Dim mRoles As String = ConfigSettings.RegistrationRoles
                    If Not String.IsNullOrEmpty(mGroups) Then mSaveGroups = True
                    If Not String.IsNullOrEmpty(mRoles) Then mSaveRoles = True
                    mAccountProfileToSave.AddedBy = mCurrentAccountProfile.Id
                    mAccountProfileToSave.AddedDate = Now
                    mAccountProfileToSave.SetGroups(mGroups)
                    mAccountProfileToSave.SetRoles(mRoles)
                    mAccountProfileToSave.PasswordLastSet = DateTime.Now
                    mAccountProfileToSave.LastLogOn = DateTime.Now
                    mAccountProfileToSave.Password = CryptoUtility.Encrypt(ConfigSettings.RegistrationPassword, ConfigSettings.EncryptionType)
                    mAccountProfileToSave.Status = Integer.Parse(ConfigSettings.RegistrationStatusId)
                    If HttpContext.Current.Request.QueryString("Action").ToString.ToUpper(CultureInfo.InvariantCulture).IndexOf("REGISTER") > -1 Then mAccountProfileToSave.Status = SystemStatus.Active
                    Dim mClientChoiceState As MClientChoicesState = ClientChoicesUtility.GetClientChoicesState(ConfigSettings.RegistrationAccountChoicesAccount, True)
                    Dim mSecurityEntityProfile As MSecurityEntityProfile = SecurityEntityUtility.GetProfile(Integer.Parse(ConfigSettings.RegistrationSecurityEntityId))
                    Dim mCurrentSecurityEntityId As String = mClientChoiceState(MClientChoices.SecurityEntityId)

                    mClientChoiceState.IsDirty = False
                    mClientChoiceState(MClientChoices.AccountName) = mAccountProfileToSave.Account
                    mClientChoiceState(MClientChoices.SecurityEntityId) = mSecurityEntityProfile.Id.ToString(CultureInfo.InvariantCulture)
                    mClientChoiceState(MClientChoices.SecurityEntityName) = mSecurityEntityProfile.Name
                    Try
                        AccountUtility.Save(mAccountProfileToSave, mSaveRoles, mSaveGroups, mSecurityEntityProfile)
                        ClientChoicesUtility.Save(mClientChoiceState, False)
                        AccountUtility.SetPrincipal(mAccountProfileToSave)
                        mRetVal = "Your account has been created"
                    Catch ex As Exception
                        mLog.Error(ex)
                    End Try
                Else
                    mRetVal = "The account '" + uiProfile.Account + "' already exists please choose a different account/email"
                End If
            Else
                If Not HttpContext.Current.Items("EditId") Is Nothing Or mCurrentAccountProfile.Status = DirectCast(SystemStatus.SetAccountDetails, Integer) Then
                    Dim mEditId As Integer
                    If Not HttpContext.Current.Items("EditId") Is Nothing Then
                        mEditId = Integer.Parse(HttpContext.Current.Items("EditId").ToString())
                    Else
                        mEditId = mCurrentAccountProfile.Id
                    End If
                    If mEditId = uiProfile.Id Then
                        Dim mSecurityInfo As MSecurityInfo = New MSecurityInfo(FunctionUtility.CurrentProfile, AccountUtility.CurrentProfile())
                        If Not mSecurityInfo Is Nothing Then
                            If mEditId <> -1 Then
                                If mCurrentAccountProfile.Id <> uiProfile.Id Then mSecurityInfo = New MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditOtherAccount", True)), mCurrentAccountProfile)
                                If mSecurityInfo.MayEdit Or mCurrentAccountProfile.Status = DirectCast(SystemStatus.SetAccountDetails, Integer) Then
                                    Dim mGroupTabSecurity As MSecurityInfo = New MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_View_Account_Group_Tab", True)), mCurrentAccountProfile)
                                    Dim mRoleTabSecurity As MSecurityInfo = New MSecurityInfo(FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_View_Account_Role_Tab", True)), mCurrentAccountProfile)
                                    mAccountProfileToSave = AccountUtility.GetProfile(mEditId)
                                    mAccountProfileToSave = populateAccountProfile(uiProfile, mAccountProfileToSave)
                                    mAccountProfileToSave.Id = uiProfile.Id
                                    Dim mGroups As String = String.Join(",", uiProfile.AccountGroups.Groups)
                                    Dim mRoles As String = String.Join(",", uiProfile.AccountRoles.Roles)
                                    If mGroupTabSecurity.MayView And FunctionUtility.CurrentProfile().Action.ToLowerInvariant() = ConfigSettings.GetAppSettingValue("Actions_EditOtherAccount", True).ToLower(CultureInfo.InvariantCulture) Then
                                        If mAccountProfileToSave.GetCommaSeparatedAssignedGroups <> mGroups Then
                                            mSaveGroups = True
                                            mAccountProfileToSave.SetGroups(mGroups)
                                        End If
                                    End If
                                    If mRoleTabSecurity.MayView And FunctionUtility.CurrentProfile().Action.ToLowerInvariant() = ConfigSettings.GetAppSettingValue("Actions_EditOtherAccount", True).ToLower(CultureInfo.InvariantCulture) Then
                                        If mAccountProfileToSave.GetCommaSeparatedAssignedRoles <> mRoles Then
                                            mSaveRoles = True
                                            mAccountProfileToSave.SetRoles(mRoles)
                                        End If
                                    End If
                                    mAccountProfileToSave.AddedBy = mCurrentAccountProfile.Id
                                    mAccountProfileToSave.AddedDate = Now
                                    AccountUtility.Save(mAccountProfileToSave, mSaveRoles, mSaveGroups)
                                    mLog.Debug("Saved account " + mAccountProfileToSave.Account + " by " + mCurrentAccountProfile.Account)
                                    mRetVal = True
                                Else
                                    Dim mError As Exception = New Exception("The account (" + AccountUtility.CurrentProfile.Account + ") being used does not have the correct permissions to edit")
                                    mLog.Error(mError)
                                    Return Me.InternalServerError(mError)
                                End If
                            Else
                                If mSecurityInfo.MayAdd Then
                                    mSaveGroups = True
                                    mSaveRoles = True
                                    mAccountProfileToSave = populateAccountProfile(uiProfile, mAccountProfileToSave)
                                    mAccountProfileToSave.Id = -1
                                    mAccountProfileToSave.AddedBy = mCurrentAccountProfile.Id
                                    mAccountProfileToSave.AddedDate = DateTime.Now

                                    mAccountProfileToSave.AddedBy = mCurrentAccountProfile.Id
                                    mAccountProfileToSave.AddedDate = DateTime.Now
                                    mAccountProfileToSave.PasswordLastSet = DateTime.Now
                                    mAccountProfileToSave.LastLogOn = DateTime.Now
                                    mAccountProfileToSave.Password = CryptoUtility.Encrypt(ConfigSettings.RegistrationPassword, ConfigSettings.EncryptionType)
                                    mAccountProfileToSave.Status = ConfigSettings.AutoCreateAccountStatusId
                                    Dim mGroups As String = String.Join(",", uiProfile.AccountGroups.Groups)
                                    Dim mRoles As String = String.Join(",", uiProfile.AccountRoles.Roles)
                                    mAccountProfileToSave.SetGroups(mGroups)
                                    mAccountProfileToSave.SetRoles(mRoles)
                                    Try
                                        AccountUtility.Save(mAccountProfileToSave, mSaveRoles, mSaveGroups)
                                    Catch ex As Exception
                                        mLog.Error(ex)
                                    End Try
                                    mLog.Debug("Added account " + mAccountProfileToSave.Account + " by " + mCurrentAccountProfile.Account)
                                    mRetVal = "true"
                                Else
                                    Dim mError As Exception = New Exception("The account (" + AccountUtility.CurrentProfile.Account + ") being used does not have the correct permissions to add")
                                    mLog.Error(mError)
                                    Return Me.InternalServerError(mError)
                                End If
                            End If
                        Else
                            Dim mError As Exception = New Exception("Security Info is not in context nothing has been saved!!!!")
                            mLog.Error(mError)
                            Return Me.InternalServerError(mError)
                        End If
                    Else
                        Dim mError As Exception = New Exception("Identifier you have last looked at does not match the one passed in nothing has been saved!!!!")
                        mLog.Error(mError)
                        Return Me.InternalServerError(mError)
                    End If
                End If

            End If
            Return Me.Ok(mRetVal)
        End Function

        <HttpPost>
        Public Function SaveClientChoices(ByVal choices As MUIAccountChoices) As IHttpActionResult
            If choices Is Nothing Then Throw New ArgumentNullException("choices", "choices cannot be a null reference (Nothing in Visual Basic)!")
            Dim mRetVal As String = False
            Dim mClientChoicesState As MClientChoicesState = ClientChoicesUtility.GetClientChoicesState(AccountUtility.CurrentProfile().Account)
            mClientChoicesState(MClientChoices.Action) = choices.Action
            mClientChoicesState(MClientChoices.BackColor) = choices.BackColor
            mClientChoicesState(MClientChoices.ColorScheme) = choices.ColorScheme
            mClientChoicesState(MClientChoices.HeadColor) = choices.HeadColor
            mClientChoicesState(MClientChoices.HeaderForeColor) = choices.HeaderForeColor
            mClientChoicesState(MClientChoices.RowBackColor) = choices.RowBackColor
            mClientChoicesState(MClientChoices.AlternatingRowBackColor) = choices.AlternatingRowBackColor
            mClientChoicesState(MClientChoices.LeftColor) = choices.LeftColor
            mClientChoicesState(MClientChoices.RecordsPerPage) = choices.RecordsPerPage.ToString()
            mClientChoicesState(MClientChoices.SubheadColor) = choices.SubheadColor
            ClientChoicesUtility.Save(mClientChoicesState)
            AccountUtility.RemoveInMemoryInformation(True)
            Return Me.Ok(mRetVal)
        End Function

        <HttpPost>
        Public Function SelectSecurityEntity(<FromUri()> ByVal selectedSecurityEntityId As Integer) As IHttpActionResult
            Dim targetSEProfile As MSecurityEntityProfile = SecurityEntityUtility.GetProfile(selectedSecurityEntityId)
            Dim currentSEProfile As MSecurityEntityProfile = SecurityEntityUtility.CurrentProfile()
            Dim mClientChoicesState As MClientChoicesState = ClientChoicesUtility.GetClientChoicesState(AccountUtility.CurrentProfile().Account)
            Dim mMessageProfile As MMessageProfile = Nothing
            Try
                If Not ConfigSettings.CentralManagement Then
                    'SecurityEntityUtility.SetSessionSecurityEntity(targetSEProfile)
                    mClientChoicesState(MClientChoices.SecurityEntityId) = targetSEProfile.Id
                    mClientChoicesState(MClientChoices.SecurityEntityName) = targetSEProfile.Name
                Else
                    If currentSEProfile.ConnectionString = targetSEProfile.ConnectionString Then
                        mClientChoicesState(MClientChoices.SecurityEntityId) = targetSEProfile.Id
                        mClientChoicesState(MClientChoices.SecurityEntityName) = targetSEProfile.Name
                    Else
                        mClientChoicesState(MClientChoices.SecurityEntityId) = ConfigSettings.DefaultSecurityEntityId
                        mClientChoicesState(MClientChoices.SecurityEntityName) = "System"
                    End If
                End If
                ClientChoicesUtility.Save(mClientChoicesState)
                AccountUtility.RemoveInMemoryInformation(True)
                mMessageProfile = MessageUtility.GetProfile("ChangedSelectedSecurityEntity")
            Catch ex As Exception
                Dim mLog As Logger = Logger.Instance()
                mMessageProfile = MessageUtility.GetProfile("NoDataFound")
                Dim myEx As New Exception("SelectSecurityEntity:: reported an error.", ex)
                mLog.Error(myEx)
            End Try
            ' update all of your in memory information
            Return Ok(mMessageProfile.Body)
        End Function

        Private Function populateAccountProfile(ByVal uiProfile As MUIAccountProfile, ByVal accountProfile As MAccountProfile) As MAccountProfile
            If accountProfile Is Nothing Then accountProfile = New MAccountProfile()
            accountProfile.Account = uiProfile.Account
            accountProfile.Email = uiProfile.EMail
            accountProfile.EnableNotifications = uiProfile.EnableNotifications
            accountProfile.FirstName = uiProfile.FirstName
            If AccountUtility.CurrentProfile().IsSystemAdmin Then
                accountProfile.IsSystemAdmin = uiProfile.IsSystemAdmin
            End If
            accountProfile.LastName = uiProfile.LastName
            accountProfile.Location = uiProfile.Location
            accountProfile.MiddleName = uiProfile.MiddleName
            accountProfile.Name = uiProfile.Account
            accountProfile.PreferredName = uiProfile.PreferredName
            accountProfile.Status = uiProfile.Status
            accountProfile.TimeZone = uiProfile.TimeZone
            Return accountProfile
        End Function
    End Class
End Namespace
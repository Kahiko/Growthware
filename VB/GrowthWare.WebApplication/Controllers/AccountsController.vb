Imports System.Net
Imports System.Web.Http
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.Model.Profiles.Base
Imports GrowthWare.WebSupport
Imports System.Globalization
Imports GrowthWare.Framework.Model.Enumerations

Namespace Controllers
    Public Class AccountsController
        Inherits ApiController

        <HttpGet>
        Public Function GetPreferences() As MUIAccountChoices
            Dim mClientChoicesState As MClientChoicesState = ClientChoicesUtility.GetClientChoicesState(AccountUtility.CurrentProfile().Account)
            Dim mRetVal As MUIAccountChoices = New MUIAccountChoices(mClientChoicesState)
            mRetVal.Environment = GWWebHelper.DisplayEnvironment
            mRetVal.Version = GWWebHelper.Version
            Return mRetVal
        End Function

        <HttpPost>
        Public Function Logon(ByVal jsonData As LogonInfo) As IHttpActionResult
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
                AccountUtility.SetPrincipal(mAccountProfile)
                mRetVal = "true"
            Else
                Dim mAccountProfile As MAccountProfile = AccountUtility.GetProfile(jsonData.Account)
                If mAccountProfile IsNot Nothing Then
                    If mAccountProfile.Account.ToUpper(New CultureInfo("en-US", False)) = jsonData.Account.ToUpper(New CultureInfo("en-US", False)) Then
                        If ConfigSettings.AuthenticationType.ToUpper() = "INTERNAL" Then
                            mRetVal = "Request"
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

        'Public Function ChangePassword(<FromBody()> ByVal oldPassword As String, <FromBody()> ByVal newPassword As String) As IHttpActionResult

        <HttpPost>
        Public Function ChangePassword(ByVal mChangePassword As MChangePassword) As IHttpActionResult
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
        Public Function SelectSecurityEntity(<FromUri()> ByVal selectedSecurityEntityId As Integer) As IHttpActionResult
            Dim targetSEProfile As MSecurityEntityProfile = SecurityEntityUtility.GetProfile(selectedSecurityEntityId)
            Dim currentSEProfile As MSecurityEntityProfile = SecurityEntityUtility.CurrentProfile()
            Dim mClientChoicesState As MClientChoicesState = CType(HttpContext.Current.Cache(MClientChoices.SessionName), MClientChoicesState)
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
                Dim myMessageProfile As New MMessageProfile
                Dim mLog As Logger = Logger.Instance()
                mMessageProfile = MessageUtility.GetProfile("NoDataFound")
                Dim myEx As New Exception("SelectSecurityEntity:: reported an error.", ex)
                mLog.Error(myEx)
            End Try
            ' update all of your in memory information
            Return Ok(mMessageProfile.Body)
        End Function


        <HttpPost>
        Public Function Save(ByVal uiProfile As UIAccountProfile) As IHttpActionResult
            If uiProfile Is Nothing Then Throw New ArgumentNullException("uiProfile", "uiProfile cannot be a null reference (Nothing in Visual Basic)!")
            Dim mRetVal As Boolean = False
            Dim mSaveGroups As Boolean = False
            Dim mSaveRoles As Boolean = False
            Dim mLog As Logger = Logger.Instance()
            If Not HttpContext.Current.Items("EditId") Is Nothing Then
                Dim mEditId = Integer.Parse(HttpContext.Current.Items("EditId").ToString())
                If mEditId = uiProfile.Id Then
                    Dim mSecurityInfo As MSecurityInfo = DirectCast(HttpContext.Current.Items("SecurityInfo"), MSecurityInfo)
                    If Not mSecurityInfo Is Nothing Then
                        If mEditId <> -1 Then
                            If mSecurityInfo.MayEdit Then
                                Dim mAccountProfile As MAccountProfile = AccountUtility.GetProfile(mEditId)
                                mAccountProfile = populateAccountProfile(uiProfile, mAccountProfile)
                                mAccountProfile.Id = uiProfile.Id
                                Dim mGroups = String.Join(",", uiProfile.AccountGroups.Groups)
                                Dim mRoles = String.Join(",", uiProfile.AccountRoles.Roles)
                                If uiProfile.CanSaveGroups Then
                                    If mAccountProfile.GetCommaSeparatedAssignedGroups <> mGroups Then
                                        mSaveGroups = True
                                        mAccountProfile.SetGroups(mGroups)
                                    End If
                                End If
                                If uiProfile.CanSaveRoles Then
                                    If mAccountProfile.GetCommaSeparatedAssignedRoles <> mRoles Then
                                        mSaveRoles = True
                                        mAccountProfile.SetRoles(mRoles)
                                    End If
                                End If

                                AccountUtility.Save(mAccountProfile, mSaveRoles, mSaveGroups)
                                mRetVal = True
                            Else
                                Dim mError As Exception = New Exception("The account (" + AccountUtility.CurrentProfile.Account + ") being used does not have the correct permissions to add")
                                mLog.Error(mError)
                                Return Me.InternalServerError(mError)
                            End If
                        Else
                            If mSecurityInfo.MayAdd Then
                                Dim mAccountProfile As MAccountProfile = New MAccountProfile()
                                mAccountProfile = populateAccountProfile(uiProfile, mAccountProfile)
                                mAccountProfile.Id = -1
                                mAccountProfile.AddedBy = AccountUtility.CurrentProfile().Id
                                mAccountProfile.AddedDate = Now
                                mAccountProfile.UpdatedBy = mAccountProfile.AddedBy
                                mAccountProfile.UpdatedDate = mAccountProfile.AddedDate
                                Dim mGroups = String.Join(",", uiProfile.AccountGroups.Groups)
                                Dim mRoles = String.Join(",", uiProfile.AccountRoles.Roles)
                                If uiProfile.CanSaveGroups Then
                                    If mAccountProfile.GetCommaSeparatedAssignedGroups <> mGroups Then
                                        mSaveGroups = True
                                        mAccountProfile.SetGroups(mGroups)
                                    End If
                                End If
                                If uiProfile.CanSaveRoles Then
                                    If mAccountProfile.GetCommaSeparatedAssignedRoles <> mRoles Then
                                        mSaveRoles = True
                                        mAccountProfile.SetRoles(mRoles)
                                    End If
                                End If
                                AccountUtility.Save(mAccountProfile, mSaveRoles, mSaveGroups)
                                mRetVal = True
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
            Return Me.Ok(mRetVal)
        End Function

        Private Function populateAccountProfile(ByVal uiProfile As UIAccountProfile, ByVal accountProfile As MAccountProfile) As MAccountProfile
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

    Public Class MChangePassword
        Public Property OldPassword() As String
        Public Property NewPassword() As String
    End Class

    Public Class LogonInfo
        Public Property Account() As String
        Public Property Password() As String
    End Class

    ''' <summary>
    ''' Class MUIAccountChoices
    ''' </summary>
    Public Class MUIAccountChoices
        Inherits MProfile

        ''' <summary>
        ''' Initializes a new instance of the <see cref="MUIAccountChoices"/> class.
        ''' </summary>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="MUIAccountChoices"/> class.
        ''' </summary>
        ''' <param name="clientChoicesState">State of the client choices.</param>
        Public Sub New(ByVal clientChoicesState As MClientChoicesState)
            If clientChoicesState(MClientChoices.AccountName) <> Nothing Then AccountName = clientChoicesState(MClientChoices.AccountName).ToString()
            If clientChoicesState(MClientChoices.Action) <> Nothing Then Action = clientChoicesState(MClientChoices.Action).ToString()
            If clientChoicesState(MClientChoices.BackColor) <> Nothing Then BackColor = clientChoicesState(MClientChoices.BackColor).ToString()
            If clientChoicesState(MClientChoices.ColorScheme) <> Nothing Then ColorScheme = clientChoicesState(MClientChoices.ColorScheme).ToString()
            If clientChoicesState(MClientChoices.HeadColor) <> Nothing Then HeadColor = clientChoicesState(MClientChoices.HeadColor).ToString()
            If clientChoicesState(MClientChoices.LeftColor) <> Nothing Then LeftColor = clientChoicesState(MClientChoices.LeftColor).ToString()
            If clientChoicesState(MClientChoices.RecordsPerPage) <> Nothing Then RecordsPerPage = Integer.Parse(clientChoicesState(MClientChoices.RecordsPerPage).ToString())
            If clientChoicesState(MClientChoices.SecurityEntityId) <> Nothing Then SecurityEntityID = Integer.Parse(clientChoicesState(MClientChoices.SecurityEntityId).ToString())
            If clientChoicesState(MClientChoices.SecurityEntityName) <> Nothing Then SecurityEntityName = clientChoicesState(MClientChoices.SecurityEntityName).ToString()
            If clientChoicesState(MClientChoices.SubheadColor) <> Nothing Then SubheadColor = clientChoicesState(MClientChoices.SubheadColor).ToString()
        End Sub

        ''' <summary>
        ''' Gets or sets the name of the account.
        ''' </summary>
        ''' <value>The name of the account.</value>
        Public Property AccountName As String

        ''' <summary>
        ''' Gets or sets the action.
        ''' </summary>
        ''' <value>The action.</value>
        Public Property Action As String

        ''' <summary>
        ''' Gets or sets the color of the back.
        ''' </summary>
        ''' <value>The color of the back.</value>
        Public Property BackColor As String

        ''' <summary>
        ''' Gets or sets the color scheme.
        ''' </summary>
        ''' <value>The color scheme.</value>
        Public Property ColorScheme As String

        ''' <summary>
        ''' Gets or sets the environment.
        ''' </summary>
        ''' <value>The environment.</value>
        Public Property Environment As String

        ''' <summary>
        ''' Gets or sets the color of the head.
        ''' </summary>
        ''' <value>The color of the head.</value>
        Public Property HeadColor As String

        ''' <summary>
        ''' Gets or sets the color of the header foreground color.
        ''' </summary>
        ''' <value>The color of the header foreground color.</value>
        Public Property HeaderForeColor As String

        ''' <summary>
        ''' Gets or sets the color of the left.
        ''' </summary>
        ''' <value>The color of the left.</value>
        Public Property LeftColor As String

        ''' <summary>
        ''' Gets or sets the records per page.
        ''' </summary>
        ''' <value>The records per page.</value>
        Public Property RecordsPerPage As Integer

        ''' <summary>
        ''' Gets or sets the account.
        ''' </summary>
        ''' <value>The account.</value>
        Public Property Account As String

        ''' <summary>
        ''' Gets or sets the security entity ID.
        ''' </summary>
        ''' <value>The security entity ID.</value>
        Public Property SecurityEntityID As Integer

        ''' <summary>
        ''' Gets or sets the version.
        ''' </summary>
        ''' <value>The version.</value>
        Public Property Version As String

        ''' <summary>
        ''' Gets or sets the name of the security entity.
        ''' </summary>
        ''' <value>The name of the security entity.</value>
        Public Property SecurityEntityName As String

        ''' <summary>
        ''' Gets or sets the color of the subhead.
        ''' </summary>
        ''' <value>The color of the subhead.</value>
        Public Property SubheadColor As String

        ''' <summary>
        ''' Gets or sets the color of the row background color.
        ''' </summary>
        ''' <value>The color of the row background color.</value>
        Public Property RowBackColor As String

        ''' <summary>
        ''' Gets or sets the color of the alternating row background color.
        ''' </summary>
        ''' <value>The color of the alternating row background color.</value>
        Public Property AlternatingRowBackColor As String
    End Class

    Public Class UIAccountProfile
        Public Account As String
        Public AccountGroups As UIAccountGroups
        Public AccountRoles As UIAccountRoles
        Public CanSaveRoles As Boolean
        Public CanSaveGroups As Boolean
        Public EMail As String
        Public EnableNotifications As Boolean
        Public FirstName As String
        Public Id As Integer
        Public IsSystemAdmin As Boolean
        Public LastName As String
        Public Location As String
        Public MiddleName As String
        Public PreferredName As String
        Public Status As Integer
        Public TimeZone As Integer
    End Class

    Public Class UIAccountRoles
        Public Roles() As String
    End Class

    Public Class UIAccountGroups
        Public Groups() As String
    End Class
End Namespace
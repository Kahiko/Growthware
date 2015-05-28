Imports System.Configuration
Imports System.Web
Imports System.Text.RegularExpressions
Imports System.Globalization
Imports GrowthWare.Framework.Model.Enumerations
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.Framework.Common
Imports System.Web.Configuration
Imports System.Web.SessionState
Imports GrowthWare.Framework.BusinessData
Imports System.IO

Namespace Context
    ''' <summary>
    ''' HttpContext entry point
    ''' </summary>
    ''' <remarks>Will initiate the ClientChoicesHttpModule</remarks>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")>
    Public Class HttpContextModule
        Implements IHttpModule, IRequiresSessionState

        Private m_DisposedValue As Boolean ' To detect redundant calls
        Private m_Filter As OutputFilterStream

        ''' <summary>
        ''' Implements IDispose
        ''' </summary>
        ''' <param name="disposing">Boolean</param>
        ''' <remarks></remarks>
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.m_DisposedValue Then
                If disposing Then
                    If Not m_Filter Is Nothing Then m_Filter.Dispose()
                End If
            End If
            Me.m_DisposedValue = True
        End Sub

        ''' <summary>
        ''' Implements Dispose
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Dispose() Implements IHttpModule.Dispose
            Me.Dispose(True)
        End Sub

        ''' <summary>
        ''' Initializes the HTTP module
        ''' </summary>
        ''' <param name="context">as HttpApplication</param>
        ''' <remarks>
        ''' Servers as a wrapper to the ClientChoicesHttpModule as well as 
        ''' placing the current function, security profiles into the context.
        ''' </remarks>
        Public Sub Init(ByVal context As System.Web.HttpApplication) Implements System.Web.IHttpModule.Init
            If Not context Is Nothing Then
                'Dim mInitClientChoices As New ClientChoicesHttpModule()
                'mInitClientChoices.Init(context)
                AddHandler context.Context.ApplicationInstance.Error, AddressOf Me.onApplicationError
                AddHandler context.BeginRequest, AddressOf Me.onBeginRequest
                AddHandler context.AcquireRequestState, AddressOf Me.onAcquireRequestState
                AddHandler context.EndRequest, AddressOf Me.onEndRequest
            End If
        End Sub

        ''' <summary>
        ''' Ons the application error.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        Private Sub onApplicationError(ByVal sender As Object, ByVal e As EventArgs)
            Dim mEx As Exception = HttpContext.Current.Server.GetLastError
            If mEx IsNot Nothing Then
                Dim mLog As Logger = Logger.Instance()
                GWWebHelper.ExceptionError = mEx
                mLog.Error(mEx)
                If (mEx.GetType() Is GetType(BusinessLogicLayerException)) Then
                    Dim config As Configuration = WebConfigurationManager.OpenWebConfiguration("~")
                    ConfigSettings.SetEnvironmentValue(config, False, "DB_Status", "OffLine", False)
                End If
            End If
            ' Next Line prevents the ASP.NET Error page from being shown.
            HttpContext.Current.Server.ClearError()
        End Sub

        ''' <summary>
        ''' Ons the state of the acquire request.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        Private Sub onAcquireRequestState(ByVal sender As Object, ByVal e As EventArgs)
            Dim mLog As Logger = Logger.Instance()
            Dim mAccountName As String = AccountUtility.HttpContextUserName()
            mLog.Debug("Started")
            mLog.Debug("CurrentExecutionFilePath " + HttpContext.Current.Request.CurrentExecutionFilePath)
            mLog.Debug("HttpContextUserName: " + mAccountName)
            If HttpContext.Current.Session Is Nothing Then
                mLog.Debug("No Session!")
                mLog.Debug("Ended")
                Exit Sub
            End If
            If Not HttpContext.Current.Session.Item("EditId") Is Nothing Then HttpContext.Current.Items("EditId") = HttpContext.Current.Session.Item("EditId")
            Dim mAccountProfile = AccountUtility.GetProfile(mAccountName)
            Dim mClientChoicesState As MClientChoicesState = ClientChoicesUtility.GetClientChoicesState(mAccountName)
            HttpContext.Current.Items(MClientChoices.SessionName) = mClientChoicesState
            Dim mAction As String = GWWebHelper.GetQueryValue(HttpContext.Current.Request, "Action")
            If String.IsNullOrEmpty(mAction) Then
                mLog.Debug("No Action!")
                mLog.Debug("Ended")
                Exit Sub
            End If
            If Not processRequest() Then
                mLog.Debug("Request not for processing!")
                mLog.Debug("Ended")
                Exit Sub
            End If
            Dim mException As WebSupportException = Nothing

            mLog.Debug("Action: " + mAction)
            Dim mFunctionProfile As MFunctionProfile = FunctionUtility.CurrentProfile()
            If mFunctionProfile Is Nothing Then mFunctionProfile = FunctionUtility.GetProfile(mAction)

            If Not mFunctionProfile.Source.ToUpper(CultureInfo.InvariantCulture).Contains("MENUS") AndAlso Not (mAction.ToUpper(CultureInfo.InvariantCulture) = "LOGOFF" Or mAction.ToUpper(CultureInfo.InvariantCulture) = "LOGON" Or mAction.ToUpper(CultureInfo.InvariantCulture) = "CHANGEPASSWORD") Then
                If mAccountProfile Is Nothing And mAccountName.ToUpper(CultureInfo.InvariantCulture) <> "ANONYMOUS" Then
                    Dim mMessage As String = "Could not find account '" + mAccountName + "'"
                    mLog.Info(mMessage)
                    If ConfigSettings.AutoCreateAccount Then
                        mMessage = "Creating new account for '" + mAccountName + "'"
                        mLog.Info(mMessage)
                        Dim mCurrentAccountProfile As MAccountProfile = AccountUtility.GetProfile("System")
                        Dim mAccountProfileToSave As MAccountProfile = New MAccountProfile()
                        mAccountProfileToSave.Id = -1
                        Dim mSaveGroups As Boolean = True
                        Dim mSaveRoles As Boolean = True
                        Dim mGroups = ConfigSettings.RegistrationGroups
                        Dim mRoles = ConfigSettings.RegistrationRoles
                        If String.IsNullOrEmpty(mGroups) Then mSaveGroups = False
                        If String.IsNullOrEmpty(mRoles) Then mSaveRoles = False
                        mAccountProfileToSave.Account = AccountUtility.HttpContextUserName
                        mAccountProfileToSave.FirstName = "Auto created"
                        mAccountProfileToSave.MiddleName = ""
                        mAccountProfileToSave.LastName = "Auto created"
                        mAccountProfileToSave.PreferredName = "Auto created"
                        mAccountProfileToSave.Email = "change@me.com"
                        mAccountProfileToSave.Location = "Hawaii"
                        mAccountProfileToSave.AddedBy = mCurrentAccountProfile.Id
                        mAccountProfileToSave.AddedDate = Now
                        mAccountProfileToSave.SetGroups(mGroups)
                        mAccountProfileToSave.SetRoles(mRoles)
                        mAccountProfileToSave.PasswordLastSet = DateTime.Now
                        mAccountProfileToSave.LastLogOn = DateTime.Now
                        mAccountProfileToSave.Password = CryptoUtility.Encrypt(ConfigSettings.RegistrationPassword, ConfigSettings.EncryptionType)
                        mAccountProfileToSave.Status = DirectCast(SystemStatus.SetAccountDetails, Integer)
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
                            mFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditAccount", True))
                            mException = New WebSupportException("Your account details need to be set.")
                            GWWebHelper.ExceptionError = mException
                            Dim mEditAccountPage As String = GWWebHelper.RootSite + ConfigSettings.AppName + mFunctionProfile.Source
                            FunctionUtility.SetCurrentProfile(mFunctionProfile)
                            HttpContext.Current.Response.Redirect(mEditAccountPage + "?Action=" + mFunctionProfile.Action)
                        Catch ex As Exception
                            mLog.Error(ex)
                        End Try
                        mAccountProfile = AccountUtility.GetProfile(mAccountProfileToSave.Account)
                    End If
                End If
                FunctionUtility.SetCurrentProfile(mFunctionProfile)
                Dim mSecurityInfo = New MSecurityInfo(mFunctionProfile, mAccountProfile)
                HttpContext.Current.Items("SecurityInfo") = mSecurityInfo
                Select Case mAccountProfile.Status
                    Case DirectCast(SystemStatus.ChangePassword, Integer)
                        mException = New WebSupportException("Your password needs to be changed before any other action can be performed.")
                        GWWebHelper.ExceptionError = mException
                        mFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_ChangePassword", True))
                        Dim mChangePasswordPage As String = GWWebHelper.RootSite + ConfigSettings.AppName + mFunctionProfile.Source
                        HttpContext.Current.Response.Redirect(mChangePasswordPage + "?Action=" + mFunctionProfile.Action)
                    Case DirectCast(SystemStatus.SetAccountDetails, Integer)
                        If HttpContext.Current.Request.Path.ToUpper(CultureInfo.InvariantCulture).IndexOf("/API/", StringComparison.OrdinalIgnoreCase) = -1 Then
                            mFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_EditAccount", True))
                            If mAction.ToUpper(CultureInfo.InvariantCulture) <> mFunctionProfile.Action.ToUpper(CultureInfo.InvariantCulture) Then
                                mException = New WebSupportException("Your account details need to be set.")
                                GWWebHelper.ExceptionError = mException
                                Dim mChangePasswordPage As String = GWWebHelper.RootSite + ConfigSettings.AppName + mFunctionProfile.Source
                                HttpContext.Current.Response.Redirect(mChangePasswordPage + "?Action=" + mFunctionProfile.Action)
                            End If
                        End If
                    Case Else
                        Dim mPage As String = String.Empty
                        If Not mSecurityInfo.MayView Then
                            If mAccountProfile.Account.ToUpper(CultureInfo.InvariantCulture) = "ANONYMOUS" Then
                                mException = New WebSupportException("Your session has timed out.<br/>Please sign in.")
                                GWWebHelper.ExceptionError = mException
                                mFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_Logon", True))
                                mPage = GWWebHelper.RootSite + ConfigSettings.AppName + mFunctionProfile.Source
                                HttpContext.Current.Response.Redirect(mPage + "?Action=" + mFunctionProfile.Action)
                            End If
                            mFunctionProfile = FunctionUtility.GetProfile(ConfigSettings.GetAppSettingValue("Actions_AccessDenied", True))
                            mLog.Warn("Access was denied to Account: " + mAccountProfile.Account + " for Action: " + mFunctionProfile.Action)
                            mPage = GWWebHelper.RootSite + ConfigSettings.AppName + mFunctionProfile.Source
                            HttpContext.Current.Response.Redirect(mPage + "?Action=" + mFunctionProfile.Action)
                        End If
                End Select
            Else
                mLog.Debug("Menu data or Logoff/Logon requested")
            End If
            processOverridePage(mFunctionProfile)
            Dim mike As String = String.Empty
        End Sub

        ''' <summary>
        ''' Ons the begin request.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        Private Sub onBeginRequest(ByVal sender As Object, ByVal e As EventArgs)
            Dim mStartLogInfo As Boolean = CBool(HttpContext.Current.Application("StartLogInfo"))
            Dim mClearedCache As Boolean = CBool(HttpContext.Current.Application("ClearedCache"))
            If Not mStartLogInfo Then
                HttpContext.Current.Application("StartLogInfo") = "True"
                Dim mLog As Logger = Logger.Instance
                mLog.SetThreshold(LogPriority.Info)
                mLog.Info("Starting Core Web Administration Version: " & GWWebHelper.CoreWebAdministrationVersion)
                Dim mCurrentLevel As String = ConfigSettings.LogPriority.ToUpper(CultureInfo.InvariantCulture)
                mLog.SetThreshold(mLog.GetLogPriorityFromText(mCurrentLevel))
            End If
            If ConfigSettings.CentralManagement Then
                If Not mClearedCache Then
                    CacheController.RemoveAllCache()
                    HttpContext.Current.Application("ClearedCache") = "True"
                End If
            End If
            If processRequest() Then
                m_Filter = New OutputFilterStream(HttpContext.Current.Response.Filter)
                HttpContext.Current.Response.Filter = m_Filter
            End If
        End Sub

        ''' <summary>
        ''' Ons the end request.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        ''' <exception cref="System.Exception"></exception>
        Private Sub onEndRequest(ByVal sender As Object, ByVal e As EventArgs)
            If processRequest() Then
                Dim mState As MClientChoicesState = CType(HttpContext.Current.Items(MClientChoices.SessionName), MClientChoicesState)
                If mState IsNot Nothing Then
                    If mState.IsDirty Then
                        ClientChoicesUtility.Save(mState)
                    End If
                End If
                Dim mContext As HttpContext = TryCast(sender, HttpApplication).Context
                Dim mSendError As Boolean = False
                Try
                    If mContext.Response.Headers("jsonerror") IsNot Nothing Then
                        Dim mError As String = String.Empty
                        If m_Filter IsNot Nothing Then
                            mError = m_Filter.ReadStream()
                            If mContext.Response.Headers("jsonerror").ToString().ToUpperInvariant().Trim() = "TRUE" Then
                                mSendError = True
                                formatError(mError)
                                Throw (New WebSupportException([String].Concat("An AJAX error has occurred: ", System.Environment.NewLine, mError)))
                            End If
                        Else
                            If mContext.Response.Headers("jsonerror").ToString().ToUpperInvariant().Trim() = "TRUE" Then
                                mSendError = True
                                Throw (New WebSupportException([String].Concat("An AJAX error has occurred: ", System.Environment.NewLine)))
                            End If
                        End If
                    End If
                Catch ex As WebSupportException
                    If mSendError Then
                        Dim mLog As Logger = Logger.Instance()
                        If Not ex.ToString().Contains("Invalid JSON primitive") Then
                            mLog.Error(ex)
                        End If
                        If Not mContext Is Nothing Then
                            Dim mCurrentResponse As HttpResponse = mContext.Response
                            If Not mCurrentResponse Is Nothing Then
                                mCurrentResponse.Clear()
                                mCurrentResponse.Write("{""Message"":""We are very sorry but an error has occurred, please try your request again.""}")
                                mCurrentResponse.ContentType = "text/html"
                                mCurrentResponse.StatusDescription = "500 Internal Error"
                                mCurrentResponse.StatusCode = 500
                                mCurrentResponse.TrySkipIisCustomErrors = True
                                mCurrentResponse.Flush()
                                HttpContext.Current.Server.ClearError()
                                HttpContext.Current.ApplicationInstance.CompleteRequest()
                            End If
                        End If
                    End If
                Finally
                    If m_Filter IsNot Nothing Then
                        m_Filter.Dispose()
                        m_Filter = Nothing
                    End If
                End Try
            End If
        End Sub

        ''' <summary>
        ''' Chars the match.
        ''' </summary>
        ''' <param name="match">The match.</param>
        ''' <returns>System.String.</returns>
        Private Function charMatch(ByVal match As Match) As String
            Dim code = match.Groups("code").Value
            Dim value As Integer = Convert.ToInt32(code, 16)
            Return (ChrW(value)).ToString()
        End Function

        ''' <summary>
        ''' Formats the error.
        ''' </summary>
        ''' <param name="message">The message.</param>
        Private Sub formatError(ByRef message As String)
            'http://stackoverflow.com/questions/6990347/how-to-decode-u0026-in-a-url
            message = message.Replace("\r\n", System.Environment.NewLine)
            message = HttpUtility.UrlDecode(message)
            message = Regex.Replace(message, "\\u(?<code>\d{4})", AddressOf charMatch)
        End Sub

        ''' <summary>
        ''' Determines if the request if one of "ASPX", "ASHX", OR "ASMX"
        ''' </summary>
        ''' <returns>boolean</returns>
        ''' <remarks>There's no need to process logic for the other file types or extension</remarks>
        Private Shared Function processRequest() As Boolean
            Dim mLog As Logger = Logger.Instance()
            mLog.Debug("Started")
            Dim mRetval As Boolean = False
            If Not HttpContext.Current Is Nothing Then
                Dim mAction = HttpContext.Current.Request.QueryString("Action")
                Dim mPath As String = HttpContext.Current.Request.Path.ToUpper(CultureInfo.InvariantCulture)
                ' this was added because for some reason the httpmodule is fireing twice
                ' the first time does begin and end but no AcquireRequestState
                ' the second time fires the begin, end, and AcquireRequestState but drops the file extention
                ' this does not happen in c#
                If Not String.IsNullOrEmpty(mAction) And mPath.IndexOf(".") = -1 Then
                    Dim mFunctionProfile As MFunctionProfile = FunctionUtility.GetProfile(mAction)
                    If Not mFunctionProfile Is Nothing Then
                        mPath = mFunctionProfile.Source.ToUpperInvariant()
                    End If
                End If
                Dim mFileExtension = mPath.Substring(mPath.LastIndexOf(".", StringComparison.OrdinalIgnoreCase) + 1)
                Dim mProcessingTypes As String() = {"ASPX", "ASHX", "ASMX", "HTM", "HTML"}
                If mProcessingTypes.Contains(mFileExtension) Or mPath.IndexOf("/API/", StringComparison.OrdinalIgnoreCase) > -1 Then
                    mRetval = True
                End If
            End If
            mLog.Debug(mRetval.ToString())
            mLog.Debug("Ended")
            Return mRetval
        End Function

        Private Shared Sub processOverridePage(ByVal functionProfile As MFunctionProfile)
            If Not functionProfile Is Nothing Then
                ' do not process API calls
                If HttpContext.Current.Request.Path.ToUpper(CultureInfo.InvariantCulture).IndexOf("/API/", StringComparison.OrdinalIgnoreCase) = -1 Then
                    Dim mLog As Logger = Logger.Instance()
                    Dim mPage As String = "/" + ConfigSettings.AppName + functionProfile.Source
                    Dim mSecProfile As MSecurityEntityProfile = SecurityEntityUtility.CurrentProfile()
                    Dim mSkinLocation As String = "/Public/Skins/" + mSecProfile.Skin + "/"
                    mPage = mPage.Replace("/", "\")
                    Dim mSystemOverridePage As String = mPage.Replace("\System\", "\Overrides\")
                    Dim mSkinOverridePage As String = mPage.Replace("\System\", mSkinLocation)
                    If File.Exists(HttpContext.Current.Server.MapPath(mSystemOverridePage)) Then
                        mLog.Debug("Transferring to system override page: " + mSystemOverridePage)
                        HttpContext.Current.Server.Execute(mSystemOverridePage, False)
                        HttpContext.Current.ApplicationInstance.CompleteRequest()
                    ElseIf File.Exists(HttpContext.Current.Server.MapPath(mSkinOverridePage)) Then
                        mLog.Debug("Transferring to skin override page: " + mSkinOverridePage)
                        HttpContext.Current.Server.Execute(mSkinOverridePage, False)
                        HttpContext.Current.ApplicationInstance.CompleteRequest()
                    End If
                End If

            End If
        End Sub

    End Class

End Namespace

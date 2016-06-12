Imports ApplicationBase.BusinessLogic
Imports ApplicationBase.BusinessLogic.ClientChoices
Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Common.Logging
Imports ApplicationBase.Common.Security
Imports ApplicationBase.Model
Imports ApplicationBase.Model.Messages
Imports ApplicationBase.Model.Accounts
Imports ApplicationBase.Model.Special.ClientChoices
Imports System.Web.Security
Imports log4net

Partial Class Logon
	Inherits ClientChoices.ClientChoicesUserControl

	Protected WithEvents imgBtnLogon As Web.UI.WebControls.ImageButton
	Private logonSuccessfull As Boolean = False

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim myCryptoUtil As New CryptoUtil
		btnLogon.Attributes.Item("onclick") = Page.ClientScript.GetPostBackEventReference(btnLogon, "").ToString() & ";"
		If Not Request.QueryString("Account") Is Nothing Then
			Dim attemptDecrypt As String = String.Empty
			' going to try to decrypt because client appears to be using a hyperlink
			Try
                If BaseSettings.enableEncryption Then
                    attemptDecrypt = myCryptoUtil.DecryptTripleDES(Request.QueryString("Account").Trim)
                    txtAccount.Text = attemptDecrypt
                    attemptDecrypt = myCryptoUtil.DecryptTripleDES(Request.QueryString("Password").Trim)
                    Password.Value = attemptDecrypt
                Else
                    txtAccount.Text = myCryptoUtil.DecryptTripleDES(Request.QueryString("Account").Trim)
                    Password.Value = Request.QueryString("Password").Trim
                End If
			Catch ex As Exception
				If ex.Message <> "Thread was being aborted." Then
					Dim log As AppLogger = AppLogger.GetInstance
					log.Warn(ex.Message.Replace("user", "account"))
					ErrorMsg.InnerText = ex.Message.Replace("user", "account")
					ErrorMsg.Visible = True
				End If
				Exit Sub
			End Try
			btnLogon_Click(sender, e)
		End If
	End Sub

	Protected Sub imgBtnLogon_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnLogon.Click
		btnLogon_Click(sender, e)
	End Sub

	Protected Sub btnLogon_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLogon.ServerClick
		Dim log As AppLogger = AppLogger.GetInstance
		Try
			Dim myAccountUtility As New AccountUtility(HttpContext.Current)
			Dim accountProfileInfo As MAccountProfileInfo = myAccountUtility.GetAccountProfileInfo(txtAccount.Text.Trim, True)
            Dim AuthenticationType As String = BaseSettings.authenticationType
            Dim LDAPDomain As String = BaseSettings.LDAPDomain
            Dim LDAPServer As String = BaseSettings.LDAPServer
			If BAccount.LoginClient(accountProfileInfo, txtAccount.Text, Password.Value, AuthenticationType, LDAPServer, LDAPDomain) Then
				accountProfileInfo = myAccountUtility.GetAccountProfileInfo(txtAccount.Text)
				If accountProfileInfo.SYSTEM_STATUS_ID = MSystemStatus.value.Disabled Then
					Dim strError As String
					strError = "Disabled Account " & accountProfileInfo.ACCOUNT & " attempted a logon when the account is disabled."
					log.Warn(strError)
					ErrorMsg.InnerText = "Invalid account or bad password."
					ErrorMsg.Visible = True
					Exit Sub
				End If
				finishLogon(Password.Value)
			Else
				accountProfileInfo = myAccountUtility.GetAccountProfileInfo(txtAccount.Text)
				If Not accountProfileInfo Is Nothing Then
					If accountProfileInfo.SYSTEM_STATUS_ID = MSystemStatus.value.Disabled Then
						Dim strError As String
						strError = "Disabled Account " & accountProfileInfo.ACCOUNT & " attempted a logon when the account is disabled."
						log.Warn(strError)
					End If
					btnRequestPasswordReset.Visible = True
					RequiredFieldValidator2.Enabled = False
				End If
				ErrorMsg.InnerText = "Invalid account or bad password."
				ErrorMsg.Visible = True
			End If
		Catch ex As Threading.ThreadAbortException
			' is ok 
		Catch ex As Exception
			If ex.Message <> "Thread was being aborted." Then
				'ErrorMsg.InnerText = ex.Message.Replace("user", "account")
				ErrorMsg.InnerText = "Invalid Account or Password"
				ErrorMsg.Visible = True
				log.Warn(ex.Message)
			End If
		End Try
	End Sub	' btnLogon_Click
	Private Sub finishLogon(ByVal Password As String)
		' Please NOTE:
		'	When using ldap authentication the application must have an entry
		'	in the TBL_ACCOUNTS table.  Authentication will be performed
		'	by LDAP and the password entered by the client will be stored in
		'	the accounts table.  This is done to allow seamless access to
		'	third party application that utilitze the same LDAP server such
		'	as Business Objects.
		Dim myCryptoUtil As New CryptoUtil
		Dim log As AppLogger = AppLogger.GetInstance
		Dim accountProfileInfo As MAccountProfileInfo
		Dim myAccountUtility As New AccountUtility(HttpContext.Current)
		accountProfileInfo = myAccountUtility.GetAccountProfileInfo(txtAccount.Text)
        If Not accountProfileInfo Is Nothing Then
            If BaseSettings.SyncPassword Then
                accountProfileInfo.PWD = myCryptoUtil.EncryptTripleDES(Password)
            End If
            accountProfileInfo.LAST_LOGIN = DateTime.Now
            Dim updateSuccessfull As Boolean = BAccount.UpdateProfile(accountProfileInfo)
            If updateSuccessfull Then
                logonSuccessfull = True
            Else
                log.Error("Unable to update the client profile")
                ErrorMsg.InnerText = "Unable to perform logon." & vbCrLf & "Please contact the system administrator."
                ErrorMsg.Visible = True
                Exit Sub
            End If
            log.Debug("Logged on " & txtAccount.Text)
            myAccountUtility.RemoveAccountInMemoryInformation()
            If Not Request.QueryString("RequestedAction") Is Nothing Then
                NavControler.NavTo(Request.QueryString("RequestedAction") & BaseSettings.getURL)
            End If
        Else
            ' The account has been authenticated but could not be located in the TBL_ACCOUNTS table
            log.Error("Account " & txtAccount.Text & " attempted a logon without an entry in the TBL_ACCOUNTS table")
            logonSuccessfull = False
            ErrorMsg.InnerText = "Account not authorized to use " & BaseSettings.appDisplayedName & "." & vbCrLf & "Please contact the system administrator."
            ErrorMsg.Visible = True
        End If
		If logonSuccessfull Then
			FormsAuthentication.SetAuthCookie(txtAccount.Text, False)
			HttpContext.Current.Session("isSession") = True
            NavControler.NavTo(BaseSettings.defaultAction)
		End If
	End Sub
	Private Sub btnRequestPasswordReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRequestPasswordReset.Click
		Dim myAccountUtility As New AccountUtility(HttpContext.Current)
		Dim myAccountProfileInfo As MAccountProfileInfo = myAccountUtility.GetAccountProfileInfo(txtAccount.Text, True)
		Dim myCryptoUtil As New CryptoUtil
		If Not myAccountProfileInfo Is Nothing Then
            Dim randomPassword As String = BaseHelperOld.GetRandomPassword
			Dim clearTextAccount As String = String.Empty
            Dim clearTextPassword As String = BaseHelperOld.GetRandomPassword
			clearTextPassword = randomPassword
			clearTextAccount = myAccountProfileInfo.ACCOUNT
			randomPassword = myCryptoUtil.EncryptTripleDES(randomPassword)

			myAccountProfileInfo.ACCOUNT = Server.UrlEncode(myCryptoUtil.EncryptTripleDES(txtAccount.Text))
			myAccountProfileInfo.PWD = Server.UrlEncode(randomPassword)
			Dim msgInfo As MMessageInfo
			msgInfo = BMessages.GetMessage("ClientRequestChange")
			NotifyUtility.SendMail(msgInfo, myAccountProfileInfo)

			myAccountProfileInfo.ACCOUNT = clearTextAccount
			myAccountProfileInfo.PWD = randomPassword
			myAccountProfileInfo.SYSTEM_STATUS_ID = MSystemStatus.value.ChangePassword
			Dim accountProfileInfoUpdateSuccessfull As Boolean = BAccount.UpdateProfile(myAccountProfileInfo)
			If accountProfileInfoUpdateSuccessfull Then			 '
				myAccountProfileInfo = myAccountUtility.GetAccountProfileInfo(myAccountProfileInfo.ACCOUNT, True)
			End If
		Else
			Dim appLogger As AppLogger = appLogger.GetInstance
			Dim ex As New Exception("Could not reset account for " & txtAccount.Text & " AccountUtility.GetProfile(txtAccount.Text) did not produce any information")
			appLogger.Error(ex)
			ErrorMsg.InnerText = "Can not reset the account"
			btnRequestPasswordReset.Visible = False
		End If
	End Sub
End Class
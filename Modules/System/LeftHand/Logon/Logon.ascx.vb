Imports BLL.Base.ClientChoices
Imports BLL.Base.SQLServer
Imports BLL.Special
Imports Common.Security.BaseSecurity
Imports DALModel.Base
Imports DALModel.Base.Messages
Imports DALModel.Special.Accounts
Imports DALModel.Special.ClientChoices
Imports System.Web.Security
Imports log4net

Public Class Logon
    Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
    Protected WithEvents Password As System.Web.UI.HtmlControls.HtmlInputText
	Protected WithEvents txtAccount As System.Web.UI.WebControls.TextBox
    Protected WithEvents ErrorMsg As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents anchorCreateAccount As System.Web.UI.HtmlControls.HtmlAnchor
    Protected WithEvents btnLogon As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents btnRequestPasswordReset As System.Web.UI.WebControls.Button
	Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
	Protected WithEvents RequiredFieldValidator2 As System.Web.UI.WebControls.RequiredFieldValidator

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

	Protected WithEvents imgBtnLogon As Web.UI.WebControls.ImageButton
	Private logonSuccessfull As Boolean = False

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim strOut As String
		Dim myCryptoUtil As New CryptoUtil
		If Not Request.QueryString("Account") Is Nothing Then
			Dim attemptDecrypt As String = String.Empty
			' going to try to decrypt because client appears to be using a hyperlink
			Try
				If Not Request.QueryString("Account") Is Nothing Then
					attemptDecrypt = myCryptoUtil.DecryptTripleDES(Request.QueryString("Account"))
					txtAccount.Text = attemptDecrypt
					attemptDecrypt = myCryptoUtil.DecryptTripleDES(Request.QueryString("Password"))
					Password.Value = attemptDecrypt
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

	Protected Sub btnLogon_Click(ByVal sender As Object, ByVal e As EventArgs)
		Dim log As AppLogger = AppLogger.GetInstance
		Try
			Dim attemptDecrypt As String
			Dim myAccountUtility As New AccountUtility(HttpContext.Current)
			Dim accountProfileInfo As MAccountProfileInfo = myAccountUtility.GetAccountProfileInfo(txtAccount.Text.Trim, True)
			Dim AuthenticationType As String = BaseHelper.AuthenticationType
			Dim LDAPDomain As String = BaseHelper.LDAPDomain
			Dim LDAPServer As String = BaseHelper.LDAPServer
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
			accountProfileInfo.PWD = myCryptoUtil.EncryptTripleDES(Password)
			accountProfileInfo.LAST_LOGIN = DateTime.Now
			If BaseHelper.SyncPassword Then
				Dim updateSuccessfull As Boolean = BAccount.UpdateProfile(accountProfileInfo)
				If updateSuccessfull Then
					logonSuccessfull = True
				Else
					log.Error("Unable to update the client profile")
					ErrorMsg.InnerText = "Unable to perform logon." & vbCrLf & "Please contact the system administrator."
					ErrorMsg.Visible = True
					Exit Sub
				End If
			Else
				logonSuccessfull = True
			End If
			log.Debug("Logged on " & txtAccount.Text)
			myAccountUtility.RemoveAccountInMemoryInformation()
			If Not Request.QueryString("RequestedAction") Is Nothing Then
				NavControler.NavTo(Request.QueryString("RequestedAction") & BaseHelper.GetURL)
			End If
		Else
			' The account has been authenticated but could not be located in the TBL_ACCOUNTS table
			log.Error("Account " & txtAccount.Text & " attempted a logon without an entry in the TBL_ACCOUNTS table")
			logonSuccessfull = False
			ErrorMsg.InnerText = "Account not authorized to use " & BaseHelper.AppDisplayedName & "." & vbCrLf & "Please contact the system administrator."
			ErrorMsg.Visible = True
		End If
		If logonSuccessfull Then
			FormsAuthentication.SetAuthCookie(txtAccount.Text, True)
			HttpContext.Current.Session("isSession") = True
			NavControler.NavTo(BaseHelper.DefaultAction)
		End If
	End Sub
	Private Sub btnRequestPasswordReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRequestPasswordReset.Click
		Dim myAccountUtility As New AccountUtility(HttpContext.Current)
		Dim myAccountProfileInfo As MAccountProfileInfo = myAccountUtility.GetAccountProfileInfo(txtAccount.Text, True)
		Dim myCryptoUtil As New CryptoUtil
		If Not myAccountProfileInfo Is Nothing Then
			Dim randomPassword As String = BaseHelper.GetRandomPassword
			Dim clearTextAccount As String = String.Empty
			Dim clearTextPassword As String = BaseHelper.GetRandomPassword
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
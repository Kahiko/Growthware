Imports Common.Security.BaseSecurity
Imports BLL.Special
Imports DALModel.Base
Imports DALModel.Special.Accounts
Imports DALModel.Special.ClientChoices
Imports BLL.Base.ClientChoices
Imports System.Resources
Imports System.Reflection

Public Class ChangePassword
    Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
    Protected WithEvents OldPassword As System.Web.UI.HtmlControls.HtmlInputText
    Protected WithEvents NewPassword As System.Web.UI.HtmlControls.HtmlInputText
    Protected WithEvents NewPassword2 As System.Web.UI.HtmlControls.HtmlInputText
    Protected WithEvents btnChangePassword As System.Web.UI.WebControls.Button
    Protected WithEvents ClientMSG As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredFieldValidator2 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredFieldValidator3 As System.Web.UI.WebControls.RequiredFieldValidator
	Protected WithEvents trOldPassword As System.Web.UI.HtmlControls.HtmlTableRow

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim myAccountUtility As New AccountUtility(HttpContext.Current)
		Dim myaccountProfileInfo As MAccountProfileInfo = myAccountUtility.GetAccountProfileInfo(HttpContext.Current.User.Identity.Name, True)
		If myaccountProfileInfo.SYSTEM_STATUS_ID = MSystemStatus.value.ChangePassword Then
			trOldPassword.Visible = False
		Else
			trOldPassword.Visible = True
		End If
	End Sub

	Private Sub btnChangePassword_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChangePassword.Click
		Dim myAccountUtility As New AccountUtility(HttpContext.Current)
		Dim myaccountProfileInfo As MAccountProfileInfo = myAccountUtility.GetAccountProfileInfo(HttpContext.Current.User.Identity.Name, True)
		Dim myCryptoUtil As New CryptoUtil
		Dim isUpdate As Boolean
		Dim retVal As Boolean
		Dim PageIsValid As Boolean = False
		Dim log As AppLogger = AppLogger.GetInstance
		PageIsValid = validateForm()
		If Not PageIsValid Then Exit Sub
		myaccountProfileInfo.PWD = myCryptoUtil.EncryptTripleDES(NewPassword.Value.Trim)
		myaccountProfileInfo.SYSTEM_STATUS_ID = MSystemStatus.value.Active
		Select Case BaseHelper.AuthenticationType.ToLower
			Case "internal"
				retVal = BAccount.UpdateProfile(myaccountProfileInfo)
				If retVal Then
					log.Debug("Changed password using internal authentication for " & myaccountProfileInfo.ACCOUNT)
					ClientMSG.InnerHtml = "Your Password has been changed."
				Else
					log.Error("Faild to change password using internal authentication for " & myaccountProfileInfo.ACCOUNT)
					ClientMSG.InnerHtml = "Password change faild."
				End If
				' update the session
				myaccountProfileInfo = myAccountUtility.GetAccountProfileInfo(myaccountProfileInfo.ACCOUNT, True)
			Case "ldap"
				' Note:
				'	Code has not been test!, but is believed to work
				Dim AuthenticationType As String = BaseHelper.AuthenticationType
				Dim LDAPDomain As String = BaseHelper.LDAPDomain
				Dim LDAPServer As String = BaseHelper.LDAPServer
				Dim objLDAPUtility As New LDAPUtility
				Try
					retVal = objLDAPUtility.SetPasswordLdap(LDAPServer, LDAPDomain, myaccountProfileInfo.ACCOUNT, OldPassword.Value.Trim, NewPassword.Value.Trim)
					If retVal Then
						log.Debug("Changed LDAP password using ldap authentication for " & myaccountProfileInfo.ACCOUNT)
						ClientMSG.InnerHtml = "Your Password has been changed."
						retVal = BAccount.UpdateProfile(myaccountProfileInfo)
						If retVal Then
							log.Debug("Changed password using ldap authentication for " & myaccountProfileInfo.ACCOUNT)
							ClientMSG.InnerHtml = "Your Password has been changed."
							' update the session
							myaccountProfileInfo = myAccountUtility.GetAccountProfileInfo(myaccountProfileInfo.ACCOUNT, True)
						Else
							log.Error("Faild to change password using ldap authentication for " & myaccountProfileInfo.ACCOUNT)
							ClientMSG.InnerHtml = "Password change faild."
						End If
					Else
						log.Error("Faild to change LDAP password using ldap authentication for " & myaccountProfileInfo.ACCOUNT)
						ClientMSG.InnerHtml = "Password change faild."
					End If
				Catch ex As Exception
					log.Error("Faild to change LDAP password using ldap authentication for " & myaccountProfileInfo.ACCOUNT)
					Throw ex
				End Try
		End Select
	End Sub

	Private Function validateForm() As Boolean
		Dim myAccountUtility As New AccountUtility(HttpContext.Current)
		Dim myaccountProfileInfo As MAccountProfileInfo = myAccountUtility.GetAccountProfileInfo(HttpContext.Current.User.Identity.Name, True)
		Dim myCryptoUtil As New CryptoUtil
		Dim retVal As Boolean = True
		If trOldPassword.Visible Then
			Try
				If (OldPassword.Value <> myCryptoUtil.DecryptTripleDES(myaccountProfileInfo.PWD)) And (OldPassword.Value <> myaccountProfileInfo.PWD) Then
					ClientMSG.InnerHtml &= "The Old password did not match the current Password<br>" & vbCrLf
					retVal = False
				End If
			Catch ex As Exception
				If ex.Message <> "Length of the data to decrypt is invalid." Then
					Throw ex
				End If
			End Try
		End If
		If NewPassword.Value.Trim.ToLower <> NewPassword2.Value.Trim.ToLower Then
			ClientMSG.InnerHtml &= "The new password does not match the Confirm Password<br>" & vbCrLf
			retVal = False
		End If
		Return retVal
	End Function

	Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
		Dim myAccountUtility As New AccountUtility(HttpContext.Current)
		Dim myaccountProfileInfo As MAccountProfileInfo = myAccountUtility.GetAccountProfileInfo(HttpContext.Current.User.Identity.Name, True)
		If myaccountProfileInfo.SYSTEM_STATUS_ID = MSystemStatus.value.ChangePassword Then
			trOldPassword.Visible = False
		Else
			trOldPassword.Visible = True
		End If
	End Sub
End Class
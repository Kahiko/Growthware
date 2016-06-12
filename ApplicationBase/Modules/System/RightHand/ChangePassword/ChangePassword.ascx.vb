Imports ApplicationBase.BusinessLogic
Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Common.Logging
Imports ApplicationBase.Common.Security
Imports ApplicationBase.Model
Imports ApplicationBase.Model.Accounts
Imports ApplicationBase.Model.Special.ClientChoices

Partial Class ChangePassword
	Inherits ClientChoices.ClientChoicesUserControl

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
		Dim retVal As Boolean
		Dim PageIsValid As Boolean = False
		Dim log As AppLogger = AppLogger.GetInstance
		PageIsValid = validateForm()
		If Not PageIsValid Then Exit Sub
		myaccountProfileInfo.PWD = myCryptoUtil.EncryptTripleDES(NewPassword.Value.Trim)
		myaccountProfileInfo.SYSTEM_STATUS_ID = MSystemStatus.value.Active
        Select Case BaseSettings.AuthenticationType.ToLower
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
                Dim AuthenticationType As String = BaseSettings.AuthenticationType
                Dim LDAPDomain As String = BaseSettings.LDAPDomain
                Dim LDAPServer As String = BaseSettings.LDAPServer
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
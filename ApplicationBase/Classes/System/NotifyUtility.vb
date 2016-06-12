Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Model.Messages
Imports ApplicationBase.Model.Messages.Notify
Imports ApplicationBase.Model.Accounts
Imports ApplicationBase.Model.Special.ClientChoices
Imports System.Net.Mail

#Region " Notes "
' The NotifyUtility Class Contains static methods for
' working with notification e-mails.
#End Region
Public Class NotifyUtility
	'*********************************************************************
	' SendMail Method
	' Sends a single email message using the MMessageInfo object.
	' as a source of 
	'*********************************************************************
	Public Shared Sub SendMail(ByVal MessageInfo As MMessageInfo, ByVal accountProfileInfo As MAccountProfileInfo, Optional ByVal From As String = "UseDefault", Optional ByVal FormatHTML As Boolean = True)
		Dim formatInfo As New MNotifyFormatInfo
		' Get the a mail client and set the Smtp server
        Dim mailClient As New SmtpClient(BaseSettings.SmtpServer)
		Dim mailSubject As String
		Dim mailBody As String
		' Set common format info properties
		formatInfo.ContentTitle = MessageInfo.Title
		mailSubject = MessageInfo.Title
		mailBody = MessageInfo.Body
		formatInfo.Username = accountProfileInfo.ACCOUNT
		formatInfo.FullUsername = accountProfileInfo.First_Name & " " & accountProfileInfo.Last_Name
		formatInfo.Password = accountProfileInfo.PWD
        formatInfo.Server = BaseSettings.rootSite
		If From = "UseDefault" Then
            From = BaseSettings.SMTPFrom
		End If
		mailSubject = FormatEmail(formatInfo, mailSubject)
		mailBody = FormatEmail(formatInfo, mailBody)
		mailClient.Send(From, accountProfileInfo.EMAIL, mailSubject, mailBody)
	End Sub	' SendMail

	'*********************************************************************
	'
	' SendMail Method overloaded
	'
	' Sends a single email.
	'
	'*********************************************************************
	Public Shared Sub SendMail(ByVal MailSubject As String, ByVal MailBody As String, ByVal Recipient As String, Optional ByVal From As String = "UseDefault", Optional ByVal FormatHTML As Boolean = True)
		' Get the a mail client and set the Smtp server
        Dim mailClient As New SmtpClient(BaseSettings.SMTPServer)
        If From = "UseDefault" Then From = BaseSettings.SMTPFrom
		mailClient.Send(From, Recipient, MailSubject, MailBody)
	End Sub

	'*********************************************************************
	'
	' FormatEmail Method
	'
	' Formats notification email. 
	'
	'*********************************************************************
	Public Shared Function FormatEmail(ByVal formatInfo As MNotifyFormatInfo, ByVal [text] As String) As String
		' Perform replacements
		[text] = [text].Replace("<Username>", formatInfo.Username)
		[text] = [text].Replace("<EditProfileLink>", formatInfo.EditProfileLink)
		[text] = [text].Replace("<Password>", formatInfo.Password)
		[text] = [text].Replace("<FullName>", formatInfo.FullUsername)
		[text] = [text].Replace("<Server>", formatInfo.Server)
		[text] = [text].Replace("<FormName>", formatInfo.FormName)
		Return [text]
	End Function	'FormatEmail
End Class 'NotifyUtility
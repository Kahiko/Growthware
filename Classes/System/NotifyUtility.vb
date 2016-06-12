Imports DALModel.Base.Messages
Imports DALModel.Base.Messages.Notify
Imports DALModel.Special.Accounts
Imports DALModel.Special.ClientChoices
Imports System.Web.Mail

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
	Public Shared Sub SendMail(ByVal MessageInfo As MMessageInfo, ByVal accountProfileInfo As MAccountProfileInfo, Optional ByVal From As String = "UseDefault", Optional ByVal Format As Mail.MailFormat = MailFormat.Text)
		Dim formatInfo As New MNotifyFormatInfo
		Dim mailSubject As String
		Dim mailBody As String
		Dim mailMessage As MailMessage = Nothing
		' Get the Smtp server
		Dim smtpServer As String = BaseHelper.SmtpServer
		' Set common format info properties
		formatInfo.ContentTitle = MessageInfo.Title

		mailSubject = MessageInfo.Title
		mailBody = MessageInfo.Body

		formatInfo.Username = accountProfileInfo.ACCOUNT
		formatInfo.FullUsername = accountProfileInfo.First_Name & " " & accountProfileInfo.Last_Name
		formatInfo.Password = accountProfileInfo.PWD
		formatInfo.Server = BaseHelper.RootSite

		mailMessage = New MailMessage
		If From = "UseDefault" Then
			mailMessage.From = BaseHelper.SMTPFrom
		Else
			mailMessage.From = From
		End If
		mailMessage.To = accountProfileInfo.EMAIL
		mailMessage.Subject = FormatEmail(formatInfo, mailSubject)
		mailMessage.Body = FormatEmail(formatInfo, mailBody)
		mailMessage.BodyFormat = Format
		SmtpMail.SmtpServer = smtpServer
		SmtpMail.Send(mailMessage)
	End Sub	' SendMail

	'*********************************************************************
	'
	' SendMail Method overloaded
	'
	' Sends a single email.
	'
	'*********************************************************************
	Public Shared Sub SendMail(ByVal MailSubject As String, ByVal MailBody As String, ByVal Recipient As String, Optional ByVal From As String = "UseDefault", Optional ByVal Format As Mail.MailFormat = MailFormat.Text)
		Dim mailMessage As MailMessage = Nothing
		' Get the Smtp server
		Dim smtpServer As String = BaseHelper.SmtpServer

		mailMessage = New MailMessage
		If From = "UseDefault" Then
			mailMessage.From = BaseHelper.SMTPFrom
		Else
			mailMessage.From = From
		End If
		mailMessage.To = Recipient
		mailMessage.Subject = MailSubject
		mailMessage.Body = MailBody
		mailMessage.BodyFormat = Format
		SmtpMail.SmtpServer = smtpServer
		SmtpMail.Send(mailMessage)

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
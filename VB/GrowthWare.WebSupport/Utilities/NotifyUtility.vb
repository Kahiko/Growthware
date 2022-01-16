Imports System.IO
Imports System.Net.Mail
Imports System.Net.Mime
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.Model.Profiles.Interfaces

Namespace Utilities
    ''' <summary>
    ''' NotifyUtility was created to handle email notifications.
    ''' </summary>
    Public Class NotifyUtility
        Const USE_DEFAULT_TEXT = "UseDefault"

        Public Shared Sub SendMail(ByVal messageProfile As IMessageProfile, ByVal accountProfile As MAccountProfile, Optional ByVal from As String = USE_DEFAULT_TEXT)
            If canSendMail() = False Then Exit Sub
            If from = USE_DEFAULT_TEXT Then
                from = ConfigSettings.SmtpFrom
            End If
            If from.Trim.Length = 0 Then
                Dim log As Logger = Logger.Instance
                log.Error("SMTP From not set in the WEB.CONFIG file")
                Exit Sub
            End If
            Dim mailClient As SmtpClient = getMailClient()
            If ConfigSettings.SmtpAccount.Trim.Length > 0 And ConfigSettings.SmtpPassword.Trim.Length > 0 Then
                Dim basicAuthenticationInfo As New System.Net.NetworkCredential(ConfigSettings.SmtpAccount.Trim, ConfigSettings.SmtpPassword.Trim)
                mailClient.Credentials = basicAuthenticationInfo
            End If
            Dim mailMessage As New MailMessage(New MailAddress(from), New MailAddress(accountProfile.Email))
            mailMessage.Subject = messageProfile.Title
            messageProfile.FormatBody()
            mailMessage.Body = messageProfile.Body
            mailMessage.IsBodyHtml = messageProfile.FormatAsHtml
            If from = "UseDefault" Then
                from = ConfigSettings.SmtpFrom
            End If
            mailClient.Send(mailMessage)
        End Sub

        Public Shared Sub SendMail(ByVal messageProfile As IMessageProfile, ByVal commaSeporatedRecipients As String, Optional ByVal from As String = USE_DEFAULT_TEXT)
            If canSendMail() = False Then Exit Sub
            If from = USE_DEFAULT_TEXT Then
                from = ConfigSettings.SmtpFrom
            End If
            Dim log As Logger = Logger.Instance
            If from.Trim.Length = 0 Then
                log.Error("SMTP From not set in the WEB.CONFIG file")
                Exit Sub
            End If
            Dim mRecipient As String = String.Empty
            Dim mRecipients As String() = commaSeporatedRecipients.Split(",")
            For Each mRecipient In mRecipients
                Dim mAccountProfile As New MAccountProfile
                mAccountProfile.Email = mRecipient
                Try
                    SendMail(messageProfile, mAccountProfile, from)
                Catch ex As Exception
                    log.Error(ex)
                End Try
            Next
        End Sub

        ''' <summary>
        ''' Sends the mail.
        ''' </summary>
        ''' <param name="body">The body.</param>
        ''' <param name="subject">The subject.</param>
        ''' <param name="formatAsHTML">The format as HTML.</param>
        ''' <param name="accountProfile">The account profile.</param>
        ''' <param name="file">The file.</param>
        ''' <param name="contentType">Type of the content.</param>
        Public Shared Sub SendMail(ByVal body As String, ByVal subject As String, ByVal formatAsHTML As Boolean, ByRef accountProfile As MAccountProfile, ByRef file As FileInfo, ByVal contentType As ContentType)
            If Not canSendMail() Then Return
            Dim mFrom As String = ConfigSettings.SmtpFrom
            If String.IsNullOrEmpty(mFrom.Trim()) Then
                Dim log As Logger = Logger.Instance()
                log.Error("SMTP From not set in the WEB.CONFIG file")
                Return
            End If
            Dim mailClient As SmtpClient = getMailClient()

            Dim mailMessage As MailMessage = New MailMessage(New MailAddress(mFrom), New MailAddress(accountProfile.Email))
            mailMessage.Subject = subject
            mailMessage.Body = body
            mailMessage.IsBodyHtml = formatAsHTML
            Dim mStream As Stream = file.OpenRead()
            Dim Attachment As Attachment = New Attachment(mStream, contentType)
            Attachment.Name = file.Name
            mailMessage.Attachments.Add(Attachment)

            mailClient.Send(mailMessage)
        End Sub

        '' <summary>
        '' Gets the notification status.
        '' </summary>
        '' <param name="functionID">The function ID.</param>
        '' <param name="securitySeqID">The security seq ID.</param>
        '' <param name="account">The account.</param>
        '' <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        Public Shared Function GetNotificationStatus(ByVal functionID As Integer, ByVal securitySeqID As Integer, ByVal account As String) As Boolean
            Dim retVal As Boolean = True
            'Dim mBll As New BNotifications(SecurityEntityUtility.GetCurrentProfile, ConfigSettings.CentralManagement)
            'retVal = mBll.GetNotificationStatus(functionID, securitySeqID, account)
            Return retVal
        End Function

        '' <summary>
        '' Saves the specified function ID.
        '' </summary>
        '' <param name="functionID">The function ID.</param>
        '' <param name="securitySeqID">The security seq ID.</param>
        '' <param name="acct">The acct.</param>
        '' <param name="status">if set to <c>true</c> [status].</param>
        Public Shared Sub Save(ByVal functionID As Integer, ByVal securitySeqID As Integer, ByVal acct As String, ByVal status As Boolean)
            'Dim mBll As New BNotifications(SecurityEntityUtility.GetCurrentProfile, ConfigSettings.CentralManagement)
            'mBll.Save(functionID, securitySeqID, acct, status)
        End Sub

        '' <summary>
        '' Sends the notifications.
        '' </summary>
        '' <param name="messageProfile">The message profile.</param>
        '' <param name="securityEntityID">The security entity ID.</param>
        '' <param name="functionSeqID">The function seq ID.</param>
        Public Shared Sub SendNotifications(ByVal messageProfile As IMessageProfile, ByVal securityEntityID As Integer, ByVal functionSeqID As Integer)
            'Dim mBll As New BNotifications(SecurityEntityUtility.GetCurrentProfile, ConfigSettings.CentralManagement)
            'Dim mDataTable As New DataTable
            'Try
            '    mDataTable = mBll.GetNotifications(securityEntityID, functionSeqID)
            'Catch ex As Exception
            '    ' consume the exception
            'End Try
            'If mDataTable.Rows.Count > 0 Then
            '    Dim mRecipients As String = String.Empty
            '    Dim mDataRow As DataRow
            '    For Each mDataRow In mDataTable.Rows
            '        mRecipients = mRecipients & mDataRow.Item(0).ToString
            '    Next
            '    SendMail(messageProfile, mRecipients)
            'End If
        End Sub

        ''' <summary>
        ''' Gets the mail client.
        ''' </summary>
        ''' <returns>SmtpClient.</returns>
        Private Shared Function getMailClient() As SmtpClient
            Dim mMailClient As SmtpClient = New SmtpClient(ConfigSettings.SmtpServer)
            If ConfigSettings.SmtpAccount.Trim.Length > 0 And ConfigSettings.SmtpPassword.Trim.Length > 0 Then
                Dim basicAuthenticationInfo As New System.Net.NetworkCredential(ConfigSettings.SmtpAccount.Trim, ConfigSettings.SmtpPassword.Trim)
                mMailClient.Credentials = basicAuthenticationInfo
            End If
            Return mMailClient
        End Function

        ''' <summary>
        ''' Determines whether this instance [can send mail].
        ''' </summary>
        ''' <returns><c>true</c> if this instance [can send mail]; otherwise, <c>false</c>.</returns>
        Private Shared Function canSendMail() As Boolean
            Dim mRetVal As Boolean = True
            If ConfigSettings.SmtpServer.Trim.Length = 0 Then
                Dim log As Logger = Logger.Instance
                log.Error("SMTP Server not set in the WEB.CONFIG file, no mail can be sent!")
                mRetVal = False
            End If
            Return mRetVal
        End Function
    End Class
End Namespace

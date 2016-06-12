using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.Framework.Model.Profiles.Interfaces;
using System;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace GrowthWare.WebSupport.Utilities
{
    /// <summary>
    /// NotifyUtility was created to handle email notifications.
    /// </summary>
    public static class NotifyUtility
    {
        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="messageProfile">Object implementing IMessageProfile</param>
        /// <param name="accountProfile">The account profile.</param>
        public static void SendMail(IMessageProfile messageProfile, MAccountProfile accountProfile)
        {
            if (!canSendMail()) return;
            string mFrom = ConfigSettings.SmtpFrom;
            if (mFrom.Trim().Length == 0)
            {
                Logger log = Logger.Instance();
                log.Error("SMTP From not set in the WEB.CONFIG file");
                return;
            }
            SmtpClient mailClient = getSmtpClient();
            MailMessage mailMessage = new MailMessage(new MailAddress(mFrom), new MailAddress(accountProfile.Email));
            mailMessage.Subject = messageProfile.Title;
            messageProfile.FormatBody();
            mailMessage.Body = messageProfile.Body;
            mailMessage.IsBodyHtml = messageProfile.FormatAsHtml;
            mailClient.Send(mailMessage);
        }

        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="formatAsHTML">if set to <c>true</c> [format as HTML].</param>
        /// <param name="accountProfile">The account profile.</param>
        /// <param name="file">The file.</param>
        /// <param name="contentType">Type of the content.</param>
        public static void SendMail(string body, string subject, bool formatAsHTML, MAccountProfile accountProfile, FileInfo file, ContentType contentType)
        {
            if (!canSendMail()) return;
            string mFrom = ConfigSettings.SmtpFrom;
            if (mFrom.Trim().Length == 0)
            {
                Logger log = Logger.Instance();
                log.Error("SMTP From not set in the WEB.CONFIG file");
                return;
            }
            SmtpClient mailClient = getSmtpClient();

            MailMessage mailMessage = new MailMessage(new MailAddress(mFrom), new MailAddress(accountProfile.Email));
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = formatAsHTML;
            Stream mStream = file.OpenRead();
            Attachment attachment = new Attachment(mStream, contentType);
            attachment.Name = file.Name;
            mailMessage.Attachments.Add(attachment);

            mailClient.Send(mailMessage);
        }

        /// <summary>
        /// Gets the SMTP client.
        /// </summary>
        /// <returns>SmtpClient.</returns>
        private static SmtpClient getSmtpClient()
        {
            SmtpClient mMailClient = new SmtpClient(ConfigSettings.SmtpServer);
            if (ConfigSettings.SmtpAccount.Trim().Length > 0 & ConfigSettings.SmtpPassword.Trim().Length > 0)
            {
                System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential(ConfigSettings.SmtpAccount.Trim(), ConfigSettings.SmtpPassword.Trim());
                mMailClient.Credentials = basicAuthenticationInfo;
            }
            return mMailClient;
        }

        /// <summary>
        /// Determines whether this instance [can send mail].
        /// </summary>
        /// <returns><c>true</c> if this instance [can send mail]; otherwise, <c>false</c>.</returns>
        private static bool canSendMail()
        {
            bool mRetVal = true;
            if (ConfigSettings.SmtpServer.Trim().Length == 0)
            {
                Logger log = Logger.Instance();
                log.Error("SMTP Server not set in the WEB.CONFIG file, no mail can be sent!");
                mRetVal = false;
            }
            return mRetVal;
        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Net.Mime;
using System.Net.Mail;
using System.Net;
using System.Threading;
using GrowthWare.Framework;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Interfaces;
using GrowthWare.Web.Support.Helpers;
using System.Threading.Tasks;

namespace GrowthWare.Web.Support.Utilities;

public static class MessageUtility
{
    private static BMessages m_BusinessLogic = null;

    private static CacheHelper m_CacheHelper = CacheHelper.Instance();

    private static string s_MessagesUnitCachedDVName = "dvMessages";

    private static string s_MessagesUnitCachedCollectionName = "_MessagesCollection";

    private static Logger m_Logger = Logger.Instance();

    private static int m_WaitFor = 250;

    /// <summary>
    /// Messages the name of the unit cached collection.
    /// </summary>
    /// <param name="securityEntityId">The security entity ID.</param>
    /// <returns>System.String.</returns>
    public static string MessagesUnitCachedCollectionName(int securityEntityId)
    {
        return securityEntityId.ToString(CultureInfo.InvariantCulture) + s_MessagesUnitCachedCollectionName;
    }

    /// <summary>
    /// Messages the name of the unit cached DV.
    /// </summary>
    /// <param name="securityEntityId">The security entity ID.</param>
    /// <returns>System.String.</returns>
    public static string MessagesUnitCachedDVName(int securityEntityId)
    {
        return securityEntityId.ToString(CultureInfo.InvariantCulture) + s_MessagesUnitCachedDVName;
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
            log.Error("SMTP Server not set in the GrowthWare.json file, no mail can be sent!");
            mRetVal = false;
        }
        return mRetVal;
    }

    /// <summary>
    /// Returns the business logic object used to access the database.
    /// </summary>
    /// <returns></returns>
    private static async Task<BMessages> getBusinessLogic()
    {
        if(m_BusinessLogic == null || ConfigSettings.CentralManagement == true)
        {
            m_BusinessLogic = new(await SecurityEntityUtility.CurrentProfile());
        }
        return m_BusinessLogic;
    }

    /// <summary>
    /// Gets the name of the message profile by.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>MMessage.</returns>
    public static async Task<MMessage> GetProfile(string name)
    {
        Collection<MMessage> mAllMessages = await Messages();
        var mResult = from mProfile in mAllMessages
                      where mProfile.Name.ToLower(CultureInfo.CurrentCulture) == name.ToLower(CultureInfo.CurrentCulture)
                      select mProfile;
        MMessage mRetVal = new MMessage();
        try
        {
            mRetVal = mResult.First();
        }
        catch (InvalidOperationException)
        {
            String mMSG = "Count not find function: " + name + " in the database";
            Logger mLog = Logger.Instance();
            mLog.Error(mMSG);
            mRetVal = null;
        }
        return mRetVal;
    }

    /// <summary>
    /// Gets the SMTP client.
    /// </summary>
    /// <returns>SmtpClient.</returns>
    private static SmtpClient getSmtpClient()
    {
        // set the common SmtpClient values
        SmtpClient mMailClient = new()
        {
            DeliveryMethod = SmtpDeliveryMethod.Network,
            EnableSsl = ConfigSettings.SmtpEnableSsl,
            Host = ConfigSettings.SmtpServer,
            Port = ConfigSettings.SmtpPort,
            Timeout = 20000,
            UseDefaultCredentials = true,
        };
        // if we have credentials, use them
        string mSmtpServer = ConfigSettings.SmtpAccount.Trim();
        CryptoUtility.TryDecrypt(ConfigSettings.SmtpPassword.Trim(), out string mPassword, ConfigSettings.EncryptionType);
        if (ConfigSettings.SmtpAccount.Trim().Length > 0 & mPassword.Length > 0)
        {
            mMailClient.UseDefaultCredentials = false;
            NetworkCredential mNetworkCredential = new(mSmtpServer, mPassword);
            mMailClient.Credentials = mNetworkCredential;
        }
        return mMailClient;
    }

    /// <summary>
    /// Gets the message profile by id.
    /// </summary>
    /// <param name="messageSeqId">The Message Sequence ID.</param>
    /// <returns>MMessage.</returns>
    public static async Task<MMessage> GetProfile(int messageSeqId)
    {
        Collection<MMessage> mAllMessages = await Messages();
        var mResult = from mProfile in mAllMessages
                      where mProfile.Id == messageSeqId
                      select mProfile;
        MMessage mRetVal = null;
        try
        {
            mRetVal = mResult.First();
        }
        catch (InvalidOperationException ex)
        {
            Logger mLog = Logger.Instance();
            mLog.Error(ex);
            mRetVal = null;
        }
        return mRetVal;
    }

    /// <summary>
    /// Gets the messages for the current security entity from cache.
    /// </summary>
    /// <returns>Collection{MMessage}.</returns>
    /// <remarks>
    /// If the messages are not in the cache, they are retrieved from the database and added to the cache.
    /// </remarks>
    public static async Task<Collection<MMessage>> Messages()
    {
        MSecurityEntity mSecurityEntity = await SecurityEntityUtility.CurrentProfile();
        int mSecurityEntityId = mSecurityEntity.Id;
        string mCacheName = MessagesUnitCachedCollectionName(mSecurityEntityId);
        Collection<MMessage> mMessageCollection = null;
        if (mMessageCollection == null)
        {
            BMessages mBusinessLogic = await getBusinessLogic();
            mMessageCollection = await mBusinessLogic.GetMessages(mSecurityEntityId);
            CacheHelper.Instance().AddToCache(mCacheName, mMessageCollection);
        }
        return mMessageCollection;
    }

    /// <summary>
    /// Removes the cached messages DV.
    /// </summary>
    public static async Task RemoveCachedMessagesDV()
    {
        MSecurityEntity mCurrentSecurityEntity = await SecurityEntityUtility.CurrentProfile();
        m_CacheHelper.RemoveFromCache(MessagesUnitCachedDVName(mCurrentSecurityEntity.Id));
    }

    /// <summary>
    /// Removes the cached messages collection.
    /// </summary>
    public static async Task RemoveCachedMessagesCollection()
    {
        // int mySecurityEntity = ClientChoicesUtility.SelectedSecurityEntity();
        // CacheHelper.RemoveFromCache(MessagesUnitCachedCollectionName(mySecurityEntity));
        await RemoveCachedMessagesDV();
    }

    /// <summary>
    /// Saves the specified profile.
    /// </summary>
    /// <param name="profile">The profile.</param>
    public static async Task<int> Save(MMessage profile)
    {
        BMessages mBusinessLogic = await getBusinessLogic();
        int mRetVal = -1;
        mRetVal = await mBusinessLogic.Save(profile);
        await RemoveCachedMessagesCollection();
        return mRetVal;
    }

    /// <summary>
    /// Sends the mail.
    /// </summary>
    /// <param name="messageProfile">Object implementing IMessageProfile</param>
    /// <param name="accountProfile">The account profile.</param>
    /// <returns>bool</returns>
    public static bool SendMail(IMessage messageProfile, MAccountProfile accountProfile)
    {
        bool mRetVal = false;
        if (!canSendMail()) return mRetVal;
        string mSmtpAccount = ConfigSettings.SmtpAccount;
        string mSmtpFrom = ConfigSettings.SmtpFrom;
        if (mSmtpAccount.Trim().Length == 0)
        {            
            m_Logger.Error("SMTP From not set in the GrowthWare.json file");
            return mRetVal;
        }
        if(String.IsNullOrWhiteSpace(mSmtpFrom))
        {
            mSmtpFrom = mSmtpAccount;
        }
        SmtpClient mSmtpClient = getSmtpClient();
        messageProfile.FormatBody();
        MailMessage mMailMessage = new()
        {
            Body = messageProfile.Body,
            From = new MailAddress(mSmtpAccount, mSmtpFrom),
            Subject = messageProfile.Title,
            IsBodyHtml = messageProfile.FormatAsHtml
        };
        mMailMessage.To.Add(new MailAddress(accountProfile.Email, accountProfile.FirstName + " " + accountProfile.LastName));
        int mNumberOfRetries = ConfigSettings.SmtpNumberOfRetries;
        for (int mNumberOfTries = 1; mNumberOfTries < mNumberOfRetries; mNumberOfTries++)
        {
            try
            {
                mSmtpClient.Send(mMailMessage);
                mNumberOfTries = mNumberOfRetries;
                mRetVal = true;
            }
            catch (System.Exception ex)
            {
                Thread.Sleep(m_WaitFor);
                m_Logger.Error(ex);
            }
        }
        return mRetVal;
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
    /// <returns>bool</returns>
    public static bool SendMail(string body, string subject, bool formatAsHTML, MAccountProfile accountProfile, FileInfo file, ContentType contentType)
    {
        bool mRetVal = false;
        if (!canSendMail()) return mRetVal;
        string mFrom = ConfigSettings.SmtpFrom;
        if (mFrom.Trim().Length == 0)
        {
            Logger log = Logger.Instance();
            log.Error("SMTP From not set in the WEB.CONFIG file");
            return mRetVal;
        }
        SmtpClient mSmtpClient = getSmtpClient();
        MailMessage mMailMessage = new(new MailAddress(mFrom), new MailAddress(accountProfile.Email))
        {
            Subject = subject,
            Body = body,
            IsBodyHtml = formatAsHTML
        };
        Stream mStream = file.OpenRead();
        Attachment attachment = new(mStream, contentType)
        {
            Name = file.Name
        };
        mMailMessage.Attachments.Add(attachment);
        int mNumberOfRetries = ConfigSettings.SmtpNumberOfRetries;
        for (int mNumberOfTries = 1; mNumberOfTries < mNumberOfRetries; mNumberOfTries++)
        {
            try
            {
                mSmtpClient.Send(mMailMessage);
                mNumberOfTries = mNumberOfRetries;
                mRetVal = true;
            }
            catch (System.Exception ex)
            {
                Thread.Sleep(m_WaitFor);
                m_Logger.Error(ex);
            }
        }
        return mRetVal;
    }
}
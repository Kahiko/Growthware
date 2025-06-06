﻿using System;
using System.IO;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using GrowthWare.Framework.Enumerations;
using System.Reflection;

namespace GrowthWare.Framework;

public class ConfigSettings
{
    private static readonly IConfiguration m_Configuration;
    private static readonly string m_ConnectionString = string.Empty;
    private static readonly string m_SettingsDirectory = string.Empty;
    private static string s_CentralManagement = string.Empty;
    private static bool m_CentralManagement;

    private static string m_Version = string.Empty;

    static ConfigSettings()
    {
        m_SettingsDirectory = findConfigDirectory();
        m_Configuration = new ConfigurationBuilder()
            .SetBasePath(m_SettingsDirectory)
            .AddJsonFile("GrowthWare.json", optional: true, reloadOnChange: true)
            // .AddEnvironmentVariables()
            //.AddCommandLine(args)
            .Build();
        String mDal = DataAccessLayer;
        if(m_ConnectionString == null || (string.IsNullOrEmpty(m_ConnectionString) || string.IsNullOrWhiteSpace(m_ConnectionString)))
        {
            m_ConnectionString = GetAppSettingValue("DAL_" + mDal + "_ConnectionString", true);
            string mDecryptedValue = string.Empty;
            CryptoUtility.TryDecrypt(m_ConnectionString, out mDecryptedValue, EncryptionType);
            m_ConnectionString = mDecryptedValue;
        }
    }        

    /// <summary>
    /// Gets or sets a value indicating whether [auto create account].
    /// </summary>
    /// <value><c>true</c> if [auto create account]; otherwise, <c>false</c>..</value>
    public static bool AutoCreateAccount
    {
        get { return Boolean.Parse(GetAppSettingValue("Auto_Create_Account", true)); }
    }
    /// <summary>
    /// Gets the automatic create StatusId.
    /// </summary>
    /// <value>The automatic create StatusId.</value>
    public static int AutoCreateAccountStatusId
    {
        get { return int.Parse(GetAppSettingValue("Auto_Create_Account_StatusId", true), CultureInfo.InvariantCulture); }
    }

    /// <summary>
    /// Returns "Anonymous"
    /// </summary>
    /// <remarks>
    /// To prevent spelling or typing mistakes
    /// </remarks>
    public static string Anonymous
    {
        get {
            return "Anonymous";
        }
    }
    /// <summary>
    /// Returns App_Displayed_Name from the GrowthWare.jsonIG file
    /// </summary>
    /// <value></value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static string AppDisplayedName
    {
        get { return GetAppSettingValue("App_Displayed_Name"); }
    }

    /// <summary>
    /// Retruns Append_To_File from the GrowthWare.jsonIG file
    /// </summary>
    /// <value></value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static string AppendToFile
    {
        get { return GetAppSettingValue("Append_To_File"); }
    }

    public static string Audience
    {
        get { return GetAppSettingValue("Audience", true); }
    }

    /// <summary>
    /// Returns Authentication_Type from the CONFIG file.
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static string AuthenticationType
    {
        get { return GetAppSettingValue("Authentication_Type", true); }
    }

    /// <summary>
    /// Returns Base_Page from the CONFIG file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static string BasePage
    {
        get { return GetAppSettingValue("Base_Page"); }
    }

    public static string Issuer
    {
        get { return GetAppSettingValue("Issuer", true); }
    }

    public static string Secret
    {
        get { return GetAppSettingValue("Secret", true); }
    }

    /// <summary>
    /// Returns Security_Entity_From_Url from the CONFIG file
    /// </summary>
    public static bool SecurityEntityFromUrl
    {
        get 
        {
            string mStringValue = GetAppSettingValue("Security_Entity_From_Url", false);
            _ = bool.TryParse(mStringValue, out bool mRetVal);
            return mRetVal;
        }
    }

    /// <summary>
    /// Returns Security_Entity_Translation from the CONFIG file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static string SecurityEntityTranslation
    {
        get { return GetAppSettingValue("Security_Entity_Translation"); }
    }

    /// <summary>
    /// Returns Central_Management from the CONFIG file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static bool CentralManagement
    {
        get { 
            if(String.IsNullOrEmpty(s_CentralManagement)) {
                s_CentralManagement = GetAppSettingValue("Central_Management");
                m_CentralManagement = bool.Parse(s_CentralManagement);
            }
            return m_CentralManagement;
        }
    }

    /// <summary>
    /// Returns the Decrypted connection string as defined in the GrowthWare.json file by environment/DAL
    /// </summary>
    /// <value>Desired data access layer "Oracle" and default "SQLServer" connection string information</value>
    /// <returns>String</returns>
    /// <remarks>The GrowthWare.json value can be encrypted or clear text</remarks>
    public static string ConnectionString
    {
        get { return m_ConnectionString; }
    }

    /// <summary>
    /// Returns the Conversion_Pattern from the CONFIG file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static string ConversionPattern
    {
        get { return GetAppSettingValue("Conversion_Pattern"); }
    }

    /// <summary>
    /// Returns DAL from the CONFIG file, and is generaly the name of the 
    /// datastore being used IE SQLServer or Oracle
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static string DataAccessLayer
    {
        get { return GetAppSettingValue("DAL", true); }
    }

    /// <summary>
    /// Returns the name of the data access layer assembly name as defined in the GrowthWare.json file by environment
    /// </summary>
    /// <value>Desired data access layer "Oracle" and default "SQLServer"</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static string DataAccessLayerAssemblyName
    {
        get { return GetAppSettingValue("DAL_" + DataAccessLayer + "_Assembly_Name", true); }
    }

    /// <summary>
    /// Returns the DAL_SQLServer_Name_Space from the CONFIG file
    /// </summary>
    /// <param name="dataAccessLayer"></param>
    /// <value></value>
    /// <returns>DataAccessLayerNamespace using SQLServer</returns>
    /// <remarks></remarks>
    public static string DataAccessLayerNamespace
    {
        get { return GetAppSettingValue("DAL_" + DataAccessLayer + "_Name_Space", true); }
    }

    /// <summary>
    /// Returns Default_Action from the CONFIG file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static string DefaultAction
    {
        get { return GetAppSettingValue("Default_Action"); }
    }

    /// <summary>
    /// Returns Default_Authenticated_Action from the CONFIG file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static string DefaultAuthenticatedAction
    {
        get { return GetAppSettingValue("Default_Authenticated_Action"); }
    }

    /// <summary>
    /// Returns Default_Security_Entity_ID from the CONFIG file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static int DefaultSecurityEntityID
    {
        get { return int.Parse(GetAppSettingValue("Default_Security_Entity_ID", true), CultureInfo.InvariantCulture); }
    }

    /// <summary>
    /// Returns Expected_Up_By from the CONFIG file
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
    public static string ExpectedUpBy
    {
        get { return GetAppSettingValue("Expected_Up_By"); }
    }

    /// <summary>
    /// Returns Enable_Cache from the CONFIG file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static bool EnableCache
    {
        get { return bool.Parse(GetAppSettingValue("Enable_Cache", true)); }
    }

    /// <summary>
    /// Retruns Enable_Pooling from the CONFIF file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks>For future use</remarks>
    public static string EnablePooling
    {
        get { return GetAppSettingValue("Enable_Pooling", true); }
    }

    /// <summary>
    /// Returns Environment from the CONFIG file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks>Used as a prefix to appSettings, allows multipule environment settings to co-exist in one CONFIG file.</remarks>
    public static string Environment
    {
        get { return GetAppSettingValue("Environment") + "_"; }
    }

    /// <summary>
    /// Returns Environments from the CONFIG file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks>Comma seportated "list" of environments </remarks>
    public static string Environments
    {
        get { return GetAppSettingValue("Environments"); }
    }

    /// <summary>
    /// Returns Environment without the leading underscore
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static string EnvironmentDisplayed
    {
        get
        {
            if (Environment.Substring(Environment.Length - 1, 1) == "_")
            {
                return Environment.Substring(0, Environment.Length - 1);
            }
            else
            {
                return Environment;
            }
        }
    }

    /// <summary>
    /// Returns Encryption_Type from the CONFIG file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static EncryptionType EncryptionType
    {
        get
        {
            return (EncryptionType)int.Parse(GetAppSettingValue("Encryption_Type"), CultureInfo.InvariantCulture);
        }
    }

    /// <summary>
    /// Returns Encryption_SaltExpression from the CONFIG file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static String EncryptionSaltExpression
    {
        get
        {
            return GetAppSettingValue("Encryption_SaltExpression");
        }
    }
    /// <summary>
    /// Returns Failed_Attempts from the CONFIG file
    /// </summary>
    /// <value>int</value>
    /// <returns>int</returns>
    /// <remarks></remarks>
    public static int FailedAttempts
    {
        get { return int.Parse(GetAppSettingValue("Failed_Attempts", true), CultureInfo.InvariantCulture); }
    }

    private static string findConfigDirectory()
    {
        string mCurrentDirectory = Directory.GetCurrentDirectory();
        while (true)
        {
            string mJsonFilePath = Path.Combine(mCurrentDirectory, "GrowthWare.json");
            if (File.Exists(mJsonFilePath))
            {
                return mCurrentDirectory;
            }
            string mParentDirectory = Directory.GetParent(mCurrentDirectory).FullName;
            if (mParentDirectory == mCurrentDirectory)
            {
                // We've reached the root directory, so stop searching
                break;
            }
            mCurrentDirectory = mParentDirectory;
        }
        throw new FileNotFoundException("GrowthWare.json file not found");
    }

    /// <summary>
    /// Returns Force_HTTPS from the CONFIG file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static bool ForceHttps
    {
        get { return Convert.ToBoolean(GetAppSettingValue("Force_HTTPS", true), CultureInfo.InvariantCulture); }
    }

    /// <summary>
    /// Returns App_Name from the CONFIG file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static string AppName
    {
        get { return GetAppSettingValue("App_Name", true); }
    }

    public static string Actions_EditAccount
    {
        get { return GetAppSettingValue("Actions_EditAccount", true); }
    }

    public static string Actions_EditGroups
    {
        get { return GetAppSettingValue("Actions_EditGroups", true); }
    }

    public static string Actions_EditNameValueParent
    {
        get { return GetAppSettingValue("Actions_EditNameValueParent", true); }
    }

    public static string Actions_EditMessages
    {
        get { return GetAppSettingValue("Actions_EditMessages", true); }
    }

    public static string Actions_EditRoles
    {
        get { return GetAppSettingValue("Actions_EditRoles", true); }
    }

    public static string Actions_EditSecurityEntities
    {
        get { return GetAppSettingValue("Actions_EditSecurityEntity", true); }
    }

    public static string Actions_EditFeedback
    {
        get { return GetAppSettingValue("Actions_EditFeedback", true); }
    }

    /// <summary>
    /// Returns DB_Status from the CONFIG file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static string DBStatus
    {
        get { return GetAppSettingValue("DB_Status"); }
    }

    public static string PathToPDBFiles
    {
        get { return GetAppSettingValue("DAL_" + DataAccessLayer + "_Path_To_DB_Files", true); }
    }

    /// <summary>
    /// Returns RegistrationDefaultGroups from the CONFIG file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static string RegistrationDefaultGroups
    {
        get { return GetAppSettingValue("RegistrationDefaultGroups", true); }
    }

    /// <summary>
    /// Returns RegistrationDefaultRoles from the CONFIG file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static string RegistrationDefaultRoles
    {
        get { return GetAppSettingValue("RegistrationDefaultRoles", true); }
    }

    /// <summary>
    /// Return RegistrationPassword from the COFNIG file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks>String</remarks>
    public static string RegistrationPassword 
    {
        get { return GetAppSettingValue("RegistrationPassword", true); }
    }

    /// <summary>
    /// Retrun AppDisplayedName and Remember_Me from the CONFIG file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static string RememberCookieName
    {
        get { return AppDisplayedName + "Remember_Me"; }
    }

    /// <summary>
    /// Return RequestBodySize_N_Bytes from the CONFIG file and is used as the "chunk" size when 
    /// uploading files.
    /// </summary>
    public static int RequestBodySize
    {
        get { return int.Parse(GetAppSettingValue("RequestBodySize_N_Bytes", true), CultureInfo.InvariantCulture); }
    }

    /// <summary>
    /// Return JWT_Refresh_Cookie_TTL_Days from the CONFIG file
    /// </summary>
    /// <remarks>
    /// Used to determine when the refresh cookie expires. (See AbstractAccountController.setTokenCookie)
    /// </remarks>
    public static int JWT_Refresh_Cookie_TTL_Days
    {
        get { return int.Parse(GetAppSettingValue("JWT_Refresh_Cookie_TTL_Days", true), CultureInfo.InvariantCulture); }
    }

    /// <summary>
    /// Return JWT_Refresh_CookieName from the CONFIG file
    /// </summary>
    /// <remarks>
    /// Provides the name of the refresh cookie name. (See AbstractAccountController.setTokenCookie)
    /// The cookie name will also be used in the UI!
    /// </remarks>
    public static string JWT_Refresh_CookieName
    {
        get { return GetAppSettingValue("JWT_Refresh_CookieName", true); }
    }

    /// <summary>
    /// Return JWT_Refresh_Token_DB_TTL_Days from the CONFIG file
    /// </summary>
    /// <remarks>
    /// Used to determine how long a refresh token is kept in the datastore.
    /// (See AccountUtility.removeOldRefreshTokens)
    /// </remarks>
    public static int JWT_Refresh_Token_DB_TTL_Days
    {
        get { return int.Parse(GetAppSettingValue("JWT_Refresh_Token_DB_TTL_Days", true), CultureInfo.InvariantCulture); }
    }

    /// <summary>
    /// Return JWT_Refresh_Token_Expires_Days from the CONFIG file
    /// </summary>
    /// <remarks>
    /// Determines how long a refresh token is valid for. The effect is that the account will be
    /// logged on again after this many days so long as the refresh token is still valid.
    /// (See: JwtUtility.GenerateRefreshToken)
    /// </remarks>
    public static int JWT_Refresh_Token_Expires_Days
    {
        get { return int.Parse(GetAppSettingValue("JWT_Refresh_Token_Expires_Days", true), CultureInfo.InvariantCulture); }
    }

    /// <summary>
    /// Return JWT_Token_TTL_Minutes from the CONFIG file
    /// </summary>
    /// <remarks>
    /// Used to determine how long the JWT is valid for.
    /// (See: JwtUtility.GenerateJwtToken)
    /// </remarks>
    public static int JWT_Token_TTL_Minutes
    {
        get { return int.Parse(GetAppSettingValue("JWT_Token_TTL_Minutes", true), CultureInfo.InvariantCulture); }
    }

    /// <summary>
    /// Return LDAP_Domain from the CONFIG file
    /// </summary>
    /// <value></value>
    /// <returns>String</returns>
    /// <remarks>String</remarks>
    public static string LdapDomain
    {
        get { return GetAppSettingValue("LDAP_Domain", true); }
    }

    /// <summary>
    /// Returns LDAP_Server from the CONFIG file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static string LdapServer
    {
        get { return GetAppSettingValue("LDAP_Server", true); }
    }

    /// <summary>
    /// Returns Log_Path from the CONFIG file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static string LogPath
    {
        get { return GetAppSettingValue("Log_Path", true); }
    }

    /// <summary>
    /// Returns Log_Priority from the CONFIG file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static string LogPriority
    {
        get { return GetAppSettingValue("Log_Priority", true); }
    }

    /// <summary>
    /// Returns Log_Retention from the CONFIG file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static string LogRetention
    {
        get { return GetAppSettingValue("Log_Retention", true); }
    }

    /// <summary>
    /// Returns Server_Side_View_State from the CONFIG file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static bool ServerSideViewState
    {
        get { return bool.Parse(GetAppSettingValue("Server_Side_View_State")); }
    }

    /// <summary>
    /// Returns Server_Side_View_State_Pages from the CONFIG file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static int ServerSideViewStatePages
    {
        get { return int.Parse(GetAppSettingValue("Server_Side_View_State_Pages"), CultureInfo.InvariantCulture); }
    }

    /// <summary>
    /// Sets the environment value.
    /// </summary>
    /// <param name="config">The config.</param>
    /// <param name="isNew">if set to <c>true</c> [is new].</param>
    /// <param name="configName">Name of the config.</param>
    /// <param name="configValue">The config value.</param>
    /// <param name="deleteEnvironment">if set to <c>true</c> [delete environment].</param>
    public static void SetEnvironmentValue(bool isNew, string configName, string configValue, bool deleteEnvironment)
    {
        // if (config == null) throw new ArgumentNullException("config", "config cannot be a null reference (Nothing in Visual Basic)!");
        // if (string.IsNullOrEmpty(configName)) throw new ArgumentNullException("configName", "configName cannot be a null reference (Nothing in Visual Basic)!");
        // if (string.IsNullOrEmpty(configName)) throw new ArgumentNullException("configValue", "configValue cannot be a null reference (Nothing in Visual Basic)!");

        // if (!deleteEnvironment)
        // {
        //     if (!isNew)
        //     {
        //         try
        //         {
        //             System.Configuration.KeyValueConfigurationElement configSetting = config.AppSettings.Settings[configName];
        //             configSetting.Value = configValue;
        //         }
        //         catch
        //         {
        //             config.AppSettings.Settings.Add(configName, configValue);
        //         }
        //     }
        //     else
        //     {
        //         config.AppSettings.Settings.Add(configName, configValue);
        //     }
        // }
        // else
        // {
        //     config.AppSettings.Settings.Remove(configName);
        // }
        // config.Save();
    }

    /// <summary>
    /// Returns decrypted SMTP_Account from the CONFIG file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static string SmtpAccount
    {
        get
        {
            try
            {
                // return CryptoUtility.decryptDES(GetAppSettingValue("SMTP_Account", true), EncryptionType);
                string mRetVal = GetAppSettingValue("SMTP_Account", true);
                CryptoUtility.TryDecrypt(mRetVal, out mRetVal, EncryptionType);
                return mRetVal;
            }
            catch (CryptoUtilityException)
            {
                return GetAppSettingValue("SMTP_Account", true);
            }
        }
    }

    /// <summary>
    /// Returns SMTP_From from the CONFIG file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static string SmtpFrom
    {
        get { return GetAppSettingValue("SMTP_From", true); }
    }

    public static int SmtpNumberOfRetries
    {
        get
        {
            bool mParseInt = int.TryParse(GetAppSettingValue("SMTP_NumberOfRetries", true), out int mRetVal);
            // Assign a default value
            if (!mParseInt) mRetVal = 1;
            return mRetVal;            
        }
    }

    /// <summary>
    /// Returns SMTP_Server from the CONFIG file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static string SmtpServer
    {
        get { return GetAppSettingValue("SMTP_Server", true); }
    }

    /// <summary>
    /// Returns config setting for Strip_Domain_From_Http_Context_UserName.
    /// </summary>
    /// <returns>Boolean.</returns>
    public static Boolean StripDomainFromHttpContextUserName
    {
        get {
            Boolean mRetVal = false;
            try
            {
                mRetVal = bool.Parse(GetAppSettingValue("Strip_Domain_From_Http_Context_UserName"));
            }
            catch(Exception)
            {
                throw;
            }
            return mRetVal;
        }
    }

    /// <summary>
    /// Returns SMTP_Password from CONFIG file
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
    public static string SmtpPassword
    {
        get
        {
            try
            {
                // return CryptoUtility.decryptDES(GetAppSettingValue("SMTP_Password",true), EncryptionType);
                string mRetVal = GetAppSettingValue("SMTP_Password", true);
                CryptoUtility.TryDecrypt(mRetVal, out mRetVal, EncryptionType);
                return mRetVal;
            }
            catch (CryptoUtilityException)
            {
                return GetAppSettingValue("SMTP_Password", true);
            }
        }
    }

    /// <summary>
    /// Returns XX_SMTP_EnableSsl from the CONFIG file or a default of 25
    /// </summary>
    public static int SmtpPort
    {
        get 
        {
            bool mParseInt = int.TryParse(GetAppSettingValue("SMTP_Port", true), out int mRetVal);
            if (!mParseInt) mRetVal = 25;
            return mRetVal;
        }
    }

    /// <summary>
    /// Returns SMTP_Domain from the CONFIG file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static string SmtpDomain
    {
        get { return GetAppSettingValue("SMTP_Domain", true); }
    }

    /// <summary>
    /// Returns SMTP_EnableSsl from the CONFIG file
    /// </summary>
    public static bool SmtpEnableSsl
    {
        get
        {
            return bool.Parse(GetAppSettingValue("SMTP_EnableSsl", true));
        }
    }

    /// <summary>
    /// Returns Synchronize_Password from the CONFIG file
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
    public static string SyncPassword
    {
        get { return bool.Parse(GetAppSettingValue("Synchronize_Password", true)).ToString(); }
    }

    /// <summary>
    /// Returns SKIN_TYPE from the CONFIG file
    /// </summary>
    /// <value>String</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static string SkinType
    {
        get { return GetAppSettingValue("SKIN_TYPE", true); }
    }

    /// <summary>
    /// Returns Under_Construction from the CONFIG file
    /// </summary>
    /// <value>Stirng</value>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static bool UnderConstruction
    {
        get { return bool.Parse(GetAppSettingValue("Under_Construction")); }
    }

    public static string Version
    {
        /*
            * To update the Version edit the GrowthWare.Framework.csproj
            */
        get 
        { 
            if (string.IsNullOrWhiteSpace(m_Version)) 
            {
                m_Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(); 
            }
            return m_Version;
        }
    }

    public static string View_Account_Group_Tab
    {
            get { return GetAppSettingValue("Actions_View_Account_Group_Tab", true); }
    }

    public static string View_Account_Role_Tab
    {
            get { return GetAppSettingValue("Actions_View_Account_Role_Tab", true); }
    }

    public static string View_Function_Group_Tab
    {
            get { return GetAppSettingValue("Actions_View_Function_Group_Tab", true); }
    }

    public static string View_Function_Role_Tab
    {
            get { return GetAppSettingValue("Actions_View_Function_Role_Tab", true); }
    }

    /// <summary>
    /// Returns Assembly from the web config and used to load the correct assembly name
    /// </summary>
    public static string WebAssemblyName
    {
        get { return GetAppSettingValue("Assembly", false); }
    }


    /// <summary>
    /// Truncates a given string and adds ...
    /// </summary>
    /// <param name="text">String to be truncated</param>
    /// <param name="length">Point at which the texted is truncated</param>
    /// <returns>String</returns>
    /// <remarks></remarks>
    public static string TruncateWithEllipsis(string text, int length)
    {
        if (!string.IsNullOrEmpty(text)) 
        {
            if (text.Length > length)
            {
                text = text.Substring(0, length) + "...";
            }
        }
        return text;
    }


    /// <summary>
    /// Return a value from the configuration file appsettings section
    /// </summary>
    /// <param name="settingName"></param>
    /// <param name="fromEnvironment"></param>
    /// <returns>String</returns>
    public static string GetAppSettingValue(string settingName, Boolean fromEnvironment)
    {
        string mRetVal = string.Empty;
        try
        {
            if (fromEnvironment)
            {
                mRetVal = m_Configuration.GetSection("AppSettings")[Environment + settingName].ToString();
            }
            else
            {
                mRetVal = m_Configuration.GetSection("AppSettings")[settingName].ToString();
            }                
        }
        catch (System.Exception)
        {
            string mMsg = "Could not find entry for {0} (fromEnvironment: '{1}') in file '{3}'";
            throw new Exception(String.Format(mMsg, settingName, fromEnvironment, m_SettingsDirectory + @"\GrowthWare.json"));
        }
        return mRetVal;
    }

    /// <summary>
    /// Return a value from the configuration file appsettings section
    /// </summary>
    /// <param name="settingName"></param>
    /// <returns>String</returns>
    /// <remarks>Overloaded method calls GetAppSettingValue(string settingName, Boolean fromEnvironment) passing false for fromEnvironment</remarks>
    public static string GetAppSettingValue(string settingName)
    {
        return GetAppSettingValue(settingName, false);
    }

}

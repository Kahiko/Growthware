using System;
using System.IO;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using GrowthWare.Framework.Enumerations;

namespace GrowthWare.Framework
{
    public class ConfigSettings
    {
        private static readonly IConfiguration m_Configuration;
        private static readonly string m_ConnectionString = string.Empty;
        private static readonly string m_SettingsDirectory = string.Empty;
        private static string s_CentralManagement = string.Empty;
        private static bool m_CentralManagement;

        static ConfigSettings()
        {
            m_SettingsDirectory = Directory.GetCurrentDirectory();
#if(DEBUG)
               // In Debug mode - use parent directory.
                m_SettingsDirectory = Directory.GetParent(m_SettingsDirectory).ToString();
                // Console.WriteLine("m_SettingsDirectory: " + m_SettingsDirectory);
#else
                // Console.WriteLine("m_SettingsDirectory: " + m_SettingsDirectory);
#endif
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
                try
                {
                    CryptoUtility.Decrypt(m_ConnectionString, EncryptionType);
                }
                catch (CryptoUtilityException)
                {
                    // do nothing atm the values is more than likely clear text
                }
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
        /// Returns App_Displayed_Name from the WEB.CONFIG file
        /// </summary>
        /// <value></value>
        /// <returns>String</returns>
        /// <remarks></remarks>
        public static string AppDisplayedName
        {
            get { return GetAppSettingValue("App_Displayed_Name"); }
        }

        /// <summary>
        /// Retruns Append_To_File from the WEB.CONFIG file
        /// </summary>
        /// <value></value>
        /// <returns>String</returns>
        /// <remarks></remarks>
        public static string AppendToFile
        {
            get { return GetAppSettingValue("Append_To_File"); }
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

        public static string Secret
        {
            get { return GetAppSettingValue("Secret", true); }
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
        /// Returns the connection string as defined in the web.conf file by environment/DAL
        /// </summary>
        /// <value>Desired data access layer "Oracle" and defaul "SQLServer" connection string information</value>
        /// <returns>String</returns>
        /// <remarks>The web.conf value can be encrypted or clear text</remarks>
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
        /// Returns the name of the data access layer assembly name as defined in the web.conf file by environment
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
        /// <value>String</value>
        /// <returns>String</returns>
        /// <remarks></remarks>
        public static int FailedAttempts
        {
            get { return int.Parse(GetAppSettingValue("Failed_Attempts", true), CultureInfo.InvariantCulture); }
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

        /// <summary>
        /// Gets the RefreshTokenTTL value from Growthware.json for the given environment.
        /// </summary>
        /// <value></value>
        public static int RefreshTokenTTL
        {
            get { return int.Parse(GetAppSettingValue("RefreshTokenTTL", true));}
        }

        /// <summary>
        /// Gets or sets a value indicating the account to use for the client choices.
        /// </summary>
        /// <value>string value from config file</value>
        /// <returns>string</returns>
        public static string RegistrationAccountChoicesAccount
        {
            get { return GetAppSettingValue("RegistrationAccountChoicesAccount", true); }
        }

        /// <summary>
        /// Returns RegistrationGroups from the CONFIG file
        /// </summary>
        /// <value>String</value>
        /// <returns>String</returns>
        /// <remarks></remarks>
        public static string RegistrationGroups
        {
            get { return GetAppSettingValue("RegistrationGroups", true); }
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
        /// Gets the registration create roles.
        /// </summary>
        /// <value>The registration roles.</value>
        public static string RegistrationRoles
        {
            get { return GetAppSettingValue("RegistrationRoles", true); }
        }

        /// <summary>
        /// Gets the RegistrationStatusIds account.
        /// </summary>
        /// <value>The RegistrationStatusIds.</value>
        public static string RegistrationStatusId 
        {
            get { return GetAppSettingValue("RegistrationStatusId", true); }
        }

        /// <summary>
        /// Gets the RegistrationSecurityEntityID.
        /// </summary>
        /// <value>The RegistrationSecurityEntityID.</value>
        public static int RegistrationSecurityEntityID
        {
            get { return int.Parse(GetAppSettingValue("RegistrationSecurityEntityID", true), CultureInfo.InvariantCulture); }
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
                    return CryptoUtility.Decrypt(GetAppSettingValue("SMTP_Account", true), EncryptionType);
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
                    return CryptoUtility.Decrypt(GetAppSettingValue("SMTP_Password",true), EncryptionType);
                }
                catch (CryptoUtilityException)
                {
                    return GetAppSettingValue("SMTP_Password", true);
                }
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
            get { return GetAppSettingValue("Version"); }
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
            if (fromEnvironment)
            {
                mRetVal = m_Configuration.GetSection("AppSettings")[Environment + settingName].ToString();
            }
            else
            {
                mRetVal = m_Configuration.GetSection("AppSettings")[settingName].ToString();
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
}

using GrowthWare.Framework.Model.Enumerations;
using System;
using System.Configuration;
using System.Globalization;

namespace GrowthWare.Framework.Common
{
    /// <summary>
    /// Servers as a collection of configuration information
    /// </summary>
    static public class ConfigSettings
    {


        /// <summary>
        /// Gets the automatic create security entity.
        /// </summary>
        /// <value>The automatic create security entity.</value>
        public static string AutoCreateSecurityEntity
        {
            get { return GetAppSettingValue("Auto_Create_SecurityEntity", true); }
        }

        /// <summary>
        /// Retruns App_Displayed_Name from the WEB.CONFIG file
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
            get { return bool.Parse(GetAppSettingValue("Central_Management")); }
        }

        /// <summary>
        /// Returns the connection string as defined in the web.conf file by environment/DAL
        /// </summary>
        /// <param name="dataAccessLayer">String</param>
        /// <value>Desired data access layer "Oracle" and defaul "SQLServer" connection string information</value>
        /// <returns>String</returns>
        /// <remarks>The web.conf value can be encrypted or clear text</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public static string ConnectionString(String dataAccessLayer)
        {
            String retVal = GetAppSettingValue("DAL_" + dataAccessLayer + "_Connectionstring", true);
            try
            {
                retVal = CryptoUtility.Decrypt(retVal, EncryptionType);
            }
            catch (CryptoUtilityException ex)
            {
                Logger mLog = Logger.Instance();
                mLog.Info(ex);
            }
            return retVal;
        }

        /// <summary>
        /// Returns the connection string as defined in the web.conf file by environment/DAL
        /// </summary>
        /// <returns>String</returns>
        /// <remarks>The web.conf value can be encrypted or clear text</remarks>
        public static string ConnectionString()
        {
            return ConnectionString("SQLServer");
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
        /// <param name="dataAccessLayer">String</param>
        /// <value>Desired data access layer "Oracle" and default "SQLServer"</value>
        /// <returns>String</returns>
        /// <remarks></remarks>
        public static string DataAccessLayerAssemblyName(string dataAccessLayer)
        {
            return GetAppSettingValue("DAL_" + dataAccessLayer + "_Assembly_Name", true);
        }

        /// <summary>
        /// Returns the DAL_SQLServer_Name_Space from the CONFIG file
        /// </summary>
        /// <param name="dataAccessLayer"></param>
        /// <value></value>
        /// <returns>DataAccessLayerNamespace using SQLServer</returns>
        /// <remarks></remarks>
        public static string DataAccessLayerNamespace(string dataAccessLayer)
        {
            return GetAppSettingValue("DAL_" + dataAccessLayer + "_Name_Space", true);
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
        public static int DefaultSecurityEntityId
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
        /// Return RegistrationPostAction from the COFNIG file
        /// </summary>
        /// <value>String</value>
        /// <returns>String</returns>
        /// <remarks>String</remarks>
        public static string RegistrationPostAction
        {
            get { return GetAppSettingValue("RegistrationPostAction", true); }
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
        /// Gets the RegistrationSecurityEntityId.
        /// </summary>
        /// <value>The RegistrationSecurityEntityId.</value>
        public static string RegistrationSecurityEntityId
        {
            get { return GetAppSettingValue("RegistrationSecurityEntityId", true); }
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void SetEnvironmentValue(Configuration config, bool isNew, string configName, string configValue, bool deleteEnvironment)
        {
            if (config == null) throw new ArgumentNullException("config", "config cannot be a null reference (Nothing in Visual Basic)!");
            if (string.IsNullOrEmpty(configName)) throw new ArgumentNullException("configName", "configName cannot be a null reference (Nothing in Visual Basic)!");
            if (string.IsNullOrEmpty(configName)) throw new ArgumentNullException("configValue", "configValue cannot be a null reference (Nothing in Visual Basic)!");

            if (!deleteEnvironment)
            {
                if (!isNew)
                {
                    try
                    {
                        System.Configuration.KeyValueConfigurationElement configSetting = config.AppSettings.Settings[configName];
                        configSetting.Value = configValue;
                    }
                    catch
                    {
                        config.AppSettings.Settings.Add(configName, configValue);
                    }
                }
                else
                {
                    config.AppSettings.Settings.Add(configName, configValue);
                }
            }
            else
            {
                config.AppSettings.Settings.Remove(configName);
            }
            config.Save();
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
        public static Boolean StripDomainFromHttpContextUserName()
        {
            Boolean mRetVal = false;
            try
            {
                mRetVal = bool.Parse(GetAppSettingValue("Strip_Domain_From_Http_Context_UserName"));
            }
            catch(Exception ex)
            {
                Logger mLog = Logger.Instance();
                mLog.Error(ex);
                throw;
            }
            return mRetVal;
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
                catch (CryptoUtilityException ex)
                {
                    Logger mLog = Logger.Instance();
                    mLog.Debug(ex);
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
            if (fromEnvironment) 
            {
                return ConfigurationManager.AppSettings[Environment + settingName];
            } 
            else 
            {
                return ConfigurationManager.AppSettings[settingName];
            }
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

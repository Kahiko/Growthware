Imports System.Configuration
Imports System.Web
Imports System.Text
Imports System.Text.RegularExpressions

Namespace Common
    ''' <summary>
    ''' Servers as a collection of configuration information
    ''' </summary>
    Public Module ConfigSettings

        ''' <summary>
        ''' Returns App_Displayed_Name from the CONFIG file
        ''' </summary>
        ''' <value></value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property AppDisplayedName() As String
            Get
                Return GetAppSettingValue("App_Displayed_Name")
            End Get
        End Property

        ''' <summary>
        ''' Retruns Append_To_File from the CONFIG file
        ''' </summary>
        ''' <value></value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property AppendToFile() As String
            Get
                Return GetAppSettingValue("Append_To_File")
            End Get
        End Property

        ''' <summary>
        ''' Returns App_Name from the CONFIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property AppName() As String
            Get
                Return GetAppSettingValue("App_Name", True)
            End Get
        End Property

        ''' <summary>
        ''' Returns Authentication_Type from the CONFIG file.
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property AuthenticationType() As String
            Get
                Return GetAppSettingValue("Authentication_Type", True)
            End Get
        End Property

        ''' <summary>
        ''' Returns Base_Page from the CONFIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property BasePage() As String
            Get
                Return GetAppSettingValue("Base_Page")
            End Get
        End Property

        ''' <summary>
        ''' Returns Security_Entity_Translation from the CONFIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property SecurityEntityTranslation() As String
            Get
                Return GetAppSettingValue("Security_Entity_Translation")
            End Get
        End Property

        ''' <summary>
        ''' Returns Central_Management from the CONFIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property CentralManagement() As Boolean
            Get
                Return CBool(GetAppSettingValue("Central_Management"))
            End Get
        End Property

        ''' <summary>
        ''' Sets the environment value.
        ''' </summary>
        ''' <param name="config">The config.</param>
        ''' <param name="isNew">if set to <c>true</c> [is new].</param>
        ''' <param name="configName">Name of the config.</param>
        ''' <param name="configValue">The config value.</param>
        ''' <param name="deleteEnvironment">if set to <c>true</c> [delete environment].</param>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
        Sub SetEnvironmentValue(ByVal config As Configuration, ByVal isNew As Boolean, ByVal configName As String, ByVal configValue As String, ByVal deleteEnvironment As Boolean)
            If config Is Nothing Then Throw New ArgumentNullException("config", "config cannot be a null reference (Nothing in Visual Basic)!")
            If String.IsNullOrEmpty(configName) Then Throw New ArgumentNullException("configName", "configName cannot be a null reference (Nothing in Visual Basic)!")
            If String.IsNullOrEmpty(configValue) Then Throw New ArgumentNullException("configValue", "configValue cannot be a null reference (Nothing in Visual Basic)!")
            If Not deleteEnvironment Then
                If Not isNew Then
                    Try
                        Dim configSetting As System.Configuration.KeyValueConfigurationElement = config.AppSettings.Settings.Item(configName)
                        configSetting.Value = configValue
                    Catch ex As Exception
                        config.AppSettings.Settings.Add(configName, configValue)
                    End Try
                Else
                    config.AppSettings.Settings.Add(configName, configValue)
                End If
            Else
                config.AppSettings.Settings.Remove(configName)
            End If
            config.Save()
        End Sub

        ''' <summary>
        ''' Returns the connection string as defined in the CONFIG file by environment/DAL
        ''' </summary>
        ''' <param name="dataAccessLayer">String</param>
        ''' <value>Desired data access layer "Oracle" and default "SQLServer" connection string information</value>
        ''' <returns>String</returns>
        ''' <remarks>The CONFIG value can be encrypted or clear text</remarks>
        ReadOnly Property ConnectionString(ByVal dataAccessLayer As String) As String
            Get
                Dim mRetVal As String = GetAppSettingValue("DAL_" & dataAccessLayer & "_Connectionstring", True)
                Try
                    mRetVal = CryptoUtility.Decrypt(GetAppSettingValue("DAL_" & dataAccessLayer & "_Connectionstring", True), EncryptionType)
                Catch ex As CryptoUtilityException
                    Dim mLog As Logger = Logger.Instance()
                    mLog.Info(ex)
                End Try
                Return mRetVal
            End Get
        End Property

        ''' <summary>
        ''' Returns the connection string as defined in the CONFIG file by environment/DAL
        ''' </summary>
        ''' <value>Desired data access layer "Oracle" and default "SQLServer" connection string information</value>
        ''' <returns>String</returns>
        ''' <remarks>The CONFIG value can be encrypted or clear text</remarks>
        ReadOnly Property ConnectionString() As String
            Get
                Return ConnectionString("SQLServer")
            End Get
        End Property

        ''' <summary>
        ''' Returns the Conversion_Pattern from the CONFIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property ConversionPattern() As String
            Get
                Return GetAppSettingValue("Conversion_Pattern")
            End Get
        End Property

        ''' <summary>
        ''' Returns DAL from the CONFIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property DataAccessLayer() As String
            Get
                Return GetAppSettingValue("DAL", True)
            End Get
        End Property

        ''' <summary>
        ''' Returns the name of the data access layer assembly name as defined in the web.conf file by environment
        ''' </summary>
        ''' <param name="dataAccessLayer">String</param>
        ''' <value>Desired data access layer "Oracle" and default "SQLServer"</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property DataAccessLayerAssemblyName(ByVal dataAccessLayer As String) As String
            Get
                Return GetAppSettingValue("DAL_" + dataAccessLayer + "_Assembly_Name", True)
            End Get
        End Property

        ''' <summary>
        ''' Overloaded method calls DAL_AssemblyName passing "SQLServer"
        ''' </summary>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property DataAccessLayerAssemblyName() As String
            Get
                Return DataAccessLayerAssemblyName("SQLServer")
            End Get
        End Property

        ''' <summary>
        ''' Returns the DAL_xx_Name_Space from the CONFIG file
        ''' </summary>
        ''' <param name="dataAccessLayer"></param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property DataAccessLayerNamespace(ByVal dataAccessLayer As String) As String
            Get
                Return GetAppSettingValue("DAL_" & dataAccessLayer & "_Name_Space", True)
            End Get
        End Property

        ''' <summary>
        ''' Returns the DAL_SQLServer_Name_Space from the CONFIG file
        ''' </summary>
        ''' <value></value>
        ''' <returns>DataAccessLayerNamespace using SQLServer</returns>
        ''' <remarks></remarks>
        ReadOnly Property DataAccessLayerNamespace() As String
            Get
                Return DataAccessLayerNamespace("SQLServer")
            End Get
        End Property

        ''' <summary>
        ''' Returns Default_Action from the CONFIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property DefaultAction() As String
            Get
                Return GetAppSettingValue("Default_Action")
            End Get
        End Property

        ''' <summary>
        ''' Returns Default_Authenticated_Action from the CONFIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property DefaultAuthenticatedAction() As String
            Get
                Return GetAppSettingValue("Default_Authenticated_Action")
            End Get
        End Property

        ''' <summary>
        ''' Returns Default_Security_Entity_ID from the CONFIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property DefaultSecurityEntityId() As String
            Get
                Return GetAppSettingValue("Default_Security_Entity_ID", True)
            End Get
        End Property

        ''' <summary>
        ''' Returns Expected_Up_By from the CONFIG file
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property ExpectedUpBy() As String
            Get
                Return GetAppSettingValue("Expected_Up_By")
            End Get
        End Property

        ''' <summary>
        ''' Returns Enable_Cache from the CONFIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property EnableCache() As Boolean
            Get
                Return GetAppSettingValue("Enable_Cache", True)
            End Get
        End Property

        ''' <summary>
        ''' Retruns Enable_Pooling from the CONFIF file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks>For future use</remarks>
        ReadOnly Property EnablePooling() As String
            Get
                Return GetAppSettingValue("Enable_Pooling", True)
            End Get
        End Property

        ''' <summary>
        ''' Returns Environment from the CONFIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks>Used as a prefix to appSettings, allows multipule environment settings to co-exist in one CONFIG file.</remarks>
        ReadOnly Property Environment() As String
            Get
                Return GetAppSettingValue("Environment") & "_"
            End Get
        End Property

        ''' <summary>
        ''' Returns Environments from the CONFIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks>Comma seportated "list" of environments </remarks>
        ReadOnly Property Environments() As String
            Get
                Return GetAppSettingValue("Environments")
            End Get
        End Property

        ''' <summary>
        ''' Returns Environment without the trailing underscore
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property EnvironmentDisplayed() As String
            Get
                If Right(Environment, 1) = "_" Then
                    Return Left(Environment, Environment.Length - 1)
                Else
                    Return Environment
                End If
            End Get
        End Property

        ''' <summary>
        ''' Returns Encryption_Type from the CONFIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property EncryptionType() As String
            Get
                Return CInt(GetAppSettingValue("Encryption_Type"))
            End Get
        End Property

        ''' <summary>
        ''' Returns Failed_Attempts from the CONFIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property FailedAttempts() As Integer
            Get
                Return CInt(GetAppSettingValue("Failed_Attempts", True))
            End Get
        End Property

        ''' <summary>
        ''' Returns Force_HTTPS from the CONFIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property ForceHttps() As Boolean
            Get
                Return CBool(GetAppSettingValue("Force_HTTPS", True))
            End Get
        End Property

        ''' <summary>
        ''' Returns DB_Status from the CONFIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property DBStatus() As String
            Get
                Return GetAppSettingValue("DB_Status")
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets a value indicating the account to use for the client choices.
        ''' </summary>
        ''' <value>string value from config file</value>
        ''' <returns>string</returns>
        ReadOnly Property RegistrationAccountChoicesAccount() As String
            Get
                Return GetAppSettingValue("RegistrationAccountChoicesAccount", True)
            End Get
        End Property

        ''' <summary>
        ''' Returns RegistrationGroups from the CONFIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property RegistrationGroups As String
            Get
                Return GetAppSettingValue("RegistrationGroups", True)
            End Get
        End Property

        ''' <summary>
        ''' Return RegistrationPassword from the COFNIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks>String</remarks>
        ReadOnly Property RegistrationPassword As String
            Get
                Return GetAppSettingValue("RegistrationPassword", True)
            End Get
        End Property

        ''' <summary>
        ''' Return RegistrationPostAction from the COFNIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks>String</remarks>
        ReadOnly Property RegistrationPostAction() As String
            Get
                Return GetAppSettingValue("RegistrationPostAction", True)
            End Get
        End Property

        ''' <summary>
        ''' Gets the registration create roles.
        ''' </summary>
        ''' <value>The registration roles.</value>
        ReadOnly Property RegistrationRoles As String
            Get
                Return GetAppSettingValue("RegistrationRoles", True)
            End Get
        End Property

        ''' <summary>
        ''' Gets the RegistrationStatusIds account.
        ''' </summary>
        ''' <value>The RegistrationStatusIds.</value>
        ReadOnly Property RegistrationStatusId As String
            Get
                Return GetAppSettingValue("RegistrationStatusId", True)
            End Get
        End Property

        ''' <summary>
        ''' Gets the RegistrationSecurityEntityId.
        ''' </summary>
        ''' <value>The RegistrationSecurityEntityId.</value>
        ReadOnly Property RegistrationSecurityEntityId As String
            Get
                Return GetAppSettingValue("RegistrationSecurityEntityId", True)
            End Get
        End Property

        ''' <summary>
        ''' Retrun AppDisplayedName and Remember_Me from the CONFIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property RememberCookieName() As String
            Get
                Return AppDisplayedName & "Remember_Me"
            End Get
        End Property

        ''' <summary>
        ''' Return LDAP_Domain from the CONFIG file
        ''' </summary>
        ''' <value></value>
        ''' <returns>String</returns>
        ''' <remarks>String</remarks>
        ReadOnly Property LdapDomain() As String
            Get
                Return GetAppSettingValue("LDAP_Domain", True)
            End Get
        End Property

        ''' <summary>
        ''' Returns LDAP_Server from the CONFIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property LdapServer() As String
            Get
                Return GetAppSettingValue("LDAP_Server", True)
            End Get
        End Property

        ''' <summary>
        ''' Returns Log_Path from the CONFIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property LogPath() As String
            Get
                Return GetAppSettingValue("Log_Path", True)
            End Get
        End Property

        ''' <summary>
        ''' Returns Log_Priority from the CONFIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property LogPriority() As String
            Get
                Return GetAppSettingValue("Log_Priority", True)
            End Get
        End Property

        ''' <summary>
        ''' Returns Log_Retention from the CONFIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property LogRetention() As String
            Get
                Return GetAppSettingValue("Log_Retention", True)
            End Get
        End Property

        ''' <summary>
        ''' Returns Server_Side_View_State from the CONFIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property ServerSideViewState() As Boolean
            Get
                Return CBool(GetAppSettingValue("Server_Side_View_State"))
            End Get
        End Property

        ''' <summary>
        ''' Returns config setting for Strip_Domain_From_Http_Context_UserName.
        ''' </summary>
        ''' <value><c>true</c> if [strip domain from HTTP context user name]; otherwise, <c>false</c>.</value>
        ReadOnly Property StripDomainFromHttpContextUserName() As Boolean
            Get
                Dim mRetVal As Boolean = False
                Try
                    mRetVal = CBool(GetAppSettingValue("Strip_Domain_From_Http_Context_UserName"))
                Catch ex As Exception
                    Dim mLog As Logger = Logger.Instance()
                    mLog.Error(ex)
                    Throw
                End Try

                Return mRetVal
            End Get
        End Property


        ''' <summary>
        ''' Returns Server_Side_View_State_Pages from the CONFIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property ServerSideViewStatePages() As Integer
            Get
                Return CInt(GetAppSettingValue("Server_Side_View_State_Pages"))
            End Get
        End Property

        ''' <summary>
        ''' Returns decrypted SMTP_Account from the CONFIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property SmtpAccount() As String
            Get
                Try
                    Return CryptoUtility.Decrypt(GetAppSettingValue("SMTP_Account", True), EncryptionType)
                Catch ex As CryptoUtilityException
                    Return GetAppSettingValue("SMTP_Account", True)
                End Try
            End Get
        End Property

        ''' <summary>
        ''' Returns SMTP_From from the CONFIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property SmtpFrom() As String
            Get
                Return GetAppSettingValue("SMTP_From", True)
            End Get
        End Property

        ''' <summary>
        ''' Returns SMTP_Server from the CONFIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property SmtpServer() As String
            Get
                Return GetAppSettingValue("SMTP_Server", True)
            End Get
        End Property

        ''' <summary>
        ''' Returns SMTP_Password from CONFIG file
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property SmtpPassword() As String
            Get
                Try
                    Return CryptoUtility.Decrypt(GetAppSettingValue("SMTP_Password", True), EncryptionType)
                Catch ex As CryptoUtilityException
                    Dim mLog As Logger = Logger.Instance()
                    mLog.Debug(ex)
                    Return GetAppSettingValue("SMTP_Password", True)
                End Try
            End Get
        End Property

        ''' <summary>
        ''' Returns SMTP_Domain from the CONFIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property SmtpDomain() As String
            Get
                Return GetAppSettingValue("SMTP_Domain", True)
            End Get
        End Property

        ''' <summary>
        ''' Returns Synchronize_Password from the CONFIG file
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property SyncPassword() As String
            Get
                Return CBool(GetAppSettingValue("Synchronize_Password", True))
            End Get
        End Property

        ''' <summary>
        ''' Returns SKIN_TYPE from the CONFIG file
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property SkinType() As String
            Get
                Return GetAppSettingValue("SKIN_TYPE", True)
            End Get
        End Property

        ''' <summary>
        ''' Returns Under_Construction from the CONFIG file
        ''' </summary>
        ''' <value>Stirng</value>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        ReadOnly Property UnderConstruction() As Boolean
            Get
                Return CBool(GetAppSettingValue("Under_Construction"))
            End Get
        End Property

        ''' <summary>
        ''' Truncates a given string and adds ...
        ''' </summary>
        ''' <param name="text">String to be truncated</param>
        ''' <param name="length">Point at which the texted is truncated</param>
        ''' <returns>String</returns>
        ''' <remarks></remarks>
        Public Function TruncateWithEllipsis(ByVal [text] As String, ByVal length As Integer) As String
            If Not String.IsNullOrEmpty([text]) Then
                If [text].Length > length Then
                    [text] = [text].Substring(0, length) + "..."
                End If
            End If
            Return [text]
        End Function 'TruncateWithEllipsis

        Public Function GetAppSettingValue(ByVal settingName As String, ByVal fromEnvironment As Boolean) As String
            If fromEnvironment Then
                Return ConfigurationManager.AppSettings(Environment & settingName)
            Else
                Return ConfigurationManager.AppSettings(settingName)
            End If
        End Function

        Public Function GetAppSettingValue(ByVal settingName As String) As String
            Return GetAppSettingValue(settingName, False)
        End Function
    End Module
End Namespace
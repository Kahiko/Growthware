Imports System.Configuration
Imports System.Web
Imports System.Text
Imports System.Text.RegularExpressions

Namespace Common
	''' <summary>
	''' Servers as a collection of configuration information
	''' </summary>
	Public Class ConfigSettings
		''' <summary>
		''' Retruns Always_Left_Nav from the CONFIG file
		''' </summary>
		''' <value></value>
		''' <returns>Boolean</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property AlwaysLeftNav() As Boolean
			Get
				Return CBool(ConfigurationManager.AppSettings("Always_Left_Nav"))
			End Get
		End Property

		''' <summary>
		''' Retruns App_Displayed_Name from the CONFIG file
		''' </summary>
		''' <value></value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property AppDisplayedName() As String
			Get
				Return ConfigurationManager.AppSettings("App_Displayed_Name")
			End Get
		End Property

		''' <summary>
		''' Retruns Append_To_File from the CONFIG file
		''' </summary>
		''' <value></value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property AppendToFile() As String
			Get
				Return ConfigurationManager.AppSettings("Append_To_File")
			End Get
		End Property

		''' <summary>
		''' Returns App_Name from the CONFIG file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property AppName() As String
			Get
				Return ConfigurationManager.AppSettings(Environment & "App_Name")
			End Get
		End Property

		''' <summary>
		''' Returns Authentication_Type from the CONFIG file.
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property AuthenticationType() As String
			Get
				Return ConfigurationManager.AppSettings(Environment & "Authentication_Type")
			End Get
		End Property

		''' <summary>
		''' Returns Base_Page from the CONFIG file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property BasePage() As String
			Get
				Return ConfigurationManager.AppSettings("Base_Page")
			End Get
		End Property

		''' <summary>
		''' Returns Security_Entity_Translation from the CONFIG file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property SecurityEntityTranslation() As String
			Get
				Return ConfigurationManager.AppSettings("Security_Entity_Translation")
			End Get
		End Property

		''' <summary>
		''' Retruns BO_Server from the CONFIG file
		''' </summary>
		''' <value>Sting</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property BusinessObjectsServer() As String
			Get
				Return ConfigurationManager.AppSettings(Environment & "BO_Server")
			End Get
		End Property

		''' <summary>
		''' Returns BO_Authentication_Type from the CONFIG file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property BusinessObjectsAuthenticationType() As String
			Get
				Return ConfigurationManager.AppSettings(Environment & "BO_Authentication_Type")
			End Get
		End Property

		''' <summary>
		''' Returns Central_Management from the CONFIG file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property CentralManagement() As Boolean
			Get
				Return CBool(ConfigurationManager.AppSettings("Central_Management"))
			End Get
		End Property

		''' <summary>
		''' Returns the connection string as defined in the CONFIG file by environment/DAL
		''' </summary>
		''' <param name="dataAccessLayer">String</param>
		''' <value>Desired data access layer "Oracle" and default "SQLServer" connection string information</value>
		''' <returns>String</returns>
		''' <remarks>The CONFIG value can be encrypted or clear text</remarks>
		Shared ReadOnly Property ConnectionString(Optional ByVal DataAccessLayer As String = "SQLServer") As String
			Get
				Return CryptoUtility.Decrypt(ConfigurationManager.AppSettings(Environment & "DAL_" & DataAccessLayer & "_Connectionstring"), EncryptionType)
			End Get
		End Property

		''' <summary>
		''' Returns the Conversion_Pattern from the CONFIG file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property ConversionPattern() As String
			Get
				Return ConfigurationManager.AppSettings("Conversion_Pattern")
			End Get
		End Property

		''' <summary>
		''' Returns DAL from the CONFIG file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property DAL() As String
			Get
				Return ConfigurationManager.AppSettings(Environment & "DAL")
			End Get
		End Property

		''' <summary>
		''' Returns the name of the data access layer assembly name as defined in the web.conf file by environment
		''' </summary>
		''' <param name="dataAccessLayer">String</param>
		''' <value>Desired data access layer "Oracle" and default "SQLServer"</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property DAL_AssemblyName(Optional ByVal dataAccessLayer As String = "SQLServer") As String
			Get
				Return ConfigurationManager.AppSettings(Environment & "DAL_" & dataAccessLayer & "_Assembly_Name")
			End Get
		End Property

		''' <summary>
		''' Returns the DAL_xx_Name_Space from the CONFIG file
		''' </summary>
		''' <param name="dataAccessLayer"></param>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Shared ReadOnly Property DAL_Namespace(Optional ByVal dataAccessLayer As String = "SQLServer") As String
			Get
				Return ConfigurationManager.AppSettings(Environment & "DAL_" & dataAccessLayer & "_Name_Space")
			End Get
		End Property

		''' <summary>
		''' Returns Default_Action from the CONFIG file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property DefaultAction() As String
			Get
				Return ConfigurationManager.AppSettings("Default_Action")
			End Get
		End Property

		''' <summary>
		''' Returns Default_Authenticated_Action from the CONFIG file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property DefaultAuthenticatedAction() As String
			Get
				Return ConfigurationManager.AppSettings("Default_Authenticated_Action")
			End Get
		End Property

		''' <summary>
		''' Returns Default_Security_Entity_ID from the CONFIG file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property DefaultSecurityEntityID() As String
			Get
				Return ConfigurationManager.AppSettings(Environment & "Default_Security_Entity_ID")
			End Get
		End Property

		''' <summary>
		''' Returns Expected_Up_By from the CONFIG file
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Shared ReadOnly Property ExpectedUpBy() As String
			Get
				Return ConfigurationManager.AppSettings("Expected_Up_By")
			End Get
		End Property

		''' <summary>
		''' Returns Enable_Cache from the CONFIG file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property EnableCache() As Boolean
			Get
				Return ConfigurationManager.AppSettings(Environment & "Enable_Cache")
			End Get
		End Property

		''' <summary>
		''' Retruns Enable_Pooling from the CONFIF file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks>For future use</remarks>
		Shared ReadOnly Property EnablePooling() As String
			Get
				Return ConfigurationManager.AppSettings(Environment & "Enable_Pooling")
			End Get
		End Property

		''' <summary>
		''' Returns Environment from the CONFIG file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks>Used as a prefix to appSettings, allows multipule environment settings to co-exist in one CONFIG file.</remarks>
		Shared ReadOnly Property Environment() As String
			Get
				Return ConfigurationManager.AppSettings("Environment") & "_"
			End Get
		End Property

		''' <summary>
		''' Returns Environments from the CONFIG file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks>Comma seportated "list" of environments </remarks>
		Shared ReadOnly Property Environments() As String
			Get
				Return ConfigurationManager.AppSettings("Environments")
			End Get
		End Property

		''' <summary>
		''' Returns Environment without the leading underscore
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property EnvironmentDisplayed() As String
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
		Shared ReadOnly Property EncryptionType() As String
			Get
				Return CInt(ConfigurationManager.AppSettings("Encryption_Type"))
			End Get
		End Property

		''' <summary>
		''' Returns Failed_Attempts from the CONFIG file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property FailedAttempts() As Integer
			Get
				Return CInt(ConfigurationManager.AppSettings(Environment & "Failed_Attempts"))
			End Get
		End Property

		''' <summary>
		''' Returns Force_HTTPS from the CONFIG file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property ForceHTTPS() As Boolean
			Get
				Return CBool(ConfigurationManager.AppSettings(Environment & "Force_HTTPS"))
			End Get
		End Property

		''' <summary>
		''' Returns DB_Status from the CONFIG file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property DBStatus() As String
			Get
				Return ConfigurationManager.AppSettings("DB_Status")
			End Get
		End Property

		''' <summary>
		''' Returns Registering_Roles from the CONFIG file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property RegisteringRoles() As String
			Get
				Return ConfigurationManager.AppSettings(Environment & "Registering_Roles")
			End Get
		End Property

		''' <summary>
		''' Return Registration_Post_Action from the COFNIG file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks>String</remarks>
		Shared ReadOnly Property RegistrationPostAction()
			Get
				Return ConfigurationManager.AppSettings(Environment & "Registration_Post_Action")
			End Get
		End Property

		''' <summary>
		''' Retrun AppDisplayedName and Remember_Me from the CONFIG file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property RememberCookieName() As String
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
		Shared ReadOnly Property LDAPDomain() As String
			Get
				Return ConfigurationManager.AppSettings(Environment & "LDAP_Domain")
			End Get
		End Property

		''' <summary>
		''' Returns LDAP_Server from the CONFIG file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property LDAPServer() As String
			Get
				Return ConfigurationManager.AppSettings(Environment & "LDAP_Server")
			End Get
		End Property

		''' <summary>
		''' Returns Log_Path from the CONFIG file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property LogPath() As String
			Get
				Return ConfigurationManager.AppSettings(Environment & "Log_Path")
			End Get
		End Property

		''' <summary>
		''' Returns Log_Priority from the CONFIG file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property LogPriority() As String
			Get
				Return ConfigurationManager.AppSettings(Environment & "Log_Priority")
			End Get
		End Property

		''' <summary>
		''' Returns Log_Retention from the CONFIG file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property LogRetention() As String
			Get
				Return ConfigurationManager.AppSettings(Environment & "Log_Retention")
			End Get
		End Property

		''' <summary>
		''' Returns Server_Side_View_State from the CONFIG file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property ServerSideViewState() As Boolean
			Get
				Return CBool(ConfigurationManager.AppSettings("Server_Side_View_State"))
			End Get
		End Property

		''' <summary>
		''' Returns Server_Side_View_State_Pages from the CONFIG file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property ServerSideViewStatePages() As Integer
			Get
				Return CInt(ConfigurationManager.AppSettings("Server_Side_View_State_Pages"))
			End Get
		End Property

		''' <summary>
		''' Returns decrypted SMTP_Account from the CONFIG file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property SMTPAccount() As String
			Get
				Return CryptoUtility.Decrypt(ConfigurationManager.AppSettings(Environment & "SMTP_Account"), EncryptionType)
			End Get
		End Property

		''' <summary>
		''' Returns SMTP_From from the CONFIG file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property SMTPFrom() As String
			Get
				Return ConfigurationManager.AppSettings(Environment & "SMTP_From")
			End Get
		End Property

		''' <summary>
		''' Returns SMTP_Server from the CONFIG file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property SMTPServer() As String
			Get
				Return ConfigurationManager.AppSettings(Environment & "SMTP_Server")
			End Get
		End Property

		''' <summary>
		''' This method strips all tags from a string (good for removing all HTML.)
		''' </summary>
		''' <param name="text">string</param>
		''' <returns>string</returns>
		Public Shared Function StripTags(ByVal [text] As String) As String
			[text] = Regex.Replace([text], "&nbsp;", "", RegexOptions.IgnoreCase)
			Return Regex.Replace([text], "<.+?>", "", RegexOptions.Singleline)
		End Function 'StripTags     

		''' <summary>
		''' Returns SMTP_Password from CONFIG file
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Shared ReadOnly Property SMTPPassword() As String
			Get
				Try
					Return CryptoUtility.Decrypt(ConfigurationManager.AppSettings(Environment & "SMTP_Password"), EncryptionType)
				Catch ex As Exception
					Return ConfigurationManager.AppSettings(Environment & "SMTP_Password")
				End Try
			End Get
		End Property

		''' <summary>
		''' Returns SMTP_Domain from the CONFIG file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property SMTPDomain() As String
			Get
				Return ConfigurationManager.AppSettings(Environment & "SMTP_Domain")
			End Get
		End Property

		''' <summary>
		''' Returns Synchronize_Password from the CONFIG file
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Shared ReadOnly Property SyncPassword() As String
			Get
				Return CBool(ConfigurationManager.AppSettings(Environment & "Synchronize_Password"))
			End Get
		End Property

		''' <summary>
		''' Returns SKIN_TYPE from the CONFIG file
		''' </summary>
		''' <value>String</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property SkinType() As String
			Get
				Return ConfigurationManager.AppSettings(Environment & "SKIN_TYPE")
			End Get
		End Property

		''' <summary>
		''' Returns Under_Construction from the CONFIG file
		''' </summary>
		''' <value>Stirng</value>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Shared ReadOnly Property UnderConstruction() As Boolean
			Get
				Return CBool(ConfigurationManager.AppSettings("Under_Construction"))
			End Get
		End Property

		''' <summary>
		''' Truncates a given string and adds ...
		''' </summary>
		''' <param name="text">String to be truncated</param>
		''' <param name="length">Point at which the texted is truncated</param>
		''' <returns>String</returns>
		''' <remarks></remarks>
		Public Shared Function TruncateWithEllipsis(ByVal [text] As String, ByVal length As Integer) As String
			If [text].Length > length Then
				[text] = [text].Substring(0, length) + "..."
			End If
			Return [text]
		End Function 'TruncateWithEllipsis
	End Class
End Namespace
Imports DALModel.Base.BusinessUnits
Imports DALModel.Base.Modules

#Region " Notes "
' The BaseHelper class gives common access to application settings
' and other common routines
#End Region
Public Class BaseHelper
	Private Shared _DefaultBusinessUnitID As String = String.Empty
	Private Shared _ExceptionError As Exception = Nothing
	Private Shared _BusinessUnitTranslation As String = String.Empty
    Private Shared _SyncPassword As String = String.Empty
	Private Shared _UnderConstruction As Boolean = False
	Private Shared _ExpectedUpBy As String = String.Empty
	Private Shared _BasePage As String = String.Empty
	Private Shared _FQDNBasePage As String = String.Empty
	Private Shared _DefaultAction As String = String.Empty
	Private Shared _SmtpServer As String = String.Empty
	Private Shared _ImagePath As String = String.Empty
	Private Shared _ScriptPath As String = String.Empty
	Private Shared _StylesPath As String = String.Empty
	Private Shared _AppDisplayedName As String = String.Empty
	Private Shared _Environment As String = String.Empty
	Private Shared _Version As String = String.Empty
	Private Shared _AuthenticationType As String = String.Empty
	Private Shared _LDAPDomain As String = String.Empty
	Private Shared _LDAPServer As String = String.Empty
	Private Shared _AlwaysLeftNav As String = String.Empty
	Private Shared _LogPath As String = String.Empty
	Private Shared _AppName As String = String.Empty
	Private Shared _SMTPFrom As String = String.Empty
	Private Shared _BOServer As String = String.Empty
	Private Shared _BOAuthenticationType As String = String.Empty
	Private Shared _UI As String = String.Empty
	Private Shared _UISkinPath As String = String.Empty
	Private Shared _forceHTTPS As String = String.Empty
	Private Shared _HTTPSchema As String = String.Empty
	Private Shared _PageSupport As Boolean = False

	Public Shared ReadOnly Property BusinessUnitTranslation() As String
		Get
			If _BusinessUnitTranslation = String.Empty Then
				_BusinessUnitTranslation = ConfigurationSettings.AppSettings("BusinessUnitTranslation")
			End If
			Return _BusinessUnitTranslation
		End Get
	End Property

	Public Shared ReadOnly Property PageSupport() As Boolean
		Get
			Return ConfigurationSettings.AppSettings("PageSupport")
		End Get
	End Property

	Public Shared ReadOnly Property UnderConstruction() As Boolean
		Get
			Return ConfigurationSettings.AppSettings("UnderConstruction")
		End Get
	End Property

	Public Shared ReadOnly Property ExpectedUpBy() As String
		Get
			If _ExpectedUpBy = String.Empty Then
				_ExpectedUpBy = ConfigurationSettings.AppSettings("ExpectedUpBy")
			End If
			Return _ExpectedUpBy
		End Get
	End Property

	Public Shared Property ExceptionError() As Exception
		Get
			Return _ExceptionError
		End Get
		Set(ByVal Value As Exception)
			_ExceptionError = Value
		End Set
	End Property

	Public Shared ReadOnly Property BasePage() As String
		Get
			If _BasePage = String.Empty Then
				_BasePage = ConfigurationSettings.AppSettings("BasePage")
			End If
			Return _BasePage
		End Get
	End Property
	'*********************************************************************
	'
	' AppPath Property
	'
	' Represents the path of the current application.
	'
	'*********************************************************************
	Public Shared ReadOnly Property AppPath() As String
		Get
			If HttpContext.Current.Request.ApplicationPath = "/" Then
				Return String.Empty
			End If
			Return HttpContext.Current.Request.ApplicationPath
		End Get
	End Property

	'*********************************************************************
	'
	' DefaultAction Property
	'
	' Represents the default action or module to load
	'
	'*********************************************************************
	Public Shared ReadOnly Property DefaultAction() As String
		Get
			If _DefaultAction = String.Empty Then
				_DefaultAction = ConfigurationSettings.AppSettings("DefaultAction")
			End If
			Return _DefaultAction
		End Get
	End Property

	'*********************************************************************
	'
	' RootSite Property
	'
	' Represents the Root web site
	'
	'*********************************************************************
	Public Shared ReadOnly Property RootSite() As String
		Get
			If _HTTPSchema = String.Empty Then
				If ForceHTTPS Then
					_HTTPSchema = "HTTPS"
				Else
					_HTTPSchema = HttpContext.Current.Request.Url.Scheme
				End If
			End If

			If HttpContext.Current.Request.ApplicationPath = "/" Then
				Return _HTTPSchema & "://" & HttpContext.Current.Request.ServerVariables("HTTP_HOST") & "/"
			Else
				Return _HTTPSchema & "://" & HttpContext.Current.Request.ServerVariables("HTTP_HOST") & "/" & AppName & "/"
			End If
		End Get
	End Property

	'*********************************************************************
	'
	' FQDNBasePage Property
	'
	' Represents the FQDN of the Base page for the web application
	'
	'*********************************************************************
	Public Shared ReadOnly Property FQDNBasePage(Optional ByVal State As String = "YY") As String
		Get
			If _FQDNBasePage = String.Empty Then
				_FQDNBasePage = RootSite & ConfigurationSettings.AppSettings("BasePage")
			Else
				If Not (_FQDNBasePage = RootSite & ConfigurationSettings.AppSettings("BasePage")) Then
					_FQDNBasePage = RootSite & ConfigurationSettings.AppSettings("BasePage")
				End If
			End If
			Return _FQDNBasePage
		End Get
	End Property	' FQDNBasePage

	'*********************************************************************
	'
	' SyncPassword Property
	'
	' Indicates if the password should be syncronized.
	' The password should be syncronized if the authentication method
	' is not internal otherwise the password in the data store is
	' already current.
	' This will happen when the authentication method is say
	' ldap.  In which case there are other ways to change
	' the password outside of our process.  When this is the case
	' the password in our data store should then be changed
	' to what the client has entered.
	' This allows us to use the entered password
	' in third party software such as Business Objects.
	'
	'*********************************************************************
	Public Shared ReadOnly Property SyncPassword() As String
		Get
			If _SyncPassword = String.Empty Then
				If BaseHelper.AuthenticationType.Trim.ToLower <> "internal" Then
					_SyncPassword = True
				Else
					_SyncPassword = False
				End If
			End If
			Return _SyncPassword
		End Get
	End Property	' SyncPassword

	'*********************************************************************
	'
	' SmtpServer Property
	'
	' Retrieves the SMTP Server from the web.config.
	'
	'*********************************************************************

	Public Shared ReadOnly Property SmtpServer() As String
		Get
			If _SmtpServer = String.Empty Then
				_SmtpServer = ConfigurationSettings.AppSettings(Environment & "SMTPServer")
			End If
			Return _SmtpServer
		End Get
	End Property

	'*********************************************************************
	'
	' ImagePath Property
	'
	' Represents the environment of the application
	'
	'*********************************************************************
	Public Shared ReadOnly Property ImagePath() As String
		Get
			If _ImagePath = String.Empty OrElse Not (_ImagePath = RootSite & "Images/") Then
				_ImagePath = RootSite & "Images/"
			End If
			'Return _ImagePath
			Return RootSite & "Images/"
		End Get
	End Property

	'*********************************************************************
	'
	' ScriptPath Property
	'
	' Represents the script path
	'
	'*********************************************************************
	Public Shared ReadOnly Property ScriptPath() As String
		Get
			If _ScriptPath = String.Empty OrElse Not (_ScriptPath = RootSite & "Scripts/") Then
				_ScriptPath = RootSite & "Scripts/"
			End If
			Return _ScriptPath
		End Get
	End Property

	'*********************************************************************
	'
	' StylesPath Property
	'
	' Represents the Styles path
	'
	'*********************************************************************
	Public Shared ReadOnly Property StylesPath() As String
		Get
			If _StylesPath = String.Empty OrElse Not (_StylesPath = RootSite & "Styles/") Then
				_StylesPath = RootSite & "Styles/"
			End If
			Return _StylesPath
		End Get
	End Property
	'*********************************************************************
	'
	' AppName Property
	'
	' Represents the name of the appliation
	'
	'*********************************************************************
	Public Shared ReadOnly Property AppName() As String
		Get
			' need to test this on a web server before we can use it.
			'If _AppName = String.Empty Then
			'	_AppName = System.Reflection.Assembly.GetCallingAssembly.GetName.Name
			'End If
			If _AppName = String.Empty Then
				_AppName = ConfigurationSettings.AppSettings("AppName")
			End If
			Return _AppName
		End Get
	End Property

	'*********************************************************************
	'
	' SMTPFrom Property
	'
	' Represents the SMTP from property in the web.config file
	'
	'*********************************************************************
	Public Shared ReadOnly Property SMTPFrom() As String
		Get
			If _SMTPFrom = String.Empty Then
				_SMTPFrom = ConfigurationSettings.AppSettings(Environment & "SMTPFrom")
			End If
			Return _SMTPFrom
		End Get
	End Property	'SMTPFrom

	'*********************************************************************
	'
	' BOServer Property
	'
	' Represents the BOServer property in the web.config file
	'
	'*********************************************************************
	Public Shared ReadOnly Property BOServer() As String
		Get
			If _BOServer = String.Empty Then
				_BOServer = ConfigurationSettings.AppSettings(Environment & "BOServer")
			End If
			Return _BOServer
		End Get
	End Property	'BOServer

	'*********************************************************************
	'
	' BOServer Property
	'
	' Represents the BOServer property in the web.config file
	'
	'*********************************************************************
	Public Shared ReadOnly Property BOAuthenticationType() As String
		Get
			If _BOAuthenticationType = String.Empty Then
				_BOAuthenticationType = ConfigurationSettings.AppSettings(Environment & "BOAuthenticationType")
			End If
			Return _BOAuthenticationType
		End Get
	End Property	'BOServer

	'*********************************************************************
	'
	' AppDisplayedName Property
	'
	' Represents the displayed name of the appliation
	'
	'*********************************************************************
	Public Shared ReadOnly Property AppDisplayedName() As String
		Get
			If _AppDisplayedName = String.Empty Then
				'Dim titleAttr As System.Reflection.AssemblyTitleAttribute
				'Dim asm As System.Reflection.Assembly
				'asm = System.Reflection.Assembly.GetExecutingAssembly
				'titleAttr = asm.GetCustomAttributes(GetType(System.Reflection.AssemblyTitleAttribute), False)(0)
				'_AppDisplayedName = titleAttr.Title
				_AppDisplayedName = ConfigurationSettings.AppSettings("AppDisplayedName")
			End If
			Return _AppDisplayedName
		End Get
	End Property

	'*********************************************************************
	'
	' Environment Property
	'
	' Represents the environment of the application
	'
	'*********************************************************************
	Public Shared ReadOnly Property Environment() As String
		Get
			If _Environment = String.Empty Then
				_Environment = ConfigurationSettings.AppSettings("Environment")
			End If
			Return _Environment
		End Get
	End Property
	'*********************************************************************
	'
	' Environment Property
	'
	' Represents the environment of the application
	'
	'*********************************************************************
	Public Shared ReadOnly Property Verison() As String
		Get
			If _Version = String.Empty Then
				_Version = System.Reflection.Assembly.GetCallingAssembly.GetName.Version.ToString
			End If
			Return _Version
		End Get
	End Property

	'
	'*********************************************************************
	'
	' AuthenticationType Property
	'
	' Represents the AuthenticationType of the application
	'
	'*********************************************************************
	Public Shared ReadOnly Property AuthenticationType() As String
		Get
			If _AuthenticationType = String.Empty Then
				_AuthenticationType = ConfigurationSettings.AppSettings(Environment & "AuthenticationType")
			End If
			Return _AuthenticationType
		End Get
	End Property

	Public Shared ReadOnly Property LDAPDomain() As String
		Get
			If _LDAPDomain = String.Empty Then
				_LDAPDomain = ConfigurationSettings.AppSettings(Environment & "LDAPDomain")
			End If
			Return _LDAPDomain
		End Get
	End Property

	Public Shared ReadOnly Property LDAPServer() As String
		Get
			If _LDAPServer = String.Empty Then
				_LDAPServer = ConfigurationSettings.AppSettings(Environment & "LDAPServer")
			End If
			Return _LDAPServer
		End Get
	End Property
	'*********************************************************************
	' GetURL Method
	' Returns the URL including all URL parameters.
	'*********************************************************************
	Public Shared Function GetURL() As String
		Dim context As HttpContext
		context = HttpContext.Current
		Dim item
		Dim myURL As String = "&"
		For Each item In context.Request.QueryString
			If item.tolower <> "action" And item.tolower <> "returnurl" And item.tolower <> "wfn" Then
				myURL &= item & "=" & context.Server.UrlEncode(context.Request.QueryString(item)) & "&"
			End If
		Next
		myURL = myURL.Substring(0, Len(myURL) - 1)
		Return myURL
	End Function	' GetURL

	'*********************************************************************
	'
	' GetRandomString function
	'
	' Returns a lowercase alpha charactor
	'
	'*********************************************************************
	Public Shared Function GetRandomPassword(Optional ByVal Low As Integer = 65, Optional ByVal High As Integer = 90) As String
		Dim retVal As String
		retVal = System.Guid.NewGuid().ToString()
		Return retVal
	End Function	' GetRandomString

	Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Long)

	'*********************************************************************
	'
	' GetRandomNumber function
	'
	' Returns a random number given the max and min - 0
	'
	'*********************************************************************
	Public Shared Function GetRandomNumber(ByVal MaxNumber As Integer, Optional ByVal MinNumber As Integer = 0) As Integer
		Dim retVal As Integer = 0
		'initialize random number generator
		Dim r As New Random(System.DateTime.Now.Millisecond)
		'if passed incorrect arguments, swap them
		'can also throw exception or return 0
		If MinNumber > MaxNumber Then
			Dim t As Integer = MinNumber
			MinNumber = MaxNumber
			MaxNumber = t
		End If
		retVal = r.Next(MinNumber, MaxNumber)
		Sleep(CType((System.DateTime.Now.Millisecond * (retVal / 100)), Long))
		Return retVal
	End Function	'GetRandomNumber

	'*********************************************************************
	'
	' AlwaysLeftNav Property
	'
	' Represents AlwaysLeftNav setting in the web.config file.
	'
	'*********************************************************************
	Public Shared ReadOnly Property AlwaysLeftNav() As Boolean
		Get
			If _AlwaysLeftNav = String.Empty Then
				_AlwaysLeftNav = ConfigurationSettings.AppSettings("AlwaysLeftNav")
			End If
			Return _AlwaysLeftNav
		End Get
	End Property

	'*********************************************************************
	'
	' LogPath Property
	'
	' Retrieves the log path from the web.config.
	'
	'*********************************************************************

	Public Shared ReadOnly Property LogPath() As String
		Get
			If _LogPath = String.Empty Then
				_LogPath = ConfigurationSettings.AppSettings(Environment & "LogPath")
			End If
			Return _LogPath
		End Get
	End Property

	'*********************************************************************
	'
	' UI Property
	'
	' Retrieves the UI path from the web.config.
	'
	'*********************************************************************

	Public Shared ReadOnly Property UI(ByVal BUSINESS_UNIT_SEQ_ID As Integer) As String
		Get
			Dim businessUnitProfileInfo As New MBusinessUnitProfileInfo
			Dim businessUnitProfileInfoCollection As New MBusinessUnitProfileInfoCollection
			BusinessUnitUtility.GetBusinessProfileCollection(businessUnitProfileInfoCollection)
			Dim retVal As String = "Default"
			businessUnitProfileInfo = businessUnitProfileInfoCollection.GetBusinessUnitByID(BUSINESS_UNIT_SEQ_ID)
			retVal = businessUnitProfileInfo.Skin
			Return retVal
		End Get
	End Property

	'*********************************************************************
	'
	' UIPath Property
	'
	' Retrieves the UI theme path from the web.config.
	'
	'*********************************************************************
	Public Shared ReadOnly Property UIPath() As String
		Get
			If _UISkinPath = String.Empty Then
				_UISkinPath = ConfigurationSettings.AppSettings(Environment & "UISkinPath")
			End If
			Return _UISkinPath
		End Get
	End Property

	Private Shared ReadOnly Property ForceHTTPS() As Boolean
		Get
			If _forceHTTPS = String.Empty Then
				_forceHTTPS = ConfigurationSettings.AppSettings(Environment & "ForceHTTPS")
			End If
			Return CBool(_forceHTTPS)
		End Get
	End Property


	'*********************************************************************
	'
	' SelectBusinessUnit method
	' Navigates to the select a business unit module with a return url
	'
	'*********************************************************************
	Public Shared Sub SelectBusinessUnit()
		Dim myRedirect As String
		Dim moduleProfileInfo As MModuleProfileInfo
		moduleProfileInfo = AppModulesUtility.GetCurrentModule()
		myRedirect += "select a business unit"
		myRedirect += "&ReturnURL=" & moduleProfileInfo.Action
		HttpContext.Current.Session("clientMSG") = moduleProfileInfo.Name & "  is not valid with the " & BaseHelper.BusinessUnitTranslation & " you have selected.<br>Please choose an appropriate " & BaseHelper.BusinessUnitTranslation
		NavControler.NavTo(myRedirect)
	End Sub	'SelectBusinessUnit

	'*********************************************************************
	'
	' SetDropSelection method
	' Sets the selected value of a drop down list
	'
	'*********************************************************************
	Public Shared Sub SetDropSelection(ByRef theDropDown As WebControls.DropDownList, ByVal SelectedVale As String)
		Try
			Dim X As Integer
			For X = 0 To theDropDown.Items.Count - 1
				If theDropDown.Items(X).Value = SelectedVale Then
					theDropDown.SelectedIndex = X
					Exit For
				End If
			Next
		Catch ex As Exception
			Throw ex
		End Try
	End Sub	'SetDropSelection

	Public Shared Sub ShowUnderConstruction()
		Try
			If BaseHelper.UnderConstruction Then
				HttpContext.Current.Response.Redirect(BaseHelper.RootSite & "Pages/System/UnderConstruction.aspx")
			End If
		Catch ex As Exception
			Dim mike As String = String.Empty
		End Try
	End Sub

	'*********************************************************************
	' DefaultBusinessUnitID Property
	' Returns the default business unit id.
	'*********************************************************************
	Public Shared ReadOnly Property DefaultBusinessUnitID() As Integer
		Get
			If _DefaultBusinessUnitID = String.Empty Then
				_DefaultBusinessUnitID = ConfigurationSettings.AppSettings("DefaultBusinessUnitID")
			End If
			Return CInt(_DefaultBusinessUnitID)
		End Get
	End Property	'DefaultBusinessUnitID
End Class
Imports DALModel.Base.Accounts.Security
Imports DALModel.Base.Modules
Imports System.Web
Imports System.Web.SessionState
Imports System.Text

Public Class Global
	Inherits System.Web.HttpApplication

#Region " Component Designer Generated Code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
    End Sub

#End Region

	Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
		Dim log As AppLogger = AppLogger.GetInstance
        Dim originalPriority As log4net.Priority = log.convertPriorityTextToPriority(ConfigurationSettings.AppSettings(BaseHelper.Environment & "LogPriority").ToLower)
        log.SetThreshold(log4net.Priority.DEBUG)
		log.Debug("Starting " & BaseHelper.AppDisplayedName & " version: " & BaseHelper.Verison)
		log.SetThreshold(originalPriority)
	End Sub

	Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
		Dim myAction As String = Request.QueryString("Action")
		If myAction Is Nothing Then
			If HttpContext.Current.User.Identity.IsAuthenticated Then
				NavControler.NavTo(BaseHelper.DefaultAction)
			Else
				If Right(HttpContext.Current.Request.Path.ToLower, Len(BaseHelper.BasePage)) = BaseHelper.BasePage.ToLower Then
					NavControler.NavTo("generichome")
				End If
			End If
		End If
	End Sub

	Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
		' ensure the site is not under construction if it is then
		' do not alow any page to be rendered!
		If BaseHelper.UnderConstruction Then
			If Not HttpContext.Current.Request.Path.EndsWith("UnderConstruction.aspx") Then
				BaseHelper.ShowUnderConstruction()
			End If
			Exit Sub
		End If
		If BaseHelper.PageSupport Then
			' Get a module profile
            Dim moduleProfileInfo As MModuleProfileInfo
			' Figure out the module and add to context
			If Not Request.QueryString("Action") Is Nothing Then
				' Populate the module profile information
				' Note:  AppModulesUtility.GetModuleInfoFromAction will attempt to get the
				' information from cache if it's not there then
				' the cache is populated.
				moduleProfileInfo = AppModulesUtility.GetModuleInfoByAction(Request.QueryString("Action").ToLower)
			Else
				' Populate the module profile information
				moduleProfileInfo = AppModulesUtility.GetModuleInfoByAction("generichome")
			End If
			If moduleProfileInfo Is Nothing Then Exit Sub
			' Add module to context
			HttpContext.Current.Items("ModuleProfileInfo") = moduleProfileInfo
		End If
	End Sub

	Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
		If BaseHelper.UnderConstruction Then
			Exit Sub
		End If
		If BaseHelper.PageSupport Then
            Dim moduleProfileInfo As MModuleProfileInfo = AppModulesUtility.GetCurrentModule()
			If moduleProfileInfo Is Nothing Then Exit Sub
			Dim accountSecurityInfo As MAccountSecurityInfo
			If HttpContext.Current.Request.IsAuthenticated Then
				Dim myAccountUtility As New AccountUtility(HttpContext.Current)
				myAccountUtility.GetAllAccountRoles()
				accountSecurityInfo = New MAccountSecurityInfo(moduleProfileInfo)
				HttpContext.Current.Items(myAccountUtility.SessionAccountSecurityInfo) = accountSecurityInfo
				' NOTE:
				'   Global security enforcement happens in the following two
				'   locations:
				'       1.) Here in the global.asax.vb ... view rights
				'   
				'       2.) Change password status check 
				'           happens through the NavControler when 
				'           NavControler BuildModuleList is invoked.
				'	Other than that any further enforcement of security is
				'	up to the developer of the page/module.
				If Not accountSecurityInfo.MayView Then
					NavControler.NavTo("AccessDenied&RequestedAction=" & Context.Request.QueryString("Action"))
				End If
			Else
				Try
					accountSecurityInfo = New MAccountSecurityInfo(moduleProfileInfo)
					If Not accountSecurityInfo.MayView Then
						NavControler.NavTo("AccessDenied")
					End If
				Catch ex As Exception
					NavControler.NavTo("Logon&RequestedAction=" & Context.Request.QueryString("Action") & BaseHelper.GetURL)
				End Try
			End If

		End If
	End Sub

	Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
		Dim ex As Exception = Server.GetLastError.GetBaseException
		Dim log As AppLogger = AppLogger.GetInstance
		If ex.Message.IndexOf("Cannot redirect after HTTP headers have been sent") >= 0 Then
			Exit Sub
		End If
		log.Error(ex)
		BaseHelper.ExceptionError = ex
		NavControler.NavTo(ex)
	End Sub

	Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
		' Fires when the session ends
	End Sub

	Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
		' Fires when the application ends
	End Sub
End Class
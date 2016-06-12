Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Common.Logging
Imports ApplicationBase.Model.Modules
Imports ApplicationBase.Model.Special.ClientChoices
Imports ApplicationBase.Model.Accounts.Security

Public Class _DefaultPage
	Inherits ClientChoices.ClientChoicesPage

	Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

		Dim moduleProfileInfo As MModuleProfileInfo
        ' Get a module profile
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
                NavControler.NavTo("Logon&RequestedAction=" & Context.Request.QueryString("Action") & BaseSettings.GetURL)
            End Try
        End If

		Dim myPage As New Control
		Dim myControlSource As String = String.Empty
		Dim currentModule As MModuleProfileInfo = AppModulesUtility.GetCurrentModule
		Try
            myControlSource = "UI/" & BaseHelperOld.UI(ClientChoicesState(MClientChoices.BusinessUnitID)) & "/Default.ascx"
			myPage = Me.LoadControl(myControlSource)
			plcPage.Controls.Add(myPage)
		Catch ex As Exception
			Dim myErrorMSG As String = String.Empty
            myErrorMSG = "A critical error has occured within the UI.  The UI is: " & BaseHelperOld.UI(ClientChoicesState(MClientChoices.BusinessUnitID))
			Dim myEx As New ApplicationException(myErrorMSG, ex)
			Dim log As AppLogger = AppLogger.GetInstance
			log.Fatal(myEx)
            myControlSource = "UI/Default/Default.ascx"
			myPage = Me.LoadControl(myControlSource)
			plcPage.Controls.Add(myPage)
		End Try
		If Not currentModule Is Nothing Then
			Me.Page.EnableViewState = currentModule.EnableViewState
		End If
	End Sub
End Class
Imports ApplicationBase.Common.Logging
Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Model
Imports ApplicationBase.Model.Modules
Imports ApplicationBase.Model.WorkFlows
Imports ApplicationBase.Model.Accounts
Imports ApplicationBase.Model.Special.ClientChoices
Imports System.IO

#Region " Notes "
' The NavControler class controls navigation in the 
' application by loading any given usercontrol into
' an asp:placeholder or by redirecting to a page.
' The NavControler will also accept an action
' The NavControler class also controls workflow in
' the application by updating or removing a WorkFlowProfileInfo 
' object in the HTTP session.
#End Region
Public Class NavControler

	'*********************************************************************
	' This NavTo Method
	' Handles navagation action.
	'*********************************************************************
	Public Shared Sub NavTo(ByVal Action As String)
		On Error Resume Next
		' you have reached this by way of menu navigation and
		' any referance to some flow should be removed from the session
		' if it exists
        HttpContext.Current.Response.Redirect(BaseSettings.FQDNPage & "?Action=" & Action)
	End Sub	'NavTo

	'*********************************************************************
	' This NavTo Method
	' Handles loading the appropriate module into the UI.
	'*********************************************************************
	Public Shared Sub NavTo(ByVal Action As String, ByVal ThePage As Page, ByVal PlaceHolder As PlaceHolder)
		' ensure that the action will match case
		Action = Action.ToLower
		chkChangePass(Action)
		' get the current module profile information
		Dim moduleProfileInfo As MModuleProfileInfo = AppModulesUtility.GetCurrentModule
		Try
			' Ensure the source is somewhat valid
			If moduleProfileInfo.Source.Trim = "none" OrElse moduleProfileInfo.Source.Trim.Length = 0 Then
				moduleProfileInfo = AppModulesUtility.GetModuleInfoByAction("notavailable")
				BuildModuleList(PlaceHolder, moduleProfileInfo, ThePage)
				Exit Sub
			End If
			If Right(moduleProfileInfo.Source, 5).ToLower = ".ascx" Then
				BuildModuleList(PlaceHolder, moduleProfileInfo, ThePage)
            Else
                If Not Left(moduleProfileInfo.Source, 4).ToLower = "http" Then
                    Dim theURL As String = "?Action=" & Action
                    theURL &= BaseHelperOld.GetURL()
                    HttpContext.Current.Response.Redirect(BaseSettings.RootSite & moduleProfileInfo.Source & theURL)
                Else
                    HttpContext.Current.Response.Redirect(moduleProfileInfo.Source)
                End If
            End If
		Catch ex As Exception
			moduleProfileInfo = AppModulesUtility.GetModuleInfoByAction("unknownerror")
			BuildModuleList(PlaceHolder, moduleProfileInfo, ThePage)
		End Try
	End Sub	'NavTo

	'*********************************************************************
	' This NavTo Method
	' Handles navagation to the systems error "page".
	'*********************************************************************
	Public Shared Sub NavTo(ByRef ex As Exception)
        Dim myURL As String = "~/" & BaseSettings.FQDNPage & "?Action=error"
		Dim myPath As String = HttpContext.Current.Request.Path
        If InStr(myPath, BaseSettings.AppPath) <> 0 Then
            myPath = Mid(myPath, Len(BaseSettings.AppPath) + 2, Len(myPath))
        End If
        myURL = BaseSettings.FQDNPage & "?Action=error&ReturnURL=" & HttpContext.Current.Request.QueryString("Action") & BaseSettings.GetURL
		Try
			HttpContext.Current.Response.Redirect(myURL)

		Catch

		End Try
	End Sub	'NavTo

	'*********************************************************************
	' This NavTo Method
	' Handles navagation for any work flow.
	'*********************************************************************
	Private Shared Sub NavTo(ByVal WorkFlowProfileInfo As MWorkFlowProfileInfo, Optional ByVal URL As String = "")
		Dim myModuleProfileInfo As MModuleProfileInfo = AppModulesUtility.GetModulesByID(WorkFlowProfileInfo.Action)
        HttpContext.Current.Response.Redirect(BaseSettings.FQDNPage & "?Action=" & myModuleProfileInfo.Action & URL)
	End Sub

	'*********************************************************************
	' BuildModuleList Method
	' Loads a module into a control.
	'*********************************************************************
	Public Shared Sub BuildModuleList(ByVal parent As Control, ByVal myModuleProfileInfo As MModuleProfileInfo, ByRef ThePage As Page)
		If (myModuleProfileInfo Is Nothing) Then Exit Sub
		'Dim ModuleList() As String = Split(Modules, ";")		  ' here incase there are more than one module to load
		Dim i As Integer
		Dim myAccountUtility As New AccountUtility(HttpContext.Current)
		'For i = 0 To ModuleList.Length - 1 
		' this code at one time loaded more than one module per place holder
		' not that the system has been redesigned to accomidate web applications
		' and not portals this is no longer necessary.
		' The code is being left here for future use!
        Dim Action As String = BaseSettings.defaultAction
		For i = 0 To 0
			If parent.ClientID.IndexOf("Left") > 0 Then			' if this is a righthand module and status is change password
				HttpContext.Current.Items("leftModuleProfileInfo") = myModuleProfileInfo
			End If
            If ((Not BaseSettings.alwaysLeftNav) And parent.ID.Trim.ToLower.IndexOf("right") > -1) Then
                If Not HttpContext.Current.User.Identity.IsAuthenticated Then
                    Action = "generichome"
                    myModuleProfileInfo = AppModulesUtility.GetModuleInfoByAction(Action)
                End If
            End If
			Dim moduleSource As String = myModuleProfileInfo.Source
			If ((moduleSource <> "") And (moduleSource <> "System.DBNull")) Then
				Try
					If Not HttpContext.Current.Request.QueryString("Action") Is Nothing Then Action = HttpContext.Current.Request.QueryString("Action").ToLower.Trim
					Dim UIModule As New Control
					Dim UISpecificModuleSource As String = String.Empty
					Dim UISpecificModuleSourcePath As String = String.Empty
					UISpecificModuleSource = moduleSource
                    UISpecificModuleSourcePath = UISpecificModuleSource.Replace("Modules\", BaseSettings.uIPath() & BaseHelperOld.UI(ClientChoicesUtility.GetSelectedBusinessUnit) & "\UserControls\")
					If Not File.Exists(UISpecificModuleSourcePath) Then					  ' if there is no ui specific module
						' load default module
						UIModule = ThePage.LoadControl("~\" & moduleSource)
						parent.Controls.Add(UIModule)
						parent.Controls.Add(New LiteralControl("<br>"))
					Else
						' load UI specific module
                        UISpecificModuleSource = UISpecificModuleSource.Replace("Modules\", "UI\" & BaseHelperOld.UI(ClientChoicesUtility.GetSelectedBusinessUnit) & "\UserControls\")
						UIModule = ThePage.LoadControl("~\" & UISpecificModuleSource)
						parent.Controls.Add(UIModule)
						parent.Controls.Add(New LiteralControl("<br>"))
					End If
				Catch ex As Exception
					Dim litError As New LiteralControl
					litError.Text = "Error Could not load module<br>" & moduleSource & "<br>" & vbCrLf
					litError.Text += ex.Message
					parent.Controls.Add(litError)
				End Try
			End If
		Next i
	End Sub	' BuildModuleList

	'*********************************************************************
	' Start Method
	' Starts a flow process based on the name of the flow
	'*********************************************************************
	Public Shared Sub StartWorkFlow(ByVal WorkFlowName As String, Optional ByVal URL As String = "")
		Dim myWorkFlowProfile As MWorkFlowProfileInfo
		Dim myWorkFlowProfileCollection As MWorkFlowProfileInfoCollection = Nothing
		myWorkFlowProfileCollection = WorkFlowUtility.GetWorkFlowCollection(WorkFlowName, myWorkFlowProfileCollection)
		If myWorkFlowProfileCollection Is Nothing Then
			Dim ex As New ApplicationException("Could not get the work flow collection for " & WorkFlowName)
            BaseHelperOld.ExceptionError = ex
			NavControler.NavTo(ex)
		End If
		myWorkFlowProfile = myWorkFlowProfileCollection.GetWorkFlowByOrder(1)
		HttpContext.Current.Session(WorkFlowUtility.SessionWFPName) = myWorkFlowProfile
		NavControler.NavTo(myWorkFlowProfile, URL)
	End Sub

	'*********************************************************************
	' RemoveWorkFlow Method
	' Removes work flow information from the session.
	'*********************************************************************
	Public Shared Sub RemoveWorkFlow()
		HttpContext.Current.Session.Remove(WorkFlowUtility.SessionWFPName)
	End Sub

	'*********************************************************************
	' EndWorkFlow Method
	' Navigates to the last order in the work flow
	'*********************************************************************
	Public Shared Sub [End](Optional ByVal saveWorkFlow As Boolean = False)
		On Error Resume Next
		Dim lastWorkFlowProfile As MWorkFlowProfileInfo
		Dim currentWorkFlowProfile As MWorkFlowProfileInfo
		Dim myWorkFlowProfileCollection As MWorkFlowProfileInfoCollection = Nothing
		Dim lastOrder As Integer = 0
		Dim myModuleProfileInfo As MModuleProfileInfo = Nothing
		currentWorkFlowProfile = GetCurrentWFP()
		myWorkFlowProfileCollection = WorkFlowUtility.GetWorkFlowCollection(currentWorkFlowProfile.WorkFlowName, myWorkFlowProfileCollection)
		lastOrder = myWorkFlowProfileCollection.Count
		lastWorkFlowProfile = myWorkFlowProfileCollection.GetWorkFlowByOrder(lastOrder)
		If Not lastWorkFlowProfile Is Nothing Then
			myModuleProfileInfo = AppModulesUtility.GetModulesByID(lastWorkFlowProfile.Action)
			If Not saveWorkFlow Then
				HttpContext.Current.Session.Remove(WorkFlowUtility.SessionWFPName)
			Else
				WorkFlowUtility.SetWorkFlow(lastWorkFlowProfile)
			End If
			NavControler.NavTo(myModuleProfileInfo.Action)
		Else
			Dim ex As New ApplicationException("Work Flow Error ... could not retrieve the last profile")
			Dim myAppLogger As AppLogger = AppLogger.GetInstance
			myAppLogger.Error(ex)
			NavTo("home")
		End If
	End Sub

	'*********************************************************************
	' Next Method
	' Determins what the next action in a flow is and
	' utilizes the private NavTo method to perfor the
	' necessary navigation.
	' This method also keeps the current WorkFlowProfileInfo
	' current.
	'*********************************************************************
	Public Shared Sub [Next]()
		On Error Resume Next
		Dim nextWorkFlowProfile As MWorkFlowProfileInfo
		Dim currentWorkFlowProfile As MWorkFlowProfileInfo
		Dim myWorkFlowProfileCollection As MWorkFlowProfileInfoCollection = Nothing
		Dim nextOrder As Integer = 0
		Dim myModuleProfileInfo As MModuleProfileInfo
		currentWorkFlowProfile = GetCurrentWFP()
		myWorkFlowProfileCollection = WorkFlowUtility.GetWorkFlowCollection(currentWorkFlowProfile.WorkFlowName, myWorkFlowProfileCollection)
		nextOrder = currentWorkFlowProfile.Order + 1
		nextWorkFlowProfile = myWorkFlowProfileCollection.GetWorkFlowByOrder(nextOrder)
		If Not nextWorkFlowProfile Is Nothing Then
			myModuleProfileInfo = AppModulesUtility.GetModulesByID(nextWorkFlowProfile.Action)
			HttpContext.Current.Session(WorkFlowUtility.SessionWFPName) = nextWorkFlowProfile
            NavControler.NavTo(myModuleProfileInfo.Action & BaseSettings.getURL)
		Else
			Dim ex As New ApplicationException("Work Flow Error ... no next")
			Dim myAppLogger As AppLogger = AppLogger.GetInstance
			myAppLogger.Error(ex)
			'Throw ex
			NavTo("home")
		End If
	End Sub

	'*********************************************************************
	' Previous Method
	' Determins what the previous action in a flow is and
	' utilizes the private NavTo method to perfor the
	' necessary navigation.
	' This method also keeps the current WorkFlowProfileInfo
	' current.
	'*********************************************************************
	Public Shared Sub Previous()
		On Error Resume Next
		Dim previousWorkFlowProfile As MWorkFlowProfileInfo
		Dim currentWorkFlowProfile As MWorkFlowProfileInfo
		Dim myWorkFlowProfileCollection As MWorkFlowProfileInfoCollection = Nothing
		Dim nextOrder As Integer = 0
		Dim myModuleProfileInfo As MModuleProfileInfo
		currentWorkFlowProfile = GetCurrentWFP()
		myWorkFlowProfileCollection = WorkFlowUtility.GetWorkFlowCollection(currentWorkFlowProfile.WorkFlowName, myWorkFlowProfileCollection)
		nextOrder = currentWorkFlowProfile.Order - 1
		previousWorkFlowProfile = myWorkFlowProfileCollection.GetWorkFlowByOrder(nextOrder)
		If Not previousWorkFlowProfile Is Nothing Then
			myModuleProfileInfo = AppModulesUtility.GetModulesByID(previousWorkFlowProfile.Action)
			HttpContext.Current.Session(WorkFlowUtility.SessionWFPName) = previousWorkFlowProfile
            NavControler.NavTo(myModuleProfileInfo.Action & BaseSettings.GetURL)
		Else
			myModuleProfileInfo = AppModulesUtility.GetModulesByID(currentWorkFlowProfile.Action)
            NavTo(myModuleProfileInfo.Action & BaseSettings.GetURL)
		End If
	End Sub

	'*********************************************************************
	' Current Method
	' Determins what the current action in a flow is and
	' utilizes the private NavTo method to perfor the
	' necessary navigation.
	'*********************************************************************
	Public Shared Sub Current()
		On Error Resume Next
		Dim currentWorkFlowProfile As MWorkFlowProfileInfo
		currentWorkFlowProfile = GetCurrentWFP()
		If Not currentWorkFlowProfile Is Nothing Then
			Dim myModuleProfileInfo As MModuleProfileInfo = AppModulesUtility.GetModulesByID(currentWorkFlowProfile.Action)
            NavTo(myModuleProfileInfo.Action & BaseSettings.GetURL)
		End If
	End Sub

	'*********************************************************************
	' GetCurrentWFP Function
	' Returns the current work flow profile.
	'*********************************************************************
	Public Shared Function GetCurrentWFP() As MWorkFlowProfileInfo
		Dim myWorkFlowProfileInfo As MWorkFlowProfileInfo
		myWorkFlowProfileInfo = HttpContext.Current.Session(WorkFlowUtility.SessionWFPName)
		Return myWorkFlowProfileInfo
	End Function

	'*********************************************************************
	' chkChangePass Method
	' Ensures that any given accounts status
	' is not change passwor.  If is it
	' then all navigation attempts will
	' result in navigating to the change password
	' page/module
	'*********************************************************************
	Private Shared Sub chkChangePass(ByVal Action As String)
		Dim requestingAccount As String = HttpContext.Current.User.Identity.Name
		Dim myAccountUtility As New AccountUtility(HttpContext.Current)
		Dim accountProfileInfo As MAccountProfileInfo
		If requestingAccount = "" Then
			requestingAccount = "Anonymous"
		End If
		accountProfileInfo = myAccountUtility.GetAccountProfileInfo(requestingAccount)
		If accountProfileInfo.SYSTEM_STATUS_ID = MSystemStatus.value.ChangePassword And Not Action.Trim.ToLower = "changepassword" Then
			NavTo("changepassword")
		End If
	End Sub
End Class
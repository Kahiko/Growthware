Imports System.Web.Security
Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Model.Accounts
Imports ApplicationBase.Model.Special.ClientChoices
Imports ApplicationBase.Model.Modules

Partial Class LeftHandModulesLoader
	Inherits ClientChoices.ClientChoicesUserControl

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim myModuleProfileInfo As MModuleProfileInfo
        If BaseSettings.AlwaysLeftNav Then
            myModuleProfileInfo = AppModulesUtility.GetModuleInfoByAction("navigation")
            NavControler.BuildModuleList(LeftUIModules, myModuleProfileInfo, Me.Page)
        Else
            If Context.User.Identity.IsAuthenticated Then
                Dim accountProfileInfo As MAccountProfileInfo
                Dim myAccountUtility As New AccountUtility(HttpContext.Current)
                accountProfileInfo = myAccountUtility.GetAccountProfileInfo(Context.User.Identity.Name, False)
                Dim LeftMenuItems As Integer = NavMenuUtility.NumberOfMenuItems(False, accountProfileInfo.ACCOUNT_SEQ_ID, ClientChoicesState(MClientChoices.BusinessUnitID))
                If LeftMenuItems > 0 Then
                    myModuleProfileInfo = AppModulesUtility.GetModuleInfoByAction("navigation")
                    NavControler.BuildModuleList(LeftUIModules, myModuleProfileInfo, Me.Page)
                Else
                    Dim myControl As Web.UI.Control = Me.Parent.FindControl("LeftHandModulesLoader")
                    If Not myControl Is Nothing Then
                        myControl.Visible = False
                    End If
                End If
            Else
                myModuleProfileInfo = AppModulesUtility.GetModuleInfoByAction("Logon")
                NavControler.BuildModuleList(LeftUIModules, myModuleProfileInfo, Me.Page)
            End If
        End If
	End Sub	'Page_Load
End Class
Imports System.Web.Security
Imports BLL.Base.ClientChoices
Imports DALModel.Special.Accounts
Imports DALModel.Special.ClientChoices
Imports DALModel.Base.Modules

Public Class LeftHandModulesLoader
    Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents LeftUIModules As System.Web.UI.WebControls.PlaceHolder

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim myModuleProfileInfo As MModuleProfileInfo
        If BaseHelper.AlwaysLeftNav Then
			myModuleProfileInfo = AppModulesUtility.GetModuleInfoByAction("navigation")
			NavControler.BuildModuleList(LeftUIModules, myModuleProfileInfo, Me.Page)
        Else
            If context.User.Identity.IsAuthenticated Then
				Dim accountProfileInfo As MAccountProfileInfo
				Dim myAccountUtility As New AccountUtility(HttpContext.Current)
				accountProfileInfo = myAccountUtility.GetAccountProfileInfo(context.User.Identity.Name, False)
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
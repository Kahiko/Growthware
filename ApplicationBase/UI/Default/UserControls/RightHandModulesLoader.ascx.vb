Imports ApplicationBase.Model.Modules
Imports ApplicationBase.Common.Globals

Partial Class RightHandModulesLoader
	Inherits ClientChoices.ClientChoicesUserControl

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		' loads the control into the place holder
		Dim currentModule As MModuleProfileInfo = AppModulesUtility.GetCurrentModule()
        Dim thePageTitle As System.Web.UI.HtmlControls.HtmlTitle
		If Not Request.QueryString("action") = vbNullString Then
			NavControler.NavTo(Request.QueryString("action").ToLower, Me.Page, RightUIModules)
		Else
			If HttpContext.Current.User.Identity.IsAuthenticated Then
                NavControler.NavTo(BaseSettings.DefaultAction, Me.Page, RightUIModules)
			Else
				NavControler.NavTo("generichome", Me.Page, RightUIModules)
			End If
		End If
		If Not currentModule Is Nothing Then
            thePageTitle = Me.Parent.FindControl("PageTitle")
            thePageTitle.Text = currentModule.Description
		End If
	End Sub
End Class
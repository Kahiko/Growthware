Imports BLL.Base.ClientChoices
Imports DALModel.Base.Modules

Public Class RightHandModulesLoader
    Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents RightUIModules As System.Web.UI.WebControls.PlaceHolder

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
		' loads the control into the place holder
		Dim currentModule As MModuleProfileInfo = AppModulesUtility.GetCurrentModule()
		Dim thePageTitle As System.Web.UI.HtmlControls.HtmlGenericControl
		If Not Request.QueryString("action") = vbNullString Then
			NavControler.NavTo(Request.QueryString("action").ToLower, Me.Page, RightUIModules)
		Else
            If HttpContext.Current.User.Identity.IsAuthenticated Then
				NavControler.NavTo(BaseHelper.DefaultAction, Me.Page, RightUIModules)
            Else
				NavControler.NavTo("generichome", Me.Page, RightUIModules)
            End If
		End If
		If Not currentModule Is Nothing Then
			thePageTitle = Me.Parent.FindControl("PageTitle")
			thePageTitle.InnerHtml = currentModule.Description
		End If
	End Sub
End Class
Imports BLL.Base.ClientChoices
Imports DALModel.Base.Modules

Public Class AccessDenied
	Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents insufficientRights As System.Web.UI.WebControls.PlaceHolder
	Protected WithEvents mustLogon As System.Web.UI.WebControls.PlaceHolder
	Protected WithEvents requesedAction As System.Web.UI.HtmlControls.HtmlGenericControl
	Protected WithEvents requesedAction1 As System.Web.UI.HtmlControls.HtmlGenericControl

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
		'Put user code to initialize the page here
		' get a module profile information class
		Dim moduleProfileInfo As MModuleProfileInfo
		' get a modulecollection 
		Dim moduleCollection As New MModuleCollection
		If Not IsPostBack Then
			Try
				' get the module collection from cache
				' cache has been loaded by global.asax.vb
				moduleProfileInfo = AppModulesUtility.GetModuleInfoByAction(Request.QueryString("RequestedAction").ToLower)
				If Context.Request.IsAuthenticated Then
					requesedAction.InnerHtml = moduleProfileInfo.Name
					insufficientRights.Visible = True
				Else
					requesedAction1.InnerHtml = moduleProfileInfo.Name
					mustLogon.Visible = True
				End If
			Catch ex As Exception
				' do nothing
			End Try
		End If
	End Sub
End Class
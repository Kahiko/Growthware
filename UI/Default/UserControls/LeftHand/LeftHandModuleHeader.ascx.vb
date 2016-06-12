Imports BLL.Base.ClientChoices
Imports DALModel.Base.Modules

Public Class LeftHandModuleHeader
    Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents tdEdit As System.Web.UI.HtmlControls.HtmlTableCell
	Protected WithEvents tdClose As System.Web.UI.HtmlControls.HtmlTableCell
	Protected WithEvents anchorEditPage As System.Web.UI.HtmlControls.HtmlAnchor
	Public ModuleSource As String = ""
	Public Title As String
	Public canEdit As Boolean
	Public canClose As Boolean
	Protected WithEvents anchorClose As System.Web.UI.HtmlControls.HtmlAnchor
	Private _EditPage As String = "# edit"
	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object

	Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

	Public Property EditPage() As String
		Get
			Return _EditPage
		End Get
        Set(ByVal Value As String)
            _EditPage = BaseHelper.RootSite & Value
        End Set
    End Property

	Private RedirectURL As String = String.Empty
	Private NeedRedirect As Boolean = False

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		tdEdit.Visible = False
		tdClose.Visible = False
		anchorEditPage.HRef = EditPage
		If canEdit Then
			tdEdit.Visible = True
		End If
		If canClose Then
			tdClose.Visible = True
		End If
	End Sub	'Page_Load

	Private Sub anchorClose_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles anchorClose.ServerClick
		'RedirectURL = "home"
		'NeedRedirect = True
	End Sub	'anchorClose_ServerClick
	Private Function TrimEnd(ByVal source As String, ByVal trimchar As String) As String
		Dim tc() As Char = trimchar.ToCharArray()
		TrimEnd = source.TrimEnd(tc)
	End Function	'TrimEnd

	'*********************************************************************
	'
	' Page_PreRender Method
	'
	' Performs any action needed before the page is rendered.
	' In most cases we will do any type of redirect here to avoid.
	' header errors.
	'
	'*********************************************************************
	Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
		If NeedRedirect Then NavControler.NavTo(RedirectURL)
		Dim moduleProfileInfo As MModuleProfileInfo = CType(HttpContext.Current.Items("leftModuleProfileInfo"), MModuleProfileInfo)
		If Not moduleProfileInfo Is Nothing Then
			Title = moduleProfileInfo.Name
		Else
			Title = "&nbsp;"
		End If
	End Sub	' Page_PreRender

End Class

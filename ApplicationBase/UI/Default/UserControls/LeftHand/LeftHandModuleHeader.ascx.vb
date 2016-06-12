Imports ApplicationBase.Model.Modules
Imports ApplicationBase.Common.Globals

Partial Class LeftHandModuleHeader
	Inherits ClientChoices.ClientChoicesUserControl

	Public ModuleSource As String = ""
	Public Title As String
	Public canEdit As Boolean
	Public canClose As Boolean
	Private _EditPage As String = "# edit"

	Public Property EditPage() As String
		Get
			Return _EditPage
		End Get
		Set(ByVal Value As String)
            _EditPage = BaseSettings.RootSite & Value
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

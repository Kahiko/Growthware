Imports Common.Cache

Public Class UpdateAnonymousClientProfileInfo
	Inherits System.Web.UI.UserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents btnClearAnonymous As System.Web.UI.WebControls.Button

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
	End Sub

	Private Sub btnClearAnonymous_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearAnonymous.Click
		Dim myAccountUtility As New AccountUtility(HttpContext.Current)
		CacheControler.RemoveFromCache(myAccountUtility.AnonymousAccountProfileInfo)
		CacheControler.RemoveFromCache("anonymousMenu")
	End Sub
End Class

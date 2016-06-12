Imports BLL.Base.ClientChoices
Imports DALModel.Special.ClientChoices

Public Class Home
    Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents SideImage As System.Web.UI.WebControls.Image
	Protected WithEvents lblAppName As System.Web.UI.WebControls.Label

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
		On Error Resume Next
		If Not IsPostBack Then
			lblAppName.Text = BaseHelper.AppDisplayedName
			SideImage.ImageUrl = BaseHelper.ImagePath & SideImage.ImageUrl & ClientChoicesState(MClientChoices.ColorScheme) & ".gif"
		End If
	End Sub	'Page_Load
End Class

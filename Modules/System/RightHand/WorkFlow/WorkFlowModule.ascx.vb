Imports DALModel

Public Class WorkFlowModule
	Inherits System.Web.UI.UserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub

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
		Dim WorkFlowName As String = Request.QueryString("WFN")
		If WorkFlowName Is Nothing OrElse WorkFlowName.Trim.Length = 0 Then
			Dim ex As New ApplicationException("Must set the WFN= parameter in the URL")
			BaseHelper.ExceptionError = ex
			NavControler.NavTo(ex)
		End If
		Dim myURL As String = BaseHelper.GetURL()
		NavControler.StartWorkFlow(WorkFlowName, myURL)
	End Sub

End Class

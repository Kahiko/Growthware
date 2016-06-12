Imports BLL.Base.ClientChoices

Public Class SetLogLevel
    Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents dropLogLevel As System.Web.UI.WebControls.DropDownList
	Protected WithEvents btnSetLogLevel As System.Web.UI.WebControls.Button

	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object

	Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

	Private log As AppLogger = AppLogger.GetInstance
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		dropLogLevel.SelectedIndex = log.CurrentLogLevel
	End Sub

	Private Sub btnSetLogLevel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetLogLevel.Click
		log.SetThreshold(log4net.Priority.WARN)
		log.Warn("Setting log level to " & dropLogLevel.SelectedItem.Text & " by " & HttpContext.Current.User.Identity.Name)
		Select Case dropLogLevel.SelectedValue
			Case 0
				log.SetThreshold(log4net.Priority.DEBUG)
			Case 1
				log.SetThreshold(log4net.Priority.INFO)
			Case 2
				log.SetThreshold(log4net.Priority.WARN)
			Case 3
				log.SetThreshold(log4net.Priority.ERROR)
			Case 4
				log.SetThreshold(log4net.Priority.FATAL)
			Case Else
				log.SetThreshold(log4net.Priority.ERROR)
		End Select
	End Sub
End Class

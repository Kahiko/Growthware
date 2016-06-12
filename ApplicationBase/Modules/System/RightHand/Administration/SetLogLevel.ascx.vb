Imports ApplicationBase.Common.Logging

Partial Class SetLogLevel
	Inherits ClientChoices.ClientChoicesUserControl

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

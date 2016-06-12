Imports BLL.Base.ClientChoices
Imports DALModel.Base.Accounts.Security
Imports DALModel.Base.Modules
Imports DALModel.Special.ClientChoices

Public Class CalendarModule
    Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents CalendarControl As System.Web.UI.WebControls.Calendar
	Protected WithEvents lblInstructions As System.Web.UI.WebControls.Label
	Protected WithEvents txtComments As System.Web.UI.WebControls.TextBox
	Protected WithEvents BtnSave As System.Web.UI.WebControls.Button
	Protected WithEvents BtnDelete As System.Web.UI.WebControls.Button
	Protected WithEvents litError As System.Web.UI.WebControls.Literal

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
		If Not IsPostBack Then
			CalendarControl.TitleStyle.BackColor = ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.HeadColor))
			CalendarControl.DayStyle.BackColor = ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.LeftColor))
			CalendarControl.DayHeaderStyle.BackColor = ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.SubheadColor))
			CalendarControl.SelectedDayStyle.BackColor = ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.SubheadColor))
			CalendarControl.TodayDayStyle.BackColor = ColorTranslator.FromHtml(ClientChoicesState(MClientChoices.HeadColor))
			Dim moduleProfileInfo As MModuleProfileInfo = AppModulesUtility.GetCurrentModule
			Dim myAccountUtility As New AccountUtility(HttpContext.Current)
			Dim accountSecurityInfo As MAccountSecurityInfo = myAccountUtility.GetAccountSecurityInfo(accountSecurityInfo)
			If accountSecurityInfo.MayDelete Then
				BtnDelete.Visible = True
			End If
			If accountSecurityInfo.MayAdd OrElse accountSecurityInfo.MayEdit Then
				BtnSave.Visible = True
			End If
		End If
		litError.Text = String.Empty
		litError.Visible = False
	End Sub

	Private Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
		Dim selectedDate As Date
		If Not CheckDate(selectedDate) Then Exit Sub
		Dim Account As String = context.User.Identity.Name
		Dim CalendarName As String = Request.QueryString("Action").Trim.ToLower
		Dim success As Boolean = False
		Try
			success = CalendarUtility.SaveCalendarData(ClientChoicesState(MClientChoices.BusinessUnitID), CalendarName, Account & " " & txtComments.Text.Trim, selectedDate)
        Catch ex As Exception
            If InStr(ex.Message, "duplicate key") > 0 Then
                litError.Text = "<br>Entry already exist no save performed"
                litError.Visible = True
            Else
                Throw ex
            End If
        End Try
    End Sub

    Private Sub BtnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnDelete.Click
        Dim selectedDate As Date
        If Not CheckDate(selectedDate) Then Exit Sub
        Dim Account As String = context.User.Identity.Name
		Dim CalendarName As String = Request.QueryString("Action").Trim.ToLower
        Dim success As Boolean = False
        Try
			success = CalendarUtility.DeleteCalendarData(ClientChoicesState(MClientChoices.BusinessUnitID), CalendarName, txtComments.Text.Trim, selectedDate)
            If Not success Then
                litError.Text = "<br>Could not delete the entry"
                litError.Visible = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Function CheckDate(ByRef selectedDate As Date) As Boolean
        Dim retVal As Boolean = False
        selectedDate = CalendarControl.SelectedDate.Date
        If selectedDate = "#12:00:00 AM#" Then
            litError.Text &= "<br>Please Select a Date first"
        End If
        If txtComments.Text.Trim.Length = 0 Then
            litError.Text &= "<br>Please enter a comment"
        End If
        If litError.Text.Length = 0 Then
            litError.Visible = False
            retVal = True
        Else
            litError.Visible = True
            retVal = False
        End If
        Return retVal
    End Function
    Private Sub CalendarControl_DayRender(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DayRenderEventArgs) Handles CalendarControl.DayRender
        Dim Account As String = context.User.Identity.Name
		Dim CalendarName As String = Request.QueryString("Action").Trim.ToLower
        Dim myDS As New DataSet
        Dim success As Boolean = False
		success = CalendarUtility.GetCalendarData(ClientChoicesState(MClientChoices.BusinessUnitID), CalendarName, myDS)
        If success Then
            Dim x As Integer
            Dim strEntry As String = String.Empty
            If e.Day.IsOtherMonth = False Then
                For x = 0 To myDS.Tables(0).Rows.Count - 1
                    If e.Day.DayNumberText = Day(myDS.Tables(0).Rows(x).Item(2)) And e.Day.Date.Month = Month(myDS.Tables(0).Rows(x).Item(2)) Then
                        strEntry = strEntry & "<LI>" & myDS.Tables(0).Rows(x).Item(3) & "</LI>"
                    End If
                Next
                e.Cell.Controls.Add(New LiteralControl("<BR>" & strEntry))
            End If
        End If
    End Sub
End Class
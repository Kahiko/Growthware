Imports System.Text
Imports ApplicationBase.Common.Globals

Public Class DatePicker
	Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents Calendar1 As System.Web.UI.WebControls.Calendar

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
		Dim objBuilder As New StringBuilder
        objBuilder.AppendFormat("        <link href=""{0}"" type=""text/css"" rel=""stylesheet""/>" & vbCrLf, BaseSettings.StylesPath & "Main.css")
		Controls.Add(New LiteralControl(objBuilder.ToString()))
	End Sub

	Private Sub Calendar1_DayRender(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DayRenderEventArgs) Handles Calendar1.DayRender
		'// Clear the link from this day
		e.Cell.Controls.Clear()

		'// Add the custom link
		Dim Link As System.Web.UI.HtmlControls.HtmlGenericControl
		Link = New System.Web.UI.HtmlControls.HtmlGenericControl
		Link.TagName = "a"
		Link.InnerText = e.Day.DayNumberText
		Link.Attributes.Add("href", String.Format("JavaScript:window.opener.document.forms.{0}.value = '{1:d}'; window.close();", Request.QueryString("field"), e.Day.Date))

		'// By default, this will highlight today's date.
		If e.Day.IsSelected Then
			Link.Attributes.Add("style", Me.Calendar1.SelectedDayStyle.ToString())
		End If


		'// Now add our custom link to the page
		e.Cell.Controls.Add(Link)

	End Sub
End Class

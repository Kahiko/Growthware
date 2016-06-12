Imports System.Text
Imports BLL.Base.ClientChoices
Imports DALModel.Special.ClientChoices

Public Class _Default
    Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub

	Protected WithEvents plcPageHeaderControl As System.Web.UI.WebControls.PlaceHolder
	Protected WithEvents plcLeftHandModulesLoader As System.Web.UI.WebControls.PlaceHolder

	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object

	Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

	Protected PageTitle As System.Web.UI.HtmlControls.HtmlGenericControl
	Protected CustomStyles As System.Web.UI.HtmlControls.HtmlGenericControl

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strBuilder As New StringBuilder
        Dim myCustomStyles As Web.UI.HtmlControls.HtmlGenericControl
        myCustomStyles = Me.FindControl("CustomStyles")
        strBuilder.Append("             .tab {background: url('" & BaseHelper.ImagePath & "yellow.gif') ; background-repeat: repeat; background-color: #FFFFFF;}" & vbCrLf)
        strBuilder.Append("             .tabr {background: url('" & BaseHelper.ImagePath & "flapr.gif') ; background-repeat: no-repeat; background-color: #FFFFFF;}" & vbCrLf)
        strBuilder.Append("             .tabl {background: url('" & BaseHelper.ImagePath & "flapl.gif') ; background-repeat: no-repeat; background-color: #FFFFFF;}" & vbCrLf)
		strBuilder.Append("             ul#tabStripUL a" & vbCrLf)
		strBuilder.Append("             {" & vbCrLf)
		strBuilder.Append("             	float:left;" & vbCrLf)
		strBuilder.Append("             	width:80px;" & vbCrLf)
		strBuilder.Append("             	text-decoration:none;" & vbCrLf)
		strBuilder.Append("             	background: " & ClientChoicesState(MClientChoices.SubheadColor) & ";" & vbCrLf)
		strBuilder.Append("             	color: #999" & vbCrLf)
		strBuilder.Append("             }" & vbCrLf)
		myCustomStyles.InnerHtml = strBuilder.ToString
		strBuilder = New StringBuilder
		strBuilder.Append("        <script language=""JavaScript"" type=""text/javascript"">" & vbCrLf)
        strBuilder.Append("           var imagePath='" & BaseHelper.ImagePath & "';" & vbCrLf)
		strBuilder.Append("           var HeadColor='" & ClientChoicesState(MClientChoices.HeadColor) & "';" & vbCrLf)
		strBuilder.Append("           var SubheadColor='" & ClientChoicesState(MClientChoices.SubheadColor) & "';" & vbCrLf)
		strBuilder.Append("           var calendarPage='" & BaseHelper.RootSite & "Pages/System/DatePicker.aspx" & "';" & vbCrLf)
		strBuilder.Append("        </script>" & vbCrLf)
		Page.RegisterClientScriptBlock("CustomScripts", strBuilder.ToString)
	End Sub
End Class

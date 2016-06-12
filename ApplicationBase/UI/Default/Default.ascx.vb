Imports System.Text
Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Model.Special.ClientChoices

Partial Public Class _Default
	Inherits ClientChoices.ClientChoicesUserControl

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Dim strBuilder As New StringBuilder
		Dim myCustomStyles As Web.UI.HtmlControls.HtmlGenericControl
		myCustomStyles = Me.FindControl("CustomStyles")
        strBuilder.Append("             .tab {background: url('" & BaseSettings.ImagePath & "yellow.gif') ; background-repeat: repeat; background-color: #FFFFFF;}" & vbCrLf)
        strBuilder.Append("             .tabr {background: url('" & BaseSettings.ImagePath & "flapr.gif') ; background-repeat: no-repeat; background-color: #FFFFFF;}" & vbCrLf)
        strBuilder.Append("             .tabl {background: url('" & BaseSettings.ImagePath & "flapl.gif') ; background-repeat: no-repeat; background-color: #FFFFFF;}" & vbCrLf)
        strBuilder.Append("             ul#tabStripUL a" & vbCrLf)
        strBuilder.Append("             {" & vbCrLf)
        strBuilder.Append("             	float:left;" & vbCrLf)
        strBuilder.Append("             	width:80px;" & vbCrLf)
        strBuilder.Append("             	text-decoration:none;" & vbCrLf)
        strBuilder.Append("             	background: " & ClientChoicesState(MClientChoices.SubheadColor) & ";" & vbCrLf)
        strBuilder.Append("             	font-weight: bold;" & vbCrLf)
        strBuilder.Append("             	color: White" & vbCrLf)
        strBuilder.Append("             }" & vbCrLf)
        strBuilder.Append("             ul#tabStripUL a:hover" & vbCrLf)
        strBuilder.Append("             {" & vbCrLf)
        strBuilder.Append("             	float:left;" & vbCrLf)
        strBuilder.Append("             	width:80px;" & vbCrLf)
        strBuilder.Append("             	text-decoration:none;" & vbCrLf)
        strBuilder.Append("             	background: " & ClientChoicesState(MClientChoices.HeadColor) & ";" & vbCrLf)
        strBuilder.Append("             	color: White" & vbCrLf)
        strBuilder.Append("             }" & vbCrLf)
        strBuilder.Append("             ul#tabStripUL a:active" & vbCrLf)
        strBuilder.Append("             {" & vbCrLf)
        strBuilder.Append("             	float:left;" & vbCrLf)
        strBuilder.Append("             	width:80px;" & vbCrLf)
        strBuilder.Append("             	text-decoration:none;" & vbCrLf)
        strBuilder.Append("             	background: " & ClientChoicesState(MClientChoices.HeadColor) & ";" & vbCrLf)
        strBuilder.Append("             	color: White" & vbCrLf)
        strBuilder.Append("             }" & vbCrLf)
        myCustomStyles.InnerHtml = strBuilder.ToString
        strBuilder = New StringBuilder
        strBuilder.Append("        <script language=""JavaScript"" type=""text/javascript"">" & vbCrLf)
        strBuilder.Append("           var imagePath='" & BaseSettings.ImagePath & "';" & vbCrLf)
		strBuilder.Append("           var HeadColor='" & ClientChoicesState(MClientChoices.HeadColor) & "';" & vbCrLf)
		strBuilder.Append("           var SubheadColor='" & ClientChoicesState(MClientChoices.SubheadColor) & "';" & vbCrLf)
        strBuilder.Append("           var calendarPage='" & BaseSettings.rootSite & "Pages/System/DatePicker.aspx" & "';" & vbCrLf)
		strBuilder.Append("        </script>" & vbCrLf)
		Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "CustomScripts", strBuilder.ToString)
	End Sub

End Class
Imports ApplicationBase.Common.Globals
Imports System.Collections
Imports System.Collections.Specialized
Imports System.Web.Security
Imports System.Drawing

Partial Class PageHeaderControl
	Inherits ClientChoices.ClientChoicesUserControl

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		If Not Context.User.Identity.IsAuthenticated Then
			Dim ContentLink As String = "&nbsp;"
			tdClientSecurityInformation.InnerHtml = ContentLink
		End If
        AppImage.ImageUrl = BaseSettings.ImagePath & "umms.gif"
	End Sub	'Page_Load

	Function MakeHtmlCell(ByVal cellContents As String, Optional ByVal align As String = "center", Optional ByVal valign As String = "top", Optional ByVal bgColor As String = "white") As HtmlTableCell
		Dim theCell As New HtmlTableCell
		theCell.ColSpan = 100
		theCell.Align = align
		theCell.VAlign = valign
		theCell.BgColor = bgColor
		theCell.InnerHtml = cellContents
		Return theCell
	End Function	'MakeHtmlCell
End Class
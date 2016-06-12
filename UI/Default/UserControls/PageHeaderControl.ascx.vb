Imports System.Collections
Imports System.Collections.Specialized
Imports System.Web.Security
Imports System.Drawing
Imports BLL.Base.ClientChoices

Public Class PageHeaderControl
    Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub

	Protected WithEvents AppImage As System.Web.UI.WebControls.Image
	Protected WithEvents tdClientSecurityInformation As System.Web.UI.HtmlControls.HtmlTableCell

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
		If Not context.User.Identity.IsAuthenticated Then
			Dim ContentLink As String = "&nbsp;"
			tdClientSecurityInformation.InnerHtml = ContentLink
        End If
        AppImage.ImageUrl = BaseHelper.ImagePath & "umms.gif"
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
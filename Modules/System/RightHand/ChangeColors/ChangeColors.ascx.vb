Imports BLL.Base.ClientChoices
Imports DALModel.Special.ClientChoices
Imports System.Collections
Imports System.Collections.Specialized

Public Class ChangeColors
	Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
    Protected WithEvents Radio1 As System.Web.UI.HtmlControls.HtmlInputRadioButton
    Protected WithEvents Radio2 As System.Web.UI.HtmlControls.HtmlInputRadioButton
    Protected WithEvents Radio3 As System.Web.UI.HtmlControls.HtmlInputRadioButton
    Protected WithEvents Radio4 As System.Web.UI.HtmlControls.HtmlInputRadioButton
    Protected WithEvents Radio5 As System.Web.UI.HtmlControls.HtmlInputRadioButton
    Protected WithEvents trSubmit As System.Web.UI.HtmlControls.HtmlTableRow
	Protected WithEvents blueImage As System.Web.UI.HtmlControls.HtmlImage
    Protected WithEvents greenImage As System.Web.UI.HtmlControls.HtmlImage
    Protected WithEvents yellowImage As System.Web.UI.HtmlControls.HtmlImage
    Protected WithEvents purpleImage As System.Web.UI.HtmlControls.HtmlImage
    Protected WithEvents redImage As System.Web.UI.HtmlControls.HtmlImage
	Protected WithEvents btnSave As System.Web.UI.WebControls.Button

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
			blueImage.Src = BaseHelper.ImagePath & "option_blue.gif"
			greenImage.Src = BaseHelper.ImagePath & "option_green.gif"
			yellowImage.Src = BaseHelper.ImagePath & "option_yellow.gif"
			purpleImage.Src = BaseHelper.ImagePath & "option_purple.gif"
			redImage.Src = BaseHelper.ImagePath & "option_red.gif"
			Dim X As Integer
			For X = 1 To 5
				Dim button As HtmlInputRadioButton = Me.FindControl("Radio" & X)
				If Left(button.Value, Len(ClientChoicesState(MClientChoices.ColorScheme))) = ClientChoicesState(MClientChoices.ColorScheme).ToLower Then
					button.Checked = True
					Exit For
				End If
			Next
		End If
	End Sub	'Page_Load

	Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
		Dim X As Integer
		For X = 1 To 5
			Dim button As HtmlInputRadioButton = Me.FindControl("Radio" & X)
			If ((button.Checked)) Then
				Dim colorValues As String = button.Value
				Dim colorList() As String = Split(colorValues, ",")
				ClientChoicesState(MClientChoices.ColorScheme) = colorList(0)
				ClientChoicesState(MClientChoices.HeadColor) = colorList(1)
				ClientChoicesState(MClientChoices.SubheadColor) = colorList(2)
				ClientChoicesState(MClientChoices.BackColor) = colorList(3)
				ClientChoicesState(MClientChoices.LeftColor) = colorList(4)
				Exit For
			End If
		Next
		NavControler.NavTo(Request.QueryString("action"))
	End Sub
End Class
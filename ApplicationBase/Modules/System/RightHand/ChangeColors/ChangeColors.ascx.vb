Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Model.Special.ClientChoices
Imports System.Collections
Imports System.Collections.Specialized

Partial Class ChangeColors
	Inherits ClientChoices.ClientChoicesUserControl

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		If Not IsPostBack Then
            blueImage.Src = BaseSettings.ImagePath & "option_blue.gif"
            greenImage.Src = BaseSettings.ImagePath & "option_green.gif"
            yellowImage.Src = BaseSettings.ImagePath & "option_yellow.gif"
            purpleImage.Src = BaseSettings.ImagePath & "option_purple.gif"
            redImage.Src = BaseSettings.ImagePath & "option_red.gif"
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
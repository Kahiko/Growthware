Imports ApplicationBase.Common.CustomWebControls
Imports ApplicationBase.ClientChoices

Partial Public Class TestHtmlEdit
    Inherits ClientChoicesUserControl

    Protected WithEvents HtmlTextBox1 As HtmlTextBox

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim mike As String = String.Empty
    End Sub
End Class
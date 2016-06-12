
Partial Class RandomNumbers
	Inherits ClientChoices.ClientChoicesUserControl

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
	End Sub

	Private Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
		Dim Numbers As String = String.Empty
		Dim X As Integer = 1
		For X = 1 To CInt(txtAmountOfNumbers.Text)
            Numbers &= BaseHelperOld.GetRandomNumber(CInt(txtMaxNumber.Text), CInt(txtMinNumber.Text)) & ", "
		Next
		litResults.Text &= Left(Numbers, Len(Numbers) - 2) & "<br>"
	End Sub
End Class

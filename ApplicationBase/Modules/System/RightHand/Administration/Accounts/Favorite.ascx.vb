Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Model.Special.ClientChoices

Partial Class Favorite
	Inherits ClientChoices.ClientChoicesUserControl

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim myFavorite As String = ClientChoicesState(MClientChoices.Action)
        If myFavorite.Trim.Length = 0 Then myFavorite = BaseSettings.DefaultAction
		NavControler.NavTo(myFavorite)
	End Sub
End Class

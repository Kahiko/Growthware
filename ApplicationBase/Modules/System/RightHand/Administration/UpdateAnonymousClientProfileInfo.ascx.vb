Imports ApplicationBase.Common.Cache

Partial Class UpdateAnonymousClientProfileInfo
	Inherits System.Web.UI.UserControl

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
	End Sub

	Private Sub btnClearAnonymous_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearAnonymous.Click
		Dim myAccountUtility As New AccountUtility(HttpContext.Current)
		CacheControler.RemoveFromCache(myAccountUtility.AnonymousAccountProfileInfo)
		CacheControler.RemoveFromCache("anonymousMenu")
	End Sub
End Class

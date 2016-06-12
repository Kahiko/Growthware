Imports ApplicationBase.Common.Cache
Imports ApplicationBase.Common.CustomWebControls
Imports ApplicationBase.Model.Accounts
Imports ApplicationBase.Model.Special.ClientChoices
Imports System.Data

Partial Class LineMenu
	Inherits ClientChoices.ClientChoicesUserControl

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim Account As String = Context.User.Identity.Name
		Dim accountProfileInfo As New MAccountProfileInfo
		Dim myAccountUtility As New AccountUtility(HttpContext.Current)
		Dim anonymousMenuCache As String = "anonymousLineMenu"
		Dim BusinessUnitID As Integer = CInt(ClientChoicesState(MClientChoices.BusinessUnitID))
		Account = ClientChoicesState(MClientChoices.AccountName)
		If Account = "" Then
			Account = "Anonymous"
		End If
		accountProfileInfo = myAccountUtility.GetAccountProfileInfo(Account)
		If Not Account.ToLower = "anonymous" Then
			Dim lineMenuDataSet As New DataSet
			lineMenuDataSet = CType(HttpContext.Current.Session("LineMenu"), DataSet)
			If lineMenuDataSet Is Nothing Then
				lineMenuDataSet = NavMenuUtility.GetLineMenuLinks(False, accountProfileInfo.ACCOUNT_SEQ_ID, BusinessUnitID)
				HttpContext.Current.Session("LineMenu") = lineMenuDataSet
			End If
			NavTrail1.DataSource = lineMenuDataSet.Tables(0).DefaultView
		Else
			Dim anonymousMenu As New DataSet
			anonymousMenu = CType(HttpContext.Current.Cache(anonymousMenuCache), DataSet)
			If Not anonymousMenu Is Nothing Then
				NavTrail1.DataSource = anonymousMenu.Tables(0).DefaultView
			Else
				anonymousMenu = NavMenuUtility.GetLineMenuLinks(False, accountProfileInfo.ACCOUNT_SEQ_ID, BusinessUnitID)
				CacheControler.AddToCacheDependency(anonymousMenuCache, anonymousMenu)
				NavTrail1.DataSource = anonymousMenu.Tables(0).DefaultView
			End If
		End If
		NavTrail1.DataBind()
	End Sub

	'Public Sub anchorLogoff_ServerClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles anchorLogoff.ServerClick
	'    Dim myAccountUtility As New AccountUtility(HttpContext.Current)
	'    myAccountUtility.SignOut()
	'    NavControler.NavTo("generichome")
	'End Sub 'anchorLogoff_ServerClick

	'Private Sub anchorUpdate_ServerClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles anchorUpdate.ServerClick
	'	Dim myAccountUtility As New AccountUtility(HttpContext.Current)
	'	myAccountUtility.RemoveAccountInMemoryInformation()
	'	NavControler.NavTo(Request.QueryString("Action") & BaseHelper.GetURL)
	'End Sub	'anchorUpdate_ServerClick

	'Private Sub anchorFavorite_ServerClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles anchorFavorite.ServerClick
	'	Dim myFavorite As String = ClientChoicesState("Action")
	'	If myFavorite.Trim.Length = 0 Then myFavorite = BaseHelper.DefaultAction
	'	NavControler.NavTo(myFavorite)
	'End Sub	'anchorFavorite_ServerClick
End Class

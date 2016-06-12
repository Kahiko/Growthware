Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Model.Accounts.Security
Imports ApplicationBase.Model.Accounts
Imports ApplicationBase.Model.Special.ClientChoices
Imports System.Data

Partial Class ChooseBusinessUnit
	Inherits ClientChoices.ClientChoicesUserControl

	Private NeedRedirect As Boolean = False
	Private RedirectURL As String = String.Empty

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim myDataSet As New DataSet
		Dim myAccountUtility As New AccountUtility(HttpContext.Current)
		Dim myAccountProfileInfo As New MAccountProfileInfo
		myAccountProfileInfo = myAccountUtility.GetAccountProfileInfo(HttpContext.Current.User.Identity.Name)
		Dim myAccountSecurityInfo As New MAccountSecurityInfo
		myAccountUtility.GetAccountSecurityInfo(myAccountSecurityInfo)
		Dim isAdmin As Boolean = False
		If myAccountSecurityInfo.IsSystemAdministrator OrElse HttpContext.Current.User.IsInRole("Super User") Then
			isAdmin = True
		End If
		BusinessUnitUtility.GetValidBusinessUnits(myDataSet, myAccountProfileInfo.ACCOUNT_SEQ_ID, isAdmin)
		dropBusinessUnits.DataSource = myDataSet.Tables(0).DefaultView
		dropBusinessUnits.DataValueField = "BUSINESS_UNIT_SEQ_ID"
		dropBusinessUnits.DataTextField = "Name"
		dropBusinessUnits.DataBind()
        BaseHelperOld.SetDropSelection(dropBusinessUnits, ClientChoicesState(MClientChoices.BusinessUnitID))
		If Not IsPostBack Then
			If Not Context.Session("clientMSG") Is Nothing Then
				clientMSG.Text = Context.Session("clientMSG")
				Context.Session.Remove("clientMSG")
			End If
		End If
	End Sub

	Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
		If NeedRedirect Then NavControler.NavTo(RedirectURL)
	End Sub	' Page_PreRender

	Private Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click
		' save the client state to the database must be done before getting the roles
		ClientChoicesState(MClientChoices.BusinessUnitID) = dropBusinessUnits.SelectedValue
		ClientChoicesState(MClientChoices.BusinessUnitName) = dropBusinessUnits.SelectedItem.Text
		' update all of your in memeory information
		If HttpContext.Current.User.Identity.IsAuthenticated Then
			Dim myAccountUtility As New AccountUtility(HttpContext.Current)
			myAccountUtility.RemoveAccountInMemoryInformation()
		End If
        Context.Session("clientMSG") = "The " & BaseSettings.BusinessUnitTranslation & " has been changed to " & dropBusinessUnits.SelectedItem.Text
		RedirectURL = Request.QueryString("Action")
		NeedRedirect = True
		' refresh the view
		If Not Request.QueryString("ReturnURL") Is Nothing Then NavControler.NavTo(Request.QueryString("ReturnURL"))
	End Sub
End Class
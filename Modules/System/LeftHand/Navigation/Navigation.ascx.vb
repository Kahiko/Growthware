Imports Common.Cache
Imports BLL.Base.ClientChoices
Imports DALModel.Special.Accounts
Imports DALModel.Special.ClientChoices

Public Class Navigation
    Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents NavigationMenu As System.Web.UI.WebControls.Repeater
	Protected WithEvents lnkMenu As System.Web.UI.HtmlControls.HtmlAnchor

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
		Dim Account As String = context.User.Identity.Name
		Dim accountProfileInfo As New MAccountProfileInfo
		Dim myAccountUtility As New AccountUtility(HttpContext.Current)
		Dim anonymousMenuCache As String = "anonymousMenu"
		Dim State As String = ClientChoicesState(MClientChoices.BusinessUnitID)
		Account = ClientChoicesState(MClientChoices.AccountName)
		If Account = "" Then
			Account = "Anonymous"
		End If
		accountProfileInfo = myAccountUtility.GetAccountProfileInfo(Account)
		If Not Account.ToLower = "anonymous" Then
			NavigationMenu.DataSource = NavMenuUtility.GetLinks(False, accountProfileInfo.ACCOUNT_SEQ_ID, State)
		Else
			Dim anonymousMenu As New DataSet
			anonymousMenu = CType(HttpContext.Current.Cache(anonymousMenuCache), DataSet)
			If Not anonymousMenu Is Nothing Then
				NavigationMenu.DataSource = anonymousMenu
			Else
				anonymousMenu = NavMenuUtility.GetLinks(False, accountProfileInfo.ACCOUNT_SEQ_ID, State)
				CacheControler.AddToCacheDependency(anonymousMenuCache, anonymousMenu)
				NavigationMenu.DataSource = anonymousMenu
			End If
		End If
		NavigationMenu.DataBind()
	End Sub	'Page_Load

	Private Sub NavigationMenu_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles NavigationMenu.ItemDataBound
		If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
			Dim myHyperLink As HtmlAnchor = CType(e.Item.FindControl("lnkMenu"), HtmlAnchor)
			Dim myNavImage As HtmlImage = CType(e.Item.FindControl("NavIcon"), HtmlImage)
			If Not (myNavImage Is Nothing) Then
				myNavImage.Attributes.Add("alt", e.Item.DataItem(0))
			End If
            If Not (myHyperLink Is Nothing) Then
                myHyperLink.InnerHtml = e.Item.DataItem(0)
                myHyperLink.Attributes.Add("href", BaseHelper.FQDNBasePage & "?Action=" & Trim(e.Item.DataItem(1)))
            End If
        End If
	End Sub
End Class
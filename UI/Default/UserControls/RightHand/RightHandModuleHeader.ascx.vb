Imports BLL.Base.ClientChoices
Imports DALModel.Base.Accounts.Security
Imports DALModel.Special.ClientChoices
Imports DALModel.Base.Modules

Public Class RightHandModuleHeader
	Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents tdEdit As System.Web.UI.HtmlControls.HtmlTableCell
	Protected WithEvents tdClose As System.Web.UI.HtmlControls.HtmlTableCell
	Protected WithEvents anchorClose As System.Web.UI.HtmlControls.HtmlAnchor
	Protected WithEvents anchorEditPage As System.Web.UI.HtmlControls.HtmlAnchor
	Protected WithEvents imgClose As System.Web.UI.HtmlControls.HtmlImage
	Protected WithEvents imgEdit As System.Web.UI.HtmlControls.HtmlImage
	Public Title As String
	Private _EditPage As String = "# edit"
	Public canEdit As Boolean
	Public canClose As Boolean
	Public ModuleSource As String = ""
	Protected WithEvents trLeftHandModuleHeader As System.Web.UI.HtmlControls.HtmlTableRow
	Protected WithEvents trRightHandModuleHeader As System.Web.UI.HtmlControls.HtmlTableRow
	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object

	Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

	Public Property EditPage() As String
		Get
			Return _EditPage
		End Get
        Set(ByVal Value As String)
            _EditPage = BaseHelper.FQDNBasePage & Value
        End Set
    End Property

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		' code left for future use
		Dim moduleProfileInfo As MModuleProfileInfo = AppModulesUtility.GetCurrentModule()
		Dim myAccountUtility As New AccountUtility(HttpContext.Current)
		Dim clientSecurityInfo As MAccountSecurityInfo = myAccountUtility.GetAccountSecurityInfo(clientSecurityInfo)
		imgClose.Src = BaseHelper.ImagePath & "x.gif"
        imgEdit.Src = BaseHelper.ImagePath & "edit.gif"
		tdEdit.Visible = False
		tdClose.Visible = False
		anchorEditPage.HRef = EditPage
		If canEdit Then
			If Not clientSecurityInfo Is Nothing Then
				If clientSecurityInfo.IsSystemAdministrator Then
					tdEdit.Visible = True
					imgEdit.Visible = True
				End If
			End If
		End If
		If canClose Then
			tdClose.Visible = True
		End If
		Title = moduleProfileInfo.Description
	End Sub
	Private Sub anchorClose_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles anchorClose.ServerClick
		If (String.Compare(ClientChoicesState(MClientChoices.AccountName), "ANONYMOUS") <> 0) Then

			Dim pageIndex As Integer = 0

			If (Not Request.Cookies("_PageIndex") Is Nothing) Then
				pageIndex = Int32.Parse(Request.Cookies("_PageIndex").Value)
			End If

			Dim leftModules As String = ClientChoicesState("PageModules_" + pageIndex.ToString() + "R")
			Dim moduleList() As String = Split(leftModules, ";")

			Dim s As String = ""
			Dim i As Integer

			For i = 0 To moduleList.Length - 1
				If (String.Compare(ModuleSource.ToLower, moduleList(i).ToLower) <> 0) Then
					s += moduleList(i) + ";"
				End If
			Next i

			ClientChoicesState("PageModules_" + pageIndex.ToString() + "R") = TrimEnd(s, ";")
			Cache(MClientChoices.sessionName) = ClientChoicesState()
		End If
	End Sub

	Private Function TrimEnd(ByVal source As String, ByVal trimchar As String) As String
		Dim tc() As Char = trimchar.ToCharArray()
		TrimEnd = source.TrimEnd(tc)
	End Function	'TrimEnd
End Class

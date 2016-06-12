Imports ApplicationBase.Common.Globals
Imports ApplicationBase.Model.Accounts.Security
Imports ApplicationBase.Model.Special.ClientChoices
Imports ApplicationBase.Model.Modules

Partial Class RightHandModuleHeader
	Inherits ClientChoices.ClientChoicesUserControl

	Public Title As String
	Private _EditPage As String = "# edit"
	Public canEdit As Boolean
	Public canClose As Boolean
	Public ModuleSource As String = ""

	Public Property EditPage() As String
		Get
			Return _EditPage
		End Get
		Set(ByVal Value As String)
            _EditPage = BaseSettings.FQDNPage & Value
		End Set
	End Property

	Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		' code left for future use
		Dim moduleProfileInfo As MModuleProfileInfo = AppModulesUtility.GetCurrentModule()
		Dim myAccountUtility As New AccountUtility(HttpContext.Current)
		Dim clientSecurityInfo As New MAccountSecurityInfo
		myAccountUtility.GetAccountSecurityInfo(clientSecurityInfo)
        imgClose.Src = BaseSettings.ImagePath & "x.gif"
        imgEdit.Src = BaseSettings.ImagePath & "edit.gif"
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
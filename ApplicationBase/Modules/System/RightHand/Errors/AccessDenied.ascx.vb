Imports ApplicationBase.Model.Modules

Partial Class AccessDenied
	Inherits ClientChoices.ClientChoicesUserControl

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
		' get a module profile information class
		Dim moduleProfileInfo As MModuleProfileInfo
		' get a modulecollection 
		Dim moduleCollection As New MModuleCollection
		If Not IsPostBack Then
			Try
				' get the module collection from cache
				' cache has been loaded by global.asax.vb
				moduleProfileInfo = AppModulesUtility.GetModuleInfoByAction(Request.QueryString("RequestedAction").ToLower)
				If Context.Request.IsAuthenticated Then
					requesedAction.InnerHtml = moduleProfileInfo.Name
					insufficientRights.Visible = True
				Else
					requesedAction1.InnerHtml = moduleProfileInfo.Name
					mustLogon.Visible = True
				End If
			Catch ex As Exception
				' do nothing
			End Try
		End If
	End Sub
End Class
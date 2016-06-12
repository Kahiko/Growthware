Imports ApplicationBase.Model.Modules
Imports ApplicationBase.Common.CustomWebControls

Partial Class ModulesSecurity
	Inherits System.Web.UI.UserControl

	Public Modules As MModuleProfileInfo

	Public AllRoles As ArrayList



	Public ReadOnly Property ViewRoles() As String()
		Get
			Return ctlViewRoles.SelectedItems
		End Get
	End Property

	Public ReadOnly Property ViewRolesChanged() As Boolean
		Get
			Return ctlViewRoles.Changed
		End Get
	End Property

	Public ReadOnly Property AddRoles() As String()
		Get
			Return ctlAddRoles.SelectedItems
		End Get
	End Property

	Public ReadOnly Property AddRolesChanged() As Boolean
		Get
			Return ctlAddRoles.Changed
		End Get
	End Property


	Public ReadOnly Property EditRoles() As String()
		Get
			Return ctlEditRoles.SelectedItems
		End Get
	End Property

	Public ReadOnly Property EditRolesChanged() As Boolean
		Get
			Return ctlEditRoles.Changed
		End Get
	End Property


	Public ReadOnly Property DeleteRoles() As String()
		Get
			Return ctlDeleteRoles.SelectedItems
		End Get
	End Property

	Public ReadOnly Property DeleteRolesChanged() As Boolean
		Get
			Return ctlDeleteRoles.Changed
		End Get
	End Property
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
		If Not IsPostBack Then
			ctlViewRoles.DataSource = AllRoles
			ctlAddRoles.DataSource = AllRoles
			ctlEditRoles.DataSource = AllRoles
			ctlDeleteRoles.DataSource = AllRoles

			If Not (Modules Is Nothing) Then
				ctlViewRoles.SelectedItems = Modules.ViewRoles
				ctlAddRoles.SelectedItems = Modules.AddRoles
				ctlEditRoles.SelectedItems = Modules.EditRoles
				ctlDeleteRoles.SelectedItems = Modules.DeleteRoles
			End If

			DataBind()
		End If
	End Sub

End Class

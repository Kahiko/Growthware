Imports ApplicationBase.Model.Special.ClientChoices
Imports ApplicationBase.Common.CustomWebControls

Partial Class RolesControl
	Inherits ClientChoices.ClientChoicesUserControl

	Public AllRoles As ArrayList
	Public SelectedViewRoles As Array
	Public SelectedAddRoles As Array
	Public SelectedEditRoles As Array
	Public SelectedDeleteRoles As Array

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
			If Not SelectedViewRoles Is Nothing Then ctlViewRoles.SelectedItems = SelectedViewRoles
			If Not SelectedAddRoles Is Nothing Then ctlAddRoles.SelectedItems = SelectedAddRoles
			If Not SelectedEditRoles Is Nothing Then ctlEditRoles.SelectedItems = SelectedEditRoles
			If Not SelectedDeleteRoles Is Nothing Then ctlDeleteRoles.SelectedItems = SelectedDeleteRoles
			DataBind()
		End If
	End Sub
End Class
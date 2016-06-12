Imports ApplicationBase.Model.Group
Imports ApplicationBase.BusinessLogic
Imports ApplicationBase.Model.Special.ClientChoices

Partial Class GroupRoles
	Inherits ClientChoices.ClientChoicesUserControl

	Public ReadOnly Property Roles() As String()
		Get
			Return ctlRoles.SelectedItems
		End Get
	End Property

	Public ReadOnly Property RolesChanged() As Boolean
		Get
			Return ctlRoles.Changed
		End Get
	End Property

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
		If Not IsPostBack Then
			' Get roles
			If Not Request.QueryString("groupid") Is Nothing Then
				ctlRoles.SelectedItems = BGroups.GetRolesByBusinessUnit(Request.QueryString("groupid"), ClientChoicesState(MClientChoices.BusinessUnitID))
			End If
			ctlRoles.DataSource = BusinessUnitUtility.GetAllRolesForBusinessUnit(ClientChoicesState(MClientChoices.BusinessUnitID)).Tables(0).DefaultView
			ctlRoles.DataField = "Role_Name"
			ctlRoles.DataBind()
		End If
	End Sub
End Class

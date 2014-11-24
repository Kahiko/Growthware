Public Class RolesControl
    Inherits System.Web.UI.UserControl

    Public AllRoles As ArrayList
    Public SelectedViewRoles As Array
    Public SelectedAddRoles As Array
    Public SelectedEditRoles As Array
    Public SelectedDeleteRoles As Array

    Public ReadOnly Property ViewRolesChanged() As Boolean
        Get
            Return ctlViewRoles.Changed
        End Get
    End Property

    Public ReadOnly Property ViewRoles() As String
        Get
            Return ctlViewRoles.SelectedState
        End Get
    End Property

    Public ReadOnly Property AddRoles() As String
        Get
            Return ctlAddRoles.SelectedState
        End Get
    End Property

    Public ReadOnly Property AddRolesChanged() As Boolean
        Get
            Return ctlAddRoles.Changed
        End Get
    End Property

    Public ReadOnly Property EditRoles() As String
        Get
            Return ctlEditRoles.SelectedState
        End Get
    End Property

    Public ReadOnly Property EditRolesChanged() As Boolean
        Get
            Return ctlEditRoles.Changed
        End Get
    End Property

    Public ReadOnly Property DeleteRoles() As String
        Get
            Return ctlDeleteRoles.SelectedState
        End Get
    End Property

    Public ReadOnly Property DeleteRolesChanged() As Boolean
        Get
            Return ctlDeleteRoles.Changed
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ctlViewRoles.DataSource = AllRoles
        ctlAddRoles.DataSource = AllRoles
        ctlEditRoles.DataSource = AllRoles
        ctlDeleteRoles.DataSource = AllRoles
        If Not SelectedViewRoles Is Nothing Then ctlViewRoles.SelectedItems = SelectedViewRoles
        If Not SelectedAddRoles Is Nothing Then ctlAddRoles.SelectedItems = SelectedAddRoles
        If Not SelectedEditRoles Is Nothing Then ctlEditRoles.SelectedItems = SelectedEditRoles
        If Not SelectedDeleteRoles Is Nothing Then ctlDeleteRoles.SelectedItems = SelectedDeleteRoles
        DataBind()
    End Sub

End Class
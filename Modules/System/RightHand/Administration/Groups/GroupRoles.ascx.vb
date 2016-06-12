Imports DALModel.Base.Group
Imports BLL.Base.SQLServer
Imports DALModel.Special.ClientChoices

Public Class GroupRoles
    Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents ctlRoles As Common.CustomWebControls.ListPicker

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

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

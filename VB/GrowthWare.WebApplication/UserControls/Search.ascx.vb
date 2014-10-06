Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport
Imports GrowthWare.Framework.Common

Public Class Search
    Inherits System.Web.UI.UserControl

    Public ReadOnly Property ClientChoicesState() As MClientChoicesState
        Get
            Return CType(Context.Items(MClientChoices.SessionName), MClientChoicesState)
        End Get
    End Property

    Private m_ShowAddLink As Boolean = True
    Private m_ShowSelect As Boolean = False
    Private m_ShowDeleteAll As Boolean = False
    Private m_ShowRefresh As Boolean = False

    Public Property SearchURL() As String

    Public Property ShowAddLink() As Boolean
        Get
            Return m_ShowAddLink
        End Get
        Set(ByVal value As Boolean)
            m_ShowAddLink = value
        End Set
    End Property

    Public Property ShowSelect() As Boolean
        Get
            Return m_ShowSelect
        End Get
        Set(value As Boolean)
            m_ShowSelect = value
        End Set
    End Property

    Public Property ShowDeleteAll() As Boolean
        Get
            Return m_ShowDeleteAll
        End Get
        Set(value As Boolean)
            m_ShowDeleteAll = value
        End Set
    End Property

    Public Property ShowRefresh() As Boolean
        Get
            Return m_ShowRefresh
        End Get
        Set(value As Boolean)
            m_ShowRefresh = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not m_ShowDeleteAll Then imgDeleteAll.Style.Add("display", "none")
        If Not m_ShowSelect Then cmdSelect.Style.Add("display", "none")
        addNew.Visible = m_ShowAddLink
        btnRefesh.Visible = m_ShowRefresh
        If ClientChoicesState(MClientChoices.RecordsPerPage) <> Nothing Then
            txtRecordsPerPage.Value = ClientChoicesState(MClientChoices.RecordsPerPage)
        Else
            txtRecordsPerPage.Value = "10"
        End If
        imgDeleteAll.Src = GWWebHelper.RootSite + "Public/Images/GrowthWare/delete_red.png"
    End Sub

End Class
Imports DALModel.Base.Group
Imports BLL.Base.SQLServer
Imports DALModel.Special.ClientChoices

Public Class GroupsGeneral
    Inherits ClientChoices.ClientChoicesUserControl

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtGroup_seq_id As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents litGroup As System.Web.UI.WebControls.Literal
    Protected WithEvents txtGrpName As System.Web.UI.WebControls.TextBox
    Protected WithEvents litGroupWarning As System.Web.UI.WebControls.Literal
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents txtGrpDescription As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnSave As System.Web.UI.WebControls.Button

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private _updatedGroupSeqId As Integer
    Private _updatedGroupName As String

    Public Property UpdatedGroupSeqId() As Integer
        Get
            Return _updatedGroupSeqId
        End Get
        Set(ByVal Value As Integer)
            _updatedGroupSeqId = Value
        End Set
    End Property

    Public Property UpdatedGroupName() As String
        Get
            Return _updatedGroupName
        End Get
        Set(ByVal Value As String)
            _updatedGroupName = Value
        End Set
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            Dim groupInfoToUpdate As MGroupInfo
            GetGroupInfoInstance(groupInfoToUpdate)
            PopulatePage(groupInfoToUpdate)
        End If
    End Sub

    Public Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim TheNewId As Integer
        Page.Validate()
        If Page.IsValid Then
            Dim groupInfoToUpdate As MGroupInfo

            GetGroupInfoInstance(groupInfoToUpdate)
            PopulateFromPage(groupInfoToUpdate)

            groupInfoToUpdate.BusinessUnitId = ClientChoicesState(MClientChoices.BusinessUnitID)

            If Request.QueryString("Action").ToLower = "addgroupinfo" Then
                TheNewId = BGroups.AddGroup(groupInfoToUpdate)
                groupInfoToUpdate.GroupId = TheNewId
            Else
                BGroups.UpdateAGroup(groupInfoToUpdate)
            End If
            UpdatedGroupSeqId = groupInfoToUpdate.GroupId
            UpdatedGroupName = groupInfoToUpdate.GroupName
        End If
    End Sub ' btnSave_Click

    Private Function PopulatePage(ByVal groupInfo As MGroupInfo) As MGroupInfo
        With groupInfo
            txtGrpName.Text = groupInfo.GroupName.Trim
            txtGrpDescription.Text = groupInfo.GroupDescription.Trim
        End With

    End Function

    Private Sub PopulateFromPage(ByRef groupInfo As MGroupInfo)
        With groupInfo
            .GroupName = txtGrpName.Text
            .GroupDescription = txtGrpDescription.Text
        End With
    End Sub

    Private Sub GetGroupInfoInstance(ByRef groupInfo As MGroupInfo)
        If Request.QueryString("Action").IndexOf("edit") > -1 Then
            If Not Request.QueryString("groupId") Is Nothing Then
                groupInfo = BGroups.GetGroupInfo(Request.QueryString("groupId"))
            End If
        Else
            groupInfo = New MGroupInfo
        End If
    End Sub
End Class
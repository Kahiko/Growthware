Imports ApplicationBase.Model.Group
Imports ApplicationBase.BusinessLogic
Imports ApplicationBase.Model.Special.ClientChoices

Partial Class GroupsGeneral
	Inherits ClientChoices.ClientChoicesUserControl

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
			Dim groupInfoToUpdate As New MGroupInfo
			GetGroupInfoInstance(groupInfoToUpdate)
			PopulatePage(groupInfoToUpdate)
		End If
	End Sub

	Public Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
		Dim TheNewId As Integer
		Page.Validate()
		If Page.IsValid Then
			Dim groupInfoToUpdate As New MGroupInfo

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
	End Sub	' btnSave_Click

	Private Function PopulatePage(ByVal groupInfo As MGroupInfo) As MGroupInfo
		With groupInfo
			txtGrpName.Text = groupInfo.GroupName.Trim
			txtGrpDescription.Text = groupInfo.GroupDescription.Trim
		End With
		Return groupInfo
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
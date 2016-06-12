Imports ApplicationBase.BusinessLogic
Imports ApplicationBase.Model.Group
Imports ApplicationBase.Model.Special.ClientChoices
Imports ApplicationBase.Model.Accounts.Security

Partial Class AddEditGroups
	Inherits ClientChoices.ClientChoicesUserControl

	Protected WithEvents myGroupsGeneral As GroupsGeneral
	Protected WithEvents myGroupsRoles As GroupRoles

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim myAccountUtility As New AccountUtility(HttpContext.Current)
		Dim accountSecurityInfo As New MAccountSecurityInfo
		myAccountUtility.GetAccountSecurityInfo(accountSecurityInfo)
		Dim groupInfoTobesaved As New MGroupInfo
		If Not IsPostBack Then
			' get the client security information
			If Not accountSecurityInfo.MayEdit And Not accountSecurityInfo.MayAdd Then btnSave.Visible = False
			bottomTabStrip.BgColor = ClientChoicesState(MClientChoices.HeadColor)
		End If
		If Request.QueryString("Action").Trim.ToLower.IndexOf("edit") > -1 Then
			If Not Request.QueryString("groupId") Is Nothing Then
				groupInfoTobesaved = BGroups.GetGroupInfo(Request.QueryString("groupId"))
			End If
		End If
		If groupInfoTobesaved.GroupName.Trim.Length > 0 Then
			litGroupName.Text = groupInfoTobesaved.GroupName
		Else
			litGroupName.Text = "New"
		End If

	End Sub

	Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
		Page.Validate()
		If Page.IsValid Then
			' Update the general information
			myGroupsGeneral.btnSave_Click(sender, e)
			Dim mySeq_id As Integer = myGroupsGeneral.UpdatedGroupSeqId
			' Update Roles
			If myGroupsRoles.RolesChanged OrElse Request.QueryString("Action").ToLower = "addgroupinfo" Then
				Try
					BGroups.UpdateRoles(mySeq_id, ClientChoicesState(MClientChoices.BusinessUnitID), myGroupsRoles.Roles)
				Catch ex As Exception
					Throw New ApplicationException("Error Writing to the DATABASE", ex)
				End Try
			End If
			NavControler.NavTo("editgroupinfo&groupid=" & myGroupsGeneral.UpdatedGroupSeqId)
		End If
	End Sub

End Class

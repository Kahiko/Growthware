Imports GrowthWare.WebSupport.Base
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.Framework.Common

Public Class EditGroupMembers
    Inherits ClientChoicesPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim updatingAccount As MAccountProfile = Nothing
        Dim mGroupProfile As MGroupProfile = GroupUtility.GetProfile(CInt(HttpContext.Current.Request("GroupSeqId")))
        txtEditID.Text = mGroupProfile.Id
        litGroup.Text = mGroupProfile.Name
        updatingAccount = AccountUtility.CurrentProfile()
        HttpContext.Current.Session.Add("EditId", mGroupProfile.Id)
        Dim myDataView As DataView = RoleUtility.GetAllRolesBySecurityEntity(ClientChoicesState(MClientChoices.SecurityEntityId)).DefaultView
        If Not updatingAccount.IsSystemAdmin Then
            Dim rowFilter As String = "IS_SYSTEM <> 1 AND IS_SYSTEM_ONLY <> 1"
            myDataView.RowFilter = rowFilter
        End If
        Try
            Dim myGroupProfile As New MGroupProfile
            myGroupProfile = GroupUtility.GetProfile(txtEditID.Text)
            litGroup.Text = myGroupProfile.Name
            Dim myProfile As New MGroupRoles
            myProfile.SecurityEntityId = ClientChoicesState(MClientChoices.SecurityEntityId)
            myProfile.GroupSeqId = txtEditID.Text
            ctlMembers.SelectedItems = GroupUtility.GetSelectedRoles(myProfile)
        Catch ex As Exception
            Dim log As Logger = Logger.Instance()
            log.Debug(ex)
        End Try
        ctlMembers.DataSource = myDataView
        ctlMembers.DataField = "Name"
        ctlMembers.DataBind()
    End Sub

End Class
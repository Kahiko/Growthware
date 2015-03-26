Imports GrowthWare.WebSupport.Base
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.Framework.Common

Public Class EditGroupMembers
    Inherits ClientChoicesPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim updatingAccount As MAccountProfile = Nothing
        'txtEditID.Text = GWWebHelper.GetQueryValue(HttpContext.Current.Request, GWWebHelper.GROUP_DATA_KEY_FIELD)
        txtEditID.Text = 1
        litGroup.Text = GroupUtility.GetProfile(CInt(txtEditID.Text)).Name
        updatingAccount = AccountUtility.CurrentProfile()
        'Dim myBRoles As New BRoles(SecurityEntityUtility.CurrentProfile, WebConfigSettings.CentralManagement)
        'Dim myBGroups As New BGroups(SecurityEntityUtility.CurrentProfile, WebConfigSettings.CentralManagement)

        'Dim myDataView As DataView = myBRoles.GetRolesBySecurityEntity(ClientChoicesState(MClientChoices.SecurityEntityId)).DefaultView
        'If Not updatingAccount.IsSystemAdmin Then
        '    Dim rowFilter As String = "IS_SYSTEM <> 1 AND IS_SYSTEM_ONLY <> 1"
        '    myDataView.RowFilter = rowFilter
        'End If
        'Try
        '    Dim myGroupProfile As New MGroupProfile
        '    myGroupProfile = GroupUtility.GetProfile(txtEditID.Text)
        '    litGroup.Text = myGroupProfile.Name
        '    Dim myProfile As New MGroupRoles
        '    myProfile.SecurityEntityId = ClientChoicesState(MClientChoices.SecurityEntityId)
        '    myProfile.GroupSeqId = txtEditID.Text
        '    ctlMembers.SelectedItems = myBGroups.GetSelectedRoles(myProfile)
        'Catch ex As Exception
        '    Dim log As Logger = Logger.Instance()
        '    log.Debug(ex)
        'End Try
        'ctlMembers.DataSource = myDataView
        'ctlMembers.DataField = "Name"
        'ctlMembers.DataBind()
    End Sub

End Class
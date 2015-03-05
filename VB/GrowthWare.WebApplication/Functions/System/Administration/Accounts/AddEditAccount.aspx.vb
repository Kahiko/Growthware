Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.WebSupport
Imports GrowthWare.WebSupport.Base
Imports System.Globalization

Public Class AddEditAccount
    Inherits ClientChoicesPage

    'Private m_Profile As MAccountProfile = Nothing
    'Private m_Action As String = Nothing

    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    btnSave.Visible = False
    '    m_Action = GWWebHelper.GetQueryValue(Request, "Action")
    '    If Not String.IsNullOrEmpty(Request.QueryString("AccountSeqID")) Then
    '        Dim mAccountSeqID As Integer = Integer.Parse(Request.QueryString("AccountSeqID").ToString())
    '        If Not mAccountSeqID = -1 Then
    '            m_Profile = AccountUtility.GetProfile(mAccountSeqID)
    '        Else
    '            m_Profile = New MAccountProfile()
    '        End If
    '    Else
    '        m_Profile = AccountUtility.CurrentProfile()
    '        btnSave.Visible = True
    '        hdnCanSaveRoles.Value = False
    '        hdnCanSaveGroups.Value = False
    '        hdnCanSaveStatus.Value = False
    '        tdStatus.Style.Add("display", "none")
    '        dropStatus.Style.Add("display", "none")
    '        If m_Action.ToUpper(CultureInfo.InvariantCulture) = "REGISTER" Then
    '            m_Profile = New MAccountProfile()
    '            trAccount.Visible = False
    '            tabsDerivedRoles.Visible = False
    '            derivedRolesTab.Visible = False
    '        End If
    '    End If
    '    HttpContext.Current.Session.Add("EditId", m_Profile.Id)
    '    populatePage()
    'End Sub

    'Private Sub populatePage()
    '    populateGeneral()
    '    populateRoles()
    '    populateGroups()
    '    If String.IsNullOrEmpty(hdnCanSaveStatus.Value.ToString()) Then
    '        hdnCanSaveStatus.Value = True
    '    End If
    '    Dim mRoleTabSecurity As MSecurityInfo = New MSecurityInfo(FunctionUtility.GetProfile("View_Account_Role_Tab"), AccountUtility.CurrentProfile)
    '    Dim mGroupTabSecurity As MSecurityInfo = New MSecurityInfo(FunctionUtility.GetProfile("View_Account_Group_Tab"), AccountUtility.CurrentProfile)
    '    If String.IsNullOrEmpty(hdnCanSaveRoles.Value.ToString()) Then
    '        hdnCanSaveRoles.Value = mRoleTabSecurity.MayView
    '        tabsRoles.Visible = mRoleTabSecurity.MayView
    '        rolesTab.Visible = mRoleTabSecurity.MayView
    '    Else
    '        hdnCanSaveRoles.Value = False
    '        tabsRoles.Visible = False
    '        rolesTab.Visible = False
    '    End If
    '    If String.IsNullOrEmpty(hdnCanSaveGroups.Value.ToString()) Then
    '        hdnCanSaveGroups.Value = mGroupTabSecurity.MayView
    '        tabsGroups.Visible = mGroupTabSecurity.MayView
    '        groupsTab.Visible = mGroupTabSecurity.MayView
    '    Else
    '        hdnCanSaveGroups.Value = False
    '        tabsGroups.Visible = False
    '        groupsTab.Visible = False
    '    End If
    '    trSysAdmin.Visible = AccountUtility.CurrentProfile.IsSystemAdmin
    'End Sub

    'Private Sub populateGeneral()
    '    txtAccount_seq_id.Value = m_Profile.Id
    '    txtAccount_seq_id.Style.Add("display", "none")
    '    txtAccount.Text = m_Profile.Account
    '    chkSysAdmin.Checked = m_Profile.IsSystemAdmin
    '    litFailedAttempts.Text = m_Profile.FailedAttempts
    '    txtFailedAttempts.Text = m_Profile.FailedAttempts
    '    txtFirstName.Text = m_Profile.FirstName
    '    txtLastName.Text = m_Profile.LastName
    '    txtMiddleName.Text = m_Profile.MiddleName
    '    txtPreferredName.Text = m_Profile.PreferredName
    '    txtEmail.Text = m_Profile.Email
    '    txtLocation.Text = m_Profile.Location
    '    chkEnableNotifications.Checked = m_Profile.EnableNotifications
    '    NameValuePairUtility.SetDropSelection(dropStatus, m_Profile.Status.ToString())
    '    NameValuePairUtility.SetDropSelection(dropTimezone, m_Profile.TimeZone.ToString())
    'End Sub

    'Private Sub populateRoles()
    '    ctlRoles.DataSource = RoleUtility.GetRolesArrayListBySecurityEntity(SecurityEntityUtility.CurrentProfile().Id)
    '    ctlRoles.SelectedItems = m_Profile.AssignedRoles.ToArray()
    '    ctlRoles.DataBind()
    '    For Each role As String In m_Profile.DerivedRoles
    '        lstBoxRoles.Items.Add(role)
    '    Next
    'End Sub

    'Private Sub populateGroups()
    '    ctlGroups.DataSource = GroupUtility.GetGroupsArrayListBySecurityEntity(SecurityEntityUtility.CurrentProfile().Id)
    '    ctlGroups.SelectedItems = m_Profile.Groups.ToArray()
    '    ctlGroups.DataBind()
    'End Sub

End Class
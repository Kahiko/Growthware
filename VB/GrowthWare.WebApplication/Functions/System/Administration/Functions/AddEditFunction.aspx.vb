Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport
Imports GrowthWare.WebSupport.BasePages

Public Class AddEditFunction
    Inherits BaseWebpage

    Private m_Profile As MFunctionProfile = Nothing

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not String.IsNullOrEmpty(Request.QueryString("FunctionSeqID")) Then
            Dim mFunctionSeqID As Integer = Integer.Parse(Request.QueryString("FunctionSeqID").ToString())
            If Not mFunctionSeqID = -1 Then
                m_Profile = FunctionUtility.GetProfile(mFunctionSeqID)
            Else
                m_Profile = New MFunctionProfile()
            End If
            HttpContext.Current.Session.Add("EditId", m_Profile.Id)
            populatePage()
        End If
    End Sub

    Private Sub populatePage()
        If m_Profile.Id = -1 Then
            divAction.Visible = False
            txtAction.Visible = True
            txtSource.Visible = True
        End If
        populateGeneral()
        populateFucntionDrop()
        populateFunctionTypes()
        populateNavigationTypes()
        populateDerivedRoles()
        populateLinkBehaviors()
        populateParent()
        populateDirectoryInformation()
    End Sub

    Private Sub populateGeneral()
        divFunctionSeqId.InnerHtml = m_Profile.Id
        txtName.Value = m_Profile.Name
        txtDescription.Value = m_Profile.Description
        txtNotes.Text = m_Profile.Notes
        txtKeyWords.Text = m_Profile.MetaKeywords

        divAction.InnerHtml = m_Profile.Action
        divAction.Visible = True
        txtAction.Text = m_Profile.Action

        txtSource.Text = m_Profile.Source

        chkEnableViewState.Checked = m_Profile.EnableViewState
        chkEnableNotifications.Checked = m_Profile.EnableNotifications
        chkRedirectOnTimeout.Checked = m_Profile.RedirectOnTimeout
        chkNoUI.Checked = m_Profile.NoUI
        chkIsNav.Checked = m_Profile.IsNavigable

        RolesControl.AllRoles = RoleUtility.GetRolesArrayListBySecurityEntity(SecurityEntityUtility.CurrentProfile().Id)
        RolesControl.SelectedAddRoles = m_Profile.AssignedAddRoles.ToArray()
        RolesControl.SelectedDeleteRoles = m_Profile.AssignedDeleteRoles.ToArray()
        RolesControl.SelectedEditRoles = m_Profile.AssignedEditRoles.ToArray()
        RolesControl.SelectedViewRoles = m_Profile.AssignedViewRoles.ToArray()

        GroupsControl.AllGroups = GroupUtility.GetGroupsArrayListBySecurityEntity(SecurityEntityUtility.CurrentProfile().Id)
        GroupsControl.SelectedAddGroups = m_Profile.AddGroups.ToArray()
        GroupsControl.SelectedDeleteGroups = m_Profile.DeleteGroups.ToArray()
        GroupsControl.SelectedEditGroups = m_Profile.EditGroups.ToArray()
        GroupsControl.SelectedViewGroups = m_Profile.ViewGroups.ToArray()
    End Sub

    Private Sub populateFucntionDrop()
        Dim mDataView As DataView = FunctionUtility.GetFunctionMenuOrder(m_Profile).DefaultView
        If mDataView.Count > 0 Then
            mDataView.Sort = "[Name] ASC"
            dropFunctions.DataSource = mDataView
            dropFunctions.DataValueField = "FUNCTION_SEQ_ID"
            dropFunctions.DataTextField = "NAME"
            dropFunctions.DataBind()
            If m_Profile.Id <> -1 Then
                NameValuePairUtility.SetDropSelection(dropFunctions, m_Profile.Id.ToString())
            End If

        End If
    End Sub

    Private Sub populateFunctionTypes()
        dropFunctionType.DataSource = FunctionTypeUtility.FunctionTypes()
        dropFunctionType.DataTextField = "NAME"
        dropFunctionType.DataValueField = "FUNCTION_TYPE_SEQ_ID"
        dropFunctionType.DataBind()
        If m_Profile.Id <> -1 Then
            NameValuePairUtility.SetDropSelection(dropFunctionType, m_Profile.FunctionTypeSeqId.ToString())
        End If
    End Sub

    Private Sub populateNavigationTypes()
        Dim mDataTable As DataTable = Nothing
        Dim mNavType As Integer = GWWebHelper.LinkBehaviorNavigationTypesSequenceId
        NameValuePairUtility.GetNameValuePairDetails(mDataTable, mNavType)
        dropNavType.DataSource = mDataTable
        dropNavType.DataTextField = "NVP_DET_TEXT"
        dropNavType.DataValueField = "NVP_SEQ_DET_ID"
        dropNavType.DataBind()
        If m_Profile.Id <> -1 Then
            NameValuePairUtility.SetDropSelection(dropNavType, m_Profile.NavigationTypeSeqId.ToString())
        End If
        mDataTable.Dispose()
    End Sub

    Private Sub populateLinkBehaviors()
        Dim mDataTable As DataTable = Nothing
        Dim mNavType As Integer = GWWebHelper.LinkBehaviorNameValuePairSequenceId
        NameValuePairUtility.GetNameValuePairDetails(mDataTable, mNavType)
        dropLinkBehavior.DataSource = mDataTable
        dropLinkBehavior.DataTextField = "NVP_DET_TEXT"
        dropLinkBehavior.DataValueField = "NVP_SEQ_DET_ID"
        dropLinkBehavior.DataBind()
        If m_Profile.Id <> -1 Then
            NameValuePairUtility.SetDropSelection(dropLinkBehavior, m_Profile.LinkBehavior.ToString())
        End If
        mDataTable.Dispose()
    End Sub

    Private Sub populateDerivedRoles()
        For Each role As String In m_Profile.DerivedAddRoles
            lstBoxAddRoles.Items.Add(role)
        Next
        For Each role As String In m_Profile.DerivedDeleteRoles
            lstBoxDeleteRoles.Items.Add(role)
        Next
        For Each role As String In m_Profile.DerivedEditRoles
            lstBoxEditRoles.Items.Add(role)
        Next
        For Each role As String In m_Profile.DerivedViewRoles
            lstBoxViewRoles.Items.Add(role)
        Next

    End Sub

    Private Sub populateParent()
        Dim mResult = From mProfile In FunctionUtility.Functions() Where mProfile.ParentId <> m_Profile.Id Select mProfile
        dropNavParent.DataValueField = "Id"
        dropNavParent.DataTextField = "Name"
        dropNavParent.DataSource = mResult
        dropNavParent.DataBind()
        If m_Profile.Id <> -1 Then
            NameValuePairUtility.SetDropSelection(dropNavParent, m_Profile.ParentId)
        End If
    End Sub

    Private Sub populateDirectoryInformation()
        Dim mProfile As MDirectoryProfile = DirectoryUtility.GetProfile(m_Profile.Id)
        If mProfile Is Nothing Then
            mProfile = New MDirectoryProfile()
        End If
        txtDirectory.Text = mProfile.Directory
        chkImpersonation.Checked = mProfile.Impersonate
        txtAccount.Text = mProfile.ImpersonateAccount
        txtPassword.Text = mProfile.ImpersonatePassword
        txtHidPwd.Text = mProfile.ImpersonatePassword
    End Sub

End Class
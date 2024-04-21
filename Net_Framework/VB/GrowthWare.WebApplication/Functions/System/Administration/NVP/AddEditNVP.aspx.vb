Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.WebSupport.Base
Imports GrowthWare.Framework.Common

Public Class AddEditNVP
    Inherits ClientChoicesPage

    Private m_NVPToUpdate As MNameValuePair = Nothing

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not String.IsNullOrEmpty(Request.QueryString("NVP_SEQ_ID")) Then
            Dim mNameValuePairSeqID As Integer = Integer.Parse(Request.QueryString("NVP_SEQ_ID").ToString())
            If Not mNameValuePairSeqID = -1 Then
                m_NVPToUpdate = NameValuePairUtility.GetNameValuePair(mNameValuePairSeqID)
            Else
                m_NVPToUpdate = New MNameValuePair()
            End If
            HttpContext.Current.Session.Add("EditId", m_NVPToUpdate.Id)
        Else
            m_NVPToUpdate = New MNameValuePair()
            hdnCanSaveRoles.Value = False
            hdnCanSaveGroups.Value = False
            tdStatus.Style.Add("display", "none")
            dropStatus.Style.Add("display", "none")
        End If
        populatePage(m_NVPToUpdate)
    End Sub

    Private Sub populatePage(ByVal profile As MNameValuePair)
        populateGeneral(profile)
        populateRolesAndGroups(profile)
    End Sub

    Private Sub populateGeneral(ByVal profile As MNameValuePair)
        txtNVP_SEQ_ID.Text = profile.Id
        txtSchemaName.Text = profile.SchemaName
        litSchemaName.Text = profile.SchemaName
        txtSTATIC_NAME.Text = profile.StaticName
        litStaticName.Text = profile.StaticName
        txtDisplay.Text = profile.Display
        If Not String.IsNullOrEmpty(profile.SchemaName) Then
            lblTableName.InnerHtml = profile.SchemaName + "." + profile.StaticName
        End If
        txtDescription.Text = profile.Description
        If profile.Id > -1 Then
            txtSchemaName.Style.Add("display", "none")
            RequiredFieldValidator1.Enabled = False
            Alphanumeric.Enabled = False
            txtSTATIC_NAME.Style.Add("display", "none")
            RequiredFieldValidator2.Enabled = False
            Alphanumeric2.Enabled = False
            litSchemaName.Visible = True
            litStaticName.Visible = True
        Else
            txtSchemaName.Style.Add("display", "")
            txtSTATIC_NAME.Style.Add("display", "")
            litSchemaName.Visible = False
            litStaticName.Visible = False

        End If
        NameValuePairUtility.SetDropSelection(dropStatus, profile.Status)
    End Sub

    Private Sub populateRolesAndGroups(ByVal profile As MNameValuePair)
        Dim mDVRoles As DataView = RoleUtility.GetAllRolesBySecurityEntity(ClientChoicesState(MClientChoices.SecurityEntityId)).DefaultView()
        Dim mDVGroups As DataView = GroupUtility.GetAllGroupsBySecurityEntity(ClientChoicesState(MClientChoices.SecurityEntityId)).DefaultView
        Try
            ctlGroups.SelectedItems = NameValuePairUtility.GetSelectedGroups(m_NVPToUpdate.Id)
            ctlRoles.SelectedItems = NameValuePairUtility.GetSelectedRoles(m_NVPToUpdate.Id)
        Catch ex As DataException
            Dim mLog As Logger = Logger.Instance()
            mLog.Debug(ex)
        End Try
        ctlGroups.DataSource = mDVGroups
        ctlGroups.DataField = "Name"
        ctlGroups.DataBind()
        ctlRoles.DataSource = mDVRoles
        ctlRoles.DataField = "Name"
        ctlRoles.DataBind()
    End Sub

End Class
Imports GrowthWare.WebSupport
Imports GrowthWare.WebSupport.BasePages
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.Common
Imports System.Collections.ObjectModel

Public Class AddEditSecurityEntities
    Inherits BaseWebpage

    Protected m_Profile As MSecurityEntityProfile = Nothing

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not String.IsNullOrEmpty(Request.QueryString("SecurityEntitySeqId")) Then
            Dim mAccountSeqID As Integer = Integer.Parse(Request.QueryString("SecurityEntitySeqId").ToString())
            If Not mAccountSeqID = -1 Then
                m_Profile = SecurityEntityUtility.GetProfile(mAccountSeqID)
            Else
                m_Profile = New MSecurityEntityProfile()
                m_Profile.DataAccessLayerAssemblyName = "GrowthWareFramework"
                m_Profile.DataAccessLayerNamespace = "GrowthWare.Framework.DataAccessLayer.SQLServer.V2008"
            End If
        Else
            m_Profile = SecurityEntityUtility.CurrentProfile()
        End If
        HttpContext.Current.Session.Add("EditId", m_Profile.Id)
        populatePage()
    End Sub

    Private Sub populatePage()
        litSecurityEntity.Text = m_Profile.Name
        txtSecurityEntity.Text = m_Profile.Name
        If m_Profile.Id = -1 Then
            litSecurityEntity.Visible = False
            txtSecurityEntity.Style.Add("display", "")
        Else
            litSecurityEntity.Visible = True
            txtSecurityEntity.Style.Add("display", "none")
        End If
        txtSeqID.Text = m_Profile.Id
        txtDescription.Text = m_Profile.Description
        txtURL.Text = m_Profile.Url
        Try
            txtConnectionstring.Text = CryptoUtility.Decrypt(m_Profile.ConnectionString, SecurityEntityUtility.CurrentProfile.EncryptionType)
        Catch ex As Exception
            txtConnectionstring.Text = m_Profile.ConnectionString
        End Try
        litSecurityEntityTranslation.Text = ConfigSettings.SecurityEntityTranslation
        txtAssembly_Name.Text = m_Profile.DataAccessLayerAssemblyName
        txtName_Space.Text = m_Profile.DataAccessLayerNamespace
        Dim myDirectoryInfo As New MDirectoryProfile
        Dim dvSkin As New DataView
        dvSkin = FileUtility.GetDirectoryTableData(GWWebHelper.SkinPath, myDirectoryInfo, False).DefaultView
        dvSkin.RowFilter = "Type = 'folder'"
        dropSkin.DataSource = dvSkin
        dropSkin.DataTextField = "Name"
        dropSkin.DataValueField = "Name"
        dropSkin.DataBind()

        Dim dvStyles As New DataView
        dvStyles = FileUtility.GetDirectoryTableData(Server.MapPath("~\Public\SiteStyles"), myDirectoryInfo, True).DefaultView
        dvStyles.RowFilter = "[Name] like '%.css'"
        dropStyles.DataSource = dvStyles
        dropStyles.DataTextField = "ShortFileName"
        dropStyles.DataValueField = "ShortFileName"
        dropStyles.DataBind()
        Dim mProfiles As Collection(Of MSecurityEntityProfile) = SecurityEntityUtility.Profiles()
        dropParent.DataSource = mProfiles
        dropParent.DataTextField = "Name"
        dropParent.DataValueField = "Id"
        dropParent.DataBind()
        Dim lstItem As New ListItem
        lstItem.Text = "None"
        lstItem.Value = "-1"
        dropParent.Items.Add(lstItem)
        NameValuePairUtility.SetDropSelection(dropParent, m_Profile.ParentSeqId)
        NameValuePairUtility.SetDropSelection(dropSkin, m_Profile.Skin)
        NameValuePairUtility.SetDropSelection(dropStyles, m_Profile.Style)
        NameValuePairUtility.SetDropSelection(dropStatus, m_Profile.StatusSeqId)
        NameValuePairUtility.SetDropSelection(dropDAL, m_Profile.DataAccessLayer)
        NameValuePairUtility.SetDropSelection(dropEncryptionType, m_Profile.EncryptionType)
    End Sub

End Class
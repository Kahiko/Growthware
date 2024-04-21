Imports GrowthWare.WebSupport
Imports GrowthWare.WebSupport.Base
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.Framework.Model.Profiles

Public Class AddEditRole
    Inherits BaseWebpage
    Protected m_Profile As MRoleProfile = Nothing

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mRoleSeqId As String = GWWebHelper.GetQueryValue(Request, "RoleSeqID")
        If Not String.IsNullOrEmpty(mRoleSeqId) Then
            Dim mRoleID As Integer = Integer.Parse(mRoleSeqId)
            If Not mRoleID = -1 Then
                m_Profile = RoleUtility.GetProfile(mRoleID)
            Else
                m_Profile = New MRoleProfile()
            End If
            HttpContext.Current.Session.Add("EditId", m_Profile.Id)
            populatePage()
        End If
    End Sub

    Private Sub populatePage()
        txtRoleSeqId.Value = m_Profile.Id.ToString()
        txtRole.Text = m_Profile.Name
        txtDescription.Text = m_Profile.Description
        chkIsSystem.Checked = m_Profile.IsSystem
        chkIsSystemOnly.Checked = m_Profile.IsSystemOnly

        If m_Profile.IsSystem Or m_Profile.IsSystemOnly Then
            txtRole.Enabled = False
            chkIsSystem.Enabled = False
            chkIsSystemOnly.Enabled = False
        End If
    End Sub

End Class
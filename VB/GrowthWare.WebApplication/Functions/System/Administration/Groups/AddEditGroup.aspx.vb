Imports GrowthWare.WebSupport.Base
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.WebSupport
Imports GrowthWare.Framework.Model.Profiles

Public Class AddEditGroup
    Inherits BaseWebpage

    Protected m_Profile As MGroupProfile = Nothing

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mGroupSeqId As String = GWWebHelper.GetQueryValue(Request, "GroupSeqID")
        If Not String.IsNullOrEmpty(mGroupSeqId) Then
            Dim mGroupID As Integer = Integer.Parse(mGroupSeqId)
            If Not mGroupID = -1 Then
                m_Profile = GroupUtility.GetProfile(mGroupID)
            Else
                m_Profile = New MGroupProfile()
            End If
            HttpContext.Current.Session.Add("EditId", m_Profile.Id)
            populatePage()
        End If
    End Sub

    Private Sub populatePage()
        txtGroupSeqId.Value = m_Profile.Id.ToString()
        txtGroup.Text = m_Profile.Name
        txtDescription.Text = m_Profile.Description
    End Sub

End Class
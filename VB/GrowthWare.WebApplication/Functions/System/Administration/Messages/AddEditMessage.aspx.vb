Imports GrowthWare.WebSupport.Base
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.Framework.Common
Imports GrowthWare.Framework.Model.Profiles.Interfaces

Public Class AddEditMessage
    Inherits BaseWebpage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not String.IsNullOrEmpty(Request.QueryString("messageSeqId")) Then
            Dim mMessageSeqId As Integer = Integer.Parse(Request.QueryString("messageSeqId").ToString())
            Dim mProfile As MMessageProfile = New MMessageProfile()
            If mMessageSeqId > -1 Then
                mProfile = MessageUtility.GetProfile(mMessageSeqId)
            End If
            populatePage(mProfile)
        End If
    End Sub

    Private Sub populatePage(ByVal profile As MMessageProfile)
        txtMessageSeqID.Value = profile.Id.ToString()
        lblName.Text = profile.Name
        txtName.Text = profile.Name
        txtDescription.Text = profile.Description
        txtMessageTitle.Text = profile.Title
        txtMessageBody.Text = profile.Body
        chkFormatAsHTML.Checked = profile.FormatAsHtml

        If profile.Id = -1 Then
            txtName.Style.Add("display", "inline")
            lblName.Visible = False
        End If

        Dim mProfile As IMessageProfile = Nothing
        Try
            mProfile = ObjectFactory.Create("GrowthWare.Framework", "GrowthWare.Framework.Model.Profiles", "M" + profile.Name)
        Catch ex As Exception
            Dim mLog As Logger = Logger.Instance()
            mLog.Debug(ex)
        Finally
            If mProfile Is Nothing Then
                txtTags.Text = profile.GetTags(System.Environment.NewLine)
            Else
                txtTags.Text = mProfile.GetTags(System.Environment.NewLine)
            End If
        End Try
    End Sub

End Class
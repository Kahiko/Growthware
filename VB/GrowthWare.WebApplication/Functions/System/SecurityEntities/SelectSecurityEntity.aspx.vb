Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.Framework.Common
Imports GrowthWare.WebSupport
Imports GrowthWare.WebSupport.BasePages

Public Class SelectSecurityEntity
    Inherits ClientChoicesPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mAccountProfile As MAccountProfile = AccountUtility.CurrentProfile()
        Try
            dropSecurityEntities.DataSource = SecurityEntityUtility.GetValidSecurityEntities(mAccountProfile.Account, ClientChoicesState(MClientChoices.SecurityEntityId), mAccountProfile.IsSystemAdmin)
            dropSecurityEntities.DataValueField = "SE_SEQ_ID"
            dropSecurityEntities.DataTextField = "NAME"
            dropSecurityEntities.DataBind()
            'NameValuePairUtility.SetDropSelection(dropSecurityEntities, ClientChoicesState(MClientChoices.SecurityEntityId))
        Catch ex As Exception
            Dim mMessageProfile As MMessageProfile = Nothing
            Dim mLog As Logger = Logger.Instance()
            Dim myEx As New Exception("SelectSecurityEntity:: reported an error.", ex)
            mLog.Error(myEx)
            mMessageProfile = MessageUtility.GetProfile("NoDataFound")
            clientMessage.InnerHtml = mMessageProfile.Body
            clientMessage.Visible = True
            dropSecurityEntities.Visible = False
            btnGo.Visible = False
        End Try
    End Sub

End Class
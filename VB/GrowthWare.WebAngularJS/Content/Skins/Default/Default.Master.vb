Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.WebSupport

Public Class _Default1
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mSecurityEntityID As Integer = SecurityEntityUtility.DefaultProfile.Id
        Dim mAccountProfile As MAccountProfile = AccountUtility.CurrentProfile()
        If Not mAccountProfile Is Nothing Then
            mSecurityEntityID = Integer.Parse(ClientChoicesUtility.GetClientChoicesState(AccountUtility.CurrentProfile().Account)(MClientChoices.SecurityEntityId).ToString())
        End If
        Dim mSecurityEntityProfile As MSecurityEntityProfile = SecurityEntityUtility.GetProfile(mSecurityEntityID)
        formStyles.Attributes("href") = GWWebHelper.RootSite + "/Content/FormStyles/" + mSecurityEntityProfile.Style + ".css"
    End Sub

End Class
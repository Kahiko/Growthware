Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.WebSupport

Public Class _Default2
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mSecurityEntityID As Integer = Integer.Parse(ClientChoicesUtility.GetClientChoicesState(AccountUtility.CurrentProfile.Account).Item(MClientChoices.SecurityEntityId).ToString())
        Dim mSecurityEntityProfile As MSecurityEntityProfile = SecurityEntityUtility.GetProfile(mSecurityEntityID)
        formStyles.Attributes("href") = GWWebHelper.RootSite + "/Content/SiteStyles/" + mSecurityEntityProfile.Style + ".css"
    End Sub

End Class
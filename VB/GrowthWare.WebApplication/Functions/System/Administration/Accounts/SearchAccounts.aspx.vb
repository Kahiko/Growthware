Imports GrowthWare.WebSupport
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.Framework.Model.Profiles

Public Class SearchAccounts
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mAction As String = GWWebHelper.GetQueryValue(Request, "action")
        If Not String.IsNullOrEmpty(mAction) Then
            Dim mFunctionProfile As MFunctionProfile = FunctionUtility.GetProfile(mAction)
            Dim mSecurityInfo As MSecurityInfo = New MSecurityInfo(mFunctionProfile, AccountUtility.CurrentProfile().DerivedRoles)
            SearchControl.ShowAddLink = mSecurityInfo.MayAdd
        End If
    End Sub

End Class
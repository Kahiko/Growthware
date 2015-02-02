Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport
Imports GrowthWare.WebSupport.Base
Imports GrowthWare.WebSupport.Utilities

Public Class SearchMessages
    Inherits ClientChoicesPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mAction As String = GWWebHelper.GetQueryValue(Request, "action")
        If Not String.IsNullOrEmpty(mAction) Then
            Dim mAccountProfile As MAccountProfile = AccountUtility.CurrentProfile()
            Dim mFunctionProfile As MFunctionProfile = FunctionUtility.GetProfile(mAction)
            Dim mSecurityInfo As MSecurityInfo = New MSecurityInfo(mFunctionProfile, mAccountProfile)
            SearchControl.ShowAddLink = mSecurityInfo.MayAdd
        End If
    End Sub

End Class
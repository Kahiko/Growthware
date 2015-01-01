Imports GrowthWare.WebSupport
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.BasePages

Public Class SearchFunctions
    Inherits BaseWebpage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mAction As String = GWWebHelper.GetQueryValue(Request, "action")
        If Not String.IsNullOrEmpty(mAction) Then
            Dim mFunctionProfile As MFunctionProfile = FunctionUtility.GetProfile(mAction)
            Dim mSecurityInfo As MSecurityInfo = New MSecurityInfo(mFunctionProfile, AccountUtility.CurrentProfile())
            SearchControl.ShowAddLink = mSecurityInfo.MayAdd
        End If
    End Sub

End Class
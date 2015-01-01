Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.BasePages
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.Framework.Common

Public Class Favorite
    Inherits ClientChoicesPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mAction As String = ClientChoicesState(MClientChoices.Action)
        Dim mFunctionProfile As MFunctionProfile = FunctionUtility.GetProfile(mAction)
        Dim mAccountProfile As MAccountProfile = AccountUtility.CurrentProfile()
        Dim mSecurityInfo As MSecurityInfo = New MSecurityInfo(mFunctionProfile, mAccountProfile)
        If mSecurityInfo.MayView Then
            Dim mScript As String = "<script type='text/javascript' language='javascript'>window.location.hash = '?Action=" + mAction + "'</script>"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "", mScript)
        Else
            Response.Write("Your favorite is not available.  Please ensure that you have chosen the correct " + ConfigSettings.SecurityEntityTranslation)
        End If
    End Sub

End Class
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.WebSupport.Utilities

Public Class FileManagerControl
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SearchControl.ShowAddLink = False
        Dim mDirectoryProfile As MDirectoryProfile = DirectoryUtility.GetProfile(FunctionUtility.CurrentProfile().Id)
        Dim mLinks As String = FileUtility.GetDirectoryLinks("/", mDirectoryProfile.FunctionSeqId)
        directorySelector.InnerHtml = mLinks
        Dim mFunctionProfile As MFunctionProfile = FunctionUtility.CurrentProfile()
        Dim mAccountProfile As MAccountProfile = AccountUtility.CurrentProfile()
        Dim mSI As MSecurityInfo = New MSecurityInfo(mFunctionProfile, mAccountProfile)
        UploadControl.Visible = mSI.MayAdd
        tdNewDirectory.Visible = mSI.MayAdd
        SearchControl.ShowDeleteAll = mSI.MayDelete
        SearchControl.ShowSelect = mSI.MayDelete
    End Sub

End Class
Imports GrowthWare.Framework.Model.Profiles
Imports System.IO
Imports GrowthWare.WebSupport.BasePages
Imports GrowthWare.WebSupport.Utilities

Public Class index
    Inherits BaseWebpage

    Protected Shadows Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        Dim mSecProfile As MSecurityEntityProfile = SecurityEntityUtility.CurrentProfile()
        Dim mMasterPage As String = "Public/Skins/" + mSecProfile.Skin + "/Default.Master"
        Dim fileName As String = String.Empty
        fileName = HttpContext.Current.Server.MapPath("~\\") + mMasterPage
        If Not File.Exists(fileName) Then
            mMasterPage = "Public/Skins/Default/Default.Master"
        End If
        Me.MasterPageFile = mMasterPage
        MyBase.Page_PreInit(sender, e)
    End Sub

End Class
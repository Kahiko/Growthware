Imports GrowthWare.Framework.Model.Profiles
Imports System.IO
Imports GrowthWare.WebSupport.Utilities
Imports GrowthWare.WebSupport.Base

Public Class Index
    Inherits BaseWebpage

    Protected Shadows Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        Dim mSecProfile As MSecurityEntityProfile = SecurityEntityUtility.CurrentProfile()
        Dim mMasterPage As String = "Public/Skins/" + mSecProfile.Skin + "/Default.master"
        Dim fileName As String = String.Empty
        fileName = HttpContext.Current.Server.MapPath("~\") + mMasterPage
        If Not File.Exists(fileName) Then
            mMasterPage = "Public/Skins/Default/Default.Master"
        End If
        Me.MasterPageFile = mMasterPage
        MyBase.Page_PreInit(sender, e)
    End Sub

End Class